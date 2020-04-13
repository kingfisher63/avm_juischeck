/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using Fluent;
using Muon.DotNetExtensions;
using Muon.Windows;
using Muon.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using JuisCheck.Lang;
using JuisCheck.Properties;

namespace JuisCheck
{
	public sealed partial class MainWindow : RibbonWindow
	{
		private static readonly Dictionary<string, string> languageDictionary = new Dictionary<string, string>() {
			{ "de", JCstring.ComboBoxValueLanguageDE },
			{ "en", JCstring.ComboBoxValueLanguageEN },
			{ "es", JCstring.ComboBoxValueLanguageES },
			{ "fr", JCstring.ComboBoxValueLanguageFR },
			{ "it", JCstring.ComboBoxValueLanguageIT },
			{ "nl", JCstring.ComboBoxValueLanguageNL },
			{ "pl", JCstring.ComboBoxValueLanguagePL }
		};

		public List<ComboBoxValue>	Backstage_DefaultAnnexValues		{ get; private set; }
		public List<ComboBoxValue>	Backstage_DefaultBuildTypeValues	{ get; private set; }
		public List<ComboBoxValue>	Backstage_DefaultCountryValues		{ get; private set; }
		public List<ComboBoxValue>	Backstage_DefaultLanguageValues		{ get; private set; }
		public List<ComboBoxValue>	Backstage_DefaultOEMValues			{ get; private set; }
		public List<ComboBoxValue>	Backstage_UILanguageValues 			{ get; private set; }

		private void Backstage_Init1()
		{
			Backstage_DefaultAnnexValues     = JuisDevice.GetAnnexValues().Prepend(string.Empty, JCstring.ComboBoxValueNotSet);
			Backstage_DefaultBuildTypeValues = JuisDevice.GetFirmwareBuildTypeValues().Prepend("-1", JCstring.ComboBoxValueNotSet);
			Backstage_DefaultCountryValues   = Device.GetCountryValues().Prepend(string.Empty, JCstring.ComboBoxValueNotSet);
			Backstage_DefaultLanguageValues  = Device.GetLanguageValues().Prepend(string.Empty, JCstring.ComboBoxValueNotSet);
			Backstage_DefaultOEMValues       = Device.GetOemValues().Prepend(string.Empty, JCstring.ComboBoxValueNotSet);

			Backstage_UILanguageValues = new List<ComboBoxValue>();
			foreach (string language in App.AdditionalLanguages) {
				if (languageDictionary.TryGetValue(language, out string languageFull)) {
					Backstage_UILanguageValues.Add(new ComboBoxValue(language, languageFull));
				}
			}
			Backstage_UILanguageValues.Add(new ComboBoxValue("en", JCstring.ComboBoxValueLanguageEN)); // Main language
			Backstage_UILanguageValues.Prepend("auto", JCstring.ComboBoxValueAutomatic);
			Backstage_UILanguageValues.Sort(1, Backstage_UILanguageValues.Count-1, new ComboBoxValueComparer(App.defaultDisplayStringComparison));
		}

		private void Backstage_Init2()
		{
		}

		private void Backstage_CleanRecentFiles()
		{
			foreach (UIElement element in dpRecentFiles.Children) {
				if (element is RecentFileButton button) {
					if (!button.IsEnabled) {
						recentFiles.Remove((string)button.Tag);
					}
				}
			}
		}

		private void Backstage_PopulateRecentFiles()
		{
			dpRecentFiles.Children.Clear();
			foreach (string recentFile in recentFiles.GetMaxItems(programSettings.RecentFilesMax)) {
				try {
					RecentFileButton button = new RecentFileButton {
						DirPath   = Path.GetDirectoryName(recentFile),
						FileName  = Path.GetFileName(recentFile),
						IsEnabled = File.Exists(recentFile),
						Tag       = recentFile,
						ToolTip   = recentFile
					};
					DockPanel.SetDock(button, Dock.Top);

					dpRecentFiles.Children.Add(button);
				}
				catch (ArgumentException) {
					recentFiles.Remove(recentFile);
				}
				catch (PathTooLongException) {
					recentFiles.Remove(recentFile);
				}
			}
		}

		// Command: Backstage_CmdAbout

		public static readonly RoutedCommand Backstage_CmdAbout = new RoutedCommand();

		private void Backstage_CmdAbout_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = true;
		}

