using System;
using System.Windows;
using System.Windows.Controls;

namespace JuisCheck
{
	public static class ContextMenuHelper
	{
		public static UIElement GetPlacementTarget(object o)
		{
			if (o == null) {
				throw new ArgumentNullException(nameof(o));
			}

			if (o is DependencyObject element) {
				while (element != null) {
					if (element is ContextMenu contextMenu) {
						return contextMenu.PlacementTarget;
					}
					element = LogicalTreeHelper.GetParent(element);
				}
			}

			return null;
		}
	}
}
