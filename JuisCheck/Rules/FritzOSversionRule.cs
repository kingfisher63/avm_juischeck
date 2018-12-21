using System.Globalization;
using System.Linq;
using System.Windows.Controls;

using JuisCheck.Lang;

namespace JuisCheck.Rules
{
	public class FritzOSversionRule : ValidationRule
	{
		public bool AcceptEmpty	{ get; set; } = false;

		public override ValidationResult Validate( object value, CultureInfo cultureInfo )
		{
			string	strval = ((string)value).Trim();
			if (string.IsNullOrWhiteSpace(strval)) {
				return AcceptEmpty ? ValidationResult.ValidResult : new ValidationResult(false, JCstring.validationErrorEmptyOrWhiteSpace);
			}

			string[] parts = strval.Split('.');
			if (parts.Length != 3) {
				return new ValidationResult(false, JCstring.validationErrorInvalidFritzOSversion);
			}

			if (!CheckPart(parts[0], 1, 3) || !CheckPart(parts[1], 2, 2) || !CheckPart(parts[2], 2, 3)) {
				return new ValidationResult(false, JCstring.validationErrorInvalidFritzOSversion);
			}

			return ValidationResult.ValidResult;
		}

		protected bool CheckPart( string part, int minLength, int MaxLength)
		{
			if (part.Length < minLength) { return false; }
			if (part.Length > MaxLength) { return false; }

			return part.All(char.IsDigit);
		}
	}
}
