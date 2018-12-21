/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) 2018 Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

using JuisCheck.JUIS;
using JuisCheck.Lang;
using JuisCheck.Properties;

namespace JuisCheck
{
	[Serializable]
	[XmlType("Device")]
	public class Device : INotifyPropertyChanged
	{
		// Annex

		private static readonly Dictionary<string, string> annexDictionary = new Dictionary<string, string>() {
			{ "A",     JCstring.comboboxValueAnnexA     },
			{ "B",     JCstring.comboboxValueAnnexB     },
			{ "Kabel", JCstring.comboboxValueAnnexCable },
			{ "Ohne",  JCstring.comboboxValueAnnexNone  }
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
					value = string.Format(JCstring.comboboxValueUnknown, Annex);
				}
				return value;
			}
		}

		// FirmwareBuildType

		public const int	firmwareBuildTypeRelease     =    1;
		public const int	firmwareBuildTypeBetaInhouse = 1000;
		public const int	firmwareBuildTypeBetaPublic  = 1001;

		private static readonly Dictionary<int, string> firmwareBuildTypeDictionary = new Dictionary<int, string>() {
			{ firmwareBuildTypeRelease,     JCstring.comboboxValueBuildtypeRelease     },
			{ firmwareBuildTypeBetaInhouse, JCstring.comboboxValueBuildtypeBetaInhouse },
			{ firmwareBuildTypeBetaPublic,  JCstring.comboboxValueBuildtypeBetaPublic  }
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
					value = string.Format(JCstring.comboboxValueUnknown, FirmwareBuildType);
				}
				return value;
			}
		}

		// Country

		private static readonly Dictionary<string, string> countryDictionary = new Dictionary<string, string>()	{
			{ "0234", JCstring.comboboxValueCountry0234 },
			{ "0255", JCstring.comboboxValueCountry0255 },
			{ "0256", JCstring.comboboxValueCountry0256 },
			{ "0264", JCstring.comboboxValueCountry0264 },
			{ "027",  JCstring.comboboxValueCountry027  },
			{ "030",  JCstring.comboboxValueCountry030  },
			{ "031",  JCstring.comboboxValueCountry031  },
			{ "032",  JCstring.comboboxValueCountry032  },
			{ "033",  JCstring.comboboxValueCountry033  },
			{ "034",  JCstring.comboboxValueCountry034  },
			{ "0351", JCstring.comboboxValueCountry0351 },
			{ "0352", JCstring.comboboxValueCountry0352 },
			{ "0353", JCstring.comboboxValueCountry0353 },
			{ "0357", JCstring.comboboxValueCountry0357 },
			{ "0358", JCstring.comboboxValueCountry0358 },
			{ "036",  JCstring.comboboxValueCountry036  },
			{ "0371", JCstring.comboboxValueCountry0371 },
			{ "0372", JCstring.comboboxValueCountry0372 },
			{ "0376", JCstring.comboboxValueCountry0376 },
			{ "0382", JCstring.comboboxValueCountry0382 },
			{ "0385", JCstring.comboboxValueCountry0385 },
			{ "0386", JCstring.comboboxValueCountry0386 },
			{ "0387", JCstring.comboboxValueCountry0387 },
			{ "0389", JCstring.comboboxValueCountry0389 },
			{ "039",  JCstring.comboboxValueCountry039  },
			{ "041",  JCstring.comboboxValueCountry041  },
			{ "0420", JCstring.comboboxValueCountry0420 },
			{ "0421", JCstring.comboboxValueCountry0421 },
			{ "043",  JCstring.comboboxValueCountry043  },
			{ "044",  JCstring.comboboxValueCountry044  },
			{ "045",  JCstring.comboboxValueCountry045  },
			{ "046",  JCstring.comboboxValueCountry046  },
			{ "047",  JCstring.comboboxValueCountry047  },
			{ "048",  JCstring.comboboxValueCountry048  },
			{ "049",  JCstring.comboboxValueCountry049  },
			{ "054",  JCstring.comboboxValueCountry054  },
			{ "061",  JCstring.comboboxValueCountry061  },
			{ "064",  JCstring.comboboxValueCountry064  },
			{ "066",  JCstring.comboboxValueCountry066  },
			{ "0972", JCstring.comboboxValueCountry0972 }
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
					value = string.Format(JCstring.comboboxValueUnknown, Country);
				}
				return value;
			}
		}

		// Language

		private static readonly Dictionary<string, string> languageDictionary = new Dictionary<string, string>()
		{
			{ "de", JCstring.comboboxValueLanguageDE },
			{ "en", JCstring.comboboxValueLanguageEN },
			{ "es", JCstring.comboboxValueLanguageES },
			{ "fr", JCstring.comboboxValueLanguageFR },
			{ "it", JCstring.comboboxValueLanguageIT },
			{ "nl", JCstring.comboboxValueLanguageNL },
			{ "pl", JCstring.comboboxValueLanguagePL }
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
					value = string.Format(JCstring.comboboxValueUnknown, Language);
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
			countryValues.Add(new ComboBoxValue("99", JCstring.comboboxValueCountry99));
			countryDictionary.Add("99", JCstring.comboboxValueCountry99);

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
		[XmlElement("DeviceName")]
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
		[XmlElement("DeviceAddress")]
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
		[XmlElement("ProductName")]
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
		[XmlElement("HardwareMajor")]
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
		[XmlElement("HardwareMinor")]
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
		[XmlElement("SerialNumber")]
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
		[XmlElement("FirmwareMajor")]
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
		[XmlElement("FirmwareMinor")]
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
		[XmlElement("FirmwarePatch")]
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
		[XmlElement("FirmwareBuildNumber")]
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
		[XmlElement("FirmwareBuildType")]
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
		[XmlElement("OEM")]
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
		[XmlElement("Annex")]
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
		[XmlElement("Country")]
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
		[XmlElement("Language")]
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

		[XmlElement("Flag")]
		public string[] Flag {
			get {
				return Flags.Split(new char[] { '\n', '\r', '\t', ' '}, StringSplitOptions.RemoveEmptyEntries);
			}
			set {
				Flags = value == null ? string.Empty : string.Join("\n", value);
			}
		}

		protected	string	_BaseFritzOS = string.Empty;
		[XmlElement("BaseFritzOS")]
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
		[XmlElement("UpdateAvailable")]
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
		[XmlElement("UpdateInfo")]
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
		[XmlElement("UpdateImageURL")]
		public		string	 UpdateImageURL
		{
			get => _UpdateImageURL;
			set {
				string oldvalue = _UpdateImageURL;
				string newvalue = value ?? string.Empty;

				_UpdateImageURL = newvalue;
				RaisePropertyChanged(nameof(UpdateImageURL), newvalue != oldvalue);
			}
		}

		protected	string	_UpdateInfoURL = string.Empty;
		[XmlElement("UpdateInfoURL")]
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
		[XmlElement("UpdateLastChecked")]
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
				device.DeviceName      = trim ? DeviceName.Trim()        : DeviceName;
				device.DeviceAddress   = trim ? DeviceAddress.Trim()     : DeviceAddress;
			}

			device.ProductName         = trim ? ProductName.Trim()       : ProductName;
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

		public void MakeUpdateCurrent()
		{
			if (!UpdateAvailable) {
				throw new InvalidOperationException("No update available");
			}

			char[]		infoSeparator        = new char[] { ' ' };
			string[]	infoParts;

			char[]		dectVersionSeparator = new char[] { '.' };
			char[]		juisVersionSeparator = new char[] { '.', '-' };
			string[]	versionParts;

			infoParts = UpdateInfo.Split(infoSeparator, StringSplitOptions.RemoveEmptyEntries);
			if (infoParts.Length == 0) {
				throw new FormatException();
			}

			switch (DeviceKind) {
				case DeviceKind.DECT:
					versionParts = infoParts[0].Split(dectVersionSeparator);
					if (versionParts.Length !=2) {
						throw new FormatException();
					}

					try {
						FirmwareMajor = (int)Convert.ToUInt32(versionParts[0]);
						FirmwareMinor = (int)Convert.ToUInt32(versionParts[1]);
					}
					catch {
						throw new FormatException();
					}

					ClearUpdateInfo();
					break;

				case DeviceKind.JUIS:
					versionParts = infoParts[0].Split(juisVersionSeparator);
					if (versionParts.Length !=3 && versionParts.Length != 4) {
						throw new FormatException();
					}

					try {
						FirmwareMajor       = (int)Convert.ToUInt32(versionParts[0]);
						FirmwareMinor       = (int)Convert.ToUInt32(versionParts[1]);
						FirmwarePatch       = (int)Convert.ToUInt32(versionParts[2]);
						FirmwareBuildNumber = versionParts.Length == 3 ? 0: (int)Convert.ToUInt32(versionParts[3]);
					}
					catch {
						throw new FormatException();
					}

					if (versionParts.Length == 3) {
						FirmwareBuildType = firmwareBuildTypeRelease;
					} else {
						if (infoParts.Length > 1) {
							switch (infoParts[1].ToLower()) {
								case "inhaus":
								case "intern":
									FirmwareBuildType = firmwareBuildTypeBetaInhouse;
									break;

								case "beta":
								case "extern":
								case "labbeta":
								case "labor":
									FirmwareBuildType = firmwareBuildTypeBetaPublic;
									break;

								default:
									// Keep curr build type
									break;
							}
						}
					}

					ClearUpdateInfo();
					break;
			}
		}

		public void QueryDevice()
		{
			switch (DeviceKind) {
				case DeviceKind.DECT:
					throw new InvalidOperationException("Attempt to query a DECT device");

				case DeviceKind.JUIS:
					QueryJuisDevice();
					break;

				default:
					throw new InvalidOperationException("Unsupported device kind");
			}
		}

		protected void QueryJuisDevice()
		{
			if (string.IsNullOrWhiteSpace(DeviceAddress)) {
				throw new ArgumentException("Device address is null or whitespace");
			}

			XmlRootAttribute	xmlRootAttribute  = new XmlRootAttribute() { ElementName = "BoxInfo", Namespace = "http://juis.avm.de/updateinfo" };
			XmlSerializer		boxInfoSerializer = new XmlSerializer(typeof(BoxInfo), xmlRootAttribute);

			using (XmlReader xmlReader = XmlReader.Create(string.Format("http://{0}/juis_boxinfo.xml", DeviceAddress.Trim()))) {
				BoxInfo boxInfo = (BoxInfo)boxInfoSerializer.Deserialize(xmlReader);

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

				ClearUpdateInfo();
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
