/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.Globalization;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Threading;

using JuisCheck.Lang;
using JuisCheck.Properties;

namespace JuisCheck
{
	public class DectDevice : Device
	{
		public const string					predefinedDectBaseID            = "63022b32-3ecb-436d-bc6c-00861df70fff";
		public const string					predefinedDectBaseName          = "FRITZ!Box 7590";
		public const int					predefinedDectBaseHW            = 226;
		public const int					predefinedDectBaseFirmwareMajor = 154;
		public const int					predefinedDectBaseFirmwareMinor = 7;
		public const int					predefinedDectBaseFirmwarePatch = 50;
		public const int					predefinedDectBaseBuildNumber   = 101716;
		public const int					predefinedDectBaseBuildType     = JuisDevice.firmwareBuildTypeRelease;
		public const string					predefinedDectBaseAnnex         = "B";

		/***********************************/
		/* Device type specific properties */
		/* - including derived properties  */
		/* - in alphabetical order         */
		/***********************************/

		private string _DectBase;
		public  string  DectBase {
			get => _DectBase ?? string.Empty;
			set {
				if (_DectBase != value) {
					_DectBase  = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(MasterBaseStr));
				}
			}
		}

		public override string DeviceAddressStr => string.Empty;

		public override string FirmwareBuildTypeStr => string.Empty;

