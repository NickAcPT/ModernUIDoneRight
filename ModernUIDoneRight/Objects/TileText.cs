/*
 * 
 * Software Criado por NickAc
 * Ficheiro Criado em 16/11/2016 às 06:58 PM
 * 
 */
using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace NickAc.ModernUIDoneRight.Objects
{
	public class TileText
	{
		public TileText()
		{
		}
		public TileText(string text, Font font, Point location)
		{
			this.text = text;
			this.font = font;
			this.location = location;
		}
		Point location = Point.Empty;
		public virtual Point Location {
			get {
				return location;
			}
			set {
				location = value;
			}
		}
		
		String text = "TileText";
		public virtual String Text {
			get {
				return text;
			}
			set {
				text = value;
			}
		}
		
		Font font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
		public virtual Font Font {
			get {
				return font;
			}
			set {
				font = value;
			}
		}
		public override string ToString()
		{
			return "TileText";
		}
	}
}
