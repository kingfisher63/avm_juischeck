using System.Xml.Serialization;

namespace JuisCheck.XML
{
	[XmlType("Setting")]
	public class JC2Setting
	{
		// Note: the member order determines the order of attributes in the XML output.

		[XmlAttribute("name")]	public string Name	{ get; set; }

		// Note: the member order determines the order of elements in the XML output.

		[XmlElement("Value")]	public string Value { get; set; }

		/****************/
		/* Constructors */
		/****************/

		public JC2Setting()
		{
		}

		public JC2Setting( string name, string value )
		{
			Name  = name;
			Value = value;
		}
	}
}
