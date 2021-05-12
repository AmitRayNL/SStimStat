using SStimStat.DAL;
using SStimStat.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SStimStat.BAL
{
    public class TableLookupF
    {
        private UnitOfWork unitOfWork;

        public TableLookupF()
        {
            unitOfWork = new UnitOfWork();
        }

        public Tuple<bool, double> GetPValue(double fValueCalculated, int dfNumerator, int dfDenominator)
        {
            var pValue = GetPValue(fValueCalculated, dfNumerator, dfDenominator, 0.025);
            if (pValue.Item1 != true )
            {
                pValue = GetPValue(fValueCalculated, dfNumerator, dfDenominator, 0.05);
            }
            return pValue;
        }

        public Tuple<bool, double> GetPValue(double fValueCalculated, int dfNumerator, int dfDenominator, double alpha)
        {
            if (dfDenominator > 40 && dfDenominator < 60)
            {
                dfDenominator = dfDenominator.GetNearerValue(40, 60);
            }
            else if (dfDenominator > 60 && dfDenominator < 120)
            {
                dfDenominator = dfDenominator.GetNearerValue(60, 120);
            }
            else if (dfDenominator > 120 && dfDenominator < 250)
            {
                dfDenominator = dfDenominator.GetNearerValue(120, 250);
            }
            else if (dfDenominator > 250 && dfDenominator < 400)
            {
                dfDenominator = dfDenominator.GetNearerValue(250, 400);
            }
            else if (dfDenominator > 400 && dfDenominator < 1000)
            {
                dfDenominator = dfDenominator.GetNearerValue(400, 1000);
            }
            else if (dfDenominator > 1000)
            {
                dfDenominator = 1000;
            }

            if (alpha.IsEqualTo(0.025))
            {
                return GetPValueForAlpha_0_025(fValueCalculated, dfDenominator, dfNumerator);
            }
            else
            {
                return GetPValueForAlpha_0_05(fValueCalculated, dfDenominator, dfNumerator);
            }
        }

        public Tuple<bool, double> GetPValueForAlpha_0_025(double fValueCalculated, int dfNumerator, int dfDenominator)
        {
            var tRowFTable = unitOfWork.F_alpha_0_025Repository.GetByID(dfDenominator);
            double fValueFromTable;

            if (dfNumerator == 1)
            {
                fValueFromTable = tRowFTable.ndf_1.Value;
            }
            else if (dfNumerator == 2)
            {
                fValueFromTable = tRowFTable.ndf_2.Value;
            }
            else if (dfNumerator == 3)
            {
                fValueFromTable = tRowFTable.ndf_3.Value;
            }
            else if (dfNumerator == 4)
            {
                fValueFromTable = tRowFTable.ndf_4.Value;
            }
            else if (dfNumerator == 5)
            {
                fValueFromTable = tRowFTable.ndf_5.Value;
            }
            else if (dfNumerator == 6)
            {
                fValueFromTable = tRowFTable.ndf_6.Value;
            }
            else if (dfNumerator == 7)
            {
                fValueFromTable = tRowFTable.ndf_7.Value;
            }
            else if (dfNumerator == 8)
            {
                fValueFromTable = tRowFTable.ndf_8.Value;
            }
            else if (dfNumerator == 9)
            {
                fValueFromTable = tRowFTable.ndf_9.Value;
            }
            else if (dfNumerator <= 11)
            {
                fValueFromTable = tRowFTable.ndf_10.Value;
            }
            else if (dfNumerator <= 13)
            {
                fValueFromTable = tRowFTable.ndf_12.Value;
            }
            else if (dfNumerator <= 17)
            {
                fValueFromTable = tRowFTable.ndf_15.Value;
            }
            else if (dfNumerator <= 22)
            {
                fValueFromTable = tRowFTable.ndf_20.Value;
            }
            else if (dfNumerator <= 27)
            {
                fValueFromTable = tRowFTable.ndf_24.Value;
            }
            else if (dfNumerator <= 35)
            {
                fValueFromTable = tRowFTable.ndf_30.Value;
            }
            else if (dfNumerator <= 50)
            {
                fValueFromTable = tRowFTable.ndf_40.Value;
            }
            else if (dfNumerator <= 90)
            {
                fValueFromTable = tRowFTable.ndf_60.Value;
            }
            else
            {
                fValueFromTable = tRowFTable.ndf_120.Value;
            }

            bool actualPValueIsLess = true;
            if (fValueCalculated <= fValueFromTable)
            {
                actualPValueIsLess = false;
            }

            return new Tuple<bool, double>(actualPValueIsLess, 0.025);
        }

        public Tuple<bool, double> GetPValueForAlpha_0_05(double fValueCalculated, int dfNumerator, int dfDenominator)
        {
            var tRowFTable = unitOfWork.F_alpha_0_05Repository.GetByID(dfDenominator);
            double fValueFromTable;

            if (dfNumerator == 1)
            {
                fValueFromTable = tRowFTable.ndf_1.Value;
            }
            else if (dfNumerator == 2)
            {
                fValueFromTable = tRowFTable.ndf_2.Value;
            }
            else if (dfNumerator == 3)
            {
                fValueFromTable = tRowFTable.ndf_3.Value;
            }
            else if (dfNumerator == 4)
            {
                fValueFromTable = tRowFTable.ndf_4.Value;
            }
            else if (dfNumerator == 5)
            {
                fValueFromTable = tRowFTable.ndf_5.Value;
            }
            else if (dfNumerator == 6)
            {
                fValueFromTable = tRowFTable.ndf_6.Value;
            }
            else if (dfNumerator == 7)
            {
                fValueFromTable = tRowFTable.ndf_7.Value;
            }
            else if (dfNumerator == 8)
            {
                fValueFromTable = tRowFTable.ndf_8.Value;
            }
            else if (dfNumerator == 9)
            {
                fValueFromTable = tRowFTable.ndf_9.Value;
            }
            else if (dfNumerator <= 11)
            {
                fValueFromTable = tRowFTable.ndf_10.Value;
            }
            else if (dfNumerator <= 13)
            {
                fValueFromTable = tRowFTable.ndf_12.Value;
            }
            else if (dfNumerator <= 17)
            {
                fValueFromTable = tRowFTable.ndf_15.Value;
            }
            else if (dfNumerator <= 22)
            {
                fValueFromTable = tRowFTable.ndf_20.Value;
            }
            else if (dfNumerator <= 27)
            {
                fValueFromTable = tRowFTable.ndf_24.Value;
            }
            else if (dfNumerator <= 35)
            {
                fValueFromTable = tRowFTable.ndf_30.Value;
            }
            else if (dfNumerator <= 50)
            {
                fValueFromTable = tRowFTable.ndf_40.Value;
            }
            else if (dfNumerator <= 90)
            {
                fValueFromTable = tRowFTable.ndf_60.Value;
            }
            else
            {
                fValueFromTable = tRowFTable.ndf_120.Value;
            }

            bool actualPValueIsLess = true;
            if (fValueCalculated <= fValueFromTable)
            {
                actualPValueIsLess = false;
            }

            return new Tuple<bool, double>(actualPValueIsLess, 0.05);
        }

    }
}