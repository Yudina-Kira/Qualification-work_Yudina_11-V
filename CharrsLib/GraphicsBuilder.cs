using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.AxHost;

namespace ChartsLib
{
    public static class GraphicsBuilder
    {
        #region --- Lab 1 ---

        public static void BuildCharge(Series charge, double u, double tau, double xMax, double steps = 250)
        {
            for (int i = 0; i <= steps; i++)
            {
                double t =  i * xMax / steps;
                double value = DataCalculating.Charge(u, t, tau);
                charge.Points.AddXY(t, value);
            }
        }

        public static void BuildDischarge(Series discharge, double u, double tau, double xMax, double steps = 250)
        {
            for (int i = 0; i <= steps; i++)
            {
                double t = i * xMax / steps;
                double uCharged = DataCalculating.UCharged(u);
                double value = DataCalculating.Discharge(uCharged, t, tau);
                discharge.Points.AddXY(t, value);
            }
        }

        public static void BuildChargeDischarge(Series chargeDischarge, double u, double tau, double T, double D, double newXMax, double steps = 250)
        {
            double tCharge = D * T;
            double tDischarge = (1 - D) * T;

            double lastX = 0;
            double lastU = 0;

            while (lastX < newXMax)
            {
                for (int i = 0; i <= steps; i++)
                {
                    double t = (i * tCharge / steps);
                    double value = u - (u - lastU) * Math.Exp(-t / tau);

                    double tTotalCharge = t + lastX;
                    if (tTotalCharge > newXMax) break;

                    chargeDischarge.Points.AddXY(tTotalCharge, value);
                }

                lastU = u - (u - lastU) * Math.Exp(-tCharge / tau);

                for (int i = 0; i <= steps; i++)
                {
                    double t = i * tDischarge / steps;
                    double value = DataCalculating.Discharge(lastU, t, tau);

                    double tTotalDischarge = t + lastX + tCharge;
                    if (tTotalDischarge > newXMax) break;

                    chargeDischarge.Points.AddXY(tTotalDischarge, value);
                }

                lastU = lastU * Math.Exp(-tDischarge / tau);

                lastX += T;
            }
        }

        #endregion

        #region --- Lab 2 ---

        public static void BuildUtRC(Series Ut, double u0, double omega, double xMax, double phi = 0, double steps = 1000)
        {
            for (int i = 0; i < steps; i++)
            {
                double t = i * xMax / steps;
                double value = DataCalculating.UtRC(u0, omega, t, phi);
                Ut.Points.AddXY(t, value);
            }
        }

        public static void BuildItRC(Series It, double i0, double omega, double xMax, double phi = 0, double steps = 1000)
        {
            for (int i = 0; i < steps; i++)
            {
                double t = i * xMax / steps;
                double value = DataCalculating.ItRC(i0, omega, t, phi);
                It.Points.AddXY(t, value);
            }
        }

        #endregion

        #region --- Lab 3 ---

        public static void BuildUtRL(Series Ut, double u0, double omega, double xMax, double phi = 0, double steps = 1000)
        {
            for (int i = 0; i < steps; i++)
            {
                double t = i * xMax / steps;
                double value = DataCalculating.UtRL(u0, omega, t, phi);
                Ut.Points.AddXY(t, value);
            }
        }

        public static void BuildItRL(Series It, double i0, double omega, double xMax, double phi = 0, double steps = 1000)
        {
            for (int i = 0; i < steps; i++)
            {
                double t = i * xMax / steps;
                double value = DataCalculating.ItRL(i0, omega, t, phi);
                It.Points.AddXY(t, value);
            }
        }

        #endregion

        public static void BuildExperiment(Series series, ushort[] ch)
        {
            for (int i = 0; i < ch.Length; i++)
            {
                series.Points.AddXY(i, ch[i]);
            }
        }

        public static void BuildExperiment(Series series1, ushort[] ch1, Series series2, ushort[] ch2)
        {
            for (int i = 0; i < ch1.Length && i < ch2.Length; i++)
            {
                series1.Points.AddXY(i, ch1[i]);
                series2.Points.AddXY(i, ch2[i]);
            }
        }
    }
}
