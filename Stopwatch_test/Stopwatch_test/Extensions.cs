using System.ComponentModel;

namespace Stopwatch_test
{
	public static class Extensions
	{
		public static void RaisePropertyChanged(this INotifyPropertyChanged userControl, string propertyName, PropertyChangedEventHandler propertyChanged)
		{
			propertyChanged?.Invoke(userControl, new PropertyChangedEventArgs(propertyName));
		}
	}
}