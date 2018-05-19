using System;
using System.Drawing;
using System.Windows.Forms;

namespace NickAc.ModernUIDoneRight.Utils
{
    public static class GraphicUtils
    {

        public static void DrawHamburgerButton(Graphics g, SolidBrush secondary, Rectangle hamburgerRectangle, Color foreColor, Control c, bool smallButton = false)
        {
            if (Control.MouseButtons != MouseButtons.None && hamburgerRectangle.Contains(c.PointToClient(Cursor.Position)))
            {
                g.FillRectangle(secondary, hamburgerRectangle);
            }

            using (var forePen = new Pen(foreColor, 3))
            {
                //Draw hamburger icon
                var rect = hamburgerRectangle;
                const int barSize = 2;
                var spacingSides = smallButton ? 6 : 4;
                const int interval = 3;
                var centerX = rect.Right - (rect.Width / 2);
                var centerY = rect.Bottom - (rect.Height / 2);
                var topLine = centerY - (barSize * 2) - interval;
                var bottomLine = centerY + (barSize * 2) + interval;

                var oldMode = g.SmoothingMode;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                //Top
                g.DrawLine(forePen, rect.Left + spacingSides, topLine, rect.Right - spacingSides,
                    topLine);

                //Middle
                g.DrawLine(forePen, rect.Left + spacingSides, centerY, rect.Right - spacingSides,
                    centerY);

                //Bottom
                g.DrawLine(forePen, rect.Left + spacingSides, bottomLine, rect.Right - spacingSides,
                    bottomLine);

                g.SmoothingMode = oldMode;
            }
        }

        public static Rectangle OffsetAndReturn(this Rectangle rect, Point offset)
        {
            if (offset.Equals(Point.Empty))
                return rect;
            var newR = new Rectangle(rect.Location, rect.Size);
            newR.Offset(offset);
            return newR;
        }

        public static Rectangle OffsetAndReturn(this Rectangle rect, int x, int y)
        {
            if (x.Equals(Point.Empty.X) && y.Equals(Point.Empty.Y))
                return rect;
            var newR = new Rectangle(rect.Location, rect.Size);
            newR.Offset(x, y);
            return newR;
        }

        //Method taken from https://stackoverflow.com/a/2241471
        //All credits go to the author: JYelton(https://stackoverflow.com/users/161052/jyelton)
        public static int PerceivedBrightness(Color c)
        {
            return (int)Math.Sqrt(
            c.R * c.R * .299 +
            c.G * c.G * .587 +
            c.B * c.B * .114);
        }

        public static Color ForegroundColorForBackground(Color back)
        {
            return !IsDark(back) ? Color.Black : Color.White;
        }
        
        public static bool IsDark(this Color c)
        {
            return PerceivedBrightness(c) < 130;
        }
        public static bool IsDarker(this Color c)
        {
            return PerceivedBrightness(c) < 100;
        }


        public static void DrawRectangleBorder(Rectangle rect, Graphics g, Color borderColor)
        {
            using (var p = new Pen(borderColor))
            {
                //Top
                g.DrawLine(p, rect.Left + 1, rect.Top, rect.Right - 1, rect.Top);
                //Bottom
                g.DrawLine(p, rect.Left + 1, rect.Bottom - 1, rect.Right - 1, rect.Bottom - 1);
                //Left
                g.DrawLine(p, rect.Left, rect.Top, rect.Left, rect.Bottom);
                //Right
                g.DrawLine(p, rect.Right - 1, rect.Top, rect.Right - 1, rect.Bottom);
            }
        }
        
        public static void DrawCenteredText(Graphics g, string text, Font f, Rectangle rect, Color textColor, bool horizontal = true, bool vertical = true)
        {
            var sb = new SolidBrush(textColor);
            var stringFormat = new StringFormat();
            if (horizontal) stringFormat.Alignment = StringAlignment.Center;      // -- Horizontal Alignment
            if (vertical) stringFormat.LineAlignment = StringAlignment.Center;      // || Vertical Alignment

            g.DrawString(text, f, sb, rect, stringFormat);
            stringFormat.Dispose();
            sb.Dispose();
        }


    }
}
