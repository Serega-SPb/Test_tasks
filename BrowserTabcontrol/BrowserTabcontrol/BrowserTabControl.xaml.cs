using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BrowserTabcontrol
{
	public partial class BrowserTabControl
	{
		private Task _clearer;

		public BrowserTabControl()
		{
			InitializeComponent();
			CollectionViewSource.GetDefaultView(Items).CollectionChanged += (s, e) =>  ChangeSize();
			SizeChanged += (s, e) => ChangeSize();
		}

		#region Dependency property

		public static readonly DependencyProperty MinHeaderWidthProperty = DependencyProperty.Register(
			"MinHeaderWidth", typeof(double), typeof(BrowserTabControl), new UIPropertyMetadata(5d));

		public double MinHeaderWidth
		{
			get { return (double) GetValue(MinHeaderWidthProperty); }
			set { SetValue(MinHeaderWidthProperty, value); }
		}

		public static readonly DependencyProperty MaxHeaderWidthProperty = DependencyProperty.Register(
			"MaxHeaderWidth", typeof(double), typeof(BrowserTabControl), new UIPropertyMetadata(150d));

		public double MaxHeaderWidth
		{
			get { return (double) GetValue(MaxHeaderWidthProperty); }
			set { SetValue(MaxHeaderWidthProperty, value); }
		}

		public static readonly DependencyProperty HeaderWidthProperty = DependencyProperty.Register(
			"HeaderWidth", typeof(double), typeof(BrowserTabControl), new UIPropertyMetadata(default(double)));

		public double HeaderWidth
		{
			get { return (double)GetValue(HeaderWidthProperty); }
			set { SetValue(HeaderWidthProperty, value); }
		}

		#endregion

		private void OnPrevMouseLeftBtnDown(object sender, MouseButtonEventArgs e)
		{
			var drag = sender as TabItem;

			var btn = GetParent<Button>(e.OriginalSource as DependencyObject);
			if (btn != null && btn.Tag.Equals("Close"))
			{
				CloseTab(drag);
				return;
			}
			
			if (drag !=null)
			{
				DragDrop.DoDragDrop(drag, drag, DragDropEffects.Move);
				drag.IsSelected = true;
			}
		}

		private void OnDrop(object sender, DragEventArgs e)
		{
			var drop = e.Data.GetData(typeof(TabItem)) as TabItem;
			var target = sender as TabItem;

			if(drop == null || target == null || drop.Equals(target))
				return;

			var indexTarget = Items.IndexOf(target);
			Items.Remove(drop);
			Items.Insert(indexTarget, drop);

		}

		private void CloseTab(TabItem item)
		{
			Items.Remove(item);
			item.Content = null;
			item.IsSelected = false;

			if (_clearer == null)
			{
				_clearer = new Task(Unload);
				_clearer.Start();
			}
			else
				_clearer.ContinueWith(o => Unload());
		}

		private void Unload()
		{
			Thread.Sleep(10);
			GC.Collect();
		}

		private void ChangeSize()
		{
			var count = Items.Count;
			var result = (ActualWidth - 50) / (count + 1) - 12;

			if (result < MinHeaderWidth)
			{
				HeaderWidth = MinHeaderWidth;
				return;
			}
			HeaderWidth = result;
		}

		private T GetParent<T>(DependencyObject element) where T : DependencyObject
		{
			DependencyObject parent = VisualTreeHelper.GetParent(element);

			if (parent == null)
				return null;

			var result = parent as T;
			if (result != null)
				return result;

			return GetParent<T>(parent);
		}

		private void CreateTabClick(object sender, RoutedEventArgs e)
		{
			var item = new TabItem()
			{
				Header = $"Tab {Items.Count + 1}",
				Content = CreateBorder(),
			};
			if (Items.Count == 0)
				item.IsSelected = true;
			Items.Add(item);
		}

		private Random _rnd = new Random();
		private Border CreateBorder()
		{
			return new Border()
			{
				Background = new SolidColorBrush(Color.FromRgb((byte)_rnd.Next(255), (byte)_rnd.Next(255), (byte)_rnd.Next(255))),
				HorizontalAlignment = HorizontalAlignment.Stretch,
				VerticalAlignment = VerticalAlignment.Stretch,		
			};
		}
	}
}
