using SStimStat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SStimStat.Utils;
using SStimDataContracts;

namespace SStimStat.BAL
{
    public class TableLookupZ
    {
        private UnitOfWork unitOfWork;

        public TableLookupZ()
        {
            unitOfWork = new UnitOfWork();
        }
        public double GetZValue(int sampleSize, double sampleProportion, double populationProportion)
        {
            var se = Math.Sqrt(populationProportion * (1.0 - populationProportion) / (double)sampleSize);
            var distFromPopulationPropotion = sampleProportion - populationProportion;
            return distFromPopulationPropotion / se;
        }

        public double GetPValue(double zValue, IneqEnums tailProp)
        {
            double pValue;
            int integerPart = Convert.ToInt32(Math.Truncate(zValue));
            bool negative = zValue < 0;
            double decimalPart = zValue.GetDecimalPart();

            int decimal_1 = Convert.ToInt32(Math.Truncate((decimalPart * 10)));
            int decimal_2 = Convert.ToInt32(Math.Truncate((decimalPart * 100 - decimal_1 * 10)));

            double zFine = (double)decimal_2 / 100.0;

            int id = integerPart * 10;
            if (negative)
            {
                id -= decimal_1;
                id += 50;
            }
            else
            {
                id += decimal_1;
                id += 51;
            }
            var zRow = unitOfWork.ZRepository.GetByID(id);

            if (zFine < 0.005)
            {
                pValue = zRow.c_0.Value;
            }
            else if (zFine < 0.015)
            {
                pValue = zRow.c_0_01.Value;
            }
            else if (zFine < 0.025)
            {
                pValue = zRow.c_0_02.Value;
            }
            else if (zFine < 0.035)
            {
                pValue = zRow.c_0_03.Value;
            }
            else if (zFine < 0.045)
            {
                pValue = zRow.c_0_04.Value;
            }
            else if (zFine < 0.055)
            {
                pValue = zRow.c_0_05.Value;
            }
            else if (zFine < 0.065)
            {
                pValue = zRow.c_0_06.Value;
            }
            else if (zFine < 0.075)
            {
                pValue = zRow.c_0_07.Value;
            }
            else if (zFine < 0.085)
            {
                pValue = zRow.c_0_08.Value;
            }
            else
            {
                pValue = zRow.c_0_09.Value;
            }

            if (tailProp == IneqEnums.GT)
            {
                pValue = 1.0 - pValue;
            }
            else if (tailProp == IneqEnums.NEQ)
            {
                if (zValue < 0)
                {
                    pValue *= 2;
                }
                else
                {
                    pValue = 1.0 - pValue;
                    pValue *= 2;
                }
            }

            return pValue;
        }

        public double GetCriticalZValue(double reqdConfidenceLevel)
        {
            double criticalValue;
            double pValueForGivenCL = (1.0 - reqdConfidenceLevel) / 2.0 + reqdConfidenceLevel;

            var tRowList = unitOfWork.ZRepository.Get(r => r.c_0 <= pValueForGivenCL && r.c_0_09 >= pValueForGivenCL);

            var tRow = tRowList.Single();
            double crticalValueCoarse = tRow.Z.Value;
            double crticalValueFine = 0.0;

            if (pValueForGivenCL.IsLessThan(tRow.c_0_01.Value))
            {
                if(!pValueForGivenCL.IsNearerValueFirst(tRow.c_0.Value, tRow.c_0_01.Value))
                {
                    crticalValueFine = 0.01;
                }
            }
            else if (pValueForGivenCL.IsLessThan(tRow.c_0_02.Value))
            {
                crticalValueFine = 0.02;
                if (pValueForGivenCL.IsNearerValueFirst(tRow.c_0_01.Value, tRow.c_0_02.Value))
                {
                    crticalValueFine = 0.01;
                }
            }
            else if (pValueForGivenCL.IsLessThan(tRow.c_0_03.Value))
            {
                crticalValueFine = 0.03;
                if(pValueForGivenCL.IsNearerValueFirst(tRow.c_0_02.Value, tRow.c_0_03.Value))
                {
                    crticalValueFine = 0.02;
                }
            }
            else if (pValueForGivenCL.IsLessThan(tRow.c_0_04.Value))
            {
                crticalValueFine = 0.04;
                if(pValueForGivenCL.IsNearerValueFirst(tRow.c_0_03.Value, tRow.c_0_04.Value))
                {
                    crticalValueFine = 0.03;
                }
            }
            else if (pValueForGivenCL.IsLessThan(tRow.c_0_05.Value))
            {
                crticalValueFine = 0.05;
                if(pValueForGivenCL.IsNearerValueFirst(tRow.c_0_04.Value, tRow.c_0_05.Value))
                {
                    crticalValueFine = 0.04;
                }
            }
            else if (pValueForGivenCL.IsLessThan(tRow.c_0_06.Value))
            {
                crticalValueFine = 0.06;
                if(pValueForGivenCL.IsNearerValueFirst(tRow.c_0_05.Value, tRow.c_0_06.Value))
                {
                    crticalValueFine = 0.05;
                }
            }
            else if (pValueForGivenCL.IsLessThan(tRow.c_0_07.Value))
            {
                crticalValueFine = 0.07;
                if(pValueForGivenCL.IsNearerValueFirst(tRow.c_0_06.Value, tRow.c_0_07.Value))
                {
                    crticalValueFine = 0.06;
                }
            }
            else if (pValueForGivenCL.IsLessThan(tRow.c_0_08.Value))
            {
                crticalValueFine = 0.08;
                if(pValueForGivenCL.IsNearerValueFirst(tRow.c_0_07.Value, tRow.c_0_08.Value))
                {
                    crticalValueFine = 0.07;
                }
            }
            else if (pValueForGivenCL.IsLessThan(tRow.c_0_09.Value))
            {
                crticalValueFine = 0.09;
                if(pValueForGivenCL.IsNearerValueFirst(tRow.c_0_08.Value, tRow.c_0_09.Value))
                {
                    crticalValueFine = 0.08;
                }
            }
            else
            {
                crticalValueFine = 0.09;
            }

            criticalValue = crticalValueCoarse + crticalValueFine;
            return criticalValue;
        }

        public LookupResult GetValuesInTheRequiredCI(
            int sampleSize,
            double sampleProportion,
            double requiredCI)
        {
            var se = Math.Sqrt(sampleProportion * (1.0 - sampleProportion) / (double)sampleSize);
            LookupResult lookupResult = new LookupResult();
            var criticalValue = GetCriticalZValue(requiredCI);
            var me = criticalValue * se;
            lookupResult.ValueRangeLower = sampleProportion - me;
            lookupResult.ValueRangeUpper = sampleProportion + me;

            return lookupResult;
        }
    }
}