using System;
using System.Collections.ObjectModel;
using System.Timers;

namespace Stopwatch_test
{
	public class Stopwatch:NotificationBase
	{
		private Timer _timer = new Timer();
		private TimeSpan _time;
		private StopwatchState _state;

		public Stopwatch()
		{
			_timer.Interval = 10;
			_timer.Elapsed += (s, e) => { Time += TimeSpan.FromMilliseconds(_timer.Interval); };
		}

		public StopwatchState State
		{
			get { return _state; }
			private set
			{
				_state = value; 
				OnStateChangedEvent();
			}
		}

		public TimeSpan Time
		{
			get => _time;
			private set
			{
				_time = value;
				OnPropertyChanged(nameof(Time));
			}
		}

		public ObservableCollection<TimeSpan> Rounds { get; } = new ObservableCollection<TimeSpan>();


		public void Start()
		{
			_timer.Start();
			State = StopwatchState.Play;
		}

		public void Pause()
		{
			_timer.Stop();
			State = StopwatchState.Pause;
		}

		public void Reset()
		{
			if(State!=StopwatchState.Pause)
				Pause();
			_timer.Close();
			Rounds.Clear();
			Time = TimeSpan.Zero;
			State = StopwatchState.Reset;
		}

		public void Round()
		{
			Rounds.Add(Time);
		}

		public event EventHandler StateChangedEvent;

		private void OnStateChangedEvent()
		{
			StateChangedEvent?.Invoke(this, EventArgs.Empty);
		}
	}

	public enum StopwatchState
	{
		Reset,
		Play,
		Pause
	}
}