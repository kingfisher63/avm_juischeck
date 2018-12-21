using System;
using System.Globalization;
using System.Windows.Controls;

using JuisCheck.Lang;

namespace JuisCheck.Rules
{
	public class ComboBoxValueSelectedRule : ValidationRule
	{
		public override ValidationResult Validate( object value, CultureInfo cultureInfo )
		{
			if ((int)value < 0) {
				return new ValidationResult(false, JCstring.validationErrorNoValueSelected);
			}
			return ValidationResult.ValidResult;
		}
	}
}
