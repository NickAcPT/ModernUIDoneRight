using NickAc.ModernUIDoneRight.Utils;
using System.Drawing;
using System.Windows.Forms;
using static NickAc.ModernUIDoneRight.Utils.GraphicUtils;

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
        public Color ForegroundColor => ForegroundColorForBackground(SecondaryColor);
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
