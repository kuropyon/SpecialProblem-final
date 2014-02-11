using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPprotoype
{
    class EquationList
    {
        public double minDomain;
        public double maxDomain;
        public double[] derivative;
        public double[] equation;

        public EquationList(int arrayLength,double min,double max)
        {
            this.equation = new double[arrayLength];
            this.minDomain = min;
            this.maxDomain = max;
        }

   
    }
}
