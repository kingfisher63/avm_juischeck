/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace JuisCheck
{
	public class DeviceCollection : ObservableCollection<Device>
	{
		public CollectionSettings Settings { get; protected set; } = new CollectionSettings();

		private string _FileName = null;
		public  string  FileName
		{
			get => _FileName;
			protected set {
				if (_FileName != value) {
					_FileName  = value;
					NotifyPropertyChanged();
				}

			}
		}

		private bool _IsModified = false;
		public  bool  IsModified
		{
			get => _IsModified;
			protected set {
				if (_IsModified != value) {
					_IsModified  = value;
					NotifyPropertyChanged();
				}
			}
		}

		/****************/
		/* Constructors */
		/****************/

		public DeviceCollection () : base()
		{
			CollectionChanged        += CollectionChanged_Handler;
			Settings.PropertyChanged += Settings_PropertyChanged_Handler;
		}

		/*****************/
		/* Other methods */
		/*****************/

		protected void AddItemPropertyChangedHandlers()
		{
			AddItemPropertyChangedHandlers((IList)Items);
		}

		protected void AddItemPropertyChangedHandlers( IList devices )
		{
            if (devices == null) {
                throw new ArgumentNullException(nameof(devices));
            }

			foreach (Device device in devices) {
				device.PropertyChanged += Item_PropertyChanged_Handler;
			}
		}

		public void Empty()
		{
			Clear();

			FileName   = null;
			IsModified = false;
		}

		public Device FindByID( string id )
		{
			return this.FirstOrDefault( d => d.ID == id );
		}

		public void Load( string fileName )
		{
			FileName = fileName;

			bool isModified = XML.JCData.LoadFromFile(fileName, this);
			foreach (Device device in this) {
				if (string.IsNullOrWhiteSpace(device.ID)) {
					device.ID  = Guid.NewGuid().ToString();
					isModified = true;
				}
			}
			Settings.NotifySettingsProperties();

			IsModified = isModified;
		}

		protected void RemoveItemPropertyChangedHandlers( IList devices )
		{
            if (devices == null) {
                throw new ArgumentNullException(nameof(devices));
            }

			foreach (Device device in devices) {
				device.PropertyChanged -= Item_PropertyChanged_Handler;
			}
		}

		public void Save( string fileName = null )
		{
			FileName = fileName ?? FileName;
			XML.JCData.SaveToFile(FileName, this);

			IsModified = false;
		}

		// Event handler: CollectionChanged

		private void CollectionChanged_Handler( object sender, NotifyCollectionChangedEventArgs evt )
		{
			switch (evt.Action) {
				case NotifyCollectionChangedAction.Add:
					AddItemPropertyChangedHandlers(evt.NewItems);
					IsModified = true;
					break;

				case NotifyCollectionChangedAction.Move:
					IsModified = true;
					break;

				case NotifyCollectionChangedAction.Remove:
					RemoveItemPropertyChangedHandlers(evt.OldItems);
					IsModified = true;
					break;

				case NotifyCollectionChangedAction.Replace:
					RemoveItemPropertyChangedHandlers(evt.OldItems);
					AddItemPropertyChangedHandlers(evt.NewItems);
					IsModified = true;
					break;

				case NotifyCollectionChangedAction.Reset:
					IsModified = true;
					break;
			}
		}

		// Event handler: Item_PropertyChanged

		private void Item_PropertyChanged_Handler( object sender, PropertyChangedEventArgs evt )
		{
			if (evt.PropertyName == nameof(Device.DeviceName)) {
				foreach (Device device in this) {
					device.NotifyDeviceNameChanged(((Device)sender).ID);
				}
			}

			if (evt.PropertyName != nameof(Device.IsSelected)) {
				IsModified = true;
			}
		}

		// Event handler: Settings_PropertyChanged

		private void Settings_PropertyChanged_Handler( object sender, PropertyChangedEventArgs evt )
		{
			IsModified = true;
		}

		// Event source: CollectionPropertyChanged
		//
		// Note: we cannot use the INotifyPropertyChanged implementation from the
		//       ObservableCollection class because PropertyChanged is protected.

		public event PropertyChangedEventHandler CollectionPropertyChanged;

		protected void NotifyPropertyChanged( [CallerMemberName] string propertyName = null )
		{
			CollectionPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
