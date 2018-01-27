using NickAc.ModernUIDoneRight.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace NickAc.ModernUIDoneRight.Controls
{
    public class ModernShadowPanel : Panel
    {
        private Bitmap FrozenImage { get; set; }

        public void Freeze()
        {
            FrozenImage = new Bitmap(Size.Width, Size.Height);
            using (var g = Graphics.FromImage(FrozenImage)) {
                foreach (Control c in Controls) {
                    DrawControlShadow(c, g);
                }
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
            foreach (Control c in Controls) {
                DrawControlShadow(c, e.Graphics);
            }
        }

        private void DrawControlShadow(Control c, Graphics g)
        {
            ShadowUtils.DrawOutsetShadow(g, Color.Black, 1, 0, 20, 0, c);
        }
    }
}