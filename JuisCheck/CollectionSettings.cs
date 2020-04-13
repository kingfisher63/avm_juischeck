/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

using JuisCheck.Properties;

namespace JuisCheck
{
	public sealed class CollectionSettings : INotifyPropertyChanged
	{
		private static readonly Settings					programSettings = Settings.Default;
		private readonly		Dictionary<string,string>	settings        = new Dictionary<string, string>();

		public const string keyDataGridColumnVisibleCountry = "DataGridColumnVisibleCountry";
		public bool DataGridColumnVisibleCountry
		{
			get => GetBoolValue(keyDataGridColumnVisibleCountry);
			set {
				settings[keyDataGridColumnVisibleCountry] = value.ToString(CultureInfo.InvariantCulture);
				NotifyPropertyChanged();
			}
		}

		public const string keyDataGridColumnVisibleDeviceAddress = "DataGridColumnVisibleDeviceAddress";
		public bool DataGridColumnVisibleDeviceAddress
		{
			get => GetBoolValue(keyDataGridColumnVisibleDeviceAddress);
			set {
				settings[keyDataGridColumnVisibleDeviceAddress] = value.ToString(CultureInfo.InvariantCulture);
				NotifyPropertyChanged();
			}
		}

		public const string keyDataGridColumnVisibleFirmware = "DataGridColumnVisibleFirmware";
		public bool DataGridColumnVisibleFirmware
		{
			get => GetBoolValue(keyDataGridColumnVisibleFirmware);
			set {
				settings[keyDataGridColumnVisibleFirmware] = value.ToString(CultureInfo.InvariantCulture);
				NotifyPropertyChanged();
			}
		}

		public const string keyDataGridColumnVisibleFirmwareBuildType = "DataGridColumnVisibleFirmwareBuildType";
		public bool DataGridColumnVisibleFirmwareBuildType
		{
			get => GetBoolValue(keyDataGridColumnVisibleFirmwareBuildType);
			set {
				settings[keyDataGridColumnVisibleFirmwareBuildType] = value.ToString(CultureInfo.InvariantCulture);
				NotifyPropertyChanged();
			}
		}

		public const string keyDataGridColumnVisibleHardware = "DataGridColumnVisibleHardware";
		public bool DataGridColumnVisibleHardware
		{
			get => GetBoolValue(keyDataGridColumnVisibleHardware);
			set {
				settings[keyDataGridColumnVisibleHardware] = value.ToString(CultureInfo.InvariantCulture);
				NotifyPropertyChanged();
			}
		}

		public const string keyDataGridColumnVisibleLanguage = "DataGridColumnVisibleLanguage";
		public bool DataGridColumnVisibleLanguage
		{
			get => GetBoolValue(keyDataGridColumnVisibleLanguage);
			set {
				settings[keyDataGridColumnVisibleLanguage] = value.ToString(CultureInfo.InvariantCulture);
				NotifyPropertyChanged();
			}
		}

		public const string keyDataGridColumnVisibleMasterBase = "DataGridColumnVisibleMasterBase";
		public bool DataGridColumnVisibleMasterBase
		{
			get => GetBoolValue(keyDataGridColumnVisibleMasterBase);
			set {
				settings[keyDataGridColumnVisibleMasterBase] = value.ToString(CultureInfo.InvariantCulture);
				NotifyPropertyChanged();
			}
		}

		public const string keyDataGridColumnVisibleOEM = "DataGridColumnVisibleOEM";
		public bool DataGridColumnVisibleOEM
		{
			get => GetBoolValue(keyDataGridColumnVisibleOEM);
			set {
				settings[keyDataGridColumnVisibleOEM] = value.ToString(CultureInfo.InvariantCulture);
				NotifyPropertyChanged();
			}
		}

		public const string keyDataGridColumnVisibleProductName = "DataGridColumnVisibleProductName";
		public bool DataGridColumnVisibleProductName
		{
			get => GetBoolValue(keyDataGridColumnVisibleProductName);
			set {
				settings[keyDataGridColumnVisibleProductName] = value.ToString(CultureInfo.InvariantCulture);
				NotifyPropertyChanged();
			}
		}

		public const string keyDataGridColumnVisibleUpdateFileName = "DataGridColumnVisibleUpdateFileName";
		public bool DataGridColumnVisibleUpdateFileName
		{
			get => GetBoolValue(keyDataGridColumnVisibleUpdateFileName);
			set {
				settings[keyDataGridColumnVisibleUpdateFileName] = value.ToString(CultureInfo.InvariantCulture);
				NotifyPropertyChanged();
			}
		}

