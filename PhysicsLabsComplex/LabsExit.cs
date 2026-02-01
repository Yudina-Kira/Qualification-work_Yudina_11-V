using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace PhysicsLabsComplex
{
    public partial class LabsExit : Form
    {
        public static bool closing = false;

        private Form parentForm;

        public LabsExit(Form parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;

            //setting cursor on active elements
            {
                CursorSetting.SetHandCursor(buttons1);
                CursorSetting.SetHandCursor(buttons2);
            }
        }

        private void buttons1_Click(object sender, EventArgs e)
        {
            closing = true;
            this.Close();
            var listOfLabsForm = new ListOfLabs();
            listOfLabsForm.Show();
        }

        private void buttons2_Click(object sender, EventArgs e)
        {
            closing = true;
            this.Close();
            var menuForm = new Menu();
            menuForm.Show();
        }

        private void LabsExit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!closing && parentForm != null && !parentForm.IsDisposed)
            {
                //parentForm.Enabled = true;
                parentForm.Focus();
            }
        }
    }
}
