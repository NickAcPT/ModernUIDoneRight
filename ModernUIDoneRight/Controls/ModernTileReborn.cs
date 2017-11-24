using NickAc.ModernUIDoneRight.Objects;
using NickAc.ModernUIDoneRight.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
		#region Constructor
		public TilePanelReborn()
		{
			DoubleBuffered = true;
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
		}
		#endregion
		#region Events

		
		protected void OnPaintOuterRectParent(object sender, PaintEventArgs pevent)
		{
			if (isHovered) {
				using (var g = Parent.CreateGraphics()) {
					using (var br = new SolidBrush(lightBackColor)) {
						g.FillRectangle(br, getOuterRectangle());
					}
				}
			}
		}
		bool isHovered;
		protected override void OnMouseHover(EventArgs e)
		{
			base.OnMouseHover(e);
			isHovered = true;
			if (CanBeHovered) {
				Parent.Refresh();
			}
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			isHovered = false;
			if (CanBeHovered) {
				Parent.Refresh();
			}
		}
		#endregion
		public int GetPercentage(int size, int percent)
		{
			return size * percent / 100;
		}
		public Rectangle GetImageRect()
		{
			if (brandedTile)
				return DisplayRectangle;

            return new Rectangle(0, 0, GetPercentage(Width, 50), GetPercentage(Height, 50));
		}
		#region Paint
		protected override void OnPaint(PaintEventArgs e)
		{
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
			//Draw gradient
			using (LinearGradientBrush brush = new LinearGradientBrush(displayRectangle, Color.FromArgb(75, 0, 0, 0), Color.FromArgb(7, 0, 0, 0), LinearGradientMode.Horizontal)) {
				e.Graphics.FillRectangle(brush, displayRectangle);
			}

			if (Image != null) {
				Rectangle imgRect = GetImageRect();
				//Draw Image
				if (!brandedTile) {
					ControlPaintWrapper.ZoomDrawImage(e.Graphics, Image, imgRect.Center(Rectangle.FromLTRB(displayRectangle.Left, displayRectangle.Top, displayRectangle.Right, ((string.Empty.Equals(Text.Trim()) ? displayRectangle.Bottom : getTextRectangle().Top)))));
				} else {
					e.Graphics.DrawImage(image, imgRect);
				}
			}
			if (!brandedTile) {
				using (var sb = new SolidBrush(ForeColor)) {
					using (var tF = ControlPaintWrapper.CreateStringFormat(this, ContentAlignment.BottomLeft, false)) {
						e.Graphics.DrawString(Text, Font, sb, getTextRectangle(), tF);
						foreach (var t in texts) {
							if (t != null) {
								e.Graphics.DrawString(t.Text, t.Font, sb, t.Location.X, t.Location.Y);
							}
						}
					}
				}
			}
		}
		#endregion
		#region Properties and Internal Stuff
		List<TileText> texts = new List<TileText>();
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public List<TileText> Texts {
			get {
				return texts;
			}
			set {
				texts = value;
			}
		}

		bool brandedTile = false;
		public bool BrandedTile {
			get {
				return brandedTile;
			}
			set {
				brandedTile = value;
				Refresh();
			}
		}

		void UpdateParentHoverEvent(bool isH)
		{
			if (Parent != null) {
				if (isH) {
					Parent.Paint += OnPaintOuterRectParent;
				} else {
					Parent.Paint -= OnPaintOuterRectParent;
				}
			}
		}

		bool canBeHovered;
		public bool CanBeHovered {
			get {
				return canBeHovered;
			}
			set {
				canBeHovered = value;
				UpdateParentHoverEvent(value);
			}
		}
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
		Color lightlightBackColor;
		Color lightBackColor;

		public Rectangle getTextRectangle()
		{
			var rectangle = new Rectangle(0, Height - 32, Width, 32);
			rectangle.Inflate(-8, -8);
			return rectangle;
		}
		public Rectangle getOuterRectangle()
		{
			var rectangle = Bounds;
			rectangle.Inflate(4, 4);
			return rectangle;
		}

		protected void UpdateSurface()
		{
            Refresh();
		}


		Image image;
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
		bool checkable;

		public bool Checkable {
			get { return checkable; }
			set {
				checkable = value;
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
		#endregion
	}
}