using EvaluationsCLI;
using Generators;
using Mutations;
using Optimizers;
using StopConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaheuristicsCS.Optimizers
{
    class PoorMansCMAES : AOptimizer<double>
    {
        private readonly ARealMutationES11Adaptation mutationAdaptation;
        private readonly RealRandomGenerator randomGeneration;
        private readonly int memorySize;

        private List<(double, List<double>)> bestSolutions;

        public PoorMansCMAES(IEvaluation<double> evaluation, AStopCondition stopCondition, ARealMutationES11Adaptation mutationAdaptation, int memorySize, int? seed = null)
            : base(evaluation, stopCondition)
        {
            this.mutationAdaptation = mutationAdaptation;
            randomGeneration = new RealRandomGenerator(evaluation.pcConstraint, seed);
            this.memorySize = memorySize;
            bestSolutions = new List<(double, List<double>)>();
        }

        protected override void Initialize(DateTime startTime)
        {
            List<double> initialSolution = randomGeneration.Create(evaluation.iSize);
            double value = evaluation.dEvaluate(initialSolution);
            AddSolution(initialSolution, value);

            CheckNewBest(initialSolution, value);
        }

        protected override bool RunIteration(long itertionNumber, DateTime startTime)
        {
            List<double> candidateSolution = GetMiddlePoint();

            mutationAdaptation.Mutation.Mutate(candidateSolution);
            double candidateValue = evaluation.dEvaluate(candidateSolution);
            AddSolution(candidateSolution, candidateValue);

            mutationAdaptation.Adapt(Result.BestValue, Result.BestSolution, candidateValue, candidateSolution);

            return CheckNewBest(candidateSolution, candidateValue);
        }

        private void AddSolution(List<double> solution, double value)
        {
            bestSolutions.Add((value, solution));
            bestSolutions.Sort((x, y) =>
            {
                if (x.Item1 == y.Item1)
                {
                    return 0;
                }
                else if (x.Item1 < y.Item1)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            });
            while (bestSolutions.Count > memorySize)
            {
                bestSolutions.RemoveAt(memorySize);
            }
        }

        private List<List<double>> GetBestSolutions()
        {
            List<List<double>> solutions = new List<List<double>>();
            foreach (var p in bestSolutions)
            {
                solutions.Add(p.Item2);
            }
            return solutions;
        }

        private List<double> GetMiddlePoint()
        {
            List<double> middlePoint = new List<double>();
            var positions = GetBestSolutions()
                .SelectMany(inner => inner.Select((item, index) => new { item, index }))
                .GroupBy(i => i.index, i => i.item)
                .Select(g => g.ToList())
                .ToList();
            foreach (List<double> position in positions)
            {
                middlePoint.Add(position.Average());
            }
            if (bestSolutions.Count == memorySize)
            {
                for (int dim = 0; dim < evaluation.iSize; dim++)
                {
                    double sumOfSquaresOfDifferences = positions[dim].Select(val => (val - middlePoint[dim]) * (val - middlePoint[dim])).Sum();
                    mutationAdaptation.Mutation.Sigmas(dim, Math.Sqrt(sumOfSquaresOfDifferences / memorySize));
                }
            }
            return middlePoint;
        }
    }
}
