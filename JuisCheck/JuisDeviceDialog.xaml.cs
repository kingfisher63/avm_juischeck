/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using Muon.DotNetExtensions;
using Muon.Windows;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
		private	readonly JuisDevice	origDevice;

		public	JuisDevice			DeviceData			{ get; private set; }
		public	int					SelectedIndex		{ get; set;         }	// Dummy for ComboBox SelectedIndex validation

		public	List<ComboBoxValue>	AnnexValues			{ get; private set; }
		public	List<ComboBoxValue>	BuildTypeValues		{ get; private set; }
		public	List<ComboBoxValue>	CountryValues		{ get; private set; }
		public	List<ComboBoxValue>	LanguageValues		{ get; private set; }
		public	List<ComboBoxValue> MeshMasterValues	{ get; private set; }
		public	List<ComboBoxValue>	OEMValues			{ get; private set; }

		public JuisDeviceDialog( JuisDevice device )
		{
			origDevice = device ?? throw new ArgumentNullException(nameof(device));
			DeviceData = new JuisDevice(device);

			InitComboBoxValues(device);
			InitializeComponent();
			DataContext = this;
		}

		// Initialize the Mesh Master Values list

		private void InitComboBoxValues( JuisDevice device )
		{
			AnnexValues     = JuisDevice.GetAnnexValues().AppendMissingAsUnknown(device.Annex);
			BuildTypeValues = JuisDevice.GetFirmwareBuildTypeValues().AppendMissingAsUnknown(device.FirmwareBuildType);
			CountryValues   = Device.GetCountryValues().AppendMissingAsUnknown(device.Country);
			LanguageValues  = Device.GetLanguageValues().AppendMissingAsUnknown(device.Language);
			OEMValues       = Device.GetOemValues().AppendMissingAsUnknown(device.OEM);

			MeshMasterValues = new List<ComboBoxValue>();
			foreach (JuisDevice masterDevice in App.GetMainWindow().Devices.Where(d => d is JuisDevice && d.ID != device.ID)) {
				MeshMasterValues.Add(new ComboBoxValue(masterDevice.ID, masterDevice.DeviceName));
			}
			MeshMasterValues.Sort(new ComboBoxValueComparer(new NaturalStringComparer(App.defaultDisplayStringComparison)));
			MeshMasterValues.Prepend(string.Empty, JCstring.ComboBoxValueNoMaster);
			MeshMasterValues.AppendMissingAsUnknown(device.MeshMaster);
		}

		// Routed command: CmdOk

		public static readonly RoutedCommand CmdOK = new RoutedCommand();

		private void CmdOK_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = !this.GetTreeHasError();
		}

		private void CmdOK_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			DeviceData.TrimStrings();
			origDevice.CopyFrom(DeviceData);

			DialogResult = true;
		}

		// Routed command: CmdQuery

		public static readonly RoutedCommand CmdQuery = new RoutedCommand();

		private void CmdQuery_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = !string.IsNullOrWhiteSpace(DeviceData.DeviceAddress);
		}

		private void CmdQuery_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			try {
				DeviceData.QueryDevice();
			}
			catch (Exception ex) {
				MessageBoxEx2.Show(new MessageBoxEx2Params {
					CaptionText = JCstring.MessageCaptionError,
					MessageText = string.Format(CultureInfo.CurrentCulture, JCstring.MessageTextDeviceQueryFailed.Unescape(), ex.Message),
					Image       = MessageBoxEx2Image.Error,
					ButtonText  = new string[] { JCstring.DialogButtonTextOk },
					Owner       = this
				});
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