		public const string keyDataGridColumnVisibleUpdateInfo = "DataGridColumnVisibleUpdateInfo";
		public bool DataGridColumnVisibleUpdateInfo
		{
			get => GetBoolValue(keyDataGridColumnVisibleUpdateInfo);
			set {
				settings[keyDataGridColumnVisibleUpdateInfo] = value.ToString(CultureInfo.InvariantCulture);
				NotifyPropertyChanged();
			}
		}

		public const string keyDataGridColumnVisibleUpdateLastChecked = "DataGridColumnVisibleUpdateLastChecked";
		public bool DataGridColumnVisibleUpdateLastChecked
		{
			get => GetBoolValue(keyDataGridColumnVisibleUpdateLastChecked);
			set {
				settings[keyDataGridColumnVisibleUpdateLastChecked] = value.ToString(CultureInfo.InvariantCulture);
				NotifyPropertyChanged();
			}
		}

		/****************/
		/* Constructors */
		/****************/

		public CollectionSettings()
		{
			Reset();
		}

		/******************/
		/* Other medthods */
		/******************/

		public void Add( string settingName, string settingValue )
		{
			if (settingName  == null) { throw new ArgumentNullException(nameof(settingName));  }
			if (settingValue == null) { throw new ArgumentNullException(nameof(settingValue)); }

			settings[settingName] = settingValue;
		}

		private bool GetBoolValue( string settingName, bool defaultValue = false ) {
			if (settingName == null) { throw new ArgumentNullException(nameof(settingName)); }

			if (settings.TryGetValue(settingName, out string stringValue)) {
				if (bool.TryParse(stringValue, out bool value)) {
					return value;
				}
			}

			return defaultValue;
		}

		public Dictionary<string,string> GetDictionary()
		{
			return settings;
		}

		public void Reset()
		{
			SetDefaultsValues();
		}

		public void NotifySettingsProperties()
		{
			NotifyPropertyChanged(nameof(DataGridColumnVisibleCountry));
			NotifyPropertyChanged(nameof(DataGridColumnVisibleDeviceAddress));
			NotifyPropertyChanged(nameof(DataGridColumnVisibleFirmware));
			NotifyPropertyChanged(nameof(DataGridColumnVisibleFirmwareBuildType));
			NotifyPropertyChanged(nameof(DataGridColumnVisibleHardware));
			NotifyPropertyChanged(nameof(DataGridColumnVisibleLanguage));
			NotifyPropertyChanged(nameof(DataGridColumnVisibleMasterBase));
			NotifyPropertyChanged(nameof(DataGridColumnVisibleOEM));
			NotifyPropertyChanged(nameof(DataGridColumnVisibleProductName));
			NotifyPropertyChanged(nameof(DataGridColumnVisibleUpdateFileName));
			NotifyPropertyChanged(nameof(DataGridColumnVisibleUpdateInfo));
			NotifyPropertyChanged(nameof(DataGridColumnVisibleUpdateLastChecked));
		}

		private void SetDefaultsValues()
		{
			DataGridColumnVisibleCountry           = programSettings.DefaultDataGridColumnVisibleCountry;
			DataGridColumnVisibleDeviceAddress     = programSettings.DefaultDataGridColumnVisibleDeviceAddress;
			DataGridColumnVisibleFirmware          = programSettings.DefaultDataGridColumnVisibleFirmware;
			DataGridColumnVisibleFirmwareBuildType = programSettings.DefaultDataGridColumnVisibleFirmwareBuildType;
			DataGridColumnVisibleHardware          = programSettings.DefaultDataGridColumnVisibleHardware;
			DataGridColumnVisibleLanguage          = programSettings.DefaultDataGridColumnVisibleLanguage;
			DataGridColumnVisibleMasterBase        = programSettings.DefaultDataGridColumnVisibleMasterBase;
			DataGridColumnVisibleOEM               = programSettings.DefaultDataGridColumnVisibleOEM;
			DataGridColumnVisibleProductName       = programSettings.DefaultDataGridColumnVisibleProductName;
			DataGridColumnVisibleUpdateFileName    = programSettings.DefaultDataGridColumnVisibleUpdateFileName;
			DataGridColumnVisibleUpdateInfo        = programSettings.DefaultDataGridColumnVisibleUpdateInfo;
			DataGridColumnVisibleUpdateLastChecked = programSettings.DefaultDataGridColumnVisibleUpdateLastChecked;
		}

		// Interface: INotifyPropertyChanged

		public event PropertyChangedEventHandler	PropertyChanged;

		private void NotifyPropertyChanged( [CallerMemberName] string propertyName = null )
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
