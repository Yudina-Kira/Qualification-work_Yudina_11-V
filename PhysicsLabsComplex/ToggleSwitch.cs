using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace PhysicsLabsComplex
{
    public class ToggleSwitch : Control
    {
        #region -- Змінні --

        Rectangle rect;

        int TogglePosX_ON;
        int TogglePosX_OFF;
        bool isAnimating = false;

        AnimationToggle ToggleAnim;

        #endregion

        #region -- Властивості --

        public bool Checked { get; set; } = false;

        public Color BackColorON { get; set; } = Color.FromArgb(87, 191, 100);

        #endregion

        public ToggleSwitch()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            DoubleBuffered = true;

            Size = new Size(40, 15);

            Font = new Font("Verdana", 9F, FontStyle.Regular);
            BackColor = Color.FromArgb(152, 167, 174);

            rect = new Rectangle(1, 1, Width - 3, Height - 3);
            TogglePosX_OFF = rect.X;
            TogglePosX_ON = rect.Width - rect.Height;

            ToggleAnim = new AnimationToggle();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            ToggleAnim.Value = Checked == true ? TogglePosX_ON : TogglePosX_OFF;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            rect = new Rectangle(1, 1, Width - 3, Height - 3);
            TogglePosX_OFF = rect.X;
            TogglePosX_ON = rect.Width - rect.Height;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics gr = e.Graphics;
            gr.SmoothingMode = SmoothingMode.HighQuality;
            gr.Clear(Parent.BackColor);

            Pen TSPen = new Pen(Color.DarkGray, 3);
            Pen TSPenToggle = new Pen(Color.DarkGray, 3);

            GraphicsPath rectPath = Drawer.RoundedRectangle(rect, rect.Height);

            Rectangle rectToggle = new Rectangle((int)ToggleAnim.Value, rect.Y, rect.Height, rect.Height);

            gr.DrawPath(TSPen, rectPath);

            if (Checked)
            {
                if (AnimatorToggle.IsWork == false)
                {
                    rectToggle.Location = new Point(TogglePosX_ON, rect.Y);
                }
                gr.FillPath(new SolidBrush(BackColorON), rectPath);
            }
            else
            {
                if (AnimatorToggle.IsWork == false)
                {
                    rectToggle.Location = new Point(TogglePosX_OFF, rect.Y);
                }
                gr.FillPath(new SolidBrush(BackColor), rectPath);
            }

            gr.DrawEllipse(TSPenToggle, rectToggle);
            gr.FillEllipse(new SolidBrush(Color.White), rectToggle);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (isAnimating) return;

            base.OnMouseDown(e);

            SwitchToggle();
        }

        private void SwitchToggle()
        {
            isAnimating = true;

            if (Checked == true)
            {
                ToggleAnim = new AnimationToggle("Toggle_" + Handle, Invalidate, ToggleAnim.Value, TogglePosX_OFF);
            }
            else
            {
                ToggleAnim = new AnimationToggle("Toggle_" + Handle, Invalidate, ToggleAnim.Value, TogglePosX_ON);
            }

            Checked = !Checked;

            AnimatorToggle.Request(ToggleAnim, true);

            ToggleAnim.OnAnimationCompleted += () =>
            {
                isAnimating = false;
            };
        }

        public void SetChecked(bool value)
        {
            if (Checked == value) return;
            if (isAnimating) return;

            SwitchToggle();
        }
    }
}
