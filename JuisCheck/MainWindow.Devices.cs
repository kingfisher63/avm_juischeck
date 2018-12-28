/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) 2018 Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using Fluent;
using Muon.DotNetExtensions;
using Muon.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

using JuisCheck.Lang;

namespace JuisCheck
{
	public sealed partial class MainWindow : RibbonWindow
	{
		private ICollectionView Devices_collectionView;

		private void Devices_EditDevice( Device device )
		{
			Window deviceDialog;

			switch (device.DeviceKind) {
				case DeviceKind.DECT:
					deviceDialog = new DectDeviceDialog(device);
					break;

				case DeviceKind.JUIS:
					deviceDialog = new JuisDeviceDialog(device);
					break;

				default:
					throw new InvalidOperationException("Unsupported device kind");
			}

			deviceDialog.Owner                 = this;
			deviceDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

			if (deviceDialog.ShowDialog() == true) {
				if (Devices.IndexOf(device) < 0) {
					Devices.Add(device);
				}

				Devices_RefreshView();
				Devices_SelectDevice(device);
			}
		}

		private void Devices_Init()
		{
			Devices_collectionView = CollectionViewSource.GetDefaultView(dgDevices.ItemsSource);
			Devices_InitSorting();
		}

		private void Devices_InitSorting()
		{
			dgcDeviceName .SortDirection = null;
			dgcProductName.SortDirection = null;

			DeviceComparer.SetComparer(Devices_collectionView as ListCollectionView, dgcDeviceName);
		}

		private void Devices_RefreshView()
		{
			Devices_collectionView.Refresh();
		}

		private void Devices_SelectAll()
		{
			foreach (Device device in Devices) {
				device.IsSelected = true;
			}
		}

		private void Devices_SelectDevice( Device device )
		{
			Devices_SelectNone();
			device.IsSelected = true;
			dgDevices.ScrollIntoView(device);
		}

		private void Devices_SelectNone()
		{
			dgDevices.SelectedItem = null;
			foreach (Device device in Devices) {
				device.IsSelected = false;
			}
		}

		// Predicate functions

		private static bool IsSelectedDevice					(Device d) => d.IsSelected;
		private static bool IsSelectedDeviceWithUpdateAvailable	(Device d) => d.IsSelected && d.UpdateAvailable;
		private static bool IsSelectedDeviceWithUpdateInfo		(Device d) => d.IsSelected && !string.IsNullOrWhiteSpace(d.UpdateInfo);
		private static bool IsSelectedDeviceWithUpdateImageURL	(Device d) => d.IsSelected && !string.IsNullOrWhiteSpace(d.UpdateImageURL);

		// Routed command: Devices_CmdAddDECT

		public static RoutedCommand Devices_CmdAddDECT = new RoutedCommand();

		private void Devices_CmdAddDECT_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = true;
		}

		private void Devices_CmdAddDECT_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Device newDevice = new Device(DeviceKind.DECT) {
				Annex       = AppSettings.DefaultAnnex,
				BaseFritzOS = AppSettings.DefaultBaseFritzOS,
				Country     = AppSettings.DefaultCountry,
				Language    = AppSettings.DefaultLanguage,
				OEM         = AppSettings.DefaultOEM
			};

