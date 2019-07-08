using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;

namespace GZipTest
{
	public class Compressor
	{
		private Logger _logger = Logger.Instance;
		private FileHelper _fileHelper;
		private readonly int _threadCount;

		private string _sourceFile;
		private string _resultFile;
		private CompressionMode _mode;
		private bool _isStop;

		//private readonly BlockingCollection<Block> _inputBlocks = new BlockingCollection<Block>(25);
		private readonly ProducerConsumerQueue<Block> _inputBlocks = new ProducerConsumerQueue<Block>(25);
		//private readonly BlockingCollection<Block> _outputBlocks = new BlockingCollection<Block>(25);
		private readonly ProducerConsumerQueue<Block> _outputBlocks = new ProducerConsumerQueue<Block>(25);

		public Compressor(string sourceFile, string resultFile, CompressionMode mode)
		{
			_fileHelper = new FileHelper();
			_sourceFile = sourceFile;
			_resultFile = resultFile;
			_mode = mode;
			_threadCount = Environment.ProcessorCount;
		}
		
		public void Start()
		{
			_logger.OnLogEvent(_mode == CompressionMode.Compress
					? $"Start compressing\t{DateTime.Now.TimeOfDay}"
					: $"Start decompressing\t{DateTime.Now.TimeOfDay}", ConsoleColor.Green);

			_isStop = false;

			_fileHelper.ReadFile(_sourceFile,_inputBlocks,_mode==CompressionMode.Decompress);
			_index = 0;

			var threadUse =_threadCount > 3 ? _threadCount - 2 : 1;

			for (int i = 0; i < threadUse; i++)
			{
				var th = new Thread(() =>
				{
					_logger.Wrapper(() =>
					{
						do
						{
							if (_isStop)
								return;

							if (_mode == CompressionMode.Compress)
								Compress();
							else
								Decompress();

						} while (!_fileHelper.ReadingEnd || _inputBlocks.Count>0);
					});
				});
				th.Start();
			}
			_fileHelper.WriteFile(_resultFile,_outputBlocks, _mode == CompressionMode.Decompress);
		}

		public void Cancel()
		{
			if (_isStop)
				return;
			Stop();
			_logger.OnLogEvent("Operation has been stopped by user", ConsoleColor.Red);
		}

		private void Stop()
		{
			_fileHelper.Stop();
			_isStop = true;
			
			new Thread(() =>
			{
				Thread.Sleep(100);
				_inputBlocks.Dispose();
				_outputBlocks.Dispose();
				GC.Collect();
			}).Start();
		}

		private void Compress()
		{
			var inputBlock = _inputBlocks.Take();
			using (var inputStream = new MemoryStream(inputBlock.BlockBytes))
			{
				using (var outputStream = new MemoryStream())
				{
					using (var compressionStream = new GZipStream(outputStream, _mode))
					{
						inputStream.CopyTo(compressionStream);
					}
					if (_isStop)
						return;
					var outputBytes = outputStream.ToArray();
					var size = BitConverter.GetBytes(outputBytes.Length);
					var outputBlock = new Block(inputBlock.Id, size.Concat(outputBytes).ToArray());
					AddOutputBlockById(outputBlock);
				}
			}
		}

		private void Decompress()
		{
			var inputBlock = _inputBlocks.Take();
			using (var inputStream = new MemoryStream(inputBlock.BlockBytes))
			{
				var outputBytes = new byte[_fileHelper.BlockSize];
				using (var outputStream = new MemoryStream(outputBytes,true))
				{
					using (var decompressionStream = new GZipStream(inputStream, _mode))
					{
						decompressionStream.CopyTo(outputStream);
					}
					if (_isStop)
						return;
					outputBytes = outputStream.ToArray();
					var outputBlock = new Block(inputBlock.Id, outputBytes);
					AddOutputBlockById(outputBlock);
				}
			}
		}

		private object _lock = new object();
		private int _index;
		private List<Block> _tempStorage = new List<Block>();

		private void AddOutputBlockById(Block block)
		{
			lock (_lock)
			{
				if (_isStop)
					return;

				if (block.Id == _index)
				{
					_outputBlocks.Add(block);
					_index++;

					var next = _tempStorage.FirstOrDefault(b => b.Id == _index);
					if (next != null)
					{
						_tempStorage.Remove(next);
						AddOutputBlockById(next);
					}
				}
				else
					_tempStorage.Add(block);
			}
		}
	}
}