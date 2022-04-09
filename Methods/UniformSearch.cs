using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minimum.Methods
{
    internal class UniformSearch : AbstractSearch
    {
        public UniformSearch(double a, double b, double eps, int L, Func<double, double> f) : base(a, b, eps, L, f)
        {
        }

        protected override double CalcNumberOfIterations(double a, double b, int L, double eps)
        {
            return Math.Log(eps / (b - a), 2 / (double)L);
        }

        public override (double, double, int) Search()
        {
            double[] xValues = new double[L + 1];
            double[] fValues = new double[L + 1];

            double ar = a, br = b;

            double result = 0;
            double resultFuncValue = 0;

            int actualIterations = 0;

            for (int i = 0; i < N; i++)
            {
                double length = (br - ar) / L;

                // define segments
                for (int j = 0; j <= L; j++)
                {
                    xValues[j] = ar + j * length;
                }


                // find function values in the ends of those segments
                for (int j = 0; j <= L; j++)
                {
                    fValues[j] = f(xValues[j]);
                }


                double minVal = double.MaxValue;
                int k = 0;

                for (int j = 1; j <= L - 1; j++)
                {
                    if (minVal > fValues[j])
                    {
                        minVal = fValues[j];
                        k = j;
                    }
                }

                ar = xValues[k - 1];
                br = xValues[k + 1];

                if (minVal > fValues[0])
                {
                    ar = xValues[0];
                    br = xValues[1];
                }

                if (minVal > fValues[L])
                {
                    ar = xValues[L - 1];
                    br = xValues[L];
                }

                if (Math.Abs(br - ar) <= eps)
                {
                    result = (ar + br) / 2;
                    resultFuncValue = minVal;
                    actualIterations = i;
                    break;
                }
            }

            return (result, resultFuncValue, actualIterations);
        }
    }
}
