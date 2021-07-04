using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EvaluationsCLI;
using MetaheuristicsCS.Optimizers;
using Mutations;
using Optimizers;
using StopConditions;


namespace MetaheuristicsCS
{
    class MetaheuristicsCS
    {
        private static Object lockObj = new Object();
        private static int totalCounter = 0;
        private static int progressCounter = 0;

        private static void ReportOptimizationResult<Element>(OptimizationResult<Element> optimizationResult)
        {
            Console.WriteLine("value: {0}", optimizationResult.BestValue);
            Console.WriteLine("\twhen (time): {0}s", optimizationResult.BestTime);
            Console.WriteLine("\twhen (iteration): {0}", optimizationResult.BestIteration);
            Console.WriteLine("\twhen (FFE): {0}", optimizationResult.BestFFE);
        }

        private static string ResultToString(string info, string method, OptimizationResult<double> result)
        {
            return info + ";" + method + ";" + result.BestValue.ToString() + ";" + result.BestTime.ToString() + ";" + result.BestIteration.ToString() + ";" + result.BestFFE.ToString();
        }

        private static string RunExperiment(IEvaluation<double> evaluation, string method, string problemInfo, int maxIterations, int parameter, int? seed = null)
        {
            lock (evaluation)
            {
                IterationsStopCondition stopCondition = new IterationsStopCondition(evaluation.dMaxValue, maxIterations);

                if (method == "RS")
                {
                    RealRandomSearch rs = new RealRandomSearch(evaluation, stopCondition, seed);
                    rs.Run();
                    return ResultToString(problemInfo, method, rs.Result);
                }
                else if (method == "ES11")
                {
                    List<double> sigmas = Enumerable.Repeat(0.1, evaluation.iSize).ToList();
                    RealGaussianMutation mutation = new RealGaussianMutation(sigmas, evaluation, seed);
                    RealNullRealMutationES11Adaptation mutationAdaptation = new RealNullRealMutationES11Adaptation(mutation);

                    RealEvolutionStrategy11 es11 = new RealEvolutionStrategy11(evaluation, stopCondition, mutationAdaptation, seed);

                    es11.Run();

                    return ResultToString(problemInfo, method, es11.Result);
                }
                else if (method == "RestartingES11")
                {
                    List<double> sigmas = Enumerable.Repeat(0.1, evaluation.iSize).ToList();
                    RealGaussianMutation mutation = new RealGaussianMutation(sigmas, evaluation, seed);
                    RealNullRealMutationES11Adaptation mutationAdaptation = new RealNullRealMutationES11Adaptation(mutation);

                    RealRestartingEvolutionStrategy11 es11 = new RealRestartingEvolutionStrategy11(evaluation, stopCondition, mutationAdaptation, parameter, seed);

                    es11.Run();

                    return ResultToString(problemInfo, method, es11.Result);
                }
                else if (method == "PoorMansCMAES")
                {
                    List<double> sigmas = Enumerable.Repeat(0.1, evaluation.iSize).ToList();
                    RealGaussianMutation mutation = new RealGaussianMutation(sigmas, evaluation, seed);
                    RealNullRealMutationES11Adaptation mutationAdaptation = new RealNullRealMutationES11Adaptation(mutation);

                    PoorMansCMAES pmcmaes = new PoorMansCMAES(evaluation, stopCondition, mutationAdaptation, parameter, seed);

                    pmcmaes.Run();

                    return ResultToString(problemInfo, method, pmcmaes.Result);
                }
                else if (method == "CMAES")
                {
                    CMAES cmaes = new CMAES(evaluation, stopCondition, 1, seed);

                    cmaes.Run();

                    return ResultToString(problemInfo, method, cmaes.Result);
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        private static string PrepareExperiment(object parameters)
        {
            var p = parameters as (IEvaluation<double>, string, string, int, int, int?)?;
            if (p.HasValue)
            {
                var result = RunExperiment(p.Value.Item1, p.Value.Item2, p.Value.Item3, p.Value.Item4, p.Value.Item5, p.Value.Item6);
                lock (lockObj)
                {
                    progressCounter++;
                    Console.WriteLine("Progress: {0}/{1} {2}%", progressCounter, totalCounter, 100 * progressCounter / (double)totalCounter);
                }
                return result;
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        private static void RunExperiments(List<(IEvaluation<double>, string)> evaluations, List<string> methods, int maxIterations, int repeats, int start, string filename, int? seed = null)
        {
            var results = new List<OptimizationResult<double>>();
            List<Task<string>> tasks = new List<Task<string>>();
            lock (lockObj)
            {
                foreach(string method in methods)
                {
                    foreach (var evaluation in evaluations)
                    {
                        for (int i = 0; i < repeats; i++)
                        {
                            tasks.Add(new Task<string>(PrepareExperiment, (evaluation.Item1, method, evaluation.Item2, maxIterations, 10, seed)));
                        }
                    }
                }
                totalCounter = tasks.Count() - start;
                Console.WriteLine("{0} experiments started...", totalCounter);
                tasks = tasks.GetRange(start, totalCounter);
            }
            foreach (var task in tasks)
            {
                task.Start();
            }
            StreamWriter sw;
            if (start == 0)
            {
                sw = new StreamWriter(filename);
                sw.WriteLine("problem;variables;method;fitness;time;iteration;FFE");
            }
            else
            {
                sw = new StreamWriter(filename, true);
            }
            foreach (var task in tasks)
            {
                var result = task.Result;
                sw.WriteLine(result);
                sw.Flush();
            }
            sw.Close();
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

        private static int CountLines(string filename)
        {
            if (File.Exists(filename))
            {
                return File.ReadLines(filename).Count();
            }
            else
            {
                return 0;
            }
        }

        static void Main(string[] args)
        {
            int? seed = null;
            List<(IEvaluation<double>, string)> evaluations = new List<(IEvaluation<double>, string)>();
            List<int> variables = new List<int> { 2, 5, 10 };
            foreach (int variable in variables)
            {
                evaluations.Add((new CRealSphereEvaluation(variable), "sphere;" + variable.ToString()));
            }
            foreach (int variable in variables)
            {
                evaluations.Add((new CRealSphere10Evaluation(variable), "sphere10;" + variable.ToString()));
            }
            foreach (int variable in variables)
            {
                evaluations.Add((new CRealEllipsoidEvaluation(variable), "ellipsoid;" + variable.ToString()));
            }
            foreach (int variable in variables)
            {
                evaluations.Add((new CRealStep2SphereEvaluation(variable), "step2sphere;" + variable.ToString()));
            }
            foreach (int variable in variables)
            {
                evaluations.Add((new CRealRastriginEvaluation(variable), "rastrigin;" + variable.ToString()));
            }
            foreach (int variable in variables)
            {
                evaluations.Add((new CRealAckleyEvaluation(variable), "ackley;" + variable.ToString()));
            }
            List<string> methods = new List<string> { "RS", "ES11", "RestartingES11", "PoorMansCMAES", "CMAES" };
            string filename = ".\\CMAES-results.csv";
            int lines = CountLines(filename) - 1;
            if (lines < 0)
            {
                lines = 0;
            }
            RunExperiments(evaluations, methods, 1000, 20, lines, filename, seed);
        }
    }
}
