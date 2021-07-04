using EvaluationsCLI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluations
{
    class CheckingBinaryKnapsackEvaluation : CBinaryKnapsackEvaluation
    {
        public CheckingBinaryKnapsackEvaluation(EBinaryKnapsackInstance eInstance)
            : base(eInstance) { }

        public override double dEvaluate(IList<bool> lSolution)
        {
            //Console.WriteLine(dCalculateWeight(lSolution).ToString());
            if(dCalculateWeight(lSolution) > dCapacity)
            {
                return 0;
            }
            else
            {
                return base.dEvaluate(lSolution);
            }
        }
    }
}
