using NickAc.ModernUIDoneRight.Objects;
using NickAc.ModernUIDoneRight.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NickAc.ModernUIDoneRight.Controls
{
    /// <summary>
    /// TilePanel Reborn.
    /// </summary>
    public class TilePanelReborn : Control
    {
        #region Fields

        private bool brandedTile;

        private bool canBeHovered;

        private bool checkable;

        private bool hasDrawn;

        private Image image;

        private bool isHovered;

        private Color lightBackColor;

        private Color lightlightBackColor;

        private List<TileText> texts = new List<TileText>();

        #endregion

        #region Constructors

        public TilePanelReborn()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        #endregion

        #region Properties
        public override Color BackColor {
            get {
                return base.BackColor;
            }
            set {
                base.BackColor = value;
                lightBackColor = ControlPaint.Light(value);
                lightlightBackColor = ControlPaint.LightLight(value);
            }
        }

        public bool BrandedTile {
            get {
                return brandedTile;
            }
            set {
                brandedTile = value;
                Refresh();
            }
        }

        public bool CanBeHovered {
            get {
                return canBeHovered;
            }
            set {
                canBeHovered = value;
                //UpdateParentHoverEvent(value);
            }
        }

        public bool Checkable {
            get { return checkable; }
            set {
                checkable = value;
                UpdateSurface();
                Refresh();
            }
        }

        public bool Flat { get; set; }
        public Image Image {
            get {
                return image;
            }
            set {
                image = value;
                UpdateSurface();
                Refresh();
            }
        }

        [Browsable(true)]
        public override string Text {
            get {
                return base.Text;
            }
            set {
                base.Text = value;
                Refresh();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<TileText> Texts {
            get {
                return texts;
            }
            set {
                texts = value;
            }
        }

        #endregion

        #region Methods

        public Rectangle GetImageRect()
        {
            if (brandedTile)
                return DisplayRectangle;

            return new Rectangle(0, 0, GetPercentage(Width, 50), GetPercentage(Height, 50));
        }

        public Rectangle GetOuterRectangle()
        {
            var rectangle = Bounds;
            rectangle.Inflate(3, 3);
            return rectangle;
        }

        public int GetPercentage(int size, int percent)
        {
            return size * percent / 100;
        }

        public Rectangle GetTextRectangle()
        {
            var rectangle = new Rectangle(0, Height - 32, Width, 32);
            rectangle.Inflate(-8, -8);
            return rectangle;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            isHovered = true;
            if (CanBeHovered) {
                Parent.Invalidate(GetOuterRectangle());
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            isHovered = false;
            if (CanBeHovered) {
                Parent.Invalidate(GetOuterRectangle());
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!hasDrawn) {
                hasDrawn = true;
                OnLoad();
            }
            //Draw the outer Rectangle
            using (var solidBrush = new SolidBrush(lightlightBackColor)) {
                e.Graphics.FillRectangle(solidBrush, DisplayRectangle);
            }

            //Draw the inside color
            Rectangle displayRectangle = DisplayRectangle;
            displayRectangle.Inflate(-1, -1);
            using (var solidBrush = new SolidBrush(BackColor)) {
                e.Graphics.FillRectangle(solidBrush, DisplayRectangle);
            }
            if (!Flat) {
                //Draw gradient
                using (LinearGradientBrush brush = new LinearGradientBrush(displayRectangle, Color.FromArgb(75, 0, 0, 0), Color.FromArgb(7, 0, 0, 0), LinearGradientMode.Horizontal)) {
                    e.Graphics.FillRectangle(brush, displayRectangle);
                }
            }
            else {
                using (SolidBrush sb = new SolidBrush(Color.FromArgb(35, Color.Black))) {
                    e.Graphics.FillRectangle(sb, displayRectangle);
                }
            }

            if (Image != null) {
                Rectangle imgRect = GetImageRect();
                //Draw Image
                if (!brandedTile) {
                    ControlPaintWrapper.ZoomDrawImage(e.Graphics, Image, imgRect.Center(Rectangle.FromLTRB(displayRectangle.Left, displayRectangle.Top, displayRectangle.Right, ((string.Empty.Equals(Text.Trim()) ? displayRectangle.Bottom : GetTextRectangle().Top)))));
                }
                else {
                    e.Graphics.DrawImage(image, imgRect);
                }
            }
            if (!brandedTile) {
                using (var sb = new SolidBrush(ForeColor)) {
                    using (var tF = ControlPaintWrapper.CreateStringFormat(this, ContentAlignment.BottomLeft, false)) {
                        e.Graphics.DrawString(Text, Font, sb, GetTextRectangle(), tF);
                        foreach (var t in texts) {
                            if (t != null) {
                                e.Graphics.DrawString(t.Text, t.Font, sb, t.Location.X, t.Location.Y);
                            }
                        }
                    }
                }
            }
        }

        protected void OnPaintOuterRectParent(object sender, PaintEventArgs pevent)
        {
            if (isHovered) {
                using (var br = new SolidBrush(lightBackColor)) {
                    pevent.Graphics.FillRectangle(br, GetOuterRectangle());
                }
            }
        }

        protected void UpdateSurface()
        {
            Refresh();
        }

        private void OnLoad()
        {
            if (Parent != null) {
                Parent.Paint += OnPaintOuterRectParent;
            }
        }

        #endregion
    }
}