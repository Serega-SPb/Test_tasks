using System;
using NTT_Test.Helpers;

namespace NTT_Test.Logics
{
	public class CollectionFilter : NotifyPropertyChangedBase
	{
		private bool _dateFilter;
		private bool _directionFilter;
		private bool _intensityFilter;
		private bool _objectAFilter;
		private bool _typeAFilter;
		private bool _lastitudeAFilter;
		private bool _longitudeAFilter;
		private bool _objectBFilter;
		private bool _typeBFilter;
		private bool _lastitudeBFilter;
		private bool _longitudeBFilter;
		private DateTime _fromDate;
		private DateTime _toDate;
		private Direction _direction;
		private int _intensity;
		private object _objectA;
		private object _typeA;
		private double _fromLastitudeA;
		private double _toLastitudeA;
		private double _fromLongitudeA;
		private double _toLongitudeA;
		private object _objectB;
		private object _typeB;
		private double _fromLastitudeB;
		private double _toLastitudeB;
		private double _fromLongitudeB;
		private double _toLongitudeB;

		#region Flags

		public bool DateFilter
		{
			get => _dateFilter;
			set
			{
				_dateFilter = value;
				OnPropertyChanged(nameof(DateFilter));
			}
		}

		public bool DirectionFilter
		{
			get => _directionFilter;
			set
			{
				_directionFilter = value;
				OnPropertyChanged(nameof(DirectionFilter));
			}
		}

		public bool IntensityFilter
		{
			get => _intensityFilter;
			set
			{
				_intensityFilter = value;
				OnPropertyChanged(nameof(IntensityFilter));
			}
		}

		public bool ObjectAFilter
		{
			get => _objectAFilter;
			set
			{
				_objectAFilter = value;
				OnPropertyChanged(nameof(ObjectAFilter));
			}
		}

		public bool TypeAFilter
		{
			get => _typeAFilter;
			set
			{
				_typeAFilter = value;
				OnPropertyChanged(nameof(TypeAFilter));
			}
		}

		public bool LastitudeAFilter
		{
			get => _lastitudeAFilter;
			set
			{
				_lastitudeAFilter = value;
				OnPropertyChanged(nameof(LastitudeAFilter));
			}
		}

		public bool LongitudeAFilter
		{
			get => _longitudeAFilter;
			set
			{
				_longitudeAFilter = value;
				OnPropertyChanged(nameof(LongitudeAFilter));
			}
		}

		public bool ObjectBFilter
		{
			get => _objectBFilter;
			set
			{
				_objectBFilter = value;
				OnPropertyChanged(nameof(ObjectBFilter));
			}
		}

		public bool TypeBFilter
		{
			get => _typeBFilter;
			set
			{
				_typeBFilter = value;
				OnPropertyChanged(nameof(TypeBFilter));
			}
		}

		public bool LastitudeBFilter
		{
			get => _lastitudeBFilter;
			set
			{
				_lastitudeBFilter = value;
				OnPropertyChanged(nameof(LastitudeBFilter));
			}
		}

		public bool LongitudeBFilter
		{
			get => _longitudeBFilter;
			set
			{
				_longitudeBFilter = value;
				OnPropertyChanged(nameof(LongitudeBFilter));
			}
		}

		#endregion

		public DateTime FromDate
		{
			get => _fromDate;
			set
			{
				if (value > ToDate)
					ToDate = value;
				_fromDate = value;
				OnPropertyChanged(nameof(FromDate));
			}
		}

		public DateTime ToDate
		{
			get => _toDate;
			set
			{
				if (value < FromDate)
					FromDate = value;
				_toDate = value;
				OnPropertyChanged(nameof(ToDate));
			}
		}

		public Direction Direction
		{
			get => _direction;
			set
			{
				_direction = value;
				OnPropertyChanged(nameof(Direction));
			}
		}

		public int Intensity
		{
			get => _intensity;
			set
			{
				_intensity = value;
				OnPropertyChanged(nameof(Intensity));
			}
		}


		public object ObjectA
		{
			get => _objectA;
			set
			{
				_objectA = value;
				OnPropertyChanged(nameof(ObjectA));
			}
		}

