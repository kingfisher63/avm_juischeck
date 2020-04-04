using System;
using System.IO;

namespace JuisCheck.XML
{
	public static class JCData
	{
		public static bool LoadFromFile( string fileName, DeviceCollection devices )
		{
			if (fileName == null) {
				throw new ArgumentNullException(nameof(fileName));
			}
			if (devices == null) {
				throw new ArgumentNullException(nameof(devices));
			}

			// Read data only once

			string xml = File.ReadAllText(fileName);

			// Try JuisCheck2 file format

			try {
				JC2Data.LoadFromString(xml, devices);
				return false;
			}
			catch (InvalidOperationException) {
			}

			// Try JuisCheck1 file format

			JC1Data.LoadFromString(xml, devices);
			return true;
		}

		public static void SaveToFile( string fileName, DeviceCollection devices )
		{
			if (fileName == null) {
				throw new ArgumentNullException(nameof(fileName));
			}
			if (devices == null) {
				throw new ArgumentNullException(nameof(devices));
			}

			JC2Data.SaveToFile(fileName, devices);
		}
	}
}
