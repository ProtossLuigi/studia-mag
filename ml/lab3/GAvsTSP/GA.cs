using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GAvsTSP
{
    class GA
    {
        public static Random rng = new Random();

        public static (List<int> , List<(double, double, double)>) Run(TSP evaluation, int generations, int popSize, double xProb, double mProb, int tourSize)
        {
            List<(double, double, double)> stats = new List<(double, double, double)>();
            List<List<int>> population = new List<List<int>>();
            while (population.Count < popSize)
            {
                population.Add(GetRandomSolution(evaluation));
            }
            List<double> scores = EvaluateAll(evaluation, population);
            stats.Add(GetPopulationStats(scores));
            for (int gen=1; gen<generations; gen++)
            {
                List<List<int>> newPopulation = new List<List<int>>();
                while (newPopulation.Count < popSize)
                {
                    var newIndividual = Tournament(population, scores, tourSize);
                    if (rng.NextDouble() < xProb)
                    {
                        newIndividual = Crossover(newIndividual, Tournament(population, scores, tourSize));
                    }
                    if (rng.NextDouble() < mProb)
                    {
                        newIndividual = Mutate(newIndividual);
                    }
                    newPopulation.Add(newIndividual);
                }
                population = newPopulation;
                scores = EvaluateAll(evaluation, population);
                stats.Add(GetPopulationStats(scores));
            }
            return (FindBest(population, scores), stats);
        }

        private static List<double> EvaluateAll(TSP evaluation, List<List<int>> population)
        {
            List<double> scores = new List<double>();
            foreach (var individual in population)
            {
                scores.Add(evaluation.Evaluate(individual));
            }
            return scores;
        }

        public static List<int> GetRandomSolution(TSP evaluation)
        {
            return Enumerable.Range(0, evaluation.size).ToList().OrderBy(x => rng.Next()).ToList();
        }

        private static (double, double, double) GetPopulationStats(List<double> scores)
        {
            return (scores.Min(), scores.Max(), scores.Average());
        }

        private static List<int> FindBest(List<List<int>> population, List<double> scores)
        {
            var maxScore = scores.Min();
            return population[scores.FindIndex((val) => val == maxScore)];
        }

        private static List<int> Tournament(List<List<int>> population, List<double> scores, int tourSize)
        {
            var sample = Enumerable.Range(0, scores.Count).ToList().OrderBy(x => rng.Next()).Take(tourSize);
            int? min = null;
            foreach (int i in sample)
            {
                if (!min.HasValue || scores[i] < scores[min.Value])
                {
                    min = i;
                }
            }
            return population[min.Value];
        }

        private static List<int> Crossover(List<int> a, List<int> b)
        {
            int start = rng.Next(0, a.Count);
            int end = rng.Next(0, a.Count);
            while (start == end)
            {
                end = rng.Next(0, a.Count);
            }
            if (start > end)
            {
                var temp = end;
                end = start;
                start = temp;
            }
            var part2 = a.GetRange(start, end - start);
            var partb = b.ToList();
            partb.RemoveAll((val) => part2.Contains(val));
            var crossovered = partb.GetRange(0, start);
            crossovered.AddRange(part2);
            crossovered.AddRange(partb.GetRange(start, a.Count - end));
            return crossovered;
        }

        private static List<int> Mutate(List<int> solution)
        {
            solution = solution.ToList();
            int start = rng.Next(0, solution.Count);
            int end = rng.Next(0, solution.Count);
            while (start == end)
            {
                end = rng.Next(0, solution.Count);
            }
            if (start > end)
            {
                var temp = end;
                end = start;
                start = temp;
            }
            var range = solution.GetRange(start, end - start);
            range.Reverse();
            solution.RemoveRange(start, end - start);
            solution.InsertRange(start, range);
            return solution;
        }
    }
}
