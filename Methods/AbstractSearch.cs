using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minimum.Methods
{

    abstract class AbstractSearch
    {
        protected double a = 0;
        protected double b = 0;
        protected double eps = 0.001;
        protected int L = 1;
        public int N { get; set; } = 2;
        protected Func<double, double> f;

        public AbstractSearch(double a, double b, double eps, int L, Func<double, double> f)
        {
            this.a = a;
            this.b = b;
            this.eps = eps;
            this.L = L;
            N = (int)Math.Ceiling(CalcNumberOfIterations(a, b, L, eps));
            this.f = f;
        }

        protected abstract double CalcNumberOfIterations(double a, double b, int l, double eps);

        public abstract (double x, double y, int N) Search();
    }
}
