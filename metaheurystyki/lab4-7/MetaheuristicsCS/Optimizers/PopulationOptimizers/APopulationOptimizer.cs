using System;
using System.Collections.Generic;

using EvaluationsCLI;
using Generators;
using Mutations;
using Selections;
using StopConditions;
using Utility;

namespace Optimizers.PopulationOptimizers
{
    abstract class APopulationOptimizer<Element> : AOptimizer<Element>
    {
        protected readonly AGenerator<Element> generator;
        protected readonly ASelection selection;
        protected readonly IMutation<Element> mutation;

        protected readonly int populationSize;
        protected List<Individual<Element>> population;

        public APopulationOptimizer(IEvaluation<Element> evaluation, AStopCondition stopCondition, AGenerator<Element> generator, 
                                    ASelection selection, IMutation<Element> mutation, int populationSize)
            : base(evaluation, stopCondition)
        {
            this.generator = generator;
            this.selection = selection;
            this.mutation = mutation;

            this.populationSize = populationSize;
            population = new List<Individual<Element>>();
        }

        protected override sealed void Initialize(DateTime startTime)
        {
            population.Clear();
            for(int i = 0; i < populationSize; ++i)
            {
                population.Add(CreateIndividual());
            }

            Evaluate();
            CheckNewBest();
        }

        protected virtual void Evaluate()
        {
            foreach(Individual<Element> individual in population)
            {
                individual.Evaluate(evaluation);
            }
        }

        protected void Select()
        {
            selection.Select(ref population);
        }

        protected void Mutate()
        {
            foreach (Individual<Element> individual in population)
            {
                individual.Mutate(mutation);
            }
        }

        protected virtual bool CheckNewBest(bool onlyImprovements = true)
        {
            Individual<Element> bestInPopulation = population[0];
            for(int i = 1; i < population.Count; ++i)
            {
                if(population[i].Fitness > bestInPopulation.Fitness)
                {
                    bestInPopulation = population[i];
                }
            }

            return CheckNewBest(bestInPopulation.Genotype, bestInPopulation.Fitness, onlyImprovements);
        }

        protected Individual<Element> CreateIndividual(List<Element> genotype = null)
        {
            if(genotype == null)
            {
                genotype = generator.Create(evaluation.iSize);
            }

            return new Individual<Element>(genotype);
        }
    }
}
