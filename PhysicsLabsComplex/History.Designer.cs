namespace PhysicsLabsComplex
{
    partial class History
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
            System.Windows.Forms.TreeNode treeNode46 = new System.Windows.Forms.TreeNode("1.1. Перші уявлення про електрику");
            System.Windows.Forms.TreeNode treeNode47 = new System.Windows.Forms.TreeNode("1.2. Розвиток техніки, перші генератори заряду");
            System.Windows.Forms.TreeNode treeNode48 = new System.Windows.Forms.TreeNode("1.3. Відкриття різних властивостей заряду");
            System.Windows.Forms.TreeNode treeNode49 = new System.Windows.Forms.TreeNode("1.4. Лейденська банка");
            System.Windows.Forms.TreeNode treeNode50 = new System.Windows.Forms.TreeNode("1.5. Від статичної електрики до електричного струму");
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.saveButton = new PhysicsLabsComplex.Buttons();
            this.toolColorPanel = new System.Windows.Forms.Panel();
            this.toggleSwitch1 = new PhysicsLabsComplex.ToggleSwitch();
            this.widthTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.resetButton = new PhysicsLabsComplex.Buttons();
            this.colorButton = new PhysicsLabsComplex.Buttons();
            this.laserButton = new PhysicsLabsComplex.Buttons();
            this.markerButton = new PhysicsLabsComplex.Buttons();
            this.penButton = new PhysicsLabsComplex.Buttons();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.groupBox1.Controls.Add(this.saveButton);
            this.groupBox1.Controls.Add(this.toolColorPanel);
            this.groupBox1.Controls.Add(this.toggleSwitch1);
            this.groupBox1.Controls.Add(this.widthTextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.resetButton);
            this.groupBox1.Controls.Add(this.colorButton);
            this.groupBox1.Controls.Add(this.laserButton);
            this.groupBox1.Controls.Add(this.markerButton);
            this.groupBox1.Controls.Add(this.penButton);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox1.Location = new System.Drawing.Point(689, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(685, 135);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Режим малювання";
            // 
            // saveButton
            // 
            this.saveButton.BackColor = System.Drawing.Color.LimeGreen;
            this.saveButton.Enabled = false;
            this.saveButton.ForeColor = System.Drawing.Color.White;
            this.saveButton.Location = new System.Drawing.Point(501, 40);
            this.saveButton.Name = "saveButton";
            this.saveButton.Rounding = 100;
            this.saveButton.RoundingEnable = false;
            this.saveButton.Size = new System.Drawing.Size(130, 31);
            this.saveButton.TabIndex = 9;
            this.saveButton.Text = "Зберегти";
            this.saveButton.TextHover = null;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // toolColorPanel
            // 
            this.toolColorPanel.Enabled = false;
            this.toolColorPanel.Location = new System.Drawing.Point(434, 40);
            this.toolColorPanel.Name = "toolColorPanel";
            this.toolColorPanel.Size = new System.Drawing.Size(31, 31);
            this.toolColorPanel.TabIndex = 8;
            // 
            // toggleSwitch1
            // 
            this.toggleSwitch1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(152)))), ((int)(((byte)(167)))), ((int)(((byte)(174)))));
            this.toggleSwitch1.BackColorON = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(191)))), ((int)(((byte)(100)))));
            this.toggleSwitch1.Checked = false;
            this.toggleSwitch1.Font = new System.Drawing.Font("Verdana", 9F);
            this.toggleSwitch1.Location = new System.Drawing.Point(199, 0);
            this.toggleSwitch1.Name = "toggleSwitch1";
            this.toggleSwitch1.Size = new System.Drawing.Size(50, 20);
            this.toggleSwitch1.TabIndex = 3;
            this.toggleSwitch1.Text = "toggleSwitch1";
            this.toggleSwitch1.Click += new System.EventHandler(this.toggleSwitch1_Click);
            // 
            // widthTextBox
            // 
            this.widthTextBox.Enabled = false;
            this.widthTextBox.Location = new System.Drawing.Point(378, 87);
            this.widthTextBox.Name = "widthTextBox";
            this.widthTextBox.Size = new System.Drawing.Size(87, 26);
            this.widthTextBox.TabIndex = 7;
            this.widthTextBox.Text = "6";
            this.widthTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.widthTextBox.TextChanged += new System.EventHandler(this.widthTextBox_TextChanged);
            this.widthTextBox.Leave += new System.EventHandler(this.widthTextBox_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(284, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Товщина:";
            // 
            // resetButton
            // 
            this.resetButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.resetButton.Enabled = false;
            this.resetButton.ForeColor = System.Drawing.Color.White;
            this.resetButton.Location = new System.Drawing.Point(501, 85);
            this.resetButton.Name = "resetButton";
            this.resetButton.Rounding = 100;
            this.resetButton.RoundingEnable = false;
            this.resetButton.Size = new System.Drawing.Size(130, 31);
            this.resetButton.TabIndex = 5;
            this.resetButton.Text = "Очистити";
            this.resetButton.TextHover = null;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // colorButton
            // 
            this.colorButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(181)))), ((int)(((byte)(195)))));
            this.colorButton.Enabled = false;
            this.colorButton.ForeColor = System.Drawing.Color.White;
            this.colorButton.Location = new System.Drawing.Point(288, 40);
            this.colorButton.Name = "colorButton";
            this.colorButton.Rounding = 100;
            this.colorButton.RoundingEnable = false;
            this.colorButton.Size = new System.Drawing.Size(130, 31);
            this.colorButton.TabIndex = 4;
            this.colorButton.Text = "Колір";
            this.colorButton.TextHover = null;
            this.colorButton.Click += new System.EventHandler(this.colorButton_Click);
            // 
            // laserButton
            // 
            this.laserButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(181)))), ((int)(((byte)(195)))));
            this.laserButton.Enabled = false;
            this.laserButton.ForeColor = System.Drawing.Color.White;
            this.laserButton.Location = new System.Drawing.Point(31, 85);
            this.laserButton.Name = "laserButton";
            this.laserButton.Rounding = 100;
            this.laserButton.RoundingEnable = false;
            this.laserButton.Size = new System.Drawing.Size(218, 31);
            this.laserButton.TabIndex = 3;
            this.laserButton.Text = "Лазерна вказівка";
            this.laserButton.TextHover = null;
            this.laserButton.Click += new System.EventHandler(this.laserButton_Click);
            // 
            // markerButton
            // 
            this.markerButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(181)))), ((int)(((byte)(195)))));
            this.markerButton.Enabled = false;
            this.markerButton.ForeColor = System.Drawing.Color.White;
            this.markerButton.Location = new System.Drawing.Point(147, 40);
            this.markerButton.Name = "markerButton";
            this.markerButton.Rounding = 100;
            this.markerButton.RoundingEnable = false;
            this.markerButton.Size = new System.Drawing.Size(102, 31);
            this.markerButton.TabIndex = 2;
            this.markerButton.Text = "Маркер";
            this.markerButton.TextHover = null;
            this.markerButton.Click += new System.EventHandler(this.markerButton_Click);
            // 
            // penButton
            // 
            this.penButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(181)))), ((int)(((byte)(195)))));
            this.penButton.Enabled = false;
            this.penButton.ForeColor = System.Drawing.Color.White;
            this.penButton.Location = new System.Drawing.Point(31, 40);
            this.penButton.Name = "penButton";
            this.penButton.Rounding = 100;
            this.penButton.RoundingEnable = false;
            this.penButton.Size = new System.Drawing.Size(102, 31);
            this.penButton.TabIndex = 1;
            this.penButton.Text = "Олівець";
            this.penButton.TextHover = null;
            this.penButton.Click += new System.EventHandler(this.penButton_Click);
            // 
            // treeView1
            // 
            this.treeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.treeView1.Location = new System.Drawing.Point(12, 12);
            this.treeView1.Name = "treeView1";
            treeNode46.Name = "Node1_1";
            treeNode46.Text = "1.1. Перші уявлення про електрику";
            treeNode47.Name = "Node1_2";
            treeNode47.Text = "1.2. Розвиток техніки, перші генератори заряду";
            treeNode48.Name = "Node1_3";
            treeNode48.Text = "1.3. Відкриття різних властивостей заряду";
            treeNode49.Name = "Node1_4";
            treeNode49.Text = "1.4. Лейденська банка";
            treeNode50.Name = "Node1_5";
            treeNode50.Text = "1.5. Від статичної електрики до електричного струму";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode46,
            treeNode47,
            treeNode48,
            treeNode49,
            treeNode50});
            this.treeView1.Size = new System.Drawing.Size(659, 135);
            this.treeView1.TabIndex = 4;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(12, 162);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(1362, 601);
            this.webBrowser1.TabIndex = 3;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // History
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(67)))), ((int)(((byte)(92)))));
            this.ClientSize = new System.Drawing.Size(1386, 776);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.webBrowser1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1404, 823);
            this.Name = "History";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Історична довідка";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.History_FormClosing);
            this.Load += new System.EventHandler(this.History_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Buttons saveButton;
        private System.Windows.Forms.Panel toolColorPanel;
        private ToggleSwitch toggleSwitch1;
        private System.Windows.Forms.TextBox widthTextBox;
        private System.Windows.Forms.Label label1;
        private Buttons resetButton;
        private Buttons colorButton;
        private Buttons laserButton;
        private Buttons markerButton;
        private Buttons penButton;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.ColorDialog colorDialog1;
    }
}