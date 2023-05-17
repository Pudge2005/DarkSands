using System.Collections;
using System.Collections.Generic;

namespace DevourDev.Utility
{
    public static class BigNumConverter
    {
        private const string _thousandPostfix = "k";
        private const string _millionPostfix = "mills";
        private const string _billionPostfix = "bills";
        private const string _trillionPostfix = "trills";
        private const string _quadrillionPostfix = "quads";

        private const double _thousandNum = 1_000d;
        private const double _millionNum = 1_000_000d;
        private const double _billionNum = 1_000_000_000d;
        private const double _trillionNum = 1_000_000_000_000d;
        private const double _quadrillionNum = 1_000_000_000_000_000d;


        public static string ConvertBigOrLittleNum(double num)
        {
            if (IsBigNum(num))
                return Convert(num);

            return num.ToString("F3");
        }

        public static string ConvertBigOrLittleNum(decimal num)
        {
            if (IsBigNum(num))
                return Convert(num);

            return num.ToString("F3");
        }

        public static bool IsBigNum(double num)
        {
            return num > _thousandNum;
        }

        public static bool IsBigNum(decimal num)
        {
            return (double)num > _thousandNum;
        }

        public static string Convert(decimal num)
        {
            return Convert((double)num);
        }

        public static string Convert(double num)
        {
            return num switch
            {
                > _quadrillionNum => FormatInternal(num / _quadrillionNum, _quadrillionPostfix),
                > _trillionNum => FormatInternal(num / _trillionNum, _trillionPostfix),
                > _billionNum => FormatInternal(num / _billionNum, _billionPostfix),
                > _millionNum => FormatInternal(num / _millionNum, _millionPostfix),
                > _thousandNum => FormatInternal(num / _thousandNum, _thousandPostfix),
                _ => FormatInternal(num),
            };
        }


        private static string FormatInternal(double num)
        {
            return num.ToString("N0");
        }

        private static string FormatInternal(double num, in string postfix)
        {
            return $"{num:N3}{postfix}";
        }
    }
}
