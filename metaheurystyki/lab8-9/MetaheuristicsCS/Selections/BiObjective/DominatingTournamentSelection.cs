using DominationComparers.BiObjective;
using Optimizers.Framework.PopulationOptimizers;
using Selections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Selections.BiObjective
{
    class DominatingTournamentSelection : ASelection<Tuple<double, double>>
    {
        private readonly int size;
        private readonly IDominationComparer dominationComparer;
        private readonly Shuffler shuffler;

        public DominatingTournamentSelection(int size, IDominationComparer dominationComparer, int? seed = null)
        {
            this.size = size;
            this.dominationComparer = dominationComparer;
            shuffler = new Shuffler(seed);
        }

        protected override void AddToNewPopulation<Element>(List<Individual<Element, Tuple<double, double>>> population,
            List<Individual<Element, Tuple<double, double>>> newPopulation)
        {
            List<int> indices = Utils.CreateIndexList(population.Count);

            for (int i = 0; i < population.Count; ++i)
            {
                shuffler.Shuffle(indices);

                newPopulation.Add(new Individual<Element, Tuple<double, double>>(TournamentWinner(population, indices)));
            }
        }

        private Individual<Element, Tuple<double, double>> TournamentWinner<Element>(List<Individual<Element, Tuple<double, double>>> population, List<int> indices)
        {
            Individual<Element, Tuple<double, double>> winner = population[indices[0]];
            for (int i = 1; i < size; ++i)
            {
                var candidate = population[indices[i]];
                if (dominationComparer.Compare(candidate.Fitness, winner.Fitness) > 0)
                {
                    winner = candidate;
                }
            }
            return winner;
        }
    }
}
