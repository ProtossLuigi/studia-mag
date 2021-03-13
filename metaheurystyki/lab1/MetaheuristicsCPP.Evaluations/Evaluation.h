#pragma once

#include "Constraint.h"

#include <vector>

using namespace Constraints;

using namespace std;

namespace Evaluations
{
	template <typename TElement>
	class IEvaluationProfile
	{
	public:
		virtual ~IEvaluationProfile() = default;

		virtual int iGetSize() = 0;
		virtual double dGetMaxValue() = 0;
		virtual IConstraint<TElement> &cGetConstraint() = 0;
	};//class IEvaluationProfile


	template <typename TElement>
	class IEvaluation : public IEvaluationProfile<TElement>
	{
	public:
		virtual double dEvaluate(vector<TElement> &vSolution) = 0;

		virtual long long iGetFFE() = 0;
	};//class IEvaluation : public IEvaluationProfile<TElement>


	template <typename TElement>
	class CEvaluation : public IEvaluation<TElement>
	{
	public:
		CEvaluation(int iSize, double dMaxValue) : i_size(iSize), d_max_value(dMaxValue), i_ffe(0) { }

		CEvaluation(const CEvaluation<TElement> &cOther) = delete;
		CEvaluation(CEvaluation<TElement> &&cOther) = delete;

		virtual double dEvaluate(vector<TElement> &vSolution) final
		{
			double d_value = d_evaluate(vSolution);
			i_ffe++;

			return d_value;
		}//virtual double dEvaluate(vector<TElement> &vSolution) final

		virtual int iGetSize() { return i_size; }
		virtual double dGetMaxValue() { return d_max_value; }
		virtual long long iGetFFE() { return i_ffe; }

		CEvaluation<TElement>& operator=(const CEvaluation<TElement> &cOther) = delete;
		CEvaluation<TElement>& operator=(CEvaluation<TElement> &&cOther) = delete;

	protected:
		virtual double d_evaluate(vector<TElement> &vSolution) = 0;

	private:
		int i_size;
		double d_max_value;
		long long i_ffe;
	};//class CEvaluation : public IEvaluation<TElement>
}//namespace Evaluations
