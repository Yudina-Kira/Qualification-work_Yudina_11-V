namespace PhysicsLabsComplex
{
    partial class Clues
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.clueTable = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.clueTable)).BeginInit();
            this.SuspendLayout();
            // 
            // clueTable
            // 
            this.clueTable.AllowUserToAddRows = false;
            this.clueTable.AllowUserToDeleteRows = false;
            this.clueTable.AllowUserToResizeColumns = false;
            this.clueTable.AllowUserToResizeRows = false;
            this.clueTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.clueTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.clueTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.clueTable.DefaultCellStyle = dataGridViewCellStyle1;
            this.clueTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clueTable.Location = new System.Drawing.Point(0, 0);
            this.clueTable.MultiSelect = false;
            this.clueTable.Name = "clueTable";
            this.clueTable.RowHeadersVisible = false;
            this.clueTable.RowHeadersWidth = 51;
            this.clueTable.RowTemplate.Height = 24;
            this.clueTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.clueTable.Size = new System.Drawing.Size(482, 613);
            this.clueTable.TabIndex = 0;
            this.clueTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.clueTable_CellContentClick);
            this.clueTable.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.clueTable_CellMouseClick);
            this.clueTable.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.clueTable_CellValueChanged);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "№";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 40;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Напрямок";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 125;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column3.HeaderText = "Підказка";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Clues
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(482, 613);
            this.Controls.Add(this.clueTable);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Clues";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Підказки";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Clues_FormClosing);
            this.Load += new System.EventHandler(this.Clues_Load);
            this.Shown += new System.EventHandler(this.Clues_Shown);
            this.LocationChanged += new System.EventHandler(this.Clues_LocationChanged);
            ((System.ComponentModel.ISupportInitialize)(this.clueTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.DataGridView clueTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
    }
}