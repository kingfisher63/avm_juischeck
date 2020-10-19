using System;
using System.Xml.Serialization;

namespace JuisCheck.XML
{
	[XmlType("DectDevice")]
	public class JC2DectDevice
	{
		// Note: the member order determines the order of attributes in the XML output.

		[XmlAttribute("id")]				public string		ID					{ get; set; }

		// Note: the member order determines the order of elements in the XML output.

		[XmlElement("DeviceName")]			public string		DeviceName			{ get; set; }
		[XmlElement("ProductName")]			public string		ProductName			{ get; set; }
		[XmlElement("HardwareMajor")]		public int			HardwareMajor		{ get; set; }
		[XmlElement("HardwareMinor")]		public int			HardwareMinor		{ get; set; }
		[XmlElement("FirmwareMajor")]		public int			FirmwareMajor		{ get; set; }
		[XmlElement("FirmwareMinor")]		public int			FirmwareMinor		{ get; set; }
		[XmlElement("FirmwareMinor2")]		public int			FirmwareMinor2		{ get; set; }
		[XmlElement("FirmwareMinor3")]		public int			FirmwareMinor3		{ get; set; }
		[XmlElement("FirmwareMinor4")]		public int			FirmwareMinor4		{ get; set; }
		[XmlElement("OEM")]					public string		OEM					{ get; set; }
		[XmlElement("Country")]				public string		Country				{ get; set; }
		[XmlElement("Language")]			public string		Language			{ get; set; }
		[XmlElement("DectBase")]			public string		DectBase			{ get; set; }
		[XmlElement("UpdateAvailable")]		public bool			UpdateAvailable		{ get; set; }
		[XmlElement("UpdateInfo")]			public string		UpdateInfo			{ get; set; }
		[XmlElement("UpdateImageURL")]		public string		UpdateImageURL		{ get; set; }
		[XmlElement("UpdateInfoURL")]		public string		UpdateInfoURL		{ get; set; }
		[XmlElement("UpdateLastChecked")]	public DateTime?	UpdateLastChecked	{ get; set; }

		public JC2DectDevice()
		{
		}

		public JC2DectDevice( DectDevice dectDevice )
		{
			if (dectDevice == null) {
				throw new ArgumentNullException(nameof(dectDevice));
			}

			ID                = dectDevice.ID;

			DeviceName        = dectDevice.DeviceName;
			ProductName       = dectDevice.ProductName;
			HardwareMajor     = dectDevice.HardwareMajor;
			HardwareMinor     = dectDevice.HardwareMinor;
			FirmwareMajor     = dectDevice.FirmwareMajor;
			FirmwareMinor     = dectDevice.FirmwareMinor;
			FirmwareMinor2    = dectDevice.FirmwareMinor2;
			FirmwareMinor3    = dectDevice.FirmwareMinor3;
			FirmwareMinor4    = dectDevice.FirmwareMinor4;
			OEM               = dectDevice.OEM;
			Country           = dectDevice.Country;
			Language          = dectDevice.Language;
			DectBase          = dectDevice.DectBase;
			UpdateAvailable   = dectDevice.UpdateAvailable;
			UpdateInfo        = dectDevice.UpdateInfo;
			UpdateImageURL    = dectDevice.UpdateImageURL;
			UpdateInfoURL     = dectDevice.UpdateInfoURL;
			UpdateLastChecked = dectDevice.UpdateLastChecked;
		}
	}
}
