/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) 2018 Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace JuisCheck
{
	public class ComboBoxEx : ComboBox
	{
		// DependencyProperty: MaxDropDownItems

		public static readonly DependencyProperty MaxDropDownItemsProperty = DependencyProperty.Register("MaxDropDownItems", typeof(int), typeof(ComboBoxEx), new FrameworkPropertyMetadata(0));

		public int MaxDropDownItems
		{
			get => (int)GetValue(MaxDropDownItemsProperty);
			set => 		SetValue(MaxDropDownItemsProperty, value);
		}

		// Event: DropDownOpened

		protected override void OnDropDownOpened( EventArgs evt )
		{
			base.OnDropDownOpened(evt);

			if (MaxDropDownItems > 0) {
				Dispatcher.BeginInvoke(DispatcherPriority.Normal,
					new Action(() => {
						if (ItemContainerGenerator.ContainerFromIndex(0) is UIElement container) {
							if (container.RenderSize.Height > 0) {
								MaxDropDownHeight = container.RenderSize.Height * MaxDropDownItems + 2;
							}
						}
					})
				);
			}
		}
	}
}
