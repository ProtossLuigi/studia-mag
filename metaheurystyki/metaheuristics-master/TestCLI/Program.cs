using ConstraintsCLI;
using EvaluationsCLI;
using System;
using System.Collections.Generic;

namespace TestCLR
{
    class Program
    {
        static void Main(string[] args)
        {
            CBinaryOneMaxEvaluation test = new CBinaryOneMaxEvaluation(5);

            bool[] arr = { true, true, true, false, false };
            List<bool> list = new List<bool> { true, true, true, false, false };

            Console.WriteLine(test.dEvaluate(arr));
            Console.WriteLine(test.dEvaluate(list));

            Console.WriteLine(test.iSize);
            Console.WriteLine(test.dMaxValue);

            IConstraint<bool> constraint = test.pcConstraint;

            Console.WriteLine(constraint.tGetLowerBound(0));
            Console.WriteLine(constraint.tGetUpperBound(0));

            CBinaryNKLandscapesEvaluation nk = new CBinaryNKLandscapesEvaluation(100);

            bool[] sol = new bool[100];

            Console.WriteLine(nk.dEvaluate(sol));

            CBinaryIsingSpinGlassEvaluation ising = new CBinaryIsingSpinGlassEvaluation(484);

            sol = new bool[484];

            Console.WriteLine(ising.dEvaluate(sol));
        }
    }
}
