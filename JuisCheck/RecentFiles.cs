/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) 2018 Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.Collections.Generic;
using System.Linq;

using JuisCheck.Properties;

namespace JuisCheck
{
	public static class RecentFiles
	{
		private static List<string> fileNames;

		public static void Add(string fileName)
		{
			LoadFileNames();

			int index = fileNames.FindIndex(rf => string.Compare(rf, fileName, true) == 0);
			if (index >= 0) {
				fileNames.RemoveAt(index);
			}
			fileNames.Insert(0, fileName);
		}

		public static List<string> GetFileNames( bool all = false )
		{
			LoadFileNames();

			return new List<string>(fileNames.Take(all ? int.MaxValue : Settings.Default.RecentFilesMax));
		}

		private static void LoadFileNames()
		{
			if (fileNames == null) {
				fileNames = new List<string>(Settings.Default.RecentFiles.Split('|'));
			}
		}

        public static void Remove(string fileName)
        {
            LoadFileNames();

            int index = fileNames.FindIndex(rf => string.Compare(rf, fileName, true) == 0);
			if (index >= 0)	{
				fileNames.RemoveAt(index);
			}
		}

		public static void Save() {
			LoadFileNames();

			int maxRecentFiles = Settings.Default.RecentFilesMax;
			while (fileNames.Count > maxRecentFiles) {
				fileNames.RemoveAt(maxRecentFiles);
			}

			Settings.Default.RecentFiles = string.Join("|", fileNames);
		}
	}
}
