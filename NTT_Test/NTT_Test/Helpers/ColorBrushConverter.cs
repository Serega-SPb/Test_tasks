using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace NTT_Test.Helpers
{
	public class ColorBrushConverter:IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return new BrushConverter().ConvertFrom(value.ToString());
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}