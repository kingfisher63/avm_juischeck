/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

using JuisCheck.Lang;
using JuisCheck.Properties;

namespace JuisCheck
{
	public class JuisDevice : Device
	{
		/************************************/
		/* Annex values and display strings */
		/************************************/

		private static readonly Dictionary<string, string> annexDictionary = new Dictionary<string, string>() {
			{ "A",     JCstring.ComboBoxValueAnnexA     },
			{ "B",     JCstring.ComboBoxValueAnnexB     },
			{ "Kabel", JCstring.ComboBoxValueAnnexCable },
			{ "Ohne",  JCstring.ComboBoxValueAnnexNone  }
		};

		private static readonly List<ComboBoxValue> annexValues =
			annexDictionary.Select(annex => new ComboBoxValue(annex.Key, annex.Value)).ToList();

		public static List<ComboBoxValue> GetAnnexValues()
		{
			return new List<ComboBoxValue>(annexValues);
		}

		/**************************************************/
		/* Firmware build type values and display strings */
		/**************************************************/

		public const int	firmwareBuildTypeRelease     =    1;
		public const int	firmwareBuildTypeBetaInhouse = 1000;
		public const int	firmwareBuildTypeBetaPublic  = 1001;
		public const int	firmwareBuildTypeBetaTEST    = 1006;
		public const int	firmwareBuildTypeBetaPLUS    = 1007;

		private static readonly Dictionary<int, string> firmwareBuildTypeDictionary = new Dictionary<int, string>() {
			{ firmwareBuildTypeRelease,     JCstring.ComboBoxValueBuildtypeRelease     },
			{ firmwareBuildTypeBetaInhouse, JCstring.ComboBoxValueBuildtypeBetaInhouse },
			{ firmwareBuildTypeBetaPublic,  JCstring.ComboBoxValueBuildtypeBetaPublic  },
			{ firmwareBuildTypeBetaTEST,    JCstring.ComboBoxValueBuildtypeBetaTest    },
			{ firmwareBuildTypeBetaPLUS,    JCstring.ComboBoxValueBuildtypeBetaPLUS    }
		};

		private static readonly List<ComboBoxValue> firmwareBuildTypeValues =
			firmwareBuildTypeDictionary.Select(type => new ComboBoxValue(type.Key.ToString(CultureInfo.InvariantCulture), type.Value)).ToList();

		public static List<ComboBoxValue> GetFirmwareBuildTypeValues()
		{
			return new List<ComboBoxValue>(firmwareBuildTypeValues);
		}

		/******************/
		/* Static methods */
		/******************/

		public static string[] FlagsToArray( string flags )
		{
			return flags == null ? new string[0] : flags.Split(new char[] { '\n', '\r', '\t', ' '}, StringSplitOptions.RemoveEmptyEntries);
		}

		public static List<string> FlagsToList( string flags )
		{
			return FlagsToArray(flags).ToList();
		}

		public static string FlagsToString( IEnumerable<string> flags )
		{
			return flags == null ? string.Empty : string.Join("\n", flags);
		}

		/***********************************/
		/* Device type specific properties */
		/* - including derived properties  */
		/* - in alphabetical order         */
		/***********************************/

		private string _Annex;
		public  string  Annex
		{
			get => _Annex ?? string.Empty;
			set {
				if (_Annex != value) {
					_Annex  = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(AnnexFull));
				}
			}
		}

		public string AnnexFull
		{
			get {
				if (!annexDictionary.TryGetValue(Annex, out string value)) {
					value = string.Format(CultureInfo.CurrentCulture, JCstring.ComboBoxValueUnknown, Annex);
				}
				return value;
			}
		}

		private string _DeviceAddress;
		public  string  DeviceAddress
		{
			get => _DeviceAddress ?? string.Empty;
			set {
				if (_DeviceAddress != value) {
					_DeviceAddress  = value;
					NotifyPropertyChanged();
				}
			}
		}

		public override string DeviceAddressStr
		{
			get => _DeviceAddress;
		}

