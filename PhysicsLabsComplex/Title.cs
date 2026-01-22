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
    public partial class Title : Form
    {
        public Title()
        {
            InitializeComponent();
            CursorSetting.SetHandCursor(round1);
            panel1.BackColor = Color.FromArgb(170, 2, 20, 40);
            this.Icon = null;
        }

        private void round1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var menuForm = new Menu();
            menuForm.Show();
        }
    }
}
