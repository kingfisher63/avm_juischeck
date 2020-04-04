/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System.Windows.Controls;

namespace JuisCheck
{
	public class MultiSelectDataGrid : DataGrid
	{
		public MultiSelectDataGrid() : base()
		{
			CanSelectMultipleItems     = true;
			EnableColumnVirtualization = false;
			EnableRowVirtualization    = false;
		}
	}
}
