using Crossovers;
using EvaluationsCLI;
using Generators;
using Mutations;
using Selections;
using StopConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizers.PopulationOptimizers
{
    class GAwithDSM : GeneticAlgorithm<bool>
    {
        private int interval;
        public List<List<List<double>>> dsms { get; }

        public GAwithDSM(IEvaluation<bool> evaluation, AStopCondition stopCondition, AGenerator<bool> generator,
                                ASelection selection, ACrossover crossover, IMutation<bool> mutation, int populationSize, int interval = 10)
            : base(evaluation, stopCondition, generator, selection, crossover, mutation, populationSize)
        {
            this.interval = interval;
            dsms = new List<List<List<double>>>();
            if (crossover is DSMCrossover)
            {
                List<List<double>> tempDSM = new List<List<double>>();
                for(int i=0; i<evaluation.iSize; i++)
                {
                    tempDSM.Add(Enumerable.Repeat(0.0, evaluation.iSize).ToList());
                }
                ((DSMCrossover)crossover).dsm = tempDSM;
            }
        }

        protected override bool RunIteration(long itertionNumber, DateTime startTime)
        {
            bool foundNewBest = base.RunIteration(itertionNumber, startTime);
            if(itertionNumber % interval == 0)
            {
                var newDSM = CalculateDSM();
                dsms.Add(newDSM);
                if(crossover is DSMCrossover)
                {
                    ((DSMCrossover)crossover).dsm = newDSM;
                }
            }
            return foundNewBest;
        }

        private List<List<double>> CalculateDSM()
        {
            List<int> geneCounts = Enumerable.Repeat(0, evaluation.iSize).ToList();
            List<List<int[]>> pairs = new List<List<int[]>>();
            List<List<double>> dsm = new List<List<double>>();
            for(int i=0; i<evaluation.iSize; i++)
            {
                pairs.Add(new List<int[]>());
                dsm.Add(Enumerable.Repeat(0.0, evaluation.iSize).ToList());
                for(int j=0; j<evaluation.iSize; j++)
                {
                    pairs[i].Add(Enumerable.Repeat(0, 4).ToArray());
                }
            }

            foreach(var individual in population)
            {
                var genotype = individual.Genotype;
                for(int i=0; i<evaluation.iSize; i++)
                {
                    geneCounts[i] += Convert.ToInt32(genotype[i]);
                    for(int j=i; j<evaluation.iSize; j++)
                    {
                        int p = 2 * Convert.ToInt32(genotype[i]) + Convert.ToInt32(genotype[j]);
                        pairs[i][j][p] += 1;
                        pairs[j][i][p] += 1;
                    }
                }
            }

            for(int i=0; i<evaluation.iSize; i++)
            {
                for(int j=i; j<evaluation.iSize; j++)
                {
                    double val = 0.0;
                    for(int k=0; k<4; k++)
                    {
                        var valk = (pairs[i][j][k] / (double)populationSize) * (Math.Log(pairs[i][j][k]) + Math.Log(populationSize) - Math.Log((k & 2) != 0 ? geneCounts[i] : populationSize - geneCounts[i]) - Math.Log((k & 1) != 0 ? geneCounts[j] : populationSize - geneCounts[j]));
                        if(!Double.IsNaN(valk))
                            val += valk;
                    }
                    dsm[i][j] = val;
                    dsm[j][i] = val;
                }
            }

            return dsm;
        }
    }
}
