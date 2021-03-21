using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mutations
{
    class RealJumpingRealMutationES11Adaptation : ARealMutationES11Adaptation
    {
        public RealJumpingRealMutationES11Adaptation(RealGaussianMutation mutation)
            : base(mutation) { }

        public override void Adapt(double beforeMutationValue, List<double> beforeMutationSolution, double afterMutationValue, List<double> afterMutationSolution)
        {
            if(afterMutationValue > beforeMutationValue)
            {
                Mutation.MultiplySigmas(0.5);
            }
            else
            {
                Mutation.MultiplySigmas(2);
            }
        }
    }
}
