# JuisCheck for Windows

JuisCheck for Windows is a tool to query the AVM update information services
(JUIS and CATI) for device firmware updates. JuisCheck for Windows supports
all types of AVM devices (FRITZ!Box, FRITZ!DECT, FRITZ!Powerline and FRITZ!WLAN).

## Basic program usage

+ Run *JuisCheck.exe* in the *bin\Release* folder.
+ Add your devices.
+ Select one or more devices and click *Find Updates*.

## File tab (backstage)

This tab contains a menu with buttons to open, close and save your device
collections. It is also the place to manage recent files and program settings.

### Backstage menu buttons

+ *Save* (Ctrl+S). Save the device collection to the file from which it was loaded.
+ *Save As...* (Ctrl+Alt+S). Save the device collection to a different file.
+ *Open...* (Ctrl+O). Load a device collection from a file.
+ *Close* (Ctrl+W). Close the device collection. If the data is modified you will
  be prompted if you want to save the data.
+ *Recent*. Open the Recent Files panel.
+ *Settings*. Open the Settings panel.
+ *About*. Display program information.
+ *Exit*. Exit the program. If the data is modified you will be prompted if you
  want to save the data.

### Recent files

JuisCheck for Windows remembers recently opened device collection files up to
the configured number (the default is 10). Click on a recent file to open it.
If a file does not exist, it will be greyed out.

#### Context menu

Right-click on a recent file to open the context menu. This menu has items
to open the file and to remove the file from the list (this does not delete
the file).

#### Buttons

+ *Remove non-existing files from list*. Remove the recent files that are
  greyed out.
+ *Remove all files from list*. Remove all recent files (empties the recent
  files list).

### Settings

Here you can customize program behavior and configure defaults for some device
properties.

#### Buttons

+ *Reset to defaults*. Reset all settings to their defaults. This also resets
  the visibility of the data grid columns.

## Devices tab

### Ribbon menu buttons

#### Devices section

+ *Add Device*. Add a FRITZ!Box, FRITZ!Powerline or FRITZ!WLAN device. Firmware
  update information for these devices is retrieved from the AVM JUIS service.
+ *Add DECT*. Add a FRITZ!DECT device. Firmware update information for DECT devices
  is retrieved from the AVM CATI service.
+ *Delete*. Remove the selected devices from the device collection.
+ *Edit*. Edit the selected device.
+ *Select All*. Select all devices in the device collection.
+ *Select None*. Unselect all devices in the device collection.

#### Firmware Update section

+ *Find Updates*. Query the appropriate AVM update information services for the
  selected devices.
+ *Copy URLs*. Copy the image download URLs for the selected devices to the clipboard.
+ *Get Image*. Download the firmware image for the selected device using the default
  web browser.
+ *View Info*. View the release notes for the selected device (if available) using
  the default web browser (not available for FRITZ!DECT devices).
+ *Make Current*. Make the updated version the current firmware version for the
  selected devices and clear the update information.
+ *Clear Updates*. Clear the update information for the seleted devices.

*Note:* Buttons are only enabled if they can do something useful. For example, the
*View Info* button is only enabled if the update information service supplied a URL
for information about the update.

### Data grid

The data grid shows the devices in the collection (one device per row). Each
column shows a device property. Some columns are hidden by default.

Click on a device to select only that device. You can select multiple devices
(ctrl-click on a device to select/unselect it). Double-click on a device to edit it.

#### Context menu

Right-click on a data grid header to open the context menu. The context menu contains
the column list. Visible columns have a checkmark. Click on a column item to show/hide
the column. You cannot hide the *Device name* column.

## Device dialogs

These dialogs open when you click the *Add Device*, *Add DECT* or *Edit* button.
Input fields are checked for valid input. A field with invalid input will have a
red border. In this case the field has a tooltip that explains the reason for this.
If any field has invalid input, you cannot click the *OK* button.

If the device is a device that uses the JUIS service (all devices except DECT),
you can supply a network address (IP address or DNS name) and click the *Query*
button. JuisCheck for Windows will then populate the device properties from the
*juis_boxinfo.xml* file retrieved from the device.

## Device collection files

Device collections are stored in XML files. The *examples* folder contains a number
of device collections that are provided as a courtesy. Despite considerable efforts
from the author you must not assume that the data is either complete or fully correct.

## Miscellaneous

### Compatibility

+ Microsoft Windows (32-bit or 64-bit)
+ Microsoft .NET Framework 4.0 or later

JuisCheck for Windows has been tested on Windows 7 and Windows 10. The program
will probably also run on older Windows versions.

### Privacy

JuisCheck for Windows does not collect any information (neither personal nor
otherwise).

### Rebuilding from source

JuisCheck for Windows has been built using Visual Studio 2017. The Vitevic
Assembly Embedder extension is used to embed 3rd party library DLLs as
resources into the executable.

If you want/need to build the solution without the Vitevic Assembly Embedder
extension, you must set *Copy Local* to *True* for the following references:

+ ControlzEx
+ Fluent
+ Muon.DotNetExtensions
+ Muon.Windows
+ Muon.Windows.Controls