		public object TypeA
		{
			get => _typeA;
			set
			{
				_typeA = value;
				OnPropertyChanged(nameof(TypeA));
			}
		}

		public double FromLatitudeA
		{
			get => _fromLastitudeA;
			set
			{
				if (value > ToLatitudeA)
					ToLatitudeA = value;
				_fromLastitudeA = value;
				OnPropertyChanged(nameof(FromLatitudeA));
			}
		}

		public double ToLatitudeA
		{
			get => _toLastitudeA;
			set
			{
				if (value < FromLatitudeA)
					FromLatitudeA = value;
				_toLastitudeA = value;
				OnPropertyChanged(nameof(ToLatitudeA));
			}
		}

		public double FromLongitudeA
		{
			get => _fromLongitudeA;
			set
			{
				if (value > ToLongitudeA)
					ToLongitudeA = value;
				_fromLongitudeA = value;
				OnPropertyChanged(nameof(FromLongitudeA));
			}
		}

		public double ToLongitudeA
		{
			get => _toLongitudeA;
			set
			{
				if (value < FromLongitudeA)
					FromLongitudeA = value;
				_toLongitudeA = value;
				OnPropertyChanged(nameof(ToLongitudeA));
			}
		}


		public object ObjectB
		{
			get => _objectB;
			set
			{
				_objectB = value;
				OnPropertyChanged(nameof(ObjectB));
			}
		}

		public object TypeB
		{
			get => _typeB;
			set
			{
				_typeB = value;
				OnPropertyChanged(nameof(TypeB));
			}
		}

		public double FromLatitudeB
		{
			get => _fromLastitudeB;
			set
			{
				if (value > ToLatitudeB)
					ToLatitudeB = value;
				_fromLastitudeB = value;
				OnPropertyChanged(nameof(FromLatitudeB));
			}
		}

		public double ToLatitudeB
		{
			get => _toLastitudeB;
			set
			{
				if (value < FromLatitudeB)
					FromLatitudeB = value;
				_toLastitudeB = value;
				OnPropertyChanged(nameof(ToLatitudeB));
			}
		}

		public double FromLongitudeB
		{
			get => _fromLongitudeB;
			set
			{
				if (value > ToLongitudeB)
					ToLongitudeB = value;
				_fromLongitudeB = value;
				OnPropertyChanged(nameof(FromLongitudeB));
			}
		}

		public double ToLongitudeB
		{
			get => _toLongitudeB;
			set
			{
				if (value < FromLongitudeB)
					FromLongitudeB = value;
				_toLongitudeB = value;
				OnPropertyChanged(nameof(ToLongitudeB));
			}
		}


		public bool FilterPredicate(ObjectLink link)
		{
			if (DateFilter)
				if (link.Date < FromDate || link.Date > ToDate)
					return false;
			

			if (DirectionFilter)
				if (link.Direction != Direction)
					return false;
			
			if (IntensityFilter)
				if (link.Intensity != Intensity)
					return false;

			if (ObjectAFilter)
				if (!link.ObjectA.Object.Equals(ObjectA))
					return false;

			if (TypeAFilter)
				if (!link.ObjectA.Type.Equals(TypeA))
					return false;

			if (LastitudeAFilter)
				if (link.ObjectA.Latitude < FromLatitudeA || link.ObjectA.Latitude > ToLatitudeA)
					return false;

			if (LongitudeAFilter)
				if (link.ObjectA.Longitude < FromLongitudeA || link.ObjectA.Longitude > ToLongitudeA)
					return false;


			if (ObjectBFilter)
				if (!link.ObjectB.Object.Equals(ObjectB))
					return false;

			if (TypeBFilter)
				if (!link.ObjectB.Type.Equals(TypeB))
					return false;

			if (LastitudeBFilter)
				if (link.ObjectB.Latitude < FromLatitudeB || link.ObjectB.Latitude > ToLatitudeB)
					return false;

			if (LongitudeBFilter)
				if (link.ObjectB.Longitude < FromLongitudeB || link.ObjectB.Longitude > ToLongitudeB)
					return false;

			return true;
		}


	}
}