using Crossovers;
using EvaluationsCLI;
using Generators;
using Mutations;
using Selections;
using StopConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizers.PopulationOptimizers
{
    class ResettingGA<Element> : GeneticAlgorithm<Element>
    {
        Random rng;
        int patience;
        int badStreak = 0;

        public ResettingGA(IEvaluation<Element> evaluation, AStopCondition stopCondition, AGenerator<Element> generator,
                                ASelection selection, ACrossover crossover, IMutation<Element> mutation, int populationSize, int patience, int? seed = null)
            : base(evaluation, stopCondition, generator, selection, crossover, mutation, populationSize)
        {
            this.patience = patience;
            rng = seed.HasValue ? new Random(seed.Value) : new Random();
        }

        protected override bool RunIteration(long itertionNumber, DateTime startTime)
        {
            Select();

            Crossover();
            Evaluate();

            bool foundNewBest = CheckNewBest();

            Mutate();
            Evaluate();

            foundNewBest = CheckNewBest() || foundNewBest;

            if (CheckStuck(foundNewBest))
            {
                population = population.OrderBy(x => rng.Next()).ToList();
                for(int i=0; i<populationSize/2; i++)
                {
                    population[i] = new Individual<Element>(generator.Create(evaluation.iSize));
                }
            }

            return foundNewBest;
        }

        private bool CheckStuck(bool newBest)
        {
            if (newBest)
            {
                badStreak = 0;
            }
            else
            {
                badStreak++;
                if(badStreak > patience)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
