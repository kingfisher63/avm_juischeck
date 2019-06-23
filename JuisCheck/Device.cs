/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) 2018 Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

using JuisCheck.Lang;
using JuisCheck.Properties;

namespace JuisCheck
{
	[Serializable]
	[XmlType(TypeName = "Device")]
	public class Device : INotifyPropertyChanged
	{
		// Annex

		private static readonly Dictionary<string, string> annexDictionary = new Dictionary<string, string>() {
			{ "A",     JCstring.ComboBoxValueAnnexA     },
			{ "B",     JCstring.ComboBoxValueAnnexB     },
			{ "Kabel", JCstring.ComboBoxValueAnnexCable },
			{ "Ohne",  JCstring.ComboBoxValueAnnexNone  }
		};

		private static readonly List<ComboBoxValue> annexValues;

		public static List<ComboBoxValue> GetAnnexValues()
		{
			return new List<ComboBoxValue>(annexValues);
		}

		[XmlIgnore]
		public string AnnexFull
		{
			get {
				if (!annexDictionary.TryGetValue(Annex, out string value)) {
					value = string.Format(JCstring.ComboBoxValueUnknown, Annex);
				}
				return value;
			}
		}

		// FirmwareBuildType

		public const int	firmwareBuildTypeRelease     =    1;
		public const int	firmwareBuildTypeBetaInhouse = 1000;
		public const int	firmwareBuildTypeBetaPublic  = 1001;
		public const int	firmwareBuildTypeBetaPLUS    = 1007;

		private static readonly Dictionary<int, string> firmwareBuildTypeDictionary = new Dictionary<int, string>() {
			{ firmwareBuildTypeRelease,     JCstring.ComboBoxValueBuildtypeRelease     },
			{ firmwareBuildTypeBetaInhouse, JCstring.ComboBoxValueBuildtypeBetaInhouse },
			{ firmwareBuildTypeBetaPublic,  JCstring.ComboBoxValueBuildtypeBetaPublic  },
			{ firmwareBuildTypeBetaPLUS,    JCstring.ComboBoxValueBuildtypeBetaPLUS    }
		};

		private static readonly List<ComboBoxValue> firmwareBuildTypeValues;

		public static List<ComboBoxValue> GetFirmwareBuildTypeValues()
		{
			return new List<ComboBoxValue>(firmwareBuildTypeValues);
		}

		[XmlIgnore]
		public string FirmwareBuildTypeFull
		{
			get {
				if (!firmwareBuildTypeDictionary.TryGetValue(FirmwareBuildType, out string value)) {
					value = string.Format(JCstring.ComboBoxValueUnknown, FirmwareBuildType);
				}
				return value;
			}
		}

		// Country

