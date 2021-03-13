using System.Collections.Generic;

using ConstraintsCLI;
using Utility;

namespace Generations
{
    class RealRandomGeneration : AGeneration<double>
    {
        private readonly UniformRealRandom rng;

        public RealRandomGeneration(IConstraint<double> constraint, int? seed = null)
            : base(constraint)
        {
            rng = new UniformRealRandom(seed);
        }

        public override List<double> Fill(List<double> solution)
        {
            for (int i = 0; i < solution.Capacity; ++i)
            {
                solution.Add(rng.Next(constraint.tGetLowerBound(i), constraint.tGetUpperBound(i)));
            }

            return solution;
        }
    }
}
