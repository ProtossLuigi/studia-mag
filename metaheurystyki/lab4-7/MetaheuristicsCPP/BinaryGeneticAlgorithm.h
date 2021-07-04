#pragma once

#include "Crossover.h"
#include "Mutation.h"
#include "PopulationOptimizer.h"
#include "Selection.h"
#include "StopCondition.h"

#include <algorithm>
#include <ctime>
#include <numeric>
#include <random>

using namespace Crossovers;
using namespace Mutations;
using namespace Selections;
using namespace StopConditions;

namespace Optimizers
{
	class CBinaryGeneticAlgorithm : public CPopulationOptimizer<bool>
	{
	public:
		CBinaryGeneticAlgorithm(IEvaluation<bool> &cEvaluation,
			IStopCondition &cStopCondition,
			IGenerator<bool> &cGeneration,
			ICrossover<bool> &cCrossover,
			IMutation<bool> &cMutation,
			ISelection<bool> &cSelection,
			mt19937 &cRandomEngine,
			int iPopulationSize);

	protected:
		virtual bool b_run_iteration(long long iIterationNumber, clock_t tStartTime);

	private:
		void v_crossover();

		ICrossover<bool> &c_crossover;

		mt19937 &c_random_engine;
	};//class CBinaryGeneticAlgorithm : public CPopulationOptimizer<bool>
}//namespace Optimizers