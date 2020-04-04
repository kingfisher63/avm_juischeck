/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using JuisCheck.Lang;

namespace JuisCheck
{
	public class ComboBoxValue
	{
		public string	Value			{ get; private set; }
		public string	DisplayString	{ get; private set; }

		public ComboBoxValue( string value, string displayString )
		{
			Value         = value;
			DisplayString = displayString;
		}
	}

	public static class ComboBoxValueExt
	{
		public static List<ComboBoxValue> AppendMissingAsUnknown( this List<ComboBoxValue> values, int currentValue )
		{
			if (values == null) {
				throw new ArgumentNullException(nameof(values));
			}

			if (currentValue >= 0) {
				string currentValueStr = currentValue.ToString(CultureInfo.InvariantCulture);
				if (!values.Any( v => currentValueStr == v.Value )) {
					values.Add(new ComboBoxValue(currentValueStr, string.Format(CultureInfo.CurrentCulture, JCstring.ComboBoxValueUnknown, currentValueStr)));
				}
			}

			return values;
		}

		public static List<ComboBoxValue> AppendMissingAsUnknown( this List<ComboBoxValue> values, string currentValue )
		{
			if (values == null) {
				throw new ArgumentNullException(nameof(values));
			}

			if (!string.IsNullOrWhiteSpace(currentValue)) {
				if (!values.Any( v => currentValue == v.Value )) {
					values.Add(new ComboBoxValue(currentValue, string.Format(CultureInfo.CurrentCulture, JCstring.ComboBoxValueUnknown, currentValue)));
				}
			}

			return values;
		}

		public static List<ComboBoxValue> Prepend( this List<ComboBoxValue> values, string value, string displayString )
		{
			values.Insert(0, new ComboBoxValue(value ?? throw new ArgumentNullException(nameof(value)), displayString ?? throw new ArgumentNullException(nameof(displayString))));

			return values;
		}
	}
}