			Devices_EditDevice(newDevice);
			Devices_SetDataGridFocus();
		}

		private void Devices_SetDataGridFocus()
		{
			dgDevices.Focus();
		}

		// Routed command: Devices_CmdAddJUIS

		public static RoutedCommand Devices_CmdAddJUIS = new RoutedCommand();

		private void Devices_CmdAddJUIS_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = true;
		}

		private void Devices_CmdAddJUIS_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Device newDevice = new Device(DeviceKind.JUIS) {
				Annex             = AppSettings.DefaultAnnex,
				FirmwareBuildType = AppSettings.DefaultFirmwareBuildType,
				Country           = AppSettings.DefaultCountry,
				Language          = AppSettings.DefaultLanguage,
				OEM               = AppSettings.DefaultOEM
			};

			Devices_EditDevice(newDevice);
			Devices_SetDataGridFocus();
		}

		// Routed command: Devices_CmdClearUpdates

		public static RoutedCommand Devices_CmdClearUpdates = new RoutedCommand();

		private void Devices_CmdClearUpdates_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = Devices.Any(predicate: IsSelectedDeviceWithUpdateInfo);
		}

		private void Devices_CmdClearUpdates_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			foreach (Device device in Devices.Where(predicate: IsSelectedDeviceWithUpdateInfo)) {
				device.ClearUpdateInfo();
			}

			Devices_RefreshView();
			Devices_SetDataGridFocus();
		}

		// Routed command: Devices_CmdCopy

		public static RoutedCommand Devices_CmdCopy = new RoutedCommand();

		private void Devices_CmdCopy_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = Devices.Count(predicate: IsSelectedDevice) == 1;
		}

		private void Devices_CmdCopy_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Devices_EditDevice(new Device(Devices.First(predicate: IsSelectedDevice)));
			Devices_SetDataGridFocus();
		}

		// Routed command: Devices_CmdCopyURLs

		public static RoutedCommand Devices_CmdCopyURLs = new RoutedCommand();

		private void Devices_CmdCopyURLs_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = Devices.Any(predicate: IsSelectedDeviceWithUpdateImageURL);
		}

		private void Devices_CmdCopyURLs_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			string[] urls = Devices.Where(predicate: IsSelectedDeviceWithUpdateImageURL).Select( d => d.UpdateImageURL ).ToArray();
			if (!App.SafeClipboardSetText(string.Join("\r\n", urls) + "\r\n")) {
				ShowErrorMessage(JCstring.MessageTextClipboardCopyFailure.Unescape());
			}

			Devices_SetDataGridFocus();
		}

		// Routed command: Devices_CmdDelete

		public static RoutedCommand Devices_CmdDelete = new RoutedCommand();

		private void Devices_CmdDelete_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = Devices.Any(predicate: IsSelectedDevice);
		}

		private void Devices_CmdDelete_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			int		count   = Devices.Count(predicate: IsSelectedDevice);
			string	message = count == 1 ? JCstring.MessageTextDeleteOneDevice : string.Format(JCstring.MessageTextDeleteMultipleDevices, count);

			MessageBoxExResult result = MessageBoxEx.Show(
				new MessageBoxExParams {
					CaptionText = JCstring.MessageCaptionDelete,
					MessageText = message,
					Image       = MessageBoxExImage.Question,
					Button      = MessageBoxExButton.YesNo,
					Owner       = this
				}
			);

			if (result == MessageBoxExResult.Yes) {
				List<Device> deleteDevices = Devices.Where(predicate: IsSelectedDevice).ToList();
				foreach (Device device in deleteDevices) {
					Devices.Remove(device);
				}
			}

			Devices_SetDataGridFocus();
		}

		// Routed command: Devices_CmdEdit

		public static RoutedCommand Devices_CmdEdit = new RoutedCommand();

		private void Devices_CmdEdit_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = Devices.Count(predicate: IsSelectedDevice) == 1;
		}

		private void Devices_CmdEdit_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Devices_EditDevice(Devices.First(predicate: IsSelectedDevice));
			Devices_SetDataGridFocus();
		}

		// Routed command: Devices_CmdFindUpdates

		public static RoutedCommand Devices_CmdFindUpdates = new RoutedCommand();

		private void Devices_CmdFindUpdates_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = Devices.Any(predicate: IsSelectedDevice);
		}

		private void Devices_CmdFindUpdates_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			List<Device>		devices     = new List<Device>(Devices_collectionView.Cast<Device>());
			FindUpdatesDialog	findDialog  = new FindUpdatesDialog(devices.Where(predicate: IsSelectedDevice).ToList()) {
				Owner                 = this,
				WindowStartupLocation = WindowStartupLocation.CenterOwner
			};

			findDialog.ShowDialog();
			findDialog.Dispose();

			Devices_RefreshView();
			Devices_SetDataGridFocus();
		}

		// Routed command: Devices_CmdGetImage

		public static RoutedCommand Devices_CmdGetImage = new RoutedCommand();

		private void Devices_CmdGetImage_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			if (Devices.Count(predicate: IsSelectedDevice) == 1) {
				evt.CanExecute = !string.IsNullOrWhiteSpace(Devices.First(predicate: IsSelectedDevice).UpdateImageURL);
			}
		}

		private void Devices_CmdGetImage_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Process.Start(Devices.First(predicate: IsSelectedDevice).UpdateImageURL);
			Devices_SetDataGridFocus();
		}

		// Routed command: Devices_CmdMakeCurrent

		public static RoutedCommand Devices_CmdMakeCurrent = new RoutedCommand();

		private void Devices_CmdMakeCurrent_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = Devices.Any(predicate: IsSelectedDeviceWithUpdateAvailable);
		}

		private void Devices_CmdMakeCurrent_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			foreach (Device device in Devices.Where(predicate: IsSelectedDeviceWithUpdateAvailable)) {
				try {
					device.MakeUpdateCurrent();
				}
				catch (FormatException) {
					ShowErrorMessage(string.Format(JCstring.MessageTextInvalidUpdateVersion.Unescape(), device.DeviceName, device.UpdateInfo));
				}
			}

			Devices_SetDataGridFocus();
		}

		// Routed command: Devices_CmdSelectAll

		public static RoutedCommand Devices_CmdSelectAll = new RoutedCommand();

		private void Devices_CmdSelectAll_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = Devices.Count != 0;
		}

		private void Devices_CmdSelectAll_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Devices_SelectAll();
			Devices_SetDataGridFocus();
		}

		// Routed command: Devices_CmdSelectNone

		public static RoutedCommand Devices_CmdSelectNone = new RoutedCommand();

		private void Devices_CmdSelectNone_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = Devices.Count != 0;
		}

		private void Devices_CmdSelectNone_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Devices_SelectNone();
			Devices_SetDataGridFocus();
		}

		// Routed command: Devices_CmdViewInfo

		public static RoutedCommand Devices_CmdViewInfo = new RoutedCommand();

		private void Devices_CmdViewInfo_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			if (Devices.Count(predicate: IsSelectedDevice) == 1) {
				evt.CanExecute = !string.IsNullOrWhiteSpace(Devices.First(predicate: IsSelectedDevice).UpdateInfoURL);
			}
		}

		private void Devices_CmdViewInfo_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Process.Start(Devices.First(predicate: IsSelectedDevice).UpdateInfoURL);
			Devices_SetDataGridFocus();
		}

		// Event: Devices_DataGrid_MouseDoubleClick

		private void Devices_DataGrid_MouseDoubleClick_Handler( object sender, MouseButtonEventArgs evt )
		{
			if (ItemsControl.ContainerFromElement(sender as DataGrid, evt.OriginalSource as DependencyObject) is DataGridRow row) {
				dgDevices.SelectedItem = row.Item;
				Devices_EditDevice(row.Item as Device);
			}

			Devices_SetDataGridFocus();
		}

		// Event: Devices_DataGrid_Sorting

		private void Devices_DataGrid_Sorting_Handler( object sender, DataGridSortingEventArgs evt )
		{
			evt.Handled = DeviceComparer.SetComparer(Devices_collectionView as ListCollectionView, evt.Column);
			Devices_SelectNone();
		}
	}
}
