using SStimStat.DAL;
using SStimDataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SStimStat.Utils;

namespace SStimStat.BAL
{
    public class TableLookupT
    {
        private UnitOfWork unitOfWork;
        public TableLookupT()
        {
            unitOfWork = new UnitOfWork();
        }

        public LookupResult GetCValue(double tValue, int df)
        {
            LookupResult tLookupResultCValue = new LookupResult();

            if (df > 100 && df <= 1000)
            {
                df = 1000;
            }
            else if (df > 1000)
            {
                df = 10000;
            }

            var tRow = unitOfWork.TRepository.GetByID(df);

            if (tValue.IsLessThan(tRow.c_0_25.Value))
            {
                tLookupResultCValue.ValueRangeLower = 0.0;
                tLookupResultCValue.ValueRangeUpper = 0.5;
            }
            else if (tValue.IsEqualTo(tRow.c_0_25.Value))
            {
                tLookupResultCValue.ValueRangeLower = 0.5;
                tLookupResultCValue.ValueRangeUpper = 0.5;
            }
            else if (tValue.IsLessThan(tRow.c_0_1.Value))
            {
                tLookupResultCValue.ValueRangeLower = 0.5;
                tLookupResultCValue.ValueRangeUpper = 0.8;
            }
            else if (tValue.IsEqualTo(tRow.c_0_1.Value))
            {
                tLookupResultCValue.ValueRangeLower = 0.8;
                tLookupResultCValue.ValueRangeUpper = 0.8;
            }
            else if (tValue.IsLessThan(tRow.c_0_05.Value))
            {
                tLookupResultCValue.ValueRangeLower = 0.8;
                tLookupResultCValue.ValueRangeUpper = 0.9;
            }
            else if (tValue.IsEqualTo(tRow.c_0_05.Value))
            {
                tLookupResultCValue.ValueRangeLower = 0.9;
                tLookupResultCValue.ValueRangeUpper = 0.9;
            }
            else if (tValue.IsLessThan(tRow.c_0_025.Value))
            {
                tLookupResultCValue.ValueRangeLower = 0.9;
                tLookupResultCValue.ValueRangeUpper = 0.95;
            }
            else if (tValue.IsEqualTo(tRow.c_0_025.Value))
            {
                tLookupResultCValue.ValueRangeLower = 0.95;
                tLookupResultCValue.ValueRangeUpper = 0.95;
            }
            else if (tValue.IsLessThan(tRow.c_0_01.Value))
            {
                tLookupResultCValue.ValueRangeLower = 0.95;
                tLookupResultCValue.ValueRangeUpper = 0.98;
            }
            else if (tValue.IsEqualTo(tRow.c_0_01.Value))
            {
                tLookupResultCValue.ValueRangeLower = 0.98;
                tLookupResultCValue.ValueRangeUpper = 0.98;
            }
            else if (tValue.IsLessThan(tRow.c_0_005.Value))
            {
                tLookupResultCValue.ValueRangeLower = 0.98;
                tLookupResultCValue.ValueRangeUpper = 0.99;
            }
            else if (tValue.IsEqualTo(tRow.c_0_005.Value))
            {
                tLookupResultCValue.ValueRangeLower = 0.99;
                tLookupResultCValue.ValueRangeUpper = 0.99;
            }
            else
            {
                tLookupResultCValue.ValueRangeLower = 0.99;
                tLookupResultCValue.ValueRangeUpper = 0.999;
            }
            return tLookupResultCValue;
        }

        public double GetCriticalTValue(double reqdConfidenceLevel, int df)
        {
            double criticalValue;

            if (df > 100 && df <= 1000)
            {
                df = 1000;
            }
            else if (df > 1000)
            {
                df = 10000;
            }

            var tRow = unitOfWork.TRepository.GetByID(df);

            if (reqdConfidenceLevel.IsLessThan(0.5))
            {
                criticalValue = 0.0;
            }
            else if (reqdConfidenceLevel.IsLessThan(0.8))
            {
                criticalValue = tRow.c_0_25.Value;
            }
            else if (reqdConfidenceLevel.IsLessThan(0.9))
            {
                criticalValue = tRow.c_0_1.Value;
            }
            else if (reqdConfidenceLevel.IsLessThan(0.95))
            {
                criticalValue = tRow.c_0_05.Value;
            }
            else if (reqdConfidenceLevel.IsLessThan(0.98))
            {
                criticalValue = tRow.c_0_025.Value;
            }
            else if (reqdConfidenceLevel.IsLessThan(0.99))
            {
                criticalValue = tRow.c_0_01.Value;
            }
            else if (reqdConfidenceLevel.IsLessThan(0.998))
            {
                criticalValue = tRow.c_0_005.Value;
            }
            else
            {
                criticalValue = tRow.c_0_005.Value;
            }
            return criticalValue;
        }


