using System;
using System.Collections.Generic;
using System.Text;

namespace ActiveDevelop.Generic
{
    using System;

    public static class MathExtension
    {

        public static bool IsApproximatelyEqual(this double FirstValue, double SecondValue, double Precision = 0.00000001)
        {
            return Math.Abs(FirstValue - SecondValue) < Precision;
        }

        public static bool IsApproximatelyEqual(this float FirstValue, float SecondValue, float Precision = 0.00000001F)
        {
            return Math.Abs(FirstValue - SecondValue) < Precision;
        }

        public static bool IsApproximatelyEqual(this double[] FirstValueSeries, double[] SecondValueSeries, double Precision = 0.00000001)
        {
            var indexer = 0;
            foreach (var doubleValue in FirstValueSeries)
            {
                if (!(Math.Abs(doubleValue - SecondValueSeries[indexer]) < Precision))
                {
                    return false;
                }
                indexer += 1;
            }
            return true;
        }

        public static bool IsApproximatelyEqual(this float[] FirstValueSeries, float[] SecondValueSeries, float Precision = 0.00000001F)
        {
            var indexer = 0;
            foreach (var singleValue in FirstValueSeries)
            {
                if (!(Math.Abs(singleValue - SecondValueSeries[indexer]) < Precision))
                {
                    return false;
                }
                indexer += 1;
            }
            return true;
        }

    }
}
