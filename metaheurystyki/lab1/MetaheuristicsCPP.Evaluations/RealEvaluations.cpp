#include "RealEvaluations.h"

using namespace Evaluations;


CRealEvaluation::CRealEvaluation(int iSize, double dMaxValue, double dLowerBound, double dUpperBound)
	: CEvaluation(iSize, dMaxValue), c_constraint(dLowerBound, dUpperBound)
{

}//CRealEvaluation::CRealEvaluation(int iSize, double dMaxValue, double dLowerBound, double dUpperBound)


CRealSquareSumEvaluation::CRealSquareSumEvaluation(int iSize)
	: CRealEvaluation(iSize, 0, -5, 5)
{

}//CRealSquareSumEvaluation::CRealSquareSumEvaluation(int iSize)

double CRealSquareSumEvaluation::d_evaluate(vector<double> &vSolution)
{
	double d_value = 0;

	for (int i = 0; i < iGetSize(); i++)
	{
		d_value -= vSolution[i] * vSolution[i];
	}//for (int i = 0; i < iGetSize(); i++)

	return d_value;
}//double CRealSquareSumEvaluation::d_evaluate(vector<double> &vSolution)