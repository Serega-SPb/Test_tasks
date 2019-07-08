using System;
using System.Collections.Generic;
using Test_TaskModel_Viewer.Extensions;

namespace Test_TaskModel_Viewer.Classes
{
	public class DateRange : NotificationBase
	{
		private DateTime _start;
		private DateTime _end;

		public DateRange(DateTime start, DateTime end)
		{
			_start = start;
			_end = end;
			OnPropertyChanged(nameof(Start));
			OnPropertyChanged(nameof(End));
		}

		public DateTime Start
		{
			get { return _start; }
			set
			{
				if (value > End)
					value = End;
				_start = value;
				OnPropertyChanged(nameof(Start));
			}
		}

		public DateTime End
		{
			get { return _end; }
			set
			{
				if (value < Start)
					value = Start;
				_end = value;
				OnPropertyChanged(nameof(End));
			}
		}

		public IEnumerable<DateTime> GetMonthYearArray()
		{
			var iterator = new DateTime(Start.Year, Start.Month, 1);
			while (iterator < End)
			{
				yield return new DateTime(iterator.Year,iterator.Month,1);
				iterator = iterator.AddMonths(1);
			}
		}

		public int DiffDaysStart(DateTime start)
		{
			return GetDiff(Start,start);
		}

		public int DiffDaysEnd(DateTime end)
		{
			return GetDiff(End, end);
		}

		private int GetDiff(DateTime current, DateTime value)
		{
			return (value - current).Days;
		}

		public void Shift(int start,int end)
		{
			_start = Start.AddDays(start);
			_end = End.AddDays(end);
			OnPropertyChanged(nameof(Start));
			OnPropertyChanged(nameof(End));
		}

		public bool DateInRange(DateTime date)
		{
			return Start <= date && date <= End;
		}

		public override string ToString()
		{
			return $"{Start.Date:dd:MM:yyyy} - {End.Date:dd:MM:yyyy}";
		}

		public override bool Equals(object obj)
		{
			if (!(obj is DateRange dr))
				return false;

			if (!Start.Equals(dr.Start))
				return false;
			if (!End.Equals(dr.End))
				return false;
			return true;
		}
	}
}