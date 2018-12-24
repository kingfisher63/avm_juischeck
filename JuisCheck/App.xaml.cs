/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) 2018 Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using Muon.DotNetExtensions;
using Muon.Windows;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using WinForms = System.Windows.Forms;

using JuisCheck.Lang;
using JuisCheck.Properties;

namespace JuisCheck
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static bool SafeClipboardSetText( string text )
		{
			try {
				// Does retries internally if needed (System.Windows.Clipboard does not do this)
				WinForms.Clipboard.SetDataObject(text, true);
				return true;
			}
			catch (ExternalException) {
				return false;
			}
		}

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

		// Event: DispatcherUnhandledException

		private void DispatcherUnhandledException_Handler( object sender, DispatcherUnhandledExceptionEventArgs evt )
        {
			string exceptionText = string.Empty;

			for (Exception exception = evt.Exception; exception != null; exception = exception.InnerException) {
				exceptionText += string.Format("Type      = {0}\r\n", exception.GetType().Name);
				exceptionText += string.Format("Message   = {0}\r\n", exception.Message);

				if (exception is ExternalException externalException) {
					exceptionText += string.Format("HResult   = 0x{0:X8}", externalException.ErrorCode);
				}

				if (exception is XamlParseException xamlParseException) {
					exceptionText += string.Format("XAML URI  = {0}\r\n", xamlParseException.BaseUri.ToString());
					exceptionText += string.Format("XAML line = {0}\r\n", xamlParseException.LineNumber);
					exceptionText += string.Format("XAML pos  = {0}\r\n", xamlParseException.LinePosition);
					exceptionText += string.Format("XAML key  = {0}\r\n", xamlParseException.KeyContext);
					exceptionText += string.Format("XAML name = {0}\r\n", xamlParseException.NameContext);
				}

				exceptionText += string.Format("HasInner  = {0}\r\n", exception.InnerException != null);
				exceptionText += string.Format("{0}\r\n\r\n", exception.StackTrace);
			}

			string message;
			if (SafeClipboardSetText(exceptionText)) {
				message = JCstring.MessageTextUnhandledExceptionCopySuccess.Unescape();
			} else {
				message = JCstring.MessageTextUnhandledExceptionCopyFailure.Unescape();
			}

			MessageBoxEx.Show(
				new MessageBoxExParams {
					CaptionText = JCstring.MessageCaptionFatalError,
					MessageText = string.Format(message, evt.Exception.GetType().Name),
					Image       = MessageBoxExImage.Error,
					Button      = MessageBoxExButton.OK
				}
			);

			Environment.Exit(1);
		}
	}
}
