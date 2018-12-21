using System;
using System.Globalization;
using System.Windows.Data;

namespace JuisCheck.Rules
{
	[ValueConversion(typeof(DateTime?), typeof(string))]
	public class NullableDateTimeToStringConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if (value == null) {
				return string.Empty;
			}

			return ((DateTime)value).ToString((parameter as string) ?? "G");
		}
 
		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}
