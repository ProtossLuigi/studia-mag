using System;
using System.Collections.Generic;

using Crossovers;
using EvaluationsCLI;
using Generators;
using Mutations;
using Selections;
using StopConditions;
using Utility;

namespace Optimizers.PopulationOptimizers
{
    class GeneticAlgorithm<Element> : APopulationOptimizer<Element>
    {
        protected ACrossover crossover;

        public GeneticAlgorithm(IEvaluation<Element> evaluation, AStopCondition stopCondition, AGenerator<Element> generator, 
                                ASelection selection, ACrossover crossover, IMutation<Element> mutation, int populationSize)
            : base(evaluation, stopCondition, generator, selection, mutation, populationSize)
        {
            this.crossover = crossover;
        }

        protected override bool RunIteration(long itertionNumber, DateTime startTime)
        {
            Select();

            Crossover();
            Evaluate();

            bool foundNewBest = CheckNewBest();

            Mutate();
            Evaluate();

            return CheckNewBest() || foundNewBest;
        }

        protected void Crossover()
        {
            for(int i = 0; i < population.Count; i += 2)
            {
                Individual<Element> parent1 = population[i];
                Individual<Element> parent2 = population[i + 1];

                List<Element> offspringGenotype1 = new List<Element>(parent1.Genotype);
                List<Element> offspringGenotype2 = new List<Element>(parent2.Genotype);

                if(crossover.Crossover(parent1.Genotype, parent2.Genotype, offspringGenotype1, offspringGenotype2))
                {
                    population[i]     = CreateIndividual(offspringGenotype1);
                    population[i + 1] = CreateIndividual(offspringGenotype2);
                }
            }
        }
    }
}
