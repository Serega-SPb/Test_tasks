﻿using System;
using System.Xml.Serialization;

namespace Test_TaskModel_Viewer.Classes
{
	[Serializable]
	public class Visual
	{
		[XmlElement("id")]
		public int Id { get; set; }

		[XmlElement("parent_id")]
		public int? ParentId { get; set; }

		[XmlElement("name")]
		public string Name { get; set; }

		[XmlElement("id_technology_card")]
		public int? IdTechnologyCard { get; set; }

		[XmlElement("start_date")]
		public DateTime StartDate { get; set; }

		[XmlElement("end_date")]
		public DateTime EndDate { get; set; }
	}
}