using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace JuisCheck.XML
{
	public class JC1Device
	{
		[XmlAttribute("kind")]				public JC1DeviceKind	DeviceKind			{ get; set; } = JC1DeviceKind.Unknown;

		[XmlElement("DeviceName")]			public string			DeviceName			{ get; set; }
		[XmlElement("DeviceAddress")]		public string			DeviceAddress		{ get; set; }
		[XmlElement("ProductName")]			public string			ProductName			{ get; set; }
		[XmlElement("HardwareMajor")]		public int				HardwareMajor		{ get; set; }
		[XmlElement("HardwareMinor")]		public int				HardwareMinor		{ get; set; }
		[XmlElement("SerialNumber")]		public string			SerialNumber		{ get; set; }
		[XmlElement("FirmwareMajor")]		public int				FirmwareMajor		{ get; set; }
		[XmlElement("FirmwareMinor")]		public int				FirmwareMinor		{ get; set; }
		[XmlElement("FirmwarePatch")]		public int				FirmwarePatch		{ get; set; }
		[XmlElement("FirmwareBuildNumber")]	public int				FirmwareBuildNumber	{ get; set; }
		[XmlElement("FirmwareBuildType")]	public int				FirmwareBuildType	{ get; set; }
		[XmlElement("OEM")]					public string			OEM					{ get; set; }
		[XmlElement("Annex")]				public string			Annex				{ get; set; }
		[XmlElement("Country")]				public string			Country				{ get; set; }
		[XmlElement("Language")]			public string			Language			{ get; set; }
		[XmlElement("Flag")]				public List<string>		Flags				{ get; } = new List<string>();
		[XmlElement("BaseFritzOS")]			public string			BaseFritzOS			{ get; set; }	// Not used in JuisCheck 2.x
		[XmlElement("UpdateAvailable")]		public bool				UpdateAvailable		{ get; set; }
		[XmlElement("UpdateInfo")]			public string			UpdateInfo			{ get; set; }
		[XmlElement("UpdateImageURL")]		public string			UpdateImageURL		{ get; set; }
		[XmlElement("UpdateInfoURL")]		public string			UpdateInfoURL		{ get; set; }
		[XmlElement("UpdateLastChecked")]	public DateTime?		UpdateLastChecked	{ get; set; }

		public JC1Device()
		{
		}
	}
}
