using EvaluationsCLI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluations
{
    public class PenalizingBinaryKnapsackEvaluation : CBinaryKnapsackEvaluation
    {
        public PenalizingBinaryKnapsackEvaluation(EBinaryKnapsackInstance eInstance)
            : base(eInstance) { }

        public override double dEvaluate(IList<bool> lSolution)
        {
            double val = base.dEvaluate(lSolution);
            val -= Penalize(val, lSolution);
            return val;
        }

        private double Penalize(double score, IList<bool> lSolution)
        {
            double weight = dCalculateWeight(lSolution);
            if(weight <= dCapacity)
            {
                return 0.0;
            }
            double punishment = score;
            for(int i=0; i<iSize; i++)
            {
                if(lSolution[i] && weight - lWeights[i] <= dCapacity && lProfits[i] < score)
                {
                    punishment = lProfits[i];
                }
            }
            return punishment + 1;
        }
    }
}
