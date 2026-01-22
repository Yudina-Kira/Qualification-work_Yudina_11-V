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
    public partial class ListOfLabs : Form
    {
        bool closing = false;

        public ListOfLabs()
        {
            InitializeComponent();
            CursorSetting.SetHandCursor(buttons1);
            CursorSetting.SetHandCursor(buttons2);
            CursorSetting.SetHandCursor(buttons3);
        }

        private void buttons1_Click(object sender, EventArgs e)
        {
            closing = true;
            this.Close();
            var lab1Form = new Lab_1();
            lab1Form.Show();
        }

        private void buttons2_Click(object sender, EventArgs e)
        {
            closing = true;
            this.Close();
            var lab2Form = new Lab_2();
            lab2Form.Show();
        }

        private void buttons3_Click(object sender, EventArgs e)
        {
            closing = true;
            this.Close();
            var lab3Form = new Lab_3();
            lab3Form.Show();
        }

        private void ListOfLabs_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!closing)
            {
                var menuForm = new Menu();
                menuForm.Show();
            }
        }
    }
}
