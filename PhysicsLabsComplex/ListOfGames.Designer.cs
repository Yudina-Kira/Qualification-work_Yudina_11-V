namespace PhysicsLabsComplex
{
    partial class ListOfGames
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
            this.cards2 = new PhysicsLabsComplex.Cards();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttons2 = new PhysicsLabsComplex.Buttons();
            this.cards1 = new PhysicsLabsComplex.Cards();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttons1 = new PhysicsLabsComplex.Buttons();
            this.cards2.SuspendLayout();
            this.cards1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cards2
            // 
            this.cards2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(140)))), ((int)(((byte)(180)))));
            this.cards2.Controls.Add(this.panel2);
            this.cards2.Controls.Add(this.buttons2);
            this.cards2.Location = new System.Drawing.Point(445, 50);
            this.cards2.Name = "cards2";
            this.cards2.Rounding = 20;
            this.cards2.RoundingEnable = true;
            this.cards2.Size = new System.Drawing.Size(338, 334);
            this.cards2.TabIndex = 13;
            this.cards2.TextHover = null;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(155)))), ((int)(((byte)(162)))));
            this.panel2.BackgroundImage = global::PhysicsLabsComplex.Properties.Resources.Screenshot_2025_12_11_113133;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Location = new System.Drawing.Point(20, 18);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(300, 221);
            this.panel2.TabIndex = 8;
            // 
            // buttons2
            // 
            this.buttons2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(181)))), ((int)(((byte)(195)))));
            this.buttons2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttons2.ForeColor = System.Drawing.Color.White;
            this.buttons2.Location = new System.Drawing.Point(20, 256);
            this.buttons2.Name = "buttons2";
            this.buttons2.Rounding = 100;
            this.buttons2.RoundingEnable = true;
            this.buttons2.Size = new System.Drawing.Size(300, 60);
            this.buttons2.TabIndex = 4;
            this.buttons2.Text = "Картки на пам\'ять";
            this.buttons2.TextHover = null;
            this.buttons2.Click += new System.EventHandler(this.buttons2_Click);
            // 
            // cards1
            // 
            this.cards1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(140)))), ((int)(((byte)(180)))));
            this.cards1.Controls.Add(this.panel1);
            this.cards1.Controls.Add(this.buttons1);
            this.cards1.Location = new System.Drawing.Point(56, 50);
            this.cards1.Name = "cards1";
            this.cards1.Rounding = 20;
            this.cards1.RoundingEnable = true;
            this.cards1.Size = new System.Drawing.Size(338, 334);
            this.cards1.TabIndex = 12;
            this.cards1.TextHover = null;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(155)))), ((int)(((byte)(162)))));
            this.panel1.BackgroundImage = global::PhysicsLabsComplex.Properties.Resources.Screenshot_2026_01_17_173420;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Location = new System.Drawing.Point(20, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(300, 221);
            this.panel1.TabIndex = 8;
            // 
            // buttons1
            // 
            this.buttons1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(181)))), ((int)(((byte)(195)))));
            this.buttons1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttons1.ForeColor = System.Drawing.Color.White;
            this.buttons1.Location = new System.Drawing.Point(20, 256);
            this.buttons1.Name = "buttons1";
            this.buttons1.Rounding = 100;
            this.buttons1.RoundingEnable = true;
            this.buttons1.Size = new System.Drawing.Size(300, 60);
            this.buttons1.TabIndex = 4;
            this.buttons1.Text = "Кросворд";
            this.buttons1.TextHover = null;
            this.buttons1.Click += new System.EventHandler(this.buttons1_Click);
            // 
            // ListOfGames
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(67)))), ((int)(((byte)(92)))));
            this.ClientSize = new System.Drawing.Size(838, 441);
            this.Controls.Add(this.cards2);
            this.Controls.Add(this.cards1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ListOfGames";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Перелік ігор";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ListOfGames_FormClosing);
            this.cards2.ResumeLayout(false);
            this.cards1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Cards cards1;
        private System.Windows.Forms.Panel panel1;
        private Buttons buttons1;
        private Cards cards2;
        private System.Windows.Forms.Panel panel2;
        private Buttons buttons2;
    }
}