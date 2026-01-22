using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhysicsLabsComplex
{
    public partial class ImageView : Form
    {
        public ImageView(Image img)
        {
            InitializeComponent();
            pictureBox1.Image = img;

            RegulateFormSize(img);
        }

        private void RegulateFormSize(Image img)
        {
            int scrW = Screen.PrimaryScreen.WorkingArea.Width;
            int scrH = Screen.PrimaryScreen.WorkingArea.Height;

            float imgW = img.Width;
            float imgH = img.Height;
            float ratio = imgW / imgH;

            int maxW = (int)(scrW * 0.70);
            int maxH = (int)(scrH * 0.90);

            int finalW, finalH;

            finalW = maxW;
            finalH = (int)(finalW / ratio);

            if (finalH > maxH)
            {
                finalH = maxH;
                finalW = (int)(finalH * ratio);
            }

            this.Size = new Size(finalW, finalH + 35);
        }
    }
}
