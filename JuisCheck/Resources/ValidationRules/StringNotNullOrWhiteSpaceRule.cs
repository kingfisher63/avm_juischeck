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
	public class StringNotNullOrWhiteSpaceRule : ValidationRule
	{
		public override ValidationResult Validate( object value, CultureInfo cultureInfo )
		{
			if (!(value is string strval)) {
				throw new InvalidOperationException("Invalid value type");
			}

			if (string.IsNullOrWhiteSpace(strval)) {
				return new ValidationResult(false, JCstring.ValidationErrorEmptyOrWhiteSpace);
			}

			return ValidationResult.ValidResult;
		}
	}
}
