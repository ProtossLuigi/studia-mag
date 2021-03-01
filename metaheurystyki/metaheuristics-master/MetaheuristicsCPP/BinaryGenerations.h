#pragma once

#include "Generation.h"

#include <random>

namespace Generations
{
	class CBinaryRandomGeneration : public CGeneration<bool>
	{
	public:
		CBinaryRandomGeneration(IConstraint<bool> &cConstraint, mt19937 &cRandomEngine);

		virtual void vFill(vector<bool> &vSolution);

	private:
		mt19937 &c_random_engine;
	};//class CBinaryRandomGeneration : public CGeneration<bool>
}//namespace Generations