		private static readonly Dictionary<string, string> countryDictionary = new Dictionary<string, string>()	{
			{ "0234", JCstring.ComboBoxValueCountry0234 },
			{ "0255", JCstring.ComboBoxValueCountry0255 },
			{ "0256", JCstring.ComboBoxValueCountry0256 },
			{ "0264", JCstring.ComboBoxValueCountry0264 },
			{ "027",  JCstring.ComboBoxValueCountry027  },
			{ "030",  JCstring.ComboBoxValueCountry030  },
			{ "031",  JCstring.ComboBoxValueCountry031  },
			{ "032",  JCstring.ComboBoxValueCountry032  },
			{ "033",  JCstring.ComboBoxValueCountry033  },
			{ "034",  JCstring.ComboBoxValueCountry034  },
			{ "0351", JCstring.ComboBoxValueCountry0351 },
			{ "0352", JCstring.ComboBoxValueCountry0352 },
			{ "0353", JCstring.ComboBoxValueCountry0353 },
			{ "0357", JCstring.ComboBoxValueCountry0357 },
			{ "0358", JCstring.ComboBoxValueCountry0358 },
			{ "036",  JCstring.ComboBoxValueCountry036  },
			{ "0371", JCstring.ComboBoxValueCountry0371 },
			{ "0372", JCstring.ComboBoxValueCountry0372 },
			{ "0376", JCstring.ComboBoxValueCountry0376 },
			{ "0382", JCstring.ComboBoxValueCountry0382 },
			{ "0385", JCstring.ComboBoxValueCountry0385 },
			{ "0386", JCstring.ComboBoxValueCountry0386 },
			{ "0387", JCstring.ComboBoxValueCountry0387 },
			{ "0389", JCstring.ComboBoxValueCountry0389 },
			{ "039",  JCstring.ComboBoxValueCountry039  },
			{ "041",  JCstring.ComboBoxValueCountry041  },
			{ "0420", JCstring.ComboBoxValueCountry0420 },
			{ "0421", JCstring.ComboBoxValueCountry0421 },
			{ "043",  JCstring.ComboBoxValueCountry043  },
			{ "044",  JCstring.ComboBoxValueCountry044  },
			{ "045",  JCstring.ComboBoxValueCountry045  },
			{ "046",  JCstring.ComboBoxValueCountry046  },
			{ "047",  JCstring.ComboBoxValueCountry047  },
			{ "048",  JCstring.ComboBoxValueCountry048  },
			{ "049",  JCstring.ComboBoxValueCountry049  },
			{ "054",  JCstring.ComboBoxValueCountry054  },
			{ "061",  JCstring.ComboBoxValueCountry061  },
			{ "064",  JCstring.ComboBoxValueCountry064  },
			{ "066",  JCstring.ComboBoxValueCountry066  },
			{ "0972", JCstring.ComboBoxValueCountry0972 }
		};

		private static readonly List<ComboBoxValue> countryValues;

		public static List<ComboBoxValue> GetCountryValues()
		{
			return new List<ComboBoxValue>(countryValues);
		}

		[XmlIgnore]
		public string CountryFull
		{
			get {
				if (!countryDictionary.TryGetValue(Country, out string value)) {
					value = string.Format(JCstring.ComboBoxValueUnknown, Country);
				}
				return value;
			}
		}

		// Language

		private static readonly Dictionary<string, string> languageDictionary = new Dictionary<string, string>()
		{
			{ "de", JCstring.ComboBoxValueLanguageDE },
			{ "en", JCstring.ComboBoxValueLanguageEN },
			{ "es", JCstring.ComboBoxValueLanguageES },
			{ "fr", JCstring.ComboBoxValueLanguageFR },
			{ "it", JCstring.ComboBoxValueLanguageIT },
			{ "nl", JCstring.ComboBoxValueLanguageNL },
			{ "pl", JCstring.ComboBoxValueLanguagePL }
		};

		private static readonly List<ComboBoxValue> languageValues;

		public static List<ComboBoxValue> GetLanguageValues()
		{
			return new List<ComboBoxValue>(languageValues);
		}

		[XmlIgnore]
		public string LanguageFull
		{
			get {
				if (!languageDictionary.TryGetValue(Language, out string value)) {
					value = string.Format(JCstring.ComboBoxValueUnknown, Language);
				}
				return value;
			}
		}

		// OEM

		private static readonly List<ComboBoxValue> oemValues = new List<ComboBoxValue> {
			new ComboBoxValue("1und1",  "1und1" ),
			new ComboBoxValue("avm",    "avm"   ),
			new ComboBoxValue("avme",   "avme"  ),
			new ComboBoxValue("ewetel", "ewetel"),
			new ComboBoxValue("otwo",   "otwo"  )
		};

		public static List<ComboBoxValue> GetOemValues()
		{
			return new List<ComboBoxValue>(oemValues);
		}

		static Device()
		{
			annexValues = annexDictionary.Select( annex => new ComboBoxValue(annex.Key, annex.Value) ).ToList();

			firmwareBuildTypeValues = firmwareBuildTypeDictionary.Select( type => new ComboBoxValue(type.Key.ToString(), type.Value) ).ToList();

			countryValues = countryDictionary.Select( country => new ComboBoxValue(country.Key, country.Value) ).ToList();
			countryValues.Sort();
			countryValues.Add(new ComboBoxValue("99", JCstring.ComboBoxValueCountry99));
			countryDictionary.Add("99", JCstring.ComboBoxValueCountry99);

			languageValues = languageDictionary.Select( language => new ComboBoxValue(language.Key, language.Value) ).ToList();
			languageValues.Sort();
		}

