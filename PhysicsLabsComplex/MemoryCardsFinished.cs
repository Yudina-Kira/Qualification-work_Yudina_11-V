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
    public partial class MemoryCardsFinished : Form
    {
        public static bool closing = false;

        public bool RestartCardsGame { get; private set; } = false;

        public MemoryCardsFinished(int resultCount, int time)
        {
            InitializeComponent();
            label6.Text = resultCount.ToString();
            label5.Text = $"{time / 60}:{(time % 60):00}";

            //setting cursor on active elements
            {
                CursorSetting.SetHandCursor(buttons1);
                CursorSetting.SetHandCursor(buttons2);
            }
        }

        private void buttons1_Click(object sender, EventArgs e)
        {
            RestartCardsGame = true;
            this.Close();
        }

        private void buttons2_Click(object sender, EventArgs e)
        {
            closing = true;
            this.Close();
            var listOfGamesForm = new ListOfGames();
            listOfGamesForm.Show();
        }
    }
}
