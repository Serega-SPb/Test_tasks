using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GZipTest
{
	public class ProducerConsumerQueue<T>:IDisposable where T:class 
	{
		private readonly Queue<T> _queue;
		private bool _isDisposed;
		private object _mutex = new object();
		private int _capacity;

		public int Count => _queue.Count;

		public ProducerConsumerQueue()
		{
			_queue = new Queue<T>();
			_capacity = -1;
		}

		public ProducerConsumerQueue(int capacity)
		{
			_queue = new Queue<T>(capacity);
			_capacity = capacity;
		}

		public void Add(T item)
		{
			if (item == null)
				throw new ArgumentNullException(nameof(item));

			if (_capacity!=-1)
				while (_queue.Count >= _capacity && !_isDisposed)
				{
					Thread.Sleep(50);
				}

			lock (_mutex)
			{
				if (_isDisposed)
					return;
				
				_queue.Enqueue(item);
				Monitor.Pulse(_mutex);
			}
		}

		public T Take()
		{
			lock (_mutex)
			{
				while (_queue.Count == 0 && !_isDisposed)
					Monitor.Wait(_mutex);

				return !_queue.Any() ? null : _queue.Dequeue();
			}
		}


		public void Dispose()
		{
			lock (_mutex)
			{
				_isDisposed = true;
				_queue.Clear();
				Monitor.PulseAll(_mutex);
			}
		}
	}
}