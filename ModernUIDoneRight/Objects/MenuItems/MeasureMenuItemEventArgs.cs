using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NickAc.ModernUIDoneRight.Objects.MenuItems
{
    public class MeasureMenuItemEventArgs : EventArgs
    {
        public MeasureMenuItemEventArgs(Font font, Graphics graphics, Size itemSize)
        {
            Font = font;
            Graphics = graphics;
            ItemSize = itemSize;
        }

        public Font Font { get; set; }
        public Graphics Graphics { get; set; }
        public Size ItemSize { get; set; } = Size.Empty;
    }
}
