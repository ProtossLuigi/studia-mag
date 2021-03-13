#pragma once

#include "BinaryGenerations.h"
#include "SamplingOptimizer.h"

using namespace Generations;

namespace Optimizers
{
	class CBinaryRandomSearch : public CSamplingOptimizer<bool>
	{
	public:
		CBinaryRandomSearch(IEvaluation<bool> &cEvaluation, IStopCondition &cStopCondition, mt19937 &cRandomEngine);

	private:
		CBinaryRandomGeneration c_random_generation;
	};//class CBinaryRandomSearch : public CSamplingOptimizer<bool>
}//namespace Optimizers