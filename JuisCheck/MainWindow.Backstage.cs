/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) 2018 Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using Fluent;
using Muon.DotNetExtensions;
using Muon.Windows;
using Muon.Windows.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using WinControls = System.Windows.Controls;

using JuisCheck.Lang;

namespace JuisCheck
{
	public sealed partial class MainWindow : RibbonWindow
	{
		public List<ComboBoxValue>	Backstage_AnnexValues     => Device.GetAnnexValues().PrependNotSet();
		public List<ComboBoxValue>	Backstage_BuildTypeValues => Device.GetFirmwareBuildTypeValues().PrependNotSet("-1");
		public List<ComboBoxValue>	Backstage_CountryValues   => Device.GetCountryValues().PrependNotSet();
		public List<ComboBoxValue>	Backstage_LanguageValues  => Device.GetLanguageValues().PrependNotSet();
		public List<ComboBoxValue>	Backstage_OEMValues       => Device.GetOemValues().PrependNotSet();

		private void Backstage_Init()
		{
		}

		private void Backstage_CleanRecentFiles()
		{
			foreach (UIElement element in dpRecentFiles.Children) {
				if (element is RecentFileButton button) {
					if (!button.IsEnabled) {
						RecentFiles.Remove(button.Tag as string);
					}
				}
			}
		}

		private void Backstage_PopulateRecentFiles()
		{
			List<string> paths = RecentFiles.GetFileNames();

			dpRecentFiles.Children.Clear();
			foreach (string path in paths) {
				try {
					RecentFileButton button = new RecentFileButton {
						DirPath   = Path.GetDirectoryName(path),
						FileName  = Path.GetFileName(path),
						IsEnabled = File.Exists(path),
						Tag       = path,
						ToolTip   = path
					};
					DockPanel.SetDock(button, Dock.Top);

					dpRecentFiles.Children.Add(button);
				}
				catch {
				}
			}
		}

		// Command: Backstage_CmdAbout

		public static RoutedCommand Backstage_CmdAbout = new RoutedCommand();

		private void Backstage_CmdAbout_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = true;
		}

		private void Backstage_CmdAbout_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Dispatcher.BeginInvoke(DispatcherPriority.Background,
				new Action(() => {
					MessageBoxEx.Show(
						new MessageBoxExParams {
							CaptionText = JCstring.MessageCaptionAbout,
							MessageText = JCstring.MessageTextAbout.Unescape(),
							Image       = MessageBoxExImage.Information,
							Button      = MessageBoxExButton.OK,
							Owner       = this
						}
					);
				})
			);
		}

		// Command: Backstage_CmdClose

		public static RoutedCommand Backstage_CmdClose = new RoutedCommand();

		private void Backstage_CmdClose_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = true;
		}

		private void Backstage_CmdClose_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => CloseDeviceCollection()));
		}

		// Command: Backstage_CmdExit

		public static RoutedCommand Backstage_CmdExit = new RoutedCommand();

		private void Backstage_CmdExit_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = true;
		}

		private void Backstage_CmdExit_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Close()));
		}

		// Command: Backstage_CmdOpen

		public static RoutedCommand Backstage_CmdOpen = new RoutedCommand();

		private void Backstage_CmdOpen_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = true;
		}

		private void Backstage_CmdOpen_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => OpenDeviceCollection()));
		}

		// Command: Backstage_CmdRecentFilesClean

		public static RoutedCommand Backstage_CmdRecentFilesClean = new RoutedCommand();

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

		public static RoutedCommand Backstage_CmdRecentFilesClear = new RoutedCommand();

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
			RecentFiles.Clear();
			Backstage_PopulateRecentFiles();
		}

		// Command: Backstage_CmdRecentFileOpen

		public static RoutedCommand Backstage_CmdRecentFileOpen = new RoutedCommand();

		private void Backstage_CmdRecentFileOpen_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = true;
		}

		private void Backstage_CmdRecentFileOpen_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			PopupService.RaiseDismissPopupEvent(evt.Source, DismissPopupMode.Always);
			if (evt.Source is RecentFileButton button) {
				Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => OpenDeviceCollection(button.Tag as string)));
			}
		}

		// Command: Backstage_CmdRecentFileRemove

		public static RoutedCommand Backstage_CmdRecentFileRemove = new RoutedCommand();

		private void Backstage_CmdRecentFileRemove_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = true;
		}

		private void Backstage_CmdRecentFileRemove_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			if (evt.Source is RecentFileButton button) {
				RecentFiles.Remove(button.Tag as string);
				Backstage_PopulateRecentFiles();
			}
		}

		// Command: Backstage_CmdSave

		public static RoutedCommand Backstage_CmdSave = new RoutedCommand();

		private void Backstage_CmdSave_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = true;
		}

		private void Backstage_CmdSave_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => SaveDeviceCollection()));
		}

		// Command: Backstage_CmdSaveAs

		public static RoutedCommand Backstage_CmdSaveAs = new RoutedCommand();

		private void Backstage_CmdSaveAs_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = true;
		}

		private void Backstage_CmdSaveAs_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => SaveDeviceCollectionAs()));
		}

		// Command: Backstage_CmdSettingsDefault

		public static RoutedCommand Backstage_CmdSettingsDefault = new RoutedCommand();

		private void Backstage_CmdSettingsDefault_CanExecute( object sender, CanExecuteRoutedEventArgs evt )
		{
			evt.CanExecute = true;
		}

		private void Backstage_CmdSettingsDefault_Executed( object sender, ExecutedRoutedEventArgs evt )
		{
			AppSettings.Reset();
		}

		// Event: Backstage_IsOpenChanged

		private void Backstage_IsOpenChanged_Handler( object sender, DependencyPropertyChangedEventArgs evt )
		{
			if ((sender as Backstage).IsOpen) {
				backstageTabRecentFiles.IsSelected = true;
				Backstage_PopulateRecentFiles();
			} else {
				Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => SetDataGridFocus()));
			}
		}

		// Event: RecentFile_ContextMenuOpenClick

		private void Backstage_RecentFile_ContextMenuOpenClick_Handler( object sender, RoutedEventArgs evt )
		{
			if (evt.Source is WinControls.MenuItem menuItem) {
				if (menuItem.Parent is WinControls.ContextMenu contextMenu) {
					if (contextMenu.PlacementTarget is RecentFileButton recentFileButton) {
						Backstage_CmdRecentFileOpen.Execute( null, recentFileButton );
					}
				}
			}
		}

		// Event: RecentFile_ContextMenuRemoveClick

		private void Backstage_RecentFile_ContextMenuRemoveClick_Handler( object sender, RoutedEventArgs evt )
		{
			if (evt.Source is WinControls.MenuItem menuItem) {
				if (menuItem.Parent is WinControls.ContextMenu contextMenu) {
					if (contextMenu.PlacementTarget is RecentFileButton recentFileButton) {
						Backstage_CmdRecentFileRemove.Execute(null, recentFileButton);
					}
				}
			}
		}
	}
}
