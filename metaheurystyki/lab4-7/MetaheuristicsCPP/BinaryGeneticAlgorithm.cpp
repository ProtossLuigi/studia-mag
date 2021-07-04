#include "BinaryGeneticAlgorithm.h"

using namespace Optimizers;

CBinaryGeneticAlgorithm::CBinaryGeneticAlgorithm(IEvaluation<bool> &cEvaluation, IStopCondition &cStopCondition, IGenerator<bool> &cGeneration, ICrossover<bool> &cCrossover, IMutation<bool> &cMutation, ISelection<bool> &cSelection, mt19937 &cRandomEngine, int iPopulationSize)
	: CPopulationOptimizer<bool>(cEvaluation, cStopCondition, cGeneration, cSelection, cMutation, iPopulationSize), c_crossover(cCrossover), c_random_engine(cRandomEngine)
{

}//CBinaryGeneticAlgorithm::CBinaryGeneticAlgorithm(IEvaluation<bool> &cEvaluation, IStopCondition &cStopCondition, IGenerator<bool> &cGeneration, ICrossover<bool> &cCrossover, IMutation<bool> &cMutation, ISelection<bool> &cSelection,  mt19937 &cRandomEngine, int iPopulationSize)

bool CBinaryGeneticAlgorithm::b_run_iteration(long long iIterationNumber, clock_t tStartTime)
{
	v_selection();

	v_crossover();
	v_evaluation();

	bool b_new_best = b_check_new_best();

	v_mutation();
	v_evaluation();

	return b_check_new_best() || b_new_best;
}//bool CBinaryGeneticAlgorithm::b_run_iteration(long long iIterationNumber, clock_t tStartTime)

void CBinaryGeneticAlgorithm::v_crossover()
{
	vector<CIndividual<bool>*> *pv_new_population = new vector<CIndividual<bool>*>();
	pv_new_population->reserve(pv_population->size());

	vector<size_t> v_indexes(pv_population->size());
	
	iota(v_indexes.begin(), v_indexes.end(), 0);
	shuffle(v_indexes.begin(), v_indexes.end(), c_random_engine);

	CIndividual<bool> *pc_parent_0, *pc_parent_1, *pc_child_0, *pc_child_1;
	vector<bool> *pv_child_0_genotype, *pv_child_1_genotype;

	for (size_t i = 0; i < pv_population->size(); i += 2)
	{
		pc_parent_0 = pv_population->at(i + 0);
		pc_parent_1 = pv_population->at(i + 1);

		pv_child_0_genotype = new vector<bool>((size_t)c_evaluation.iGetSize());
		pv_child_1_genotype = new vector<bool>((size_t)c_evaluation.iGetSize());

		if (c_crossover.bCross(pc_parent_0->vGetGenotype(), pc_parent_1->vGetGenotype(), *pv_child_0_genotype, *pv_child_1_genotype))
		{
			delete pc_parent_0;
			delete pc_parent_1;

			pc_child_0 = pc_create_individual(pv_child_0_genotype);
			pc_child_1 = pc_create_individual(pv_child_1_genotype);

			pv_population->at(i + 0) = pc_child_0;
			pv_population->at(i + 1) = pc_child_1;
		}//if (c_crossover.bCross(pc_parent_0->vGetGenotype(), pc_parent_1->vGetGenotype(), *pv_child_0_genotype, *pv_child_1_genotype))
		else
		{
			delete pv_child_0_genotype;
			delete pv_child_1_genotype;
		}//else if (c_crossover.bCross(pc_parent_0->vGetGenotype(), pc_parent_1->vGetGenotype(), *pv_child_0_genotype, *pv_child_1_genotype))
	}//for (size_t i = 0; i < pv_population->size(); i += 2)
}//void CBinaryGeneticAlgorithm::v_crossover()