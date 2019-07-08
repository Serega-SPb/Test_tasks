namespace Stopwatch_test
{
	public class StopwatchViewModel:NotificationBase
	{

		public StopwatchViewModel()
		{
			Stopwatch.StateChangedEvent += (s, e) =>
			{
				OnPropertyChanged(nameof(CanStart));
				OnPropertyChanged(nameof(CanPause));
				OnPropertyChanged(nameof(CanReset));
				OnPropertyChanged(nameof(CanRound));
			};
		}

		public Stopwatch Stopwatch { get; } = new Stopwatch();

		public bool CanStart => Stopwatch.State != StopwatchState.Play;
		public bool CanPause => Stopwatch.State == StopwatchState.Play;
		public bool CanReset => Stopwatch.State == StopwatchState.Play || Stopwatch.State == StopwatchState.Pause;
		public bool CanRound => Stopwatch.State == StopwatchState.Play;

		public DelegateCommand StartCommand => new DelegateCommand(Stopwatch.Start);
		public DelegateCommand PauseCommand => new DelegateCommand(Stopwatch.Pause);
		public DelegateCommand ResetCommand => new DelegateCommand(Stopwatch.Reset);
		public DelegateCommand RoundCommand => new DelegateCommand(Stopwatch.Round);
	}
}