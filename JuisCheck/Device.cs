/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

using JuisCheck.Lang;

namespace JuisCheck
{
	public abstract class Device : INotifyPropertyChanged
	{
		/**************************************/
		/* Country values and display strings */
		/**************************************/

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
			{ "0972", JCstring.ComboBoxValueCountry0972 },
			{ "99",   JCstring.ComboBoxValueCountry99   }
		};

		public static List<ComboBoxValue> GetCountryValues()
		{
			List<ComboBoxValue> countryValues = countryDictionary.Select(country => new ComboBoxValue(country.Key, country.Value)).ToList();
			countryValues.Sort(0, countryValues.Count-1, new ComboBoxValueComparer(App.defaultDisplayStringComparison));

			return countryValues;
		}

		/***************************************/
		/* Language values and display strings */
		/***************************************/

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

		public static List<ComboBoxValue> GetLanguageValues()
		{
			List<ComboBoxValue> languageValues = languageDictionary.Select(language => new ComboBoxValue(language.Key, language.Value)).ToList();
			languageValues.Sort(new ComboBoxValueComparer(App.defaultDisplayStringComparison));

			return languageValues;
		}

		/**********************************/
		/* OEM values and display strings */
		/**********************************/

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

		/**********************************/
		/* Generic device properties      */
		/* - including derived properties */
		/* - in alphabetical order        */
		/**********************************/

