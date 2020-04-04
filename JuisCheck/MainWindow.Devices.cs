/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using Fluent;
using Microsoft.Win32;
using Muon.DotNetExtensions;
using Muon.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

using JuisCheck.Lang;
using JuisCheck.Properties;

namespace JuisCheck
{
	public sealed partial class MainWindow : RibbonWindow
	{
		public	DeviceCollection		Devices				{ get; private set; } = new DeviceCollection();
		public	CollectionViewSource	Devices_ViewSource	{ get; private set; }

		private void Devices_Init1()
		{
		}

		private void Devices_Init2()
		{
			Devices_ViewSource = (CollectionViewSource)Resources["cvsDevices"];
			Devices_InitDataGrid();
		}

		private void Devices_InitDataGrid()
		{
			foreach (DataGridColumn column in dgDevices.Columns) {
				column.SortDirection = null;
				column.Width         = new DataGridLength(0, DataGridLengthUnitType.Pixel);
				column.Width         = new DataGridLength(0, DataGridLengthUnitType.Auto );
			}

			Devices_SetComparer((ListCollectionView)Devices_ViewSource.View, dgcDeviceName);
		}

		private void Devices_RefreshView()
		{
			Devices_ViewSource.View.Refresh();
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

		private static bool Devices_SetComparer( ListCollectionView collectionView, DataGridColumn column )
		{
			DeviceProperty		sortProperty;
			ListSortDirection	sortDirection;

			if (collectionView == null || column == null) {
				return false;
			}

			switch (column.SortMemberPath) {
				case nameof(Device.DeviceAddressStr):	sortProperty = DeviceProperty.DeviceAddressStr; break;
				case nameof(Device.DeviceName):			sortProperty = DeviceProperty.DeviceName;       break;
				case nameof(Device.FirmwareStr):		sortProperty = DeviceProperty.FirmwareStr;      break;
				case nameof(Device.ProductName):		sortProperty = DeviceProperty.ProductName;      break;
				default:								return false;
			}

			sortDirection = column.SortDirection != ListSortDirection.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending;
			column.SortDirection = sortDirection;

			collectionView.CustomSort = new DeviceComparer(sortProperty, sortDirection);
			return true;
		}

		private void Devices_SetDataGridFocus()
		{
			Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
				if (dgDevices.SelectedCells.Count != 0) {
					DataGridCellInfo cellInfo    = dgDevices.SelectedCells[0];
					FrameworkElement cellContent = cellInfo.Column.GetCellContent(cellInfo.Item);
					if (cellContent != null) {
						((DataGridCell)cellContent.Parent).Focus();
					}
				}
			}));
		}

		// Predicate functions

		private static bool IsDectClient						(Device device, string dectBaseID  ) => (device as DectDevice)?.DectBase   == dectBaseID;
		private static bool IsMeshClient						(Device device, string meshMasterID) => (device as JuisDevice)?.MeshMaster == meshMasterID;
		private static bool IsSelectedDevice					(Device device) => device.IsSelected;
		private static bool IsSelectedDeviceWithUpdateAvailable	(Device device) => device.IsSelected && device.UpdateAvailable;
		private static bool IsSelectedDeviceWithUpdateInfo		(Device device) => device.IsSelected && !string.IsNullOrWhiteSpace(device.UpdateInfo);
		private static bool IsSelectedDeviceWithUpdateImageURL	(Device device) => device.IsSelected && !string.IsNullOrWhiteSpace(device.UpdateImageURL);

		// Routed command: Devices_CmdAddDECT

		public static readonly RoutedCommand Devices_CmdAddDECT = new RoutedCommand();

		private void Devices_CmdAddDECT_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = true;
		}

		private void Devices_CmdAddDECT_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Settings settings = Settings.Default;

			DectDevice newDevice = new DectDevice() {
				Country  = settings.DefaultCountry,
				Language = settings.DefaultLanguage,
				OEM      = settings.DefaultOEM
			};

			if (newDevice.Edit(this)) {
				Devices.Add(newDevice);
				Devices_SelectDevice(newDevice);
			}

			Devices_SetDataGridFocus();
		}

		// Routed command: Devices_CmdAddJUIS

		public static readonly RoutedCommand Devices_CmdAddJUIS = new RoutedCommand();

		private void Devices_CmdAddJUIS_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = true;
		}

		private void Devices_CmdAddJUIS_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Settings settings = Settings.Default;

			JuisDevice newDevice = new JuisDevice() {
				Annex             = settings.DefaultAnnex,
				FirmwareBuildType = settings.DefaultFirmwareBuildType,
				Country           = settings.DefaultCountry,
				Language          = settings.DefaultLanguage,
				OEM               = settings.DefaultOEM
			};

			if (newDevice.Edit(this)) {
				Devices.Add(newDevice);
				Devices_SelectDevice(newDevice);
			}

			Devices_SetDataGridFocus();
		}

		// Routed command: Devices_CmdClearUpdates

		public static readonly RoutedCommand Devices_CmdClearUpdates = new RoutedCommand();

		private void Devices_CmdClearUpdates_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = Devices.Any(d => IsSelectedDeviceWithUpdateInfo(d));
		}

		private void Devices_CmdClearUpdates_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			foreach (Device device in Devices.Where(d => IsSelectedDeviceWithUpdateInfo(d))) {
				device.ClearUpdate();
			}

			Devices_SetDataGridFocus();
		}

		// Routed command: Devices_CmdCopy

		public static readonly RoutedCommand Devices_CmdCopy = new RoutedCommand();

		private void Devices_CmdCopy_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = Devices.Count(d => IsSelectedDevice(d)) == 1;
		}

		private void Devices_CmdCopy_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Device device = Devices.First(d => IsSelectedDevice(d));
			if (device is DectDevice dectDevice) {
				DectDevice newDevice = new DectDevice(dectDevice) { DeviceName = string.Empty };
				if (newDevice.Edit(this)) {
					Devices.Add(newDevice);
					Devices_SelectDevice(newDevice);
				}
			}
			if (device is JuisDevice juisDevice) {
				JuisDevice newDevice = new JuisDevice(juisDevice) { DeviceName = string.Empty };
				if (newDevice.Edit(this)) {
					Devices.Add(newDevice);
					Devices_SelectDevice(newDevice);
				}
			}

			Devices_SetDataGridFocus();
		}

		// Routed command: Devices_CmdCopyURLs

		public static readonly RoutedCommand Devices_CmdCopyURLs = new RoutedCommand();

		private void Devices_CmdCopyURLs_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = Devices.Any(d => IsSelectedDeviceWithUpdateImageURL(d));
		}

		private void Devices_CmdCopyURLs_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			string[] urls = Devices.Where(d => IsSelectedDeviceWithUpdateImageURL(d)).Select(d => d.UpdateImageURL).ToArray();
			if (!App.SafeClipboardSetText(string.Join("\r\n", urls) + "\r\n")) {
				ShowErrorMessage(JCstring.MessageTextClipboardCopyFailure.Unescape());
			}

			Devices_SetDataGridFocus();
		}

		// Routed command: Devices_CmdDelete

		public static readonly RoutedCommand Devices_CmdDelete = new RoutedCommand();

		private void Devices_CmdDelete_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = Devices.Any(d => IsSelectedDevice(d));
		}

		private void Devices_CmdDelete_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			int		count   = Devices.Count(d => IsSelectedDevice(d));
			string	message = count == 1 ? JCstring.MessageTextDeleteOneDevice : string.Format(CultureInfo.CurrentCulture, JCstring.MessageTextDeleteMultipleDevices, count);

			int result = MessageBoxEx2.Show(new MessageBoxEx2Params {
				CaptionText = JCstring.MessageCaptionDelete,
				MessageText = message,
				Image       = MessageBoxEx2Image.Question,
				ButtonText  = new string[] { JCstring.DialogButtonTextYes, JCstring.DialogButtonTextNo },
				Owner       = this
			});

			if (result == 0) {	// Yes button
				List<Device> deleteDevices = Devices.Where(d => IsSelectedDevice(d)).ToList();
				foreach (Device device in deleteDevices) {
					List<Device> meshClients = Devices.Where(d => IsMeshClient(d, device.ID)).ToList();
					foreach (Device meshClient in meshClients) {
						((JuisDevice)meshClient).MeshMaster = string.Empty;
					}

					List<Device> dectClients = Devices.Where(d => IsDectClient(d, device.ID)).ToList();
					foreach (Device dectClient in dectClients) {
						((DectDevice)dectClient).DectBase = string.Empty;
					}

					Devices.Remove(device);
				}
			}

			Devices_SetDataGridFocus();
		}

		// Routed command: Devices_CmdDownloadFirmware

		public static readonly RoutedCommand Devices_CmdDownloadFirmware = new RoutedCommand();

		private void Devices_CmdDownloadFirmware_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			if (Devices.Count(d => IsSelectedDevice(d)) == 1) {
				evt.CanExecute = !string.IsNullOrWhiteSpace(Devices.First(d => IsSelectedDevice(d)).UpdateImageURL);
			}
		}

		private void Devices_CmdDownloadFirmware_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Uri			downloadUri = new Uri(Devices.First(d => IsSelectedDevice(d)).UpdateImageURL);
			Settings	settings    = Settings.Default;

			SaveFileDialog sfd = new SaveFileDialog {
				AddExtension     = false,
				CheckFileExists  = false,
				CheckPathExists  = true,
				CreatePrompt     = false,
				Filter           = JCstring.FilterFilesAll,
				FileName         = downloadUri.Segments.Last(),
				InitialDirectory = Directory.Exists(settings.LastDownloadDirectory) ? settings.LastDownloadDirectory : App.GetDefaultDirectory(),
				OverwritePrompt  = true,
				Title            = JCstring.DialogCaptionSave,
				ValidateNames    = true
			};

			if (sfd.ShowDialog(this) != true) {
				return;
			}
			settings.LastDownloadDirectory = Path.GetDirectoryName(sfd.FileName);

			DownloadDialog downloadDialog = new DownloadDialog(downloadUri, sfd.FileName, JCstring.DialogCaptionDownloadFirmware) {
				Owner                 = this,
				WindowStartupLocation = WindowStartupLocation.CenterOwner
			};
			downloadDialog.ShowDialog();
			downloadDialog.Dispose();

			Devices_SetDataGridFocus();
		}

		// Routed command: Devices_CmdEdit

		public static readonly RoutedCommand Devices_CmdEdit = new RoutedCommand();

		private void Devices_CmdEdit_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = Devices.Count(d => IsSelectedDevice(d)) == 1;
		}

		private void Devices_CmdEdit_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Devices.First(d => IsSelectedDevice(d)).Edit(this);
			Devices_SetDataGridFocus();
		}

		// Routed command: Devices_CmdFindUpdates

		public static readonly RoutedCommand Devices_CmdFindUpdates = new RoutedCommand();

		private void Devices_CmdFindUpdates_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = Devices.Any(d => IsSelectedDevice(d));
		}

		private void Devices_CmdFindUpdates_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			List<Device>		devices     = new List<Device>(Devices_ViewSource.View.Cast<Device>());
			FindUpdatesDialog	findDialog  = new FindUpdatesDialog(devices.Where(d => IsSelectedDevice(d)).ToList()) {
				Owner                 = this,
				WindowStartupLocation = WindowStartupLocation.CenterOwner
			};

			findDialog.ShowDialog();
			findDialog.Dispose();

			Devices_SetDataGridFocus();
		}

		// Routed command: Devices_CmdMakeCurrent

		public static readonly RoutedCommand Devices_CmdMakeCurrent = new RoutedCommand();

		private void Devices_CmdMakeCurrent_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = Devices.Any(d => IsSelectedDeviceWithUpdateAvailable(d));
		}

		private void Devices_CmdMakeCurrent_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			foreach (Device device in Devices.Where(d => IsSelectedDeviceWithUpdateAvailable(d))) {
				try {
					device.MakeUpdateCurrent();
				}
				catch (FormatException) {
					ShowErrorMessage(string.Format(CultureInfo.CurrentCulture, JCstring.MessageTextInvalidUpdateVersion.Unescape(), device.DeviceName, device.UpdateInfo));
				}
			}

			Devices_SetDataGridFocus();
		}

		// Routed command: Devices_CmdSelectAll

		public static readonly RoutedCommand Devices_CmdSelectAll = new RoutedCommand();

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

		public static readonly RoutedCommand Devices_CmdSelectNone = new RoutedCommand();

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

		public static readonly RoutedCommand Devices_CmdViewInfo = new RoutedCommand();

		private void Devices_CmdViewInfo_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			if (Devices.Count(d => IsSelectedDevice(d)) == 1) {
				evt.CanExecute = !string.IsNullOrWhiteSpace(Devices.First(d => IsSelectedDevice(d)).UpdateInfoURL);
			}
		}

		private void Devices_CmdViewInfo_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Process.Start(Devices.First(d => IsSelectedDevice(d)).UpdateInfoURL);
			Devices_SetDataGridFocus();
		}

		// Event: Devices_DataGrid_MouseDoubleClick

		private void Devices_DataGrid_MouseDoubleClick_Handler( object sender, MouseButtonEventArgs evt )
		{
			if (ItemsControl.ContainerFromElement((DataGrid)sender, (DependencyObject)evt.OriginalSource) is DataGridRow row) {
				((Device)row.Item).Edit(this);
			}

			Devices_SetDataGridFocus();
		}

		// Event: Devices_DataGrid_Sorting

		private void Devices_DataGrid_Sorting_Handler( object sender, DataGridSortingEventArgs evt )
		{
			evt.Handled = Devices_SetComparer((ListCollectionView)Devices_ViewSource.View, evt.Column);
			Devices_SelectNone();
		}
	}
}
