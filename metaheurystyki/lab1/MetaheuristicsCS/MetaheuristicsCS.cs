using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EvaluationsCLI;
using Optimizers;
using StopConditions;


namespace MetaheuristicsCS
{
    class MetaheuristicsCS
    {
        private static int? _greedyIterations;
        private static Object lockObj = new Object();
        private static int progressCounter = 0;
        private static int totalCounter;

        private static string ResultToString<Element>(OptimizationResult<Element> result)
        {
            return result.BestValue.ToString() + ";" + result.BestTime.ToString() + ";" + result.BestIteration.ToString() + ";" + result.BestFFE.ToString();
        }

        private static void LogResults<Element>(string filename, List<OptimizationResult<Element>> results)
        {
            StreamWriter sw = new StreamWriter(filename);
            sw.WriteLine("fitness;time;iteration;FFE");
            foreach (OptimizationResult<Element> result in results)
            {
                sw.WriteLine(ResultToString(result));
            }
            sw.Close();
        }

        private static OptimizationResult<bool> RunExperiment(IEvaluation<bool> evaluation, int maxIterationNumber, int? seed, int? greedyIterations = null)
        {
            lock (evaluation)
            {
                IterationsStopCondition stopCondition = new IterationsStopCondition(evaluation.dMaxValue, maxIterationNumber);
                SamplingOptimizer<bool> randomSearch;
                if (greedyIterations.HasValue && greedyIterations.Value == 0)
                {
                    randomSearch = new BinaryRandomSearch(evaluation, stopCondition, seed);
                }
                else
                {
                    randomSearch = new GreedyRS(evaluation, stopCondition, greedyIterations, seed);
                }

                randomSearch.Run();

                return randomSearch.Result;
            }
        }

        private static List<OptimizationResult<bool>> RunExperiments(List<IEvaluation<bool>> evaluations, int maxIterationNumber, List<int?> greedyIterations, int runs ,int? seed)
        {
            var results = new List<OptimizationResult<bool>>();
            List<Task<OptimizationResult<bool>>> tasks = new List<Task<OptimizationResult<bool>>>();
            Func<object, OptimizationResult<bool>> f = (p) =>
            {
                var t = p as (IEvaluation<bool>, int, int?, int?)?;
                var result = RunExperiment(t.Value.Item1, t.Value.Item2, t.Value.Item3, t.Value.Item4);
                lock (lockObj)
                {
                    progressCounter++;
                    Console.WriteLine("Progress: {0}/{1} {2}%", progressCounter, totalCounter, 100 * progressCounter / (double)totalCounter);
                }
                return result;
            };
            lock (lockObj)
            {
                foreach (int? iterations in greedyIterations)
                {
                    bool greedy = iterations.HasValue ? iterations.Value != 0 : true;
                    foreach (IEvaluation<bool> evaluation in evaluations)
                    {
                        var p = (evaluation, maxIterationNumber, seed, iterations);
                        for (int i = 0; i < runs; i++)
                        {
                            tasks.Add(Task<OptimizationResult<bool>>.Factory.StartNew(f, p));
                        }
                    }
                }
                Console.WriteLine("{0} experiments started...", tasks.Count());
                totalCounter = tasks.Count();
            }
            foreach (var task in tasks)
            {
                results.Add(task.Result);
            }
            /*Parallel.ForEach(evaluations, evaluation =>
            {
                results.Add(RunExperiment(evaluation, greedy, maxIterationNumber, seed, greedyIterations));
            });*/
            return results;
        }

        private static void ReportOptimizationResult<Element>(OptimizationResult<Element> optimizationResult)
        {
            Console.WriteLine("value: {0}", optimizationResult.BestValue);
            Console.WriteLine("\twhen (time): {0}s", optimizationResult.BestTime);
            Console.WriteLine("\twhen (iteration): {0}", optimizationResult.BestIteration);
            Console.WriteLine("\twhen (FFE): {0}", optimizationResult.BestFFE);
        }

        private static void Lab1BinaryRandomSearch(IEvaluation<bool> evaluation, int? seed, int maxIterationNumber)
        {
            IterationsStopCondition stopCondition = new IterationsStopCondition(evaluation.dMaxValue, maxIterationNumber);
            BinaryRandomSearch randomSearch = new BinaryRandomSearch(evaluation, stopCondition, seed);

            randomSearch.Run();

            ReportOptimizationResult(randomSearch.Result);
        }

