using EvaluationsCLI;
using Generations;
using Optimizers;
using StopConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizers
{
    class GreedyRS : SamplingOptimizer<bool>
    {
        public GreedyRS(IEvaluation<bool> evaluation, AStopCondition stopCondition, int? greedyIterations = null, int? seed = null)
            : base(evaluation, stopCondition, new BinaryGreedyRandomGeneration(evaluation, greedyIterations, seed)) { }
    }
}
