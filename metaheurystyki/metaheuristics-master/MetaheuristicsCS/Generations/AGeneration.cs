using System.Collections.Generic;

using ConstraintsCLI;

namespace Generations
{
    abstract class AGeneration<Element>
    {
        protected IConstraint<Element> constraint;

        public AGeneration(IConstraint<Element> constraint)
        {
            this.constraint = constraint;
        }

        public List<Element> Create(int size)
        {
            return Fill(new List<Element>(size));
        }

        public abstract List<Element> Fill(List<Element> solution);
    }
}
