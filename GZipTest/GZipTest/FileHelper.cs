using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;

namespace GZipTest
{
	public class FileHelper
	{
		private Logger _logger = Logger.Instance;

		private Thread _readingThread;
		private Thread _writeingThread;

		private int _amountBlocks;
		private bool _isStoped;
		
		public bool ReadingEnd { get; private set; }
		public int BlockSize { get; } = 1000000;

		public void ReadFile(string sourceFile, ProducerConsumerQueue<Block> inputBlocks,bool withSize = false)
		{
			_readingThread = new Thread(() =>
			{
				_logger.Wrapper(() =>
				{
					using (var readingStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
					{
						_logger.OnLogEvent($"Start reading\t\t{DateTime.Now.TimeOfDay}");
						ReadingEnd = false;
						var index = _amountBlocks = 0;
						while (readingStream.CanRead)
						{
							if (_isStoped)
							{
								readingStream.Close();
								return;
							}

							var size = BlockSize;

							if (withSize)
							{
								var sizebBytes = new byte[4];
								if (readingStream.Read(sizebBytes, 0, 4) == 0)
									break;
								size = BitConverter.ToInt32(sizebBytes, 0);
							}

							var bytes = new byte[size];
							var c = readingStream.Read(bytes, 0, size);
							if (c == 0)
								break;

							var block = new Block(index, bytes)
							{
								BlockSize = size
							};
							inputBlocks.Add(block);
							index++;
							_amountBlocks++;
						}

						_logger.OnLogEvent($"Read has finished\t{DateTime.Now.TimeOfDay}", ConsoleColor.Yellow);
						ReadingEnd = true;
					}
				});
			});
			_readingThread.Start();
		}

		public void WriteFile(string sourceFile, ProducerConsumerQueue<Block> outputBlocks, bool checkLastBlock = false)
		{
			_writeingThread = new Thread(() =>
			{
				_logger.Wrapper(() =>
				{
					using (var writingStream = new FileStream(sourceFile, FileMode.Create, FileAccess.Write))
					{
						_logger.OnLogEvent($"Start writing\t\t{DateTime.Now.TimeOfDay}");
						var index = 0;
						do
						{
							if (_isStoped)
							{
								writingStream.Close();
								return;
							}

							var block = outputBlocks.Take();
							var writeLength = block.BlockBytes.Length;
							if (checkLastBlock && ReadingEnd && _amountBlocks == index+1)
							{
								for (var i = writeLength - 1; i >= 0; i--)
								{
									if (block.BlockBytes[i] == 0) continue;
									writeLength = i;
									break;
								}
							}
							writingStream.Write(block.BlockBytes, 0, writeLength);
							index++;
							_logger.OnProgressEvent(_amountBlocks, index);
						} while (!ReadingEnd || outputBlocks.Count>0 || _amountBlocks > index);

						_logger.OnLogEvent($"Write has finished\t{DateTime.Now.TimeOfDay}", ConsoleColor.Yellow);
					}
				});
			});
			_writeingThread.Start();
		}

		public void Stop()
		{
			_isStoped = true;
		}
	}
}