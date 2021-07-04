using System;
using System.Collections.Generic;

using Optimizers.PopulationOptimizers;
using Utility;

namespace Selections
{
    sealed class RouletteWheelSelection : ASelection
    {
        private readonly UniformRealRandom rng;

        public RouletteWheelSelection(int? seed = null)
        {
            rng = new UniformRealRandom(seed);
        }

        protected override void AddToNewPopulation<Element>(List<Individual<Element>> population, List<Individual<Element>> newPopulation)
        {
            List<double> accumulatedProbabilities = CreateAccumulatedProbabilities(population);

            for(int i = 0; i < population.Count; ++i)
            {
                newPopulation.Add(new Individual<Element>(SingleRouletteWheel(population, accumulatedProbabilities)));
            }
        }

        private Individual<Element> SingleRouletteWheel<Element>(List<Individual<Element>> population, List<double> accumulatedProbabilities)
        {
            double probability = rng.Next();

            int selectedIndex = 0;
            for (; selectedIndex < accumulatedProbabilities.Count - 1 && accumulatedProbabilities[selectedIndex] <= probability; ++selectedIndex) ;

            return population[selectedIndex];
        }

        private static List<double> CreateAccumulatedProbabilities<Element>(List<Individual<Element>> population)
        {
            List<double> accumulatedProbabilities = new List<double>(population.Count);

            double fitnessSum = CalculateFitnessSum(population);

            double probabilitySum = 0.0;
            foreach(Individual<Element> individual in population)
            {
                probabilitySum += individual.Fitness / fitnessSum;
                accumulatedProbabilities.Add(probabilitySum);
            }

            if(accumulatedProbabilities.Count > 0)
            {
                accumulatedProbabilities[accumulatedProbabilities.Count - 1] = 1.0;
            }

            return accumulatedProbabilities;
        }

        private static double CalculateFitnessSum<Element>(List<Individual<Element>> population)
        {
            double sum = 0.0;
            foreach(Individual<Element> individual in population)
            {
                sum += individual.Fitness;
            }

            return sum;
        }
    }
}
