/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using Muon.DotNetExtensions;
using Muon.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Threading;

using JuisCheck.Lang;

namespace JuisCheck
{
	/// <summary>
	/// Interaction logic for FindUpdatesDialog.xaml
	/// </summary>
	public sealed partial class FindUpdatesDialog : Window, IDisposable
	{
		private readonly List<Device>		queryDevices;
		private readonly BackgroundWorker	queryWorker;
		private bool						completed = false;
		private	bool						disposed  = false;

		public FindUpdatesDialog( List<Device> devices )
		{
			queryDevices = devices ?? throw new ArgumentNullException(nameof(devices));
			queryWorker  = new BackgroundWorker() {
				WorkerReportsProgress      = true,
				WorkerSupportsCancellation = true
			};
			queryWorker.DoWork             += Worker_DoWork_Handler;
			queryWorker.ProgressChanged    += Worker_ProgressChanged_Handler;
			queryWorker.RunWorkerCompleted += Worker_RunWorkerCompleted_Handler;

			InitializeComponent();
			DataContext = this;

			pbProgress.Minimum = 0;
			pbProgress.Maximum = queryDevices.Count;
			pbProgress.Value   = 0;
		}

		// Event handler: Closing

		private void Closing_Handler( object sender, CancelEventArgs evt )
		{
			if (!completed) {
				queryWorker.CancelAsync();
				IsEnabled  = false;
				evt.Cancel = true;
			}
		}

		// Event handler: Loaded

		private void Loaded_Handler( object sender, RoutedEventArgs evt )
		{
			queryWorker.RunWorkerAsync();
		}

		// Event handler: Worker_DoWork

		private void Worker_DoWork_Handler( object sender, DoWorkEventArgs evt )
		{
			BackgroundWorker	worker   = (BackgroundWorker)sender;
			int					progress = 0;

			foreach (Device device in queryDevices) {
				if (device is DectDevice dectDevice) {
					JuisDevice dectBase = null;
					Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => {
						dectBase = App.GetMainWindow().Devices.FindByID(dectDevice.DectBase) as JuisDevice;
					}));

					string message = null;
					if (string.IsNullOrWhiteSpace(dectDevice.DectBase)) {
						message = string.Format(CultureInfo.CurrentCulture, JCstring.MessageTextDectBaseNotSet, dectDevice.DeviceName);
					} else
					if (dectBase == null) {
						message = string.Format(CultureInfo.CurrentCulture, JCstring.MessageTextDectBaseNotFound, dectDevice.DeviceName);
					}

					if (message != null) {
						int result = -1;
						Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => {
							result = MessageBoxEx2.Show(new MessageBoxEx2Params {
								CaptionText = JCstring.MessageCaptionError,
								MessageText = message.Unescape(),
								Image       = MessageBoxEx2Image.Error,
								ButtonText  = new string[] { JCstring.DialogButtonTextSkip, JCstring.DialogButtonTextCancel },
								Owner       = this
							});
						}));

						if (result == 0) {	// Skip button
							continue;
						} else {			// Cancel button, close box
							break;
						}
					}

					string queryUpdateResponse = dectDevice.QueryFirmwareUpdate(dectBase);

					Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => {
						dectDevice.SetFirmwareUpdate(queryUpdateResponse);
					}));
				}

				if (device is JuisDevice juisDevice) {
					JuisDevice meshMaster = null;
					Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => {
						meshMaster = App.GetMainWindow().Devices.FindByID(juisDevice.MeshMaster) as JuisDevice;
					}));

					try {
						JUIS.UpdateInfo queryUpdateResponse = juisDevice.QueryFirmwareUpdate(meshMaster);
						Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => {
							juisDevice.SetFirmwareUpdate(queryUpdateResponse);
						}));
					}
					catch {
						Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => {
							juisDevice.SetFirmwareUpdate(null);
						}));
					}
				}

				worker.ReportProgress(++progress);
				if (worker.CancellationPending) {
					break;
				}
			}
		}

		// Event handler: Worker_ProgressChanged

		private void Worker_ProgressChanged_Handler( object sender, ProgressChangedEventArgs evt )
		{
			pbProgress.Value = evt.ProgressPercentage;
		}

		// Event handler: Worker_RunWorkerCompleted

		private void Worker_RunWorkerCompleted_Handler( object sender, RunWorkerCompletedEventArgs evt )
		{
			completed = true;
			Close();
		}

		// Interface: IDisposable

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose( bool disposing )
		{
			if (!disposed) {
				if (disposing) {
					queryWorker.Dispose();
				}
				disposed = true;
			}
		}
	}
}
