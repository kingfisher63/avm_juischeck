/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace JuisCheck.Jason
{
	[XmlRoot(ElementName = "BoxInfo", Namespace = "http://jason.avm.de/updatecheck/")]
	public class BoxInfo
	{
		[XmlElement(ElementName = "Name")]			public string		Name			{ get; set; }
		[XmlElement(ElementName = "HW")]			public int			HW				{ get; set; }
		[XmlElement(ElementName = "Version")]		public string		Version			{ get; set; }
		[XmlElement(ElementName = "Revision")]		public int			Revision		{ get; set; }
		[XmlElement(ElementName = "Serial")]		public string		Serial			{ get; set; }
		[XmlElement(ElementName = "OEM")]			public string		OEM				{ get; set; }
		[XmlElement(ElementName = "Lang")]			public string		Lang			{ get; set; }
		[XmlElement(ElementName = "Annex")]			public string		Annex			{ get; set; }
		[XmlElement(ElementName = "Lab")]			public string		Lab				{ get; set; }
		[XmlElement(ElementName = "Country")]		public string		Country			{ get; set; }
		[XmlElement(ElementName = "Flag")]			public List<string>	Flag			{ get; } = new List<string>();
		[XmlElement(ElementName = "UpdateConfig")]	public int			UpdateConfig	{ get; set; }
	}
}
