﻿using EvaluationsCLI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility
{
    class GreedyAlgorithm
    {
        private readonly IEvaluation<bool> evaluation;
        private readonly Random rng;

        public GreedyAlgorithm(IEvaluation<bool> evaluation, int? seed = null)
        {
            this.evaluation = evaluation;
            rng = seed.HasValue ? new Random(seed.Value) : new Random();
        }

        public List<bool> Greedy(List<bool> solution)
        {
            List<int> order = GenerateRandomOrder();
            foreach(int i in order)
            {
                List<bool> newSolution = new List<bool>(solution);
                newSolution[i] = !newSolution[i];
                if(evaluation.dEvaluate(newSolution) > evaluation.dEvaluate(solution))
                {
                    solution = newSolution;
                }
            }
            return solution;
        }

        private List<int> GenerateRandomOrder()
        {
            return Enumerable.Range(0, evaluation.iSize).ToList().OrderBy(x => rng.Next()).ToList();
        }
    }
}