		protected NotifyPropertyChanged	notifyPropertyChanged;

		protected DeviceKind	_DeviceKind = DeviceKind.Unknown;
		[XmlAttribute(AttributeName = "kind")]
		public		DeviceKind	 DeviceKind
		{
			get => _DeviceKind;
			set {
				DeviceKind oldvalue = _DeviceKind;
				DeviceKind newvalue = value;

				_DeviceKind = newvalue;
				RaisePropertyChanged(nameof(DeviceKind), newvalue != oldvalue);
			}
		}

		protected	string	_DeviceName = string.Empty;
		[XmlElement(ElementName = "DeviceName")]
		public		string   DeviceName
		{
			get => _DeviceName;
			set {
				string oldvalue = _DeviceName;
				string newvalue = value ?? string.Empty;

				_DeviceName = newvalue;
				RaisePropertyChanged(nameof(DeviceName), newvalue != oldvalue);
			}
		}

		protected	string	_DeviceAddress = string.Empty;
		[XmlElement(ElementName = "DeviceAddress")]
		public		string	 DeviceAddress
		{
			get => _DeviceAddress;
			set {
				string oldvalue = _DeviceAddress;
				string newvalue = value ?? string.Empty;

				_DeviceAddress = newvalue;
				RaisePropertyChanged(nameof(DeviceAddress), newvalue != oldvalue);
			}
		}

		protected	string	_ProductName = string.Empty;
		[XmlElement(ElementName = "ProductName")]
		public		string	 ProductName
		{
			get => _ProductName;
			set {
				string oldvalue = _ProductName;
				string newvalue = value ?? string.Empty;

				_ProductName = newvalue;
				RaisePropertyChanged(nameof(ProductName), newvalue != oldvalue);
			}
		}

		protected	int	_HardwareMajor = 0;
		[XmlElement(ElementName = "HardwareMajor")]
		public		int	 HardwareMajor
		{
			get => _HardwareMajor;
			set {
				int oldvalue = _HardwareMajor;
				int newvalue = value;

				_HardwareMajor = newvalue;
				RaisePropertyChanged(nameof(HardwareMajor), newvalue != oldvalue);
				RaisePropertyChanged(nameof(HardwareStr),   newvalue != oldvalue);
				SetFirmwareMajorWarning();
			}
		}

		protected	int	_HardwareMinor = 0;
		[XmlElement(ElementName = "HardwareMinor")]
		public		int	 HardwareMinor
		{
			get => _HardwareMinor;
			set {
				int oldvalue = _HardwareMinor;
				int newvalue = value;

				_HardwareMinor = newvalue;
				RaisePropertyChanged(nameof(HardwareMinor), newvalue != oldvalue);
				RaisePropertyChanged(nameof(HardwareStr),   newvalue != oldvalue);
			}
		}

		protected	string	_SerialNumber = string.Empty;
		[XmlElement(ElementName = "SerialNumber")]
		public		string	 SerialNumber
		{
			get => _SerialNumber;
			set {
				string oldvalue = _SerialNumber;
				string newvalue = value ?? string.Empty;

				_SerialNumber = newvalue;
				RaisePropertyChanged(nameof(SerialNumber), newvalue != oldvalue);
			}
		}

		protected	int	_FirmwareMajor = 0;
		[XmlElement(ElementName = "FirmwareMajor")]
		public		int	 FirmwareMajor
		{
			get => _FirmwareMajor;
			set {
				int oldvalue = _FirmwareMajor;
				int newvalue = value;

				_FirmwareMajor = newvalue;
				RaisePropertyChanged(nameof(FirmwareMajor), newvalue != oldvalue);
				RaisePropertyChanged(nameof(FirmwareStr),   newvalue != oldvalue);
				SetFirmwareMajorWarning();
			}
		}

