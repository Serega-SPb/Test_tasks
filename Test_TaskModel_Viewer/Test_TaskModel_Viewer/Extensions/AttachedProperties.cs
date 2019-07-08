using System.Windows;

namespace Test_TaskModel_Viewer.Extensions
{
	public class AttachedProperties
	{
		public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.RegisterAttached(
			"IsChecked", typeof(bool), typeof(AttachedProperties), new PropertyMetadata(default(bool)));

		public static void SetIsChecked(DependencyObject element, bool value)
		{
			element.SetValue(IsCheckedProperty, value);
		}

		public static object GetIsChecked(DependencyObject element)
		{
			return (bool)element.GetValue(IsCheckedProperty);
		}
	}
}