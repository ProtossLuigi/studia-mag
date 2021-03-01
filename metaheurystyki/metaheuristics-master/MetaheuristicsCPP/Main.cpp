#include "BinaryGenerations.h"
#include "BinaryRandomSearch.h"
#include "SamplingOptimizer.h"
#include "OptimizationResult.h"
#include "RealEvolutionStrategy11.h"
#include "RealGenerations.h"
#include "RealMutationES11Adaptations.h"
#include "StopConditions.h"

#include <BinaryEvaluations.h>
#include <iostream>
#include <random>
#include <RealEvaluations.h>

using namespace Generations;
using namespace Optimizers;
using namespace StopConditions;

using namespace Evaluations;
using namespace std;


template <typename TElement>
void v_report_optimization_result(COptimizationResult<TElement> &cOptimizationResult)
{
	cout << "value: " << cOptimizationResult.dGetBestValue() << endl;
	cout << "when (time): " << cOptimizationResult.dGetBestTime() << " s" << endl;
	cout << "when (iteration): " << cOptimizationResult.iGetBestIteration() << endl;
	cout << "when (FFE): " << cOptimizationResult.iGetBestFFE() << endl;
}//void v_report_optimization_result(COptimizationResult<TElement> &cOptimizationResult)

void v_lab_1_one_max(mt19937 &cRandomEngine)
{
	CBinaryOneMaxEvaluation c_evaluation(5);
	CIterationsStopCondition c_stop_condition(c_evaluation.dGetMaxValue(), 500);
	CBinaryRandomSearch c_random_search(c_evaluation, c_stop_condition, cRandomEngine);

	c_random_search.vRun();

	v_report_optimization_result(*c_random_search.pcGetResult());
}//void v_lab_1_one_max(mt19937 &cRandomEngine)

void v_lab_1_standard_deceptive_concatenation(mt19937 &cRandomEngine)
{
	CBinaryStandardDeceptiveConcatenationEvaluation c_evaluation(5, 1);
	CIterationsStopCondition c_stop_condition(c_evaluation.dGetMaxValue(), 500);
	CBinaryRandomSearch c_random_search(c_evaluation, c_stop_condition, cRandomEngine);

	c_random_search.vRun();

	v_report_optimization_result(*c_random_search.pcGetResult());
}//void v_lab_1_standard_deceptive_concatenation(mt19937 &cRandomEngine)

void v_lab_1_bimodal_deceptive_concatenation(mt19937 &cRandomEngine)
{
	CBinaryBimodalDeceptiveConcatenationEvaluation c_evaluation(10, 1);
	CIterationsStopCondition c_stop_condition(c_evaluation.dGetMaxValue(), 500);
	CBinaryRandomSearch c_random_search(c_evaluation, c_stop_condition, cRandomEngine);

	c_random_search.vRun();

	v_report_optimization_result(*c_random_search.pcGetResult());
}//void v_lab_1_bimodal_deceptive_concatenation(mt19937 &cRandomEngine)

void v_lab_1_ising_spin_glass(mt19937 &cRandomEngine)
{
	CBinaryIsingSpinGlassEvaluation c_evaluation(25);
	CIterationsStopCondition c_stop_condition(c_evaluation.dGetMaxValue(), 500);
	CBinaryRandomSearch c_random_search(c_evaluation, c_stop_condition, cRandomEngine);

	c_random_search.vRun();

	v_report_optimization_result(*c_random_search.pcGetResult());
}//void v_lab_1_ising_spin_glass(mt19937 &cRandomEngine)

void v_lab_1_nk_landscapes(mt19937 &cRandomEngine)
{
	CBinaryNKLandscapesEvaluation c_evaluation(10);
	CIterationsStopCondition c_stop_condition(c_evaluation.dGetMaxValue(), 500);
	CBinaryRandomSearch c_random_search(c_evaluation, c_stop_condition, cRandomEngine);

	c_random_search.vRun();

	v_report_optimization_result(*c_random_search.pcGetResult());
}//void v_lab_1_nk_landscapes(mt19937 &cRandomEngine)

void v_lab_2(mt19937 &cRandomEngine)
{
	CRealSquareSumEvaluation c_square_sum_evaluation(5);
	CRunningTimeStopCondition c_running_time_stop_condition(c_square_sum_evaluation.dGetMaxValue(), 5);
	CRealGaussianMutation c_mutation(0.1, c_square_sum_evaluation, cRandomEngine);
	CRealNullRealMutationES11Adaptation c_mutation_adaptation(c_mutation);

	CRealEvolutionStrategy11 c_optimizer(c_square_sum_evaluation, c_running_time_stop_condition, c_mutation_adaptation, cRandomEngine);

	c_optimizer.vRun();

	v_report_optimization_result(*c_optimizer.pcGetResult());
}//void v_lab_2(mt19937 &cRandomEngine)

void v_lab_3(mt19937 &cRandomEngine)
{
	CRealSquareSumEvaluation c_square_sum_evaluation(5);
	CRunningTimeStopCondition c_running_time_stop_condition(c_square_sum_evaluation.dGetMaxValue(), 5);
	CRealGaussianMutation c_mutation(0.1, c_square_sum_evaluation, cRandomEngine);
	CRealOneFifthRuleMutationES11Adaptation c_mutation_adaptation(10, 0.82, c_mutation);

	CRealEvolutionStrategy11 c_optimizer(c_square_sum_evaluation, c_running_time_stop_condition, c_mutation_adaptation, cRandomEngine);

	c_optimizer.vRun();

	v_report_optimization_result(*c_optimizer.pcGetResult());
}//void v_lab_3(mt19937 &cRandomEngine)

int main()
{
	random_device c_seed_generator;
	mt19937 c_random_engine(c_seed_generator());

	v_lab_1_one_max(c_random_engine);
	v_lab_1_standard_deceptive_concatenation(c_random_engine);
	v_lab_1_bimodal_deceptive_concatenation(c_random_engine);
	v_lab_1_ising_spin_glass(c_random_engine);
	v_lab_1_nk_landscapes(c_random_engine);

	CBinaryOneMaxEvaluation c_one_max_evaluation(50);
	CBinaryRandomGeneration c_random_generation(c_one_max_evaluation.cGetConstraint(), c_random_engine);
	CIterationsStopCondition c_iterations_stop_conditions(c_one_max_evaluation.dGetMaxValue(), 100);

	CSamplingOptimizer<bool> c_sampling_optimizer(c_one_max_evaluation, c_iterations_stop_conditions, c_random_generation);
	c_sampling_optimizer.vRun();

	CBinaryRandomSearch c_random_search(c_one_max_evaluation, c_iterations_stop_conditions, c_random_engine);
	c_random_search.vRun();

	CRealSquareSumEvaluation c_square_sum_evaluation(5);
	CRealRandomGeneration c_real_random_generation(c_square_sum_evaluation.cGetConstraint(), c_random_engine);
	CRunningTimeStopCondition c_running_time_stop_condition(c_square_sum_evaluation.dGetMaxValue(), 5);

	CSamplingOptimizer<double> c_real_sampling_optimizer(c_square_sum_evaluation, c_running_time_stop_condition, c_real_random_generation);
	c_real_sampling_optimizer.vRun();

	v_lab_2(c_random_engine);
	v_lab_3(c_random_engine);

	return 0;
}//int main()