		protected	int	_FirmwareMinor = 0;
		[XmlElement(ElementName = "FirmwareMinor")]
		public		int	 FirmwareMinor
		{
			get => _FirmwareMinor;
			set {
				int oldvalue = _FirmwareMinor;
				int newvalue = value;

				_FirmwareMinor = newvalue;
				RaisePropertyChanged(nameof(FirmwareMinor), newvalue != oldvalue);
				RaisePropertyChanged(nameof(FirmwareStr),   newvalue != oldvalue);
			}
		}

		protected	int	_FirmwarePatch = 0;
		[XmlElement(ElementName = "FirmwarePatch")]
		public		int	 FirmwarePatch
		{
			get => _FirmwarePatch;
			set {
				int oldvalue = _FirmwarePatch;
				int newvalue = value;

				_FirmwarePatch = newvalue;
				RaisePropertyChanged(nameof(FirmwarePatch), newvalue != oldvalue);
				RaisePropertyChanged(nameof(FirmwareStr),   newvalue != oldvalue);
			}
		}

		protected	int	_FirmwareBuildNumber = 0;
		[XmlElement(ElementName = "FirmwareBuildNumber")]
		public		int	 FirmwareBuildNumber
		{
			get => _FirmwareBuildNumber;
			set {
				int oldvalue = _FirmwareBuildNumber;
				int newvalue = value;

				_FirmwareBuildNumber = newvalue;
				RaisePropertyChanged(nameof(FirmwareBuildNumber), newvalue != oldvalue);
				RaisePropertyChanged(nameof(FirmwareStr),         newvalue != oldvalue);
			}
		}

		protected	int	_FirmwareBuildType = -1;
		[XmlElement(ElementName = "FirmwareBuildType")]
		public		int	 FirmwareBuildType
		{
			get => _FirmwareBuildType;
			set {
				int oldvalue = _FirmwareBuildType;
				int newvalue = value;

				_FirmwareBuildType = newvalue;
				RaisePropertyChanged(nameof(FirmwareBuildType),     newvalue != oldvalue);
				RaisePropertyChanged(nameof(FirmwareBuildTypeFull), newvalue != oldvalue);
				RaisePropertyChanged(nameof(FirmwareBuildTypeStr),  newvalue != oldvalue);
			}
		}

		protected	string	_OEM = string.Empty;
		[XmlElement(ElementName = "OEM")]
		public		string	 OEM
		{
			get => _OEM;
			set {
				string oldvalue = _OEM;
				string newvalue = value ?? string.Empty;

				_OEM = newvalue;
				RaisePropertyChanged(nameof(OEM), newvalue != oldvalue);
			}
		}

		protected	string	_Annex = string.Empty;
		[XmlElement(ElementName = "Annex")]
		public		string	 Annex
		{
			get => _Annex;
			set {
				string oldvalue = _Annex;
				string newvalue = value ?? string.Empty;

				_Annex = newvalue;
				RaisePropertyChanged(nameof(Annex),     newvalue != oldvalue);
				RaisePropertyChanged(nameof(AnnexFull), newvalue != oldvalue);
			}
		}

		protected	string	_Country = string.Empty;
		[XmlElement(ElementName = "Country")]
		public		string	 Country
		{
			get => _Country;
			set {
				string oldvalue = _Country;
				string newvalue = value ?? string.Empty;

				_Country = newvalue;
				RaisePropertyChanged(nameof(Country),     newvalue != oldvalue);
				RaisePropertyChanged(nameof(CountryFull), newvalue != oldvalue);
			}
		}

		protected	string	_Language = string.Empty;
		[XmlElement(ElementName = "Language")]
		public		string	 Language
		{
			get => _Language;
			set {
				string oldvalue = _Language;
				string newvalue = value ?? string.Empty;

				_Language = newvalue;
				RaisePropertyChanged(nameof(Language),     newvalue != oldvalue);
				RaisePropertyChanged(nameof(LanguageFull), newvalue != oldvalue);
			}
		}

