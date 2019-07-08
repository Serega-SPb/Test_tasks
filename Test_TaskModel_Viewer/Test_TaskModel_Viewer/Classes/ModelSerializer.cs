using System.IO;
using System.Xml.Serialization;

namespace Test_TaskModel_Viewer.Classes
{
	public static class ModelSerializer
	{
		public static void Serialize(string file, Model model)
		{
			using (var writer = new FileStream(file,FileMode.Create,FileAccess.Write))
			{
				var serializer = new XmlSerializer(typeof(Model));
				serializer.Serialize(writer,model);
			}
		}

		public static Model Deserialize(string file)
		{
			if (!File.Exists(file))
				return null;

			using (var reader = new FileStream(file,FileMode.Open,FileAccess.Read))
			{
				var serializer = new XmlSerializer(typeof(Model));
				var temp = serializer.Deserialize(reader);
				return (Model) temp;
			}
		}
	}
}