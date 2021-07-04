using Crossovers;
using EvaluationsCLI;
using Generators;
using Mutations;
using Optimizers.Framework.PopulationOptimizers;
using Selections;
using StopConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizers.BiObjective
{
    class ClusteringGeneticAlgorithm<Element> : GeneticAlgorithm<Element>
    {
        private readonly int no_clusters;
        private readonly Random rng;

        public ClusteringGeneticAlgorithm(IEvaluation<Element, Tuple<double, double>> evaluation, IStopCondition stopCondition,
                                AGenerator<Element> generator, ASelection<Tuple<double, double>> selection, ACrossover crossover,
                                IMutation<Element> mutation, int populationSize, int clusters, int? seed = null)
            : base(evaluation, stopCondition, generator, selection, crossover, mutation, populationSize, seed)
        {
            no_clusters = clusters;
            rng = seed.HasValue ? new Random(seed.Value) : new Random();
        }

        protected override void Crossover()
        {
            List<List<int>> clusters = Clusterize();

            shuffler.Shuffle(indices);

            for (int i = 0; i < population.Count - 1; i += 2)
            {
                int index1 = indices[i];
                int cluster = -1;
                for(int j=0; j<no_clusters; j++)
                {
                    if (clusters[j].Contains(index1))
                    {
                        cluster = j;
                        break;
                    }
                }
                int index2 = clusters[cluster][rng.Next(clusters[cluster].Count)];

                Individual<Element, Tuple<double, double>> parent1 = population[index1];
                Individual<Element, Tuple<double, double>> parent2 = population[index2];

                List<Element> offspringGenotype1 = new List<Element>(parent1.Genotype);
                List<Element> offspringGenotype2 = new List<Element>(parent2.Genotype);

                if (crossover.Crossover(parent1.Genotype, parent2.Genotype, offspringGenotype1, offspringGenotype2))
                {
                    population[index1] = CreateIndividual(offspringGenotype1);
                    population[index2] = CreateIndividual(offspringGenotype2);
                }
            }
        }

        private List<List<int>> Clusterize()
        {
            indices.Sort((int x, int y) =>
            {
                var a = population[x];
                var b = population[y];
                double val = a.Fitness.Item2 - a.Fitness.Item1 - b.Fitness.Item2 + b.Fitness.Item1;
                return Math.Sign(val);
            });

            List<List<int>> clusters = new List<List<int>>();
            int count = 0;
            for (int i=0; i<no_clusters; i++)
            {
                clusters.Add(new List<int>());
                while (count < ((double)(i+1) / no_clusters) * populationSize)
                {
                    clusters[i].Add(indices[count]);
                    count++;
                }
            }

            return clusters;
        }
    }
}