		private string _Country;
		public  string  Country
		{
			get => _Country ?? string.Empty;
			set {
				if (_Country != value) {
					_Country  = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(CountryFull));
				}
			}
		}

		public string CountryFull
		{
			get {
				if (!countryDictionary.TryGetValue(Country, out string value)) {
					value = string.Format(CultureInfo.CurrentCulture, JCstring.ComboBoxValueUnknown, Country);
				}
				return value;
			}
		}

		private string _DeviceName;
		public  string  DeviceName
		{
			get => _DeviceName ?? string.Empty;
			set {
				if (_DeviceName != value) {
					_DeviceName  = value;
					NotifyPropertyChanged();
				}
			}
		}

		private int _FirmwareMajor;
		public  int  FirmwareMajor
		{
			get => _FirmwareMajor;
			set {
				if (_FirmwareMajor != value) {
					_FirmwareMajor  = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(JuisDevice.FirmwareMajorWarning));
					NotifyPropertyChanged(nameof(FirmwareStr));
				}
			}
		}

		private int _FirmwareMinor;
		public  int  FirmwareMinor
		{
			get => _FirmwareMinor;
			set {
				if (_FirmwareMinor != value) {
					_FirmwareMinor  = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(FirmwareStr));
				}
			}
		}

		private string _ID;
		public  string  ID
		{
			get => _ID ?? string.Empty;
			set {
				if (_ID != value) {
					_ID  = value;
					NotifyPropertyChanged();
				}
			}
		}

		private bool _IsSelected;
		public  bool  IsSelected
		{
			get => _IsSelected;
			set {
				if (_IsSelected != value) {
					_IsSelected  = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _Language;
		public  string  Language
		{
			get => _Language ?? string.Empty;
			set {
				if (_Language != value) {
					_Language  = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(LanguageFull));
				}
			}
		}

		public string LanguageFull
		{
			get {
				if (!languageDictionary.TryGetValue(Language, out string value)) {
					value = string.Format(CultureInfo.CurrentCulture, JCstring.ComboBoxValueUnknown, Language);
				}
				return value;
			}
		}

		private string _OEM;
		public  string  OEM
		{
			get => _OEM ?? string.Empty;
			set {
				if (_OEM != value) {
					_OEM  = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _ProductName;
		public  string  ProductName
		{
			get => _ProductName ?? string.Empty;
			set {
				if (_ProductName != value) {
					_ProductName  = value;
					NotifyPropertyChanged();
				}
			}
		}

		private bool _UpdateAvailable;
		public  bool  UpdateAvailable
		{
			get => _UpdateAvailable;
			set {
				if (_UpdateAvailable != value) {
					_UpdateAvailable  = value;
					NotifyPropertyChanged();
				}
			}
		}

		public string UpdateFileName
		{
			get => string.IsNullOrWhiteSpace(UpdateImageURL) ? string.Empty : UpdateImageURL.Split('/').Last();
		}

		private string _UpdateImageURL;
		public  string  UpdateImageURL
		{
			get => _UpdateImageURL ?? string.Empty;
			set {
				if (_UpdateImageURL != value) {
					_UpdateImageURL  = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(UpdateFileName));
				}
			}
		}

		private string _UpdateInfo;
		public  string  UpdateInfo
		{
			get => _UpdateInfo ?? string.Empty;
			set {
				if (_UpdateInfo != value) {
					_UpdateInfo  = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _UpdateInfoURL;
		public  string  UpdateInfoURL
		{
			get => _UpdateInfoURL ?? string.Empty;
			set {
				if (_UpdateInfoURL != value) {
					_UpdateInfoURL  = value;
					NotifyPropertyChanged();
				}
			}
		}

		private DateTime? _UpdateLastChecked;
		public  DateTime?  UpdateLastChecked
		{
			get => _UpdateLastChecked;
			set {
				if (_UpdateLastChecked != value) {
					_UpdateLastChecked  = value;
					NotifyPropertyChanged();
				}
			}
		}

		/***********************/
		/* Abstract properties */
		/***********************/

		public abstract string			DeviceAddressStr		{ get; }
		public abstract string			FirmwareBuildTypeStr	{ get; }
		public abstract string			FirmwareStr				{ get; }
		public abstract string			HardwareStr				{ get; }
		public abstract string			MasterBaseStr			{ get; }
		public abstract DataTemplate	ToolTipTemplate			{ get; }

		/*****************/
		/* Other methods */
		/*****************/

		public void ClearUpdate()
		{
			UpdateAvailable   = false;
			UpdateInfo        = string.Empty;
			UpdateImageURL    = string.Empty;
			UpdateInfoURL     = string.Empty;
			UpdateLastChecked = null;
		}

		protected void CopyFrom( Device srcDevice )
		{
			if (srcDevice == null) {
				throw new ArgumentNullException(nameof(srcDevice));
			}

			Country           = srcDevice.Country;
			DeviceName        = srcDevice.DeviceName;
			FirmwareMajor     = srcDevice.FirmwareMajor;
			FirmwareMinor     = srcDevice.FirmwareMinor;
			Language          = srcDevice.Language;
			OEM               = srcDevice.OEM;
			ProductName       = srcDevice.ProductName;
			UpdateAvailable   = srcDevice.UpdateAvailable;
			UpdateImageURL    = srcDevice.UpdateImageURL;
			UpdateInfo        = srcDevice.UpdateInfo;
			UpdateInfoURL     = srcDevice.UpdateInfoURL;
			UpdateLastChecked = srcDevice.UpdateLastChecked;
		}

		protected void TrimStrings()
		{
			Country        = Country.Trim();
			DeviceName     = DeviceName.Trim();
			Language       = Language.Trim();
			OEM            = OEM.Trim();
			ProductName    = ProductName.Trim();
			UpdateImageURL = UpdateImageURL.Trim();
			UpdateInfo     = UpdateInfo.Trim();
			UpdateInfoURL  = UpdateInfoURL.Trim();
		}

		/********************/
		/* Abstract methods */
		/********************/

		public abstract void	NotifyDeviceNameChanged( string deviceID );
		public abstract bool	Edit( Window owner = null );
		public abstract void	MakeUpdateCurrent();

		// Interface: INotifyPropertyChanged

		public event PropertyChangedEventHandler	PropertyChanged;

		protected void NotifyPropertyChanged( [CallerMemberName] string propertyName = null )
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
