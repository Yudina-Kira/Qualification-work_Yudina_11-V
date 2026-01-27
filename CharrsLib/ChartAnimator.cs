using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartsLib
{
    public class ChartAnimator
    {
        private readonly Series _ut;
        private readonly Series _it;

        private LabMode labMode;
        private AnimationMode animMode;

        private readonly Timer timer;
        public event EventHandler AnimationFinished;

        private double _uMax;
        private double _iMax;
        private double _xMax;
        private double _tau;
        private double _omega;
        private double _D;
        private double _T;
        private double _phi;

        private double dt;
        private int utIndex;
        private int itIndex;
        private int uitIndex;

        private double lastU;
        private bool wasCharging;

        public ChartAnimator(Series Ut, Series It)
        {
            _ut = Ut;
            _it = It;

            timer = new Timer();
            timer.Interval = 20;
            timer.Tick += animTimerTick;
        }

        public void ConfigureChargeDischarge(double step, double xMax, double uMax, double tau, double D, double T)
        {
            dt = step;
            _xMax = xMax;
            _uMax = uMax;
            _tau = tau;
            _D = D;
            _T = T;
        }

        public void Configure(double step, double xMax, double uMax, double iMax, double omega, double phi)
        {
            dt = step;
            _xMax = xMax;
            _uMax = uMax;
            _iMax = iMax;
            _omega = omega;
            _phi = phi;
        }

        public enum LabMode
        {
            ChargeDischarge,
            RC,
            RL
        }

        public enum AnimationMode
        {
            None,
            Charge,
            Discharge,
            ChargeDischarge,
            Ut,
            It,
            UtIt
        }

        public void StartAnimation(LabMode lMode, AnimationMode aMode)
        {
            StopAnimation();

            labMode = lMode;
            animMode = aMode;

            utIndex = itIndex = uitIndex = 0;

            lastU = 0;
            wasCharging = true;

            _ut?.Points.Clear();
            _it?.Points.Clear();

            SetTickInterval(lMode);
            timer.Start();
        }

        private void SetTickInterval(LabMode l)
        {
            switch (l)
            {
                case LabMode.ChargeDischarge:
                    timer.Interval = 20;
                    break;

                case LabMode.RC:
                case LabMode.RL:
                    timer.Interval = 1;
                    break;
            }
        }

        public void StopAnimation() => StopInternal();

        private void StopInternal()
        {
            timer.Stop();
            AnimationFinished?.Invoke(this, EventArgs.Empty);
        }

        private void animTimerTick(object sender, EventArgs e)
        {
            switch (animMode)
            {
                case AnimationMode.Charge:
                    AnimateCharge();
                    break;

                case AnimationMode.Discharge:
                    AnimateDischarge();
                    break;

                case AnimationMode.ChargeDischarge:
                    AnimateChargeDischarge();
                    break;

                case AnimationMode.Ut:
                    AnimateUt();
                    break;

                case AnimationMode.It:
                    AnimateIt();
                    break;

                case AnimationMode.UtIt:
                    AnimateUtIt();
                    break;
            }
        }

        private void AnimateCharge()
        {
            double t = utIndex * dt;
            if (t > _xMax)
            {
                StopInternal();
                return;
            }

            double uValue = DataCalculating.Charge(_uMax, t, _tau);

            _ut.Points.AddXY(t, uValue);

            utIndex++;
        }

        private void AnimateDischarge()
        {
            double t = utIndex * dt;
            if (t > _xMax)
            {
                StopInternal();
                return;
            }

            double uValue = DataCalculating.Discharge(_uMax, t, _tau);

            _ut.Points.AddXY(t, uValue);

            utIndex++;
        }

        private void AnimateChargeDischarge()
        {
            double tGlobal = utIndex * dt;
            if (tGlobal > _xMax)
            {
                StopInternal();
                return;
            }

            double tInPeriod = tGlobal % _T;
            double uValue;

            if (tInPeriod < _D * _T)
            {
                if (!wasCharging)
                {
                    lastU = _ut.Points.Count > 0 ? _ut.Points[_ut.Points.Count - 1].YValues[0] : 0;
                    wasCharging = true;
                }

                double tLocal = tInPeriod;
                uValue = _uMax - (_uMax - lastU) * Math.Exp(-tLocal / _tau);
            }
            else
            {
                if (wasCharging)
                {
                    lastU = _ut.Points[_ut.Points.Count - 1].YValues[0];
                    wasCharging = false;
                }

                double tLocal = tInPeriod - _D * _T;
                uValue = lastU * Math.Exp(-tLocal / _tau);
            }

            _ut.Points.AddXY(tGlobal, uValue);
            utIndex++;
        }

        private void AnimateUt()
        {
            if (labMode == LabMode.RC)
            {
                double t = utIndex * dt;
                if (t > _xMax)
                {
                    StopInternal();
                    return;
                }

                double uValue = DataCalculating.UtRC(_uMax, _omega, t, _phi, 0);

                _ut.Points.AddXY(t, uValue);

                utIndex++;
            }
            else if (labMode == LabMode.RL)
            {
                double t = utIndex * dt;
                if (t > _xMax)
                {
                    StopInternal();
                    return;
                }

                double uValue = DataCalculating.UtRL(_uMax, _omega, t, 0);

                _ut.Points.AddXY(t, uValue);

                utIndex++;
            }
            else
            {
                StopInternal();
                return;
            }
        }

        private void AnimateIt()
        {
            if (labMode == LabMode.RC)
            {
                double t = itIndex * dt;
                if (t > _xMax)
                {
                    StopInternal();
                    return;
                }

                double iValue = DataCalculating.ItRC(_iMax, _omega, t, 0);

                _it.Points.AddXY(t, iValue);

                itIndex++;
            }
            else if (labMode == LabMode.RL)
            {
                double t = itIndex * dt;
                if (t > _xMax)
                {
                    StopInternal();
                    return;
                }

                double iValue = DataCalculating.ItRL(_iMax, _omega, t, _phi, 0);

                _it.Points.AddXY(t, iValue);

                itIndex++;
            }
            else
            {
                StopInternal();
                return;
            }
        }

        private void AnimateUtIt()
        {
            if (labMode == LabMode.RC)
            {
                double t = uitIndex * dt;
                if (t > _xMax)
                {
                    StopInternal();
                    return;
                }

                double uValue = DataCalculating.UtRC(_uMax, _omega, t, _phi, 0);
                double iValue = DataCalculating.ItRC(_iMax, _omega, t, 0);

                _ut.Points.AddXY(t, uValue);
                _it.Points.AddXY(t, iValue);

                uitIndex++;
            }
            else if (labMode == LabMode.RL)
            {
                double t = uitIndex * dt;
                if (t > _xMax)
                {
                    StopInternal();
                    return;
                }

                double uValue = DataCalculating.UtRL(_uMax, _omega, t, 0);
                double iValue = DataCalculating.ItRL(_iMax, _omega, t, _phi, 0);

                _ut.Points.AddXY(t, uValue);
                _it.Points.AddXY(t, iValue);

                uitIndex++;
            }
            else
            {
                StopInternal();
                return;
            }
        }
    }
}
