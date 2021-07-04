using EvaluationsCLI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluations
{
    class NegativeBinaryKnapsackEvaluation : CBinaryKnapsackEvaluation
    {
        public NegativeBinaryKnapsackEvaluation(EBinaryKnapsackInstance eInstance)
            : base(eInstance) { }

        public override double dEvaluate(IList<bool> lSolution)
        {
            if (dCalculateWeight(lSolution) > dCapacity)
            {
                return -base.dEvaluate(lSolution);
            }
            else
            {
                return base.dEvaluate(lSolution);
            }
        }
    }
}
