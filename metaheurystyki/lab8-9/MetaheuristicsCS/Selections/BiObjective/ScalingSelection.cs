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
    class ScalingSelection : ASelection<Tuple<double, double>>
    {
        private readonly int size;
        private readonly int axes;
        private readonly Shuffler shuffler;

        public ScalingSelection(int size, int axes, int? seed = null)
        {
            this.size = size;
            this.axes = axes;
            shuffler = new Shuffler(seed);
        }

        protected override void AddToNewPopulation<Element>(List<Individual<Element, Tuple<double, double>>> population,
            List<Individual<Element, Tuple<double, double>>> newPopulation)
        {
            List<int> indices = Utils.CreateIndexList(population.Count);

            if(axes == 1)
            {
                for (int i = 0; i < population.Count; ++i)
                {
                    shuffler.Shuffle(indices);

                    newPopulation.Add(new Individual<Element, Tuple<double, double>>(TournamentWinner(population, indices, (0.5, 0.5))));
                }
            }
            else
            {
                for (int i = 0; i < axes; i++)
                {
                    double angle = ((double)i / (axes - 1)) * Math.PI / 2;
                    (double, double) weights = (Math.Cos(angle), Math.Sin(angle));
                    for (int j = newPopulation.Count; j < (double)i/axes * population.Count; j++)
                    {
                        newPopulation.Add(new Individual<Element, Tuple<double, double>>(TournamentWinner(population, indices, weights)));
                    }
                }
            }
        }

        private Individual<Element, Tuple<double, double>> TournamentWinner<Element>(List<Individual<Element, Tuple<double, double>>> population, List<int> indices, (double, double) weights)
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
