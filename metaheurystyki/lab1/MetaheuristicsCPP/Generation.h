#pragma once

#include <Constraint.h>
#include <vector>

using namespace Constraints;

using namespace std;

namespace Generations
{
	template <typename TElement>
	class IGeneration
	{
	public:
		virtual ~IGeneration() = default;

		virtual vector<TElement> *pvCreate(int iSize) = 0;
		virtual void vFill(vector<TElement> &vSolution) = 0;
	};//class IGeneration


	template <typename TElement>
	class CGeneration : public IGeneration<TElement>
	{
	public:
		CGeneration(IConstraint<TElement> &cConstraint) : c_constraint(cConstraint) { }

		virtual vector<TElement> *pvCreate(int iSize)
		{
			vector<TElement> *pv_solution = new vector<TElement>((size_t)iSize);

			this->vFill(*pv_solution);
			
			return pv_solution;
		}//virtual vector<TElement> *pvCreate(int iSize)

	protected:
		IConstraint<TElement> &c_constraint;
	};//class CGeneration : public IGeneration<TElement>
}//namespace Generations