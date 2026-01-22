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
        //public bool IsRunning => timer.Enabled;

        private double _uMax;
        private double _iMax;
        private double _xMax;
        private double _r, _c;
        private double _omega;

        private double dt;
        private int utIndex;
        private int itIndex;
        private int uitIndex;

        public ChartAnimator(Series Ut, Series It)
        {
            _ut = Ut;
            _it = It;

            timer = new Timer();
            timer.Interval = 20;
            timer.Tick += animTimerTick;
        }

        public void ConfigureChargeDischarge(double step, double xMax, double uMax, double r, double c)
        {
            dt = step;
            _xMax = xMax;
            _uMax = uMax;
            _r = r;
            _c = c;
        }

        public void Configure(double step, double xMax, double uMax, double iMax, double omega)
        {
            dt = step;
            _xMax = xMax;
            _uMax = uMax;
            _iMax = iMax;
            _omega = omega;
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

            _ut?.Points.Clear();
            _it?.Points.Clear();

            timer.Start();
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

        }

        private void AnimateDischarge()
        {

        }

        private void AnimateChargeDischarge()
        {

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

                double uValue = _uMax * Math.Sin(_omega * t - (Math.PI / 2));

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

                double uValue = _uMax * Math.Sin(_omega * t);

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

                double iValue = _iMax * Math.Sin(_omega * t);

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

                double iValue = _iMax * Math.Sin(_omega * t - (Math.PI / 2));

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

                double uValue = _uMax * Math.Sin(_omega * t - (Math.PI / 2));
                double iValue = _iMax * Math.Sin(_omega * t);

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

                double uValue = _uMax * Math.Sin(_omega * t);
                double iValue = _iMax * Math.Sin(_omega * t - (Math.PI / 2));

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
