#include "RealEvaluations.h"

using namespace EvaluationsCLI;


CRealEvaluation::CRealEvaluation(Evaluations::CEvaluation<double> *pcNativeBinaryEvaluation)
{
	pc_native_real_evaluation = pcNativeBinaryEvaluation;
}//CRealEvaluation::CRealEvaluation(Evaluations::CEvaluation<double> *pcNativeBinaryEvaluation)

CRealEvaluation::~CRealEvaluation()
{
	delete pc_native_real_evaluation;
}//CRealEvaluation::~CRealEvaluation()

CRealEvaluation::!CRealEvaluation()
{
	delete pc_native_real_evaluation;
}//CRealEvaluation::!CRealEvaluation()

double CRealEvaluation::dEvaluate(IList<double> ^lSolution)
{
	vector<double> v_solution(0);
	v_solution.reserve(lSolution->Count);

	for (int i = 0; i < lSolution->Count; i++)
	{
		v_solution.push_back(lSolution[i]);
	}//for (int i = 0; i < lSolution->Count; i++)

	return pc_native_real_evaluation->dEvaluate(v_solution);
}//double CRealEvaluation::dEvaluate(IList<double> ^lSolution) 