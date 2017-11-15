using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NickAc.ModernUIDoneRight.Objects
{
    public class ColorScheme
    {
        #region Constructor
        public ColorScheme(Color primaryColor, Color secondaryColor)
        {
            PrimaryColor = primaryColor;
            SecondaryColor = secondaryColor;
        }
        #endregion

        #region Properties
        public Color PrimaryColor { get; set; }
        public Color SecondaryColor { get; set; }
        #endregion

        #region Static Methods
        public static Color DarkenColor(Color original)
        {
            return ControlPaint.Dark(original, 0.05F);
        }
        
        public static ColorScheme CreateSimpleColorScheme(Color original)
        {
            return new ColorScheme(original, DarkenColor(original));
        }
        #endregion
    }
}
