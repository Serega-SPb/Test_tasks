using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace NTT_Test.Logics
{
	public struct FileLoader
	{
		private static readonly Regex FileTemplate = new Regex(@"([\w\d\.\#]*);");
		private static string[] _header;
		private static bool _stop;	

		public static void LoadFile(string filename)
		{
			if (!File.Exists(filename))
			{
				MessageBox.Show("File not found", "Error");
				return;
			}
			var errors = new List<string>();
			_stop = false;
			var data = File.ReadLines(filename);
			_header = data.First().Trim().Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
			var ia = GetIndexes(typeof(TestObject), "A");
			var ib = GetIndexes(typeof(TestObject), "B");
			var il = GetIndexes(typeof(ObjectLink));

			var totalLines = data.Count()-1;
			int current = 0;

			foreach (var d in data.Skip(1))
			{
				if(_stop)return;
				current++;
				OnChangeCount(current,totalLines);

				var m =FileTemplate.Matches(d);
				if (m.Count != _header.Length)
				{
					errors.Add(d);
					continue;
				}

				try
				{
					var temp = d.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);

					var objA = new TestObject()
					{
						Object = temp[ia[0]].Trim(),
						Type = temp[ia[1]].Trim(),
						Latitude = double.Parse(temp[ia[2]].Trim(), CultureInfo.InvariantCulture),
						Longitude = double.Parse(temp[ia[3]].Trim(), CultureInfo.InvariantCulture)
					};
					var objB = new TestObject()
					{
						Object = temp[ib[0]].Trim(),
						Type = temp[ib[1]].Trim(),
						Latitude = double.Parse(temp[ib[2]].Trim(), CultureInfo.InvariantCulture),
						Longitude = double.Parse(temp[ib[3]].Trim(), CultureInfo.InvariantCulture)
					};
					var link = new ObjectLink()
					{
						ObjectA = objA,
						ObjectB = objB,
						Date = DateTime.Parse(temp[il[0]].Trim()),
						Direction = (Direction)Enum.Parse(typeof(Direction), temp[il[1]].Trim()),
						Color = (Color)new ColorConverter().ConvertFrom(temp[il[2]].Trim()),
						Intensity = int.Parse(temp[il[3]].Trim())
					};
					OnLoad(link);
				}
				catch (Exception e)
				{
					errors.Add(d);
				}
			}
			if(errors.Count>0)
				Report(errors);
			MessageBox.Show("File readed","Finish",MessageBoxButton.OK,MessageBoxImage.Information);
		}

		private static void Report(List<string> errors)
		{
			var mes = string.Empty;
			foreach (var error in errors)
			{
				mes += error + Environment.NewLine;
			}
			MessageBox.Show(mes, "Lines with error", MessageBoxButton.OK,MessageBoxImage.Error);
		}

		public static void Stop()
		{
			_stop = true;
			MessageBox.Show("Reading stoped", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private static IList<int> GetIndexes(Type type,string postfix = null)
		{
			var result = new List<int>();

			foreach (var property in type.GetProperties())
			{
				for (int i = 0; i < _header.Length; i++)
				{
					var temp = _header[i];
					if (!temp.Contains(property.Name)) continue;
					if (!string.IsNullOrWhiteSpace(postfix) && !temp.EndsWith(postfix)) continue;
					result.Add(i);
					break;
				}
			}
			return result;
		}
		#region Events

		public delegate void LoadHandler(ObjectLink link);
		public static event LoadHandler LoadEvent;
		private static void OnLoad(ObjectLink link)
		{
			LoadEvent?.Invoke(link);
		}

		public delegate void CounterHandler(int current, int total);
		public static event CounterHandler CounterEvent;
		private static void OnChangeCount(int current, int total)
		{
			CounterEvent?.Invoke(current,total);
		}

		#endregion
	}
}