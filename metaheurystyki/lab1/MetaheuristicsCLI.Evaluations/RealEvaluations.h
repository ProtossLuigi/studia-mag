#pragma once

#include "RealConstraints.h"
#include "Evaluation.h"

#include <RealEvaluations.h>

using namespace System::Collections::Generic;

namespace EvaluationsCLI
{
	public ref class CRealEvaluation : public IEvaluation<double>
	{
	public:
		CRealEvaluation(Evaluations::CEvaluation<double> *pcNativeRealEvaluation);

		virtual ~CRealEvaluation();
		!CRealEvaluation();

		virtual double dEvaluate(IList<double> ^lSolution);

		virtual property int iSize
		{
			int get() { return pc_native_real_evaluation->iGetSize(); }
		}//virtual property int iSize

		virtual property double dMaxValue
		{
			double get() { return pc_native_real_evaluation->dGetMaxValue(); }
		}//virtual property double dMaxValue

		virtual property ConstraintsCLI::IConstraint<double>^ pcConstraint
		{
			ConstraintsCLI::IConstraint<double>^ get() { return gcnew ConstraintsCLI::CRealConstraint(pc_native_real_evaluation->cGetConstraint()); }
		}//virtual property ConstraintsCLI::IConstraint<double>^ pcConstraint

		virtual property long long iFFE
		{
			long long get() { return pc_native_real_evaluation->iGetFFE(); }
		}//property long long iFFE

	private:
		Evaluations::CEvaluation<double> *pc_native_real_evaluation;
	};//public ref class CRealEvaluation : public IEvaluation<double>
}//namespace EvaluationsCLI
