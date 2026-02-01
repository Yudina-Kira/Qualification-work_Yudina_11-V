namespace PhysicsLabsComplex
{
    partial class Theory
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
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("1.1. Загальне визначення змінного струму та його характеристики");
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("1.2. Діюче значення змінного струму");
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("1.3. Багатофазний струм");
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("1.4. Переваги змінного струму");
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("1. ЗМІННИЙ СТРУМ", new System.Windows.Forms.TreeNode[] {
            treeNode24,
            treeNode25,
            treeNode26,
            treeNode27});
            System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("2.1. Імпульсний струм");
            System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("2.2. Конденсатор. Принцип роботи та характеристики");
            System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("2.3. Процеси зарядки і розрядки конденсатора");
            System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("2.4. Види конденсаторів");
            System.Windows.Forms.TreeNode treeNode33 = new System.Windows.Forms.TreeNode("2.5. Способи збільшення та зменшення загальної ємності");
            System.Windows.Forms.TreeNode treeNode34 = new System.Windows.Forms.TreeNode("ДОДАТКОВА ІНФОРМАЦІЯ ПРО КОНДЕНСАТОРИ", new System.Windows.Forms.TreeNode[] {
            treeNode32,
            treeNode33});
            System.Windows.Forms.TreeNode treeNode35 = new System.Windows.Forms.TreeNode("2. ДО ЛАБОРАТОРНОЇ РОБОТИ №1", new System.Windows.Forms.TreeNode[] {
            treeNode29,
            treeNode30,
            treeNode31,
            treeNode34});
            System.Windows.Forms.TreeNode treeNode36 = new System.Windows.Forms.TreeNode("3.1. Резистор. Принцип роботи та характеристики");
            System.Windows.Forms.TreeNode treeNode37 = new System.Windows.Forms.TreeNode("3.2. Індуктивність. Принцип роботи та характеристики");
            System.Windows.Forms.TreeNode treeNode38 = new System.Windows.Forms.TreeNode("3.3. Види опору в колах змінного струму");
            System.Windows.Forms.TreeNode treeNode39 = new System.Windows.Forms.TreeNode("3.4. Поведінка RC-ланки в колі змінного струму");
            System.Windows.Forms.TreeNode treeNode40 = new System.Windows.Forms.TreeNode("3.5. Поведінка RL-ланки в колі змінного струму");
            System.Windows.Forms.TreeNode treeNode41 = new System.Windows.Forms.TreeNode("3.6. Види резисторів");
            System.Windows.Forms.TreeNode treeNode42 = new System.Windows.Forms.TreeNode("3.7. Способи збільшення та зменшення загального опору");
            System.Windows.Forms.TreeNode treeNode43 = new System.Windows.Forms.TreeNode("ДОДАТКОВА ІНФОРМАЦІЯ ПРО РЕЗИСТОРИ", new System.Windows.Forms.TreeNode[] {
            treeNode41,
            treeNode42});
            System.Windows.Forms.TreeNode treeNode44 = new System.Windows.Forms.TreeNode("3.8. Види котушок індуктивності");
            System.Windows.Forms.TreeNode treeNode45 = new System.Windows.Forms.TreeNode("ДОДАТКОВА ІНФОРМАЦІЯ ПРО ІНДУКТИВНІСТЬ", new System.Windows.Forms.TreeNode[] {
            treeNode44});
            System.Windows.Forms.TreeNode treeNode46 = new System.Windows.Forms.TreeNode("3. ДО ЛАБОРАТОРНИХ РОБІТ №2, 3, 4", new System.Windows.Forms.TreeNode[] {
            treeNode36,
            treeNode37,
            treeNode38,
            treeNode39,
            treeNode40,
            treeNode43,
            treeNode45});
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.saveStatus = new PhysicsLabsComplex.Cards();
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
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(12, 162);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(1362, 601);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // treeView1
            // 
            this.treeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.treeView1.Location = new System.Drawing.Point(12, 12);
            this.treeView1.Name = "treeView1";
            treeNode24.Name = "Node1_1";
            treeNode24.Text = "1.1. Загальне визначення змінного струму та його характеристики";
            treeNode25.Name = "Node1_2";
            treeNode25.Text = "1.2. Діюче значення змінного струму";
            treeNode26.Name = "Node1_3";
            treeNode26.Text = "1.3. Багатофазний струм";
            treeNode27.Name = "Node1_4";
            treeNode27.Text = "1.4. Переваги змінного струму";
            treeNode28.Checked = true;
            treeNode28.Name = "Node1";
            treeNode28.Text = "1. ЗМІННИЙ СТРУМ";
            treeNode29.Name = "Node2_1";
            treeNode29.Text = "2.1. Імпульсний струм";
            treeNode30.Name = "Node2_2";
            treeNode30.Text = "2.2. Конденсатор. Принцип роботи та характеристики";
            treeNode31.Name = "Node2_3";
            treeNode31.Text = "2.3. Процеси зарядки і розрядки конденсатора";
            treeNode32.Name = "Node2_4";
            treeNode32.Text = "2.4. Види конденсаторів";
            treeNode33.Name = "Node2_5";
            treeNode33.Text = "2.5. Способи збільшення та зменшення загальної ємності";
            treeNode34.Name = "Node2_45";
            treeNode34.Text = "ДОДАТКОВА ІНФОРМАЦІЯ ПРО КОНДЕНСАТОРИ";
            treeNode35.Name = "Node2";
            treeNode35.Text = "2. ДО ЛАБОРАТОРНОЇ РОБОТИ №1";
            treeNode36.Name = "Node3_1";
            treeNode36.Text = "3.1. Резистор. Принцип роботи та характеристики";
            treeNode37.Name = "Node3_2";
            treeNode37.Text = "3.2. Індуктивність. Принцип роботи та характеристики";
            treeNode38.Name = "Node3_3";
            treeNode38.Text = "3.3. Види опору в колах змінного струму";
            treeNode39.Name = "Node3_4";
            treeNode39.Text = "3.4. Поведінка RC-ланки в колі змінного струму";
            treeNode40.Name = "Node3_5";
            treeNode40.Text = "3.5. Поведінка RL-ланки в колі змінного струму";
            treeNode41.Name = "Node3_6";
            treeNode41.Text = "3.6. Види резисторів";
            treeNode42.Name = "Node3_7";
            treeNode42.Text = "3.7. Способи збільшення та зменшення загального опору";
            treeNode43.Name = "Node3_45";
            treeNode43.Text = "ДОДАТКОВА ІНФОРМАЦІЯ ПРО РЕЗИСТОРИ";
            treeNode44.Name = "Node3_8";
            treeNode44.Text = "3.8. Види котушок індуктивності";
            treeNode45.Name = "Node_38";
            treeNode45.Text = "ДОДАТКОВА ІНФОРМАЦІЯ ПРО ІНДУКТИВНІСТЬ";
            treeNode46.Name = "Node3";
            treeNode46.Text = "3. ДО ЛАБОРАТОРНИХ РОБІТ №2, 3, 4";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode28,
            treeNode35,
            treeNode46});
            this.treeView1.Size = new System.Drawing.Size(659, 135);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.groupBox1.Controls.Add(this.saveStatus);
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
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Режим малювання";
            // 
            // saveStatus
            // 
            this.saveStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(135)))), ((int)(((byte)(142)))));
            this.saveStatus.Location = new System.Drawing.Point(639, 43);
            this.saveStatus.Name = "saveStatus";
            this.saveStatus.Rounding = 100;
            this.saveStatus.RoundingEnable = true;
            this.saveStatus.Size = new System.Drawing.Size(24, 22);
            this.saveStatus.TabIndex = 10;
            this.saveStatus.TextHover = null;
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
            this.toolColorPanel.BackColor = System.Drawing.Color.Transparent;
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
            // Theory
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
            this.Name = "Theory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Теорія";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Theory_FormClosing);
            this.Load += new System.EventHandler(this.Theory_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private Buttons laserButton;
        private Buttons markerButton;
        private Buttons penButton;
        private Buttons colorButton;
        private Buttons resetButton;
        private System.Windows.Forms.TextBox widthTextBox;
        private System.Windows.Forms.Label label1;
        private ToggleSwitch toggleSwitch1;
        private System.Windows.Forms.Panel toolColorPanel;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private Buttons saveButton;
        private Cards saveStatus;
    }
}