using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NickAc.ModernUIDoneRight.Controls;
using NickAc.ModernUIDoneRight.Utils;
using static NickAc.ModernUIDoneRight.Controls.SidebarControl;

namespace NickAc.ModernUIDoneRight
{
    public class SidebarTextItem : SideBarItem
    {
        public String Text { get; set; } = "";
        private const int DEFAULT_TEXT_HEIGHT = 32;
        private const int SIDE_OFFSET = 8;

        public SidebarTextItem(string text)
        {
            Text = text;
        }

        public Color ForeColor { get; set; } = Color.Black;

        public override void DrawItem(SidebarControl c, Graphics g, Size itemSize, bool isSelected)
        {
            using (var sb = new SolidBrush(isSelected ? GraphicUtils.ForegroundColorForBackground(c.ColorScheme.SecondaryColor) : ForeColor)) {
                using (var format = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                }) {
                    g.DrawString(Text, c.Font, sb, new Rectangle(SIDE_OFFSET, 0, itemSize.Width, itemSize.Height), format);
                }
            }
        }

        public override void MeasureItem(SidebarControl c, Graphics g, out int itemHeight)
        {
            itemHeight = Math.Max(DEFAULT_TEXT_HEIGHT, (int)g.MeasureString(Text, c.Font).Height + SIDE_OFFSET * 2);
        }
    }
}
