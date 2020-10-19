/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using Muon.DotNetExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace JuisCheck
{
	/// <summary>
	/// Interaction logic for DectDeviceDialog.xaml
	/// </summary>
	public sealed partial class DectDeviceDialog : Window
	{
		private	readonly DectDevice	origDevice;

		public	DectDevice			DeviceData		{ get; private set; }
		public	int					SelectedIndex	{ get; set;         }	// Dummy for ComboBox SelectedIndex validation

		public	List<ComboBoxValue>	CountryValues	{ get; private set; }
		public	List<ComboBoxValue>	DectBaseValues	{ get; private set; }
		public	List<ComboBoxValue>	LanguageValues	{ get; private set; }
		public	List<ComboBoxValue>	OEMValues		{ get; private set; }

		public DectDeviceDialog( DectDevice device )
		{
			origDevice = device ?? throw new ArgumentNullException(nameof(device));
			DeviceData = new DectDevice(device);

			InitComboBoxValues(device);
			InitializeComponent();
			DataContext = this;
		}

		private void InitComboBoxValues(DectDevice device)
		{
			CountryValues  = Device.GetCountryValues().AppendMissingAsUnknown(device.Country);
			LanguageValues = Device.GetLanguageValues().AppendMissingAsUnknown(device.Language);
			OEMValues      = Device.GetOemValues().AppendMissingAsUnknown(device.OEM);

			DectBaseValues = new List<ComboBoxValue>();
			foreach (JuisDevice baseDevice in App.GetMainWindow().Devices.Where(d => d is JuisDevice)) {
				DectBaseValues.Add(new ComboBoxValue(baseDevice.ID, baseDevice.DeviceName));
			}
			DectBaseValues.Add(new ComboBoxValue(DectDevice.predefinedDectBaseID, DectDevice.GetPredefinedDectBaseText()));
			DectBaseValues.Sort(new ComboBoxValueComparer(new NaturalStringComparer(App.defaultDisplayStringComparison)));
			DectBaseValues.AppendMissingAsUnknown(device.DectBase);
		}

		// Routed command: CmdOk

		public static readonly RoutedCommand CmdOK = new RoutedCommand();

		private void CmdOK_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = !this.GetTreeHasError(true, true);
		}

		private void CmdOK_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			DeviceData.TrimStrings();
			origDevice.CopyFrom(DeviceData);

			DialogResult = true;
		}

		// Event: TextBox_GotFocus

		private void TextBox_GotFocus_Handler( object sender, RoutedEventArgs evt )
		{
			if (sender is TextBox textBox) {
				textBox.SelectAll();
			}
		}
	}
}
