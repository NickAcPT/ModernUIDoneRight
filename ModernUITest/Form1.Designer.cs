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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.appBar1 = new NickAc.ModernUIDoneRight.Controls.AppBar();
            this.tilePanelReborn1 = new NickAc.ModernUIDoneRight.Controls.TilePanelReborn();
            this.SuspendLayout();
            // 
            // appBar1
            // 
            this.appBar1.Actions = ((System.Collections.Generic.List<NickAc.ModernUIDoneRight.Objects.AppAction>)(resources.GetObject("appBar1.Actions")));
            this.appBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.appBar1.IconVisible = true;
            this.appBar1.Location = new System.Drawing.Point(1, 33);
            this.appBar1.Name = "appBar1";
            this.appBar1.Size = new System.Drawing.Size(653, 50);
            this.appBar1.TabIndex = 1;
            this.appBar1.Text = "appBar1";
            this.appBar1.TextFont = new System.Drawing.Font("Segoe UI", 14F);
            // 
            // tilePanelReborn1
            // 
            this.tilePanelReborn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(119)))), ((int)(((byte)(189)))));
            this.tilePanelReborn1.BrandedTile = false;
            this.tilePanelReborn1.CanBeHovered = true;
            this.tilePanelReborn1.Checkable = true;
            this.tilePanelReborn1.Flat = true;
            this.tilePanelReborn1.ForeColor = System.Drawing.Color.White;
            this.tilePanelReborn1.Image = null;
            this.tilePanelReborn1.Location = new System.Drawing.Point(70, 115);
            this.tilePanelReborn1.Name = "tilePanelReborn1";
            this.tilePanelReborn1.Size = new System.Drawing.Size(205, 101);
            this.tilePanelReborn1.TabIndex = 2;
            this.tilePanelReborn1.Text = "tilePanelReborn1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(655, 374);
            this.ColorScheme.PrimaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(119)))), ((int)(((byte)(189)))));
            this.ColorScheme.SecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(75)))), ((int)(((byte)(120)))));
            this.Controls.Add(this.tilePanelReborn1);
            this.Controls.Add(this.appBar1);
            this.Location = new System.Drawing.Point(0, 0);
            this.MinimumSize = new System.Drawing.Size(144, 39);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private NickAc.ModernUIDoneRight.Controls.AppBar appBar1;
        private NickAc.ModernUIDoneRight.Controls.TilePanelReborn tilePanelReborn1;
    }
}