		private int _FirmwareMinor2;
		public  int  FirmwareMinor2	{
			get => _FirmwareMinor2;
			set {
				if (_FirmwareMinor2 != value) {
					_FirmwareMinor2  = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(FirmwareStr));
				}
			}
		}

		private int _FirmwareMinor3;
		public  int  FirmwareMinor3 {
			get => _FirmwareMinor3;
			set {
				if (_FirmwareMinor3 != value) {
					_FirmwareMinor3  = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(FirmwareStr));
				}
			}
		}

		private int _FirmwareMinor4;
		public  int  FirmwareMinor4 {
			get => _FirmwareMinor4;
			set {
				if (_FirmwareMinor4 != value) {
					_FirmwareMinor4  = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(FirmwareStr));
				}
			}
		}

		public override string FirmwareStr {
			get {
				if (HardwareMajor == 10) {
					return $"{FirmwareMajor}.{FirmwareMinor:D2}.{FirmwareMinor2:D2}.{FirmwareMinor3:D2}-{FirmwareMinor4:D3}";
				} else {
					return $"{FirmwareMajor}.{FirmwareMinor:D2}";
				}
			}
		}

		private int _HardwareMajor;
		public  int  HardwareMajor {
			get => _HardwareMajor;
			set {
				if (_HardwareMajor != value) {
					_HardwareMajor  = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(HardwareStr));
					NotifyPropertyChanged(nameof(IsFivePartVersion));
				}
			}
		}

		private int _HardwareMinor;
		public  int  HardwareMinor {
			get => _HardwareMinor;
			set {
				if (_HardwareMinor != value) {
					_HardwareMinor  = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(HardwareStr));
				}
			}
		}

		public override string HardwareStr => $"{HardwareMajor:D2}.{HardwareMinor:D2}";

		public bool IsFivePartVersion => HardwareMajor == 10;

		public override string MasterBaseStr {
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

		public override DataTemplate ToolTipTemplate => (DataTemplate)App.GetMainWindow().Resources["DectDeviceToolTipContentTemplate"];

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
			FirmwareMinor2    = 0;
			FirmwareMinor3    = 0;
			FirmwareMinor4    = 0;
			OEM               = jc1device.OEM;
			Country           = jc1device.Country;
			Language          = jc1device.Language;
			UpdateAvailable   = jc1device.UpdateAvailable;
			UpdateImageURL    = jc1device.UpdateImageURL;
			UpdateInfoURL     = jc1device.UpdateInfoURL;
			UpdateLastChecked = jc1device.UpdateLastChecked;
			UpdateVersion     = jc1device.UpdateInfo;
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
			FirmwareMinor2    = jc2dectDevice.FirmwareMinor2;
			FirmwareMinor3    = jc2dectDevice.FirmwareMinor3;
			FirmwareMinor4    = jc2dectDevice.FirmwareMinor4;
			OEM               = jc2dectDevice.OEM;
			Country           = jc2dectDevice.Country;
			Language          = jc2dectDevice.Language;
			DectBase          = jc2dectDevice.DectBase;
			UpdateAvailable   = jc2dectDevice.UpdateAvailable;
			UpdateImageURL    = jc2dectDevice.UpdateImageURL;
			UpdateInfoURL     = jc2dectDevice.UpdateInfoURL;
			UpdateLastChecked = jc2dectDevice.UpdateLastChecked;
			UpdateVersion     = jc2dectDevice.UpdateInfo;
		}

		/*****************/
		/* Other methods */
		/*****************/

		public void CopyFrom( DectDevice srcDevice )
		{
			base.CopyFrom(srcDevice);

			DectBase       = srcDevice.DectBase;
			FirmwareMinor2 = srcDevice.FirmwareMinor2;
			FirmwareMinor3 = srcDevice.FirmwareMinor3;
			FirmwareMinor4 = srcDevice.FirmwareMinor4;
			HardwareMajor  = srcDevice.HardwareMajor;
			HardwareMinor  = srcDevice.HardwareMinor;
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

			AVM.JUIS.BoxInfo dectBaseBoxInfo;

			if (DectBase == predefinedDectBaseID) {
				dectBaseBoxInfo = GetPredefinedDectBaseBoxInfo();
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

				dectBaseBoxInfo = dectBase.ToBoxInfo();
			}

			try {
				var updateInfo = JUIS.DeviceFirmwareUpdateCheck(dectBaseBoxInfo, ToDeviceInfo());

				dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => {
					SetFirmwareUpdate(updateInfo);
				}));
			}
			catch {
				dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => {
					SetFirmwareUpdate(null);
				}));
			}

			return null;
		}

		public AVM.JUIS.BoxInfo GetPredefinedDectBaseBoxInfo()
		{
			return new AVM.JUIS.BoxInfo {
				Name         = predefinedDectBaseName,
				HW           = predefinedDectBaseHW,
				Major        = predefinedDectBaseFirmwareMajor,
				Minor        = predefinedDectBaseFirmwareMinor,
				Patch        = predefinedDectBaseFirmwarePatch,
				Buildnumber  = predefinedDectBaseBuildNumber,
				Buildtype    = predefinedDectBaseBuildType,
				Serial       = "000000000000",
				OEM          = OEM,								// Use same as DECT device
				Lang         = Language,						// Use same as DECT device
				Country      = Country,							// Use same as DECT device
				Annex        = predefinedDectBaseAnnex,
				Flag         = new string[] { string.Empty },	// Need at least one flag (empty flag OK)
				UpdateConfig = 1,
				Provider     = string.Empty,
				ProviderName = string.Empty
			};
		}

		public static string GetPredefinedDectBaseText()
		{
			return string.Format(CultureInfo.CurrentCulture, JCstring.ComboBoxValuePredefinedDectBase, predefinedDectBaseName, $"{predefinedDectBaseFirmwareMinor}.{predefinedDectBaseFirmwarePatch}");
		}

		public override void MakeUpdateCurrent()
		{
			if (!UpdateAvailable) {
				return;
			}

			string updateVersion = UpdateVersion.Trim();

			if (IsFivePartVersion) {
				Match match = Regex.Match(updateVersion, @"^(\d+)\.(\d+)\.(\d+)\.(\d+)-(\d+)$");
				if (match.Success) {
					FirmwareMajor  = Convert.ToInt32(match.Groups[1].Value, CultureInfo.InvariantCulture);
					FirmwareMinor  = Convert.ToInt32(match.Groups[2].Value, CultureInfo.InvariantCulture);
					FirmwareMinor2 = Convert.ToInt32(match.Groups[3].Value, CultureInfo.InvariantCulture);
					FirmwareMinor3 = Convert.ToInt32(match.Groups[4].Value, CultureInfo.InvariantCulture);
					FirmwareMinor4 = Convert.ToInt32(match.Groups[5].Value, CultureInfo.InvariantCulture);
					ClearUpdate();
					return;
				}
			} else {
				Match match = Regex.Match(updateVersion, @"^(\d+)\.(\d+)$");
				if (match.Success) {
					FirmwareMajor = Convert.ToInt32(match.Groups[1].Value, CultureInfo.InvariantCulture);
					FirmwareMinor = Convert.ToInt32(match.Groups[2].Value, CultureInfo.InvariantCulture);
					ClearUpdate();
					return;
				}
			}

			throw new FormatException();
		}

		public override void NotifyDeviceNameChanged( string deviceID )
		{
			if (DectBase == deviceID) {
				NotifyPropertyChanged(nameof(MasterBaseStr));
			}
		}

		public AVM.JUIS.DeviceInfo ToDeviceInfo()
		{
			return new AVM.JUIS.DeviceInfo {
				Lang    = Language,
				MHW     = HardwareStr,
				Serial  = "000000000000",
				Type    = 1,
				Version = FirmwareStr
			};
		}

		public new void TrimStrings()
		{
			base.TrimStrings();

			DectBase = DectBase.Trim();
		}
	}
}
