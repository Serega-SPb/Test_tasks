﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace NTT_Test.Helpers
{
	public class DateConverter:IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}