using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using SStimStat.BAL;
using SStimDataContracts;

namespace SStimStat.Controllers
{
    public class InputDataHandlerController : ApiController
    {
        [Route("api/InputDataHandler/GetRegressionResults")]
        [HttpPost]
        public RegressionResult GetRegressionResults(InputDataRegression inputDataRegression)
        {
            RegressionResult regressionResult = new RegressionResult();
            //var nrObservations = inputDataRegression.ObservationsPredictorData.Count;

            RegressionStatistics regressionStatistics = new RegressionStatistics();
            var regressStats = regressionStatistics.CalculateRegressionStats(
                inputDataRegression.ResponseDataValues,
                inputDataRegression.Predictors,
                inputDataRegression.ObservationsPredictorData);

            #region COMMENTED OUT
            ////////////////////////////////
            //Dictionary<int, List<double>> dataPointsWithEachPredictor = new Dictionary<int, List<double>>();
            //for (int predictorNr = 0; predictorNr < inputDataRegression.Predictors.Count; predictorNr++)
            //{
            //    dataPointsWithEachPredictor[predictorNr] = new List<double>();

            //    for (var observationNr = 0; observationNr < nrObservations; observationNr++)
            //    {
            //        var dataPoint = inputDataRegression.ObservationsPredictorData.Values.ElementAt(observationNr).ElementAt(predictorNr);
            //        dataPointsWithEachPredictor[predictorNr].Add(dataPoint);
            //    }
            //}
            //var meanResponse = inputDataRegression.ResponseDataValues.Average();
            //List<double> modeledPointsWithOnePredictor = new List<double>();
            ////List<double> diffsFromMeanSq = new List<double>();
            //double diffsFromMeanSq = 0.0;
            //for (var observationNr = 0; observationNr < nrObservations; observationNr++)
            //{
            //    var predictedValue = regressStats.Item2[0]; // intercept

            //    for (int predictorNr = 0; predictorNr < 1; predictorNr++)
            //    {
            //        var valueDueToThisFactor = regressStats.Item2[predictorNr + 1] * inputDataRegression.ObservationsPredictorData.Values.ElementAt(observationNr).ElementAt(predictorNr);
            //        predictedValue += valueDueToThisFactor;
            //    }
            //    modeledPointsWithOnePredictor.Add(predictedValue);
            //    var diffFromMean = predictedValue - meanResponse;
            //    diffsFromMeanSq += diffFromMean * diffFromMean;
            //}

            //var se = regressStats.Item1.Item5;
            //var r12 = 
            //Statistics statistics = new Statistics();
            //var sx = statistics.GetStandardDeviation(dataPointsWithEachPredictor[0]);

            //var se_b1 = regressStats.Item1.Item5 / (sx * Math.Sqrt(nrObservations - inputDataRegression.Predictors.Count - 1));

            //////////////////////////
            #endregion

            regressionResult.Se = regressStats.Item1.Item5;
            regressionResult.RSq = regressStats.Item1.Item1;
            regressionResult.RAdjSq = regressStats.Item1.Item2;

            regressionResult.AovValuesRegression = new AovValuesRegression
            {
                Df = inputDataRegression.Predictors.Count,
                SS = regressStats.Item1.Item4 - regressStats.Item1.Item3, // SSR = SST - SSE
                MS = (regressStats.Item1.Item4 - regressStats.Item1.Item3) / inputDataRegression.Predictors.Count // MSR = SSR/(k)
            };

            regressionResult.AovValuesResidualError = new AovValuesResidualError
            {
                Df = inputDataRegression.ObservationsPredictorData.Keys.Count - inputDataRegression.Predictors.Count - 1,
                SS = regressStats.Item1.Item3, // SSE

                // MSE = SSE/(n - k -1)
                MS = regressStats.Item1.Item3 / (inputDataRegression.ObservationsPredictorData.Keys.Count - inputDataRegression.Predictors.Count - 1)
            };

            // F = MSR/MSE
            regressionResult.AovValuesRegression.FValue = regressionResult.AovValuesRegression.MS / regressionResult.AovValuesResidualError.MS;

            // P Value AovRegression
            TableLookupF tableLookupF = new TableLookupF();
            var pValue = tableLookupF.GetPValue(regressionResult.AovValuesRegression.FValue, regressionResult.AovValuesRegression.Df, regressionResult.AovValuesResidualError.Df);

            regressionResult.AovValuesRegression.PValue = pValue.Item2;
            regressionResult.AovValuesRegression.IsPValueLessThanThis = pValue.Item1;

            var mlrValuesLength = regressStats.Item2.Length;

            regressionResult.MlrValues = new MlrValues[mlrValuesLength];

            for (int count = 0; count < mlrValuesLength; count++)
            {
                regressionResult.MlrValues[count] = new MlrValues
                {
                    PredictorName = regressStats.Item2[count].Item1,
                    RegressionCoeff = regressStats.Item2[count].Item2,
                    SeCoeff = regressStats.Item2[count].Item3,
                    TValue = regressStats.Item2[count].Item4,
                    PValue = regressStats.Item2[count].Item5
                };
            }

            return regressionResult;
        }
    }
}