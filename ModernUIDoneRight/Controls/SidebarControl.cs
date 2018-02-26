using NickAc.ModernUIDoneRight.Forms;
using NickAc.ModernUIDoneRight.Objects;
using NickAc.ModernUIDoneRight.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static NickAc.ModernUIDoneRight.Utils.Animation;
using static NickAc.ModernUIDoneRight.Utils.ShadowUtils;

namespace NickAc.ModernUIDoneRight.Controls
{
    /// <summary>
    /// The sidebar control (use with AppBar)
    /// </summary>
    public class SidebarControl : Control, IShadowController
    {
        public ColorScheme ColorScheme {
            get {
                return Parent != null && Parent is ModernForm ? ((ModernForm)Parent).ColorScheme : colorScheme;
            }
            set {
                colorScheme = value;
            }
        }
        public abstract class SideBarItem
        {
            /// <summary>
            /// Called to signal to subscribers that the item was clicked
            /// </summary>
            public event MouseEventHandler Click;

            public abstract void DrawItem(SidebarControl c, Graphics g, Size itemSize, bool isSelected = false);
            public abstract void MeasureItem(SidebarControl c, Graphics g, out int itemHeight);

            public virtual void OnClick(MouseEventArgs e)
            {
                MouseEventHandler eh = Click;
                eh?.Invoke(this, e);
            }
        }

        private bool isAnimating;
        private bool wasPainted;
        private ColorScheme colorScheme;

        private int OriginalWidth { get; set; } = -1;
        public bool IsClosed { get; set; }
        public int TopBarSize { get; set; } = 100;
        public Color TopBarColor { get; set; } = Color.FromArgb(189, 189, 189);
        public int TopBarSpacing { get; set; } = 32;


        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
            if (TopBarSize > 0) {
                using (var sB = new SolidBrush(TopBarColor)) {
                    pevent.Graphics.FillRectangle(sB, 0, 0, Width, TopBarSize);
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public IList<SideBarItem> Items { get; set; } = new List<SideBarItem>();
        public SidebarControl()
        {
            Dock = DockStyle.Left;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (!wasPainted) {
                wasPainted = true;
                this.CreateDropShadow();
            }
            if (IsClosed || isAnimating) return;

            int yPos = TopBarSize + TopBarSpacing;
            using (var altColor = new SolidBrush(ColorScheme.SecondaryColor)) {
                foreach (var item in Items) {
                    int height = -1;
                    item.MeasureItem(this, e.Graphics, out height);
                    if (height < 1) continue;
                    Rectangle itemRect = new Rectangle(0, yPos, Width, height);

                    using (var bmp = new Bitmap(Width, height)) {
                        using (var g = Graphics.FromImage(bmp)) {
                            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                            bool mouseHere = itemRect.Contains(PointToClient(Cursor.Position)) && MouseButtons == MouseButtons.Left;
                            var oldForeColor = ForeColor;
                            if (mouseHere) {
                                g.FillRectangle(altColor, 0, 0, Width, height);
                            }
                            item.DrawItem(this, g, new Size(Width, height), mouseHere);
                        }
                        e.Graphics.DrawImageUnscaled(bmp, 0, yPos);
                        yPos += height;
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
            int yPos = TopBarSize + TopBarSpacing;
            using (var bmpOrig = new Bitmap(Width, Height)) {
                using (var g = Graphics.FromImage(bmpOrig)) {
                    foreach (var item in Items) {
                        int height = -1;
                        item.MeasureItem(this, g, out height);
                        if (height < 1) continue;
                        Rectangle itemRect = new Rectangle(0, yPos, Width, height);

                        bool mouseHere = itemRect.Contains(PointToClient(Cursor.Position)) && MouseButtons == MouseButtons.Left;
                        if (mouseHere) {
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
            var originalRect = Rectangle.FromLTRB(Bounds.Left, Bounds.Top, Bounds.Right + (8 * 2), Bounds.Bottom);
            if (OriginalWidth == -1)
                OriginalWidth = Width;
            Width = 0;
            IsClosed = false;
            Animation animIn = new AnimationBuilder()
                .WithAction((a) => {
                    Width++;
                })
                .WithCountLimit(OriginalWidth)
                .WithMultiplier(7)
                .WithInterval(8)
                .Build();
            Show();
            animIn.End((a) => {
                isAnimating = false;
                Parent?.Refresh();
            });
            isAnimating = true;
            Parent?.Invalidate(originalRect);
            animIn.Start();

        }
        public void HideSidebar()
        {
            if (IsClosed) return;
            var originalRect = Rectangle.FromLTRB(Bounds.Left, Bounds.Top, Bounds.Right + (8 * 2), Bounds.Bottom);
            if (OriginalWidth == -1)
                OriginalWidth = Width;
            IsClosed = true;
            Animation animOut = new AnimationBuilder()
                .WithAction((a) => {
                    Width--;
                })
                .WithCountLimit(OriginalWidth)
                .WithMultiplier(7)
                .WithInterval(8)
                .Build();
            animOut.End((a) => { Hide(); isAnimating = false; });
            isAnimating = true;
            Parent?.Invalidate(originalRect);
            animOut.Start();
        }

        public bool ShouldShowShadow()
        {
            return !isAnimating;
        }
    }
}
