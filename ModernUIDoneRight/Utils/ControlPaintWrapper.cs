/*
 * 
 * Software Criado por NickAc
 * Ficheiro Criado em 13/05/2016 às 05:53 PM
 * 
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace NickAc.ModernUIDoneRight.Utils
{
	/// <summary>
	/// ControlPaintWrapper - Copied code from decompiled ControlPaint class.
    /// All credits to the .NET developers
	/// </summary>
	public static class ControlPaintWrapper
	{
		public static Rectangle Center(this Rectangle rect, Rectangle parentRect) {
			Point center = parentRect.Center();
			
			var w = rect.Width;
			var h = rect.Height;
			
			
			return Rectangle.FromLTRB(center.X - (w / 2), center.Y - (h / 2), center.X + (w / 2), center.Y + (h / 2));
		}
		public static Point Center(this Rectangle rect) {
			Point center = new Point();
			center.X = rect.X + rect.Size.Width / 2;
			center.Y = rect.Y + rect.Size.Height / 2;
			return center;
		}
		
		private static readonly System.Drawing.ContentAlignment anyRight = (System.Drawing.ContentAlignment)1092;

		private static readonly System.Drawing.ContentAlignment anyBottom = (System.Drawing.ContentAlignment)1792;

		private static readonly System.Drawing.ContentAlignment anyCenter = (System.Drawing.ContentAlignment)546;

		private static readonly System.Drawing.ContentAlignment anyMiddle = (System.Drawing.ContentAlignment)112;

		
		public static Rectangle CalculateBackgroundImageRectangle(Rectangle bounds, Image backgroundImage, ImageLayout imageLayout)
		{
			Rectangle result = bounds;
			if (backgroundImage != null) {
				switch (imageLayout) {
					case ImageLayout.None:
						result.Size = backgroundImage.Size;
						break;
					case ImageLayout.Center: {
						result.Size = backgroundImage.Size;
						Size size = bounds.Size;
						if (size.Width > result.Width) {
							result.X = (size.Width - result.Width) / 2;
						}
						if (size.Height > result.Height) {
							result.Y = (size.Height - result.Height) / 2;
						}
						break;
					}
					case ImageLayout.Stretch:
						result.Size = bounds.Size;
						break;
					case ImageLayout.Zoom: {
						Size size2 = backgroundImage.Size;
						float num = (float)bounds.Width / (float)size2.Width;
						float num2 = (float)bounds.Height / (float)size2.Height;
						if (num < num2) {
							result.Width = bounds.Width;
							result.Height = (int)((double)((float)size2.Height * num) + 0.5);
							if (bounds.Y >= 0) {
								result.Y = (bounds.Height - result.Height) / 2;
							}
						}
						else {
							result.Height = bounds.Height;
							result.Width = (int)((double)((float)size2.Width * num2) + 0.5);
							if (bounds.X >= 0) {
								result.X = (bounds.Width - result.Width) / 2;
							}
						}
						break;
					}
				}
			}
			return result;
		}
		
		
		internal static StringAlignment TranslateAlignment(System.Drawing.ContentAlignment align)
		{
			StringAlignment result;
			// disable once BitwiseOperatorOnEnumWithoutFlags
			if ((align & ControlPaintWrapper.anyRight) != (System.Drawing.ContentAlignment)0) {
				result = StringAlignment.Far;
			}
			else {
				// disable once BitwiseOperatorOnEnumWithoutFlags
				if ((align & ControlPaintWrapper.anyCenter) != (System.Drawing.ContentAlignment)0) {
					result = StringAlignment.Center;
				}
				else {
					result = StringAlignment.Near;
				}
			}
			return result;
		}
		
		internal static StringAlignment TranslateLineAlignment(System.Drawing.ContentAlignment align)
		{
			StringAlignment result;
			if ((align & ControlPaintWrapper.anyBottom) != (System.Drawing.ContentAlignment)0) {
				result = StringAlignment.Far;
			}
			else {
				if ((align & ControlPaintWrapper.anyMiddle) != (System.Drawing.ContentAlignment)0) {
					result = StringAlignment.Center;
				}
				else {
					result = StringAlignment.Near;
				}
			}
			return result;
		}
		
		public static TextFormatFlags TranslateLineAlignmentForGDI(System.Drawing.ContentAlignment align)
		{
			TextFormatFlags result;
			// disable once BitwiseOperatorOnEnumWithoutFlags
			if ((align & ControlPaintWrapper.anyRight) != (System.Drawing.ContentAlignment)0) {
				result = TextFormatFlags.Right;
			}
			else {
				// disable once BitwiseOperatorOnEnumWithoutFlags
				if ((align & ControlPaintWrapper.anyCenter) != (System.Drawing.ContentAlignment)0) {
					result = TextFormatFlags.HorizontalCenter;
				}
				else {
					result = TextFormatFlags.Default;
				}
			}
			return result;
		}
		public static StringFormat StringFormatForAlignment(System.Drawing.ContentAlignment align)
		{
			return new StringFormat {
				Alignment = ControlPaintWrapper.TranslateAlignment(align),
				LineAlignment = ControlPaintWrapper.TranslateLineAlignment(align)
			};
		}
		
		public static StringFormat CreateStringFormat(Control ctl, ContentAlignment textAlign, bool showEllipsis)
		{
			StringFormat stringFormat = ControlPaintWrapper.StringFormatForAlignment(textAlign);
			if (ctl.RightToLeft == RightToLeft.Yes) {
				stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
			}
			if (showEllipsis) {
				stringFormat.Trimming = StringTrimming.EllipsisCharacter;
				stringFormat.FormatFlags |= StringFormatFlags.LineLimit;
			}
			if (ctl.AutoSize) {
				stringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
			}
			return stringFormat;
		}
		
		
		public static void DrawBackgroundImage(Graphics g, Image backgroundImage, Color backColor, ImageLayout backgroundImageLayout, Rectangle bounds, Rectangle clipRect, Point scrollOffset, RightToLeft rightToLeft)
		{
			if (g == null) {
				throw new ArgumentNullException(nameof(g));
			}
			if (backgroundImageLayout == ImageLayout.Tile) {
				using (TextureBrush textureBrush = new TextureBrush(backgroundImage, WrapMode.Tile)) {
					if (scrollOffset != Point.Empty) {
						Matrix transform = textureBrush.Transform;
						transform.Translate((float)scrollOffset.X, (float)scrollOffset.Y);
						textureBrush.Transform = transform;
					}
					g.FillRectangle(textureBrush, clipRect);
					return;
				}
			}
			Rectangle rectangle = ControlPaintWrapper.CalculateBackgroundImageRectangle(bounds, backgroundImage, backgroundImageLayout);
			if (rightToLeft == RightToLeft.Yes && backgroundImageLayout == ImageLayout.None) {
				rectangle.X += clipRect.Width - rectangle.Width;
			}
			using (SolidBrush solidBrush = new SolidBrush(backColor)) {
				g.FillRectangle(solidBrush, clipRect);
			}
			if (!clipRect.Contains(rectangle)) {
				if (backgroundImageLayout == ImageLayout.Stretch || backgroundImageLayout == ImageLayout.Zoom) {
					rectangle.Intersect(clipRect);
					g.DrawImage(backgroundImage, rectangle);
					return;
				}
				if (backgroundImageLayout == ImageLayout.None) {
					rectangle.Offset(clipRect.Location);
					Rectangle destRect = rectangle;
					destRect.Intersect(clipRect);
					Rectangle rectangle2 = new Rectangle(Point.Empty, destRect.Size);
					g.DrawImage(backgroundImage, destRect, rectangle2.X, rectangle2.Y, rectangle2.Width, rectangle2.Height, GraphicsUnit.Pixel);
					return;
				}
				Rectangle destRect2 = rectangle;
				destRect2.Intersect(clipRect);
				Rectangle rectangle3 = new Rectangle(new Point(destRect2.X - rectangle.X, destRect2.Y - rectangle.Y), destRect2.Size);
				g.DrawImage(backgroundImage, destRect2, rectangle3.X, rectangle3.Y, rectangle3.Width, rectangle3.Height, GraphicsUnit.Pixel);
				return;
			}
			else {
				ImageAttributes imageAttributes = new ImageAttributes();
				imageAttributes.SetWrapMode(WrapMode.TileFlipXY);
				g.DrawImage(backgroundImage, rectangle, 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel, imageAttributes);
				imageAttributes.Dispose();
			}
		}
		//The Rectangle (corresponds to the PictureBox.ClientRectangle)
		//we use here is the Form.ClientRectangle
		//Here is the Paint event handler of your Form1
		//use this method to draw the image like as the zooming feature of PictureBox
		public static void ZoomDrawImage(Graphics g, Image img, Rectangle bounds){
			decimal r1 = (decimal) img.Width/img.Height;
			decimal r2 = (decimal) bounds.Width/bounds.Height;
			int w = bounds.Width;
			int h = bounds.Height;
			if(r1 > r2){
				w = bounds.Width;
				h = (int) (w / r1);
			} else if(r1 < r2){
				h = bounds.Height;
				w = (int) (r1 * h);
			}
			int x = bounds.X + (bounds.Width - w)/2;
			int y = bounds.Y + (bounds.Height - h)/2;
			g.DrawImage(img, new Rectangle(x,y,w,h));
		}
		public static Rectangle ImageRectangleFromZoom(Image img, Rectangle originalRect)
		{
			var result = new Rectangle();
			Size size = img.Size;
			float num = Math.Min((float)originalRect.Width / (float)size.Width, (float)originalRect.Height / (float)size.Height);
			result.Width = (int)((float)size.Width * num);
			result.Height = (int)((float)size.Height * num);
			result.X = (originalRect.Width - result.Width) / 2;
			result.X += result.Width / 7;
			result.Y = originalRect.Y + (originalRect.Height - result.Height) / 2;
			return result;
	}
	}
}
