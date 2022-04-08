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
            double[] segments   = new double[L];
            double[] funcValues = new double[L];

            double rangeStart = a, rangeEnd = b;

            double result = 0;
            double resultFuncValue = 0;

            int actualIterations = 0;

            for (int i = 0; i < N; i++)
            {
                double length = (rangeEnd - rangeStart) / L;

                // define segments
                for (int j = 0; j < L; j++)
                {
                    segments[j] = rangeStart + j * length;
                }

                // find function values in the ends of those segments
                for (int j = 0; j < L; j++)
                {
                    funcValues[j] = f(segments[j]);
                }

                double minVal = Double.MaxValue;
                int kIndex = 0;

                for (int j = 1; j <= L - 1; j++)
                {
                    if (minVal > funcValues[j])
                    {
                        minVal = funcValues[j];
                        kIndex = j;
                    }
                }

                rangeStart = segments[kIndex - 1];
                rangeEnd   = segments[kIndex + 1];

                if (minVal > funcValues[0])
                {
                    rangeStart = segments[0];
                    rangeEnd   = segments[1];
                }

                if (minVal > funcValues[L - 1])
                {
                    rangeStart = segments[L - 1];
                    rangeEnd   = segments[L - 2];
                }

                if (Math.Abs(rangeEnd - rangeStart) <= eps)
                {
                    result = (rangeStart + rangeEnd) / 2;
                    resultFuncValue = minVal;
                    actualIterations = i;
                    break;
                }
            }
            Debug.WriteLine($"{rangeEnd - rangeStart}\n{(rangeEnd - rangeStart) < eps}");

            return (result, resultFuncValue, actualIterations);
        }
    }
}
