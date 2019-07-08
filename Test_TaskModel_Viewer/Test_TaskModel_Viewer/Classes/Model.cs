using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Test_TaskModel_Viewer.Classes
{
	[Serializable]
	public class Model
	{
		[XmlArray("graph")]
		[XmlArrayItem("Graph")]
		public List<Graph> Graph { get; set; }

		[XmlArray("visual")]
		[XmlArrayItem("Visual")]
		public List<Visual> Visual { get; set; }

	}
}