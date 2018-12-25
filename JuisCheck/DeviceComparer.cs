/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) 2018 Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace JuisCheck
{
	public class DeviceComparer : IComparer
	{
		public enum Property
		{
			BaseFritzOS,
			DeviceAddress,
			DeviceName,
			FirmwareStr,
			ProductName
		}

		public static bool SetComparer( ListCollectionView collectionView, DataGridColumn column )
		{
			Property			sortProperty;
			ListSortDirection	sortDirection;

			if (collectionView == null || column == null) {
				return false;
			}

			switch (column.SortMemberPath) {
				case nameof(Device.BaseFritzOS):	sortProperty = Property.BaseFritzOS;   break;
				case nameof(Device.DeviceAddress):	sortProperty = Property.DeviceAddress; break;
				case nameof(Device.DeviceName):		sortProperty = Property.DeviceName;	   break;
				case nameof(Device.FirmwareStr):	sortProperty = Property.FirmwareStr;   break;
				case nameof(Device.ProductName):	sortProperty = Property.ProductName;   break;
				default:							return false;
			}

			sortDirection = column.SortDirection != ListSortDirection.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending;
			column.SortDirection = sortDirection;

			collectionView.CustomSort = new DeviceComparer(sortProperty, sortDirection);
			return true;
		}

		protected readonly	Property				sortProperty;
		protected readonly	ListSortDirection		sortDirection;
		protected readonly	NaturalStringComparer	comparer;

		public DeviceComparer( Property property, ListSortDirection direction )
		{
			sortProperty  = property;
			sortDirection = direction;
			comparer      = new NaturalStringComparer(StringComparison.CurrentCultureIgnoreCase);
		}

		public int Compare( object dev1, object dev2 )
		{
			if (!(dev1 is Device device1)) {
				throw new ArgumentException(string.Format("Argument must be {0} type", typeof(Device).Name), nameof(dev1));
			}
			if (!(dev2 is Device device2)) {
				throw new ArgumentException(string.Format("Argument must be {0} type", typeof(Device).Name), nameof(dev2));
			}

			int result = 0;
			switch (sortProperty) {
				case Property.BaseFritzOS:		result = comparer.Compare(device1.BaseFritzOS,   device2.BaseFritzOS);   break;
				case Property.DeviceAddress:	result = comparer.Compare(device1.DeviceAddress, device2.DeviceAddress); break;
				case Property.DeviceName:		result = comparer.Compare(device1.DeviceName,    device2.DeviceName);    break;
				case Property.FirmwareStr:		result = comparer.Compare(device1.FirmwareStr,   device2.FirmwareStr);   break;
				case Property.ProductName:		result = comparer.Compare(device1.ProductName,   device2.ProductName);   break;
			}

			return sortDirection == ListSortDirection.Ascending ? result : -result;
		}
	}
}
