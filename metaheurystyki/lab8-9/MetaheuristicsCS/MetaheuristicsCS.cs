using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Crossovers;
using DominationComparers.BiObjective;
using EvaluationsCLI;
using Generators;
using Mutations;
using Optimizers.SingleObjective;
using Selections;
using Selections.BiObjective;
using Selections.SingleObjective;
using StopConditions;
using Utility;
using BiObjective = Optimizers.BiObjective;


namespace MetaheuristicsCS
{
    class MetaheuristicsCS
    {
        private static int _totalExperiments;
        private static int _completedExperiments;
        private static Object _counterLock = new object();

        private static void SetCounters(int n)
        {
            _totalExperiments = n;
            _completedExperiments = 0;
        }

        private static void ExperimentFinished()
        {
            lock (_counterLock)
            {
                _completedExperiments += 1;
                Console.WriteLine("Progress: {0}/{1} {2}%", _completedExperiments, _totalExperiments, 100 * _completedExperiments / (double)_totalExperiments);
            }
        }

        private static int? CountLines(string filename)
        {
            if (File.Exists(filename))
            {
                return File.ReadLines(filename).Count() - 1;
            }
            else
            {
                return null;
            }
        }

        private static string MakeRow(Dictionary<string, string> data, List<string> columns, string separator)
        {
            string result = "";
            foreach (string col in columns)
            {
                if (result != "")
                {
                    result += separator;
                }
                result += data[col];
            }
            return result;
        }

        private static void SaveResults(List<Task<Dictionary<string, string>>> tasks, string filename, List<string> columns, bool append = false)
        {
            string separator = ";";
            StreamWriter sw;
            if (!append)
            {
                sw = new StreamWriter(filename);
                string header = "";
                foreach (string col in columns)
                {
                    if (header != "")
                    {
                        header += separator;
                    }
                    header += col;
                }
                sw.WriteLine(header);
            }
            else
            {
                sw = new StreamWriter(filename, true);
            }
            foreach (var task in tasks)
            {
                var info = task.Result;
                string row = MakeRow(info, columns, separator);
                sw.WriteLine(row);
                sw.Flush();
            }
            sw.Close();
        }

        private static List<Task<Dictionary<string, string>>> RunExperiments<Element, EvaluationResult, OptimizationResult>(List<Experiment<Element, EvaluationResult, OptimizationResult>> experiments)
        {
            var results = new List<OptimizationResult<double>>();
            List<Task<Dictionary<string, string>>> tasks = new List<Task<Dictionary<string, string>>>();
            lock (_counterLock)
            {
                Func<Object, Dictionary<string, string>> func = ex => ((Experiment<Element, EvaluationResult, OptimizationResult>)ex).Execute();
                foreach (var e in experiments)
                {
                    tasks.Add(new Task<Dictionary<string, string>>(func, e));
                }
                SetCounters(tasks.Count);
                Console.WriteLine("{0} experiments started...", _totalExperiments);
            }
            foreach (var task in tasks)
            {
                task.Start();
            }
            return tasks;
        }

