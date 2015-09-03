using System;

namespace ActiveDevelop.MvvmBaseLib.FormulaEvaluator
{
    public partial class FormulaEvaluator
    {

        internal double Addition(double[] Args)
        {
            return Args[0] + Args[1];
        }

        internal double Substraction(double[] Args)
        {
            return Args[0] - Args[1];
        }

        internal double Multiplication(double[] Args)
        {
            return Args[0] * Args[1];
        }

        internal double Division(double[] Args)
        {
            return Args[0] / Args[1];
        }

        internal double Remainder(double[] Args)
        {
            return Convert.ToDouble(decimal.Remainder(new decimal(Args[0]), new decimal(Args[1])));
        }

        internal double Power(double[] Args)
        {
            return (System.Math.Pow(Args[0], Args[1]));
        }

        internal double Sin(double[] Args)
        {
            return Math.Sin(Args[0]);
        }

        internal double Cos(double[] Args)
        {
            return Math.Cos(Args[0]);
        }

        internal double Tan(double[] Args)
        {
            return Math.Tan(Args[0]);
        }

        internal double Sqrt(double[] Args)
        {
            return Math.Sqrt(Args[0]);
        }

        internal double PI(double[] Args)
        {
            return Math.PI;
        }

        internal double Tanh(double[] Args)
        {
            return Math.Tanh(Args[0]);
        }

        internal double LogDec(double[] Args)
        {
            return Math.Log10(Args[0]);
        }

        internal double XVar(double[] Args)
        {
            return X;
        }

        internal double YVar(double[] Args)
        {
            return Y;
        }

        internal double ZVar(double[] Args)
        {
            return Z;
        }

        public double Max(double[] Args)
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

        public double Min(double[] Args)
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

        public double X
        {
            get
            {
                return myXVariable;
            }
            set
            {
                myXVariable = value;
                IsCalculated = false;
            }
        }

        public double Y
        {
            get
            {
                return myYVariable;
            }
            set
            {
                myYVariable = value;
                IsCalculated = false;
            }
        }

        public double Z
        {
            get
            {
                return myZVariable;
            }
            set
            {
                myZVariable = value;
                IsCalculated = false;
            }
        }
    }
}