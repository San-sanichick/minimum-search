using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minimum.Methods
{
    internal class FibonacciSearch : AbstractSearch
    {
        private double tau = (Math.Sqrt(5) - 1) / 2;

        public FibonacciSearch(double a, double b, double eps, Func<double, double> f) : base(a, b, eps, 0, f)
        {
        }

        private double getFibNumber(int n)
        {
            return Math.Round(Math.Pow(1 / tau, n + 1) / Math.Sqrt(5));
        }

        public override (double, double, int) Search()
        {
            double result = 0;
            double resultFuncValue = 0;
            int actualIterations = 0;

            double ar   = a, br   = b;
            double fxr1 = 0, fxr2 = 0;
            double xr1  = 0, xr2  = 0;

            // step 1
            for (int r = 1; r <= N - 1; r++)
            {
                xr1 = ar + (br - ar) * (getFibNumber(N - 1 - r) / getFibNumber(N + 1 - r));
                xr2 = ar + (br - ar) * (getFibNumber(N - r)     / getFibNumber(N + 1 - r));

                if (r == N - 1) break;

                fxr1 = f(xr1);
                fxr2 = f(xr2);

                if (fxr1 < fxr2)
                {
                    br = xr2;
                } else if (fxr1 >= fxr2)
                {
                    ar = xr1;
                }

                actualIterations++;
            }

            Debug.WriteLine($"xr1: {xr1}, xr2: {xr2}, equal: {xr1 == xr2}");
            Debug.WriteLine($"delta should be less than: {br - ar}");

            // step 2
            double xN    = xr1 + new Random().NextDouble() * ((br - ar) / 10);
            double xNm1  = xr1;
            double fxN   = f(xN);
            double fxNm1 = f(xNm1);

            if (fxN > fxNm1)
            {
                ar = xN;
            } else if (fxN < fxNm1)
            {
                br = xN;
            }

            result          = (ar + br) / 2;
            resultFuncValue = f(result);

            return (result, resultFuncValue, actualIterations);
        }

        protected override double CalcNumberOfIterations(double a, double b, int l, double eps)
        {
            return Math.Log((Math.Sqrt(5) * (b - a)) / eps, 1 / tau) - 1;
        }
    }
}
