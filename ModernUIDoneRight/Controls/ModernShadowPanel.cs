using NickAc.ModernUIDoneRight.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NickAc.ModernUIDoneRight.Controls
{
    public class ModernShadowPanel : Panel
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
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
