using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhysicsLabsComplex
{
    public partial class Menu : Form
    {
        bool closing = false;

        public Menu()
        {
            InitializeComponent();
            CursorSetting.SetHandCursor(buttons1);
            CursorSetting.SetHandCursor(buttons2);
            CursorSetting.SetHandCursor(buttons3);
            CursorSetting.SetHandCursor(buttons4);
        }

        private void buttons1_Click(object sender, EventArgs e)
        {
            closing = true;
            this.Close();
            var historyForm = new History();
            historyForm.Show();
        }

        private void buttons2_Click(object sender, EventArgs e)
        {
            closing = true;
            this.Close();
            var filePath = GetHtmlFilePath("Загальне_визначення.html");
            var theoryForm = new Theory(filePath);
            theoryForm.Show();
        }

        private void buttons3_Click(object sender, EventArgs e)
        {
            closing = true;
            this.Close();
            var listOfLabsForm = new ListOfLabs();
            listOfLabsForm.Show();
        }

        private void buttons4_Click(object sender, EventArgs e)
        {
            closing = true;
            this.Close();
            var listOfGamesForm = new ListOfGames();
            listOfGamesForm.Show();
        }

        private string GetHtmlFilePath(string fileName)
        {
            return Path.Combine(Application.StartupPath, "HTML-theory", fileName);
        }

        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!closing) Application.Exit();
        }
    }
}
