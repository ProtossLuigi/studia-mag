using EvaluationsCLI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Generations
{
    class BinaryGreedyRandomGeneration : AGeneration<bool>
    {
        private int? _greedyIterations;
        private BinaryRandomGeneration brg;
        private GreedyAlgorithm ga;

        public BinaryGreedyRandomGeneration(IEvaluation<bool> evaluation, int? greedyIterations = null, int? seed = null)
            : base(evaluation.pcConstraint)
        {
            _greedyIterations = greedyIterations;
            brg = new BinaryRandomGeneration(evaluation.pcConstraint, seed);
            ga = new GreedyAlgorithm(evaluation, seed);
        }

        public override List<bool> Fill(List<bool> solution)
        {
            solution = brg.Fill(solution);
            if (_greedyIterations.HasValue)
            {
                for(int i=0; i<_greedyIterations.Value; i++)
                {
                    solution = ga.Greedy(solution);
                }
            }
            else
            {
                List<bool> lastSolution;
                do
                {
                    lastSolution = solution;
                    solution = ga.Greedy(lastSolution);
                } while (solution != lastSolution);
            }
            return solution;
        }
    }
}
