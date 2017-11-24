namespace ModernUITest
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.appBar1 = new NickAc.ModernUIDoneRight.Controls.AppBar();
            this.metroButton1 = new NickAc.ModernUIDoneRight.Controls.MetroButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(104, 132);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // appBar1
            // 
            this.appBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.appBar1.IconVisible = false;
            this.appBar1.Location = new System.Drawing.Point(1, 33);
            this.appBar1.Name = "appBar1";
            this.appBar1.Size = new System.Drawing.Size(653, 50);
            this.appBar1.TabIndex = 1;
            this.appBar1.Text = "appBar1";
            this.appBar1.TextFont = new System.Drawing.Font("Segoe UI", 14F);
            // 
            // metroButton1
            // 
            this.metroButton1.Location = new System.Drawing.Point(211, 132);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(75, 23);
            this.metroButton1.TabIndex = 2;
            this.metroButton1.Text = "metroButton1";
            this.metroButton1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(360, 299);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(655, 374);
            this.ColorScheme.PrimaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(119)))), ((int)(((byte)(189)))));
            this.ColorScheme.SecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(75)))), ((int)(((byte)(120)))));
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.metroButton1);
            this.Controls.Add(this.appBar1);
            this.Controls.Add(this.button1);
            this.Location = new System.Drawing.Point(0, 0);
            this.MinimumSize = new System.Drawing.Size(144, 39);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private NickAc.ModernUIDoneRight.Controls.AppBar appBar1;
        private NickAc.ModernUIDoneRight.Controls.MetroButton metroButton1;
        private System.Windows.Forms.TextBox textBox1;
    }
}

