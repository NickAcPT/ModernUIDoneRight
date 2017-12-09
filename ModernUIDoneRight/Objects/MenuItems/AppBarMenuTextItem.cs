using NickAc.ModernUIDoneRight.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NickAc.ModernUIDoneRight.Objects.MenuItems
{
    public class AppBarMenuTextItem : AppBarMenuItem
    {
        private const int minimumWidth = 150;
        private const int minimumHeight = 40;
        private const int textOffsetLeft = 10;
        public Color ForeColor { get; set; } = Color.Black;

        public AppBarMenuTextItem() : this("")
        { }

        public AppBarMenuTextItem(string text)
        {
            Text = text;
        }

        public string Text { get; set; }

        protected override void OnRenderItem(RenderMenuItemEventArgs e)
        {
            using (var brush = new SolidBrush(ForeColor)) {
                GraphicUtils.DrawCenteredText(
                    e.Graphics,
                    Text,
                    e.Font,
                    Rectangle.FromLTRB(e.Rectangle.Left + textOffsetLeft, e.Rectangle.Top, e.Rectangle.Right, e.Rectangle.Bottom),
                    ForeColor, horizontal: false);
            }
        }

        protected override void OnMeasureItem(MeasureMenuItemEventArgs e)
        {
            Size textSize = e.Graphics.MeasureString(Text, e.Font).ToSize();
            e.ItemSize = new Size(Math.Max(minimumWidth, textSize.Width), Math.Max(minimumHeight, textSize.Height));
        }

    }
}
