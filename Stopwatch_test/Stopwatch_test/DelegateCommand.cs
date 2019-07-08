﻿using System;
using System.Windows.Input;

namespace Stopwatch_test
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
			_action();
		}

		public bool CanExecute(object parameter)
		{
			return _action!=null;
		}

		public event EventHandler CanExecuteChanged;
	}
}