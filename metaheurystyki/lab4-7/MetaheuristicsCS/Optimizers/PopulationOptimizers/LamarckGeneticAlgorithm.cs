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
    class LamarckGeneticAlgorithm : KnapsackGeneticAlgorithm
    {
        public LamarckGeneticAlgorithm(CBinaryKnapsackEvaluation evaluation, AStopCondition stopCondition, AGenerator<bool> generator,
                                ASelection selection, ACrossover crossover, IMutation<bool> mutation, int populationSize)
            : base(evaluation, stopCondition, generator, selection, crossover, mutation, populationSize)
        { }

        protected override void Evaluate()
        {
            for(int i=0; i<populationSize; i++)
            {
                population[i] = OptimizeIndividual(population[i]);
            }
        }

        private Individual<bool> OptimizeIndividual(Individual<bool> individual)
        {
            var eval = (CBinaryKnapsackEvaluation)evaluation;
            var genotype = individual.Genotype;
            while(eval.dCalculateWeight(genotype) > eval.dCapacity)
            {
                int? toRemove = FindWorstItem(genotype, eval.lProfits);
                if (toRemove.HasValue)
                {
                    genotype[toRemove.Value] = false;
                }
                else
                {
                    break;
                }
            }
            while(eval.dCalculateWeight(genotype) < eval.dCapacity)
            {
                int? toAdd = FindBestItem(genotype, eval.lProfits, eval.lWeights);
                if (toAdd.HasValue)
                {
                    genotype[toAdd.Value] = true;
                }
                else
                {
                    break;
                }
            }
            var newIndividual = new Individual<bool>(genotype);
            newIndividual.Evaluate(evaluation);
            return newIndividual;
        }

        private int? FindWorstItem(List<bool> solution, IList<double> profits)
        {
            int? worst = null;
            for(int i=0; i<evaluation.iSize; i++)
            {
                if(solution[i] && (!worst.HasValue || (profits[i] < profits[worst.Value])))
                {
                    worst = i;
                }
            }
            return worst;
        }

        private int? FindBestItem(List<bool> solution, IList<double> profits, IList<double> weights)
        {
            int? best = null;
            double maxWeight = ((CBinaryKnapsackEvaluation)evaluation).dCapacity - ((CBinaryKnapsackEvaluation)evaluation).dCalculateWeight(solution);
            for (int i = 0; i < evaluation.iSize; i++)
            {
                if (!solution[i] && weights[i] <= maxWeight && (!best.HasValue || (profits[i] / weights[i] > profits[best.Value] / weights[best.Value])))
                {
                    best = i;
                }
            }
            return best;
        }
    }
}
