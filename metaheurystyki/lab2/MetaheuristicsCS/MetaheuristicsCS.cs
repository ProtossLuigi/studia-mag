using System;
using System.Collections.Generic;
using System.Linq;
using EvaluationsCLI;
using Mutations;
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

        private static void Lab2Sphere(int? seed, int variables)
        {
            CRealSphereEvaluation sphereEvaluation = new CRealSphereEvaluation(variables);

            List<double> sigmas = Enumerable.Repeat(0.1, sphereEvaluation.iSize).ToList();

            IterationsStopCondition stopCondition = new IterationsStopCondition(sphereEvaluation.dMaxValue, 1000);
            RealGaussianMutation mutation = new RealGaussianMutation(sigmas, sphereEvaluation, seed);
            RealNullRealMutationES11Adaptation mutationAdaptation = new RealNullRealMutationES11Adaptation(mutation);

            RealEvolutionStrategy11 es11 = new RealEvolutionStrategy11(sphereEvaluation, stopCondition, mutationAdaptation, seed);

            es11.Run();

            ReportOptimizationResult(es11.Result);
        }

        private static void Lab2Sphere10(int? seed, int variables)
        {
            CRealSphere10Evaluation sphere10Evaluation = new CRealSphere10Evaluation(variables);

            List<double> sigmas = Enumerable.Repeat(0.1, sphere10Evaluation.iSize).ToList();

            IterationsStopCondition stopCondition = new IterationsStopCondition(sphere10Evaluation.dMaxValue, 1000);
            RealGaussianMutation mutation = new RealGaussianMutation(sigmas, sphere10Evaluation, seed);
            RealNullRealMutationES11Adaptation mutationAdaptation = new RealNullRealMutationES11Adaptation(mutation);

            RealEvolutionStrategy11 es11 = new RealEvolutionStrategy11(sphere10Evaluation, stopCondition, mutationAdaptation, seed);

            es11.Run();

            ReportOptimizationResult(es11.Result);
        }

        private static void Lab2Ellipsoid(int? seed, int variables)
        {
            CRealEllipsoidEvaluation ellipsoidEvaluation = new CRealEllipsoidEvaluation(variables);

            List<double> sigmas = Enumerable.Repeat(0.1, ellipsoidEvaluation.iSize).ToList();

            IterationsStopCondition stopCondition = new IterationsStopCondition(ellipsoidEvaluation.dMaxValue, 1000);
            RealGaussianMutation mutation = new RealGaussianMutation(sigmas, ellipsoidEvaluation, seed);
            RealNullRealMutationES11Adaptation mutationAdaptation = new RealNullRealMutationES11Adaptation(mutation);

            RealEvolutionStrategy11 es11 = new RealEvolutionStrategy11(ellipsoidEvaluation, stopCondition, mutationAdaptation, seed);

            es11.Run();

            ReportOptimizationResult(es11.Result);
        }

        private static void Lab2Step2Sphere(int? seed, int variables)
        {
            CRealStep2SphereEvaluation step2SphereEvaluation = new CRealStep2SphereEvaluation(variables);

            List<double> sigmas = Enumerable.Repeat(0.1, step2SphereEvaluation.iSize).ToList();

            IterationsStopCondition stopCondition = new IterationsStopCondition(step2SphereEvaluation.dMaxValue, 1000);
            RealGaussianMutation mutation = new RealGaussianMutation(sigmas, step2SphereEvaluation, seed);
            RealNullRealMutationES11Adaptation mutationAdaptation = new RealNullRealMutationES11Adaptation(mutation);

            RealEvolutionStrategy11 es11 = new RealEvolutionStrategy11(step2SphereEvaluation, stopCondition, mutationAdaptation, seed);

            es11.Run();

            ReportOptimizationResult(es11.Result);
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

            Lab2Sphere(seed, 2);
            Lab2Sphere(seed, 5);
            Lab2Sphere(seed, 10);
            Lab2Sphere10(seed, 2);
            Lab2Sphere10(seed, 5);
            Lab2Sphere10(seed, 10);
            Lab2Ellipsoid(seed, 2);
            Lab2Ellipsoid(seed, 5);
            Lab2Ellipsoid(seed, 10);
            Lab2Step2Sphere(seed, 2);
            Lab2Step2Sphere(seed, 5);
            Lab2Step2Sphere(seed, 10);
        }
    }
}
