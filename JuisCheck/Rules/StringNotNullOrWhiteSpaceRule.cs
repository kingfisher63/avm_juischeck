using System.Globalization;
using System.Windows.Controls;

using JuisCheck.Lang;

namespace JuisCheck.Rules
{
	public class StringNotNullOrWhiteSpaceRule : ValidationRule
	{
		public override ValidationResult Validate( object value, CultureInfo cultureInfo )
		{
			if (string.IsNullOrWhiteSpace(value as string)) {
				return new ValidationResult(false, JCstring.ValidationErrorEmptyOrWhiteSpace);
			}

			return ValidationResult.ValidResult;
		}
	}
}
