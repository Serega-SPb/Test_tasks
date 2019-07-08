using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Test_TaskModel_Viewer.Classes;

namespace Test_TaskModel_Viewer.Converters
{
	public class ModelToGraphConverter:IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(values[0] is DateRange && values[1] is FrameworkElement && values[2] != null))
				return null;


			var modelRange = values[0] as DateRange;

			var parent = ((FrameworkElement) values[1]);

			var columnDate = (Dictionary<string, DateRange>)(parent?.Tag);
			if (modelRange == null || columnDate == null)
				return null;

			var tag = values[2];

			var blocRange = columnDate[tag.ToString()];

			return (blocRange.Start >= modelRange.Start || blocRange.End >= modelRange.Start) &&
			       (blocRange.Start <= modelRange.End || blocRange.End <= modelRange.End);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class SelectTimeRange:IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var parent = ((FrameworkElement)values[0]);
			var columnDate = (Dictionary<string, DateRange>)(parent?.Tag);
			var tag = values[1];
			return $"Период - {columnDate?[tag.ToString()]}";
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}