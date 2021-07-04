#include "BinaryKnapsackGeneticAlgorithm.h"

using namespace Optimizers;

CBinaryKnapsackGeneticAlgorithm::CBinaryKnapsackGeneticAlgorithm(CBinaryKnapsackEvaluation &cBinaryKnapsackEvaluation, IStopCondition &cStopCondition, IGenerator<bool> &cGenerator, ICrossover<bool> &cCrossover, IMutation<bool> &cMutation, IBinaryKnapsackSelection &cBinaryKnapsackSelection, mt19937 &cRandomEngine, int iPopulationSize)
	: COptimizer<bool>(cBinaryKnapsackEvaluation, cStopCondition), c_binary_knapsack_evaluation(cBinaryKnapsackEvaluation), c_generator(cGenerator), c_binary_knapsack_selection(cBinaryKnapsackSelection), c_crossover(cCrossover), c_mutation(cMutation), c_random_engine(cRandomEngine), i_population_size(iPopulationSize)
{
	pv_population = new vector<CBinaryKnapsackIndividual*>();
}//CBinaryKnapsackGeneticAlgorithm::CBinaryKnapsackGeneticAlgorithm(CBinaryKnapsackEvaluation &cBinaryKnapsackEvaluation, IStopCondition &cStopCondition, IGenerator<bool> &cGenerator, ICrossover<bool> &cCrossover, IMutation<bool> &cMutation, IBinaryKnapsackSelection &cBinaryKnapsackSelection, mt19937 &cRandomEngine, int iPopulationSize)

CBinaryKnapsackGeneticAlgorithm::~CBinaryKnapsackGeneticAlgorithm()
{
	VectorUtils::vClear(*pv_population);
	delete pv_population;
}//CBinaryKnapsackGeneticAlgorithm::~CBinaryKnapsackGeneticAlgorithm()

void CBinaryKnapsackGeneticAlgorithm::v_initialize(clock_t tStartTime)
{
	VectorUtils::vClear(*pv_population);
	pv_population->reserve((size_t)i_population_size);

	for (int i = 0; i < i_population_size; i++)
	{
		pv_population->push_back(pc_create_individual());
	}//for (int i = 0; i < i_population_size; i++)

	v_evaluation();
	b_check_new_best();
}//void CBinaryKnapsackGeneticAlgorithm::v_initialize(clock_t tStartTime)

bool CBinaryKnapsackGeneticAlgorithm::b_run_iteration(long long iIterationNumber, clock_t tStartTime)
{
	v_selection();

	v_crossover();
	v_evaluation();

	bool b_new_best = b_check_new_best();

	v_mutation();
	v_evaluation();

	return b_check_new_best() || b_new_best;
}//bool CBinaryKnapsackGeneticAlgorithm::b_run_iteration(long long iIterationNumber, clock_t tStartTime)

void CBinaryKnapsackGeneticAlgorithm::v_evaluation()
{
	for (size_t i = 0; i < pv_population->size(); i++)
	{
		pv_population->at(i)->dEvaluate();
	}//for (size_t i = 0; i < pv_population->size(); i++)
}//void CBinaryKnapsackGeneticAlgorithm::v_evaluation()

void CBinaryKnapsackGeneticAlgorithm::v_selection()
{
	c_binary_knapsack_selection.vSelect(&pv_population);
}//void CBinaryKnapsackGeneticAlgorithm::v_selection()

void CBinaryKnapsackGeneticAlgorithm::v_crossover()
{
	vector<CBinaryKnapsackIndividual*> *pv_new_population = new vector<CBinaryKnapsackIndividual*>();
	pv_new_population->reserve(pv_population->size());

	vector<size_t> v_indexes(pv_population->size());

	iota(v_indexes.begin(), v_indexes.end(), 0);
	shuffle(v_indexes.begin(), v_indexes.end(), c_random_engine);

	CBinaryKnapsackIndividual *pc_parent_0, *pc_parent_1, *pc_child_0, *pc_child_1;
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
}//void CBinaryKnapsackGeneticAlgorithm::v_crossover()

void CBinaryKnapsackGeneticAlgorithm::v_mutation()
{
	for (size_t i = 0; i < pv_population->size(); i++)
	{
		pv_population->at(i)->bMutate();
	}//for (size_t i = 0; i < pv_population->size(); i++)
}//void CBinaryKnapsackGeneticAlgorithm::v_mutation()

bool CBinaryKnapsackGeneticAlgorithm::b_check_new_best(bool bOnlyImprovements)
{
	CBinaryKnapsackIndividual *pc_current_best_individual = pv_population->front();

	for (size_t i = 1; i < pv_population->size(); i++)
	{
		if (pv_population->at(i)->dGetFitness() > pc_current_best_individual->dGetFitness())
		{
			pc_current_best_individual = pv_population->at(i);
		}//if (pv_population->at(i)->dGetFitness() > pc_current_best_individual->dGetFitness())
	}//for (size_t i = 1; i < pv_population->size(); i++)

	return COptimizer<bool>::b_check_new_best(pc_current_best_individual->vGetGenotype(), pc_current_best_individual->dGetFitness(), bOnlyImprovements);
}//bool CBinaryKnapsackGeneticAlgorithm::b_check_new_best(bool bOnlyImprovements)

CBinaryKnapsackIndividual * CBinaryKnapsackGeneticAlgorithm::pc_create_individual(vector<bool> *pvGenotype)
{
	if (pvGenotype == nullptr)
	{
		pvGenotype = c_generator.pvCreate(c_evaluation.iGetSize());
	}//if (pvGenotype == nullptr)

	return new CBinaryKnapsackIndividual(pvGenotype, c_binary_knapsack_evaluation, c_mutation);
}//CBinaryKnapsackIndividual * CBinaryKnapsackGeneticAlgorithm::pc_create_individual(vector<bool> *pvGenotype)