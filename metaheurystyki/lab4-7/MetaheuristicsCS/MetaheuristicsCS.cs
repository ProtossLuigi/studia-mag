using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Crossovers;
using Evaluations;
using EvaluationsCLI;
using Generators;
using Mutations;
using Optimizers;
using Optimizers.PopulationOptimizers;
using Selections;
using StopConditions;
using Utility;


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

        private static void ReportOptimizationResult<Element>(OptimizationResult<Element> optimizationResult)
        {
            Console.WriteLine("value: {0}", optimizationResult.BestValue);
            Console.WriteLine("\twhen (time): {0}s", optimizationResult.BestTime);
            Console.WriteLine("\twhen (iteration): {0}", optimizationResult.BestIteration);
            Console.WriteLine("\twhen (FFE): {0}", optimizationResult.BestFFE);
        }

        private static void Lab5(int? seed)
        {
            CBinaryKnapsackEvaluation evaluation = new CBinaryKnapsackEvaluation(EBinaryKnapsackInstance.knapPI_1_100_1000_1);
            IterationsStopCondition stopCondition = new IterationsStopCondition(evaluation.dMaxValue, 100);

            BinaryRandomGenerator generator = new BinaryRandomGenerator(evaluation.pcConstraint, seed);
            OnePointCrossover crossover = new OnePointCrossover(0.5, seed);
            BinaryBitFlipMutation mutation = new BinaryBitFlipMutation(1.0 / evaluation.iSize, evaluation, seed);
            TournamentSelection selection = new TournamentSelection(2, seed);

            GeneticAlgorithm<bool> ga = new GeneticAlgorithm<bool>(evaluation, stopCondition, generator, selection, crossover, mutation, 50);

            ga.Run();

            ReportOptimizationResult(ga.Result);
        }

        private static void Lab5Test(int? seed)
        {
            var instance = EBinaryKnapsackInstance.knapPI_1_100_1000_1;

            CBinaryKnapsackEvaluation evaluation = new CheckingBinaryKnapsackEvaluation(instance);
            IterationsStopCondition stopCondition = new IterationsStopCondition(evaluation.dMaxValue, 100);

            BinaryRandomGenerator generator = new BinaryRandomGenerator(evaluation.pcConstraint, seed);
            OnePointCrossover crossover = new OnePointCrossover(0.5, seed);
            BinaryBitFlipMutation mutation = new BinaryBitFlipMutation(1.0 / evaluation.iSize, evaluation, seed);
            TournamentSelection selection = new TournamentSelection(2, seed);
            GeneticAlgorithm<bool> ga = new GeneticAlgorithm<bool>(evaluation, stopCondition, generator, selection, crossover, mutation, 50);
            ga.Run();
            ReportOptimizationResult(ga.Result);

            evaluation = new CheckingBinaryKnapsackEvaluation(instance);
            stopCondition = new IterationsStopCondition(evaluation.dMaxValue, 100);
            generator = new BinaryRandomGenerator(evaluation.pcConstraint, seed);
            crossover = new OnePointCrossover(0.5, seed);
            mutation = new BinaryBitFlipMutation(1.0 / evaluation.iSize, evaluation, seed);
            selection = new TournamentSelection(2, seed);
            ga = new LamarckGeneticAlgorithm(evaluation, stopCondition, generator, selection, crossover, mutation, 50);
            ga.Run();
            ReportOptimizationResult(ga.Result);

            evaluation = new CheckingBinaryKnapsackEvaluation(instance);
            stopCondition = new IterationsStopCondition(evaluation.dMaxValue, 100);
            generator = new BinaryRandomGenerator(evaluation.pcConstraint, seed);
            crossover = new OnePointCrossover(0.5, seed);
            mutation = new BinaryBitFlipMutation(1.0 / evaluation.iSize, evaluation, seed);
            selection = new TournamentSelection(2, seed);
            ga = new BaldwinGeneticAlgorithm(evaluation, stopCondition, generator, selection, crossover, mutation, 50);
            ga.Run();
            ReportOptimizationResult(ga.Result);

            evaluation = new PenalizingBinaryKnapsackEvaluation(instance);
            stopCondition = new IterationsStopCondition(evaluation.dMaxValue, 100);
            generator = new BinaryRandomGenerator(evaluation.pcConstraint, seed);
            crossover = new OnePointCrossover(0.5, seed);
            mutation = new BinaryBitFlipMutation(1.0 / evaluation.iSize, evaluation, seed);
            selection = new TournamentSelection(2, seed);
            ga = new GeneticAlgorithm<bool>(evaluation, stopCondition, generator, selection, crossover, mutation, 50);
            ga.Run();
            ReportOptimizationResult(ga.Result);

            evaluation = new PenalizingBinaryKnapsackEvaluation(instance);
            stopCondition = new IterationsStopCondition(evaluation.dMaxValue, 100);
            generator = new BinaryRandomGenerator(evaluation.pcConstraint, seed);
            crossover = new OnePointCrossover(0.5, seed);
            mutation = new BinaryBitFlipMutation(1.0 / evaluation.iSize, evaluation, seed);
            selection = new TournamentSelection(2, seed);
            ga = new LamarckGeneticAlgorithm(evaluation, stopCondition, generator, selection, crossover, mutation, 50);
            ga.Run();
            ReportOptimizationResult(ga.Result);

            evaluation = new PenalizingBinaryKnapsackEvaluation(instance);
            stopCondition = new IterationsStopCondition(evaluation.dMaxValue, 100);
            generator = new BinaryRandomGenerator(evaluation.pcConstraint, seed);
            crossover = new OnePointCrossover(0.5, seed);
            mutation = new BinaryBitFlipMutation(1.0 / evaluation.iSize, evaluation, seed);
            selection = new TournamentSelection(2, seed);
            ga = new BaldwinGeneticAlgorithm(evaluation, stopCondition, generator, selection, crossover, mutation, 50);
            ga.Run();
            ReportOptimizationResult(ga.Result);
        }

        private static void Lab4BinaryGA(IEvaluation<bool> evaluation, ASelection selection, ACrossover crossover, int? seed)
        {
            IterationsStopCondition stopCondition = new IterationsStopCondition(evaluation.dMaxValue, 100);

            BinaryRandomGenerator generator = new BinaryRandomGenerator(evaluation.pcConstraint, seed);
            BinaryBitFlipMutation mutation = new BinaryBitFlipMutation(1.0 / evaluation.iSize, evaluation, seed);

            GeneticAlgorithm<bool> ga = new GeneticAlgorithm<bool>(evaluation, stopCondition, generator, selection, crossover, mutation, 50);

            ga.Run();

            ReportOptimizationResult(ga.Result);
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

        private static List<Experiment<bool>> PrepareExperiments41(int? start, int? seed = null)
        {
            List<Experiment<bool>> experiments = new List<Experiment<bool>>();
            for(int eval=0; eval<6; eval++)
            {
                for(int select=0; select<2; select++)
                {
                    for(int cross=0; cross<2; cross++)
                    {
                        for(int pop=20; pop<120; pop += 10)
                        {
                            for(int rep=0; rep<20; rep++)
                            {
                                experiments.Add(PrepareExperiment4(eval, select, cross, 0.5, 1.0, pop, seed));
                            }
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

        private static List<Experiment<bool>> PrepareExperiments42(int? start, int? seed = null)
        {
            List<Experiment<bool>> experiments = new List<Experiment<bool>>();
            for (int eval = 0; eval < 6; eval++)
            {
                for (int select = 0; select < 2; select++)
                {
                    for (int cross = 0; cross < 2; cross++)
                    {
                        for (double pcross = 0.1; pcross <= 1.0; pcross += 0.1)
                        {
                            for (double pmut = 0.2; pmut<=2.0; pmut += 0.2)
                            {
                                for (int rep = 0; rep < 20; rep++)
                                {
                                    experiments.Add(PrepareExperiment4(eval, select, cross, pcross, pmut, 50, seed));
                                }
                            }
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

        private static List<Experiment<bool>> PrepareExperiments51(int? start, int? seed = null)
        {
            List<Experiment<bool>> experiments = new List<Experiment<bool>>();
            for(int eval=1; eval<4; eval++)
            {
                for(int instance=0; instance<31; instance++)
                {
                    if(instance == 9)
                    {
                        instance++;
                    }
                    for(int i=0; i<20; i++)
                    {
                        experiments.Add(PrepareExperiment5(eval, instance, 1, 1, 0, 0.5, 1.0, 50, seed));
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

        private static List<Experiment<bool>> PrepareExperiments52(int? start, int? seed = null)
        {
            List<Experiment<bool>> experiments = new List<Experiment<bool>>();
            for (int algorithm = 1; algorithm < 4; algorithm++)
            {
                for (int instance = 0; instance < 31; instance++)
                {
                    if (instance == 9)
                    {
                        instance++;
                    }
                    for (int i = 0; i < 20; i++)
                    {
                        experiments.Add(PrepareExperiment5(2, instance, algorithm, 1, 0, 0.5, 1.0, 50, seed));
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

        private static List<Experiment<bool>> PrepareExperiments6(int? start, int? seed = null)
        {
            List<Experiment<bool>> experiments = new List<Experiment<bool>>();
            for (int eval = 0; eval < 6; eval++)
            {
                for (int algorithm = 0; algorithm < 2; algorithm++)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        experiments.Add(PrepareExperiment6(eval, algorithm, 1, 0, 0.1, 0.0, 50, seed));
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

        private static List<Experiment<bool>> PrepareExperiments71(int? start, int? seed = null)
        {
            List<Experiment<bool>> experiments = new List<Experiment<bool>>();
            for (int eval = 0; eval < 4; eval++)
            {
                for (int crossover = 0; crossover < 2; crossover++)
                {
                    for(int variables=10; variables<110; variables += 10)
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            experiments.Add(PrepareExperiment7(eval, 0, variables, 1, crossover, 0.1, 1.0, 50, seed));
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

        private static List<Experiment<bool>> PrepareExperiments74(int? start, int? seed = null)
        {
            List<Experiment<bool>> experiments = new List<Experiment<bool>>();
            for (int eval = 0; eval < 4; eval++)
            {
                for (int crossover = 0; crossover < 3; crossover++)
                {
                    for (int variables = 10; variables < 110; variables += 10)
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            experiments.Add(PrepareExperiment7(eval, 1, variables, 1, crossover, 0.5, 1.0, 50, seed));
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

        private static Experiment<bool> PrepareExperiment4(int evaluationType, int selectionMethod, int crossoverMethod, double crossoverProb, double mutationProb, int populationSize, int? seed = null)
        {
            IEvaluation<bool> evaluation;
            Dictionary<string, string> info = new Dictionary<string, string>();
            switch (evaluationType)
            {
                case 0:
                    evaluation = new CBinaryMax3SatEvaluation(100);
                    info["problem"] = "Max3Sat";
                    info["variables"] = "100";
                    break;
                case 1:
                    evaluation = new CBinaryIsingSpinGlassEvaluation(100);
                    info["problem"] = "ISG";
                    info["variables"] = "100";
                    break;
                case 2:
                    evaluation = new CBinaryNKLandscapesEvaluation(100);
                    info["problem"] = "NKLandscapes";
                    info["variables"] = "100";
                    break;
                case 3:
                    evaluation = new CBinaryStandardDeceptiveConcatenationEvaluation(3, 10);
                    info["problem"] = "Deceptive";
                    info["variables"] = "10";
                    break;
                case 4:
                    evaluation = new CBinaryStandardDeceptiveConcatenationEvaluation(3, 50);
                    info["problem"] = "Deceptive";
                    info["variables"] = "50";
                    break;
                case 5:
                    evaluation = new CBinaryStandardDeceptiveConcatenationEvaluation(3, 100);
                    info["problem"] = "Deceptive";
                    info["variables"] = "100";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            IterationsStopCondition stopCondition = new IterationsStopCondition(evaluation.dMaxValue, 1000);
            BinaryRandomGenerator generator = new BinaryRandomGenerator(evaluation.pcConstraint, seed);
            ASelection selection;
            switch (selectionMethod)
            {
                case 0:
                    selection = new RouletteWheelSelection(seed);
                    info["selection_method"] = "roulette";
                    break;
                case 1:
                    selection = new TournamentSelection(2, seed);
                    info["selection_method"] = "tournament";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            ACrossover crossover;
            info["crossover_prob"] = crossoverProb.ToString();
            switch (crossoverMethod)
            {
                case 0:
                    crossover = new OnePointCrossover(crossoverProb, seed);
                    info["crossover_method"] = "OnePoint";
                    break;
                case 1:
                    crossover = new UniformCrossover(crossoverProb, seed);
                    info["crossover_method"] = "uniform";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            mutationProb = mutationProb / evaluation.iSize;
            info["mutation_prob"] = mutationProb.ToString();
            BinaryBitFlipMutation mutation = new BinaryBitFlipMutation(mutationProb, evaluation, seed);
            info["population"] = populationSize.ToString();
            GeneticAlgorithm<bool> ga = new GeneticAlgorithm<bool>(evaluation, stopCondition, generator, selection, crossover, mutation, populationSize);
            Experiment<bool> experiment = new Experiment<bool>(ga, info, ExperimentFinished);
            return experiment;
        }

        private static Experiment<bool> PrepareExperiment5(int evaluationType, int instance, int algorithm, int selectionMethod, int crossoverMethod, double crossoverProb, double mutationProb, int populationSize, int? seed = null)
        {
            CBinaryKnapsackEvaluation evaluation;
            Dictionary<string, string> info = new Dictionary<string, string>();
            List<string> instances = new List<string>
            {
                "f1_l_d_kp_10_269",
                "f2_l_d_kp_20_878",
                "f3_l_d_kp_4_20",
                "f4_l_d_kp_4_11",
                "f5_l_d_kp_15_375",
                "f6_l_d_kp_10_60",
                "f7_l_d_kp_7_50",
                "f8_l_d_kp_23_10000",
                "f9_l_d_kp_5_80",
                "f10_l_d_kp_20_879",
                "knapPI_1_100_1000_1",
                "knapPI_1_200_1000_1",
                "knapPI_1_500_1000_1",
                "knapPI_1_1000_1000_1",
                "knapPI_1_2000_1000_1",
                "knapPI_1_5000_1000_1",
                "knapPI_1_10000_1000_1",
                "knapPI_2_100_1000_1",
                "knapPI_2_200_1000_1",
                "knapPI_2_500_1000_1",
                "knapPI_2_1000_1000_1",
                "knapPI_2_2000_1000_1",
                "knapPI_2_5000_1000_1",
                "knapPI_2_10000_1000_1",
                "knapPI_3_100_1000_1",
                "knapPI_3_200_1000_1",
                "knapPI_3_500_1000_1",
                "knapPI_3_1000_1000_1",
                "knapPI_3_2000_1000_1",
                "knapPI_3_5000_1000_1",
                "knapPI_3_10000_1000_1"
            };
            info["instance"] = instances[instance];
            switch (evaluationType)
            {
                case 0:
                    evaluation = new CBinaryKnapsackEvaluation((EBinaryKnapsackInstance)instance);
                    info["evaluation"] = "base";
                    break;
                case 1:
                    evaluation = new CheckingBinaryKnapsackEvaluation((EBinaryKnapsackInstance)instance);
                    info["evaluation"] = "checking";
                    break;
                case 2:
                    evaluation = new NegativeBinaryKnapsackEvaluation((EBinaryKnapsackInstance)instance);
                    info["evaluation"] = "negative";
                    break;
                case 3:
                    evaluation = new PenalizingBinaryKnapsackEvaluation((EBinaryKnapsackInstance)instance);
                    info["evaluation"] = "penalizing";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            RunningTimeStopCondition stopCondition = new RunningTimeStopCondition(evaluation.dMaxValue, 30);
            BinaryRandomGenerator generator = new BinaryRandomGenerator(evaluation.pcConstraint, seed);
            ASelection selection;
            switch (selectionMethod)
            {
                case 0:
                    selection = new RouletteWheelSelection(seed);
                    info["selection_method"] = "roulette";
                    break;
                case 1:
                    selection = new TournamentSelection(2, seed);
                    info["selection_method"] = "tournament";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            ACrossover crossover;
            info["crossover_prob"] = crossoverProb.ToString();
            switch (crossoverMethod)
            {
                case 0:
                    crossover = new OnePointCrossover(crossoverProb, seed);
                    info["crossover_method"] = "OnePoint";
                    break;
                case 1:
                    crossover = new UniformCrossover(crossoverProb, seed);
                    info["crossover_method"] = "uniform";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            mutationProb = mutationProb / evaluation.iSize;
            info["mutation_prob"] = mutationProb.ToString();
            BinaryBitFlipMutation mutation = new BinaryBitFlipMutation(mutationProb, evaluation, seed);
            info["population"] = populationSize.ToString();
            GeneticAlgorithm<bool> ga;
            switch (algorithm)
            {
                case 0:
                    ga = new GeneticAlgorithm<bool>(evaluation, stopCondition, generator, selection, crossover, mutation, populationSize);
                    info["algorithm"] = "GA";
                    break;
                case 1:
                    ga = new KnapsackGeneticAlgorithm(evaluation, stopCondition, generator, selection, crossover, mutation, populationSize);
                    info["algorithm"] = "Checking";
                    break;
                case 2:
                    ga = new LamarckGeneticAlgorithm(evaluation, stopCondition, generator, selection, crossover, mutation, populationSize);
                    info["algorithm"] = "Lamarck";
                    break;
                case 3:
                    ga = new BaldwinGeneticAlgorithm(evaluation, stopCondition, generator, selection, crossover, mutation, populationSize);
                    info["algorithm"] = "Baldwin";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Experiment<bool> experiment = new Experiment<bool>(ga, info, ExperimentFinished);
            return experiment;
        }

        private static Experiment<bool> PrepareExperiment6(int evaluationType, int algorithm, int selectionMethod, int crossoverMethod, double crossoverProb, double mutationProb, int populationSize, int? seed = null)
        {
            IEvaluation<bool> evaluation;
            Dictionary<string, string> info = new Dictionary<string, string>();
            switch (evaluationType)
            {
                case 0:
                    evaluation = new CBinaryMax3SatEvaluation(100);
                    info["problem"] = "Max3Sat";
                    info["variables"] = "100";
                    break;
                case 1:
                    evaluation = new CBinaryIsingSpinGlassEvaluation(100);
                    info["problem"] = "ISG";
                    info["variables"] = "100";
                    break;
                case 2:
                    evaluation = new CBinaryNKLandscapesEvaluation(100);
                    info["problem"] = "NKLandscapes";
                    info["variables"] = "100";
                    break;
                case 3:
                    evaluation = new CBinaryStandardDeceptiveConcatenationEvaluation(3, 10);
                    info["problem"] = "Deceptive";
                    info["variables"] = "10";
                    break;
                case 4:
                    evaluation = new CBinaryStandardDeceptiveConcatenationEvaluation(3, 50);
                    info["problem"] = "Deceptive";
                    info["variables"] = "50";
                    break;
                case 5:
                    evaluation = new CBinaryStandardDeceptiveConcatenationEvaluation(3, 100);
                    info["problem"] = "Deceptive";
                    info["variables"] = "100";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            RunningTimeStopCondition stopCondition = new RunningTimeStopCondition(evaluation.dMaxValue, 30);
            BinaryRandomGenerator generator = new BinaryRandomGenerator(evaluation.pcConstraint, seed);
            ASelection selection;
            switch (selectionMethod)
            {
                case 0:
                    selection = new RouletteWheelSelection(seed);
                    info["selection_method"] = "roulette";
                    break;
                case 1:
                    selection = new TournamentSelection(2, seed);
                    info["selection_method"] = "tournament";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            ACrossover crossover;
            info["crossover_prob"] = crossoverProb.ToString();
            switch (crossoverMethod)
            {
                case 0:
                    crossover = new OnePointCrossover(crossoverProb, seed);
                    info["crossover_method"] = "OnePoint";
                    break;
                case 1:
                    crossover = new UniformCrossover(crossoverProb, seed);
                    info["crossover_method"] = "uniform";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            mutationProb = mutationProb / evaluation.iSize;
            info["mutation_prob"] = mutationProb.ToString();
            BinaryBitFlipMutation mutation = new BinaryBitFlipMutation(mutationProb, evaluation, seed);
            info["population"] = populationSize.ToString();
            GeneticAlgorithm<bool> ga;
            switch (algorithm)
            {
                case 0:
                    ga = new GeneticAlgorithm<bool>(evaluation, stopCondition, generator, selection, crossover, mutation, populationSize);
                    info["algorithm"] = "GA";
                    break;
                case 1:
                    ga = new ResettingGA<bool>(evaluation, stopCondition, generator, selection, crossover, mutation, populationSize, 10, seed);
                    info["algorithm"] = "resetting";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Experiment<bool> experiment = new Experiment<bool>(ga, info, ExperimentFinished);
            return experiment;
        }

        private static Experiment<bool> PrepareExperiment7(int evaluationType, int algorithm, int variables, int selectionMethod, int crossoverMethod, double crossoverProb, double mutationProb, int populationSize, int? seed = null)
        {
            IEvaluation<bool> evaluation;
            Dictionary<string, string> info = new Dictionary<string, string>();
            switch (evaluationType)
            {
                case 0:
                    evaluation = new CBinaryStandardDeceptiveConcatenationEvaluation(3, variables);
                    info["problem"] = "order3standard";
                    break;
                case 1:
                    evaluation = new CBinaryBimodalDeceptiveConcatenationEvaluation(10, variables);
                    info["problem"] = "order10bimodal";
                    break;
                case 2:
                    evaluation = new ShuffledStandardDeceptiveConcatenationEvaluation(3, variables);
                    info["problem"] = "order3shuffled";
                    break;
                case 3:
                    evaluation = new ShuffledBimodalDeceptiveConcatenationEvaluation(10, variables);
                    info["problem"] = "order10shuffled";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            info["variables"] = variables.ToString();
            RunningTimeStopCondition stopCondition = new RunningTimeStopCondition(evaluation.dMaxValue, 30);
            BinaryRandomGenerator generator = new BinaryRandomGenerator(evaluation.pcConstraint, seed);
            ASelection selection;
            switch (selectionMethod)
            {
                case 0:
                    selection = new RouletteWheelSelection(seed);
                    info["selection_method"] = "roulette";
                    break;
                case 1:
                    selection = new TournamentSelection(2, seed);
                    info["selection_method"] = "tournament";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            ACrossover crossover;
            info["crossover_prob"] = crossoverProb.ToString();
            switch (crossoverMethod)
            {
                case 0:
                    crossover = new OnePointCrossover(crossoverProb, seed);
                    info["crossover_method"] = "OnePoint";
                    break;
                case 1:
                    crossover = new UniformCrossover(crossoverProb, seed);
                    info["crossover_method"] = "uniform";
                    break;
                case 2:
                    crossover = new DSMCrossover(crossoverProb, seed);
                    info["crossover_method"] = "withDSM";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            mutationProb = mutationProb / evaluation.iSize;
            info["mutation_prob"] = mutationProb.ToString();
            BinaryBitFlipMutation mutation = new BinaryBitFlipMutation(mutationProb, evaluation, seed);
            info["population"] = populationSize.ToString();
            GeneticAlgorithm<bool> ga;
            switch (algorithm)
            {
                case 0:
                    ga = new GeneticAlgorithm<bool>(evaluation, stopCondition, generator, selection, crossover, mutation, populationSize);
                    info["algorithm"] = "GA";
                    break;
                case 1:
                    ga = new GAwithDSM(evaluation, stopCondition, generator, selection, crossover, mutation, populationSize, 10);
                    info["algorithm"] = "GAwithDSM";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Experiment<bool> experiment = new Experiment<bool>(ga, info, ExperimentFinished);
            return experiment;
        }

        private static string MakeRow(Dictionary<string, string> data, List<string> columns, string separator)
        {
            string result = "";
            foreach (string col in columns)
            {
                if(result != "")
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

        private static void SaveDSMs(List<List<List<double>>> dsms, string filename)
        {
            StreamWriter sw = new StreamWriter(filename);
            foreach(var dsm in dsms)
            {
                List<string> rows = new List<string>();
                foreach(var row in dsm)
                {
                    rows.Add("[" + string.Join(", ", row.ToArray()) + "]");
                }
                sw.WriteLine("[" + string.Join(",\n", rows.ToArray()) + "]");
            }
            sw.Close();
        }

        private static List<Task<Dictionary<string, string>>> RunExperiments<Element>(List<Experiment<Element>> experiments)
        {
            var results = new List<OptimizationResult<double>>();
            List<Task<Dictionary<string, string>>> tasks = new List<Task<Dictionary<string, string>>>();
            lock (_counterLock)
            {
                Func<Object, Dictionary<string, string>> func = ex => ((Experiment<Element>)ex).Execute();
                foreach(var e in experiments)
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

        private static void Lab41(int? seed = null)
        {
            string filename = "lab41.csv";
            int? lines = CountLines(filename);

            var experiments = PrepareExperiments41(lines, seed);
            var tasks = RunExperiments<bool>(experiments);

            List<string> columns = new List<string>
            {
                "problem",
                "variables",
                "population",
                "selection_method",
                "crossover_method",
                "crossover_prob",
                "mutation_prob",
                "fitness",
                "time",
                "iteration",
                "FFE"
            };

            SaveResults(tasks, "lab41.csv", columns, lines.HasValue);
        }

        private static void Lab42(int? seed = null)
        {
            string filename = "lab42.csv";
            int? lines = CountLines(filename);

            var experiments = PrepareExperiments42(lines, seed);
            var tasks = RunExperiments<bool>(experiments);

            List<string> columns = new List<string>
            {
                "problem",
                "variables",
                "population",
                "selection_method",
                "crossover_method",
                "crossover_prob",
                "mutation_prob",
                "fitness",
                "time",
                "iteration",
                "FFE"
            };

            SaveResults(tasks, filename, columns, lines.HasValue);
        }

        private static void Lab51(int? seed = null)
        {
            string filename = "lab51.csv";
            int? lines = CountLines(filename);

            var experiments = PrepareExperiments51(lines, seed);
            var tasks = RunExperiments<bool>(experiments);

            List<string> columns = new List<string>
            {
                "evaluation",
                "instance",
                "fitness",
                "time",
                "iteration",
                "FFE"
            };

            SaveResults(tasks, filename, columns, lines.HasValue);
        }

        private static void Lab52(int? seed = null)
        {
            string filename = "lab52.csv";
            int? lines = CountLines(filename);

            var experiments = PrepareExperiments52(lines, seed);
            var tasks = RunExperiments<bool>(experiments);

            List<string> columns = new List<string>
            {
                "algorithm",
                "instance",
                "fitness",
                "time",
                "iteration",
                "FFE"
            };

            SaveResults(tasks, filename, columns, lines.HasValue);
        }

        private static void Lab6(int? seed = null)
        {
            string filename = "lab6.csv";
            int? lines = CountLines(filename);

            var experiments = PrepareExperiments6(lines, seed);
            var tasks = RunExperiments<bool>(experiments);

            List<string> columns = new List<string>
            {
                "problem",
                "variables",
                "algorithm",
                "fitness",
                "time",
                "iteration",
                "FFE"
            };

            SaveResults(tasks, filename, columns, lines.HasValue);
        }

        private static void Lab71(int? seed = null)
        {
            string filename = "lab71.csv";
            int? lines = CountLines(filename);

            var experiments = PrepareExperiments71(lines, seed);
            var tasks = RunExperiments<bool>(experiments);

            List<string> columns = new List<string>
            {
                "problem",
                "crossover_method",
                "variables",
                "fitness",
                "time",
                "iteration",
                "FFE"
            };

            SaveResults(tasks, filename, columns, lines.HasValue);
        }

        private static void Lab73(int? seed = null)
        {
            string filename = "lab73_order3.txt";
            IEvaluation<bool> evaluation = new CBinaryStandardDeceptiveConcatenationEvaluation(3, 100);
            RunningTimeStopCondition stopCondition = new RunningTimeStopCondition(evaluation.dMaxValue, 30);
            BinaryRandomGenerator generator = new BinaryRandomGenerator(evaluation.pcConstraint, seed);
            OnePointCrossover crossover = new OnePointCrossover(0.5, seed);
            BinaryBitFlipMutation mutation = new BinaryBitFlipMutation(1.0 / evaluation.iSize, evaluation, seed);
            TournamentSelection selection = new TournamentSelection(2, seed);
            GAwithDSM ga = new GAwithDSM(evaluation, stopCondition, generator, selection, crossover, mutation, 50);
            ga.Run();

            SaveDSMs(ga.dsms, filename);

            filename = "lab73_order10.txt";
            evaluation = new CBinaryBimodalDeceptiveConcatenationEvaluation(10, 100);
            stopCondition = new RunningTimeStopCondition(evaluation.dMaxValue, 30);
            generator = new BinaryRandomGenerator(evaluation.pcConstraint, seed);
            crossover = new OnePointCrossover(0.5, seed);
            mutation = new BinaryBitFlipMutation(1.0 / evaluation.iSize, evaluation, seed);
            selection = new TournamentSelection(2, seed);
            ga = new GAwithDSM(evaluation, stopCondition, generator, selection, crossover, mutation, 50);
            ga.Run();

            SaveDSMs(ga.dsms, filename);
        }

        private static void Lab74(int? seed = null)
        {
            string filename = "lab74.csv";
            int? lines = CountLines(filename);

            var experiments = PrepareExperiments74(lines, seed);
            var tasks = RunExperiments<bool>(experiments);

            List<string> columns = new List<string>
            {
                "problem",
                "crossover_method",
                "variables",
                "fitness",
                "time",
                "iteration",
                "FFE"
            };

            SaveResults(tasks, filename, columns, lines.HasValue);
        }

        static void Main(string[] args)
        {
            int? seed = null;

            // Lab41(seed);
            // Lab42(seed);
            //Lab5Test(seed);
            //Lab51(seed);
            //Lab52(seed);
            Lab6(seed);
            //Lab71(seed);
            //Lab73(seed);
            //Lab74(seed);
        }
    }
}
