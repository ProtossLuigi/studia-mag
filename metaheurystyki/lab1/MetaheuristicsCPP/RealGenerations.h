#pragma once

#include "Generation.h"

#include <random>

namespace Generations
{
	class CRealRandomGeneration : public CGeneration<double>
	{
	public:
		CRealRandomGeneration(IConstraint<double> &cConstraint, mt19937 &cRandomEngine);

		virtual void vFill(vector<double> &vSolution);

	private:
		mt19937 &c_random_engine;
	};//class CRealRandomGeneration : public CGeneration<double>
}//namespace Generations