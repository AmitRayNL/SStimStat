using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SStimStat.BAL
{
    public class Statistics
    {
        public double GetZScore(double dataValue, double meanValue, double sd)
        {
            return (dataValue - meanValue) / sd;
        }

        public double GetMeanValue(List<double> listValues)
        {
            return listValues.Average();
        }

        public double GetStandardDeviation(List<double> listValues)
        {
            double sd = 0;
            if (listValues.Any())
            {
                var count = listValues.Count;
                var avg = listValues.Average();
                double sum = listValues.Sum(v => Math.Pow(v - avg, 2));
                sd = Math.Sqrt(sum / (count - 1));
            }
            return sd;
        }

        public double GetStandardDeviationRegressionStats(List<double> listValuesDiff, int nrPredictors)
        {
            double sd = 0;
            if (listValuesDiff.Any())
            {
                var count = listValuesDiff.Count;
                double sum = listValuesDiff.Sum(v => Math.Pow(v, 2));
                sd = Math.Sqrt(sum / (count - nrPredictors - 1));
            }
            return sd;
        }


    }
}