#include "RealConstraints.h"

using namespace Constraints;


CRealBoundingBoxConstraint::CRealBoundingBoxConstraint(double dLowerBound, double dUpperBound)
	: d_lower_bound(dLowerBound), d_upper_bound(dUpperBound)
{

}//CRealBoundingBoxConstraint::CRealBoundingBoxConstraint(double dLowerBound, double dUpperBound)