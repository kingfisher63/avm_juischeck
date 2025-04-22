using System.Windows;
using System.Windows.Controls;

namespace JuisCheck
{
	public static class ExtensionMethods
	{
		public static bool GetTreeHasError(this DependencyObject node, bool skipDisabled, bool skipInvisible)
		{
			if (node is UIElement uiElement) {
				if (skipDisabled && !uiElement.IsEnabled) {
					return false;
				}
				if (skipInvisible && !uiElement.IsVisible) {
					return false;
				}
			}

			if (Validation.GetHasError(node)) {
				return true;
			}

			foreach (object childNode in LogicalTreeHelper.GetChildren(node)) {
				if (childNode is DependencyObject dependencyObject) {
					if (dependencyObject.GetTreeHasError(skipDisabled, skipInvisible)) {
						return true;
					}
				}
			}

			return false;
		}
	}
}
