using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvaluationsCLI;

namespace Evaluations
{
    class ShuffledStandardDeceptiveConcatenationEvaluation : CBinaryStandardDeceptiveConcatenationEvaluation
    {
        private List<int> permutation;

        public ShuffledStandardDeceptiveConcatenationEvaluation(int iBlockSize, int iNumberOfBlocks, int? seed = null)
            : base(iBlockSize, iNumberOfBlocks)
        {
            Random rng = seed.HasValue ? new Random(seed.Value) : new Random();
            permutation = Enumerable.Range(0, iSize).OrderBy(x => rng.Next()).ToList();
        }

        public override double dEvaluate(IList<bool> lSolution)
        {
            lSolution = permute(lSolution);
            return base.dEvaluate(lSolution);
        }

        private IList<bool> permute(IList<bool> solution)
        {
            List<bool> newSolution = new List<bool>(iSize);
            foreach(int i in permutation)
            {
                newSolution.Add(solution[i]);
            }
            return newSolution;
        }
    }
}
