/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

using JuisCheck.Lang;

namespace JuisCheck.Resources.Converters
{
	[ValueConversion(sourceType: typeof(bool), targetType: typeof(Visibility))]
	public class BoolToVisibilityConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if (targetType != typeof(Visibility)) {
				throw new InvalidOperationException(JCmessage.InvalidTargetType);
			}
			if (!(value is bool)) {
				throw new InvalidOperationException(JCmessage.InvalidValueType);
			}

			return (bool)value ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return (Visibility)value == Visibility.Visible;
		}
	}
}
