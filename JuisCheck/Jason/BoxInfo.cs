using System;
using System.Xml.Serialization;

namespace JuisCheck.Jason
{
	[Serializable]
	[XmlType(TypeName = "BoxInfo", Namespace = "http://jason.avm.de/updatecheck/")]
	public class BoxInfo
	{
		[XmlElement(ElementName = "Name")]			public string	Name			{ get; set; }
		[XmlElement(ElementName = "HW")]			public int		HW				{ get; set; }
		[XmlElement(ElementName = "Version")]		public string	Version			{ get; set; }
		[XmlElement(ElementName = "Revision")]		public int		Revision		{ get; set; }
		[XmlElement(ElementName = "Serial")]		public string	Serial			{ get; set; }
		[XmlElement(ElementName = "OEM")]			public string	OEM				{ get; set; }
		[XmlElement(ElementName = "Lang")]			public string	Lang			{ get; set; }
		[XmlElement(ElementName = "Annex")]			public string	Annex			{ get; set; }
		[XmlElement(ElementName = "Lab")]			public string	Lab				{ get; set; }
		[XmlElement(ElementName = "Country")]		public string	Country			{ get; set; }
		[XmlElement(ElementName = "Flag")]			public string[]	Flag			{ get; set; }
		[XmlElement(ElementName = "UpdateConfig")]	public int		UpdateConfig	{ get; set; }
	}
}