		[XmlElement(ElementName = "Flag")]
		public string[] Flag {
			get {
				return Flags.Split(new char[] { '\n', '\r', '\t', ' '}, StringSplitOptions.RemoveEmptyEntries);
			}
			set {
				Flags = value == null ? string.Empty : string.Join("\n", value);
			}
		}

		protected	string	_BaseFritzOS = string.Empty;
		[XmlElement(ElementName = "BaseFritzOS")]
		public		string	 BaseFritzOS
		{
			get => _BaseFritzOS;
			set {
				string oldvalue = _BaseFritzOS;
				string newvalue = value;

				_BaseFritzOS = newvalue;
				RaisePropertyChanged(nameof(BaseFritzOS), newvalue != oldvalue);
			}
		}

		protected	bool	_UpdateAvailable = false;
		[XmlElement(ElementName = "UpdateAvailable")]
		public		bool	 UpdateAvailable
		{
			get => _UpdateAvailable;
			set {
				bool oldvalue = _UpdateAvailable;
				bool newvalue = value;

				_UpdateAvailable = newvalue;
				RaisePropertyChanged(nameof(UpdateAvailable), newvalue != oldvalue);
			}
		}

		protected	string	_UpdateInfo = string.Empty;
		[XmlElement(ElementName = "UpdateInfo")]
		public		string	 UpdateInfo
		{
			get => _UpdateInfo;
			set {
				string oldvalue = _UpdateInfo;
				string newvalue = value ?? string.Empty;

				_UpdateInfo = newvalue;
				RaisePropertyChanged(nameof(UpdateInfo), newvalue != oldvalue);
			}
		}

		protected	string	_UpdateImageURL = string.Empty;
		[XmlElement(ElementName = "UpdateImageURL")]
		public		string	 UpdateImageURL
		{
			get => _UpdateImageURL;
			set {
				string oldvalue = _UpdateImageURL;
				string newvalue = value ?? string.Empty;

				_UpdateImageURL = newvalue;
				RaisePropertyChanged(nameof(UpdateImageURL), newvalue != oldvalue);
				RaisePropertyChanged(nameof(UpdateFileName), newvalue != oldvalue);
			}
		}

		protected	string	_UpdateInfoURL = string.Empty;
		[XmlElement(ElementName = "UpdateInfoURL")]
		public		string	 UpdateInfoURL
		{
			get => _UpdateInfoURL;
			set {
				string oldvalue = _UpdateInfoURL;
				string newvalue = value ?? string.Empty;

				_UpdateInfoURL = newvalue;
				RaisePropertyChanged(nameof(UpdateInfoURL), newvalue != oldvalue);
			}
		}

		protected	DateTime?	_UpdateLastChecked = null;
		[XmlElement(ElementName = "UpdateLastChecked")]
		public		DateTime?	 UpdateLastChecked
		{
			get => _UpdateLastChecked;
			set {
				DateTime? oldvalue = _UpdateLastChecked;
				DateTime? newvalue = value;

				_UpdateLastChecked = newvalue;
				RaisePropertyChanged(nameof(UpdateLastChecked), newvalue != oldvalue);
			}
		}

		[XmlIgnore]
		public string FirmwareBuildTypeStr
		{
			get {
				switch (DeviceKind) {
					case DeviceKind.DECT:
						return string.Empty;

					case DeviceKind.JUIS:
						return FirmwareBuildType.ToString();

					default:
						throw new InvalidOperationException("Unsupported device kind");
				}
			}
		}
		protected	bool	_FirmwareMajorWarning;
		[XmlIgnore]
		public		bool	 FirmwareMajorWarning
		{
			get => _FirmwareMajorWarning;
			set {
				bool oldvalue = _FirmwareMajorWarning;
				bool newvalue = value;

				_FirmwareMajorWarning = newvalue;
				RaisePropertyChanged(nameof(FirmwareMajorWarning), newvalue != oldvalue);
			}
		}

