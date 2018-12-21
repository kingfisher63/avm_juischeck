/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) 2018 Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using Muon.DotNetExtensions;
using Muon.Windows;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using JuisCheck.Lang;

namespace JuisCheck
{
	/// <summary>
	/// Interaction logic for DeviceDialog.xaml
	/// </summary>
	public sealed partial class JuisDeviceDialog : Window
	{
		private	Device	origDevice;

		public	Device	DeviceData		{ get;         private set; } = new Device(DeviceKind.JUIS, NotifyPropertyChanged.Always);
		public	int		SelectedIndex	{ private get; set;         } // Dummy for ComboBox SelectedIndex validation

		public	List<ComboBoxValue>	AnnexValues     => Device.GetAnnexValues().AppendMissingValueAsUnknown(DeviceData.Annex);
		public	List<ComboBoxValue>	BuildTypeValues => Device.GetFirmwareBuildTypeValues().AppendMissingValueAsUnknown(DeviceData.FirmwareBuildType);
		public	List<ComboBoxValue>	CountryValues   => Device.GetCountryValues().AppendMissingValueAsUnknown(DeviceData.Country);
		public	List<ComboBoxValue>	LanguageValues  => Device.GetLanguageValues().AppendMissingValueAsUnknown(DeviceData.Language);
		public	List<ComboBoxValue>	OEMValues       => Device.GetOemValues().AppendMissingValueAsUnknown(DeviceData.OEM);

		public JuisDeviceDialog( Device device )
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

		// Routed command: CmdQuery

		public static RoutedCommand CmdQuery = new RoutedCommand();

		private void CmdQuery_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = !string.IsNullOrWhiteSpace(DeviceData.DeviceAddress);
		}

		private void CmdQuery_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			try {
				DeviceData.QueryDevice();
			}
			catch ( Exception ex ) {
				MessageBoxEx.Show(
					new MessageBoxExParams {
						CaptionText = JCstring.messageCaptionError,
						MessageText = string.Format(JCstring.messageTextDeviceQueryFailed.Unescape(), ex.Message),
						Image       = MessageBoxExImage.Error,
						Button      = MessageBoxExButton.OK,
						Owner       = this
					}
				);
			}
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
