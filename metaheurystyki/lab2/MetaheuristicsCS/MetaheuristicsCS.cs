using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EvaluationsCLI;
using Mutations;
using Optimizers;
using StopConditions;


namespace MetaheuristicsCS
{
    class MetaheuristicsCS
    {
        private static Object lockObj = new Object();
        private static int progressCounter = 0;
        private static int totalCounter;

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

        private static string ResultToString<Element>(OptimizationResult<Element> result)
        {
            return result.BestValue.ToString() + "\t" + result.BestTime.ToString() + "\t" + result.BestIteration.ToString() + "\t" + result.BestFFE.ToString();
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

        private static void ReportOptimizationResult<Element>(OptimizationResult<Element> optimizationResult)
        {
            Console.WriteLine("value: {0}", optimizationResult.BestValue);
            Console.WriteLine("\twhen (time): {0}s", optimizationResult.BestTime);
            Console.WriteLine("\twhen (iteration): {0}", optimizationResult.BestIteration);
            Console.WriteLine("\twhen (FFE): {0}", optimizationResult.BestFFE);
        }

        private static List<double> AdjustSigmas(IEvaluation<double> evaluation, double defaultSigma)
        {
            List<double> newSigmas = new List<double>();
            for (int i=0; i<evaluation.iSize; i++)
            {
                double range = evaluation.pcConstraint.tGetUpperBound(i) - evaluation.pcConstraint.tGetLowerBound(i);
                newSigmas.Add(range * defaultSigma);
            }
            return newSigmas;
        }

        private static OptimizationResult<double> RunExperiment(IEvaluation<double> evaluation, int maxIterations, double sigma, bool adjustSigmas, int adaptation, int? seed = null, int archiveSize = 1, double modifier = 1.0)
        {
            lock (evaluation)
            {
                IterationsStopCondition stopCondition = new IterationsStopCondition(evaluation.dMaxValue, maxIterations);
                List<double> sigmas;
                if (adjustSigmas)
                {
                    sigmas = AdjustSigmas(evaluation, sigma);
                }
                else
                {
                    sigmas = Enumerable.Repeat(sigma, evaluation.iSize).ToList();
                }
                RealGaussianMutation mutation = new RealGaussianMutation(sigmas, evaluation, seed);
                ARealMutationES11Adaptation mutationAdaptation;
                switch (adaptation)
                {
                    case 0:
                        mutationAdaptation = new RealNullRealMutationES11Adaptation(mutation);
                        break;
                    case 1:
                        mutationAdaptation = new RealOneFifthRuleMutationES11Adaptation(archiveSize, modifier, mutation);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                RealEvolutionStrategy11 es11 = new RealEvolutionStrategy11(evaluation, stopCondition, mutationAdaptation, seed);
                es11.Run();
                return es11.Result;
            }
        }

        private static OptimizationResult<double> PrepareExperiment(object parameters)
        {
            var p = parameters as (IEvaluation<double>, int, double, bool, int, int?, int, double)?;
            if (p.HasValue)
            {
                var result = RunExperiment(p.Value.Item1, p.Value.Item2, p.Value.Item3, p.Value.Item4, p.Value.Item5, p.Value.Item6, p.Value.Item7, p.Value.Item8);
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

        private static List<OptimizationResult<double>> RunExperiments(List<IEvaluation<double>> evaluations, int maxIterations, int repeats, int start, int? seed = null)
        {
            var results = new List<OptimizationResult<double>>();
            List<Task<OptimizationResult<double>>> tasks = new List<Task<OptimizationResult<double>>>();
            lock (lockObj)
            {
                foreach (IEvaluation<double> evaluation in evaluations)
                {
                    for (int i=0; i<repeats; i++)
                    {
                        tasks.Add(new Task<OptimizationResult<double>>(PrepareExperiment, (evaluation, maxIterations, 0.1, false, 0, seed, 1, 1.0)));
                    }
                }
                foreach (IEvaluation<double> evaluation in evaluations)
                {
                    for (int i = 0; i < repeats; i++)
                    {
                        tasks.Add(new Task<OptimizationResult<double>>(PrepareExperiment, (evaluation, maxIterations, 0.1, false, 1, seed, 1, 2.0)));
                    }
                }
                foreach (IEvaluation<double> evaluation in evaluations)
                {
                    for (int i = 0; i < repeats; i++)
                    {
                        tasks.Add(new Task<OptimizationResult<double>>(PrepareExperiment, (evaluation, maxIterations, 0.1, false, 1, seed, 10, 2.0)));
                    }
                }
                foreach (IEvaluation<double> evaluation in evaluations)
                {
                    for (int i = 0; i < repeats; i++)
                    {
                        tasks.Add(new Task<OptimizationResult<double>>(PrepareExperiment, (evaluation, maxIterations, 0.01, true, 1, seed, 10, 2.0)));
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
                sw = new StreamWriter(".\\ES11-results.csv");
                sw.WriteLine("fitness\ttime\titeration\tFFE");
            }
            else
            {
                sw = new StreamWriter(".\\ES11-results.csv", true);
            }
            foreach (var task in tasks)
            {
                var result = task.Result;
                results.Add(result);
                sw.WriteLine(ResultToString(result));
                sw.Flush();
            }
            sw.Close();
            return results;
        }

        private static List<OptimizationResult<double>> RunExperimentsSynchronously(List<IEvaluation<double>> evaluations, int maxIterations, int repeats, int start, int? seed = null)
        {
            var results = new List<OptimizationResult<double>>();
            List<Task<OptimizationResult<double>>> tasks = new List<Task<OptimizationResult<double>>>();
            lock (lockObj)
            {
                foreach (IEvaluation<double> evaluation in evaluations)
                {
                    for (int i = 0; i < repeats; i++)
                    {
                        tasks.Add(new Task<OptimizationResult<double>>(PrepareExperiment, (evaluation, maxIterations, 0.01, false, 0, seed, 1, 1.0)));
                    }
                }
                foreach (IEvaluation<double> evaluation in evaluations)
                {
                    for (int i = 0; i < repeats; i++)
                    {
                        tasks.Add(new Task<OptimizationResult<double>>(PrepareExperiment, (evaluation, maxIterations, 0.01, false, 1, seed, 1, 2.0)));
                    }
                }
                foreach (IEvaluation<double> evaluation in evaluations)
                {
                    for (int i = 0; i < repeats; i++)
                    {
                        tasks.Add(new Task<OptimizationResult<double>>(PrepareExperiment, (evaluation, maxIterations, 0.01, false, 1, seed, 10, 2.0)));
                    }
                }
                foreach (IEvaluation<double> evaluation in evaluations)
                {
                    for (int i = 0; i < repeats; i++)
                    {
                        tasks.Add(new Task<OptimizationResult<double>>(PrepareExperiment, (evaluation, maxIterations, 0.01, true, 1, seed, 10, 2.0)));
                    }
                }
                totalCounter = tasks.Count() - start;
                Console.WriteLine("{0} experiments started...", totalCounter);
                tasks = tasks.GetRange(start, totalCounter);
            }
            foreach (var task in tasks)
            {
                task.RunSynchronously();
            }
            StreamWriter sw;
            if (start == 0)
            {
                sw = new StreamWriter(".\\ES11-results.csv");
                sw.WriteLine("fitness\ttime\titeration\tFFE");
            }
            else
            {
                sw = new StreamWriter(".\\ES11-results.csv", true);
            }
            foreach (var task in tasks)
            {
                var result = task.Result;
                results.Add(result);
                sw.WriteLine(ResultToString(result));
                sw.Flush();
            }
            sw.Close();
            return results;
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

        static void Main(string[] args)
        {
            int? seed = null;
            List < IEvaluation<double> > evaluations = new List<IEvaluation<double>>();
            List<int> variables = new List<int> { 2, 5, 10 };
            foreach (int variable in variables)
            {
                evaluations.Add(new CRealSphereEvaluation(variable));
            }
            foreach (int variable in variables)
            {
                evaluations.Add(new CRealSphere10Evaluation(variable));
            }
            foreach (int variable in variables)
            {
                evaluations.Add(new CRealEllipsoidEvaluation(variable));
            }
            foreach (int variable in variables)
            {
                evaluations.Add(new CRealStep2SphereEvaluation(variable));
            }
            string filename = ".\\ES11-results.csv";
            int lines = CountLines(".\\ES11-results.csv") - 1;
            if (lines < 0)
            {
                lines = 0;
            }
            List<OptimizationResult<double>> results = RunExperiments(evaluations, 500, 20, lines, seed);
            //Console.WriteLine("Experiments complete. Writing results to file...");
            //LogResults(".\\ES11-results.csv", results);
        }
    }
}
