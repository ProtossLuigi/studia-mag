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
    class TournamentSelection : ASelection<Tuple<double, double>>
    {
        private int size;
        private (double, double) weights;
        private Shuffler shuffler;

        public TournamentSelection(int size, (double, double)? weights = null, int? seed = null)
        {
            this.size = size;
            if (weights.HasValue)
            {
                this.weights = weights.Value;
            }
            else
            {
                this.weights = (1.0, 1.0);
            }
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
                if (candidate.Fitness.Item1 * weights.Item1 + candidate.Fitness.Item2 * weights.Item2 > winner.Fitness.Item1 * weights.Item1 + winner.Fitness.Item2 * weights.Item2)
                {
                    winner = candidate;
                }
            }

            return winner;
        }
    }
}
