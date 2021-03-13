using EvaluationsCLI;
using Generations;
using StopConditions;

namespace Optimizers
{
    class BinaryRandomSearch : SamplingOptimizer<bool>
    {
        public BinaryRandomSearch(IEvaluation<bool> evaluation, AStopCondition stopCondition, int? seed = null)
            : base(evaluation, stopCondition, new BinaryRandomGeneration(evaluation.pcConstraint, seed))
        {
        }
    }
}
