using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace JuisCheck.XML
{
	[XmlRoot("Devices")]
	public class JC1Data
	{
		[XmlElement("Device")]
		public List<JC1Device>	Devices { get; } = new List<JC1Device>();

		public static void LoadFromString( string xml, DeviceCollection devices )
		{
			if (xml == null) {
				throw new ArgumentNullException(nameof(xml));
			}
			if (devices == null) {
				throw new ArgumentNullException(nameof(devices));
			}

			XmlReaderSettings	xmlReaderSettings = new XmlReaderSettings() { IgnoreComments = true, IgnoreWhitespace = true };
			XmlSerializer		xmlSerializer     = new XmlSerializer(typeof(JC1Data));

			using (XmlReader xmlReader = XmlReader.Create(new StringReader(xml), xmlReaderSettings)) {
				JC1Data jc1data = (JC1Data)xmlSerializer.Deserialize(xmlReader);

				devices.Clear();
				foreach (JC1Device jc1device in jc1data.Devices) {
					if (jc1device.DeviceKind == JC1DeviceKind.DECT) {
						devices.Add(new DectDevice(jc1device));
					}
					if (jc1device.DeviceKind == JC1DeviceKind.JUIS) {
						devices.Add(new JuisDevice(jc1device));
					}
				}

				devices.Settings.Reset();
			}
		}
	}
}
