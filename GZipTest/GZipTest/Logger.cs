using System;

namespace GZipTest
{
	public class Logger
	{
		private static Logger _instance;
		public static Logger Instance => _instance ?? (_instance = new Logger());
		private Logger() { }


		public void Wrapper(Action action)
		{
			try
			{
				action();
			}
			catch (Exception exception)
			{
				OnErrorEvent(exception.Message);
			}
		}

		public delegate void LogEventHandler(string message, ConsoleColor color = ConsoleColor.White);
		public event LogEventHandler LogEvent;

		public void OnLogEvent(string message, ConsoleColor color = ConsoleColor.White)
		{
			LogEvent?.Invoke(message, color);
		}

		public delegate void ProgressEventHandler(int readBlockCount, int writeBlockCount);
		public event ProgressEventHandler ProgressEvent;
		public void OnProgressEvent(int readblockcount, int writeblockcount)
		{
			ProgressEvent?.Invoke(readblockcount, writeblockcount);
		}

		public delegate void ErrorEventHandler(string errorMessage);
		public event ErrorEventHandler ErrorEvent;
		private void OnErrorEvent(string errormessage)
		{
			ErrorEvent?.Invoke(errormessage);
		}
	}
}