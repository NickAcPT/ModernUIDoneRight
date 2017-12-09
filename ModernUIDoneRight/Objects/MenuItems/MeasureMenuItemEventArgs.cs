using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NickAc.ModernUIDoneRight.Objects.MenuItems
{
    public class MeasureMenuItemEventArgs : EventArgs
    {
        public Rectangle ItemRectangle { get; set; } = Rectangle.Empty;
    }
}
