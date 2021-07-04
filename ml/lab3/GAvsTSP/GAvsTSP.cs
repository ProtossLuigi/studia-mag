using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GAvsTSP
{
    class GAvsTSP
    {
        private static string[] vrps;

        static void Main(string[] args)
        {
            vrps = Directory.GetFiles("..\\..\\..\\vrps\\");
            /*TSP evaluation = VRPLoader.readTSP(vrps[2]);
            int repeats = 20;
            TestCrossover(evaluation, repeats);
            TestMutation(evaluation, repeats);
            TestPopSize(evaluation, repeats);
            TestGenerations(evaluation, repeats);
            TestTournament(evaluation, repeats);*/
            //CompareMethods();
            LogStats();
        }

        private static void TestGreedy()
        {
            TSP tsp = VRPLoader.readTSP(vrps[0]);
            var solution = GreedyAlgorithm.GenerateGreedySolution(tsp, 0);
            Console.WriteLine("Solution: " + ListToString(solution));
            Console.WriteLine("Distance: " + tsp.Evaluate(solution).ToString());
        }

        private static void TestGA()
        {
            TSP tsp = VRPLoader.readTSP(vrps[0]);
            List<(double, double, double)> stats;
            List<int> bestSolution;
            (bestSolution, stats) = GA.Run(tsp, 100, 100, 0.7, 0.1, 5);
            Console.WriteLine("Solution: " + ListToString(bestSolution));
            Console.WriteLine("Distance: " + tsp.Evaluate(bestSolution).ToString());
        }

        private static List<double> TestCrossover(TSP evaluation, int repeats)
        {
            List<(double, double, double)> stats;
            List<int> bestSolution;
            List<double> range = new List<double>();
            List<double> results = new List<double>();
            for (int i=0; i<10; i++)
            {
                range.Add(0.1 * (i + 1));
            }
            foreach (double prob in range)
            {
                List<double> scores = new List<double>();
                for (int i=0; i<repeats; i++)
                {
                    (bestSolution, stats) = GA.Run(evaluation, 100, 100, prob, 0.1, 5);
                    scores.Add(stats[stats.Count - 1].Item1);
                }
                results.Add(Median(scores));
            }
            File.WriteAllText(".\\crossover-test.csv", ResultsToString(range, results));
            return results;
        }

        private static List<double> TestMutation(TSP evaluation, int repeats)
        {
            List<(double, double, double)> stats;
            List<int> bestSolution;
            List<double> range = new List<double>();
            List<double> results = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                range.Add(0.1 * (i + 1));
            }
            foreach (double prob in range)
            {
                List<double> scores = new List<double>();
                for (int i = 0; i < repeats; i++)
                {
                    (bestSolution, stats) = GA.Run(evaluation, 100, 100, 0.7, prob, 5);
                    scores.Add(stats[stats.Count - 1].Item1);
                }
                results.Add(Median(scores));
            }
            File.WriteAllText(".\\mutation-test.csv", ResultsToString(range, results));
            return results;
        }

        private static List<double> TestPopSize(TSP evaluation, int repeats)
        {
            List<(double, double, double)> stats;
            List<int> bestSolution;
            List<int> range = new List<int>();
            List<double> results = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                range.Add(100 * (i + 1));
            }
            foreach (int prob in range)
            {
                List<double> scores = new List<double>();
                for (int i = 0; i < repeats; i++)
                {
                    (bestSolution, stats) = GA.Run(evaluation, prob, 100, 0.7, 0.1, 5);
                    scores.Add(stats[stats.Count - 1].Item1);
                }
                results.Add(Median(scores));
            }
            File.WriteAllText(".\\popsize-test.csv", ResultsToString(range, results));
            return results;
        }

        private static List<double> TestGenerations(TSP evaluation, int repeats)
        {
            List<(double, double, double)> stats;
            List<int> bestSolution;
            List<int> range = new List<int>();
            List<double> results = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                range.Add(100 * (i + 1));
            }
            foreach (int prob in range)
            {
                List<double> scores = new List<double>();
                for (int i = 0; i < repeats; i++)
                {
                    (bestSolution, stats) = GA.Run(evaluation, 100, prob, 0.7, 0.1, 5);
                    scores.Add(stats[stats.Count - 1].Item1);
                }
                results.Add(Median(scores));
            }
            File.WriteAllText(".\\generation-test.csv", ResultsToString(range, results));
            return results;
        }

        private static List<double> TestTournament(TSP evaluation, int repeats)
        {
            List<(double, double, double)> stats;
            List<int> bestSolution;
            List<int> range = new List<int>();
            List<double> results = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                range.Add(5 * (i + 1));
            }
            foreach (int prob in range)
            {
                List<double> scores = new List<double>();
                for (int i = 0; i < repeats; i++)
                {
                    (bestSolution, stats) = GA.Run(evaluation, 100, prob, 0.7, 0.1, 5);
                    scores.Add(stats[stats.Count - 1].Item1);
                }
                results.Add(Median(scores));
            }
            File.WriteAllText(".\\tournament-test.csv", ResultsToString(range, results));
            return results;
        }

        private static void CompareMethods()
        {
            string s = "problem;method;cost\n";
            foreach (string filename in vrps)
            {
                TSP evaluation = VRPLoader.readTSP(filename);

                List<double> scores = new List<double>();
                for (int i=0; i<100; i++)
                {
                    scores.Add(evaluation.Evaluate(GA.GetRandomSolution(evaluation)));
                }
                s += filename + ";random;" + Median(scores).ToString() + "\n";

                scores = new List<double>();
                for (int i = 0; i < evaluation.size; i++)
                {
                    scores.Add(evaluation.Evaluate(GreedyAlgorithm.GenerateGreedySolution(evaluation, i)));
                }
                s += filename + ";greedy;" + scores.Average().ToString() + "\n";

                scores = new List<double>();
                for (int i = 0; i < 10; i++)
                {
                    List<int> solution;
                    (solution, _) = GA.Run(evaluation, 100, 100, 0.6, 0.3, 10);
                    scores.Add(evaluation.Evaluate(solution));
                }
                s += filename + ";genetic;" + Median(scores).ToString() + "\n";
            }
            File.WriteAllText("comparison-test.csv", s);
        }

        private static void LogStats()
        {
            string s = "problem;generation;best;worst;average\n";
            foreach (string filename in vrps)
            {
                TSP evaluation = VRPLoader.readTSP(filename);
                List<(double, double, double)> stats;
                (_, stats) = GA.Run(evaluation, 100, 100, 0.6, 0.3, 10);
                for (int i=0; i<stats.Count; i++)
                {
                    s += filename + ";" + i.ToString() + ";" + stats[i].Item1.ToString() + ";" + stats[i].Item2.ToString() + ";" + stats[i].Item3.ToString() + "\n";
                }
            }
            File.WriteAllText("stats-test.csv", s);
        }

        private static double Median(List<double> values)
        {
            values.Sort();
            return values[values.Count / 2];
        }

        private static string ResultsToString<Element>(List<Element> column1, List<double> column2)
        {
            string s = "parameter;cost\n";
            for (int i=0; i<column1.Count; i++)
            {
                s += column1[i].ToString() + ";" + column2[i].ToString() + "\n";
            }
            return s;
        }

        private static string ListToString<Element>(List<Element> list)
        {
            string s = "";
            foreach (var item in list)
            {
                s += item.ToString() + " ";
            }
            return s;
        }
    }
}