        private static void Lab1BinaryGreedyRandomSearch(IEvaluation<bool> evaluation, int? seed, int maxIterationNumber)
        {
            IterationsStopCondition stopCondition = new IterationsStopCondition(evaluation.dMaxValue, maxIterationNumber);
            GreedyRS randomSearch = new GreedyRS(evaluation, stopCondition, _greedyIterations, seed);

            randomSearch.Run();

            ReportOptimizationResult(randomSearch.Result);
        }

        private static void Lab1OneMax(int? seed)
        {
            //Lab1BinaryRandomSearch(new CBinaryOneMaxEvaluation(5), seed, 500);
            foreach (int i in new int[] { 5, 10, 50 })
            {
                Lab1BinaryGreedyRandomSearch(new CBinaryOneMaxEvaluation(i), seed, 500);
            }
        }

        private static void Lab1StandardDeceptiveConcatenation(int? seed)
        {
            //Lab1BinaryRandomSearch(new CBinaryStandardDeceptiveConcatenationEvaluation(5, 1), seed, 500);
            foreach (int i in new int[] { 1, 2, 10 })
            {
                Lab1BinaryGreedyRandomSearch(new CBinaryStandardDeceptiveConcatenationEvaluation(5, i), seed, 500);
            }
        }

        private static void Lab1BimodalDeceptiveConcatenation(int? seed)
        {
            //Lab1BinaryRandomSearch(new CBinaryBimodalDeceptiveConcatenationEvaluation(10, 1), seed, 500);
            foreach(int i in new int[] { 1, 5, 10 })
            {
                Lab1BinaryGreedyRandomSearch(new CBinaryBimodalDeceptiveConcatenationEvaluation(10, i), seed, 500);
            }
        }

        private static void Lab1IsingSpinGlass(int? seed)
        {
            //Lab1BinaryRandomSearch(new CBinaryIsingSpinGlassEvaluation(25), seed, 500);
            foreach(int i in new int[] { 25, 49, 100, 484 })
            {
                Lab1BinaryGreedyRandomSearch(new CBinaryIsingSpinGlassEvaluation(i), seed, 500);
            }
        }

        private static void Lab1NkLandscapes(int? seed)
        {
            //Lab1BinaryRandomSearch(new CBinaryNKLandscapesEvaluation(10), seed, 500);
            foreach(int i in new int[] { 10, 50, 100, 200 })
            {
                Lab1BinaryGreedyRandomSearch(new CBinaryNKLandscapesEvaluation(i), seed, 500);
            }
        }

        static void Main(string[] args)
        {
            int? seed = null;
            List<IEvaluation<bool>> evaluations = new List<IEvaluation<bool>>();
            foreach (int i in new int[] {5, 10, 20, 30, 40, 50})
            {
                evaluations.Add(new CBinaryOneMaxEvaluation(i));
            }
            foreach (int i in new int[] { 5, 10, 20, 30, 40, 50 })
            {
                evaluations.Add(new CBinaryStandardDeceptiveConcatenationEvaluation(5, i));
            }
            foreach (int i in new int[] { 10, 20, 30, 40, 50 })
            {
                evaluations.Add(new CBinaryBimodalDeceptiveConcatenationEvaluation(10, i));
            }
            foreach (int i in new int[] { 25, 49, 100, 484 })
            {
                evaluations.Add(new CBinaryIsingSpinGlassEvaluation(i));
            }
            foreach (int i in new int[] { 10, 50, 100, 200 })
            {
                evaluations.Add(new CBinaryNKLandscapesEvaluation(i));
            }
            List<int?> greedyIterations = new List<int?> { 0, 1, 5, null };
            List<OptimizationResult<bool>> results = new List<OptimizationResult<bool>>();
            results.AddRange(RunExperiments(evaluations, 50, greedyIterations, 20, seed));
            /*Console.WriteLine("No greedy done.");
            results.AddRange(RunExperiments(evaluations, true, 5000, seed, 1));
            Console.WriteLine("1 greedy done.");
            results.AddRange(RunExperiments(evaluations, true, 5000, seed, 5));
            Console.WriteLine("5 greedy done.");
            results.AddRange(RunExperiments(evaluations, true, 5000, seed, null));
            Console.WriteLine("Auto greedy done.");*/
            LogResults(".\\GreedyRS-results.csv", results);
        }
    }
}
