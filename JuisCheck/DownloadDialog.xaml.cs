/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Threading;

using JuisCheck.Lang;

namespace JuisCheck
{
	/// <summary>
	/// Interaction logic for DownloadDialog.xaml
	/// </summary>
	public sealed partial class DownloadDialog : Window, IDisposable
	{
		private readonly WebClient	webClient;
		private readonly Uri		downloadUri;
		private readonly string		downloadFilePath;

		private bool				completed = false;
		private	bool				disposed  = false;

		public DownloadDialog( Uri uri, string filePath, string windowTitle )
		{
			downloadUri      = uri;
			downloadFilePath = filePath;

			InitializeComponent();
			DataContext = this;
			Title       = windowTitle;

			webClient = new WebClient();
			webClient.DownloadFileCompleted   += WebClient_DownloadFileCompleted_Handler;
			webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged_Handler;
		}

		// Event handler: Closing

		private void Closing_Handler( object sender, CancelEventArgs evt )
		{
			if (!completed) {
				webClient.CancelAsync();
				IsEnabled  = false;
				evt.Cancel = true;
			}
		}

		// Event handler: Loaded

		private void Loaded_Handler( object sender, RoutedEventArgs evt )
		{
			if (string.Compare(downloadUri.Scheme, "http",  StringComparison.OrdinalIgnoreCase) == 0 ||
				string.Compare(downloadUri.Scheme, "https", StringComparison.OrdinalIgnoreCase) == 0)
			{
				webClient.Headers.Add("User-Agent", $"JuisCheck/{App.GetVersion()} (MS Windows)");
				webClient.Headers.Add("Accept", "*/*");
			}

			webClient.DownloadFileAsync(downloadUri, downloadFilePath);
		}

		// Event handler: WebClient_DownloadFileCompleted

		void WebClient_DownloadFileCompleted_Handler( object sender, AsyncCompletedEventArgs evt )
		{
			if (evt.Cancelled) {
				try {
					File.Delete(downloadFilePath);
				} catch {
				}
			} else {
				if (evt.Error is Exception ex) {
					Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => {
						MessageBoxEx.Show(new MessageBoxExParams {
							CaptionText = JCstring.MessageCaptionError,
							MessageText = ex.Message,
							Image       = MessageBoxExImage.Error,
							ButtonText  = new string[] { JCstring.DialogButtonTextOk },
							Owner       = this
						});
					}));
				}
			}

			completed = true;
			Close();
		}

		// Event handler: WebClient_DownloadProgressChanged

		private void WebClient_DownloadProgressChanged_Handler( object sender, DownloadProgressChangedEventArgs evt )
		{
			if (evt.TotalBytesToReceive == 0) {
				pbProgress.IsIndeterminate = true;
			} else {
				pbProgress.Value = evt.BytesReceived * pbProgress.Maximum / evt.TotalBytesToReceive;
			}
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
					webClient.Dispose();
				}
				disposed = true;
			}
		}
	}
}
