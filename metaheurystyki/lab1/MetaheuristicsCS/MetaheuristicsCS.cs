using System;

using EvaluationsCLI;
using Optimizers;
using StopConditions;


namespace MetaheuristicsCS
{
    class MetaheuristicsCS
    {
        private static int? _greedyIterations;

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
            Console.WriteLine("0");
            _greedyIterations = 0;
            Lab1StandardDeceptiveConcatenation(seed);
            Console.WriteLine("1");
            _greedyIterations = 1;
            Lab1StandardDeceptiveConcatenation(seed);
            Console.WriteLine("5");
            _greedyIterations = 5;
            Lab1StandardDeceptiveConcatenation(seed);
            Console.WriteLine("variable");
            _greedyIterations = null;
            Lab1StandardDeceptiveConcatenation(seed);
        }
    }
}
