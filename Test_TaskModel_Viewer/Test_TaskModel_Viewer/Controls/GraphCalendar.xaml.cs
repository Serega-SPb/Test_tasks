using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Test_TaskModel_Viewer.Classes;
using Test_TaskModel_Viewer.Extensions;

namespace Test_TaskModel_Viewer.Controls
{

	public partial class GraphCalendar
	{
		public GraphCalendar()
		{
			InitializeComponent();
		}

		public static readonly DependencyProperty ModelRelationProperty = DependencyProperty.Register(
			"ModelRelation", typeof(ModelRelation), typeof(GraphCalendar), new PropertyMetadata(default(ModelRelation),OnModelsChanged));		

		public ModelRelation ModelRelation
		{
			get { return (ModelRelation) GetValue(ModelRelationProperty); }
			set { SetValue(ModelRelationProperty, value); }
		}

		private static void OnModelsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = (GraphCalendar) d;
			control.CreateHeader();
		}

		private void CreateHeader()
		{
			View.Columns.Clear();

			var relations = ModelRelation.GetAllRelations.ToArray();

			var min = relations.Min(m => m.GraphRange?.Start);
			var max = relations.Max(m => m.GraphRange?.End);
			if(min == null || max == null)
				return;

			var range = new DateRange((DateTime)min, (DateTime)max);

			foreach (var date in range.GetMonthYearArray())
			{
				var cc = new FrameworkElementFactory(typeof(ContentControl));
				cc.SetValue(TagProperty,CreateTag(date));
				cc.SetBinding(ContentProperty,new Binding());

				var dt = new DataTemplate()
				{
					VisualTree = cc
				};
				View.Columns.Add(new GridViewColumn { Header = date, CellTemplate = dt });
			}
		}

		private Dictionary<string, DateRange> CreateTag(DateTime date)
		{
			return new Dictionary<string, DateRange>()
			{
				{"1", new DateRange(date.Date, date.AddDays(9))},
				{"11", new DateRange(date.Date.AddDays(10), date.AddDays(19))},
				{"21", new DateRange(date.Date.AddDays(20), date.AddMonths(1).AddDays(-1))},
			};
		}

		private Func<DependencyObject, DependencyObject> _getParent = VisualTreeHelper.GetParent;
		private Func<DependencyObject, int, DependencyObject> _getChild = VisualTreeHelper.GetChild;

		private void OpenPopupClick(object sender, RoutedEventArgs e)
		{
			if (!(sender is Button btn))
				return;

			var popup = _getChild(_getChild(_getChild(_getChild(btn, 0), 0), 0), 0) as Popup;
			popup.IsOpen = true;
		}

		private void ClosePopupClick(object sender, RoutedEventArgs e)
		{
			if (sender is Button btn && btn.Tag is Popup p)
				p.IsOpen = false;
			e.Handled = true;
		}

		private bool _isStartChanging;
		private ModelRelation _changingModel;
		
		private void StartChangeRange(object sender, MouseButtonEventArgs e)
		{
			if (!(sender is Button btn))
				return;
			if ((bool) btn.GetValue(AttachedProperties.IsCheckedProperty))
			{
				_changingModel = btn.DataContext as ModelRelation;
				_isStartChanging = true;

				var tag = btn.Tag.ToString();
				var dict = ((ContentControl)_getParent(_getParent(_getParent(btn)))).Tag as Dictionary<string, DateRange>;
				var range = dict[tag];

				DragDrop.DoDragDrop(btn, range, DragDropEffects.Link);
			}
		}
		
		private void EndChangeRange(object sender, DragEventArgs e)
		{
			if (!(sender is Button btn) || !_isStartChanging)
				return;

			var from =  e.Data.GetData(typeof(DateRange));

			var tag = btn.Tag.ToString();
			var dict = ((ContentControl)_getParent(_getParent(_getParent(btn)))).Tag as Dictionary<string, DateRange>;

			var range = dict[tag];

			_changingModel.ChangeRange((DateRange)from, range);
			
			_isStartChanging = false;
			_changingModel = null;
		}
	}
}
