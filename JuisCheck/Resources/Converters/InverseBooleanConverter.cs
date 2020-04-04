/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.Globalization;
using System.Windows.Data;

using JuisCheck.Lang;

namespace JuisCheck.Resources.Converters
{
	[ValueConversion(sourceType: typeof(bool), targetType: typeof(bool))]
	public class InverseBooleanConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if (targetType != typeof(bool)) {
				throw new InvalidOperationException(JCmessage.InvalidTargetType);
			}
			if (!(value is bool)) {
				throw new InvalidOperationException(JCmessage.InvalidValueType);
			}

			return !(bool)value;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}
