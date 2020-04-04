using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace JuisCheck.XML
{
	[XmlType("JuisDevice")]
	public class JC2JuisDevice
	{
		// Note: the member order determines the order of attributes in the XML output.

		[XmlAttribute("id")]						public string		ID					{ get; set; }

		// Note: the member order determines the order of elements in the XML output.

		[XmlElement("DeviceName")]					public string		DeviceName			{ get; set; }
		[XmlElement("DeviceAddress")]				public string		DeviceAddress		{ get; set; }
		[XmlElement("ProductName")]					public string		ProductName			{ get; set; }
		[XmlElement("Hardware")]					public int			Hardware			{ get; set; }
		[XmlElement("SerialNumber")]				public string		SerialNumber		{ get; set; }
		[XmlElement("FirmwareMajor")]				public int			FirmwareMajor		{ get; set; }
		[XmlElement("FirmwareMinor")]				public int			FirmwareMinor		{ get; set; }
		[XmlElement("FirmwarePatch")]				public int			FirmwarePatch		{ get; set; }
		[XmlElement("FirmwareBuildNumber")]			public int			FirmwareBuildNumber	{ get; set; }
		[XmlElement("FirmwareBuildType")]			public int			FirmwareBuildType	{ get; set; }
		[XmlElement("OEM")]							public string		OEM					{ get; set; }
		[XmlElement("Annex")]						public string		Annex				{ get; set; }
		[XmlElement("Country")]						public string		Country				{ get; set; }
		[XmlElement("Language")]					public string		Language			{ get; set; }
		[XmlArray("Flags")][XmlArrayItem("Flag")]	public List<string>	Flags				{ get; } = new List<string>();
		[XmlElement("MeshMaster")]					public string		MeshMaster			{ get; set; }
		[XmlElement("UpdateAvailable")]				public bool			UpdateAvailable		{ get; set; }
		[XmlElement("UpdateInfo")]					public string		UpdateInfo			{ get; set; }
		[XmlElement("UpdateImageURL")]				public string		UpdateImageURL		{ get; set; }
		[XmlElement("UpdateInfoURL")]				public string		UpdateInfoURL		{ get; set; }
		[XmlElement("UpdateLastChecked")]			public DateTime?	UpdateLastChecked	{ get; set; }

		public JC2JuisDevice()
		{
		}

		public JC2JuisDevice( JuisDevice juisDevice )
		{
			if (juisDevice == null) {
				throw new ArgumentNullException(nameof(juisDevice));
			}

			ID                  = juisDevice.ID;

			DeviceName          = juisDevice.DeviceName;
			DeviceAddress       = juisDevice.DeviceAddress;
			ProductName         = juisDevice.ProductName;
			Hardware            = juisDevice.Hardware;
			SerialNumber        = juisDevice.SerialNumber;
			FirmwareMajor       = juisDevice.FirmwareMajor;
			FirmwareMinor       = juisDevice.FirmwareMinor;
			FirmwarePatch       = juisDevice.FirmwarePatch;
			FirmwareBuildNumber = juisDevice.FirmwareBuildNumber;
			FirmwareBuildType   = juisDevice.FirmwareBuildType;
			OEM                 = juisDevice.OEM;
			Annex               = juisDevice.Annex;
			Country             = juisDevice.Country;
			Language            = juisDevice.Language;
			Flags               = JuisDevice.FlagsToList(juisDevice.Flags);
			MeshMaster          = juisDevice.MeshMaster;
			UpdateAvailable     = juisDevice.UpdateAvailable;
			UpdateInfo          = juisDevice.UpdateInfo;
			UpdateImageURL      = juisDevice.UpdateImageURL;
			UpdateInfoURL       = juisDevice.UpdateInfoURL;
			UpdateLastChecked   = juisDevice.UpdateLastChecked;
		}
	}
}