		private void Backstage_CmdAbout_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
				MessageBoxEx2.Show(new MessageBoxEx2Params {
					CaptionText = JCstring.MessageCaptionAbout,
					MessageText = $"{App.GetProgramInfo()}\n\n{App.GetCopyright()}",
					Image       = MessageBoxEx2Image.Information,
					ButtonText  = new string[] { JCstring.DialogButtonTextOk },
					Owner       = this
				});
			}));
		}

		// Command: Backstage_CmdClose

		public static readonly RoutedCommand Backstage_CmdClose = new RoutedCommand();

		private void Backstage_CmdClose_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = true;
		}

		private void Backstage_CmdClose_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
				CloseDeviceCollection();
			}));
		}

		// Command: Backstage_CmdExit

		public static readonly RoutedCommand Backstage_CmdExit = new RoutedCommand();

		private void Backstage_CmdExit_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = true;
		}

		private void Backstage_CmdExit_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
				Close();
			}));
		}

		// Command: Backstage_CmdOpen

		public static readonly RoutedCommand Backstage_CmdOpen = new RoutedCommand();

		private void Backstage_CmdOpen_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = true;
		}

		private void Backstage_CmdOpen_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
				OpenDeviceCollection();
			}));
		}

		// Command: Backstage_CmdRecentFilesClean

		public static readonly RoutedCommand Backstage_CmdRecentFilesClean = new RoutedCommand();

		private void Backstage_CmdRecentFilesClean_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			if (dpRecentFiles != null) {
				foreach (UIElement element in dpRecentFiles.Children) {
					if (!element.IsEnabled) {
						evt.CanExecute = true;
						return;
					}
				}
			}

			evt.CanExecute = false;
		}

		private void Backstage_CmdRecentFilesClean_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Backstage_CleanRecentFiles();
			Backstage_PopulateRecentFiles();
		}

		// Command: Backstage_CmdRecentFilesClear

		public static readonly RoutedCommand Backstage_CmdRecentFilesClear = new RoutedCommand();

		private void Backstage_CmdRecentFilesClear_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			if (dpRecentFiles != null) {
				evt.CanExecute = dpRecentFiles.Children.Count != 0;
				return;
			}

			evt.CanExecute = false;
		}

		private void Backstage_CmdRecentFilesClear_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			recentFiles.Clear();
			Backstage_PopulateRecentFiles();
		}

		// Command: Backstage_CmdRecentFileOpen

		public static readonly RoutedCommand Backstage_CmdRecentFileOpen = new RoutedCommand();

		private void Backstage_CmdRecentFileOpen_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = true;
		}

		private void Backstage_CmdRecentFileOpen_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			PopupService.RaiseDismissPopupEvent(evt.Source, DismissPopupMode.Always);
			if (evt.Source is RecentFileButton button) {
				Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
					OpenDeviceCollection((string)button.Tag);
				}));
			}
		}

		// Command: Backstage_CmdRecentFileRemove

		public static readonly RoutedCommand Backstage_CmdRecentFileRemove = new RoutedCommand();

		private void Backstage_CmdRecentFileRemove_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = true;
		}

		private void Backstage_CmdRecentFileRemove_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			if (evt.Source is RecentFileButton button) {
				recentFiles.Remove((string)button.Tag);
				Backstage_PopulateRecentFiles();
			}
		}

		// Command: Backstage_CmdSave

		public static readonly RoutedCommand Backstage_CmdSave = new RoutedCommand();

		private void Backstage_CmdSave_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = true;
		}

		private void Backstage_CmdSave_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
				SaveDeviceCollection();
			}));
		}

		// Command: Backstage_CmdSaveAs

		public static readonly RoutedCommand Backstage_CmdSaveAs = new RoutedCommand();

		private void Backstage_CmdSaveAs_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = true;
		}

		private void Backstage_CmdSaveAs_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
				SaveDeviceCollectionAs();
			}));
		}

		// Command: Backstage_CmdSettingsDefault

		public static readonly RoutedCommand Backstage_CmdSettingsDefault = new RoutedCommand();

		private void Backstage_CmdSettingsDefault_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = true;
		}

		private void Backstage_CmdSettingsDefault_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			programSettings.Reset();
		}

		// Event: Backstage_IsOpenChanged

		#pragma warning disable CA1801 // Code analysis does not recognize this method as an event hander...
		private void Backstage_IsOpenChanged_Handler( object sender, DependencyPropertyChangedEventArgs evt )
		{
			if ((bool)evt.NewValue) {
				backstageTabRecentFiles.IsSelected = true;
				Backstage_PopulateRecentFiles();
			} else {
				SetDataGridFocus();
			}
		}
		#pragma warning restore CA1801

		// Event: RecentFile_ContextMenuOpenClick

		private void Backstage_RecentFile_ContextMenuOpenClick_Handler( object sender, RoutedEventArgs evt )
		{
			if (ContextMenuHelper.GetPlacementTarget(evt.Source) is RecentFileButton recentFileButton) {
				Backstage_CmdRecentFileOpen.Execute(null, recentFileButton);
			}
		}

		// Event: RecentFile_ContextMenuRemoveClick

		private void Backstage_RecentFile_ContextMenuRemoveClick_Handler( object sender, RoutedEventArgs evt )
		{
			if (ContextMenuHelper.GetPlacementTarget(evt.Source) is RecentFileButton recentFileButton) {
				Backstage_CmdRecentFileRemove.Execute(null, recentFileButton);
			}
		}
	}
}
