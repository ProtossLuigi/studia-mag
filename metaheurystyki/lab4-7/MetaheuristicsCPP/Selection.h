#pragma once

#include "Individual.h"
#include "VectorUtils.h"

#include <random>
#include <vector>

using namespace Optimizers;

using namespace std;

namespace Selections
{
	template <typename TElement>
	class ISelection
	{
	public:
		virtual ~ISelection() = default;

		virtual void vSelect(vector<CIndividual<TElement>*> **ppvPopulation) = 0;
	};//class ISelection


	template <typename TElement>
	class CSelection : public ISelection<TElement>
	{
	public:
		CSelection(mt19937 &cRandomEngine)
			: c_random_engine(cRandomEngine)
		{

		}//CSelection(mt19937 &cRandomEngine)

		virtual void vSelect(vector<CIndividual<TElement>*> **ppvPopulation) final
		{
			vector<CIndividual<TElement>*> *pv_population = *ppvPopulation;

			vector<CIndividual<TElement>*> *pv_new_population = new vector<CIndividual<TElement>*>();
			pv_new_population->reserve(pv_population->size());

			v_add_to_new_population(*pv_population, *pv_new_population);

			VectorUtils::vClear(*pv_population);
			delete pv_population;

			*ppvPopulation = pv_new_population;
		}//virtual void vSelect(vector<CIndividual<TElement>*> &vPopulation) final

	protected:
		virtual void v_add_to_new_population(vector<CIndividual<TElement>*> &vPopulation, vector<CIndividual<TElement>*> &vNewPopulation) = 0;

		mt19937 &c_random_engine;
	};//class CSelection : public ISelection<TElement>
}//namespace Selections