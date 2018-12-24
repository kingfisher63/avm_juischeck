using System.Globalization;
using System.Windows.Controls;

using JuisCheck.Lang;

namespace JuisCheck.Rules
{
	public class Int32RangeRule : ValidationRule
	{
		public int Min	{ get; set; } = int.MinValue;
		public int Max	{ get; set; } = int.MaxValue;

		public override ValidationResult Validate( object value, CultureInfo cultureInfo )
		{
			string	strval = ((string)value).Trim();

			if (strval.Length == 0) {
				return new ValidationResult(false, JCstring.ValidationErrorEmptyOrWhiteSpace);
			}

			if (!int.TryParse((string)value, out int intval)) {
				return new ValidationResult(false, JCstring.ValidationErrorInvalidCharacters);
			}

			if (intval < Min || intval > Max) {
				return new ValidationResult(false, string.Format(JCstring.ValidationErrorValueOutOfRange, Min, Max));
			}

			return ValidationResult.ValidResult;
		}
	}
}
