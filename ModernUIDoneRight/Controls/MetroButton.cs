using NickAc.ModernUIDoneRight.Forms;
using NickAc.ModernUIDoneRight.Objects;
using NickAc.ModernUIDoneRight.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NickAc.ModernUIDoneRight.Controls
{
    public class MetroButton : Button
    {

        #region Fields

        ColorScheme colorScheme = DefaultColorSchemes.Blue;

        #endregion

        #region Properties

        public ColorScheme ColorScheme {
            get {
                return Parent != null && Parent is ModernForm ? ((ModernForm)Parent).ColorScheme : colorScheme;
            }

            set {
                colorScheme = value;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Rectangle ControlBounds => new Rectangle(Point.Empty, Size);

        #endregion

        #region Methods

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Point cursorLoc = PointToClient(Cursor.Position);
            base.OnPaint(pevent);
            using (var primary = new SolidBrush(ColorScheme.PrimaryColor)) {
                using (var secondary = new SolidBrush(ColorScheme.SecondaryColor)) {
                    pevent.Graphics.FillRectangle(DisplayRectangle.Contains(cursorLoc) && MouseButtons == MouseButtons.Left ? secondary : primary, ControlBounds);
                    using (var sF = ControlPaintWrapper.StringFormatForAlignment(TextAlign)) {
                        using (var brush = new SolidBrush(ColorScheme.ForegroundColor)) {
                            pevent.Graphics.DrawString(Text, Font, brush, DisplayRectangle, sF);
                        }
                    }
                    
                }
            }
        }

        #endregion

    }
}