        /// <summary>
        /// Gets the single-tail P Value
        /// </summary>
        /// <param name="tValue"></param>
        /// <param name="df"></param>
        /// <returns></returns>
        public double GetPValue(double tValue, int df, IneqEnums tailProp)
        {
            if (tValue < 0)
            {
                tValue *= -1;
            }

            double pValue;

            LookupResult tLookupResultPValue = new LookupResult();

            if (df > 100 && df <= 1000)
            {
                df = 1000;
            }
            else if (df > 1000)
            {
                df = 10000;
            }

            var tRow = unitOfWork.TRepository.GetByID(df);

            if (tValue.IsLessThan(tRow.c_0_25.Value))
            {
                pValue = 0.5;
                tLookupResultPValue.ValueRangeLower = 0.25;
                tLookupResultPValue.ValueRangeUpper = 1.0;
            }
            else if (tValue.IsEqualTo(tRow.c_0_25.Value))
            {
                pValue = 0.25;
                tLookupResultPValue.ValueRangeLower = 0.25;
                tLookupResultPValue.ValueRangeUpper = 0.25;
            }
            else if (tValue.IsLessThan(tRow.c_0_1.Value))
            {
                pValue = 0.1;
                if (tValue.IsNearerValueFirst(tRow.c_0_25.Value, tRow.c_0_1.Value))
                {
                    pValue = 0.25;
                }
                tLookupResultPValue.ValueRangeLower = 0.1;
                tLookupResultPValue.ValueRangeUpper = 0.25;
            }
            else if (tValue.IsEqualTo(tRow.c_0_1.Value))
            {
                pValue = 0.1;
                tLookupResultPValue.ValueRangeLower = 0.1;
                tLookupResultPValue.ValueRangeUpper = 0.1;
            }
            else if (tValue.IsLessThan(tRow.c_0_05.Value))
            {
                pValue = 0.05;
                if (tValue.IsNearerValueFirst(tRow.c_0_1.Value, tRow.c_0_05.Value))
                {
                    pValue = 0.1;
                }
                tLookupResultPValue.ValueRangeLower = 0.05;
                tLookupResultPValue.ValueRangeUpper = 0.1;
            }
            else if (tValue.IsEqualTo(tRow.c_0_05.Value))
            {
                pValue = 0.05;
                tLookupResultPValue.ValueRangeLower = 0.05;
                tLookupResultPValue.ValueRangeUpper = 0.05;
            }
            else if (tValue.IsLessThan(tRow.c_0_025.Value))
            {
                pValue = 0.025;
                if (tValue.IsNearerValueFirst(tRow.c_0_05.Value, tRow.c_0_025.Value))
                {
                    pValue = 0.05;
                }
                tLookupResultPValue.ValueRangeLower = 0.025;
                tLookupResultPValue.ValueRangeUpper = 0.05;
            }
            else if (tValue.IsEqualTo(tRow.c_0_025.Value))
            {
                pValue = 0.025;
                tLookupResultPValue.ValueRangeLower = 0.025;
                tLookupResultPValue.ValueRangeUpper = 0.025;
            }
            else if (tValue.IsLessThan(tRow.c_0_01.Value))
            {
                pValue = 0.01;
                if (tValue.IsNearerValueFirst(tRow.c_0_025.Value, tRow.c_0_01.Value))
                {
                    pValue = 0.025;
                }
                tLookupResultPValue.ValueRangeLower = 0.01;
                tLookupResultPValue.ValueRangeUpper = 0.025;
            }
            else if (tValue.IsEqualTo(tRow.c_0_01.Value))
            {
                pValue = 0.01;
                tLookupResultPValue.ValueRangeLower = 0.01;
                tLookupResultPValue.ValueRangeUpper = 0.01;
            }
            else if (tValue.IsLessThan(tRow.c_0_005.Value))
            {
                pValue = 0.005;
                if (tValue.IsNearerValueFirst(tRow.c_0_01.Value, tRow.c_0_005.Value))
                {
                    pValue = 0.01;
                }
                tLookupResultPValue.ValueRangeLower = 0.005;
                tLookupResultPValue.ValueRangeUpper = 0.01;
            }
            else if (tValue.IsEqualTo(tRow.c_0_005.Value))
            {
                pValue = 0.005;
                tLookupResultPValue.ValueRangeLower = 0.005;
                tLookupResultPValue.ValueRangeUpper = 0.005;
            }
            else
            {
                pValue = 0.000;
                tLookupResultPValue.ValueRangeLower = 0.0;
                tLookupResultPValue.ValueRangeUpper = 0.005;
            }

            if (tailProp == IneqEnums.NEQ)
            {
                // Two-tail probability
                pValue *= 2.0;
                tLookupResultPValue.ValueRangeLower *= 2.0;
                tLookupResultPValue.ValueRangeUpper *= 2.0;
            }
            return pValue;
        }

        public double GetTValue(int sampleSize, double sampleMean, double populationMean, double sampleSd)
        {
            var se = sampleSd / Math.Sqrt(sampleSize);
            var distFromPopulationMean = sampleMean - populationMean;
            return distFromPopulationMean / se;
        }

        public Tuple<double, double> GetTSeValues(int sampleSize, double sampleMean, double populationMean, double sampleSd)
        {
            var se = sampleSd / Math.Sqrt(sampleSize);
            var distFromPopulationMean = sampleMean - populationMean;
            var tValue = distFromPopulationMean / se;
            Tuple<double, double> tSeValues = new Tuple<double, double>(tValue, se);
            return tSeValues;
        }


        public LookupResult GetValuesInTheRequiredCI(
            int sampleSize, 
            double sampleMean, 
            double sampleSd,
            double requiredCI)
        {
            int df = sampleSize - 1;
            LookupResult lookupResult = new LookupResult();
            var se = sampleSd / Math.Sqrt(sampleSize);
            var criticalValue = GetCriticalTValue(requiredCI, df);
            var me = criticalValue * se;
            lookupResult.ValueRangeLower = sampleMean - me;
            lookupResult.ValueRangeUpper = sampleMean + me;

            return lookupResult;
        }
    }
}