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
    public partial class Clues : Form
    {
        private Crosswords crosswordForm;

        public Clues(Crosswords crosswordForm)
        {
            InitializeComponent();
            this.crosswordForm = crosswordForm;
        }

        private void Clues_Load(object sender, EventArgs e)
        {
            DataGridViewCheckBoxColumn doneColumn = new DataGridViewCheckBoxColumn();
            doneColumn.HeaderText = "✓";
            doneColumn.Name = "Done";

            clueTable.Columns.Insert(0, doneColumn);

            clueTable.EditMode = DataGridViewEditMode.EditOnEnter;

            clueTable.RowHeadersVisible = false;
            clueTable.AllowUserToAddRows = false;

            clueTable.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            clueTable.Columns["Done"].Width = 70;     
            clueTable.Columns[1].Width = 70;

            clueTable.CellValueChanged += clueTable_CellValueChanged;
        }

        private void Clues_Shown(object sender, EventArgs e)
        {
            int totalHeight = clueTable.ColumnHeadersHeight;
            foreach (DataGridViewRow row in clueTable.Rows)
                totalHeight += row.Height;

            this.Height = totalHeight + (this.Height - this.ClientSize.Height) + 5;
        }

        #region --- Game process ---

        private void clueTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == clueTable.Columns["Done"].Index)
            {
                clueTable.CommitEdit(DataGridViewDataErrorContexts.Commit);
                clueTable.ClearSelection();
            }
        }

        private void clueTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!clueTable.Columns.Contains("Done"))
                return;

            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == clueTable.Columns["Done"].Index)
            {
                bool isChecked = Convert.ToBoolean(clueTable.Rows[e.RowIndex].Cells["Done"].Value ?? false);
                DataGridViewRow row = clueTable.Rows[e.RowIndex];

                if (isChecked)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        cell.Style.ForeColor = Color.Gray;
                        cell.Style.Font = new Font(clueTable.Font, FontStyle.Strikeout);
                    }
                }
                else
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        cell.Style.ForeColor = Color.Black;
                        cell.Style.Font = new Font(clueTable.Font, FontStyle.Regular);
                    }
                }
            }
        }

        #endregion

        private void Clues_LocationChanged(object sender, EventArgs e)
        {
            crosswordForm.SetDesktopLocation(this.Location.X - crosswordForm.Width + 9, this.Location.Y);
        }

        private void Clues_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (crosswordForm.fullClose) return;

            var gamesExitForm = new GamesExit();
            gamesExitForm.ShowDialog();

            if (!GamesExit.closing)
            {
                e.Cancel = true;
                return;
            }

            crosswordForm.CloseFromClues();
        }

        private void clueTable_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Индекс строки совпадает с индексом слова
            int wordIndex = e.RowIndex;
            id_cells word = crosswordForm.idc[wordIndex];

            // Первая клетка слова
            int startRow = word.y;
            int startCol = word.x;

            // Снять предыдущее выделение
            crosswordForm.board.ClearSelection();

            // Выделяем первую клетку
            crosswordForm.board[startCol, startRow].Selected = true;
        }
    }
}