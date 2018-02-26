using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using static System.Math;

namespace NickAc.ModernUIDoneRight.Utils
{
    public static class ShadowUtils
    {
        public interface IShadowController
        {
            bool ShouldShowShadow();
        }
        enum RenderSide
        {
            Top,
            Bottom,
            Left,
            Right
        }

        static RenderSide[] VisibleTop = { RenderSide.Bottom/*, RenderSide.Top*/ };
        static RenderSide[] VisibleBottom = { RenderSide.Top/*, RenderSide.Bottom*/ };
        static RenderSide[] VisibleLeft = { RenderSide.Right };
        static RenderSide[] VisibleRight = { RenderSide.Left };

        static bool IsVisible(RenderSide side, DockStyle st)
        {
            switch (st) {
                case DockStyle.Top:
                    return VisibleTop.Contains(side);
                case DockStyle.Bottom:
                    return VisibleBottom.Contains(side);
                case DockStyle.Left:
                    return VisibleLeft.Contains(side);
                case DockStyle.Right:
                    return VisibleRight.Contains(side);
                case DockStyle.Fill:
                    return false;
            }
            return true;
        }


        public static void DrawShadow(Graphics G, Color c, Rectangle r, int d, DockStyle st = DockStyle.None)
        {
            Color[] colors = GetColorVector(c, d).ToArray();

            if (IsVisible(RenderSide.Top, st))
                for (int i = 1; i < d; i++) {
                    //TOP
                    using (Pen pen = new Pen(colors[i], 1f))
                        G.DrawLine(pen, new Point(r.Left - Max(i - 1, 0), r.Top - i), new Point(r.Right + Max(i - 1, 0), r.Top - i));
                }

            if (IsVisible(RenderSide.Bottom, st))
                for (int i = 0; i < d; i++) {
                    //BOTTOM
                    using (Pen pen = new Pen(colors[i], 1f))
                        G.DrawLine(pen, new Point(r.Left - Max(i - 1, 0), r.Bottom + i), new Point(r.Right + i, r.Bottom + i));
                }
            if (IsVisible(RenderSide.Left, st))
                for (int i = 1; i < d; i++) {
                    //LEFT
                    using (Pen pen = new Pen(colors[i], 1f))
                        G.DrawLine(pen, new Point(r.Left - i, r.Top - i), new Point(r.Left - i, r.Bottom + i));
                }
            if (IsVisible(RenderSide.Right, st))
                for (int i = 0; i < d; i++) {
                    //RIGHT
                    using (Pen pen = new Pen(colors[i], 1f))
                        G.DrawLine(pen, new Point(r.Right + i, r.Top - i), new Point(r.Right + i, r.Bottom + Max(i - 1, 0)));
                }
        }

        //Code taken and adapted from StackOverflow (https://stackoverflow.com/a/13653167).
        //All credits go to Marino Šimić (https://stackoverflow.com/users/610204/marino-%c5%a0imi%c4%87).
        public static void DrawRoundedRectangle(this Graphics gfx, Rectangle bounds, int cornerRadius, Pen drawPen, Color fillColor)
        {
            int strokeOffset = Convert.ToInt32(Ceiling(drawPen.Width));
            bounds = Rectangle.Inflate(bounds, -strokeOffset, -strokeOffset);

            var gfxPath = new GraphicsPath();
            if (cornerRadius > 0) {
                gfxPath.AddArc(bounds.X, bounds.Y, cornerRadius, cornerRadius, 180, 90);
                gfxPath.AddArc(bounds.X + bounds.Width - cornerRadius, bounds.Y, cornerRadius, cornerRadius, 270, 90);
                gfxPath.AddArc(bounds.X + bounds.Width - cornerRadius, bounds.Y + bounds.Height - cornerRadius, cornerRadius,
                               cornerRadius, 0, 90);
                gfxPath.AddArc(bounds.X, bounds.Y + bounds.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
            } else {
                gfxPath.AddRectangle(bounds);
            }
            gfxPath.CloseAllFigures();
            using (SolidBrush brush = new SolidBrush(fillColor)) {
                gfx.FillPath(brush, gfxPath);
                if (drawPen != Pens.Transparent) {
                    var pen = new Pen(drawPen.Color);
                    pen.EndCap = pen.StartCap = LineCap.Round;
                    gfx.DrawPath(pen, gfxPath);
                    pen.Dispose();
                }
            }
            gfxPath.Dispose();
        }

        //Code taken and adapted from StackOverflow (https://stackoverflow.com/a/13653167).
        //All credits go to Marino Šimić (https://stackoverflow.com/users/610204/marino-%c5%a0imi%c4%87).
        public static void DrawOutsetShadow(Graphics g, Color shadowColor, int hShadow, int vShadow, int blur, int spread, Control control)
        {
            var rOuter = Rectangle.Inflate(control.Bounds, blur / 2, blur / 2);
            var rInner = Rectangle.Inflate(control.Bounds, blur / 2, blur / 2);
            //rInner.Offset(hShadow, vShadow);
            rInner.Inflate(-blur, -blur);
            rOuter.Inflate(spread, spread);
            //rOuter.Offset(hShadow, vShadow);
            var originalOuter = rOuter;

            var img = new Bitmap(originalOuter.Width, originalOuter.Height, g);
            var g2 = Graphics.FromImage(img);

            var currentBlur = 0;

            do {
                var transparency = (rOuter.Height - rInner.Height) / (double)(blur * 2 + spread * 2);
                var color = Color.FromArgb(((int)(200 * (transparency * transparency))), shadowColor);
                var rOutput = rInner;
                rOutput.Offset(-originalOuter.Left, -originalOuter.Top);
                g2.DrawRoundedRectangle(rOutput, 5, Pens.Transparent, color);
                rInner.Inflate(1, 1);
                currentBlur = (int)((double)blur * (1 - (transparency * transparency)));
            } while (rOuter.Contains(rInner));

            g2.Flush();
            g2.Dispose();

            g.DrawImage(img, originalOuter);

            img.Dispose();
        }

        //Code taken and adapted from https://stackoverflow.com/a/25741405
        //All credits go to TaW (https://stackoverflow.com/users/3152130/taw)
        static List<Color> GetColorVector(Color fc, int depth)
        {
            List<Color> cv = new List<Color>();
            int baseC = 65;
            float div = baseC / depth;
            for (int d = 1; d <= depth; d++) {
                cv.Add(Color.FromArgb(Max(0, baseC), fc));
                baseC -= (int)div;
            }
            return cv;
        }


        //Code taken and adapted from https://stackoverflow.com/a/25741405
        //All credits go to TaW (https://stackoverflow.com/users/3152130/taw)
        static GraphicsPath GetRectPath(Rectangle R)
        {
            byte[] fm = new byte[3];
            for (int b = 0; b < 3; b++) fm[b] = 1;
            List<Point> points = new List<Point>
            {
                new Point(R.Left, R.Bottom),
                new Point(R.Right, R.Bottom),
                new Point(R.Right, R.Top)
            };
            return new GraphicsPath(points.ToArray(), fm);
        }

        public static void CreateDropShadow(this Control ctrl)
        {
            if (ctrl.Parent != null) {
                ctrl.Parent.Paint += (s, e) => {
                    if (ctrl.Parent != null && ctrl.Visible && (!(ctrl is IShadowController) || ((IShadowController)ctrl).ShouldShowShadow()))
                        DrawShadow(e.Graphics, Color.Black, ctrl.Bounds, 7, ctrl.Dock);
                };
            }
        }
    }
}
