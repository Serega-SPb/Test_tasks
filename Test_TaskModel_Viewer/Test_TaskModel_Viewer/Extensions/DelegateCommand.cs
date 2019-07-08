using System;
using System.Windows.Input;

namespace Test_TaskModel_Viewer.Extensions
{
	public class DelegateCommand:ICommand
	{
		private readonly Action _action;

		public DelegateCommand(Action action)
		{
			_action = action;
		}

		public void Execute(object parameter)
		{
			if(CanExecute(parameter))
				_action();
		}

		public bool CanExecute(object parameter)
		{
			return _action!=null;
		}

		public event EventHandler CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}
	}

	public class DelegateCommand<T> : ICommand where T : class
	{
		private readonly Action<T> _action;

		public DelegateCommand(Action<T> action)
		{
			_action = action;
		}

		public void Execute(object parameter)
		{
			if (CanExecute(parameter))
				_action(parameter as T);
		}

		public bool CanExecute(object parameter)
		{
			return _action != null && parameter is T;
		}

		public event EventHandler CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}
	}
}