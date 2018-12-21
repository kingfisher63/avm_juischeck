/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) 2018 Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using Muon.DotNetExtensions;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace JuisCheck
{
	/// <summary>
	/// Interaction logic for DectDeviceDialog.xaml
	/// </summary>
	public partial class DectDeviceDialog : Window
	{
		private	Device	origDevice;

		public	Device	DeviceData		{ get;         private set; } = new Device(DeviceKind.DECT, NotifyPropertyChanged.Always);
		public	int		SelectedIndex	{ private get; set;         } // Dummy for ComboBox SelectedIndex validation

		public	List<ComboBoxValue>	CountryValues  => Device.GetCountryValues().AppendMissingValueAsUnknown(DeviceData.Country);
		public	List<ComboBoxValue>	LanguageValues => Device.GetLanguageValues().AppendMissingValueAsUnknown(DeviceData.Language);
		public	List<ComboBoxValue>	OEMValues      => Device.GetOemValues().AppendMissingValueAsUnknown(DeviceData.OEM);

		public DectDeviceDialog( Device device )
		{
			origDevice = device;
			origDevice.CopyTo(DeviceData);

			DataContext = this;
			InitializeComponent();
		}

		// Routed command: CmdOk

		public static RoutedCommand CmdOK = new RoutedCommand();

		private void CmdOK_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = !this.GetTreeHasError();
		}

		private void CmdOK_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			DeviceData.CopyTo(origDevice, false, true);
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
