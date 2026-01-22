 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PhysicsLabsComplex
{
    public partial class Crosswords : Form
    {
        #region --- Змінні ---

        Clues clueWindow;
        public List<id_cells> idc { get; private set; } = new List<id_cells>();
        public string crosswordFile = Application.StartupPath + "\\Crosswords\\crossword1.txt"; //def

        private CrosswordsRules rulesForm;

        public bool fullClose = false;

        #endregion

        public Crosswords()
        {
            InitializeComponent();
            board.EditingControlShowing += board_EditingControlShowing;
            clueWindow = new Clues(this);
            buildWordList();
        }

        private void Crosswords_Load(object sender, EventArgs e)
        {
            InitializeBoard();

            board.ClearSelection();

            this.StartPosition = FormStartPosition.Manual;
            clueWindow.StartPosition = FormStartPosition.Manual;

            Rectangle screen = Screen.PrimaryScreen.WorkingArea;

            int totalWidth = this.Width + clueWindow.Width + 9;
            int startX = screen.Left + (screen.Width - totalWidth) / 2;
            int startY = screen.Top + (screen.Height - this.Height) / 2;

            this.Location = new Point(startX, startY);
            clueWindow.Location = new Point(this.Location.X + this.Width + 9, this.Location.Y);

            clueWindow.Show();
            clueWindow.clueTable.AutoResizeColumns();
            clueWindow.clueTable.ClearSelection();
        }

        #region --- Setting up gameboard to play ---

        private void buildWordList()
        {
            string line = "";
            using (StreamReader sr = new StreamReader(crosswordFile))
            {
                line = sr.ReadLine();
                while ((line = sr.ReadLine()) != null)
                {
                    string[] l = line.Split('|');
                    idc.Add(new id_cells(Int32.Parse(l[0]), Int32.Parse(l[1]), l[2], l[3], l[4], l[5]));
                    clueWindow.clueTable.Rows.Add(new string[] { l[3], l[2], l[5] });
                }
            }
        }

        private void InitializeBoard()
        {
            board.BackgroundColor = Color.FromArgb(120, 135, 142);
            board.DefaultCellStyle.BackColor = Color.FromArgb(120, 135, 142);

            for (int i = 0; i < 16; i++)
            {
                var column = new DataGridViewTextBoxColumn();
                board.Columns.Add(column);
            }
            for (int i = 0; i < 14; i++)
            {
                board.Rows.Add();
            }

            //set columns' size
            foreach (DataGridViewColumn c in board.Columns)
            {
                c.Width = board.Width / board.Columns.Count;
            }
            foreach (DataGridViewRow r in board.Rows)
            {
                r.Height = board.Height / board.Rows.Count;
            }
            for (int row = 0; row < board.Rows.Count; row++)
            {
                for (int col = 0; col < board.Columns.Count; col++)
                {
                    board[col, row].ReadOnly = true;
                }
            }

            for (int i = 0; i < idc.Count; i++)
            {
                id_cells word = idc[i];
                int startCol = word.x;
                int startRow = word.y;
                char[] letters = word.word.ToCharArray();

                for (int j = 0; j < letters.Length; j++)
                {
                    if (word.direction.ToLower() == "горизонталь")
                        FormatCell(startRow, startCol + j, letters[j].ToString(), i);
                    else if (word.direction.ToLower() == "вертикаль")
                        FormatCell(startRow + j, startCol, letters[j].ToString(), i);
                }
            }
        }

        private void FormatCell(int row, int col, string letter, int number)
        {
            DataGridViewCell c = board[col, row];
            c.Style.BackColor = Color.White;
            c.ReadOnly = false;
            //c.Tag = letter;
            c.Tag = new CellData(letter, number);
        }

        private void board_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            string number = "";

            if (idc.Any(c => (number = c.number) != "" && c.x == e.ColumnIndex && c.y == e.RowIndex))
            {
                Rectangle rect = new Rectangle(e.CellBounds.X, e.CellBounds.Y, 20, 20);
                e.Graphics.FillRectangle(Brushes.White, rect);
                Font font = new Font(e.CellStyle.Font.FontFamily, 7);
                e.Graphics.DrawString(number, font, Brushes.Black, rect);
                e.PaintContent(e.ClipBounds);
                e.Handled = true;
            }
        }

        #endregion

        #region --- Game process ---

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var cell = board[e.ColumnIndex, e.RowIndex];

            if (cell.Tag == null)
                return;

            if (cell.Value == null || string.IsNullOrWhiteSpace(cell.Value.ToString()))
            {
                cell.Value = null;
                cell.Style.ForeColor = Color.Black;
                return;
            }

            string value = cell.Value.ToString().ToUpper();
            if (value.Length > 1)
                value = value.Substring(0, 1);
            cell.Value = value;

            if (cell.Tag is CellData data)
            {
                if (value == data.Letter.ToUpper())
                    cell.Style.ForeColor = Color.Green;
                else
                    cell.Style.ForeColor = Color.Red;
            }
        }

        #endregion

        #region --- ContextMenuStrip Tools ---

        private void відкритиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "TXT files (.txt)|*.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                crosswordFile = ofd.FileName;

                board.Rows.Clear();
                clueWindow.clueTable.Rows.Clear();
                idc.Clear();

                buildWordList();
                InitializeBoard();
            }
        }

        private void правилаГриToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rulesForm == null || rulesForm.IsDisposed)
            {
                rulesForm = new CrosswordsRules();
                rulesForm.Show();
            }
            else
            {
                rulesForm.WindowState = FormWindowState.Normal;
                rulesForm.BringToFront();
                rulesForm.Activate();
            }
        }

        #endregion

        #region --- Interface ---

        private void board_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox tb)
            {
                tb.MaxLength = 1;
            }
        }

        private void board_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var cell = board[e.ColumnIndex, e.RowIndex];

            if (cell.Tag is CellData data)
            {
                int wordIndex = data.WordIndex;

                clueWindow.clueTable.ClearSelection();
                clueWindow.clueTable.Rows[wordIndex].Selected = true;
                clueWindow.clueTable.FirstDisplayedScrollingRowIndex = wordIndex;
            }
            else
            {
                board.ClearSelection();
            }
        }

        private void Crosswords_LocationChanged(object sender, EventArgs e)
        {
            clueWindow.SetDesktopLocation(this.Location.X + this.Width - 9, this.Location.Y);
        }

        private void Crosswords_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fullClose) return;

            var gamesExitForm = new GamesExit();
            gamesExitForm.ShowDialog();
            if (!GamesExit.closing)
            {
                e.Cancel = true;
                return;
            }

            fullClose = true;

            if (clueWindow != null && !clueWindow.IsDisposed)
                clueWindow.Close();

            if (rulesForm != null && !rulesForm.IsDisposed)
                rulesForm.Close();
        }

        public void CloseFromClues()
        {
            if (fullClose) return;

            fullClose = true;

            this.Close();

            if (rulesForm != null && !rulesForm.IsDisposed)
                rulesForm.Close();
        }

        #endregion

    }

    public class id_cells
    {
        public int x;
        public int y;
        public string direction;
        public string number;
        public string word;
        public string clue;

        public id_cells(int x, int y, string direction, string number, string word, string clue)
        {
            this.x = x;
            this.y = y;
            this.direction = direction;
            this.number = number;
            this.word = word;
            this.clue = clue;
        }
    }

    public class CellData
    {
        public string Letter { get; set; }
        public int WordIndex { get; set; }

        public CellData(string letter, int wordIndex)
        {
            Letter = letter;
            WordIndex = wordIndex;
        }
    }
}