using System;
using System.Globalization;
using System.Windows.Data;

namespace JuisCheck.Rules
{
	[ValueConversion(sourceType: typeof(DateTime?), targetType: typeof(string))]
	public class NullableDateTimeToStringConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if (value is DateTime valueDateTime) {
				if (parameter is string parameterString) {
					return (valueDateTime).ToString(parameterString);
				} else {
					return (valueDateTime).ToString("G");
				}
			}

			return string.Empty;
		}
 
		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}
