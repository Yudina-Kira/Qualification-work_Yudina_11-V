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
    public partial class LabsInstructions : Form
    {
        private readonly string labInstructionFilePath;

        public LabsInstructions(string filePath)
        {
            InitializeComponent();
            labInstructionFilePath = filePath;
        }

        private void LabsInstructions_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = true; 
            webBrowser1.Navigate(labInstructionFilePath);
        }

        private void завантажитиToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
