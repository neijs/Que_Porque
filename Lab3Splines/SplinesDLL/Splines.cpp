#include "pch.h"
#include "mkl.h"
#include "mkl_df_defines.h"

extern "C" __declspec(dllexport) void MKLSplines(MKL_INT nodesQnty, MKL_INT nodesQntyNU,
	const double* bounds, const double* firstValue, const double* secondValue, const double* deriv, const double* nuGrid,
	double* mes1, double* mes2, double* der1, double* der2, double* integValue, int& respCode);

void MKLSplines(MKL_INT nodesQnty, MKL_INT nodesQntyNU, const double* bounds, const double* firstValue,
	const double* secondValue, const double* deriv, const double* nuGrid, double* mes1,
	double* mes2, double* der1, double* der2, double* integValue, int& respCode)
{
	try 
	{
		DFTaskPtr task;

		// FIELD HAS 2 MEASURMENTS
		int vectDim = 2;

		// DORDER NOT 0 ELEMENTS QUANTITY
		int nder = 2;

		// QUANTITY OF INTEGRALS TO CALCULATE
		MKL_INT integQnty = 1;

		// DERIVATIVE ORDER TO CALCULATE
		MKL_INT ndorder = 3;

		// SPLINE AND SECOND SPLINE DERIVATIVE
		MKL_INT* dorder = new MKL_INT[ndorder] { 1, 0, 1 };

		// ARRAY OF VECTOR FUNCTION VALUES IN INTERPOLATION NODES
		double* nodesValue = new double[nodesQnty * vectDim];

		for (int i = 0; i < nodesQnty; ++i) {
			nodesValue[i] = firstValue[i];
			nodesValue[i + nodesQnty] = secondValue[i];
		}
		const double* constNodesValue = nodesValue;

		// ARRAY OF SPLINE COEFFICIENTS VALUES
		double* splineCoeff = new double[vectDim * DF_PP_CUBIC * (nodesQnty - 1)];

		// LEFT ENDS OF THE INTEGRATION SEGMENTS
		double* leftEnds = new double[1];

		leftEnds[0] = bounds[0];

		// RIGHT ENDS OF THE INTEGRATION SEGMENTS
		double* rightEnds = new double[1];

		rightEnds[0] = bounds[1];

		// THE RESULT: MES1, MES2, DER1, DER2
		double* result = new double[vectDim * nder * nodesQntyNU];

		/*-----------------------------------------------------------------------------------------------------*/

		// CREATE TASK
		respCode = dfdNewTask1D(&task, nodesQnty, bounds, DF_UNIFORM_PARTITION, vectDim, constNodesValue, DF_MATRIX_STORAGE_ROWS);
		if (respCode != DF_STATUS_OK) return;

		// EDIT SPLINE CONFIGURATION
		respCode = dfdEditPPSpline1D(task, DF_PP_CUBIC, DF_PP_NATURAL, (DF_BC_2ND_LEFT_DER | DF_BC_2ND_RIGHT_DER),
			deriv, DF_NO_IC, NULL, splineCoeff, DF_NO_HINT);
		if (respCode != DF_STATUS_OK) return;

		// CONSTRUCT SPLINE
		respCode = dfdConstruct1D(task, DF_PP_SPLINE, DF_METHOD_STD);
		if (respCode != DF_STATUS_OK) return;

		// CALCULATE SPLINE
		respCode = dfdInterpolate1D(task, DF_INTERP, DF_METHOD_PP, nodesQntyNU, nuGrid, DF_SORTED_DATA,
			ndorder, dorder, NULL, result, DF_MATRIX_STORAGE_ROWS, NULL);
		if (respCode != DF_STATUS_OK) return;

		// MES1, MES2, DER1, DER2 EXTRACTION
		for (int i = 0, j = 0, k = 0; i < nder * nodesQntyNU; ++i) {
			if (i % 2 == 0) {
				mes1[j] = result[i];
				mes2[j] = result[i + nder * nodesQntyNU];
				++j;
			}
			else {
				der1[k] = result[i];
				der2[k] = result[i + nder * nodesQntyNU];
				++k;
			}
		} // mes1[0] der1[0] mes1[1] der[1] ... mes2[0] der2[0] mes2[1] der2[1] ...
		// CALCULATE INTEGRALS
		respCode = dfdIntegrate1D(task, DF_METHOD_PP, integQnty, leftEnds, DF_NO_HINT,
			rightEnds, DF_NO_HINT, NULL, NULL, integValue, DF_MATRIX_STORAGE_ROWS);
		if (respCode != DF_STATUS_OK) return;

		// DELETE TASK
		respCode = dfDeleteTask(&task);
		if (respCode != DF_STATUS_OK) return;

		/*-----------------------------------------------------------------------------------------------------*/

		respCode = 0;

		delete[] dorder;
		delete[] nodesValue;
		delete[] splineCoeff;
		delete[] leftEnds;
		delete[] rightEnds;
		delete[] result;
	}
	catch (...)
	{
		respCode = -1;
	}
}