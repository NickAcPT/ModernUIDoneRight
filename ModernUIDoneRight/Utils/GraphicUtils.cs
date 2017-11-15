using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace NickAc.ModernUIDoneRight.Utils
{
    public static class GraphicUtils
    {
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
            return PerceivedBrightness(back) > 130 ? Color.Black : Color.White;
        }



        public static void DrawRectangleBorder(Rectangle rect, Graphics g, Color borderColor)
        {
            using (Pen p = new Pen(borderColor))
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

        public static void DrawCenteredText(Graphics g, string text, Font f, Rectangle rect, Color textColor)
        {
            var sb = new SolidBrush(textColor);
            var stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,      // -- Horizontal Alignment
                LineAlignment = StringAlignment.Center      // || Vertical Alignment
            };
            g.DrawString(text, f, sb, rect, stringFormat);
            stringFormat.Dispose();
            sb.Dispose();
        }


    }
}
