#include "BinaryGenerations.h"

using namespace Generations;


CBinaryRandomGeneration::CBinaryRandomGeneration(IConstraint<bool> &cConstraint, mt19937 &cRandomEngine)
	: CGeneration<bool>(cConstraint), c_random_engine(cRandomEngine)
{

}//CBinaryRandomGeneration::CBinaryRandomGeneration(IConstraint<bool> &cConstraint, mt19937 &cRandomEngine)

void CBinaryRandomGeneration::vFill(vector<bool> &vSolution)
{
	bool b_lower_bound, b_upper_bound;

	for (size_t i = 0; i < vSolution.size(); i++)
	{
		b_lower_bound = c_constraint.tGetLowerBound((int)i);
		b_upper_bound = c_constraint.tGetUpperBound((int)i);

		if (b_lower_bound == b_upper_bound)
		{
			vSolution[i] = b_lower_bound;
		}//if (b_lower_bound == b_upper_bound)
		else
		{
			bernoulli_distribution c_distr(0.5);
			vSolution[i] = c_distr(c_random_engine);
		}//else if (b_lower_bound == b_upper_bound)
	}//for (size_t i = 0; i < vSolution.size(); i++)
}//void CBinaryRandomGeneration::vFill(vector<bool> &vSolution)