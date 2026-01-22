using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhysicsLabsComplex
{
    public class Cards : Panel
    {
        #region --- Властивості ---

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

        public Cards()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            DoubleBuffered = true;

            Size = new Size(340, 340);

            BackColor = Color.FromArgb(120, 135, 142);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics gr = e.Graphics;
            gr.SmoothingMode = SmoothingMode.HighQuality;

            gr.Clear(Parent.BackColor);

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

            float roundingValue = 0.1F;
            if (RoundingEnable && roundingPercent > 0)
            {
                roundingValue = Height / 100F * roundingPercent;
            }
            GraphicsPath rectPath = Drawer.RoundedRectangle(rect, roundingValue);

            gr.DrawPath(new Pen(BackColor), rectPath);
            if (BackgroundImage != null)
            {
                gr.SetClip(rectPath);
                gr.DrawImage(BackgroundImage, rect);
            }
            else
            {
                gr.FillPath(new SolidBrush(BackColor), rectPath);
            }
        }
    }
}
