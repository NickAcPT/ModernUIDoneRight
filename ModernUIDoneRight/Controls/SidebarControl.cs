using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using NickAc.ModernUIDoneRight.Forms;
using NickAc.ModernUIDoneRight.Objects;
using static NickAc.ModernUIDoneRight.Utils.Animation;
using static NickAc.ModernUIDoneRight.Utils.ShadowUtils;

namespace NickAc.ModernUIDoneRight.Controls
{
    /// <summary>
    ///     The sidebar control (use with AppBar)
    /// </summary>
    public class SidebarControl : Control, IShadowController
    {
        private ColorScheme _colorScheme;

        private bool _isAnimating;
        private bool _wasPainted;

        public SidebarControl()
        {
            Dock = DockStyle.Left;
        }

        public ColorScheme ColorScheme
        {
            get => Parent is ModernForm ? ((ModernForm) Parent).ColorScheme : _colorScheme;
            set => _colorScheme = value;
        }

        private int OriginalWidth { get; set; } = -1;
        public bool IsClosed { get; set; }
        public int TopBarSize { get; set; } = 100;
        public Color TopBarColor { get; set; } = Color.FromArgb(189, 189, 189);
        public int TopBarSpacing { get; set; } = 32;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public IList<SideBarItem> Items { get; set; } = new List<SideBarItem>();

        public bool ShouldShowShadow()
        {
            return !(_isAnimating || IsClosed);
        }

        /// <summary>
        ///     Called to signal to subscribers that sidebar was opened
        /// </summary>
        public event EventHandler SidebarOpen;

        protected virtual void OnSidebarOpen(EventArgs e)
        {
            var eh = SidebarOpen;

            eh?.Invoke(this, e);
        }


        /// <summary>
        ///     Called to signal to subscribers that sidebar was closed
        /// </summary>
        public event EventHandler SidebarClose;

        protected virtual void OnSidebarClose(EventArgs e)
        {
            var eh = SidebarClose;

            eh?.Invoke(this, e);
        }


        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
            if (TopBarSize <= 0) return;
            using (var sB = new SolidBrush(TopBarColor))
            {
                pevent.Graphics.FillRectangle(sB, 0, 0, Width, TopBarSize);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (!_wasPainted)
            {
                if (IsClosed && !DesignMode)
                {
                    OriginalWidth = Width;
                    Width = 0;
                }

                _wasPainted = true;
                this.CreateDropShadow();
            }

            if (IsClosed || _isAnimating) return;

            var yPos = TopBarSize + TopBarSpacing;
            using (var altColor = new SolidBrush(ColorScheme.SecondaryColor))
            {
                using (var backColor = new SolidBrush(BackColor))
                {
                    foreach (var item in Items)
                    {
                        item.MeasureItem(this, e.Graphics, out var height);
                        if (height < 1) continue;
                        var itemRect = new Rectangle(0, yPos, Width, height);

                        using (var bmp = new Bitmap(Width, height))
                        {
                            using (var g = Graphics.FromImage(bmp))
                            {
                                var mouseHere = itemRect.Contains(PointToClient(Cursor.Position)) &&
                                                MouseButtons == MouseButtons.Left;
                                g.FillRectangle(mouseHere ? altColor : backColor, 0, 0, Width, height);
                                item.DrawItem(this, g, new Size(Width, height), mouseHere);
                            }

                            e.Graphics.DrawImageUnscaled(bmp, 0, yPos);
                            yPos += height;
                        }
                    }
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Refresh();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Refresh();
            var yPos = TopBarSize + TopBarSpacing;
            using (var bmpOrig = new Bitmap(Width, Height))
            {
                using (var g = Graphics.FromImage(bmpOrig))
                {
                    foreach (var item in Items)
                    {
                        item.MeasureItem(this, g, out var height);
                        if (height < 1) continue;
                        var itemRect = new Rectangle(0, yPos, Width, height);

                        var mouseHere = itemRect.Contains(e.Location) && e.Button == MouseButtons.Left;
                        if (mouseHere)
                        {
                            item.OnClick(e);
                            return;
                        }

                        yPos += height;
                    }
                }
            }
        }

        public void ShowSidebar()
        {
            if (!IsClosed) return;
            var originalRect = Rectangle.FromLTRB(Bounds.Left, Bounds.Top, Bounds.Right + 8 * 2, Bounds.Bottom);
            if (OriginalWidth == -1)
                OriginalWidth = Width;
            Width = 0;
            IsClosed = false;
            var animIn = new AnimationBuilder()
                .WithAction(a => { Width++; })
                .WithCountLimit(OriginalWidth)
                .WithMultiplier(7)
                .WithInterval(8)
                .Build();
            Show();
            animIn.End(a =>
            {
                _isAnimating = false;
                Parent?.ResumeLayout();
                Parent?.Refresh();
                OnSidebarOpen(EventArgs.Empty);
            });
            _isAnimating = true;
            Parent?.Invalidate(originalRect);
            Parent?.SuspendLayout();
            animIn.Start();
        }

        public void HideSidebar()
        {
            if (IsClosed) return;
            var originalRect = Rectangle.FromLTRB(Bounds.Left, Bounds.Top, Bounds.Right + 8 * 2, Bounds.Bottom);
            if (OriginalWidth == -1)
                OriginalWidth = Width;
            IsClosed = true;
            var animOut = new AnimationBuilder()
                .WithAction(a => Width--)
                .WithCountLimit(OriginalWidth)
                .WithMultiplier(7)
                .WithInterval(8)
                .Build();
            animOut.End(a =>
            {
                Hide();
                _isAnimating = false;
                Parent?.ResumeLayout();
                OnSidebarClose(EventArgs.Empty);
            });
            _isAnimating = true;
            Parent?.Invalidate(originalRect);
            Parent?.SuspendLayout();
            animOut.Start();
        }

        public abstract class SideBarItem
        {
            /// <summary>
            ///     Called to signal to subscribers that the item was clicked
            /// </summary>
            public event MouseEventHandler Click;

            public abstract void DrawItem(SidebarControl c, Graphics g, Size itemSize, bool isSelected = false);
            public abstract void MeasureItem(SidebarControl c, Graphics g, out int itemHeight);

            public virtual void OnClick(MouseEventArgs e)
            {
                var eh = Click;
                eh?.Invoke(this, e);
            }
        }
    }
}