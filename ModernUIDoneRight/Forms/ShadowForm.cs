using System;    
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NickAc.ModernUIDoneRight.Forms
{
    public class ShadowForm : Form
    {
        #region Constructor

        #endregion

        #region Methods
        static Size ComputeMySize(Form f, int borderTimes2)
        {
            return new Size(f.Width + borderTimes2, f.Height + borderTimes2);
        }
        #endregion

        #region Properties
        public int BorderSize { get; set; } = 5;
        public float WindowOpacity { get; set; } = 0.30F;
        public Form ShadowOwner { get; set; }
        public Color ShadowColor { get; set; } = Color.Black;
        #endregion

        #region Other
        public void Show(Form f)
        {
            BackColor = ShadowColor;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            DoubleBuffered = true;
            if (f != null) {
                this.ShowInTaskbar = false;
                f.FormClosing += (sender, e) => this.Dispose();
                MaximizeBox = f.MaximizeBox;
                MinimizeBox = f.MinimizeBox;
                f.Load += (sender, e) => {
                    Left = f.Left - BorderSize;
                    Top = f.Top - BorderSize;
                    this.Opacity = WindowOpacity;
                };

                base.Show();
                this.Left = f.Left - BorderSize;
                this.Top = f.Top - BorderSize;
                switch (f.WindowState) {
                    case FormWindowState.Maximized:
                        this.Opacity = 0;
                        break;
                    default:
                        this.Opacity = WindowOpacity;
                        break;
                }

                var borderTimes2 = BorderSize * 2;
                this.Size = new Size(f.Width + borderTimes2, f.Height + borderTimes2);
                f.Move += (sender, e) => {
                    Refresh();
                    this.Left = f.Left - BorderSize;
                    this.Top = f.Top - BorderSize;
                };
                f.Owner = this;
                DoubleBuffered = true;
                ShadowOwner = f;
                f.VisibleChanged += (sender, e) => {
                    switch (f.WindowState) {
                        case FormWindowState.Maximized:
                            this.Opacity = 0;
                            break;
                        default:
                            this.Opacity = f.Visible ? WindowOpacity : 0;
                            break;
                    }
                };
                f.SizeChanged += (sender, e) => {
                    switch (f.WindowState) {
                        case FormWindowState.Maximized:
                            this.Opacity = 0;
                            break;
                        default:
                            this.Opacity = WindowOpacity;
                            break;
                    }
                    Refresh();
                    this.Size = ComputeMySize(f, borderTimes2);
                };
            }
        }
        protected override bool ShowWithoutActivation {
            get {
                return true;
            }
        }
        #endregion
    }
}