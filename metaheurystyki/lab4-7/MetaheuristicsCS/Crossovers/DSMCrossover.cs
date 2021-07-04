using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crossovers
{
    class DSMCrossover : ACrossover
    {
        public List<List<double>> dsm { set; get; }

        private readonly Random rng;

        public DSMCrossover(double probability, int? seed = null)
            : base(probability, seed)
        {
            dsm = null;
            rng = seed.HasValue ? new Random(seed.Value) : new Random();
        }

        protected override bool Cross<Element>(List<Element> parent1, List<Element> parent2, List<Element> offspring1, List<Element> offspring2)
        {
            int length = parent1.Count;
            int point1 = rng.Next(length);
            int point2;
            do
            {
                point2 = rng.Next(length);
            } while (point1 == point2);
            List<int> order = Enumerable.Range(0, length).Where(x => x != point1 && x != point2).OrderBy(x => rng.Next()).ToList();
            List<int> stayBlock = new List<int> { point1 };
            List<int> moveBlock = new List<int> { point2 };
            foreach(int i in order)
            {
                if(GetBlockDependency(moveBlock, i) > GetBlockDependency(stayBlock, i))
                {
                    moveBlock.Add(i);
                }
                else
                {
                    stayBlock.Add(i);
                }
            }

            for(int i=0; i<length; i++)
            {
                if (stayBlock.Contains(i))
                {
                    offspring1[i] = parent1[i];
                    offspring2[i] = parent2[i];
                }
                else
                {
                    offspring1[i] = parent2[i];
                    offspring2[i] = parent1[i];
                }
            }

            return true;
        }

        private double GetBlockDependency(List<int> block, int point)
        {
            List<double> blockVals = new List<double>();
            foreach(int i in block)
            {
                blockVals.Add(dsm[point][i]);
            }
            return blockVals.Average();
        }
    }
}
