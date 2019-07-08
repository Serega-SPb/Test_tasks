using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Win32;
using Test_TaskModel_Viewer.Classes;
using Test_TaskModel_Viewer.Extensions;

namespace Test_TaskModel_Viewer.MVVM
{
	public class MainViewModel:NotificationBase
	{
		private Model _model;
		private ModelRelation _modelRelation;
		private string _file = "test_task_model";

		public MainViewModel()
		{
			//SaveCommand = new DelegateCommand(Save);
			//OpenCommand = new DelegateCommand(Open);
			LoadModel(_file);
		}

		//public DelegateCommand OpenCommand { get; }
		//public DelegateCommand SaveCommand { get; }

		public ModelRelation ModelRelation
		{
			get => _modelRelation;
			set
			{
				_modelRelation = value;
				OnPropertyChanged(nameof(ModelRelation));
			}
		}

		private void LoadModel(string file)
		{
			if (!File.Exists(file))
				return;
			
			_model = ModelSerializer.Deserialize(file);
			ModelRelation = GetRelation().First();
			AddChildren(ModelRelation);
		}

		private void AddChildren(ModelRelation relation)
		{
			relation.AddRange(GetRelation(relation.Id));
			foreach (var child in relation.Children)
			{
				new Thread(() => { AddChildren(child); }).Start();
			}
		}

		private IEnumerable<ModelRelation> GetRelation(int? parentId = null)
		{
			var visuals = _model.Visual.Where(x => x.ParentId == parentId);
			if (!visuals.Any())
				yield break;
			foreach (var visual in visuals)
			{
				var graph = _model.Graph.FirstOrDefault(x => x.IdTechnologyCard == visual.IdTechnologyCard);
				yield return new ModelRelation(visual, graph);
			}
		}

		/*private void Open()
		{
			var file = OpenFileDialog(AppDomain.CurrentDomain.BaseDirectory);
			if (string.IsNullOrWhiteSpace(file))
				return;
			LoadModel(file);
		}

		private void Save()
		{
			var model = ModelRelation.GetModel();
			var file = SaveFileDialog(AppDomain.CurrentDomain.BaseDirectory);
			if(string.IsNullOrWhiteSpace(file))
				return;
			ModelSerializer.Serialize(file, model);
		}*/


		private string OpenFileDialog(string file)
		{
			if (string.IsNullOrWhiteSpace(file))
				file = AppDomain.CurrentDomain.BaseDirectory;

			var dialog = new OpenFileDialog()
			{
				InitialDirectory = file,
				Multiselect = false
			};

			var answer = dialog.ShowDialog();
			if (answer.Value != true)
				return string.Empty;
			return dialog.FileName;
		}
		private string SaveFileDialog(string file)
		{
			var dialog = new SaveFileDialog()
			{
				InitialDirectory = file
			};
			var answer = dialog.ShowDialog();
			if (answer.Value != true)
				return string.Empty;
			return dialog.FileName;
		}
	}
}