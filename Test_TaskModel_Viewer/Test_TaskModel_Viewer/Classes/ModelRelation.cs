using System;
using System.Collections.Generic;
using System.Linq;
using Test_TaskModel_Viewer.Extensions;

namespace Test_TaskModel_Viewer.Classes
{
	public class ModelRelation:NotificationBase
	{
		private bool _isExpanded;

		public ModelRelation(Visual visual, Graph graph)
		{
			Id = visual.Id;
			ParentId = visual.ParentId;
			IdSublayer1 = graph?.IdSublayer1 ?? 0;
			IdTechCard = visual.IdTechnologyCard;
			//VisualRange = new DateRange(visual.StartDate, visual.EndDate);
			GraphRange = graph != null ? new DateRange(graph.StartDate, graph.EndDate) : null;
			Name = visual.Name;

			//VisualRange.PropertyChanged += (s, e) => OnPropertyChanged(nameof(VisualRange));
			if (GraphRange != null)
				GraphRange.PropertyChanged += (s, e) => OnPropertyChanged(nameof(GraphRange));
		}

		public bool IsExpanded
		{
			get { return _isExpanded; }
			set
			{
				_isExpanded = value;
				UpdateStatus();
			}
		}

		public IEnumerable<ModelRelation> GetAllRelations => GetRelations();

		public bool IsShow => ParentStatus();

		public int IdSublayer1 { get; }

		public int? IdTechCard { get; }

		public DateRange GraphRange { get; private set; }

		//public DateRange VisualRange { get; }

		public int Id { get; }

		public int? ParentId { get; }

		public string Name { get; set; }

		public ModelRelation Parent { get; private set; }

		public List<ModelRelation> Children { get; } = new List<ModelRelation>();

		public void AddRange(IEnumerable<ModelRelation> children)
		{
			foreach (var child in children)
			{
				child.Parent = this;
				Children.Add(child);
			}
			Parent?.UpdateGraphRange();
		}

		//public Model GetModel()
		//{
		//	var model = new Model();

		//	var root = this;
		//	while (root.Parent!=null)
		//	{
		//		root = root.Parent;
		//	}

		//	var tuple = root.GetGrAndVis();
			
		//	model.Graph = tuple.Item1;
		//	model.Visual = tuple.Item2;
		//	return model;
		//}

		//private Tuple<List<Graph>, List<Visual>> GetGrAndVis()
		//{
		//	var graphs = new List<Graph>();
		//	var visuals = new List<Visual>();
		//	foreach (var child in Children)
		//	{
		//		if (!child.Children.Any())
		//		{
		//			graphs.Add(child.GetGraph());
		//		}
		//		visuals.Add(child.GetVisual());

		//		var tuple = child.GetGrAndVis();
		//		graphs.AddRange(tuple.Item1);
		//		visuals.AddRange(tuple.Item2);
		//	}
		//	return new Tuple<List<Graph>, List<Visual>>(graphs,visuals);
		//}

		//private Graph GetGraph()
		//{
		//	return new Graph()
		//	{
		//		IdTechnologyCard = IdTechCard ?? 0,
		//		IdSublayer1 = IdSublayer1,
		//		StartDate = GraphRange.Start,
		//		EndDate = GraphRange.End
		//	};
		//}

		//private Visual GetVisual()
		//{
		//	return new Visual()
		//	{
		//		Name = Name,
		//		Id = Id,
		//		IdTechnologyCard = IdTechCard,
		//		ParentId = ParentId,
		//		StartDate = GraphRange.Start,
		//		EndDate = GraphRange.End
		//	};
		//}


		public void ChangeRange(DateRange from, DateRange to)
		{

			var diff = from.DiffDaysStart(to.Start);

			if (Children.Count != 0)
				Shift(diff, diff);
			else
			{
				if (from.DateInRange(GraphRange.Start) && from.DateInRange(GraphRange.End))
				{
					if (diff > 0)
						Shift(0, diff);
					else
						Shift(diff, 0);
				}
				else
				{
					if (from.DateInRange(GraphRange.Start))
						Shift(diff, 0);

					if (from.DateInRange(GraphRange.End))
						Shift(0, diff);
				}
			}

			Parent?.UpdateGraphRange();
			OnPropertyChanged(nameof(GraphRange));
		}

		private void Shift(int start, int end)
		{
			GraphRange.Shift(start, end);
			foreach (var child in Children)
			{
				child.Shift(start,end);
			}
		}

		private IEnumerable<ModelRelation> GetRelations()
		{
			yield return this;
			foreach (var child in Children)
			{
				foreach (var relation in child.GetRelations())
				{
					yield return relation;
				}
			}
		}

		private void UpdateGraphRange()
		{
			if(Children.Any(x=>x.GraphRange == null))
				return;

			GraphRange = new DateRange(
				Children.Min(x => x.GraphRange.Start),
				Children.Max(x => x.GraphRange.End));
			GraphRange.PropertyChanged += (s, e) => OnPropertyChanged(nameof(GraphRange));
			Parent?.UpdateGraphRange();
			OnPropertyChanged(nameof(GraphRange));
		}

		private void UpdateStatus()
		{
			OnPropertyChanged(nameof(IsShow));
			foreach (var child in Children)
			{
				child.UpdateStatus();
			}
		}

		private bool ParentStatus()
		{
			return Parent == null || Parent.IsExpanded && Parent.ParentStatus();
		}
	}
}