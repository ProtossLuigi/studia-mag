using System;
using System.Collections.Generic;

using Generations;
using EvaluationsCLI;
using StopConditions;

namespace Optimizers
{
    class SamplingOptimizer<Element> : AOptimizer<Element>
    {
        private readonly AGeneration<Element> generation;

        public SamplingOptimizer(IEvaluation<Element> evaluation, AStopCondition stopCondition, AGeneration<Element> generation)
            : base(evaluation, stopCondition)
        {
            this.generation = generation;
        }

        protected override void Initialize(DateTime startTime)
        {
        }

        protected override bool RunIteration(long itertionNumber, DateTime startTime)
        {
            List<Element> solution = generation.Create(evaluation.iSize);

            return CheckNewBest(solution, evaluation.dEvaluate(solution));
        }
    }
}
