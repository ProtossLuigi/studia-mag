#pragma once

#include "Individual.h"
#include "Selection.h"

#include <numeric>
#include <random>
#include <vector>

using namespace Optimizers;

using namespace std;

namespace Selections
{
	template <typename TElement>
	class CTournamentSelection : public CSelection<TElement>
	{
	public:
		CTournamentSelection(int iTournamentSize, mt19937 &cRandomEngine)
			: CSelection<TElement>(cRandomEngine), i_tournament_size(iTournamentSize)
		{

		}//CTournamentSelection(int iTournamentSize, mt19937 &cRandomEngine)

	protected:
		virtual void v_add_to_new_population(vector<CIndividual<TElement>*> &vPopulation, vector<CIndividual<TElement>*> &vNewPopulation)
		{
			vector<size_t> v_indexes(vPopulation.size());
			iota(v_indexes.begin(), v_indexes.end(), 0);

			CIndividual<TElement> *pc_tournament_winner;

			for (size_t i = 0; i < vPopulation.size(); i++)
			{
				shuffle(v_indexes.begin(), v_indexes.end(), this->c_random_engine);

				pc_tournament_winner = pc_get_tournament_winner(vPopulation, v_indexes);

				vNewPopulation.push_back(new CIndividual<TElement>(*pc_tournament_winner));
			}//for (size_t i = 0; i < vPopulation.size(); i++)
		}//virtual void v_add_to_new_population(vector<CIndividual<TElement>*> &vPopulation, vector<CIndividual<TElement>*> &vNewPopulation)

	private:
		CIndividual<TElement> *pc_get_tournament_winner(vector<CIndividual<TElement>*> &vPopulation, vector<size_t> &vIndexes)
		{
			CIndividual<TElement> *pc_tournament_winner = vPopulation[vIndexes.front()];

			for (int i = 1; i < i_tournament_size; i++)
			{
				if (vPopulation[vIndexes[i]]->dGetFitness() > pc_tournament_winner->dGetFitness())
				{
					pc_tournament_winner = vPopulation[vIndexes[i]];
				}//if (vPopulation[vIndexes[i]]->dGetFitness() > pc_tournament_winner->dGetFitness())
			}//for (int i = 1; i < i_tournament_size; i++)

			return pc_tournament_winner;
		}//CIndividual<TElement> *pc_get_tournament_winner(vector<CIndividual<TElement>*> &vPopulation, vector<size_t> &vIndexes)

		int i_tournament_size;
	};//class CTournamentSelection : public CSelection<TElement>


	template <typename TElement>
	class CRouletteWheelSelection : public CSelection<TElement>
	{
	public:
		CRouletteWheelSelection(mt19937 &cRandomEngine)
			: CSelection<TElement>(cRandomEngine)
		{

		}//CRouletteWheelSelection(mt19937 &cRandomEngine)

	protected:
		virtual void v_add_to_new_population(vector<CIndividual<TElement>*> &vPopulation, vector<CIndividual<TElement>*> &vNewPopulation)
		{
			vector<double> v_cumulative_probabilites;
			
			v_fill_cumulative_probabilities(vPopulation, v_cumulative_probabilites);

			for (size_t i = 0; i < vPopulation.size(); i++)
			{
				vNewPopulation.push_back(new CIndividual<TElement>(*pc_single_roulette_wheel(vPopulation, v_cumulative_probabilites)));
			}//for (size_t i = 0; i < vPopulation.size(); i++)
		}//virtual void v_add_to_new_population(vector<CIndividual<TElement>*> &vPopulation, vector<CIndividual<TElement>*> &vNewPopulation)

	private:
		static double d_calculate_fitness_sum(vector<CIndividual<TElement>*> &vPopulation)
		{
			double d_fitness_sum = 0;

			for (size_t i = 0; i < vPopulation.size(); i++)
			{
				d_fitness_sum += vPopulation[i]->dGetFitness();
			}//for (size_t i = 0; i < vPopulation.size(); i++)

			return d_fitness_sum;
		}//static double d_calculate_fitness_sum(vector<CIndividual<TElement>*> &vPopulation)

		static void v_fill_cumulative_probabilities(vector<CIndividual<TElement>*> &vPopulation, vector<double> &vCumulativeProbabilites)
		{
			vCumulativeProbabilites.reserve(vPopulation.size());

			double d_fitness_sum = d_calculate_fitness_sum(vPopulation);

			double d_cumulative_probability = 0;

			for (size_t i = 0; i < vPopulation.size(); i++)
			{
				d_cumulative_probability += vPopulation[i]->dGetFitness() / d_fitness_sum;
				vCumulativeProbabilites.push_back(d_cumulative_probability);
			}//for (size_t i = 0; i < vPopulation.size(); i++)
		}//static void v_fill_cumulative_probabilities(vector<CIndividual<TElement>*> &vPopulation, vector<double> &vCumulativeProbabilites)

		CIndividual<TElement> *pc_single_roulette_wheel(vector<CIndividual<TElement>*> &vPopulation, vector<double> &vCumulativeProbabilites)
		{
			CIndividual<TElement> *pc_selected_individual = nullptr;

			uniform_real_distribution<double> c_selection_distr;

			double d_selection_value = c_selection_distr(this->c_random_engine);

			for (size_t i = 0; i < vCumulativeProbabilites.size() && pc_selected_individual == nullptr; i++)
			{
				if (d_selection_value <= vCumulativeProbabilites[i])
				{
					pc_selected_individual = vPopulation[i];
				}//if (d_selection_value <= vCumulativeProbabilites[i])
			}//for (size_t i = 0; i < vCumulativeProbabilites.size() && pc_selected_individual == nullptr; i++)

			if (pc_selected_individual == nullptr)
			{
				pc_selected_individual = vPopulation.back();
			}//if (pc_selected_individual == nullptr)

			return pc_selected_individual;
		}//CIndividual<TElement> *pc_single_roulette_wheel(vector<CIndividual<TElement>*> &vPopulation, vector<double> &vCumulativeProbabilites)
	};//class CRouletteWheelSelection : public CSelection<TElement>
}//namespace Selections