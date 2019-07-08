using System;
using System.Windows.Media;

namespace NTT_Test.Logics
{
	public class ObjectLink
	{
		public TestObject ObjectA { get; set; }

		public TestObject ObjectB { get; set; }

		public DateTime Date { get; set; }

		public Direction Direction { get; set; }

		public Color Color { get; set; }

		public int Intensity { get; set; }
	}
}