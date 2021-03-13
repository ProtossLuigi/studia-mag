#pragma once

#include "SamplingOptimizer.h"
#include "RealGenerations.h"

using namespace Generations;

namespace Optimizers
{
	class CRealRandomSearch : public CSamplingOptimizer<double>
	{
	public:
		CRealRandomSearch(IEvaluation<double> &cEvaluation, IStopCondition &cStopCondition, mt19937 &cRandomEngine);

	private:
		CRealRandomGeneration c_random_generation;
	};//class CRealRandomSearch : public CSamplingOptimizer<double>
}//namespace Optimizers