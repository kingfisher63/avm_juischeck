/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.Collections.Generic;

namespace JuisCheck
{
	public sealed class ComboBoxValueComparer : Comparer<ComboBoxValue>
	{
		private readonly IComparer<string> comparer;

		public ComboBoxValueComparer( StringComparison stringComparison )
		{
			switch (stringComparison) {
				case StringComparison.CurrentCulture:				comparer = StringComparer.CurrentCulture; break;
				case StringComparison.CurrentCultureIgnoreCase:		comparer = StringComparer.CurrentCultureIgnoreCase; break;
				case StringComparison.InvariantCulture:				comparer = StringComparer.InvariantCulture; break;
				case StringComparison.InvariantCultureIgnoreCase:	comparer = StringComparer.InvariantCultureIgnoreCase; break;
				case StringComparison.Ordinal:						comparer = StringComparer.Ordinal; break;
				case StringComparison.OrdinalIgnoreCase:			comparer = StringComparer.OrdinalIgnoreCase; break;
				default:											throw new ArgumentOutOfRangeException(nameof(stringComparison));
			}
		}

		public ComboBoxValueComparer( IComparer<string> stringComparer )
		{
			comparer = stringComparer ?? throw new ArgumentNullException(nameof(stringComparer));
		}

		public override int Compare( ComboBoxValue value1, ComboBoxValue value2 )
		{
			if (value1 == null) {
				throw new ArgumentNullException(nameof(value1));
			}
			if (value2 == null) {
				throw new ArgumentNullException(nameof(value2));
			}

			return comparer.Compare(value1.DisplayString, value2.DisplayString);
		}
	}
}
