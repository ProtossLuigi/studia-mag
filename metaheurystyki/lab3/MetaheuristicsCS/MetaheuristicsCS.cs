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

        private static void Lab3CMAES(IEvaluation<double> evaluation, int? seed)
        {
            IterationsStopCondition stopCondition = new IterationsStopCondition(evaluation.dMaxValue, 1000);

            CMAES cmaes = new CMAES(evaluation, stopCondition, 1, seed);

            cmaes.Run();

            ReportOptimizationResult(cmaes.Result);
        }

        private static void Lab3SphereCMAES(int? seed)
        {
            Lab3CMAES(new CRealSphereEvaluation(10), seed);
        }

        private static void Lab3Sphere10CMAES(int? seed)
        {
            Lab3CMAES(new CRealSphere10Evaluation(10), seed);
        }

        private static void Lab3EllipsoidCMAES(int? seed)
        {
            Lab3CMAES(new CRealEllipsoidEvaluation(10), seed);
        }

        private static void Lab3Step2SphereCMAES(int? seed)
        {
            Lab3CMAES(new CRealStep2SphereEvaluation(10), seed);
        }

        private static void Lab3RastriginCMAES(int? seed)
        {
            Lab3CMAES(new CRealRastriginEvaluation(10), seed);
        }

        private static void Lab3AckleyCMAES(int? seed)
        {
            Lab3CMAES(new CRealAckleyEvaluation(10), seed);
        }

        private static void Lab2Sphere(int? seed)
        {
            CRealSphereEvaluation sphereEvaluation = new CRealSphereEvaluation(2);

            List<double> sigmas = Enumerable.Repeat(0.1, sphereEvaluation.iSize).ToList();

            IterationsStopCondition stopCondition = new IterationsStopCondition(sphereEvaluation.dMaxValue, 1000);
            RealGaussianMutation mutation = new RealGaussianMutation(sigmas, sphereEvaluation, seed);
            RealNullRealMutationES11Adaptation mutationAdaptation = new RealNullRealMutationES11Adaptation(mutation);

            RealEvolutionStrategy11 es11 = new RealEvolutionStrategy11(sphereEvaluation, stopCondition, mutationAdaptation, seed);

            es11.Run();

            ReportOptimizationResult(es11.Result);
        }

        private static void Lab2Sphere10(int? seed)
        {
            CRealSphere10Evaluation sphere10Evaluation = new CRealSphere10Evaluation(2);

            List<double> sigmas = Enumerable.Repeat(0.1, sphere10Evaluation.iSize).ToList();

            IterationsStopCondition stopCondition = new IterationsStopCondition(sphere10Evaluation.dMaxValue, 1000);
            RealGaussianMutation mutation = new RealGaussianMutation(sigmas, sphere10Evaluation, seed);
            RealNullRealMutationES11Adaptation mutationAdaptation = new RealNullRealMutationES11Adaptation(mutation);

            RealEvolutionStrategy11 es11 = new RealEvolutionStrategy11(sphere10Evaluation, stopCondition, mutationAdaptation, seed);

            es11.Run();

            ReportOptimizationResult(es11.Result);
        }

        private static void Lab2Ellipsoid(int? seed)
        {
            CRealEllipsoidEvaluation ellipsoidEvaluation = new CRealEllipsoidEvaluation(2);

            List<double> sigmas = Enumerable.Repeat(0.1, ellipsoidEvaluation.iSize).ToList();

            IterationsStopCondition stopCondition = new IterationsStopCondition(ellipsoidEvaluation.dMaxValue, 1000);
            RealGaussianMutation mutation = new RealGaussianMutation(sigmas, ellipsoidEvaluation, seed);
            RealNullRealMutationES11Adaptation mutationAdaptation = new RealNullRealMutationES11Adaptation(mutation);

            RealEvolutionStrategy11 es11 = new RealEvolutionStrategy11(ellipsoidEvaluation, stopCondition, mutationAdaptation, seed);

            es11.Run();

            ReportOptimizationResult(es11.Result);
        }

        private static void Lab2Step2Sphere(int? seed)
        {
            CRealStep2SphereEvaluation step2SphereEvaluation = new CRealStep2SphereEvaluation(2);

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

            Lab3SphereCMAES(seed);
            Lab3Sphere10CMAES(seed);
            Lab3EllipsoidCMAES(seed);
            Lab3Step2SphereCMAES(seed);
            Lab3RastriginCMAES(seed);
            Lab3AckleyCMAES(seed);
            Console.ReadKey();
            /*
            Lab2Sphere(seed);
            Lab2Sphere10(seed);
            Lab2Ellipsoid(seed);
            Lab2Step2Sphere(seed);

            Lab1OneMax(seed);
            Lab1StandardDeceptiveConcatenation(seed);
            Lab1BimodalDeceptiveConcatenation(seed);
            Lab1IsingSpinGlass(seed);
            Lab1NkLandscapes(seed);*/
        }
    }
}
