using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GAvsTSP
{
    class GreedyAlgorithm
    {
        private int findMin(List<double> list)
        {
            int min = 0;
            for (int i=1; i<list.Count; i++)
            {
                if (list[i] < list[min])
                {
                    min = i;
                }
            }
            return min;
        }

        public static List<int> GenerateGreedySolution(TSP evaluation, int start)
        {
            List<int> solution = new List<int> { start };
            int last = start;
            List<int> remaining = Enumerable.Range(0, evaluation.size).ToList();
            remaining.Remove(start);
            while (remaining.Count > 0)
            {
                int min = remaining[0];
                for (int i=1; i<remaining.Count; i++)
                {
                    if (evaluation.distances[last][remaining[i]] < evaluation.distances[last][min])
                    {
                        min = remaining[i];
                    }
                }
                last = min;
                solution.Add(last);
                remaining.Remove(last);
            }
            return solution;
        }
    }
}
