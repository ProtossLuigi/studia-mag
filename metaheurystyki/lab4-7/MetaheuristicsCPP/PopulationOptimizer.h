#pragma once

#include "Generator.h"
#include "Individual.h"
#include "Mutation.h"
#include "Optimizer.h"
#include "Selection.h"
#include "VectorUtils.h"

#include <vector>

using namespace Generators;
using namespace Mutations;
using namespace Selections;

using namespace std;

namespace Optimizers
{
	template <typename TElement>
	class CPopulationOptimizer : public COptimizer<TElement>
	{
	public:
		CPopulationOptimizer(IEvaluation<TElement> &cEvaluation, IStopCondition &cStopCondition, IGenerator<TElement> &cGeneration, ISelection<TElement> &cSelection, IMutation<TElement> &cMutation, int iPopulationSize)
			: COptimizer<TElement>(cEvaluation, cStopCondition), c_generation(cGeneration), c_selection(cSelection), c_mutation(cMutation), i_population_size(iPopulationSize)
		{
			pv_population = new vector<CIndividual<TElement>*>();
		}//CPopulationOptimizer(IEvaluation<TElement> &cEvaluation, IStopCondition &cStopCondition, IGeneration<TElement> &cGeneration, IMutation<TElement> &cMutation, int iPopulationSize)

		virtual ~CPopulationOptimizer()
		{
			VectorUtils::vClear(*pv_population);
			delete pv_population;
		}//virtual ~CPopulationOptimizer()

	protected:
		virtual void v_initialize(clock_t tStartTime)
		{
			VectorUtils::vClear(*pv_population);
			pv_population->reserve((size_t)i_population_size);

			for (int i = 0; i < i_population_size; i++)
			{
				pv_population->push_back(pc_create_individual());
			}//for (int i = 0; i < i_population_size; i++)

			v_evaluation();
			b_check_new_best();
		}//virtual void v_initialize(clock_t tStartTime)

		void v_evaluation()
		{
			for (size_t i = 0; i < pv_population->size(); i++)
			{
				pv_population->at(i)->dEvaluate();
			}//for (size_t i = 0; i < pv_population->size(); i++)
		}//void v_evaluation()

		void v_selection()
		{
			c_selection.vSelect(&pv_population);
		}//void v_selection()

		void v_mutation()
		{
			for (size_t i = 0; i < pv_population->size(); i++)
			{
				pv_population->at(i)->bMutate();
			}//for (size_t i = 0; i < pv_population->size(); i++)
		}//void v_mutation()

		bool b_check_new_best(bool bOnlyImprovements = true)
		{
			CIndividual<TElement> *pc_current_best_individual = pv_population->front();

			for (size_t i = 1; i < pv_population->size(); i++)
			{
				if (pv_population->at(i)->dGetFitness() > pc_current_best_individual->dGetFitness())
				{
					pc_current_best_individual = pv_population->at(i);
				}//if (pv_population->at(i)->dGetFitness() > pc_current_best_individual->dGetFitness())
			}//for (size_t i = 1; i < pv_population->size(); i++)

			return COptimizer<TElement>::b_check_new_best(pc_current_best_individual->vGetGenotype(), pc_current_best_individual->dGetFitness(), bOnlyImprovements);
		}//bool b_check_new_best(bool bOnlyImprovements = true)

		virtual CIndividual<TElement> *pc_create_individual(vector<TElement> *pvGenotype = nullptr)
		{
			if (pvGenotype == nullptr)
			{
				pvGenotype = c_generation.pvCreate(this->c_evaluation.iGetSize());
			}//if (pvGenotype == nullptr)

			return new CIndividual<TElement>(pvGenotype, this->c_evaluation, c_mutation);
		}//virtual CIndividual<TElement> *pc_create_individual(vector<TElement> *pvGenotype = nullptr)

		int i_population_size;
		vector<CIndividual<TElement>*> *pv_population;

		IGenerator<TElement> &c_generation;

		ISelection<TElement> &c_selection;
		IMutation<TElement> &c_mutation;
	};//class CPopulationOptimizer : public COptimizer<TElement>
}//namespace Optimizers