/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) 2018 Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace JuisCheck
{
	public class DeviceCollection : ObservableCollection<Device>
	{
		public const string	 xmlRootElementName = "Devices";

		public string		 FileName { get; protected set; } = null;

		protected	bool	_isModified = false;
		public		bool	 IsModified
		{
			get {
				return _isModified;
			}
			protected set {
				if (_isModified != value) {
					_isModified = value;
					RaiseIsModifiedChanged();
				}
			}
		}

		public DeviceCollection () : base()
		{
			CollectionChanged += CollectionChanged_Handler;
		}

		protected void AddItemPropertyChangedHandlers()
		{
			AddItemPropertyChangedHandlers((IList)Items);
		}

		protected void AddItemPropertyChangedHandlers( IList devices )
		{
			foreach (Device device in devices) {
				device.PropertyChanged += Item_PropertyChanged_Handler;
			}
		}

		public void Empty()
		{
			Clear();

			FileName   = null;
			IsModified = true;
			IsModified = false;
		}

		public void Load( string fileName )
		{
			if (fileName == null) {
				throw new ArgumentNullException(nameof(fileName));
			}

			using (XmlReader xmlReader = XmlReader.Create(fileName)) {
				XmlSerializer	xmlSerializer = new XmlSerializer(typeof(List<Device>), new XmlRootAttribute(xmlRootElementName));
				List<Device>	devices       = (List<Device>)xmlSerializer.Deserialize(xmlReader);

				Clear();
				devices.ForEach(d => Add(d));
			}

			FileName   = fileName;
			IsModified = true;
			IsModified = false;
		}

		protected void RemoveItemPropertyChangedHandlers( IList devices )
		{
			foreach (Device device in devices) {
				device.PropertyChanged -= Item_PropertyChanged_Handler;
			}
		}

		public void Save( string fileName = null )
		{
			if (fileName == null) {
				if (FileName == null) {
					throw new ArgumentNullException(nameof(fileName));
				}
				fileName = FileName;
			}

			using (XmlWriter xmlWriter = XmlWriter.Create(fileName, new XmlWriterSettings { Indent = true })) {
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(DeviceCollection), new XmlRootAttribute(xmlRootElementName));
				xmlSerializer.Serialize(xmlWriter, this);
			}

			FileName   = fileName;
			IsModified = true;
			IsModified = false;
		}

		// Event: CollectionChanged

		protected void CollectionChanged_Handler ( object sender, NotifyCollectionChangedEventArgs evt )
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

		// Event: IsModifiedChanged

		public event EventHandler IsModifiedChanged;

		protected void RaiseIsModifiedChanged()
		{
			IsModifiedChanged?.Invoke(this, new EventArgs());
		}

		// Event: Item_PropertyChanged

		protected void Item_PropertyChanged_Handler( object sender, PropertyChangedEventArgs evt )
		{
			switch (evt.PropertyName) {
				case nameof(Device.IsSelected):
					break;

				default:
					IsModified = true;
					break;
			}
		}
	}
}
