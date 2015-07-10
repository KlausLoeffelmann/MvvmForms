using System;

namespace ActiveDevelop.MvvmBaseLib.FormulaEvaluator
{
    public partial class FormulaEvaluator
    {

        public static double Addition(double[] Args)
        {
            return Args[0] + Args[1];
        }

        public static double Substraction(double[] Args)
        {
            return Args[0] - Args[1];
        }

        public static double Multiplication(double[] Args)
        {
            return Args[0] * Args[1];
        }

        public static double Division(double[] Args)
        {
            return Args[0] / Args[1];
        }

        public static double Remainder(double[] Args)
        {
            return Convert.ToDouble(decimal.Remainder(new decimal(Args[0]), new decimal(Args[1])));
        }

        public static double Power(double[] Args)
        {
            return (System.Math.Pow(Args[0], Args[1]));
        }

        public static double Sin(double[] Args)
        {
            return Math.Sin(Args[0]);
        }

        public static double Cos(double[] Args)
        {
            return Math.Cos(Args[0]);
        }

        public static double Tan(double[] Args)
        {
            return Math.Tan(Args[0]);
        }

        public static double Sqrt(double[] Args)
        {
            return Math.Sqrt(Args[0]);
        }

        public static double PI(double[] Args)
        {
            return Math.PI;
        }

        public static double Tanh(double[] Args)
        {
            return Math.Tanh(Args[0]);
        }

        public static double LogDec(double[] Args)
        {
            return Math.Log10(Args[0]);
        }

        public static double XVar(double[] Args)
        {
            return XVariable;
        }

        public static double YVar(double[] Args)
        {
            return YVariable;
        }

        public static double ZVar(double[] Args)
        {
            return ZVariable;
        }

        public static double Max(double[] Args)
        {

            double retDouble = 0;

            if (Args.Length == 0)
            {
                return 0;
            }
            else
            {
                retDouble = Args[0];
                foreach (double locDouble in Args)
                {
                    if (retDouble < locDouble)
                    {
                        retDouble = locDouble;
                    }
                }
            }
            return retDouble;

        }

        public static double Min(double[] Args)
        {

            double retDouble = 0;

            if (Args.Length == 0)
            {
                return 0;
            }
            else
            {
                retDouble = Args[0];
                foreach (double locDouble in Args)
                {
                    if (retDouble > locDouble)
                    {
                        retDouble = locDouble;
                    }
                }
            }
            return retDouble;

        }

        public static double XVariable
        {
            get
            {
                return myXVariable;
            }
            set
            {
                myXVariable = value;
            }
        }

        public static double YVariable
        {
            get
            {
                return myYVariable;
            }
            set
            {
                myYVariable = value;
            }
        }

        public static double ZVariable
        {
            get
            {
                return myZVariable;
            }
            set
            {
                myZVariable = value;
            }
        }
    }
}