		private int _FirmwareBuildNumber;
		public  int  FirmwareBuildNumber
		{
			get => _FirmwareBuildNumber;
			set {
				if (_FirmwareBuildNumber != value) {
					_FirmwareBuildNumber  = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(FirmwareStr));
				}
			}
		}

		private int _FirmwareBuildType;
		public  int  FirmwareBuildType
		{
			get => _FirmwareBuildType;
			set {
				if (_FirmwareBuildType != value) {
					_FirmwareBuildType  = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(FirmwareBuildTypeFull));
					NotifyPropertyChanged(nameof(FirmwareBuildTypeStr));
					NotifyPropertyChanged(nameof(FirmwareStr));
				}
			}
		}

		public string FirmwareBuildTypeFull
		{
			get {
				if (!firmwareBuildTypeDictionary.TryGetValue(FirmwareBuildType, out string value)) {
					value = string.Format(CultureInfo.CurrentCulture, JCstring.ComboBoxValueUnknown, FirmwareBuildType);
				}
				return value;
			}
		}

		public override string FirmwareBuildTypeStr
		{
			get => FirmwareBuildType.ToString(CultureInfo.CurrentCulture);
		}

		public bool FirmwareMajorWarning
		{
			get {
				// 252 is the hardware ID of the FRITZ!Box 6660 Cable. This is the first known model
				// where FirmwareMajor == Hardware. 252 is therefore no more than an educated guess.
				if (Hardware >= 252) {
					return Hardware != FirmwareMajor;
				} else {
					return Hardware != FirmwareMajor + 72;
				}
			}
		}

		private int _FirmwarePatch;
		public  int  FirmwarePatch
		{
			get => _FirmwarePatch;
			set {
				if (_FirmwarePatch != value) {
					_FirmwarePatch  = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(FirmwareStr));
				}
			}
		}

		public override string FirmwareStr
		{
			get {
				if (FirmwareBuildNumber !=0 && (FirmwareBuildType != firmwareBuildTypeRelease || Settings.Default.JuisReleaseShowBuildNumber)) {
					return $"{FirmwareMajor}.{FirmwareMinor:D2}.{FirmwarePatch:D2}-{FirmwareBuildNumber}";
				} else {
					return $"{FirmwareMajor}.{FirmwareMinor:D2}.{FirmwarePatch:D2}";
				}
			}
		}

		private string _Flags;
		public  string  Flags
		{
			get => _Flags ?? string.Empty;
			set {
				if (_Flags != value) {
					_Flags  = value;
					NotifyPropertyChanged();
				}
			}
		}

		private int _Hardware;
		public  int  Hardware
		{
			get => _Hardware;
			set {
				if (_Hardware != value) {
					_Hardware  = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(HardwareStr));
					NotifyPropertyChanged(nameof(FirmwareMajorWarning));
				}
			}
		}

		public override string HardwareStr
		{
			get => Hardware.ToString(CultureInfo.CurrentCulture);
		}

		private string _MeshMaster;
		public  string  MeshMaster
		{
			get => _MeshMaster ?? string.Empty;
			set {
				if (_MeshMaster != value) {
					_MeshMaster  = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(MasterBaseStr));
				}
			}
		}

		public override string MasterBaseStr
		{
			get {
				return !(App.GetMainWindow().Devices.FindByID(MeshMaster) is JuisDevice meshMaster) ? string.Empty : meshMaster.DeviceName;
			}
		}

		private string _SerialNumber;
		public  string  SerialNumber
		{
			get => _SerialNumber ?? string.Empty;
			set {
				if (_SerialNumber != value) {
					_SerialNumber  = value;
					NotifyPropertyChanged();
				}
			}
		}

		public override DataTemplate ToolTipTemplate
		{
			get => (DataTemplate)App.GetMainWindow().Resources["JuisDeviceToolTipContentTemplate"];
		}

		/**********************/
		/* Class constructors */
		/**********************/

		public JuisDevice()
		{
			ID = Guid.NewGuid().ToString();
		}

		public JuisDevice( JuisDevice srcDevice )
		{
			if (srcDevice == null) {
				throw new ArgumentNullException(nameof(srcDevice));
			}

			ID = Guid.NewGuid().ToString();
			CopyFrom(srcDevice);
		}

		public JuisDevice( XML.JC1Device jc1device )
		{
			if (jc1device == null) {
				throw new ArgumentNullException(nameof(jc1device));
			}

			ID                  = Guid.NewGuid().ToString();

			DeviceName          = jc1device.DeviceName;
			DeviceAddress       = jc1device.DeviceAddress;
			ProductName         = jc1device.ProductName;
			Hardware            = jc1device.HardwareMajor;
			SerialNumber        = jc1device.SerialNumber;
			FirmwareMajor       = jc1device.FirmwareMajor;
			FirmwareMinor       = jc1device.FirmwareMinor;
			FirmwarePatch       = jc1device.FirmwarePatch;
			FirmwareBuildNumber = jc1device.FirmwareBuildNumber;
			FirmwareBuildType   = jc1device.FirmwareBuildType;
			OEM                 = jc1device.OEM;
			Annex               = jc1device.Annex;
			Country             = jc1device.Country;
			Language            = jc1device.Language;
			Flags               = FlagsToString(jc1device.Flags);
			UpdateAvailable     = jc1device.UpdateAvailable;
			UpdateInfo          = jc1device.UpdateInfo;
			UpdateImageURL      = jc1device.UpdateImageURL;
			UpdateInfoURL       = jc1device.UpdateInfoURL;
			UpdateLastChecked   = jc1device.UpdateLastChecked;
		}

		public JuisDevice( XML.JC2JuisDevice jc2juisDevice )
		{
			if (jc2juisDevice == null) {
				throw new ArgumentNullException(nameof(jc2juisDevice));
			}

			ID                  = jc2juisDevice.ID;

			DeviceName          = jc2juisDevice.DeviceName;
			DeviceAddress       = jc2juisDevice.DeviceAddress;
			ProductName         = jc2juisDevice.ProductName;
			Hardware            = jc2juisDevice.Hardware;
			SerialNumber        = jc2juisDevice.SerialNumber;
			FirmwareMajor       = jc2juisDevice.FirmwareMajor;
			FirmwareMinor       = jc2juisDevice.FirmwareMinor;
			FirmwarePatch       = jc2juisDevice.FirmwarePatch;
			FirmwareBuildNumber = jc2juisDevice.FirmwareBuildNumber;
			FirmwareBuildType   = jc2juisDevice.FirmwareBuildType;
			OEM                 = jc2juisDevice.OEM;
			Annex               = jc2juisDevice.Annex;
			Country             = jc2juisDevice.Country;
			Language            = jc2juisDevice.Language;
			Flags               = FlagsToString(jc2juisDevice.Flags);
			MeshMaster          = jc2juisDevice.MeshMaster;
			UpdateAvailable     = jc2juisDevice.UpdateAvailable;
			UpdateInfo          = jc2juisDevice.UpdateInfo;
			UpdateImageURL      = jc2juisDevice.UpdateImageURL;
			UpdateInfoURL       = jc2juisDevice.UpdateInfoURL;
			UpdateLastChecked   = jc2juisDevice.UpdateLastChecked;
		}

		/*****************/
		/* Other methods */
		/*****************/

		public void CopyFrom( JuisDevice srcDevice )
		{
			base.CopyFrom(srcDevice);

			Annex               = srcDevice.Annex;
			DeviceAddress       = srcDevice.DeviceAddress;
			FirmwareBuildNumber = srcDevice.FirmwareBuildNumber;
			FirmwareBuildType   = srcDevice.FirmwareBuildType;
			FirmwarePatch       = srcDevice.FirmwarePatch;
			Flags               = srcDevice.Flags;
			Hardware            = srcDevice.Hardware;
			MeshMaster          = srcDevice.MeshMaster;
			SerialNumber        = srcDevice.SerialNumber;
		}
		
		public override bool Edit( Window owner = null )
		{
			JuisDeviceDialog dialog = new JuisDeviceDialog(this);
			if (owner != null) {
				dialog.Owner                 = owner;
				dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			} else {
				dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			}

			return dialog.ShowDialog() == true;
		}

		protected static int GetFirmwareBuildType( string buildTypeStr, int defaultBuildType )
		{
			if (buildTypeStr == null) {
				throw new ArgumentNullException(nameof(buildTypeStr));
			}

			switch (buildTypeStr.ToUpperInvariant()) {
				case "RELEASE":
					return firmwareBuildTypeRelease;

				case "INHAUS":
				case "INTERN":
					return firmwareBuildTypeBetaInhouse;

				case "BETA":
				case "EXTERN":
				case "LABBETA":
				case "LABOR":
					return firmwareBuildTypeBetaPublic;

				case "TEST":
					return firmwareBuildTypeBetaTEST;

				case "PLUS":
					return firmwareBuildTypeBetaPLUS;

				default:
					return defaultBuildType;
			}
		}

		// This method parses the UpdateInfo string to determine version and build type of an available
		// update. AVM apparently hand crafts this string. As a result some variety of formats has been
		// observed. This method tries them one by one. This includes several formats that AVM might use
		// in the future.
		//
		// <MA>    Major version number. Has been seen missing in some cases.
		// <MI>    Minor version number.
		// <PA>    Patch version number.
		// <BUILD> Build number. Usually not present in Release builds.
		// <TYPE>  Build type (Labor, Inhaus, etc). May or may not be preceded by whitespace.

		public override void MakeUpdateCurrent()
		{
			if (!UpdateAvailable) {
				return;
			}

			Match match;

			// <MA>.<MI>.<PA>-<BUILD><TYPE>

			match = Regex.Match(UpdateInfo, @"^(\d+)\.(\d+)\.(\d+)-(\d+)\s*([A-Za-z]+)$");
			if (match.Success) {
				FirmwareMajor       = Convert.ToInt32(match.Groups[1].Value, CultureInfo.InvariantCulture);
				FirmwareMinor       = Convert.ToInt32(match.Groups[2].Value, CultureInfo.InvariantCulture);
				FirmwarePatch       = Convert.ToInt32(match.Groups[3].Value, CultureInfo.InvariantCulture);
				FirmwareBuildNumber = Convert.ToInt32(match.Groups[4].Value, CultureInfo.InvariantCulture);
				FirmwareBuildType   = GetFirmwareBuildType(match.Groups[5].Value, FirmwareBuildType);
				ClearUpdate();
				return;
			}

			// <MA>.<MI>.<PA>-<BUILD>

			match = Regex.Match(UpdateInfo, @"^(\d+)\.(\d+)\.(\d+)-(\d+)$");
			if (match.Success) {
				FirmwareMajor       = Convert.ToInt32(match.Groups[1].Value, CultureInfo.InvariantCulture);
				FirmwareMinor       = Convert.ToInt32(match.Groups[2].Value, CultureInfo.InvariantCulture);
				FirmwarePatch       = Convert.ToInt32(match.Groups[3].Value, CultureInfo.InvariantCulture);
				FirmwareBuildNumber = Convert.ToInt32(match.Groups[4].Value, CultureInfo.InvariantCulture);
				// FirmwareBuildType: no info => keep current value
				ClearUpdate();
				return;
			}

			// <MA>.<MI>.<PA><TYPE>

			match = Regex.Match(UpdateInfo, @"^(\d+)\.(\d+)\.(\d+)\s*([A-Za-z]+)$");
			if (match.Success) {
				FirmwareMajor       = Convert.ToInt32(match.Groups[1].Value, CultureInfo.InvariantCulture);
				FirmwareMinor       = Convert.ToInt32(match.Groups[2].Value, CultureInfo.InvariantCulture);
				FirmwarePatch       = Convert.ToInt32(match.Groups[3].Value, CultureInfo.InvariantCulture);
				FirmwareBuildNumber = 0;
				FirmwareBuildType   = GetFirmwareBuildType(match.Groups[4].Value, firmwareBuildTypeRelease);
				ClearUpdate();
				return;
			}

			// <MA>.<MI>.<PA>

			match = Regex.Match(UpdateInfo, @"^(\d+)\.(\d+)\.(\d+)$");
			if (match.Success) {
				FirmwareMajor       = Convert.ToInt32(match.Groups[1].Value, CultureInfo.InvariantCulture);
				FirmwareMinor       = Convert.ToInt32(match.Groups[2].Value, CultureInfo.InvariantCulture);
				FirmwarePatch       = Convert.ToInt32(match.Groups[3].Value, CultureInfo.InvariantCulture);
				FirmwareBuildNumber = 0;
				FirmwareBuildType   = firmwareBuildTypeRelease;
				ClearUpdate();
				return;
			}

			// <MI>.<PA>-<BUILD><TYPE>

			match = Regex.Match(UpdateInfo, @"^(\d+)\.(\d+)-(\d+)\s*([A-Za-z]+)$");
			if (match.Success) {
				// FirmwareMajor: missing => keep current value
				FirmwareMinor       = Convert.ToInt32(match.Groups[1].Value, CultureInfo.InvariantCulture);
				FirmwarePatch       = Convert.ToInt32(match.Groups[2].Value, CultureInfo.InvariantCulture);
				FirmwareBuildNumber = Convert.ToInt32(match.Groups[3].Value, CultureInfo.InvariantCulture);
				FirmwareBuildType   = GetFirmwareBuildType(match.Groups[4].Value, FirmwareBuildType);
				ClearUpdate();
				return;
			}

			// <MI>.<PA>-<BUILD>

			match = Regex.Match(UpdateInfo, @"^(\d+)\.(\d+)-(\d+)$");
			if (match.Success) {
				// FirmwareMajor: missing => keep current value
				FirmwareMinor       = Convert.ToInt32(match.Groups[1].Value, CultureInfo.InvariantCulture);
				FirmwarePatch       = Convert.ToInt32(match.Groups[2].Value, CultureInfo.InvariantCulture);
				FirmwareBuildNumber = Convert.ToInt32(match.Groups[3].Value, CultureInfo.InvariantCulture);
				// FirmwareBuildType: no info => keep current value
				ClearUpdate();
				return;
			}

			// <MI>.<PA><TYPE>

			match = Regex.Match(UpdateInfo, @"^(\d+)\.(\d+)\s*([A-Za-z]+)$");
			if (match.Success) {
				// FirmwareMajor: missing => keep current value
				FirmwareMinor       = Convert.ToInt32(match.Groups[1].Value, CultureInfo.InvariantCulture);
				FirmwarePatch       = Convert.ToInt32(match.Groups[2].Value, CultureInfo.InvariantCulture);
				FirmwareBuildNumber = 0;
				FirmwareBuildType   = GetFirmwareBuildType(match.Groups[3].Value, firmwareBuildTypeRelease);
				ClearUpdate();
				return;
			}

			// <MI>.<PA>

			match = Regex.Match(UpdateInfo, @"^(\d+)\.(\d+)$");
			if (match.Success) {
				// FirmwareMajor: missing => keep current value
				FirmwareMinor       = Convert.ToInt32(match.Groups[1].Value, CultureInfo.InvariantCulture);
				FirmwarePatch       = Convert.ToInt32(match.Groups[2].Value, CultureInfo.InvariantCulture);
				FirmwareBuildNumber = 0;
				FirmwareBuildType   = firmwareBuildTypeRelease;
				ClearUpdate();
				return;
			}

			throw new FormatException();
		}

		public override void NotifyDeviceNameChanged( string deviceID )
		{
			if (MeshMaster == deviceID) {
				NotifyPropertyChanged(nameof(MasterBaseStr));
			}
		}

		public void QueryDevice()
		{
			try {
				QueryJuisDevice();
			}
			catch (WebException ex) when (ex.Status == WebExceptionStatus.ProtocolError && ex.Message.Contains("404")) {
				QueryJasonDevice();
			}
			ClearUpdate();
		}

		public JUIS.UpdateInfo QueryFirmwareUpdate( JuisDevice meshMaster = null )
		{
			string timestamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

			JUIS.RequestHeader requestHeader = new JUIS.RequestHeader {
				ManualRequest = true,
				Nonce         = Convert.ToBase64String(Encoding.UTF8.GetBytes(timestamp)),
				UserAgent     = "Box"
			};

			using (JUIS.UpdateInfoServiceClient client = new JUIS.UpdateInfoServiceClient(new BasicHttpBinding(), new EndpointAddress(Settings.Default.AvmJuisServiceURL))) {
				return client.BoxFirmwareUpdateCheck(requestHeader, ToBoxInfo(), meshMaster?.ToBoxInfo()).UpdateInfo;
			}
		}

		protected void QueryJasonDevice()
		{
			if (string.IsNullOrWhiteSpace(DeviceAddress)) {
				throw new ArgumentException(JCmessage.InvalidDeviceAddress);
			}

			XmlReaderSettings	xmlReaderSettings = new XmlReaderSettings() { IgnoreComments = true, IgnoreWhitespace = true };
			XmlSerializer		xmlSerializer     = new XmlSerializer(typeof(Jason.BoxInfo));

			using (XmlReader xmlReader = XmlReader.Create($"http://{DeviceAddress.Trim()}/jason_boxinfo.xml", xmlReaderSettings)) {
				Jason.BoxInfo boxInfo = (Jason.BoxInfo)xmlSerializer.Deserialize(xmlReader);

				// General

				Match match = Regex.Match(boxInfo.Version, @"^(\d+)\.(\d+)\.(\d+)$");
				if (!match.Success) {
					throw new FormatException();
				}

				ProductName         = boxInfo.Name;
				Hardware            = boxInfo.HW;
				SerialNumber        = boxInfo.Serial;
				FirmwareMajor       = Convert.ToInt32(match.Groups[1].Value, CultureInfo.InvariantCulture);
				FirmwareMinor       = Convert.ToInt32(match.Groups[2].Value, CultureInfo.InvariantCulture);
				FirmwarePatch       = Convert.ToInt32(match.Groups[3].Value, CultureInfo.InvariantCulture);
				FirmwareBuildNumber = boxInfo.Revision;
				FirmwareBuildType   = GetFirmwareBuildType(boxInfo.Lab, firmwareBuildTypeRelease);
				OEM                 = boxInfo.OEM;
				Annex               = boxInfo.Annex;
				Country             = boxInfo.Country;
				Language            = boxInfo.Lang;
				Flags               = FlagsToString(boxInfo.Flag);
			}
		}

		protected void QueryJuisDevice()
		{
			if (string.IsNullOrWhiteSpace(DeviceAddress)) {
				throw new ArgumentException(JCmessage.InvalidDeviceAddress);
			}

			XmlReaderSettings	xmlReaderSettings = new XmlReaderSettings() { IgnoreComments = true, IgnoreWhitespace = true };
			XmlRootAttribute	xmlRootAttribute  = new XmlRootAttribute()  { ElementName = "BoxInfo", Namespace = "http://juis.avm.de/updateinfo" };
			XmlSerializer		xmlSerializer     = new XmlSerializer(typeof(JUIS.BoxInfo), xmlRootAttribute);

			using (XmlReader xmlReader = XmlReader.Create($"http://{DeviceAddress.Trim()}/juis_boxinfo.xml", xmlReaderSettings)) {
				JUIS.BoxInfo boxInfo = (JUIS.BoxInfo)xmlSerializer.Deserialize(xmlReader);

				ProductName         = boxInfo.Name;
				Hardware            = boxInfo.HW;
				SerialNumber        = boxInfo.Serial;
				FirmwareMajor       = boxInfo.Major;
				FirmwareMinor       = boxInfo.Minor;
				FirmwarePatch       = boxInfo.Patch;
				FirmwareBuildNumber = boxInfo.Buildnumber;
				FirmwareBuildType   = boxInfo.Buildtype;
				OEM                 = boxInfo.OEM;
				Annex               = boxInfo.Annex;
				Country             = boxInfo.Country;
				Language            = boxInfo.Lang;
				Flags               = FlagsToString(boxInfo.Flag);
			}
		}

		public void SetFirmwareUpdate( JUIS.UpdateInfo queryUpdateReponse )
		{
			ClearUpdate();

			if (queryUpdateReponse != null) {
				if (queryUpdateReponse.Found) {
					UpdateAvailable = true;
					UpdateInfo      = queryUpdateReponse.Version;
					UpdateImageURL  = queryUpdateReponse.DownloadURL;
					UpdateInfoURL   = queryUpdateReponse.InfoURL;
				} else {
					UpdateInfo = JCstring.UpdateInfoNone;
				}
			} else {
				UpdateInfo = JCstring.UpdateInfoError;
			}

			UpdateLastChecked = DateTime.Now;
		}

		public JUIS.BoxInfo ToBoxInfo()
		{
			return new JUIS.BoxInfo {
				Name         = ProductName,																// Never empty
				HW           = Hardware,
				Major        = FirmwareMajor,
				Minor        = FirmwareMinor,
				Patch        = FirmwarePatch,
				Buildnumber  = FirmwareBuildNumber,
				Buildtype    = FirmwareBuildType,
				Serial       = string.IsNullOrWhiteSpace(SerialNumber) ? "9CC7A6123456" : SerialNumber,	// Must not be empty (use fake AVM MAC address if needed)
				OEM          = OEM,																		// Never empty
				Lang         = Language,																// Never empty
				Country      = Country,																	// Never empty
				Annex        = Annex,																	// Never empty
				Flag         = Flags.Length == 0 ? new string[] { string.Empty } : FlagsToArray(Flags),	// Need at least one flag (empty flag OK)
				UpdateConfig = 1,
				Provider     = string.Empty,
				ProviderName = string.Empty
			};
		}

		public new void TrimStrings()
		{
			base.TrimStrings();

			Annex         = Annex.Trim();
			DeviceAddress = DeviceAddress.Trim();
			Flags         = Flags.Trim();
			MeshMaster    = MeshMaster.Trim();
			SerialNumber  = SerialNumber.Trim();
		}
	}
}
