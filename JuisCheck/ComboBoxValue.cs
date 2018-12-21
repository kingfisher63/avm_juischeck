/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) 2018 Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.Collections.Generic;
using System.Linq;

using JuisCheck.Lang;

namespace JuisCheck
{
	public class ComboBoxValue : IComparable
	{
		public string	Value			{ get; private set; }
		public string	DisplayString	{ get; private set; }

		public ComboBoxValue( string value, string displayString )
		{
			Value         = value;
			DisplayString = displayString;
		}

		public int CompareTo( object value2 )
		{
			return DisplayString.CompareTo(((ComboBoxValue)value2).DisplayString);
		}
	}

	public static class ComboBoxValueExt
	{
		public static List<ComboBoxValue> AppendMissingValueAsUnknown( this List<ComboBoxValue> values, int currentValue )
		{
			if (currentValue >= 0) {
				string currentValueStr = currentValue.ToString();
				if (!values.Any( v => currentValueStr == v.Value )) {
					values.Add(new ComboBoxValue(currentValueStr, string.Format(JCstring.comboboxValueUnknown, currentValueStr)));
				}
			}
			return values;
		}

		public static List<ComboBoxValue> AppendMissingValueAsUnknown( this List<ComboBoxValue> values, string currentValue )
		{
			if (!string.IsNullOrWhiteSpace(currentValue)) {
				if (!values.Any( v => currentValue == v.Value )) {
					values.Add(new ComboBoxValue(currentValue, string.Format(JCstring.comboboxValueUnknown, currentValue)));
				}
			}
			return values;
		}

		public static List<ComboBoxValue> PrependNotSet( this List<ComboBoxValue> values, string value = null )
		{
			values.Insert(0, new ComboBoxValue(value ?? string.Empty, JCstring.comboboxValueNotSet));
			return values;
		}
	}
}
