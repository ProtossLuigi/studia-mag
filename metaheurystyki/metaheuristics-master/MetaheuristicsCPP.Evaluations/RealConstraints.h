#pragma once

#include "Constraint.h"

namespace Constraints
{
	class CRealBoundingBoxConstraint : public IConstraint<double>
	{
	public:
		CRealBoundingBoxConstraint(double dLowerBound, double dUpperBound);

		virtual double tGetLowerBound(int iDimension) { return d_lower_bound; }
		virtual double tGetUpperBound(int iDimension) { return d_upper_bound; }

		virtual bool bIsFeasible(int iDimension, double tValue) { return d_lower_bound <= tValue && tValue <= d_upper_bound; }

	private:
		double d_lower_bound;
		double d_upper_bound;
	};//class CBinaryNoConstraint : public IConstraint<bool>
}//namespace Constraints