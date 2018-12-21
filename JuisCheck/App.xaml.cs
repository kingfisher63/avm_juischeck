/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) 2018 Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System.Windows;

using JuisCheck.Properties;

namespace JuisCheck
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		// Event: Exit

		private void Exit_Handler( object sender, ExitEventArgs evt )
		{
			RecentFiles.Save();
			Settings.Default.Save();
		}

		// Event: Startup

		private void Startup_Handler( object sender, StartupEventArgs evt )
		{
			Settings settings = Settings.Default;

			if (settings.SettingsUpgradeRequired) {
				settings.Upgrade();
				settings.SettingsUpgradeRequired = false;
			}
		}
	}
}
