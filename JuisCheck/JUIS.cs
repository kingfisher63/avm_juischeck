/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.Globalization;
using System.Text;

namespace JuisCheck
{
	static class JUIS
	{
		private static readonly AVM.JUIS.JUIS juis = new AVM.JUIS.JUIS();

		private static AVM.JUIS.RequestHeader GetRequestHeader()
		{
			string timestamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

			return new AVM.JUIS.RequestHeader
			{
				ManualRequest = true,
				Nonce         = Convert.ToBase64String(Encoding.UTF8.GetBytes(timestamp)),
				UserAgent     = "Box"
			};
		}

		public static AVM.JUIS.UpdateInfo BoxFirmwareUpdateCheck(AVM.JUIS.BoxInfo boxInfo, AVM.JUIS.BoxInfo boxInfoMeshMaster)
		{
			return juis.BoxFirmwareUpdateCheck(GetRequestHeader(), boxInfo, boxInfoMeshMaster).UpdateInfo;
		}

		public static AVM.JUIS.UpdateInfo DeviceFirmwareUpdateCheck(AVM.JUIS.BoxInfo boxInfo, AVM.JUIS.DeviceInfo deviceInfo)
		{
			return juis.DeviceFirmwareUpdateCheck(GetRequestHeader(), boxInfo, deviceInfo).UpdateInfo;
		}

		public static string GetServerName()
		{
			return new Uri(juis.Url).Host;
		}
	}
}
