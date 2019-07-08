using System;
using System.Xml.Serialization;

namespace Test_TaskModel_Viewer.Classes
{
	[Serializable]
	public class Graph
	{
		[XmlElement("id_sublayer1")]
		public int IdSublayer1 { get; set; }

		[XmlElement("id_technology_card")]
		public int IdTechnologyCard { get; set; }

		[XmlElement("start_date")]
		public DateTime StartDate { get; set; }

		[XmlElement("end_date")]
		public DateTime EndDate { get; set; }
	}
}