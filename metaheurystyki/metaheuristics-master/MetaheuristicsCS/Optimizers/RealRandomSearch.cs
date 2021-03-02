using EvaluationsCLI;
using Generations;
using StopConditions;

namespace Optimizers
{
    class RealRandomSearch : SamplingOptimizer<double>
    {
        public RealRandomSearch(IEvaluation<double> evaluation, AStopCondition stopCondition, int? seed = null)
            : base(evaluation, stopCondition, new RealRandomGeneration(evaluation.pcConstraint, seed))
        {
        }
    }
}
