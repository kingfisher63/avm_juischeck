/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.Globalization;
using System.Windows.Controls;

using JuisCheck.Lang;

namespace JuisCheck.Resources.ValidationRules
{
	public class ComboBoxValueSelectedRule : ValidationRule
	{
		public override ValidationResult Validate( object value, CultureInfo cultureInfo )
		{
			if (!(value is int intval)) {
				throw new InvalidOperationException(JCmessage.InvalidValueType);
			}

			if (intval < 0) {
				return new ValidationResult(false, JCstring.ValidationErrorNoValueSelected);
			}

			return ValidationResult.ValidResult;
		}
	}
}
