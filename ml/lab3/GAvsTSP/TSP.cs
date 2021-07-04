using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GAvsTSP
{
    class TSP
    {
        public int size { get; }
        public List<List<double>> distances { get; }

        public TSP(int size, List<List<double>> distances)
        {
            this.size = size;
            this.distances = distances;
        }

        public static TSP FromEuclid2D(int size, List<(int, int, int)> coords)
        {
            List<(double, double)> nodes = new List<(double, double)>(size);
            foreach (var coord in coords)
            {
                nodes.Add((coord.Item2, coord.Item3));
            }
            List<List<double>> distances = new List<List<double>>();
            for(int i=0; i< size; i++)
            {
                distances.Add(new List<double>());
                for (int j=0; j< size; j++)
                {
                    if(i > j)
                    {
                        distances[i].Add(distances[j][i]);
                    }
                    else if (i < j)
                    {
                        distances[i].Add(DistanceEuclid2D(nodes[i], nodes[j]));
                    }
                    else
                    {
                        distances[i].Add(0.0);
                    }
                }
            }
            return new TSP(size, distances);
        }

        public double Evaluate(List<int> solution)
        {
            if (solution.Count != size)
            {
                throw new ArgumentException();
            }
            double sum = 0;
            for (int i=0; i<size; i++)
            {
                if (i == size - 1)
                {
                    sum += distances[solution[i]][solution[0]];
                }
                else
                {
                    sum += distances[solution[i]][solution[i + 1]];
                }
            }
            return sum;
        }

        private static double DistanceEuclid2D((double, double) a, (double, double) b)
        {
            return Math.Sqrt(Math.Pow(a.Item1 - b.Item1, 2) + Math.Pow(a.Item2 - b.Item2, 2));
        }

    }
}
