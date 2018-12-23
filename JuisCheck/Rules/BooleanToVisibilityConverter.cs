using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace JuisCheck.Rules
{
	[ValueConversion(sourceType: typeof(bool), targetType: typeof(Visibility))]
	public class BooleanToVisibilityConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			Visibility falseVisibility = Visibility.Hidden;

			if (parameter is string parameterStr) {
				if (!string.IsNullOrEmpty(parameterStr)) {
					falseVisibility = Enum.TryParse(parameterStr, true, out Visibility result) ? result : falseVisibility;
				}
			}

			return (bool)value ? Visibility.Visible : falseVisibility;
		}
 
		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return (Visibility)value == Visibility.Visible;
		}
	}
}
