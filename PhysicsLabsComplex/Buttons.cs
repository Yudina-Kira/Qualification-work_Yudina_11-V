using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace PhysicsLabsComplex
{
    public class Buttons : Control
    {
        #region -- Властивості --
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Текст, який з'являється при наведенні курсора")]

        public string TextHover { get; set; }

        private bool roundingEnable = false;
        [Description("Вкл/Викл закруглення об'єкта")]

        public bool RoundingEnable
        {
            get => roundingEnable;
            set
            {
                roundingEnable = value;
                Refresh();
            }
        }

        private int roundingPercent = 50;
        [Browsable(true)]
        [Category("Appearance")]
        [DisplayName("Rounding [%]")]
        [DefaultValue(50)]
        [Description("Показує радіус закруглення об'єкта у процентному співвідношенні")]

        public int Rounding
        {
            get => roundingPercent;
            set
            {
                if (value >= 0 && value <= 100)
                {
                    roundingPercent = value;

                    Refresh();
                }
            }
        }

        #endregion

        #region -- Змінні --

        private StringFormat SF = new StringFormat();

        private bool MouseEntered = false;
        private bool MousePressed = false;

        AnimationButton CurtainButtonAnim = new AnimationButton();
        AnimationButton RippleButtonAnim = new AnimationButton();

        Point ClickLocation = new Point();

        #endregion

        public Buttons()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            DoubleBuffered = true;

            Size = new Size(200, 100);

            BackColor = Color.FromArgb(87, 181, 195);
            ForeColor = Color.White;

            SF.Alignment = StringAlignment.Center;
            SF.LineAlignment = StringAlignment.Center;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics gr = e.Graphics;
            gr.SmoothingMode = SmoothingMode.HighQuality;

            gr.Clear(Parent.BackColor);

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            Rectangle rectCurtain = new Rectangle(0, 0, (int)CurtainButtonAnim.Value, Height - 1);
            Rectangle rectRipple = new Rectangle(
                ClickLocation.X - (int)RippleButtonAnim.Value / 2,
                ClickLocation.Y - (int)RippleButtonAnim.Value / 2,
                (int)RippleButtonAnim.Value,
                (int)RippleButtonAnim.Value
                );

            // Закругление
            float roundingValue = 0.1F;
            if (RoundingEnable && roundingPercent > 0)
            {
                roundingValue = Height / 100F * roundingPercent;
            }
            GraphicsPath rectPath = Drawer.RoundedRectangle(rect, roundingValue);

            //основний прямокутник
            gr.DrawPath(new Pen(BackColor), rectPath);
            gr.FillPath(new SolidBrush(BackColor), rectPath);

            gr.SetClip(rectPath);

            //шторка
            gr.DrawRectangle(new Pen(Color.FromArgb(60, Color.White)), rectCurtain);
            gr.FillRectangle(new SolidBrush(Color.FromArgb(60, Color.White)), rectCurtain);

            // Ripple effect - Хвиля
            if (RippleButtonAnim.Value > 0 && RippleButtonAnim.Value < RippleButtonAnim.TargetValue)
            {
                gr.DrawEllipse(new Pen(Color.FromArgb(30, Color.Black)), rectRipple);
                gr.FillEllipse(new SolidBrush(Color.FromArgb(30, Color.Black)), rectRipple);
            }
            else if (RippleButtonAnim.Value == RippleButtonAnim.TargetValue) {
                RippleButtonAnim.Value = 0;
            }

                gr.DrawString(Text, Font, new SolidBrush(ForeColor), rect, SF);
        }

        private void ButtonCurtainAction()
        {
            if (MouseEntered)
            {
                CurtainButtonAnim = new AnimationButton("ButtonCurtain_" + Handle, Invalidate, CurtainButtonAnim.Value, Width - 1);
            }
            else
            {
                CurtainButtonAnim = new AnimationButton("ButtonCurtain_" + Handle, Invalidate, CurtainButtonAnim.Value, 0);
            }

            AnimatorButton.Request(CurtainButtonAnim, true);
        }

        private void ButtonRippleAction()
        {
            RippleButtonAnim = new AnimationButton("ButtonRipple_" + Handle, Invalidate, 0, Width);

            RippleButtonAnim.StepDivider = 15;
            AnimatorButton.Request(RippleButtonAnim, true);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            MouseEntered = true;

            ButtonCurtainAction();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            MouseEntered = false;

            ButtonCurtainAction();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            MousePressed = true;

            CurtainButtonAnim.Value = CurtainButtonAnim.TargetValue;

            ClickLocation = e.Location;

            ButtonRippleAction();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            MousePressed = false;

            Invalidate();
        }
    }
}
