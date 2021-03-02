using System;

using EvaluationsCLI;
using Optimizers;
using StopConditions;


namespace MetaheuristicsCS
{
    class MetaheuristicsCS
    {
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

        private static void Lab1OneMax(int? seed)
        {
            Lab1BinaryRandomSearch(new CBinaryOneMaxEvaluation(5), seed, 500);
        }

        private static void Lab1StandardDeceptiveConcatenation(int? seed)
        {
            Lab1BinaryRandomSearch(new CBinaryStandardDeceptiveConcatenationEvaluation(5, 1), seed, 500);
        }

        private static void Lab1BimodalDeceptiveConcatenation(int? seed)
        {
            Lab1BinaryRandomSearch(new CBinaryBimodalDeceptiveConcatenationEvaluation(10, 1), seed, 500);
        }

        private static void Lab1IsingSpinGlass(int? seed)
        {
            Lab1BinaryRandomSearch(new CBinaryIsingSpinGlassEvaluation(25), seed, 500);
        }

        private static void Lab1NkLandscapes(int? seed)
        {
            Lab1BinaryRandomSearch(new CBinaryNKLandscapesEvaluation(10), seed, 500);
        }

        static void Main(string[] args)
        {
            int? seed = null;

            Lab1OneMax(seed);
            Lab1StandardDeceptiveConcatenation(seed);
            Lab1BimodalDeceptiveConcatenation(seed);
            Lab1IsingSpinGlass(seed);
            Lab1NkLandscapes(seed);
        }
    }
}
