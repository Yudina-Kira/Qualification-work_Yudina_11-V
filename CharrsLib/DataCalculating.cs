using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartsLib
{
    public static class DataCalculating
    {
        public static double Tau(double r, double c) => r * c;

        public static double PulseDuration(double d, double t) => d * t;

        public static double UCharged(double u) => u * (1 - Math.Exp(-5));

        public static double UDischarged(double u) => u * Math.Exp(-5);

        public static double Charge(double u, double t, double tau) => u * (1 - Math.Exp(-t / tau));

        public static double Discharge(double u, double t, double tau) => u * Math.Exp(-t / tau);

        public static double UtRC(double u, double omega, double t, double phi) => u * Math.Sin(omega * t + phi - (Math.PI / 2));

        public static double ItRC(double i, double omega, double t, double phi) => i * Math.Sin(omega * t + phi);

        public static double UtRL(double u, double omega, double t, double phi) => u * Math.Sin(omega * t + phi);

        public static double ItRL(double i, double omega, double t, double phi) => i * Math.Sin(omega * t + phi - (Math.PI / 2));
    }
}
