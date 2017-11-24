using NickAc.ModernUIDoneRight.Forms;
using NickAc.ModernUIDoneRight.Objects;
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
        #region Properties

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Rectangle ControlBounds => new Rectangle(Point.Empty, Size);

        ColorScheme colorScheme = DefaultColorSchemes.Blue;

        public ColorScheme ColorScheme {
            get {
                return Parent != null && Parent is MetroForm ? ((MetroForm)Parent).ColorScheme : colorScheme;
            }

            set {
                colorScheme = value;
            }
        }
        #endregion
        #region Overriden Methods
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            using (var primary = new SolidBrush(ColorScheme.PrimaryColor)) {
                using (var seconday = new SolidBrush(ColorScheme.SecondaryColor)) {
                    pevent.Graphics.FillRectangle(primary, ControlBounds);
                }
            }
        }
        #endregion

    }
}
