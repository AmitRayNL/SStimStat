using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace SStimStat.Utils
{
    public static class Extensions
    {
        private const double epsilon = 0.000001;

        /// <summary>
        /// Returns true, if equal
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static bool IsEqualTo(this double d1, double d2)
        {
            bool equal = false;
            if (Math.Abs(d1 - d2) < epsilon)
            {
                equal = true;
            }
            return equal;     
        }

        /// <summary>
        /// Returns true, if equal
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static bool IsLessThan(this double d1, double d2)
        {
            bool isLessThan = false;
            if (d1 + epsilon < d2)
            {
                isLessThan = true;
            }
            return isLessThan;
        }

        public static double GetDecimalPart(this double d)
        {
            string doubleAsString = d.ToString("G", CultureInfo.InvariantCulture);
            int indexOfDecimal = doubleAsString.IndexOf(".");
            var decimalPartAsString = doubleAsString.Substring(indexOfDecimal);
            return double.Parse(decimalPartAsString, CultureInfo.InvariantCulture);
        }

        public static double GetNearerValue(this double d, double value1, double value2)
        {
            var diff1 = Math.Abs(d - value1);
            var diff2 = Math.Abs(d - value2);

            if (diff1.IsLessThan(diff2))
            {
                return value1;
            }
            else
            {
                return value2;
            }
        }
        public static int GetNearerValue(this int d, int value1, int value2)
        {
            var diff1 = Math.Abs(d - value1);
            var diff2 = Math.Abs(d - value2);

            if (diff1 < diff2)
            {
                return value1;
            }
            else
            {
                return value2;
            }
        }

        public static bool IsNearerValueFirst(this double d, double value1, double value2)
        {
            var isNearerValueFirst = false;
            var diff1 = Math.Abs(d - value1);
            var diff2 = Math.Abs(d - value2);

            if (diff1.IsLessThan(diff2))
            {
                isNearerValueFirst = true;
            }
            return isNearerValueFirst;
        }


    }
}