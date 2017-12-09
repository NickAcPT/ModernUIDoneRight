﻿using NickAc.ModernUIDoneRight.Forms;
using NickAc.ModernUIDoneRight.Objects;
using NickAc.ModernUIDoneRight.Objects.MenuItems;
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<AppBarMenuItem> MenuItems { get; set; } = new List<AppBarMenuItem>();

        public Font TextFont { get; set; } = new Font(SystemFonts.CaptionFont.FontFamily, 14f);

        public Rectangle TextRectangle => Rectangle.FromLTRB(XTextOffset * (IconVisible ? 2 : 1), 0, ControlBounds.Right - XTextOffset, ControlBounds.Bottom);

        public int XTextOffset => 20;

        #endregion

        #region Methods

        public Rectangle GetMenuRectangle()
        {
            int index = 0;
            int xTextOffset = XTextOffset;
            int size = Height - xTextOffset;
            int xTextOffsetHalf = XTextOffset / 2;
            int right = Width - xTextOffsetHalf - (index * size + xTextOffsetHalf * index);
            return Rectangle.FromLTRB(right - size, xTextOffsetHalf, right, Height - xTextOffsetHalf);
        }

        protected virtual void OnLoad(EventArgs e)
        {
            EventHandler eh = Load;

            eh?.Invoke(this, e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            Rectangle rectangle = GetMenuRectangle();
            if (MenuItems != null && MenuItems.Count > 0 && rectangle.Contains(e.Location)) {
                //Open new "window" to act as menu
                Color splitterColor = Color.Gray;
                const float splitterPercentage = 0.75f;
                var form = new ModernForm
                {
                    Capture = true,
                    TitlebarVisible = false,
                    Tag = MenuItems,
                    Text = "ModernMenu",
                    ShowInTaskbar = false,
                    TopMost = true,
                    Sizable = false
                };

                bool mouseDown = false;
                form.MouseDown += (s, ee) => {
                    mouseDown = true;
                    form.Invalidate();
                };
                form.MouseUp += (s, ee) => {
                    mouseDown = false;
                    form.Invalidate();
                };
                form.MouseMove += (s, ee) => {
                    form.Refresh();
                };

                form.Click += (s, ee) => {
                    //Check the click
                    if (form.Tag != null) {
                        if (form.Tag is List<AppBarMenuItem> items) {
                            using (var g = form.CreateGraphics()) {
                                int yOffset = 0;
                                foreach (var item in items) {
                                    Size itemSize = item.GetSize(Font, g);
                                    Rectangle rect = new Rectangle(new Point(form.DisplayRectangle.Left, yOffset + form.DisplayRectangle.Top), new Size(itemSize.Width, itemSize.Height - 1));

                                    if (rect.Contains(form.PointToClient(Cursor.Position))) {
                                        item.PerformClick();
                                        return;
                                    }
                                    yOffset += itemSize.Height;
                                }
                            }
                        }
                    }
                };

                form.Deactivate += (s, ee) => form.Close();
                form.Paint += (s, ee) => {
                    //Draw the menu
                    int yOffset = 0;
                    using (var splitterPen = new Pen(splitterColor)) {
                        for (int i = 0, MenuItemsCount = MenuItems.Count; i < MenuItemsCount; i++) {
                            var item = MenuItems[i];
                            Size itemSize = item.GetSize(Font, ee.Graphics);
                            Rectangle rect = new Rectangle(new Point(form.DisplayRectangle.Left, yOffset + form.DisplayRectangle.Top), new Size(itemSize.Width, itemSize.Height - 1));

                            if (mouseDown && rect.Contains(form.PointToClient(Cursor.Position))) {
                                ee.Graphics.FillRectangle(Brushes.LightGray, rect);
                            }
                            item.DrawItem(ee.Graphics, rect, Font);
                            if (i < MenuItemsCount - 1) {
                                int lineWidth = (int)(form.Width * splitterPercentage);
                                int side = (form.Width - lineWidth) / 2;
                                ee.Graphics.DrawLine(splitterPen, side, (yOffset + (itemSize.Height)), form.Width - side, (yOffset + (itemSize.Height)));
                            }
                            yOffset += itemSize.Height;
                        }
                    }
                };

                int maxWidth = -1;
                int maxHeight = -1;
                using (Graphics g = CreateGraphics()) {
                    foreach (var item in MenuItems) {
                        var size = item.GetSize(Font, g);

                        maxWidth = Math.Max(maxWidth, size.Width);
                        maxHeight += size.Height;
                    }
                }
                form.Size = new Size(maxWidth + 2, maxHeight + 2);
                Point screenLoc = PointToScreen(new Point(rectangle.Right, rectangle.Top));
                form.Location = new Point(screenLoc.X - maxWidth, screenLoc.Y);
                form.Show(this.Parent);
            }

            if (Actions != null) {
                Actions.ForEach(a => {
                    Rectangle rect = a.GetRectangle(this, Actions);
                    if (rect != Rectangle.Empty && rect.Contains(e.Location)) {
                        a.OnClick(EventArgs.Empty);
                    }
                });
            }
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
                    using (var foreColor = new SolidBrush(ColorScheme.ForegroundColor)) {
                        pevent.Graphics.FillRectangle(primary, ControlBounds);
                        if (IconVisible) {
                            pevent.Graphics.DrawIcon(((Form)Parent).Icon, Rectangle.FromLTRB(XTextOffset / 2, XTextOffset / 2, XTextOffset * 2, Height - (XTextOffset / 2)));
                        }

                        GraphicUtils.DrawCenteredText(pevent.Graphics, Parent.Text, TextFont, TextRectangle, ColorScheme.ForegroundColor, false, true);

                        if (MenuItems != null && MenuItems.Count > 0) {
                            //Draw menu icon
                            Rectangle rect = GetMenuRectangle();
                            const int circleRadius = 2;
                            const int interval = 3;
                            int centerX = rect.Right - (rect.Width / 2);
                            int centerY = rect.Bottom - (rect.Height / 2);
                            int topCircle = centerY - (circleRadius * 2) - interval;
                            int bottomCircle = centerY + (circleRadius * 2) + interval;

                            var oldMode = pevent.Graphics.SmoothingMode;
                            pevent.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                            //Top
                            pevent.Graphics.FillEllipse(foreColor, centerX - circleRadius, topCircle - circleRadius, circleRadius * 2, circleRadius * 2);

                            //Middle
                            pevent.Graphics.FillEllipse(foreColor, centerX - circleRadius, centerY - circleRadius, circleRadius * 2, circleRadius * 2);

                            //Bottom
                            pevent.Graphics.FillEllipse(foreColor, centerX - circleRadius, bottomCircle - circleRadius, circleRadius * 2, circleRadius * 2);

                            pevent.Graphics.SmoothingMode = oldMode;
                        }
                    }
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