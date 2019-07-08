using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace GZipTest
{
	class Program
	{
		private static Compressor _compressor;
		private static Logger _logger = Logger.Instance;

		static void Main(string[] args)
		{
			_logger.LogEvent += WriteLog;
			_logger.ProgressEvent += WriteProgress;
			_logger.ErrorEvent += (m) => WriteLog(m, ConsoleColor.Red);

			Console.CursorVisible = false;
			WriteLog("          GzipTest application          ", ConsoleColor.Cyan);
			WriteLog("----------------------------------------", ConsoleColor.Cyan);
			Console.CancelKeyPress += (s, e) => { _compressor?.Cancel(); };

			if (!ReadArgs(args))
			{
				Console.ReadKey();
				return;
			}
			_compressor.Start();
			Console.ReadKey();
		}

		private static bool ReadArgs(string[] args)
		{
			if (args.Length != 3)
			{
				WriteLog("Incorrect amount of arguments", ConsoleColor.Red);
				return false;
			}

			var mode = (CompressionMode)Enum.Parse(typeof(CompressionMode), args[0], true);

			var sourceFile = args[1];
			if (!File.Exists(sourceFile))
			{
				WriteLog("Source file not found", ConsoleColor.Red);
				return false;
			}

			var resultFile = args[2];
			
			_compressor = new Compressor(sourceFile,resultFile,mode);
			return true;
		}

		private static object _lock = new object();
		private static void WriteLog(string message, ConsoleColor color = ConsoleColor.White)
		{
			lock (_lock)
			{
				Console.ForegroundColor = color;
				Console.WriteLine(message);
				Debug.WriteLine(message);
			}
		}

		private static int _progressLine = -1;
		private static void WriteProgress(int readBlockCount, int writeBlockCount)
		{
			lock (_lock)
			{
				var currentLine = Console.CursorTop;
				if (_progressLine == -1)
					_progressLine = Console.CursorTop;
				if (currentLine == _progressLine)
					currentLine++;

				Console.SetCursorPosition(0, _progressLine);
				WriteLog($"Progress: [W {writeBlockCount} / R {readBlockCount}]",ConsoleColor.Blue);
				Console.SetCursorPosition(0, currentLine);
			}
		}

	}
}
