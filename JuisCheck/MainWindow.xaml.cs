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
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WinForms = System.Windows.Forms;

using JuisCheck.Lang;
using JuisCheck.Properties;

namespace JuisCheck
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public sealed partial class MainWindow : RibbonWindow
	{
		public MainWindow()
		{
			Backstage_Init1();
			Devices_Init1();

			InitializeComponent();
			DataContext = this;

			Backstage_Init2();
			Devices_Init2();

			RestoreWindowMetrics();
			SetWindowTitle();

			Settings.Default.PropertyChanged  += Settings_PropertyChanged_Handler;
			Devices.CollectionPropertyChanged += Devices_CollectionPropertyChanged_Handler;
		}

		private void CloseDeviceCollection()
		{
			if (SaveUnsavedData()) {
				Devices.Settings.Reset();
				Devices.Empty();
				Devices_InitDataGrid();
			}
		}

		private static bool IsInfinityOrNaN( double number )
		{
			return double.IsInfinity(number) || double.IsNaN(number);
		}

		private void OpenDeviceCollection( string fileName = null )
		{
			Settings settings = Settings.Default;

			if (fileName != null && string.Compare(fileName, Devices.FileName, App.defaultFileNameComparison) == 0) {
				return;
			}

			if (!SaveUnsavedData()) {
				return;
			}

			if (fileName == null) {
				OpenFileDialog ofd = new OpenFileDialog() {
					AddExtension     = false,
					CheckFileExists  = true,
					CheckPathExists  = true,
					Filter           = JCstring.FilterFilesXML,
					InitialDirectory = Directory.Exists(settings.LastDocumentDirectory) ? settings.LastDocumentDirectory : App.GetDefaultDirectory(),
					ReadOnlyChecked  = false,
					ShowReadOnly     = false,
					Title            = JCstring.DialogCaptionOpen,
					ValidateNames    = true
				};

				if (ofd.ShowDialog(this) != true ) {
					return;
				}
				settings.LastDocumentDirectory = Path.GetDirectoryName(ofd.FileName);

				fileName = ofd.FileName;
			}

			try {
				Devices_InitDataGrid();
				Devices.Load(fileName);
				RecentFiles.Add(fileName);
			}
			catch (Exception ex) {
				ShowErrorMessage(string.Format(CultureInfo.CurrentCulture, JCstring.MessageTextOpenDeviceCollectionFailed.Unescape(), ex.Message));
			}
		}

		private void RestoreWindowMetrics()
		{
			Settings settings = Settings.Default;

			if (settings.MainWindowRestoreMetrics) {
				Rectangle restoreBounds = new Rectangle(int.MinValue, int.MinValue, int.MinValue, int.MinValue);

				if (!IsInfinityOrNaN(settings.MainWindowLeft  )) { restoreBounds.X      = (int)settings.MainWindowLeft;   }
				if (!IsInfinityOrNaN(settings.MainWindowTop   )) { restoreBounds.Y      = (int)settings.MainWindowTop;    }
				if (!IsInfinityOrNaN(settings.MainWindowWidth )) { restoreBounds.Width  = (int)settings.MainWindowWidth;  }
				if (!IsInfinityOrNaN(settings.MainWindowHeight)) { restoreBounds.Height = (int)settings.MainWindowHeight; }

				// Window position: prevent window from being positioned off screen

				bool intersectsWithScreen = false;
				foreach (WinForms.Screen screen in WinForms.Screen.AllScreens) {
					if (screen.WorkingArea.IntersectsWith(restoreBounds)) {
						intersectsWithScreen = true;
					}
				}

				if (intersectsWithScreen) {
					if (restoreBounds.Left != int.MinValue) { Left = restoreBounds.Left; }
					if (restoreBounds.Top  != int.MinValue) { Top  = restoreBounds.Top;  }
				}

				// Window size

				if (restoreBounds.Width  != int.MinValue) { Width  = restoreBounds.Width;  }
				if (restoreBounds.Height != int.MinValue) { Height = restoreBounds.Height; }

				// Window state

				WindowState = settings.MainWindowMaximized ? WindowState.Maximized : WindowState.Normal;
			}
		}

		private bool SaveDeviceCollection()
		{
			try {
				Devices.Save();
				return true;
			}
			catch {
				return SaveDeviceCollectionAs();
			}
		}

		private bool SaveDeviceCollectionAs()
		{
			Settings settings = Settings.Default;

			SaveFileDialog sfd = new SaveFileDialog {
				AddExtension    = true,
				CheckFileExists = false,
				CheckPathExists = true,
				CreatePrompt    = false,
				DefaultExt      = "xml",
				Filter          = JCstring.FilterFilesXML,
				OverwritePrompt = true,
				Title           = JCstring.DialogCaptionSave,
				ValidateNames   = true
			};

			if (Devices.FileName == null) {
				sfd.InitialDirectory = Directory.Exists(settings.LastDocumentDirectory) ? settings.LastDocumentDirectory : App.GetDefaultDirectory();
				sfd.FileName         = string.Empty;
			} else {
				sfd.InitialDirectory = Path.GetDirectoryName(Devices.FileName);
				sfd.FileName         = Path.GetFileName(Devices.FileName);
			}

			if (sfd.ShowDialog(this) != true) {
				return false;
			}
			settings.LastDocumentDirectory = Path.GetDirectoryName(sfd.FileName);

			try {
				Devices.Save(sfd.FileName);
				RecentFiles.Add(sfd.FileName);
				return true;
			}
			catch (Exception ex) {
				ShowErrorMessage(string.Format(CultureInfo.CurrentCulture, JCstring.MessageTextSaveDeviceCollectionFailed.Unescape(), ex.Message));
				return false;
			}
		}

		private void SaveWindowMetrics()
		{
			Settings settings = Settings.Default;

			if (!IsInfinityOrNaN(RestoreBounds.Left  )) { settings.MainWindowLeft   = RestoreBounds.Left;   }
			if (!IsInfinityOrNaN(RestoreBounds.Top   )) { settings.MainWindowTop    = RestoreBounds.Top;    }
			if (!IsInfinityOrNaN(RestoreBounds.Width )) { settings.MainWindowWidth  = RestoreBounds.Width;  }
			if (!IsInfinityOrNaN(RestoreBounds.Height)) { settings.MainWindowHeight = RestoreBounds.Height; }
			settings.MainWindowMaximized = WindowState == WindowState.Maximized;
		}

		private bool SaveUnsavedData()
		{
			if (!Devices.IsModified) {
				return true;
			}

			while (true) {
				int result = MessageBoxEx2.Show(new MessageBoxEx2Params {
					CaptionText = JCstring.MessageCaptionUnsavedData,
					MessageText = JCstring.MessageTextUnsavedData.Unescape(),
					Image       = MessageBoxEx2Image.Warning,
					ButtonText  = new string[] { JCstring.DialogButtonTextYes, JCstring.DialogButtonTextNo, JCstring.DialogButtonTextCancel },
					Owner       = this
				});

				switch (result) {
					case 0:		// Yes button
						if (SaveDeviceCollection()) {
							return true;
						}
						break;

					case 1:		// No button
						return true;

					default:	// Cancel button, close box
						return false;
				}
			}
		}

		private void SetDataGridFocus()
		{
			Devices_SetDataGridFocus();
		}

		private void SetWindowTitle()
		{
			string modifiedInfo = Devices.IsModified ? "*" : string.Empty;
			string fileInfo     = Devices.FileName ?? JCstring.FileNameNew;

			Title = $"{modifiedInfo+fileInfo} - {App.GetProgramInfo()}";
		}

		private void ShowErrorMessage( string message )
		{
			MessageBoxEx2.Show(new MessageBoxEx2Params {
				CaptionText = JCstring.MessageCaptionError,
				MessageText = message,
				Image       = MessageBoxEx2Image.Error,
				ButtonText  = new string[] { JCstring.DialogButtonTextOk },
				Owner       = this
			});
		}

		// Event handler: Closing

		private void Closing_Handler( object sender, CancelEventArgs evt )
		{
			Settings settings = Settings.Default;

			if (!SaveUnsavedData()) {
				App.CancelRestart();
				evt.Cancel = true;
				return;
			}

			if (settings.AutoLoad || App.RestartPending) {
				settings.AutoLoadFile = !string.IsNullOrWhiteSpace(Devices.FileName) ? Devices.FileName : string.Empty;
			} else {
				settings.AutoLoadFile = string.Empty;
			}

			SaveWindowMetrics();

			evt.Cancel = false;
		}

		// Event handler: Loaded

		private void Loaded_Handler( object sender, RoutedEventArgs evt )
		{
			Settings settings = Settings.Default;

			if (File.Exists(settings.AutoLoadFile)) {
				OpenDeviceCollection(settings.AutoLoadFile);
			}
		}

		// Event handler: PreviewKeyDown
		//
		// We handle the key input bindings ourself because some keypresses are consumed
		// by controls deep down in the tree.

		private void PreviewKeyDown_Handler( object sender, KeyEventArgs evt )
		{
			foreach (InputBinding inputBinding in InputBindings) {
				if (inputBinding is KeyBinding keyBinding) {
					if (keyBinding.Key == evt.Key && keyBinding.Modifiers == evt.KeyboardDevice.Modifiers) {
						keyBinding.Command.Execute(null);
						evt.Handled = true;
						return;
					}
				}
			}
		}

		// Event handler: Devices_CollectionPropertyChanged

		private void Devices_CollectionPropertyChanged_Handler( object sender, PropertyChangedEventArgs evt )
		{
			switch (evt.PropertyName) {
				case nameof(DeviceCollection.FileName):
				case nameof(DeviceCollection.IsModified):
					SetWindowTitle();
					break;
			}
		}

		// Event handler: Settings_PropertyChanged

		private void Settings_PropertyChanged_Handler( object sender, PropertyChangedEventArgs evt )
		{
			Settings settings = Settings.Default;

			switch (evt.PropertyName) {
				case nameof(settings.JuisReleaseShowBuildNumber):
					Devices_RefreshView();
					break;

				case nameof(settings.RecentFilesMax):
					Backstage_PopulateRecentFiles();
					break;

				case nameof(settings.UserInterfaceLanguage):
					// Display message _after_ the setting has been updated on screen regardless of event handler order.
					Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
						int result = MessageBoxEx2.Show(new MessageBoxEx2Params() {
							MessageText = JCstring.MessageTextRestartLanguageChange.Unescape(),
							CaptionText = JCstring.MessageCaptionProgramRestartRequired,
							Image       = MessageBoxEx2Image.Warning,
							ButtonText  = new string[] { JCstring.DialogButtonTextYes, JCstring.DialogButtonTextNo },
							Owner       = this
						});
					
						if (result == 0) {	// Yes button
							App.Restart();
						}
					}));
					break;
			}
		}
	}
}
