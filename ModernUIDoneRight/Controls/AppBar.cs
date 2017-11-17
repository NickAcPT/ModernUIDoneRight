using NickAc.ModernUIDoneRight.Forms;
using NickAc.ModernUIDoneRight.Objects;
using NickAc.ModernUIDoneRight.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace NickAc.ModernUIDoneRight.Controls
{
    public class AppBar : Control
    {
        public int XTextOffset => /*Height*/20;
        #region Constructor
        public AppBar()
        {
            Size = new Size(10, RoundUp((int)(MetroForm.DEFAULT_TITLEBAR_HEIGHT * 1.5d)));
            Dock = DockStyle.Top;
            Load += AppBar_Load;
        }
        ColorScheme colorScheme = DefaultColorSchemes.Blue;

        public ColorScheme ColorScheme {
            get {
                return Parent != null && Parent is MetroForm ? ((MetroForm)Parent).ColorScheme : colorScheme;
            }

            set {
                colorScheme = value;
            }
        }

        private void AppBar_Load(object sender, EventArgs e)
        {
            //The control was drawn.
            //This means we can add the drop shadow
            ShadowUtils.CreateDropShadow(this);
            if (Parent != null)
            {
                Parent.Invalidate();
            }
        }
        #endregion

        #region Variables
        bool hasStartedYet;
        #endregion

        #region Events

        /// <summary>
        /// Called to signal to subscribers that this control loaded
        /// </summary>
        public event EventHandler Load;
        protected virtual void OnLoad(EventArgs e)
        {
            EventHandler eh = Load;

            eh?.Invoke(this, e);
        }
        #endregion

        #region Properties
        public Font TextFont { get; set; } = new Font(SystemFonts.CaptionFont.FontFamily, 14f);
        public Rectangle ControlBounds => new Rectangle(Point.Empty, Size);
        public Rectangle TextRectangle => Rectangle.FromLTRB(XTextOffset, 0, ControlBounds.Right - XTextOffset, ControlBounds.Bottom);
        #endregion


        #region Methods
        int RoundDown(int toRound) => toRound - toRound % 10;
        int RoundUp(int toRound) => (10 - toRound % 10) + toRound;
        #endregion

        #region Overriden Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!hasStartedYet)
            {
                hasStartedYet = true;
                OnLoad(EventArgs.Empty);
            }
            base.OnPaint(e);
        }
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
            using (var primary = new SolidBrush(ColorScheme.PrimaryColor))
            {
                using (var secondary = new SolidBrush(ColorScheme.SecondaryColor))
                {
                    pevent.Graphics.FillRectangle(primary, ControlBounds);
                    GraphicUtils.DrawCenteredText(pevent.Graphics, Parent.Text, TextFont, TextRectangle, ColorScheme.ForegroundColor, false, true);
                }
            }
        }
        #endregion
    }
}
