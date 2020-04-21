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
	public class StringInt32RangeRule : ValidationRule
	{
		public int Min	{ get; set; } = int.MinValue;
		public int Max	{ get; set; } = int.MaxValue;

		public override ValidationResult Validate( object value, CultureInfo cultureInfo )
		{
			if (!(value is string strval)) {
				throw new InvalidOperationException("Invalid value type");
			}
			strval = strval.Trim();

			if (strval.Length == 0) {
				return new ValidationResult(false, JCstring.ValidationErrorEmptyOrWhiteSpace);
			}

			if (!int.TryParse((string)value, out int intval)) {
				return new ValidationResult(false, JCstring.ValidationErrorInvalidCharacters);
			}

			if (intval < Min || intval > Max) {
				return new ValidationResult(false, string.Format(CultureInfo.CurrentCulture, JCstring.ValidationErrorValueOutOfRange, Min, Max));
			}

			return ValidationResult.ValidResult;
		}
	}
}
