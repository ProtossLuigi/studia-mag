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
using Utility;

namespace MetaheuristicsCS.Optimizers
{
    class RealRestartingEvolutionStrategy11 : AOptimizer<double>
    {
        private readonly ARealMutationES11Adaptation mutationAdaptation;
        private readonly RealRandomGenerator randomGeneration;
        private readonly int patience;

        private List<double> runBest = null;
        private double runBestValue;
        private int failStreak = 0;

        public RealRestartingEvolutionStrategy11(IEvaluation<double> evaluation, AStopCondition stopCondition, ARealMutationES11Adaptation mutationAdaptation, int patience, int? seed = null)
            : base(evaluation, stopCondition)
        {
            this.mutationAdaptation = mutationAdaptation;
            randomGeneration = new RealRandomGenerator(evaluation.pcConstraint, seed);
            this.patience = patience;
        }

        protected override void Initialize(DateTime startTime)
        {
            List<double> initialSolution = randomGeneration.Create(evaluation.iSize);

            CheckNewRunBest(initialSolution, evaluation.dEvaluate(initialSolution));
        }

        protected override bool RunIteration(long itertionNumber, DateTime startTime)
        {
            List<double> candidateSolution;

            if (runBest == null)
            {
                candidateSolution = randomGeneration.Create(evaluation.iSize);
            }
            else
            {
                candidateSolution = new List<double>(runBest);
            }

            mutationAdaptation.Mutation.Mutate(candidateSolution);
            double candidateValue = evaluation.dEvaluate(candidateSolution);

            mutationAdaptation.Adapt(Result.BestValue, Result.BestSolution, candidateValue, candidateSolution);

            int success = CheckNewRunBest(candidateSolution, candidateValue);

            if (success == 0)
            {
                failStreak++;
                if (failStreak > patience)
                {
                    ResetRun();
                }
                return false;
            }
            else
            {
                failStreak = 0;
                return success == 2;
            }
        }

        protected int CheckNewRunBest(List<double> solution, double value, bool onlyImprovements = true)
        {
            if (runBest == null || value > runBestValue || value == Result.BestValue && !onlyImprovements)
            {
                runBest = solution;
                runBestValue = value;
                if (CheckNewBest(solution, value, onlyImprovements))
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }

            return 0;
        }

        private void ResetRun()
        {
            runBest = null;
            failStreak = 0;
        }
    }
}
