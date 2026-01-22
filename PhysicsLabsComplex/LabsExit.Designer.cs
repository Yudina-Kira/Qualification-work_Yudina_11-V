namespace PhysicsLabsComplex
{
    partial class LabsExit
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttons1 = new PhysicsLabsComplex.Buttons();
            this.buttons2 = new PhysicsLabsComplex.Buttons();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(24, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(448, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ви закриваєте лабораторну роботу!\r\n";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(24, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(448, 37);
            this.label2.TabIndex = 1;
            this.label2.Text = "Куди саме Ви хочете повернутись?";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttons1
            // 
            this.buttons1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(181)))), ((int)(((byte)(195)))));
            this.buttons1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttons1.ForeColor = System.Drawing.Color.White;
            this.buttons1.Location = new System.Drawing.Point(24, 167);
            this.buttons1.Name = "buttons1";
            this.buttons1.RoundingEnable = true;
            this.buttons1.Size = new System.Drawing.Size(276, 50);
            this.buttons1.TabIndex = 2;
            this.buttons1.Text = "До списку лабораторних робіт";
            this.buttons1.TextHover = null;
            this.buttons1.Click += new System.EventHandler(this.buttons1_Click);
            // 
            // buttons2
            // 
            this.buttons2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(181)))), ((int)(((byte)(195)))));
            this.buttons2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttons2.ForeColor = System.Drawing.Color.White;
            this.buttons2.Location = new System.Drawing.Point(325, 167);
            this.buttons2.Name = "buttons2";
            this.buttons2.RoundingEnable = true;
            this.buttons2.Size = new System.Drawing.Size(147, 50);
            this.buttons2.TabIndex = 3;
            this.buttons2.Text = "До меню";
            this.buttons2.TextHover = null;
            this.buttons2.Click += new System.EventHandler(this.buttons2_Click);
            // 
            // LabsExit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(67)))), ((int)(((byte)(92)))));
            this.ClientSize = new System.Drawing.Size(497, 240);
            this.Controls.Add(this.buttons2);
            this.Controls.Add(this.buttons1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LabsExit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Вихід";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LabsExit_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Buttons buttons1;
        private Buttons buttons2;
    }
}