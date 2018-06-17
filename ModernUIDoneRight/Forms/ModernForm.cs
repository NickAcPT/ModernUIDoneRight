using NickAc.ModernUIDoneRight.Controls;
using NickAc.ModernUIDoneRight.Native;
using NickAc.ModernUIDoneRight.Objects;
using NickAc.ModernUIDoneRight.Objects.Interaction;
using NickAc.ModernUIDoneRight.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NickAc.ModernUIDoneRight.Forms
{
    public class ModernForm : Form
    {

        #region Fields

        /// <summary>
        /// The default height of the titlebar
        /// </summary>
        public const int DefaultTitlebarHeight = 32;

        /// <summary>
        /// The size of the border
        /// </summary>
        public const int SizingBorder = 7;

        private readonly List<ModernTitlebarButton> _titlebarButtons, _nativeTitlebarButtons;
        private ColorScheme _colorScheme;
        private Size _minimumSize = Size.Empty;
        private bool _mouseChanged;
        private WindowHitTestResult _windowHit = WindowHitTestResult.None;

        #endregion

        #region Constructors

        public ModernForm()
        {
            Font = SystemFonts.MessageBoxFont;
            ColorScheme = DefaultColorSchemes.Blue;

            DoubleBuffered = true;
            AutoScaleMode = AutoScaleMode.None;

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ContainerControl, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            _titlebarButtons = new List<ModernTitlebarButton>();
            _nativeTitlebarButtons = GenerateNativeButtons(DefaultTitlebarHeight, this);

            BackColor = Color.White;
            FormBorderStyle = FormBorderStyle.None;
        }

        #endregion

        #region Properties
        
        public int HamburgerButtonSize { get; set; } = 32;

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsSideBarAvailable => !IsAppBarAvailable && Controls.OfType<SidebarControl>().Any();

        
        public Rectangle HamburgerRectangle => IsSideBarAvailable ? Rectangle.FromLTRB(TextBarRectangle.Left - HamburgerButtonSize - 1, 0, TextBarRectangle.Left, TextBarRectangle.Bottom)
            : Rectangle.Empty;

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Rectangle BottomSide => Rectangle.FromLTRB(SizingBorder, FormBounds.Bottom - SizingBorder, FormBounds.Right - SizingBorder, FormBounds.Bottom);

        /// <summary>
        /// The color scheme on this window
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ColorScheme ColorScheme {
            get => _colorScheme;
            set {
                _colorScheme = value;
                Refresh();
            }
        }

        public override Rectangle DisplayRectangle => Rectangle.FromLTRB(TitlebarRectangle.Left, TitlebarRectangle.Bottom, TitlebarRectangle.Right, FormBounds.Bottom - 1);

        /// <summary>
        /// The form bounds rectangle relative to 0,0
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Rectangle FormBounds => new Rectangle(Point.Empty, Size);

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsAppBarAvailable => Controls.OfType<AppBar>().Any();

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Rectangle LeftBottom => Rectangle.FromLTRB(0, FormBounds.Bottom - SizingBorder, SizingBorder, FormBounds.Bottom);

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Rectangle LeftSide => Rectangle.FromLTRB(0, TitlebarRectangle.Bottom, SizingBorder, FormBounds.Bottom - SizingBorder);

        public override Size MaximumSize {
            get => Screen.GetWorkingArea(this).Size;
            set {
                value = Screen.GetWorkingArea(this).Size;
                base.MaximumSize = value;
            }
        }

        public override Size MinimumSize {
            get {
                if (_minimumSize.IsEmpty) {
                    var v = GetTitleBarButtonsWidth();
                    return new Size(v, TitlebarHeight + SizingBorder);
                }
                return _minimumSize;
            }
            set => _minimumSize = value;
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Rectangle RightBottom => Rectangle.FromLTRB(FormBounds.Right - SizingBorder, FormBounds.Bottom - SizingBorder, FormBounds.Right, FormBounds.Bottom);

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Rectangle RightSide => Rectangle.FromLTRB(FormBounds.Right - SizingBorder, TitlebarRectangle.Bottom, FormBounds.Right, FormBounds.Bottom - SizingBorder);

        public ShadowType ShadowType { get; set; } = ShadowType.Default;

        public bool Sizable { get; set; } = true;

        public override string Text {
            get => base.Text; set {
                base.Text = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Rectangle that represents the titlebar text
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Rectangle TextBarRectangle => Rectangle.FromLTRB((IsSideBarAvailable ? HamburgerButtonSize : 0) + TitlebarRectangle.Left, TitlebarRectangle.Top, TitlebarRectangle.Right - GetTitleBarButtonsWidth(), TitlebarRectangle.Bottom);

        public List<ModernTitlebarButton> TitlebarButtons => _titlebarButtons;

        /// <summary>
        /// Rectangle that represents all caption/titlebar buttons
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual Rectangle TitlebarButtonsRectangle {
            get {
                var btnWidth = GetTitleBarButtonsWidth();
                var titlebarRect = TitlebarRectangle;
                return Rectangle.FromLTRB(titlebarRect.Right - btnWidth, titlebarRect.Top, titlebarRect.Right, titlebarRect.Bottom);
            }
        }

        /// <summary>
        /// The font used in the titlebar
        /// </summary>
        public Font TitleBarFont { get; set; } = SystemFonts.CaptionFont;

        /// <summary>
        /// The actual height of the titlebar
        /// </summary>
        public int TitlebarHeight { get; set; } = DefaultTitlebarHeight;

        /// <summary>
        /// Rectangle that represents the complete titlebar
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual Rectangle TitlebarRectangle => Rectangle.FromLTRB(1, 1, FormBounds.Right - 1, TitlebarVisible ? TitlebarHeight + 1 : 1);

        public bool TitlebarVisible { get; set; } = true;

        private List<ModernTitlebarButton> NativeTitlebarButtons => _nativeTitlebarButtons;

        #endregion

        #region Methods

        public void HandleMouseMoveAndChild(Control c)
        {
            //Listen to mouse events
            c.MouseDown += HandleMouseEventHandler;
            c.MouseMove += MouseMoveEvent;
            foreach (Control c2 in c.Controls) {
                //Do the same for child controls
                //(Recursive method call)
                HandleMouseMoveAndChild(c2);
            }
        }

        public WindowHitTestResult HitTest(Point loc)
        {
            return HitTest(loc, Point.Empty);
        }

        public WindowHitTestResult HitTest(Point loc, Point offset)
        {
            var negativeOffset = new Point(-offset.X, -offset.Y);
            if (TitlebarButtonsRectangle.OffsetAndReturn(negativeOffset).Contains(loc))
                return WindowHitTestResult.TitleBarButtons;

            if (TextBarRectangle.OffsetAndReturn(negativeOffset).Contains(loc))
                return WindowHitTestResult.TitleBar;

            if (!Sizable)
                return WindowHitTestResult.None;

            if (LeftBottom.OffsetAndReturn(negativeOffset).Contains(loc))
                return WindowHitTestResult.BottomLeft;

            if (LeftSide.OffsetAndReturn(negativeOffset).Contains(loc))
                return WindowHitTestResult.Left;

            if (BottomSide.OffsetAndReturn(negativeOffset).Contains(loc))
                return WindowHitTestResult.Bottom;

            if (RightBottom.OffsetAndReturn(negativeOffset).Contains(loc))
                return WindowHitTestResult.BottomRight;

            if (RightSide.OffsetAndReturn(negativeOffset).Contains(loc))
                return WindowHitTestResult.Right;

            return WindowHitTestResult.None;
        }

        public void UnhandleMouseMoveAndChild(Control c)
        {
            //Remove mouse events listeners
            c.MouseDown -= HandleMouseEventHandler;
            c.MouseMove -= MouseMoveEvent;
            foreach (Control c2 in c.Controls) {
                //Do the same for child controls
                //(Recursive method call)
                UnhandleMouseMoveAndChild(c2);
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            //Detect when control is added and handle form resizing
            HandleMouseMoveAndChild(e.Control);
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            //Detect when control is removed and remove all mouse listeners
            UnhandleMouseMoveAndChild(e.Control);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode) {
                if (Location.IsEmpty) CenterToScreen();

                //Check if we can use the aero shadow
                if ((ShadowType.Equals(ShadowType.AeroShadow) || ShadowType.Equals(ShadowType.Default)) && DwmNative.ExtendFrameIntoClientArea(this, 0, 0, 0, 1)) {
                    //We can! Tell windows to allow the rendering to happen on our borderless form
                    DwmNative.AllowRenderInBorderless(this);
                } else if (ShadowType.Equals(ShadowType.Default) || ShadowType.Equals(ShadowType.FlatShadow)) {
                    //No aero for us! We must create the typical flat shadow.
                    new ShadowForm().Show(this);
                } 
                //else if (ShadowType.Equals(ShadowType.GlowShadow))
                //{
                //    var glowShadowForm = new PerPixelAlphaForm()
                //    {/*
                //        ShadowBlur = 5,
                //        ShadowSpread = 5,
                //        ShadowColor = ColorScheme.PrimaryColor*/
                //    };
                //    using (var bmp = new Bitmap(Width + 10, Height + 10))
                //    {
                //        using (var g = Graphics.FromImage(bmp))
                //        {
                //            g.FillRectangle(Brushes.Black, 0, 0, bmp.Width, bmp.Height);
                //        }
                //        glowShadowForm.SetBitmap(bmp);
                //    }

                //    glowShadowForm.Location = new Point(Location.X - 5, Location.Y - 5);
                //    glowShadowForm.Show();
                //    //SizeChanged += (s, ee) => glowShadowForm.RefreshShadow();
                //    //glowShadowForm.RefreshShadow();
                //}
            }
            base.OnLoad(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (_windowHit != WindowHitTestResult.None && _windowHit != WindowHitTestResult.TitleBar && _windowHit != WindowHitTestResult.TitleBarButtons && Sizable) {
                _windowHit = WindowHitTestResult.None;
                return;
            }
            var titlebarButtonOffset = 0;
            switch (HitTest(e.Location)) {
                case WindowHitTestResult.TitleBarButtons:
                    titlebarButtonOffset = 0;
                    HandleTitlebarButtonClick(e, ref titlebarButtonOffset, NativeTitlebarButtons);
                    HandleTitlebarButtonClick(e, ref titlebarButtonOffset, TitlebarButtons);
                    break;
            }
            if (IsSideBarAvailable && HamburgerRectangle.Contains(e.Location))
                Controls.OfType<SidebarControl>().All(ToggleSidebar);
        }

        private static bool ToggleSidebar(SidebarControl c)
        {
            if (c.IsClosed)
            {
                c.ShowSidebar();
            }
            else
                c.HideSidebar();
            return true;
        }

        private bool _isMouseDown;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _isMouseDown = true;
            var hitResult = HitTest(e.Location);

            _windowHit = hitResult;

            if (hitResult == WindowHitTestResult.TitleBar) {
                if (e.Button == MouseButtons.Left && e.Clicks == 2 && MaximizeBox)
                {
                    WindowState = (WindowState == FormWindowState.Normal
                        ? FormWindowState.Maximized
                        : FormWindowState.Normal);
                    return;
                }
                FormUtils.StartFormDragFromTitlebar(this);
            } else if (hitResult != WindowHitTestResult.TitleBarButtons && hitResult != WindowHitTestResult.None && Sizable) {
                if (!Sizable) return;
                FormUtils.StartFormResizeFromEdge(this, FormUtils.ConvertToResizeResult(hitResult));
            } 
            Invalidate(TitlebarButtonsRectangle);
            if (IsSideBarAvailable)
                Invalidate(HamburgerRectangle);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (DesignMode) return;
            if (_mouseChanged) {
                Cursor = Cursors.Default;
                _mouseChanged = false;
            }
            Invalidate(TitlebarButtonsRectangle);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (DesignMode) return;
            if (_mouseChanged) {
                Cursor = Cursors.Default;
                _mouseChanged = false;
            }
            Invalidate(TitlebarButtonsRectangle);
            if (IsSideBarAvailable)
                Invalidate(HamburgerRectangle);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (DesignMode) return;
            var hitResult = HitTest(e.Location);
            var resizeResult = FormUtils.ConvertToResizeResult(hitResult);
            var cursor = FormUtils.HitTestToCursor(resizeResult);
            if (WindowState != FormWindowState.Maximized) {
                if (_mouseChanged) {
                    base.Cursor = cursor;
                }
                _mouseChanged = true /*!cursor.Equals(Cursors.Default)*/;
            }
            Invalidate(TitlebarButtonsRectangle);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _isMouseDown = false;
            if (_mouseChanged) {
                Cursor = Cursors.Default;
                _mouseChanged = false;
            }
            Invalidate(TitlebarButtonsRectangle);
            if (IsSideBarAvailable)
                Invalidate(HamburgerRectangle);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            var curLoc = PointToClient(Cursor.Position);

            using (var primary = new SolidBrush(ColorScheme.PrimaryColor)) {
                using (var secondary = new SolidBrush(ColorScheme.SecondaryColor)) {
                    using (var mouseDownColor = new SolidBrush(ColorScheme.MouseDownColor))
                    {
                        using (var mouseHoverColor = new SolidBrush(ColorScheme.MouseHoverColor))
                        {
                            //Draw titlebar
                            if (TitlebarVisible)
                                e.Graphics.FillRectangle(IsAppBarAvailable ? secondary : primary, TitlebarRectangle);
                            //Draw form border
                            GraphicUtils.DrawRectangleBorder(FormBounds, e.Graphics, ColorScheme.SecondaryColor);

                            if (!TitlebarVisible)
                                return;
                            //Start rendering the titlebar buttons
                            var titlebarButtonOffset = 0;
                            titlebarButtonOffset = RenderTitlebarButtons(e, curLoc, mouseDownColor, mouseHoverColor,
                                NativeTitlebarButtons, ref titlebarButtonOffset);
                            titlebarButtonOffset = RenderTitlebarButtons(e, curLoc, mouseDownColor, mouseHoverColor, TitlebarButtons,
                                ref titlebarButtonOffset);
                            //Dectect if an app bar is available.
                            //If it is, draw the window title.
                            if (!IsAppBarAvailable ||
                                (Controls.OfType<AppBar>().FirstOrDefault()?.OverrideParentText ?? false))
                            {
                                GraphicUtils.DrawCenteredText(e.Graphics, Text, TitleBarFont,
                                    Rectangle.FromLTRB(TextBarRectangle.Left + SizingBorder, TextBarRectangle.Top,
                                        TextBarRectangle.Right - SizingBorder, TextBarRectangle.Bottom),
                                    ColorScheme.ForegroundColor, false, true);
                            }

                            if (IsSideBarAvailable)
                                GraphicUtils.DrawHamburgerButton(e.Graphics, secondary, HamburgerRectangle,
                                    ColorScheme.ForegroundColor, this, true);

                        }
                    }

                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var oldClip = e.Graphics.Clip;
            e.Graphics.SetClip(DisplayRectangle);
            base.OnPaint(e);
            e.Graphics.SetClip(oldClip.GetBounds(e.Graphics));
            oldClip.Dispose();
        }

        /// <summary>
        /// This method adds the default titlebar buttons.
        /// </summary>
        /// <param name="width">Button width</param>
        /// <param name="parent">Form containing the buttons</param>
        /// <returns></returns>
        private static List<ModernTitlebarButton> GenerateNativeButtons(int width, Form parent)
        {

            //Create a temporary list
            var list = new List<ModernTitlebarButton>
            {
                new NativeTitlebarButton(parent, width, NativeTitlebarButton.TitlebarAction.Close),
                new NativeTitlebarButton(parent, width, NativeTitlebarButton.TitlebarAction.Maximize),
                new NativeTitlebarButton(parent, width, NativeTitlebarButton.TitlebarAction.Minimize)
            };

            return list;
        }

        /// <summary>
        /// Get rectangle of a button
        /// </summary>
        /// <param name="offset">Offset</param>
        /// <param name="btn">The button</param>
        /// <returns></returns>
        private Rectangle GetTitlebarButtonRectangle(int offset, ModernTitlebarButton btn)
        {
            return Rectangle.FromLTRB(TitlebarRectangle.Right - btn.Width - offset, 0, TitlebarRectangle.Right - offset, TitlebarRectangle.Bottom);
        }

        /// <summary>
        /// Get total width of all the caption buttons
        /// </summary>
        /// <returns></returns>
        private int GetTitleBarButtonsWidth()
        {
            var titlebarButtonOffset = 0;
            for (var i = 0; i < NativeTitlebarButtons.Count; i++) {
                var btn = NativeTitlebarButtons[i];
                if (!btn.Visible) continue;
                var rect = GetTitlebarButtonRectangle(titlebarButtonOffset, btn);
                titlebarButtonOffset += btn.Width;
            }
            for (var i = 0; i < TitlebarButtons.Count; i++) {
                var btn = TitlebarButtons[i];
                if (!btn.Visible) continue;
                var rect = GetTitlebarButtonRectangle(titlebarButtonOffset, btn);
                titlebarButtonOffset += btn.Width;
            }
            return titlebarButtonOffset;
        }

        private void HandleMouseEventHandler(object sender, MouseEventArgs e)
        {
            //Get the control
            var c = (Control)sender;
            //Check if the window isn't and there isn't more than one click
            if ((WindowState != FormWindowState.Maximized) && e.Clicks != 1) return;
            //Check where clicked
            var result = HitTest(e.Location, c.Location);
            //Invoke method that will start firm resizing
            if (Sizable)
                FormUtils.StartFormResizeFromEdge(this, FormUtils.ConvertToResizeResult(result), c);
        }

        private int HandleTitlebarButtonClick(MouseEventArgs e, ref int titlebarButtonOffset, List<ModernTitlebarButton> buttons)
        {
            for (var i = 0; i < buttons.Count; i++) {
                var btn = buttons[i];
                if (!btn.Visible) continue;
                var rect = GetTitlebarButtonRectangle(titlebarButtonOffset, btn);
                titlebarButtonOffset += btn.Width;
                if (rect.Contains(e.Location)) {
                    btn.OnClick(e);
                }
            }

            return titlebarButtonOffset;
        }

        private void MouseMoveEvent(object sender, MouseEventArgs e)
        {
            //Get the control
            var c = ((Control)sender);

            //Check if window is maximized, if it is, stop!
            if (WindowState == FormWindowState.Maximized) return;
            //Try to see where the mouse was
            var result = HitTest(e.Location, c.Location);
            var cur = FormUtils.HitTestToCursor(FormUtils.ConvertToResizeResult(result));
            //Change the mouse accordingly
            if (!c.Cursor.Equals(cur) && _mouseChanged) {
                c.Cursor = cur;
                _mouseChanged = !cur.Equals(Cursors.Default);
            }
        }
        private int RenderTitlebarButtons(PaintEventArgs e, Point curLoc, SolidBrush secondaryDown, SolidBrush secondaryHover, IEnumerable<ModernTitlebarButton> buttons, ref int titlebarButtonOffset)
        {
            foreach (var btn in buttons)
            {
                if (!btn.Visible) continue;
                var rect = GetTitlebarButtonRectangle(titlebarButtonOffset, btn);
                if (rect.Contains(curLoc) && !DesignMode)
                    e.Graphics.FillRectangle(_isMouseDown ? secondaryDown : secondaryHover, rect);
                GraphicUtils.DrawCenteredText(e.Graphics, btn.Text, btn.Font, rect, ColorScheme.ForegroundColor);
                titlebarButtonOffset += btn.Width;
            }

            return titlebarButtonOffset;
        }

        private new void ResetBackColor()
        {
            BackColor = Color.White;
        }

        private void ResetColorScheme()
        {
            ColorScheme = DefaultColorSchemes.Blue;
        }

        #endregion
        
    }
}