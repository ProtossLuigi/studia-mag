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
    class KnapsackGeneticAlgorithm : GeneticAlgorithm<bool>
    {
        public KnapsackGeneticAlgorithm(CBinaryKnapsackEvaluation evaluation, AStopCondition stopCondition, AGenerator<bool> generator,
                                ASelection selection, ACrossover crossover, IMutation<bool> mutation, int populationSize)
            : base(evaluation, stopCondition, generator, selection, crossover, mutation, populationSize)
        {
            var emptySolution = Enumerable.Repeat(false, evaluation.iSize).ToList();
            CheckNewBest(emptySolution, 0.0, true);
        }

        protected override bool CheckNewBest(bool onlyImprovements = true)
        {
            var eval = ((CBinaryKnapsackEvaluation)evaluation);
            Individual<bool> bestInPopulation = null;
            for (int i = 1; i < population.Count; ++i)
            {
                if ((bestInPopulation == null || population[i].Fitness > bestInPopulation.Fitness) && eval.dCalculateWeight(population[i].Genotype) <= eval.dCapacity)
                {
                    bestInPopulation = population[i];
                }
            }

            if(bestInPopulation == null)
            {
                return false;
            }

            return CheckNewBest(bestInPopulation.Genotype, bestInPopulation.Fitness, onlyImprovements);
        }
    }
}
