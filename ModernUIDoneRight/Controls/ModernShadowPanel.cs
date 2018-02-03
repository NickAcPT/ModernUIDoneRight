using NickAc.ModernUIDoneRight.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace NickAc.ModernUIDoneRight.Controls
{
    public class ModernShadowPanel : Panel
    {
        private Bitmap FrozenImage { get; set; }

        public ModernShadowPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void Freeze()
        {
            FrozenImage = new Bitmap(Size.Width, Size.Height);
            using (var g = Graphics.FromImage(FrozenImage)) {
                DrawControlShadow(g);
            }
            Refresh();
        }

        public void Unfreeze()
        {
            FrozenImage = null;
            Refresh();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (FrozenImage != null)
                Freeze();
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (FrozenImage != null)
                Freeze();
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            if (FrozenImage != null)
                Freeze();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (FrozenImage != null) {
                e.Graphics.DrawImage(FrozenImage, Point.Empty);
                return;
            }
            DrawControlShadow(e.Graphics);
        }

        private const int SHADOW_OFFSET = 4;
        private const int HALF_SHADOW_OFFSET = SHADOW_OFFSET / 2;
        private const int HALF_HALF_SHADOW_OFFSET = HALF_SHADOW_OFFSET / 2;
        private void DrawControlShadow(Graphics g)
        {
            using (var brush = new SolidBrush(Color.FromArgb(150, Color.Black))) {
                using (var img = new Bitmap(Width, Height)) {
                    using (var gp = Graphics.FromImage(img)) {
                        foreach (Control c in Controls) {
                            //gp.DrawRoundedRectangle(rInner, 5, Pens.Transparent, Color.Black);
                            gp.FillRectangle(brush, Rectangle.Inflate(c.Bounds, HALF_SHADOW_OFFSET, HALF_HALF_SHADOW_OFFSET));
                        }
                    }
                    var gaussian = new GaussianBlur(img);
                    using (var result = gaussian.Process(SHADOW_OFFSET)) {
                        g.DrawImageUnscaled(result, Point.Empty);
                    }
                }
            }
        }
    }
}