		[XmlIgnore]
		public string FirmwareStr
		{
			get {
				switch (DeviceKind) {
					case DeviceKind.DECT:
						return string.Format("{0:D2}.{1:D2}", FirmwareMajor, FirmwareMinor);

					case DeviceKind.JUIS:
						string format;
						if (Settings.Default.JuisReleaseShowBuildNumber) {
							format = FirmwareBuildNumber == 0 ? "{0}.{1:D2}.{2:D2}" : "{0}.{1:D2}.{2:D2}-{3}";
						} else {
							format = FirmwareBuildType   == 1 ? "{0}.{1:D2}.{2:D2}" : "{0}.{1:D2}.{2:D2}-{3}";
						}
						return string.Format(format, FirmwareMajor, FirmwareMinor, FirmwarePatch, FirmwareBuildNumber);

					default:
						return string.Empty;
				}
			}
		}

		protected	string	_Flags = string.Empty;
		[XmlIgnore]
		public		string	 Flags
		{
			get => _Flags;
			set {
				string oldvalue = _Flags;
				string newvalue = value;

				_Flags  = newvalue;
				RaisePropertyChanged(nameof(Flags), newvalue != oldvalue);
			}
		}

		[XmlIgnore]
		public string HardwareStr
		{
			get {
				switch (DeviceKind) {
					case DeviceKind.DECT:
						return string.Format("{0:D2}.{1:D2}", HardwareMajor, HardwareMinor);

					case DeviceKind.JUIS:
						return string.Format("{0}", HardwareMajor);

					default:
						return string.Empty;
				}
			}
		}

		protected	bool	_IsSelected = false;
		[XmlIgnore]
		public		bool	 IsSelected
		{
			get => _IsSelected;
			set {
				bool oldvalue = _IsSelected;
				bool newvalue = value;

				_IsSelected = newvalue;
				RaisePropertyChanged(nameof(IsSelected), newvalue != oldvalue);
			}
		}

		[XmlIgnore]
		public DataTemplate ToolTipTemplate
		{
			get {
				switch (DeviceKind) {
					case DeviceKind.DECT:
						return (DataTemplate)Application.Current.MainWindow.Resources["DectDeviceToolTipContentTemplate"];

					case DeviceKind.JUIS:
						return (DataTemplate)Application.Current.MainWindow.Resources["JuisDeviceToolTipContentTemplate"];

					default:
						throw new InvalidOperationException("Unsupported device kind");
				}
			}
		}

		[XmlIgnore]
		public string UpdateFileName => string.IsNullOrWhiteSpace(UpdateImageURL) ? string.Empty : UpdateImageURL.Split('/').Last();

		public Device() : this(DeviceKind.Unknown)
		{
		}

		public Device( DeviceKind type, NotifyPropertyChanged notify = NotifyPropertyChanged.ValueChanged )
		{
			DeviceKind            = type;
			notifyPropertyChanged = notify;
			SetFirmwareMajorWarning();
		}

		public Device( Device device )
		{
			DeviceKind = device.DeviceKind;
			device.CopyTo(this);
		}

		public void ClearUpdateInfo()
		{
			UpdateAvailable   = false;
			UpdateInfo        = string.Empty;
			UpdateImageURL    = string.Empty;
			UpdateInfoURL     = string.Empty;
			UpdateLastChecked = null;
		}