        private static Experiment<bool, Tuple<double, double>, BiObjective.OptimizationResult<bool>> PrepareExperiment8(int evaluationType, int selectionMethod, double mutationProb, int populationSize, int? seed = null)
        {
            IEvaluation<bool, Tuple<double, double>> evaluation;
            Dictionary<string, string> info = new Dictionary<string, string>();
            DefaultDominationComparer dominationComparer = new DefaultDominationComparer();
            switch (evaluationType)
            {
                case 0:
                    evaluation = new CBinaryZeroMaxOneMaxEvaluation(100);
                    info["problem"] = "ZeroMaxOneMax";
                    break;
                case 1:
                    evaluation = new CBinaryTrapInvTrapEvaluation(5, 100);
                    info["problem"] = "Trap5InvTrap5";
                    break;
                case 2:
                    evaluation = new CBinaryLOTZEvaluation(10);
                    info["problem"] = "LOTZ";
                    break;
                case 3:
                    evaluation = new CBinaryMOMaxCutEvaluation(EBinaryBiObjectiveMaxCutInstance.maxcut_instance_100);
                    info["problem"] = "MaxCut";
                    break;
                case 4:
                    evaluation = new CBinaryMOKnapsackEvaluation(EBinaryBiObjectiveKnapsackInstance.knapsack_100);
                    info["problem"] = "MOKnapsack";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            info["variables"] = evaluation.iSize.ToString();
            RunningTimeStopCondition stopCondition = new RunningTimeStopCondition(30);
            BinaryRandomGenerator generator = new BinaryRandomGenerator(evaluation.pcConstraint, seed);
            OnePointCrossover crossover = new OnePointCrossover(0.5, seed);
            BinaryBitFlipMutation mutation = new BinaryBitFlipMutation(1.0 / evaluation.iSize, evaluation, seed);
            ASelection< Tuple<double, double>> selection;
            switch (selectionMethod)
            {
                case 0:
                    selection = new SampleBiObjectiveSelection(dominationComparer, seed);
                    info["selection_method"] = "Sample";
                    break;
                case 1:
                    selection = new Selections.BiObjective.TournamentSelection(10, (1.0, 1.0), seed);
                    info["selection_method"] = "tournament";
                    break;
                case 2:
                    selection = new Selections.BiObjective.DominatingTournamentSelection(10, dominationComparer, seed);
                    info["selection_method"] = "dominating tournament";
                    break;
                case 3:
                    selection = new Selections.BiObjective.ScalingSelection(10, populationSize/10, seed);
                    info["selection_method"] = "scaling";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            mutationProb = mutationProb / evaluation.iSize;
            info["mutation_prob"] = mutationProb.ToString();
            info["population"] = populationSize.ToString();
            BiObjective.GeneticAlgorithm<bool> ga = new BiObjective.GeneticAlgorithm<bool>(evaluation, stopCondition, generator, selection, crossover, mutation, populationSize);
            Experiment<bool, Tuple<double, double>, BiObjective.OptimizationResult<bool>> experiment = new Experiment<bool, Tuple<double, double>, BiObjective.OptimizationResult<bool>>(ga, info, ExperimentFinished);
            return experiment;
        }

        private static List<Experiment<bool, Tuple<double, double>, BiObjective.OptimizationResult<bool>>> PrepareExperiments8(int? start, int? seed = null)
        {
            List<Experiment<bool, Tuple<double, double>, BiObjective.OptimizationResult<bool>>> experiments = new List<Experiment<bool, Tuple<double, double>, BiObjective.OptimizationResult<bool>>>();
            for (int eval = 0; eval < 5; eval++)
            {
                for (int select = 0; select < 4; select++)
                {
                    for (int rep = 0; rep < 20; rep++)
                    {
                        experiments.Add(PrepareExperiment8(eval, select, 1.0, 200, seed));
                    }
                }
            }
            if (start.HasValue)
            {
                return experiments.GetRange(start.Value, experiments.Count);
            }
            else
            {
                return experiments;
            }
        }

        private static Experiment<bool, Tuple<double, double>, BiObjective.OptimizationResult<bool>> PrepareExperiment9(int method, int evaluationType, int selectionMethod, double mutationProb, int populationSize, int? seed = null)
        {
            IEvaluation<bool, Tuple<double, double>> evaluation;
            Dictionary<string, string> info = new Dictionary<string, string>();
            DefaultDominationComparer dominationComparer = new DefaultDominationComparer();
            switch (evaluationType)
            {
                case 0:
                    evaluation = new CBinaryZeroMaxOneMaxEvaluation(100);
                    info["problem"] = "ZeroMaxOneMax";
                    break;
                case 1:
                    evaluation = new CBinaryTrapInvTrapEvaluation(5, 100);
                    info["problem"] = "Trap5InvTrap5";
                    break;
                case 2:
                    evaluation = new CBinaryLOTZEvaluation(10);
                    info["problem"] = "LOTZ";
                    break;
                case 3:
                    evaluation = new CBinaryMOMaxCutEvaluation(EBinaryBiObjectiveMaxCutInstance.maxcut_instance_100);
                    info["problem"] = "MaxCut";
                    break;
                case 4:
                    evaluation = new CBinaryMOKnapsackEvaluation(EBinaryBiObjectiveKnapsackInstance.knapsack_100);
                    info["problem"] = "MOKnapsack";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            info["variables"] = evaluation.iSize.ToString();
            RunningTimeStopCondition stopCondition = new RunningTimeStopCondition(30);
            BinaryRandomGenerator generator = new BinaryRandomGenerator(evaluation.pcConstraint, seed);
            OnePointCrossover crossover = new OnePointCrossover(0.5, seed);
            BinaryBitFlipMutation mutation = new BinaryBitFlipMutation(1.0 / evaluation.iSize, evaluation, seed);
            ASelection<Tuple<double, double>> selection;
            switch (selectionMethod)
            {
                case 0:
                    selection = new SampleBiObjectiveSelection(dominationComparer, seed);
                    info["selection_method"] = "Sample";
                    break;
                case 1:
                    selection = new NSGA2Selection(10, evaluation.tMaxValue, dominationComparer, true, seed);
                    info["selection_method"] = "NSGA2";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            mutationProb = mutationProb / evaluation.iSize;
            info["mutation_prob"] = mutationProb.ToString();
            info["population"] = populationSize.ToString();
            BiObjective.GeneticAlgorithm<bool> ga;
            switch (method)
            {
                case 0:
                    ga = new BiObjective.GeneticAlgorithm<bool>(evaluation, stopCondition, generator, selection, crossover, mutation, populationSize, seed);
                    info["method"] = "GA";
                    break;
                case 1:
                    ga = new BiObjective.ClusteringGeneticAlgorithm<bool>(evaluation, stopCondition, generator, selection, crossover, mutation, populationSize, populationSize/10, seed);
                    info["method"] = "ClusteringGA";
                    break;
                case 2:
                    ga = new BiObjective.ClusteringGeneticAlgorithm2<bool>(evaluation, stopCondition, generator, selection, crossover, mutation, populationSize, populationSize / 10, seed);
                    info["method"] = "ClusteringGA2";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Experiment<bool, Tuple<double, double>, BiObjective.OptimizationResult<bool>> experiment = new Experiment<bool, Tuple<double, double>, BiObjective.OptimizationResult<bool>>(ga, info, ExperimentFinished);
            return experiment;
        }

        private static List<Experiment<bool, Tuple<double, double>, BiObjective.OptimizationResult<bool>>> PrepareExperiments9(int? start, int? seed = null)
        {
            List<Experiment<bool, Tuple<double, double>, BiObjective.OptimizationResult<bool>>> experiments = new List<Experiment<bool, Tuple<double, double>, BiObjective.OptimizationResult<bool>>>();
            for (int eval = 0; eval < 5; eval++)
            {
                for (int select = 0; select < 2; select++)
                {
                    if(select == 0)
                    {
                        for(int method = 0; method<3; method++)
                        {
                            for (int rep = 0; rep < 20; rep++)
                            {
                                experiments.Add(PrepareExperiment9(method, eval, select, 1.0, 200, seed));
                            }
                        }
                    }
                    else
                    {
                        for (int rep = 0; rep < 20; rep++)
                        {
                            experiments.Add(PrepareExperiment9(0, eval, select, 1.0, 200, seed));
                        }
                    }
                }
            }
            if (start.HasValue)
            {
                return experiments.GetRange(start.Value, experiments.Count);
            }
            else
            {
                return experiments;
            }
        }

        private static void Lab8(int? seed = null)
        {
            string filename = "lab8.csv";
            int? lines = CountLines(filename);

            var experiments = PrepareExperiments8(lines, seed);
            var tasks = RunExperiments(experiments);

            List<string> columns = new List<string>
            {
                "problem",
                "selection_method",
                "HV",
                "IGD",
                "time",
                "iteration",
                "FFE"
            };

            SaveResults(tasks, filename, columns, lines.HasValue);
        }

        private static void Lab9(int? seed = null)
        {
            string filename = "lab9.csv";
            int? lines = CountLines(filename);

            var experiments = PrepareExperiments8(lines, seed);
            var tasks = RunExperiments(experiments);

            List<string> columns = new List<string>
            {
                "problem",
                "method",
                "selection_method",
                "HV",
                "IGD",
                "time",
                "iteration",
                "FFE"
            };

            SaveResults(tasks, filename, columns, lines.HasValue);
        }

        private static void ReportBiObjectiveOptimizationResult<Element>(BiObjective.OptimizationResult<Element> optimizationResult)
        {
            Console.WriteLine("hyper volume: {0}", optimizationResult.Front.HyperVolume());
            Console.WriteLine("IGD: {0}", optimizationResult.Front.InversedGenerationalDistance());
            Console.WriteLine("\tlast update (time): {0}s", optimizationResult.LastUpdateTime);
            Console.WriteLine("\tlast update (iteration): {0}", optimizationResult.LastUpdateIteration);
            Console.WriteLine("\tlast update (FFE): {0}", optimizationResult.LastUpdateFFE);
        }

        private static void Lab9NSGA2(IEvaluation<bool, Tuple<double, double>> evaluation, int? seed)
        {
            RunningTimeStopCondition stopCondition = new RunningTimeStopCondition(5);

            DefaultDominationComparer dominationComparer = new DefaultDominationComparer();

            BinaryRandomGenerator generator = new BinaryRandomGenerator(evaluation.pcConstraint, seed);
            OnePointCrossover crossover = new OnePointCrossover(0.5, seed);
            BinaryBitFlipMutation mutation = new BinaryBitFlipMutation(1.0 / evaluation.iSize, evaluation, seed);
            SampleBiObjectiveSelection selection = new SampleBiObjectiveSelection(dominationComparer, seed);

            BiObjective.NSGA2.NSGA2<bool> nsga2 = new BiObjective.NSGA2.NSGA2<bool>(evaluation, stopCondition, generator, dominationComparer, 
                                                                                    crossover, mutation, 100, seed);

            nsga2.Run();

            ReportBiObjectiveOptimizationResult(nsga2.Result);
        }

        private static void Lab9ZeroMaxOneMax(int? seed)
        {
            Lab9NSGA2(new CBinaryZeroMaxOneMaxEvaluation(10), seed);
        }

        private static void Lab9Trap5InvTrap5(int? seed)
        {
            Lab9NSGA2(new CBinaryTrapInvTrapEvaluation(5, 10), seed);
        }

        private static void Lab9LOTZ(int? seed)
        {
            Lab9NSGA2(new CBinaryLOTZEvaluation(10), seed);
        }

        private static void Lab9MOMaxCut(int? seed)
        {
            Lab9NSGA2(new CBinaryMOMaxCutEvaluation(EBinaryBiObjectiveMaxCutInstance.maxcut_instance_6), seed);
        }

        private static void Lab9MOKnapsack(int? seed)
        {
            Lab9NSGA2(new CBinaryMOKnapsackEvaluation(EBinaryBiObjectiveKnapsackInstance.knapsack_100), seed);
        }

        private static void Lab8BiObjectiveBinaryGA(IEvaluation<bool, Tuple<double, double>> evaluation, int? seed)
        {
            RunningTimeStopCondition stopCondition = new RunningTimeStopCondition(5);

            DefaultDominationComparer dominationComparer = new DefaultDominationComparer();

            BinaryRandomGenerator generator = new BinaryRandomGenerator(evaluation.pcConstraint, seed);
            OnePointCrossover crossover = new OnePointCrossover(0.5, seed);
            BinaryBitFlipMutation mutation = new BinaryBitFlipMutation(1.0 / evaluation.iSize, evaluation, seed);
            SampleBiObjectiveSelection selection = new SampleBiObjectiveSelection(dominationComparer, seed);

            BiObjective.GeneticAlgorithm<bool> ga = new BiObjective.GeneticAlgorithm<bool>(evaluation, stopCondition, generator, selection, crossover, mutation, 100, seed);

            ga.Run();

            ReportBiObjectiveOptimizationResult(ga.Result);
        }

        private static void Lab8ZeroMaxOneMax(int? seed)
        {
            Lab8BiObjectiveBinaryGA(new CBinaryZeroMaxOneMaxEvaluation(10), seed);
        }

        private static void Lab8Trap5InvTrap5(int? seed)
        {
            Lab8BiObjectiveBinaryGA(new CBinaryTrapInvTrapEvaluation(5, 10), seed);
        }

        private static void Lab8LOTZ(int? seed)
        {
            Lab8BiObjectiveBinaryGA(new CBinaryLOTZEvaluation(10), seed);
        }

        private static void Lab8MOMaxCut(int? seed)
        {
            Lab8BiObjectiveBinaryGA(new CBinaryMOMaxCutEvaluation(EBinaryBiObjectiveMaxCutInstance.maxcut_instance_6), seed);
        }

        private static void Lab8MOKnapsack(int? seed)
        {
            Lab8BiObjectiveBinaryGA(new CBinaryMOKnapsackEvaluation(EBinaryBiObjectiveKnapsackInstance.knapsack_100), seed);
        }

        static void Main(string[] args)
        {
            int? seed = null;

            Lab9ZeroMaxOneMax(seed);
            Lab9Trap5InvTrap5(seed);
            Lab9LOTZ(seed);
            Lab9MOMaxCut(seed);
            Lab9MOKnapsack(seed);
            
            Lab8ZeroMaxOneMax(seed);
            Lab8Trap5InvTrap5(seed);
            Lab8LOTZ(seed);
            Lab8MOMaxCut(seed);
            Lab8MOKnapsack(seed);
        }
    }
}
