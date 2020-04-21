/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;

using JuisCheck.Lang;

namespace JuisCheck
{
	public sealed class DeviceComparer : Comparer<Device>
	{
		private readonly	DeviceProperty			sortProperty;
		private readonly	ListSortDirection		sortDirection;
		private readonly	NaturalStringComparer	stringComparer;

		public DeviceComparer( DeviceProperty property, ListSortDirection direction )
		{
			if (!Enum.IsDefined(typeof(DeviceProperty), property)) {
				throw new ArgumentOutOfRangeException(nameof(property));
			}
			if (!Enum.IsDefined(typeof(ListSortDirection), direction)) {
				throw new ArgumentOutOfRangeException(nameof(direction));
			}

			sortProperty   = property;
			sortDirection  = direction;
			stringComparer = new NaturalStringComparer(App.defaultDisplayStringComparison);
		}

		public override int Compare( Device device1, Device device2 )
		{
			if (device1 == null) {
				throw new ArgumentNullException(nameof(device1));
			}
			if (device2 == null) {
				throw new ArgumentNullException(nameof(device2));
			}

			int result;

			switch (sortProperty) {
				case DeviceProperty.DeviceAddressStr:	result = stringComparer.Compare(device1.DeviceAddressStr, device2.DeviceAddressStr); break;
				case DeviceProperty.DeviceName:			result = stringComparer.Compare(device1.DeviceName,       device2.DeviceName);       break;
				case DeviceProperty.FirmwareStr:		result = stringComparer.Compare(device1.FirmwareStr,      device2.FirmwareStr);      break;
				case DeviceProperty.ProductName:		result = stringComparer.Compare(device1.ProductName,      device2.ProductName);      break;
				default:								throw new ApplicationException("Unsupported comparison");
			}

			return sortDirection == ListSortDirection.Ascending ? result : -result;
		}
	}
}
