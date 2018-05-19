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
    public class ModernButton : Button
    {
        #region Enums

        [Flags]
        private enum MouseState
        {
            Outside = 1 << 0,
            Inside = 1 << 1,
            Down = 1 << 2,
            Up = 1 << 3,
        }
        #endregion

        #region Fields

        private ColorScheme _colorScheme = DefaultColorSchemes.Blue;
        private bool _customColorScheme;

        #endregion

        #region Properties

        public bool CustomColorScheme
        {
            get => _customColorScheme;
            set
            {
                if (!_customColorScheme && value && _colorScheme == DefaultColorSchemes.Blue)
                    _colorScheme = ColorScheme;
                _customColorScheme = value;
                Refresh();
            }
        }

        public ColorScheme ColorScheme
        {
            get
            {
                var form = FindForm();
                return form != null && form is ModernForm mdF && !CustomColorScheme ? mdF.ColorScheme : _colorScheme;
            }
            set { _colorScheme = value; Refresh(); }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Rectangle ControlBounds => new Rectangle(Point.Empty, Size);

        #endregion

        #region Methods

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var cursorLoc = PointToClient(Cursor.Position);
            base.OnPaint(pevent);
            using (var primary = new SolidBrush(ColorScheme.PrimaryColor))
            {
                using (var secondary = new SolidBrush(ColorScheme.SecondaryColor))
                {
                    pevent.Graphics.FillRectangle(
                        DisplayRectangle.Contains(cursorLoc) && MouseButtons == MouseButtons.Left ? secondary : primary,
                        ControlBounds);
                    using (var sF = ControlPaintWrapper.StringFormatForAlignment(TextAlign))
                    {
                        using (var brush = new SolidBrush(ColorScheme.ForegroundColor))
                        {
                            pevent.Graphics.DrawString(Text, Font, brush, DisplayRectangle, sF);
                        }
                    }
                }
            }
        }

        #endregion
    }
}