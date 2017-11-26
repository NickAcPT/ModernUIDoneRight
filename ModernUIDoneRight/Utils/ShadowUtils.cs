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
        enum RenderSide
        {
            Top,
            Bottom,
            Left,
            Right
        }

        static RenderSide[] VisibleTop = { RenderSide.Bottom, RenderSide.Top };
        static RenderSide[] VisibleBottom = { RenderSide.Top, RenderSide.Bottom };
        static RenderSide[] VisibleLeft = { RenderSide.Right };
        static RenderSide[] VisibleRight = { RenderSide.Left };

        static bool IsVisible(RenderSide side, DockStyle st)
        {
            switch (st)
            {
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


        public static void DrawShadow(Graphics G, Color c, Rectangle r, int d, Color BackColor, DockStyle st = DockStyle.None)
        {
            Color[] colors = GetColorVector(c, BackColor, d).ToArray();

            if (IsVisible(RenderSide.Top, st))
                for (int i = 1; i < d; i++)
                {
                    //TOP
                    using (Pen pen = new Pen(colors[i], 1f))
                        G.DrawLine(pen, new Point(r.Left - Max(i - 1, 0), r.Top - i), new Point(r.Right + Max(i - 1, 0), r.Top - i));
                }

            if (IsVisible(RenderSide.Bottom, st))
                for (int i = 0; i < d; i++)
                {
                    //BOTTOM
                    using (Pen pen = new Pen(colors[i], 1f))
                        G.DrawLine(pen, new Point(r.Left - Max(i - 1, 0), r.Bottom + i), new Point(r.Right + i, r.Bottom + i));
                }
            if (IsVisible(RenderSide.Left, st))
                for (int i = 1; i < d; i++)
                {
                    //LEFT
                    using (Pen pen = new Pen(colors[i], 1f))
                        G.DrawLine(pen, new Point(r.Left - i, r.Top - i), new Point(r.Left - i, r.Bottom + i));
                }
            if (IsVisible(RenderSide.Right, st))
                for (int i = 0; i < d; i++)
                {
                    //RIGHT
                    using (Pen pen = new Pen(colors[i], 1f))
                        G.DrawLine(pen, new Point(r.Right + i, r.Top - i), new Point(r.Right + i, r.Bottom + Max(i - 1, 0)));
                }
        }

        //Code taken and adapted from https://stackoverflow.com/a/25741405
        //All credits go to TaW (https://stackoverflow.com/users/3152130/taw)
        static List<Color> GetColorVector(Color fc, Color bc, int depth)
        {
            List<Color> cv = new List<Color>();
            int baseC = 100;
            float div = baseC / depth;
            for (int d = 1; d <= depth; d++)
            {
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
            List<Point> points = new List<Point>();
            points.Add(new Point(R.Left, R.Bottom));
            points.Add(new Point(R.Right, R.Bottom));
            points.Add(new Point(R.Right, R.Top));
            return new GraphicsPath(points.ToArray(), fm);
        }

        public static void CreateDropShadow(this Control ctrl)
        {
            if (ctrl.Parent != null)
            {
                ctrl.Parent.Paint += (s, e) =>
                {
                    if (ctrl.Parent != null)
                        DrawShadow(e.Graphics, Color.Black, ctrl.Bounds, 7, ctrl.Parent.BackColor, ctrl.Dock);
                };
            }
        }
    }
}
