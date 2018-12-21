using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace JuisCheck.Rules
{
	[ValueConversion(typeof(bool), typeof(Visibility))]
	public class BooleanToVisibilityConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return (bool)value ? Visibility.Visible : Visibility.Hidden;
		}
 
		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return (Visibility)value == Visibility.Visible;
		}
	}
}
