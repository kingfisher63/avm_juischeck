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
	[ValueConversion(sourceType: typeof(DateTime?), targetType: typeof(string))]
	public class NullableDateTimeToStringConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if (targetType != typeof(string)) {
				throw new InvalidOperationException("Invalid target type");
			}
			if (value == null) {
				return string.Empty;
			}
			if (!(value is DateTime)) {
				throw new InvalidOperationException("Invalid value type");
			}

			return ((DateTime)value).ToString("G", CultureInfo.CurrentCulture);
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}
