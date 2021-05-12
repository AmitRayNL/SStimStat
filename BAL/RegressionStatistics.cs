using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearRegression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SStimDataContracts;

namespace SStimStat.BAL
{
    public class RegressionStatistics
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseDataValues"></param>
        /// <param name="observations"></param>
        /// <returns>Tuple of (Tuple of (RSq, RAdjSq, sse, sst, se), Array of Tuples of (predictor name, coeff, se_coeff, t value, p value)) )</returns>
        public Tuple<Tuple<double, double, double, double, double>, Tuple<string, double, double, double, double>[]> CalculateRegressionStats(
            List<double> responseDataValues,
            List<string> predictorNames,
            Dictionary<string, List<double>> observations)
        {
            Tuple<Tuple<double, double, double, double, double>, Tuple<string, double, double, double, double>[]> regressionStats;
            var countResponseData = responseDataValues.Count();
            var nrObservations = observations.Count;
            int nrPredictors = predictorNames.Count;

            if (countResponseData == nrObservations)
            {
                #region COMMENTED OUT
                //double[][] x = new double[observations.Count][];
                //int observationNr = 0;
                //foreach (var kvp in observations)
                //{
                //    x[observationNr] = kvp.Value.ToArray();
                //    observationNr++;
                //}

                //var y = responseDataValues.ToArray();
                //var coeffs = MultipleRegression.DirectMethod(x, y, true, DirectRegressionMethod.NormalEquations);
                //var coeffs2 = Fit.MultiDim(x, y, true, DirectRegressionMethod.NormalEquations);
                #endregion

                var coeffData = GetCoeffData(predictorNames, responseDataValues, observations, out List<double> modeledPointsWithAllPredictors);
                var rSqValues = GetRSqValue(responseDataValues, modeledPointsWithAllPredictors, nrPredictors);
                regressionStats = new Tuple<
                    Tuple<double, double, double, double, double>, 
                    Tuple<string, double, double, double, double>[]>(rSqValues, coeffData);
            }
            else
            {
                regressionStats = new Tuple<
                    Tuple<double, double, double, double, double>, 
                    Tuple<string, double, double, double, double>[]>(null, null);
            }
            return regressionStats;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseDataValues"></param>
        /// <param name="observations"></param>
        /// <param name="coeffMatrix"></param>
        /// <param name="nrPredictors"></param>
        /// <returns>Tuple of (mse, List PredictedValues)</returns>
        private Tuple<double, List<double>> GetMeanSquareErrorPredictedValues(
            List<double> responseDataValues, 
            Dictionary<string, List<double>> observations, 
            Matrix<double> coeffMatrix,
            int nrPredictors)
        {
            var sse = 0.0;
            List<double> modeledPointsWithAllPredictors = new List<double>();
            var countObservations = observations.Count;
            for (var observationNr = 0; observationNr < countObservations; observationNr++)
            {
                var predictedValue = coeffMatrix[0, 0]; // intercept
                for (int predictorNr = 0; predictorNr < nrPredictors; predictorNr++)
                {
                    var matrixRowNr = predictorNr + 1;
                    var valueDueToThisFactor = coeffMatrix[matrixRowNr, 0] * observations.Values.ElementAt(observationNr).ElementAt(predictorNr);
                    predictedValue += valueDueToThisFactor;
                }
                var actualValue = responseDataValues.ElementAt(observationNr);
                var residual = actualValue - predictedValue;
                var residualSq = residual * residual;
                sse += residualSq;
                modeledPointsWithAllPredictors.Add(predictedValue);
            }
            var df = countObservations - nrPredictors - 1;
            var mse = sse / df;
            return new Tuple<double, List<double>>(mse, modeledPointsWithAllPredictors);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predictorNames"></param>
        /// <param name="responseDataValues"></param>
        /// <param name="observations"></param>
        /// <param name="predictedValues"></param>
        /// <returns>Array of tuples of (predictorName, coeff, se_coeff, t_value, p_value)</returns>
        private Tuple<string, double, double, double, double>[] GetCoeffData(
            List<string> predictorNames, 
            List<double> responseDataValues, 
            Dictionary<string, List<double>> observations,
            out List<double> predictedValues)
        {
            var nrPredictors = predictorNames.Count;
            var nrObservations = observations.Count;
            var df = nrObservations - nrPredictors - 1;
            var coeffArray = new Tuple<string, double, double, double, double>[nrPredictors + 1];
            double[,] YData = ConvertResponseDataY(responseDataValues);
            double[,] XData = ConvertPredictorDataX(observations);

            // ref: https://support.minitab.com/en-us/minitab-express/1/help-and-how-to/modeling-statistics/regression/how-to/multiple-regression/methods-and-formulas/methods-and-formulas/
            var XMatrixBuilder = Matrix<double>.Build;
            var YMatrixBuilder = Matrix<double>.Build;
            var X = XMatrixBuilder.DenseOfArray(XData);
            var Y = YMatrixBuilder.DenseOfArray(YData);

            var XT = X.Transpose();

            var XTxX = XT * X;

            var XTxX_Inv = XTxX.Inverse();
            var XTxX_Inv_x_X = XTxX_Inv * XT;
            var b = XTxX_Inv_x_X * Y;

            var meanSqErrorPredictedValues = 
                GetMeanSquareErrorPredictedValues(responseDataValues, observations, b, nrPredictors);

            var coeffSe = XTxX_Inv.Multiply(meanSqErrorPredictedValues.Item1);
            TableLookupT tableLookupT = new TableLookupT();
            for (var count = 0; count < (nrPredictors + 1); count++)
            {
                var coeff = b[count, 0];
                var se_coeff = Math.Sqrt(coeffSe[count, count]);
                var t_value = coeff / se_coeff;
                var p_value = tableLookupT.GetPValue(t_value, df, IneqEnums.NEQ);
                var predictorName = "Intercept";
                if (count > 0)
                {
                    predictorName = predictorNames.ElementAt(count - 1);
                }
                coeffArray[count] = new Tuple<string, double, double, double, double>(predictorName, coeff, se_coeff, t_value, p_value);
            }
            predictedValues = meanSqErrorPredictedValues.Item2;
            return coeffArray;
        }

        public double[,] ConvertResponseDataY(List<double> responseDataValues)
        {
            var nrObservations = responseDataValues.Count;
            var convertedArray = new double[nrObservations, 1];

            for (int row = 0; row < nrObservations; row++)
            {
                convertedArray[row, 0] = responseDataValues.ElementAt(row);
            }
            return convertedArray;
        }

        public double[,] ConvertPredictorDataX(Dictionary<string, List<double>> observations)
        {
            var nrRows = observations.Count;
            var nrCols = observations.Values.ElementAt(0).Count + 1; // Put 1 in first column
            var convertedArray = new double[nrRows, nrCols];

            for (int row = 0; row < nrRows; row++)
            {
                var kvp = observations.ElementAt(row);
                convertedArray[row, 0] = 1;
                for (int col = 1; col < nrCols; col++)
                {
                    convertedArray[row, col] = kvp.Value.ElementAt(col - 1);
                }
            }
            return convertedArray;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataPoints"></param>
        /// <param name="predictedPoints"></param>
        /// <param name="nrPredictors"></param>
        /// <returns>Tuple of RSq, RAdjSq, sse, sst, se</returns>
        public Tuple<double, double, double, double, double> GetRSqValue(List<double> dataPoints, List<double> predictedPoints, int nrPredictors)
        {
            var countObservations = dataPoints.Count;
            double sse = 0.0;
            double sst = 0.0;
            double meanValue = dataPoints.Average();
            List<double> residuals = new List<double>();

            Statistics statistics = new Statistics();

            for (var observationNr = 0; observationNr < countObservations; observationNr++)
            {
                var dataPoint = dataPoints.ElementAt(observationNr);
                var predictedPoint = predictedPoints.ElementAt(observationNr);
                var residual = dataPoint - predictedPoint;
                var residualSq = residual * residual;
                var diffFromMean = dataPoint - meanValue;
                var diffFromMeanSq = diffFromMean * diffFromMean;
                sse += residualSq;
                sst += diffFromMeanSq;
                residuals.Add(residual);
            }
            var se = statistics.GetStandardDeviationRegressionStats(residuals, nrPredictors);
            double rSqValue = 1.0 - sse / sst;
            double rAjdSqValue = 1.0 - (1.0 - rSqValue) * (countObservations - 1) / (countObservations - nrPredictors - 1);
            return new Tuple<double, double, double, double, double>(rSqValue, rAjdSqValue, sse, sst, se);
        }
    }
}