using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NickAc.ModernUIDoneRight.Objects
{

    public static class DefaultColorSchemes
    {
        public static ColorScheme Red => ColorScheme.CreateSimpleColorScheme(DefaultColors.Red);
        public static ColorScheme Blue => ColorScheme.CreateSimpleColorScheme(DefaultColors.Blue);
        public static ColorScheme Green => ColorScheme.CreateSimpleColorScheme(DefaultColors.Green);
    }
}
