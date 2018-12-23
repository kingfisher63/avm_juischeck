﻿/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) 2018 Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using Muon.DotNetExtensions;
using Muon.Windows;
using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;

using JuisCheck.Lang;
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

        private void DispatcherUnhandledException_Handler( object sender, DispatcherUnhandledExceptionEventArgs evt )
        {
			string exceptionText = string.Empty;

			for (Exception exception = evt.Exception; exception != null; exception = exception.InnerException) {
				exceptionText += string.Format("Type     = {0}\r\n", exception.GetType().Name);
				exceptionText += string.Format("Message  = {0}\r\n", exception.Message);
				exceptionText += string.Format("Source   = {0}\r\n", exception.Source);

				if (exception is XamlParseException xamlParseException) {
					exceptionText += string.Format("XAML URI  = {0}\r\n", xamlParseException.BaseUri.ToString());
					exceptionText += string.Format("XAML line = {0}\r\n", xamlParseException.LineNumber);
					exceptionText += string.Format("XAML pos  = {0}\r\n", xamlParseException.LinePosition);
					exceptionText += string.Format("XAML key  = {0}\r\n", xamlParseException.KeyContext);
					exceptionText += string.Format("XAML name = {0}\r\n", xamlParseException.NameContext);
				}

				exceptionText += string.Format("HasInner = {0}\r\n", exception.InnerException != null);
				exceptionText += string.Format("{0}\r\n\r\n", exception.StackTrace);
			}

			Clipboard.SetText(exceptionText);

			MessageBoxEx.Show(
				new MessageBoxExParams {
					CaptionText = JCstring.messageCaptionFatalError,
					MessageText = string.Format(JCstring.messageTextUnhandledException.Unescape(), evt.Exception.GetType().Name),
					Image       = MessageBoxExImage.Error,
					Button      = MessageBoxExButton.OK
				}
			);

			Environment.Exit(1);
		}
	}
}
