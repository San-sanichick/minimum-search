using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minimum.Methods
{
    internal class GoldenRatioSearch : AbstractSearch
    {
        private double tau = (Math.Sqrt(5) - 1) / 2;

        public GoldenRatioSearch(double a, double b, double eps, Func<double, double> f) : base(a, b, eps, 0, f)
        {
        }

        public override (double, double, int) Search()
        {
            double result = 0;
            double resultFuncValue = 0;

            double rangeStart = a, rangeEnd = b;

            double xr1 = 0, xr2 = 0;

            double fxr1 = 0, fxr2 = 0;

            int actualIterations = 0;

            for (int r = 0; r < N; r++)
            {
                xr1 = rangeEnd   - (rangeEnd - rangeStart) * tau;
                xr2 = rangeStart + (rangeEnd - rangeStart) * tau;

                fxr1 = f(xr1);
                fxr2 = f(xr2);

                if (fxr1 < fxr2)
                {
                    rangeEnd = xr2;
                } else if (fxr1 > fxr2)
                {
                    rangeStart = xr1;
                }

                if ((rangeEnd - rangeStart) < eps)
                {
                    result = (rangeStart + rangeEnd) / 2;
                    resultFuncValue = f(result);
                    actualIterations = r;
                    break;
                }
            }

            return (result, resultFuncValue, actualIterations);
        }

        protected override double CalcNumberOfIterations(double a, double b, int l, double eps)
        {
            return Math.Log(eps / (b - a), tau);
        }
    }
}
