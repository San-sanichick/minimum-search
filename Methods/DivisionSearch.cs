using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minimum.Methods
{
    internal class DivisionSearch : AbstractSearch
    {
        public DivisionSearch(double a, double b, double eps, Func<double, double> f) : base(a, b, eps, 0, f)
        {
        }

        public override (double, double, int) Search()
        {
            double result = 0;
            double resultFuncValue = 0;

            double rangeStart = a;
            double rangeEnd   = b;

            double x0 = 0;
            double x01 = 0;
            double x02 = 0;

            double fx01 = 0, fx02 = 0;


            int actualIterations = 0;

            for (int i = 0; i < N; i++)
            {
                // x0
                x0 = (rangeEnd + rangeStart) / 2;

                x01 = (rangeStart + x0) / 2;
                x02 = (x0 + rangeEnd) / 2;

                fx01 = f(x01);
                fx02 = f(x02);

                if (fx01 < fx02)
                {
                    rangeEnd = x0;
                } else if (fx01 >= fx02)
                {
                    rangeStart = x0;
                }

                if ((rangeEnd - rangeStart) <= eps)
                {
                    result = (rangeStart + rangeEnd) / 2;
                    resultFuncValue = f(result);
                    actualIterations = i;
                    break;
                }
            }


            return (result, resultFuncValue, actualIterations);
        }

        protected override double CalcNumberOfIterations(double a, double b, int l, double eps)
        {
            return Math.Log((b - a) / eps, 2);
        }
    }
}
