using NickAc.ModernUIDoneRight.Forms;
using NickAc.ModernUIDoneRight.Objects;
using NickAc.ModernUIDoneRight.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NickAc.ModernUIDoneRight.Controls
{
    public class AppBar : Control
    {

        #region Methods

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (Actions != null) {
                Actions.ForEach(a => {
                    Rectangle rect = a.GetRectangle(this, Actions);
                    if (rect != Rectangle.Empty && rect.Contains(e.Location)) {
                        a.OnClick(EventArgs.Empty);
                    }

                });
            }
        }

        #endregion

        #region Fields

        private ColorScheme colorScheme = DefaultColorSchemes.Blue;
        private bool hasStartedYet;
        private bool iconVisible;

        #endregion

        #region Constructors

        public AppBar()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            Size = new Size(10, RoundUp((int)(ModernForm.DEFAULT_TITLEBAR_HEIGHT * 1.5d)));
            Dock = DockStyle.Top;
            Load += AppBar_Load;
        }

        #endregion

        #region Events

        /// <summary>
        /// Called to signal to subscribers that this control loaded
        /// </summary>
        public event EventHandler Load;

        #endregion

        #region Properties
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<AppAction> Actions { get; set; } = new List<AppAction>();

        public ColorScheme ColorScheme {
            get {
                return Parent != null && Parent is ModernForm ? ((ModernForm)Parent).ColorScheme : colorScheme;
            }

            set {
                colorScheme = value;
            }
        }

        public Rectangle ControlBounds => new Rectangle(Point.Empty, Size);
        public bool IconVisible { get { return iconVisible; } set { iconVisible = value; Invalidate(); } }
        public Font TextFont { get; set; } = new Font(SystemFonts.CaptionFont.FontFamily, 14f);
        public Rectangle TextRectangle => Rectangle.FromLTRB(XTextOffset * (IconVisible ? 2 : 1), 0, ControlBounds.Right - XTextOffset, ControlBounds.Bottom);
        public int XTextOffset => /*Height*/20;

        #endregion

        #region Methods

        protected virtual void OnLoad(EventArgs e)
        {
            EventHandler eh = Load;

            eh?.Invoke(this, e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!hasStartedYet) {
                hasStartedYet = true;
                OnLoad(EventArgs.Empty);
            }
            if (Actions != null) {
                Actions.ForEach(a => {
                    Rectangle rect = a.GetRectangle(this, Actions);
                    if (rect != Rectangle.Empty && a.Image != null) {
                        ControlPaintWrapper.ZoomDrawImage(e.Graphics, a.Image, rect);
                    }

                });
            }
            base.OnPaint(e);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
            using (var primary = new SolidBrush(ColorScheme.PrimaryColor)) {
                using (var secondary = new SolidBrush(ColorScheme.SecondaryColor)) {
                    pevent.Graphics.FillRectangle(primary, ControlBounds);
                    if (IconVisible) {
                        pevent.Graphics.DrawIcon(((Form)Parent).Icon, Rectangle.FromLTRB(XTextOffset / 2, XTextOffset / 2, XTextOffset * 2, Height - (XTextOffset / 2)));
                    }
                    GraphicUtils.DrawCenteredText(pevent.Graphics, Parent.Text, TextFont, TextRectangle, ColorScheme.ForegroundColor, false, true);
                }
            }
        }

        private void AppBar_Load(object sender, EventArgs e)
        {
            //The control was drawn.
            //This means we can add the drop shadow
            ShadowUtils.CreateDropShadow(this);
            if (Parent != null) {
                Parent.Invalidate();
            }
        }
        private int RoundDown(int toRound) => toRound - toRound % 10;

        private int RoundUp(int toRound) => (10 - toRound % 10) + toRound;

        #endregion
    }
}