		public void CopyTo( Device device, bool boxInfoOnly = false, bool trim = false )
		{
			if (device.DeviceKind != DeviceKind) {
				throw new InvalidOperationException("Attempt to copy data to an incompatible device type");
			}

			if (!boxInfoOnly) {
				device.DeviceName      = trim ? DeviceName.Trim()     : DeviceName;
				device.DeviceAddress   = trim ? DeviceAddress.Trim()  : DeviceAddress;
			}

			device.ProductName         = trim ? ProductName.Trim()    : ProductName;
			device.HardwareMajor       = HardwareMajor;
			device.HardwareMinor       = HardwareMinor;
			device.FirmwareMajor       = FirmwareMajor;
			device.FirmwareMinor       = FirmwareMinor;
			device.FirmwarePatch       = FirmwarePatch;
			device.FirmwareBuildNumber = FirmwareBuildNumber;
			device.FirmwareBuildType   = FirmwareBuildType;
			device.OEM                 = trim ? OEM.Trim()            : OEM;
			device.SerialNumber        = trim ? SerialNumber.Trim()   : SerialNumber;
			device.Annex               = trim ? Annex.Trim()          : Annex;
			device.Country             = trim ? Country.Trim()        : Country;
			device.Language            = trim ? Language.Trim()       : Language;
			device.Flags               = trim ? Flags.Trim()          : Flags;
			device.BaseFritzOS         = trim ? BaseFritzOS.Trim()    : BaseFritzOS;
			device.UpdateAvailable     = UpdateAvailable;
			device.UpdateInfo          = trim ? UpdateInfo.Trim()     : UpdateInfo;
			device.UpdateImageURL      = trim ? UpdateImageURL.Trim() : UpdateImageURL;
			device.UpdateInfoURL       = trim ? UpdateInfoURL.Trim()  : UpdateInfoURL;
			device.UpdateLastChecked   = trim ? UpdateLastChecked     : UpdateLastChecked;
		}

		private int GetFirmwareBuildType( string buildTypeStr, int defaultBuildType )
		{
			switch (buildTypeStr.ToLower()) {
				case "inhaus":
				case "intern":
					return firmwareBuildTypeBetaInhouse;

				case "beta":
				case "extern":
				case "labbeta":
				case "labor":
					return firmwareBuildTypeBetaPublic;

				case "plus":
					return firmwareBuildTypeBetaPLUS;

				default:
					return defaultBuildType;
			}
		}

		public void MakeUpdateCurrent()
		{
			if (!UpdateAvailable) {
				throw new InvalidOperationException("No update available");
			}

			Match match;

			switch (DeviceKind) {
				case DeviceKind.DECT:
					match = Regex.Match(UpdateInfo, @"^(\d+)\.(\d+)$");
					if (match.Success) {
						FirmwareMajor = Convert.ToInt32(match.Groups[1].Value);
						FirmwareMinor = Convert.ToInt32(match.Groups[2].Value);
						ClearUpdateInfo();
						break;
					}

					throw new FormatException();

				case DeviceKind.JUIS:
					match = Regex.Match(UpdateInfo, @"^(\d+)\.(\d+)\.(\d+)[A-Za-z]*$");
					if (match.Success) {
						FirmwareMajor       = Convert.ToInt32(match.Groups[1].Value);
						FirmwareMinor       = Convert.ToInt32(match.Groups[2].Value);
						FirmwarePatch       = Convert.ToInt32(match.Groups[3].Value);
						FirmwareBuildNumber = 0;
						FirmwareBuildType   = firmwareBuildTypeRelease;
						ClearUpdateInfo();
						break;
					}

					match = Regex.Match(UpdateInfo, @"^(\d+)\.(\d+)\.(\d+)-(\d+)$");
					if (match.Success) {
						FirmwareMajor       = Convert.ToInt32(match.Groups[1].Value);
						FirmwareMinor       = Convert.ToInt32(match.Groups[2].Value);
						FirmwarePatch       = Convert.ToInt32(match.Groups[3].Value);
						FirmwareBuildNumber = Convert.ToInt32(match.Groups[4].Value);
						// FirmwareBuildType: no info => keep current build type
						ClearUpdateInfo();
						break;
					}

					match = Regex.Match(UpdateInfo, @"^(\d+)\.(\d+)\.(\d+)-(\d+)\s*([A-Za-z]+)$");
					if (match.Success) {
						FirmwareMajor       = Convert.ToInt32(match.Groups[1].Value);
						FirmwareMinor       = Convert.ToInt32(match.Groups[2].Value);
						FirmwarePatch       = Convert.ToInt32(match.Groups[3].Value);
						FirmwareBuildNumber = Convert.ToInt32(match.Groups[4].Value);
						FirmwareBuildType   = GetFirmwareBuildType(match.Groups[5].Value, FirmwareBuildType);
						ClearUpdateInfo();
						break;
					}

					throw new FormatException();
			}
		}

