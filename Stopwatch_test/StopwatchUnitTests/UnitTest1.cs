using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stopwatch_test;
using Stopwatch = Stopwatch_test.Stopwatch;

namespace StopwatchUnitTests
{
	[TestClass]
	public class UnitTest1
	{
		private Stopwatch _stopwatch;

		[TestMethod]
		public void CreateStopwatchTest()
		{
			_stopwatch = new Stopwatch();
			Assert.IsNotNull(_stopwatch);
			Trace.WriteLine("Create stopwatch test finish");
		}

		[TestMethod]
		public void StateStopwatchTest()
		{
			if(_stopwatch == null)
				_stopwatch = new Stopwatch();

			_stopwatch.Start();
			Assert.AreEqual(StopwatchState.Play, _stopwatch.State);
			
			_stopwatch.Pause();
			Assert.AreEqual(StopwatchState.Pause, _stopwatch.State);

			_stopwatch.Reset();
			Assert.AreEqual(StopwatchState.Reset, _stopwatch.State);

			Trace.WriteLine("State stopwatch test finish");
		}

		[TestMethod]
		public void CreateRoundTest()
		{
			if (_stopwatch == null)
				_stopwatch = new Stopwatch();

			_stopwatch.Start();

			var index = 0;
			for (int i = 0; i < 10; i++)
			{
				_stopwatch.Round();
				index++;
				Assert.AreEqual(index,_stopwatch.Rounds.Count);
				Thread.Sleep(10);
			}

			_stopwatch.Reset();
			Assert.AreEqual(0, _stopwatch.Rounds.Count);

			Trace.WriteLine("Create round test finish");
		}
	}
}
