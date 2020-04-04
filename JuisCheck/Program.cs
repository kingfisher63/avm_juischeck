/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
 
namespace JuisCheck
{
	// This class preloads DLLs that have been embedded as resources in the executing assembly so that
	// the application can be deployed as a single executable file.
	//
	// Caveat: this class cannot be used with the Visual Studio Debugger (an obscure ArgumentException
	// is thrown). Therefore the developer must select the appropriate startup object in the application
	// properties:
	// - Debug builds   => JuisCheck.App
	// - Release builds => JuisCheck.Program

	public static class Program
	{
		private static readonly Dictionary<string,Assembly> assemblies = new Dictionary<string,Assembly>();

		[STAThread]
		public static void Main()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] resourceNames     = executingAssembly.GetManifestResourceNames().Where(rn => rn.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)).ToArray();

			foreach (string resourceName in resourceNames) {
				try {
					using (Stream resourceStream = executingAssembly.GetManifestResourceStream(resourceName)) {
						if (resourceStream == null) {
							throw new FileLoadException($"{resourceName}: GetManifestResourceStream failed.");
						}

						byte[] data = new byte[resourceStream.Length];
						resourceStream.Read(data, 0, data.Length);
						assemblies.Add(resourceName, Assembly.Load(data));
					}
				}
				catch (Exception ex) {
					Debug.Print($"{resourceName}: {ex.Message}");
				}
			}

			AppDomain.CurrentDomain.AssemblyResolve += AppDomain_AssemblyResolve_Handler;
			App.Main();
		}

		private static Assembly AppDomain_AssemblyResolve_Handler( object sender, ResolveEventArgs evt )
		{
			string resourceName = $"{new AssemblyName(evt.Name).Name}.dll";

			if (assemblies.ContainsKey(resourceName)) {
				return assemblies[resourceName];
			}
 
			return null;
		}
	}
}
