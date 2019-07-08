using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Microsoft.Win32;
using NTT_Test.Helpers;
using NTT_Test.Logics;

namespace NTT_Test.ViewModels
{
	public class MainViewModel:NotifyPropertyChangedBase
	{
		private readonly ObservableCollection<ObjectLink> _dataCollection = new ObservableCollection<ObjectLink>();
		private string _file;
		private bool _isReading;
		private int _currentLine;
		private int _totalLines;
		private bool _allowFilter;

		public MainViewModel()
		{
			_dataCollection.CollectionChanged += (s, e) => { AllowFilter = _dataCollection.Count != 0; };
			OpenFileCommand = new DelegateCommand(OpenFile);
			StopReadingCommand = new DelegateCommand(StopReding);
			ApplyFilterCommand = new DelegateCommand(ApplyFilter);
			FileLoader.LoadEvent += LoadingData;
			FileLoader.CounterEvent += Counter;
		}

		#region Properties

		public DelegateCommand OpenFileCommand { get; }
		public DelegateCommand StopReadingCommand { get; }
		public DelegateCommand ApplyFilterCommand { get; }

		public ICollectionView DataCollection => CollectionViewSource.GetDefaultView(_dataCollection);

		public string File
		{
			get => _file;
			set
			{
				_file = value;
				OnPropertyChanged(nameof(File));
			}
		}

		public bool IsReading
		{
			get => _isReading;
			set
			{
				_isReading = value;
				OnPropertyChanged(nameof(IsReading));
				OnPropertyChanged(nameof(IsWaiting));
			}
		}

		public bool IsWaiting => !IsReading;

		public int CurrentLine
		{
			get => _currentLine;
			set
			{
				if(_currentLine==value)return;
				_currentLine = value;
				OnPropertyChanged(nameof(CurrentLine));
			}
		}

		public int TotalLines
		{
			get => _totalLines;
			set
			{
				if (_totalLines == value) return;
				_totalLines = value;
				OnPropertyChanged(nameof(TotalLines));
			}
		}

		public bool AllowFilter
		{
			get { return _allowFilter; }
			set
			{
				_allowFilter = value;
				OnPropertyChanged(nameof(AllowFilter));
			}
		}

		public CollectionFilter Filter { get; } = new CollectionFilter();

		#endregion

		private void OpenFile()
		{
			var dialog = new OpenFileDialog()
			{
				Multiselect = false
			};

			var answer = dialog.ShowDialog();
			if(answer!=true)return;
			_dataCollection.Clear();
			File = dialog.FileName;
			new TaskFactory().StartNew(() =>
			{
				Wrapper(()=>IsReading = true);
				FileLoader.LoadFile(File);
				Wrapper(() => IsReading = false);
			});
		}

		private void StopReding()
		{
			FileLoader.Stop();
			Wrapper(() => IsReading = false);
			CurrentLine = 0;
		}

		private void LoadingData(ObjectLink link)
		{
			Wrapper(() => { _dataCollection.Add(link); });
		}

		private void Counter(int current, int total)
		{
			Wrapper(()=>
			{
				CurrentLine = current;
				TotalLines = total;
			});		
		}

		private void ApplyFilter()
		{
			AllowFilter = false;
			DataCollection.Filter = o => Filter.FilterPredicate((ObjectLink) o);
			DataCollection.Refresh();
			AllowFilter = true;
			OnPropertyChanged(nameof(DataCollection));
		}

		private void Wrapper(Action action)
		{
			Application.Current.Dispatcher.Invoke(action);
		}
	}
}