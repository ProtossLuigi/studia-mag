#include "BinaryEvaluations.h"

using namespace EvaluationsCLI;


CBinaryEvaluation::CBinaryEvaluation(Evaluations::CEvaluation<bool> *pcNativeBinaryEvaluation)
{
	pc_native_binary_evaluation = pcNativeBinaryEvaluation;
}//CBinaryEvaluation::CBinaryEvaluation(Evaluations::CEvaluation<bool> *pcNativeBinaryEvaluation)

CBinaryEvaluation::~CBinaryEvaluation()
{
	delete pc_native_binary_evaluation;
}//CBinaryEvaluation::~CBinaryEvaluation()

CBinaryEvaluation::!CBinaryEvaluation()
{
	delete pc_native_binary_evaluation;
}//CBinaryEvaluation::!CBinaryEvaluation()

double CBinaryEvaluation::dEvaluate(IList<bool> ^lSolution)
{
	vector<bool> v_solution(0);
	v_solution.reserve(lSolution->Count);

	for (int i = 0; i < lSolution->Count; i++)
	{
		v_solution.push_back(lSolution[i]);
	}//for (int i = 0; i < lSolution->Count; i++)

	return pc_native_binary_evaluation->dEvaluate(v_solution);
}//double CBinaryEvaluation::dEvaluate(IList<bool> ^lSolution) 


CBinaryOneMaxEvaluation::CBinaryOneMaxEvaluation(int iSize)
	: CBinaryEvaluation(new Evaluations::CBinaryOneMaxEvaluation(iSize))
{

}//CBinaryOneMaxEvaluation::CBinaryOneMaxEvaluation(int iSize)


CBinaryStandardDeceptiveConcatenationEvaluation::CBinaryStandardDeceptiveConcatenationEvaluation(int iBlockSize, int iNumberOfBlocks)
	: CBinaryEvaluation(new Evaluations::CBinaryStandardDeceptiveConcatenationEvaluation(iBlockSize, iNumberOfBlocks))
{

}//CBinaryStandardDeceptiveConcatenationEvaluation::CBinaryStandardDeceptiveConcatenationEvaluation(int iBlockSize, int iNumberOfBlocks)


CBinaryBimodalDeceptiveConcatenationEvaluation::CBinaryBimodalDeceptiveConcatenationEvaluation(int iBlockSize, int iNumberOfBlocks)
	: CBinaryEvaluation(new Evaluations::CBinaryBimodalDeceptiveConcatenationEvaluation(iBlockSize, iNumberOfBlocks))
{

}//CBinaryBimodalDeceptiveConcatenationEvaluation::CBinaryBimodalDeceptiveConcatenationEvaluation(int iBlockSize, int iNumberOfBlocks)


CBinaryIsingSpinGlassEvaluation::CBinaryIsingSpinGlassEvaluation(int iSize)
	: CBinaryIsingSpinGlassEvaluation(iSize, 0)
{

}//CBinaryIsingSpinGlassEvaluation::CBinaryIsingSpinGlassEvaluation(int iSize)

CBinaryIsingSpinGlassEvaluation::CBinaryIsingSpinGlassEvaluation(int iSize, int iProblemSeed)
	: CBinaryEvaluation(new Evaluations::CBinaryIsingSpinGlassEvaluation(iSize, iProblemSeed))
{

}//CBinaryIsingSpinGlassEvaluation::CBinaryIsingSpinGlassEvaluation(int iSize, int iProblemSeed)


CBinaryNKLandscapesEvaluation::CBinaryNKLandscapesEvaluation(int iSize)
	: CBinaryNKLandscapesEvaluation(iSize, 0)
{

}//CBinaryNKLandscapesEvaluation::CBinaryNKLandscapesEvaluation(int iSize)

CBinaryNKLandscapesEvaluation::CBinaryNKLandscapesEvaluation(int iSize, int iProblemSeed)
	: CBinaryEvaluation(new Evaluations::CBinaryNKLandscapesEvaluation(iSize, iProblemSeed))
{

}//CBinaryNKLandscapesEvaluation::CBinaryNKLandscapesEvaluation(int iSize, int iProblemSeed)