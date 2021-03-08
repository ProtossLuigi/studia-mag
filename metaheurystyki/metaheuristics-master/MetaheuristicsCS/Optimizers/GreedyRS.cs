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
    class GreedyRS : AOptimizer<bool>
    {
        BinaryRandomGeneration brg;

        public GreedyRS(IEvaluation<bool> evaluation, AStopCondition stopCondition, int? seed = null)
            : base(evaluation, stopCondition)
        {
            brg = new BinaryRandomGeneration(evaluation.pcConstraint, seed);
        }


        protected override void Initialize(DateTime startTime)
        {
            throw new NotImplementedException();
        }

        protected override bool RunIteration(long itertionNumber, DateTime startTime)
        {
            throw new NotImplementedException();
        }
    }
}
