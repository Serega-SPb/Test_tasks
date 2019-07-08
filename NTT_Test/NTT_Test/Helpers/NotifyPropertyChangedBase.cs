using System.ComponentModel;

namespace NTT_Test.Helpers
{
	public class NotifyPropertyChangedBase: INotifyPropertyChanged
	{
		
		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}