		public void QueryDevice()
		{
			switch (DeviceKind) {
				case DeviceKind.DECT:
					throw new InvalidOperationException("Attempt to query a DECT device");

				case DeviceKind.JUIS:
					try {
						QueryJuisDevice();
					}
					catch (WebException ex) when (ex.Status == WebExceptionStatus.ProtocolError && ex.Message.Contains("404")) {
						QueryJasonDevice();
					}
					ClearUpdateInfo();
					break;

				default:
					throw new InvalidOperationException("Unsupported device kind");
			}
		}

		protected void QueryJasonDevice()
		{
			if (string.IsNullOrWhiteSpace(DeviceAddress)) {
				throw new ArgumentException("Device address is null or whitespace");
			}

			XmlRootAttribute	xmlRootAttribute  = new XmlRootAttribute() { ElementName = "BoxInfo", Namespace = "http://jason.avm.de/updatecheck/" };
			XmlSerializer		boxInfoSerializer = new XmlSerializer(typeof(Jason.BoxInfo), xmlRootAttribute);

			using (XmlReader xmlReader = XmlReader.Create(string.Format("http://{0}/jason_boxinfo.xml", DeviceAddress.Trim()))) {
				Jason.BoxInfo boxInfo = (Jason.BoxInfo)boxInfoSerializer.Deserialize(xmlReader);

				// General

				ProductName   = boxInfo.Name;
				HardwareMajor = boxInfo.HW;
				SerialNumber  = boxInfo.Serial;
				OEM           = boxInfo.OEM;
				Annex         = boxInfo.Annex;
				Country       = boxInfo.Country;
				Language      = boxInfo.Lang;
				Flag          = boxInfo.Flag;

				Match match = Regex.Match(boxInfo.Version, @"^(\d+)\.(\d+)\.(\d+)$");
				if (!match.Success) {
					throw new FormatException();
				}

				FirmwareMajor       = Convert.ToInt32(match.Groups[1].Value);
				FirmwareMinor       = Convert.ToInt32(match.Groups[2].Value);
				FirmwarePatch       = Convert.ToInt32(match.Groups[3].Value);
				FirmwareBuildNumber = boxInfo.Revision;
				FirmwareBuildType   = GetFirmwareBuildType(boxInfo.Lab, firmwareBuildTypeRelease);
			}
		}

		protected void QueryJuisDevice()
		{
			if (string.IsNullOrWhiteSpace(DeviceAddress)) {
				throw new ArgumentException("Device address is null or whitespace");
			}

			XmlRootAttribute	xmlRootAttribute  = new XmlRootAttribute() { ElementName = "BoxInfo", Namespace = "http://juis.avm.de/updateinfo" };
			XmlSerializer		boxInfoSerializer = new XmlSerializer(typeof(JUIS.BoxInfo), xmlRootAttribute);

			using (XmlReader xmlReader = XmlReader.Create(string.Format("http://{0}/juis_boxinfo.xml", DeviceAddress.Trim()))) {
				JUIS.BoxInfo boxInfo = (JUIS.BoxInfo)boxInfoSerializer.Deserialize(xmlReader);

				ProductName         = boxInfo.Name;
				HardwareMajor       = boxInfo.HW;
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
				Flag                = boxInfo.Flag;
			}
		}

		protected void SetFirmwareMajorWarning()
		{
			FirmwareMajorWarning = DeviceKind == DeviceKind.JUIS &&  (HardwareMajor - FirmwareMajor) != 72;
		}

		// Interface: INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged( string propertyName, bool valueChanged )
		{
			if (valueChanged || notifyPropertyChanged == NotifyPropertyChanged.Always) {
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
