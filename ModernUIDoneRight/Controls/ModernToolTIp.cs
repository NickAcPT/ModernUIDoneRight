using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using NickAc.ModernUIDoneRight.Forms;
using NickAc.ModernUIDoneRight.Objects;

namespace NickAc.ModernUIDoneRight.Controls
{
    public class ModernToolTip : ToolTip
    {
        private readonly int _opacity = 128;
        private Font _font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        private Size _size = new Size(300, 100);
        private int _margin = 5;
        public Font Font { get => _font; set => _font = value; }
        public Size Size { get => _size; set => _size = value; }
        public int Margin { get => _margin; set => _margin = value; }

        public ModernToolTip()
        {
            this.OwnerDraw = true;
            this.Draw += Handle_Draw;
            this.Popup += Handle_Popup;
        }

        void Handle_Popup(object sender, PopupEventArgs e)
        {
            e.ToolTipSize = Size;
        }


        private void Handle_Draw(object sender, DrawToolTipEventArgs e)
        {
            if (string.IsNullOrEmpty(e.ToolTipText))
            {
                return;
            }

            ColorScheme colorScheme = GetColorScheme(e.AssociatedControl);
            Graphics g = e.Graphics;

            SolidBrush brush = new SolidBrush(Color.FromArgb(_opacity, colorScheme.SecondaryColor));
            g.FillRectangle(brush, e.Bounds);

            Rectangle border = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
            g.DrawRectangle(new Pen(colorScheme.PrimaryColor, 1), border);

            Rectangle textRect = new Rectangle(e.Bounds.X + Margin, e.Bounds.Y + Margin, e.Bounds.Width - Margin, e.Bounds.Height - Margin);
            SolidBrush textBrush = new SolidBrush(colorScheme.ForegroundColor);
            g.DrawString(e.ToolTipText, Font, textBrush, textRect);

            brush.Dispose();
        }

        private ColorScheme GetColorScheme(Control associatedControl)
        {
            if (associatedControl != null)
            {
                if (associatedControl.FindForm() is ModernForm form)
                {
                    return form.ColorScheme;
                }
            }
            return DefaultColorSchemes.Blue;
        }


    }
}
