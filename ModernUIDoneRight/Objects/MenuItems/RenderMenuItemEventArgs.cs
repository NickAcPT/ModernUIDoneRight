using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NickAc.ModernUIDoneRight.Objects.MenuItems
{
    public class RenderMenuItemEventArgs : EventArgs
    {
        public RenderMenuItemEventArgs(Graphics graphics, Rectangle rectangle, Font font)
        {
            Graphics = graphics;
            Rectangle = rectangle;
            Font = font;
        }

        public Font Font { get; set; }
        public Graphics Graphics { get; set; }
        public Rectangle Rectangle { get; set; }
    }
}
