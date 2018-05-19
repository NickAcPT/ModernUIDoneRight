using NickAc.ModernUIDoneRight.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NickAc.ModernUIDoneRight.Utils
{
    class ColorSchemeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        private static readonly ColorConverter ColorConverter = new ColorConverter();
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string stringVal && ColorConverter != null)
            {
                var color = (Color) ColorConverter.ConvertFrom(stringVal);
                return ColorScheme.CreateSimpleColorScheme(color);
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(ColorScheme));
        }
    }
}
