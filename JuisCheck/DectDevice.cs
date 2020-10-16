/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Threading;

using JuisCheck.Lang;
using JuisCheck.Properties;

namespace JuisCheck
{
	public class DectDevice : Device
	{
		public const string					predefinedDectBaseID       = "63022b32-3ecb-436d-bc6c-00861df70fff";
		public const string					predefinedDectBaseProduct  = "FRITZ!Box 7590";
		public const string					predefinedDectBaseFritzOS  = "7.21";
		public const string					predefinedDectBaseFirmware = "154.07.21";

		private static readonly Settings	programSettings            = Settings.Default;

		/***********************************/
		/* Device type specific properties */
		/* - including derived properties  */
		/* - in alphabetical order         */
		/***********************************/

		private string _DectBase;
		public  string  DectBase
		{
			get => _DectBase ?? string.Empty;
			set {
				if (_DectBase != value) {
					_DectBase  = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(MasterBaseStr));
				}
			}
		}

		public override string DeviceAddressStr
		{
			get => string.Empty;
		}

		public override string FirmwareBuildTypeStr
		{
			get => string.Empty;
		}

		private int _FirmwareMinorLen;
		public  int  FirmwareMinorLen
		{
			get => _FirmwareMinorLen;
			set {
				if (_FirmwareMinorLen != value) {
					_FirmwareMinorLen  = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(FirmwareStr));
				}
			}
		}

		public override string FirmwareStr
		{
			get {
				int minorDigits = FirmwareMinorLen != 0 ? FirmwareMinorLen : 2;
				return $"{FirmwareMajor:D2}.{FirmwareMinor.ToString(CultureInfo.InvariantCulture).PadLeft(minorDigits, '0')}";
			}
		}

		private int _HardwareMajor;
		public  int  HardwareMajor
		{
			get => _HardwareMajor;
			set {
				if (_HardwareMajor != value) {
					_HardwareMajor  = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(HardwareStr));
				}
			}
		}

		private int _HardwareMinor;
		public  int  HardwareMinor
		{
			get => _HardwareMinor;
			set {
				if (_HardwareMinor != value) {
					_HardwareMinor  = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(HardwareStr));
				}
			}
		}

		public override string HardwareStr
		{
			get => $"{HardwareMajor:D2}.{HardwareMinor:D2}";
		}

		public override string MasterBaseStr
		{
			get {
				if (App.GetMainWindow().Devices.FindByID(DectBase) is JuisDevice dectBase) {
					return dectBase.DeviceName;
				}
				if (DectBase == predefinedDectBaseID) {
					return GetPredefinedDectBaseText();
				}
				return string.Empty;
			}
		}

		public override DataTemplate ToolTipTemplate
		{
			get => (DataTemplate)App.GetMainWindow().Resources["DectDeviceToolTipContentTemplate"];
		}

		/**********************/
		/* Class constructors */
		/**********************/

		public DectDevice()
		{
			ID = Guid.NewGuid().ToString();
		}

		public DectDevice( DectDevice srcDevice )
		{
			if (srcDevice == null) {
				throw new ArgumentNullException(nameof(srcDevice));
			}

			ID = Guid.NewGuid().ToString();
			CopyFrom(srcDevice);
		}

		public DectDevice( XML.JC1Device jc1device )
		{
			if (jc1device == null) {
				throw new ArgumentNullException(nameof(jc1device));
			}

			ID                = Guid.NewGuid().ToString();

			DeviceName        = jc1device.DeviceName;
			ProductName       = jc1device.ProductName;
			HardwareMajor     = jc1device.HardwareMajor;
			HardwareMinor     = jc1device.HardwareMinor;
			FirmwareMajor     = jc1device.FirmwareMajor;
			FirmwareMinor     = jc1device.FirmwareMinor;
			OEM               = jc1device.OEM;
			Country           = jc1device.Country;
			Language          = jc1device.Language;
			UpdateAvailable   = jc1device.UpdateAvailable;
			UpdateInfo        = jc1device.UpdateInfo;
			UpdateImageURL    = jc1device.UpdateImageURL;
			UpdateInfoURL     = jc1device.UpdateInfoURL;
			UpdateLastChecked = jc1device.UpdateLastChecked;
		}

		public DectDevice( XML.JC2DectDevice jc2dectDevice )
		{
			if (jc2dectDevice == null) {
				throw new ArgumentNullException(nameof(jc2dectDevice));
			}

			ID                = jc2dectDevice.ID;

			DeviceName        = jc2dectDevice.DeviceName;
			ProductName       = jc2dectDevice.ProductName;
			HardwareMajor     = jc2dectDevice.HardwareMajor;
			HardwareMinor     = jc2dectDevice.HardwareMinor;
			FirmwareMajor     = jc2dectDevice.FirmwareMajor;
			FirmwareMinor     = jc2dectDevice.FirmwareMinor;
			FirmwareMinorLen  = jc2dectDevice.FirmwareMinorLen;
			OEM               = jc2dectDevice.OEM;
			Country           = jc2dectDevice.Country;
			Language          = jc2dectDevice.Language;
			DectBase          = jc2dectDevice.DectBase;
			UpdateAvailable   = jc2dectDevice.UpdateAvailable;
			UpdateInfo        = jc2dectDevice.UpdateInfo;
			UpdateImageURL    = jc2dectDevice.UpdateImageURL;
			UpdateInfoURL     = jc2dectDevice.UpdateInfoURL;
			UpdateLastChecked = jc2dectDevice.UpdateLastChecked;
		}

		/*****************/
		/* Other methods */
		/*****************/

		public void CopyFrom( DectDevice srcDevice )
		{
			base.CopyFrom(srcDevice);

			DectBase         = srcDevice.DectBase;
			FirmwareMinorLen = srcDevice.FirmwareMinorLen;
			HardwareMajor    = srcDevice.HardwareMajor;
			HardwareMinor    = srcDevice.HardwareMinor;
		}

		public override bool Edit( Window owner = null )
		{
			DectDeviceDialog dialog = new DectDeviceDialog(this);
			if (owner != null) {
				dialog.Owner                 = owner;
				dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			} else {
				dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			}

			return dialog.ShowDialog() == true;
		}

		public override string FindFirmwareUpdate( Dispatcher dispatcher )
		{
			if (dispatcher == null) {
				throw new ArgumentNullException(nameof(dispatcher));
			}

			string dectBaseFirmware;

			if (DectBase == predefinedDectBaseID) {
				dectBaseFirmware = predefinedDectBaseFirmware;
			} else {
				if (string.IsNullOrWhiteSpace(DectBase)) {
					return string.Format(CultureInfo.CurrentCulture, JCstring.MessageTextDectBaseNotSet, DeviceName);
				}

				JuisDevice dectBase = null;
				dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => {
					dectBase = App.GetMainWindow().Devices.FindByID(DectBase) as JuisDevice;
				}));

				if (dectBase == null) {
					return string.Format(CultureInfo.CurrentCulture, JCstring.MessageTextDectBaseNotFound, DeviceName);
				}

				if (dectBase.FirmwareBuildType == 1) {
					// Release firmware
					dectBaseFirmware = $"{dectBase.FirmwareMajor}.{dectBase.FirmwareMinor:D2}.{dectBase.FirmwarePatch:D2}";
				} else {
					// Labor firmware (note: this format is to be confirmed)
					dectBaseFirmware = $"{dectBase.FirmwareMajor}.{dectBase.FirmwareMinor:D2}.{dectBase.FirmwarePatch:D2}.{dectBase.FirmwareBuildNumber}";
				}
			}

			string queryUpdateResponse = QueryFirmwareUpdate(dectBaseFirmware);

			dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => {
				SetFirmwareUpdate(queryUpdateResponse);
			}));

			return null;
		}

		public static string GetPredefinedDectBaseText()
		{
			return string.Format(CultureInfo.CurrentCulture, JCstring.ComboBoxValuePredefinedDectBase, predefinedDectBaseProduct, predefinedDectBaseFritzOS);
		}

		public override void MakeUpdateCurrent()
		{
			if (!UpdateAvailable) {
				return;
			}

			Match match = Regex.Match(UpdateInfo, @"^(\d+)\.(\d+)$");
			if (match.Success) {
				FirmwareMajor = Convert.ToInt32(match.Groups[1].Value, CultureInfo.InvariantCulture);
				FirmwareMinor = Convert.ToInt32(match.Groups[2].Value, CultureInfo.InvariantCulture);
				ClearUpdate();
				return;
			}

			throw new FormatException();
		}

		public override void NotifyDeviceNameChanged( string deviceID )
		{
			if (DectBase == deviceID) {
				NotifyPropertyChanged(nameof(MasterBaseStr));
			}
		}

		public string QueryFirmwareUpdate( string dectBaseFirmware )
		{
			if (dectBaseFirmware == null ) {
				throw new ArgumentNullException(nameof(dectBaseFirmware));
			}

			try {
				using (WebClient webClient = new WebClient()) {
					webClient.QueryString.Add("hw",      HardwareStr);
					webClient.QueryString.Add("sw",      FirmwareStr);
					webClient.QueryString.Add("oem",     OEM        );
					webClient.QueryString.Add("country", Country    );
					webClient.QueryString.Add("lang",    Language   );
					webClient.QueryString.Add("fw",      dectBaseFirmware);

					return webClient.DownloadString(programSettings.AvmCatiServiceURL);
				}
			}
			catch (WebException) {
				return null;
			}
		}

		public void SetFirmwareUpdate( string queryUpdateResponse )
		{
			ClearUpdate();

			if (queryUpdateResponse != null) {
				Match urlMatch = Regex.Match(queryUpdateResponse, @"URL=""(.+)""", RegexOptions.IgnoreCase);
				if (urlMatch.Success) {
					string	updateURL = urlMatch.Groups[1].ToString();
					string	fileName  = updateURL.Split('/').Last();

					Match fileMatch = Regex.Match(fileName, @".+\.(\d+)\.(\d+)\.avm\.de\.upd$");
					if (fileMatch.Success) {
						int    firmwareMajor = Convert.ToInt32(fileMatch.Groups[1].Value, CultureInfo.InvariantCulture);
						int    firmwareMinor = Convert.ToInt32(fileMatch.Groups[2].Value, CultureInfo.InvariantCulture);
						string updateInfo    = $"{firmwareMajor:D2}.{firmwareMinor:D2}";

						// For some products the update server returns a URL even if the device is up to date.
						// So we compare firmware versions to see if there is really an update.
						if (firmwareMajor != FirmwareMajor || firmwareMinor != FirmwareMinor) {
							UpdateAvailable = true;
							UpdateInfoIsNew = updateInfo != UpdateInfo;
							UpdateInfo      = $"{fileMatch.Groups[1]}.{fileMatch.Groups[2]}";
		 					UpdateImageURL  = updateURL;
						} else {
							UpdateInfo = JCstring.UpdateInfoNone;
						}

						FirmwareMinorLen = fileMatch.Groups[2].Value.Length;
					} else {
						UpdateInfo = JCstring.UpdateInfoError;
					}
				} else {
					UpdateInfo = JCstring.UpdateInfoNone;
				}
			} else {
				UpdateInfo = JCstring.UpdateInfoError;
			}

			UpdateLastChecked = DateTime.Now;
		}

		public new void TrimStrings()
		{
			base.TrimStrings();

			DectBase = DectBase.Trim();
		}
	}
}
