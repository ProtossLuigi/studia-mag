#pragma once

#include "RealConstraints.h"
#include "Evaluation.h"

#include <vector>

using namespace Constraints;

using namespace std;

namespace Evaluations
{
	class CRealEvaluation : public CEvaluation<double>
	{
	public:
		CRealEvaluation(int iSize, double dMaxValue, double dLowerBound, double dUpperBound);

		virtual IConstraint<double> &cGetConstraint() { return c_constraint; }

	private:
		 CRealBoundingBoxConstraint c_constraint;
	};//class CBinaryEvaluation : public IEvaluation<bool>


	class CRealSquareSumEvaluation : public CRealEvaluation
	{
	public:
		CRealSquareSumEvaluation(int iSize);

	protected:
		virtual double d_evaluate(vector<double> &vSolution);
	};//class CRealSquareSumEvaluation : public CEvaluation<double>
}//namespace Evaluations