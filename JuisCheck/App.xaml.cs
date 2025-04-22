/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using Muon.Dotnet.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
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
		public const string					defaultDataFileName            = "default.xml";
		public const string					portableProgramFileName        = "JuisCheckPortable.exe";
		public const string					programName                    = "JuisCheck";
		public const string					programVersionSuffix           = ""; // BETA, RC1, etc.

		public const StringComparison		defaultDisplayStringComparison = StringComparison.CurrentCultureIgnoreCase;
		public const StringComparison		defaultFileNameComparison      = StringComparison.OrdinalIgnoreCase;

		private static readonly Settings	programSettings                = Settings.Default;

		public static bool					IsLanguageSelectionEnabled	{ get; private set; }
		public static bool					IsPortableMode				{ get; private set; }
		public static bool					RestartPending				{ get; private set; }

		public static List<string>			AdditionalLanguages			{ get; } = new List<string>() { "de", "es", "fr", "it", "nl", "pl" };

		/*****************/
		/* Other methods */
		/*****************/

		public static void CancelRestart()
		{
			RestartPending = false;
		}

		public static string		GetAssemblyName()		{ return Assembly.GetExecutingAssembly().GetName().Name; }
		public static string		GetCopyright()			{ return Assembly.GetExecutingAssembly().GetCopyright(); }
		public static string		GetDefaultDirectory()	{ return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); }
		public static string		GetLocation()			{ return Assembly.GetExecutingAssembly().Location; }
		public static string		GetProgramDirectory()	{ return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
		public static string		GetProgramFileName()	{ return Path.GetFileName(Assembly.GetExecutingAssembly().Location); }
		public static string		GetProgramInfo()		{ return $"{programName} {GetVersion()}{programVersionSuffix}"; }
		public static MainWindow	GetMainWindow()			{ return (MainWindow)Current.MainWindow; }
		public static string		GetVersion()			{ return Assembly.GetExecutingAssembly().GetName().Version.ToString(3); }

		public static void Restart()
		{
			RestartPending = true;
			GetMainWindow().Close();
		}

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

		private static bool ServiceCertificateValidationCallback(object obt, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			if (sslPolicyErrors == SslPolicyErrors.None) {
				return true;
			}

			var cert2      = new X509Certificate2(cert);
			var commonName = cert2.GetNameInfo(X509NameType.SimpleName, false);

			if (commonName == JUIS.GetServerName()) {
				return true;
			}

			return false;
		}

		// Event handler: Exit

		private void Exit_Handler( object sender, ExitEventArgs evt )
		{
			if (!IsPortableMode) {
				programSettings.Save();
			}

			if (RestartPending) {
				Process.Start(App.GetLocation());
			}
		}

		// Event handler: Startup

		private void Startup_Handler( object sender, StartupEventArgs evt )
		{
			string	assemblyName     = GetAssemblyName();
			string	programDirectory = GetProgramDirectory();
			string	programName      = GetProgramFileName();

			// Portable mode

			IsPortableMode = string.Compare(programName, portableProgramFileName, defaultFileNameComparison) == 0;

			// Initialize settings

			if (IsPortableMode) {
				programSettings.Reset();
				programSettings.AutoLoadFile = Path.Combine(programDirectory, defaultDataFileName);
				programSettings.LastDocumentDirectory = programDirectory;
				programSettings.LastDownloadDirectory = programDirectory;
			} else
			if (programSettings.SettingsUpgradeRequired) {
				programSettings.Upgrade();
				programSettings.SettingsUpgradeRequired = false;
			}

			// Additional languages (using resource dictionaries)

			if (IsPortableMode) {
				IsLanguageSelectionEnabled = false;
			} else {
				foreach (string language in AdditionalLanguages.ToList()) {
					if (!File.Exists(Path.Combine(programDirectory, language, $"{assemblyName}.resources.dll"))) {
						AdditionalLanguages.Remove(language);
					}
				}

				if (AdditionalLanguages.Count == 0) {
					programSettings.UserInterfaceLanguage = "auto";
				}

				IsLanguageSelectionEnabled = AdditionalLanguages.Count != 0;
			}

			// Set user interface language

			CultureInfo cultureInfo;
			try {
				cultureInfo = new CultureInfo(programSettings.UserInterfaceLanguage);
			}
			catch (CultureNotFoundException) {
				cultureInfo = CultureInfo.InstalledUICulture;
			}
			Thread.CurrentThread.CurrentUICulture = cultureInfo;

			// SSL/TLS

			ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Ssl3;  // Deprecated
			ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls;   // Deprecated
			ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls11; // Deprecated
			ServicePointManager.SecurityProtocol |=  SecurityProtocolType.Tls12;

			// Ignore certificate errors for JUIS queries (AVM uses a private root CA that is not globally trused)

			ServicePointManager.ServerCertificateValidationCallback += ServiceCertificateValidationCallback;
		}

		// Event handler: DispatcherUnhandledException

		private void DispatcherUnhandledException_Handler( object sender, DispatcherUnhandledExceptionEventArgs evt )
        {
			string exceptionText = string.Empty;

			for (Exception exception = evt.Exception; exception != null; exception = exception.InnerException) {
				exceptionText += $"Type      = {exception.GetType().Name}\r\n";
				exceptionText += $"Message   = {exception.Message}\r\n";

				if (exception is ExternalException externalException) {
					exceptionText += $"HResult   = 0x{externalException.ErrorCode:X8}";
				}

				if (exception is XamlParseException xamlParseException) {
					exceptionText += $"XAML URI  = {xamlParseException.BaseUri}\r\n";
					exceptionText += $"XAML line = {xamlParseException.LineNumber}\r\n";
					exceptionText += $"XAML pos  = {xamlParseException.LinePosition}\r\n";
					exceptionText += $"XAML key  = {xamlParseException.KeyContext}\r\n";
					exceptionText += $"XAML name = {xamlParseException.NameContext}\r\n";
				}

				exceptionText += $"HasInner  = {exception.InnerException != null}\r\n";
				exceptionText += $"{exception.StackTrace}\r\n\r\n";
			}

			string message;
			if (SafeClipboardSetText(exceptionText)) {
				message = JCstring.MessageTextUnhandledExceptionCopySuccess.Unescape();
			} else {
				message = JCstring.MessageTextUnhandledExceptionCopyFailure.Unescape();
			}

			_ = MessageBoxEx.Show(new MessageBoxExParams {
					CaptionText = JCstring.MessageCaptionFatalError,
					MessageText = string.Format(CultureInfo.CurrentCulture, message, evt.Exception.GetType().Name),
					Image       = MessageBoxExImage.Error,
					ButtonText  = new string[] { JCstring.DialogButtonTextOk }
				});

			Environment.Exit(1);
		}
	}
}
