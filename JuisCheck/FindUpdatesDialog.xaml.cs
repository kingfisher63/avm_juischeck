/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) 2018 Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Threading;

using JuisCheck.JUIS;
using JuisCheck.Lang;
using JuisCheck.Properties;

namespace JuisCheck
{
	/// <summary>
	/// Interaction logic for FindUpdatesDialog.xaml
	/// </summary>
	public sealed partial class FindUpdatesDialog : Window, IDisposable
	{
		private List<Device>		queryDevices;
		private BackgroundWorker	queryWorker;
		private bool				completed = false;
		private	bool				disposed  = false;

		public FindUpdatesDialog( List<Device> devices )
		{
			queryDevices = devices;
			queryWorker  = new BackgroundWorker() {
				WorkerReportsProgress      = true,
				WorkerSupportsCancellation = true
			};
			queryWorker.DoWork             += Searcher_DoWork_Handler;
			queryWorker.ProgressChanged    += Searcher_ProgressChanged_Handler;
			queryWorker.RunWorkerCompleted += Searcher_RunWorkerCompleted_Handler;

			InitializeComponent();

			pbProgress.Minimum = 0;
			pbProgress.Maximum = queryDevices.Count;
			pbProgress.Value   = 0;

			Closing += Closing_Handler;
			Loaded  += Loaded_Handler;
		}

		private void QueryDectSoftwareUpdate( Device device )
		{
			WebClient webClient = new WebClient();
			webClient.QueryString.Add("hw",      device.HardwareStr);
			webClient.QueryString.Add("sw",      device.FirmwareStr);
			webClient.QueryString.Add("oem",     device.OEM         );
			webClient.QueryString.Add("country", device.Country     );
			webClient.QueryString.Add("lang",    device.Language    );
			webClient.QueryString.Add("fw",      device.BaseFritzOS );

			try {
				string response = webClient.DownloadString(Settings.Default.AvmCatiServiceURL);

				Match match = Regex.Match(response, "URL=\"(.+)\"", RegexOptions.IgnoreCase);
				if (match.Success) {
					string		updateURL = match.Groups[1].ToString();
					string		fileName  = updateURL.Split('/').Last();
					string[]	parts     = fileName.Split('.');

					Dispatcher.Invoke(DispatcherPriority.Normal,
						new Action(() => {
							device.ClearUpdateInfo();
							device.UpdateAvailable   = true;
							device.UpdateInfo        = parts.Length >= 4 ? string.Format("{0}.{1}", parts[2], parts[3]) : JCstring.updateInfoUnknown;
 							device.UpdateImageURL    = updateURL;
							device.UpdateLastChecked = DateTime.Now;
						})
					);
				} else {
					Dispatcher.Invoke(DispatcherPriority.Normal,
						new Action(() => {
							device.ClearUpdateInfo();
							device.UpdateInfo        = JCstring.updateInfoNone;
							device.UpdateLastChecked = DateTime.Now;
						})
					);
				}
			}
			catch {
				Dispatcher.Invoke(DispatcherPriority.Normal,
					new Action(() => {
						device.ClearUpdateInfo();
						device.UpdateInfo        = JCstring.updateInfoError;
						device.UpdateLastChecked = DateTime.Now;
					})
				);
			}
		}

		private void QueryJuisSoftwareUpdate( Device device )
		{
			string timestamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

			RequestHeader requestHeader = new RequestHeader {
				ManualRequest = true,
				Nonce         = Convert.ToBase64String(Encoding.UTF8.GetBytes(timestamp)),
				UserAgent     = "Box"
			};

			BoxInfo boxInfo = new BoxInfo {
				Name         = device.ProductName,																		// Never empty
				HW           = device.HardwareMajor,
				Major        = device.FirmwareMajor,
				Minor        = device.FirmwareMinor,
				Patch        = device.FirmwarePatch,
				Buildnumber  = device.FirmwareBuildNumber,
				Buildtype    = device.FirmwareBuildType,
				Serial       = string.IsNullOrWhiteSpace(device.SerialNumber) ? "9CC7A6123456" : device.SerialNumber,	// Must not be empty (use fake AVM MAC address if needed)
				OEM          = device.OEM,																				// Never empty
				Lang         = device.Language,																			// Never empty
				Country      = device.Country,																			// Never empty
				Annex        = device.Annex,																			// Never empty
				Flag         = device.Flag.Length == 0 ? new string[] { string.Empty } : device.Flag,					// Need at least one flag (empty flag OK)
				UpdateConfig = 1,
				Provider     = string.Empty,
				ProviderName = string.Empty
			};

			try {
				UpdateInfoServiceClient client = new UpdateInfoServiceClient(new BasicHttpBinding(), new EndpointAddress(Settings.Default.AvmJuisServiceURL));

				UpdateInfo updateInfo = client.BoxFirmwareUpdateCheck(requestHeader, boxInfo, null).UpdateInfo;
				if (updateInfo.Found) {
					Dispatcher.Invoke(DispatcherPriority.Normal,
						new Action(() => {
							device.ClearUpdateInfo();
							device.UpdateAvailable   = true;
							device.UpdateInfo        = updateInfo.Version;
							device.UpdateImageURL    = updateInfo.DownloadURL;
							device.UpdateInfoURL     = updateInfo.InfoURL;
							device.UpdateLastChecked = DateTime.Now;
						})
					);
				} else {
					Dispatcher.Invoke(DispatcherPriority.Normal,
						new Action(() => {
							device.ClearUpdateInfo();
							device.UpdateInfo        = JCstring.updateInfoNone;
							device.UpdateLastChecked = DateTime.Now;
						})
					);
				}
			}
			catch {
				Dispatcher.Invoke(DispatcherPriority.Normal,
					new Action(() => {
						device.ClearUpdateInfo();
						device.UpdateInfo        = JCstring.updateInfoError;
						device.UpdateLastChecked = DateTime.Now;
					})
				);
			}
		}

		// Event: Closing

		private void Closing_Handler( object sender, CancelEventArgs evt )
		{
			if (!completed) {
				queryWorker.CancelAsync();
				IsEnabled  = false;
				evt.Cancel = true;
			}
		}

		// Event: Loaded

		private void Loaded_Handler( object sender, RoutedEventArgs evt )
		{
			Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => queryWorker.RunWorkerAsync()));
		}

		// Event: Searcher_DoWork

		private void Searcher_DoWork_Handler( object sender, DoWorkEventArgs evt )
		{
			BackgroundWorker	bw       = sender as BackgroundWorker;
			int					progress = 0;

			foreach (Device device in queryDevices) {
				switch (device.DeviceKind) {
					case DeviceKind.DECT:
						QueryDectSoftwareUpdate(device);
						break;

					case DeviceKind.JUIS:
						QueryJuisSoftwareUpdate(device);
						break;

					default:
						throw new InvalidOperationException("Unsupported device kind");
				}

				bw.ReportProgress(++progress);
				if (bw.CancellationPending) {
					break;
				}
			}
		}

		// Event: Searcher_ProgressChanged

		private void Searcher_ProgressChanged_Handler( object sender, ProgressChangedEventArgs evt )
		{
			pbProgress.Value = evt.ProgressPercentage;
		}

		// Event: Searcher_RunWorkerCompleted

		private void Searcher_RunWorkerCompleted_Handler( object sender, RunWorkerCompletedEventArgs evt )
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
