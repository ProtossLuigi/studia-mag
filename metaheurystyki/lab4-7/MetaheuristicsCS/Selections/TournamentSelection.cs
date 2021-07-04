using System.Collections.Generic;

using Optimizers.PopulationOptimizers;
using Utility;

namespace Selections
{
    sealed class TournamentSelection : ASelection
    {
        private readonly UniformIntegerRandom rng;
        private readonly Shuffler shuffler;

        private readonly int size;

        public TournamentSelection(int size, int? seed = null)
        {
            rng = new UniformIntegerRandom(seed);
            shuffler = new Shuffler(seed);

            this.size = size;
        }

        protected override void AddToNewPopulation<Element>(List<Individual<Element>> population, List<Individual<Element>> newPopulation)
        {
            List<int> indices = Utils.CreateIndexList(population.Count);

            for(int i = 0; i < population.Count; ++i)
            {
                shuffler.Shuffle(indices);

                newPopulation.Add(new Individual<Element>(TournamentWinner(population, indices)));
            }
        }

        private Individual<Element> TournamentWinner<Element>(List<Individual<Element>> population, List<int> indices)
        {
            Individual<Element> winner = population[indices[0]];
            for(int i = 1; i < size; ++i)
            {
                if (population[indices[i]].Fitness > winner.Fitness)
                {
                    winner = population[indices[i]];
                }
            }

            return winner;
        }
    }
}
