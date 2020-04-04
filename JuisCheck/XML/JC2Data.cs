using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace JuisCheck.XML
{
	[XmlRoot("JuisCheck2")]
	public class JC2Data
	{
		public const string xmlRootElementName = "JuisCheck2";

		// Note: the member order determines the order of elements in the XML output.

		[XmlArray("DectDevices")]	public List<JC2DectDevice>	DectDevices	{ get; } = new List<JC2DectDevice>();
		[XmlArray("JuisDevices")]	public List<JC2JuisDevice>	JuisDevices	{ get; } = new List<JC2JuisDevice>();
		[XmlArray("Settings")]		public List<JC2Setting>		Settings	{ get; } = new List<JC2Setting>();

		public static void LoadFromString( string xml, DeviceCollection devices )
		{
			if (xml == null) {
				throw new ArgumentNullException(nameof(xml));
			}
			if (devices == null) {
				throw new ArgumentNullException(nameof(devices));
			}

			XmlReaderSettings	xmlReaderSettings = new XmlReaderSettings() { IgnoreComments = true, IgnoreWhitespace = true };
			XmlSerializer		xmlSerializer     = new XmlSerializer(typeof(JC2Data));

			using (XmlReader xmlReader = XmlReader.Create(new StringReader(xml), xmlReaderSettings)) {
				JC2Data jc2data = (JC2Data)xmlSerializer.Deserialize(xmlReader);

				devices.Clear();
				foreach (JC2DectDevice jc2dectDevice in jc2data.DectDevices) {
					devices.Add(new DectDevice(jc2dectDevice));
				}
				foreach (JC2JuisDevice jc2juisDevice in jc2data.JuisDevices) {
					devices.Add(new JuisDevice(jc2juisDevice));
				}

				devices.Settings.Reset();
				foreach(JC2Setting setting in jc2data.Settings) {
					devices.Settings.Add(setting.Name, setting.Value);
				}
			}
		}

		public static void SaveToFile( string fileName, DeviceCollection devices )
		{
			if (fileName == null) {
				throw new ArgumentNullException(nameof(fileName));
			}
			if (devices == null) {
				throw new ArgumentNullException(nameof(devices));
			}

			using (XmlWriter xmlWriter = XmlWriter.Create(fileName, new XmlWriterSettings { Indent = true })) {
				JC2Data jc2data = new JC2Data();

				foreach (Device device in devices) {
					if (device is DectDevice dectDevice) {
						jc2data.DectDevices.Add(new JC2DectDevice(dectDevice));
					}
					if (device is JuisDevice juisDevice) {
						jc2data.JuisDevices.Add(new JC2JuisDevice(juisDevice));
					}
				}

				foreach (KeyValuePair<string,string> setting in devices.Settings.GetDictionary()) {
					jc2data.Settings.Add(new JC2Setting(setting.Key, setting.Value));
				}

				XmlSerializer xmlSerializer = new XmlSerializer(typeof(JC2Data));
				xmlSerializer.Serialize(xmlWriter, jc2data);
			}
		}
	}
}
