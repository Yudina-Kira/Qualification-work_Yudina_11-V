using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartsLib
{
    public static class UnitsToSI
    {
        public static double MicroToBase(double value) => value * 1e-6;

        public static double MilliToBase(double value) => value * 1e-3;

        public static double KiloToBase(double value) => value * 1000;

        public static double PercentToFraction(double value) => value / 100.0;

        public static double RoundToStep(double value, double step) => Math.Round(value / step) * step;
    }
}
