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
    public partial class ListOfGames : Form
    {
        bool closing = false;

        public ListOfGames()
        {
            InitializeComponent();

            //setting cursor on active elements
            {
                CursorSetting.SetHandCursor(buttons1);
                CursorSetting.SetHandCursor(buttons2);
            }
        }

        private void ListOfGames_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!closing)
            {
                var menuForm = new Menu();
                menuForm.Show();
            }
        }

        private void buttons1_Click(object sender, EventArgs e)
        {
            closing = true;
            this.Close();
            var crosswordsForm = new Crosswords();
            crosswordsForm.Show();
        }

        private void buttons2_Click(object sender, EventArgs e)
        {
            closing = true;
            this.Close();
            var memoryCardsForm = new MemoryCards();
            memoryCardsForm.Show();
        }
    }
}
