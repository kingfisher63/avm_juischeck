# JuisCheck

JuisCheck is a tool to query the AVM update information services (JUIS and CATI)
for device firmware updates. JuisCheck supports all types of AVM devices (FRITZ!Box,
FRITZ!DECT, FRITZ!Powerline and FRITZ!WLAN).

## System requirements

+ Microsoft Windows Vista or later (32-bit or 64-bit).
+ Microsoft .NET Framework 4.5 or later.

## Installation

- Download *JuisCheck.exe* from [bin/Release](bin/Release) and store it in a folder
  on your computer (or on a thumb drive, see *Portable mode* below).

- If you want to use a user interface language other than English, also copy the
  corresponding language folders (see *Language selection* below) to the appplication
  directory.

## Upgrading from JuisCheck v1.x

+ JuisCheck v2 uses a new device collection XML file format. JuisCheck v2 can read
  JuisCheck v1 files, but will always save files in the new v2 format (incompatible
  with JuisCheck v1.x). If you want to retain files in the old v1 format, make sure
  to backup your data first. After loading a v1 file, the data is automatically
  marked as modified so that you will be prompted to save it.
<<<<<<< HEAD

+ After loading a JuisCheck v1 file, DECT devices will have no DECT base selected:
  you must edit each DECT device and select a DECT base before you can search for
  updates (see *Device dialogs* below).


# Program usage

## The basics

1. Start the program.
2. Add your devices.
3. Select one or more devices.
4. Click *Find updates* to seach for firmware updates.
5. If an update is found, select one device and click *Download firmware* to get
   the firmware file.

## File tab (backstage)

=======

+ After loading a JuisCheck v1 file, DECT devices will have no DECT base selected:
  you must edit each DECT device and select a DECT base before you can search for
  updates (see *Device dialogs* below).


# Program usage

## The basics

1. Start the program.
2. Add your devices.
3. Select one or more devices.
4. Click *Find updates* to seach for firmware updates.
5. If an update is found, select one device and click *Download firmware* to get
   the firmware file.

## File tab (backstage)

>>>>>>> 40d2bc6790ffee00a5a34b81b426510fe9ef19a2
### Backstage menu

+ *Save* (Ctrl+S). Save the device collection to the file from which it was loaded.
+ *Save As...* (Ctrl+Alt+S). Save the device collection to a different file.
+ *Open...* (Ctrl+O). Load a device collection from a file.
+ *Close* (Ctrl+W). Close the device collection. If the data is modified you will
  be prompted to save the data.
+ *Recent*. Open the Recent Files panel.
+ *Settings*. Open the Settings panel.
+ *About*. Display program information.
+ *Exit* (Ctrl+X). Exit the program. If the data is modified you will be prompted
  to save the data.

### Recent files panel

JuisCheck remembers recently opened device collection files up to the configured
number (the default is 10). Click on a recent file to open it. If a file no longer
exists, it will be greyed out.

Right-click on a recent file to open a context menu. This menu has items
to open the file and to remove the file from the list (this does not delete
the file).

The recent files panel has two buttons at the bottom:

+ *Remove non-existing files from list*. Remove the recent files that are
  greyed out.
+ *Remove all files from list*. Remove all recent files (empties the recent
  files list).

### Settings panel

Here you can customize program behavior, configure defaults for some device
properties and configure which columns will be visible by default for a new
device collection.

The settings panel has one button at the bottom:

+ *Reset to defaults*. Reset all settings to their defaults.

### Devices tab

#### Ribbon buttons: Devices section

+ *Add device* (Insert). Add a FRITZ!Box, FRITZ!Powerline or FRITZ!WLAN device. Firmware
  update information for these devices is retrieved from the AVM JUIS service.
+ *Add DECT* (Ctrl+Insert). Add a FRITZ!DECT device. Firmware update information for DECT devices
  is retrieved from the AVM CATI service.
+ *Delete* (Delete). Remove the selected device(s) from the device collection.
+ *Edit* (Ctrl+E). Edit the selected device.
+ *Copy* (Ctrl+C). Add a device that is a copy of the selected device.
+ *Select all* (Ctrl+A). Select all devices in the device collection.
+ *Select none* (Ctrl+N). Deselect all devices in the device collection.

#### Ribbon buttons: Firmware update section

+ *Find updates* (Ctrl+F). Query the appropriate AVM update information services for the
  selected devices.
+ *Copy URLs* (Ctrl+U). Copy the image download URLs for the selected device(s) to the clipboard.
+ *Download firmware* (Ctrl+D). Download the firmware image for the selected device using the
  built-in download engine.
+ *View info* (Ctrl+I). View the release notes for the selected device (if available) using
  the default web browser (not available for FRITZ!DECT devices).
+ *Make current* (Ctrl+M). Make the updated version the current firmware version for the
  selected device(s) and clear the update information.
+ *Clear updates* (Shift+U)	. Clear the update information for the seleted device(s).

**Note:** Buttons are enabled if the associated action is meaningful. For example, the
*View info* button is only enabled if the update information service supplied a URL
for information about the update for the selected device.

#### Data table

The table shows the devices in the collection (one device per row). Each column shows a
device property. Some columns are hidden by default. Click on a device to select only that
device. You can select multiple devices (ctrl-click on a device to select/unselect it).
Double-click on a device to edit it.

Left click on a column header to sort the table by that column. Left click again to
reverse the sort direction. Right-click on a column header to open a context menu. The
context menu contains the column list. Visible columns have a checkmark. Click on a
column item to hide/unhide the column. You cannot hide the *Device name* column. 

**Note:** JuisCheck v2 stores column visibility in the collection XML file so that you can
tailor the view of each device collection. Hiding or unhiding a column will therefore mark
the data as modified. You can configure the default column visibility for new collections
in the settings panel.

### Device dialogs

These dialogs open when you click the *Add Device*, *Add DECT* or *Edit* button.
Input fields are checked for valid input. A field with invalid input will have a
red border. In this case the field has a tooltip that explains the reason for this.
If any field has invalid input, you cannot click the *OK* button.

For DECT devices you must select a device in the same device collection as a DECT
base (for example a FRITZ!Box 7590). The FRITZ!OS version of the DECT base is used
in update queries to the CATI service.

For non-DECT devices you can optionally select another device in the same device
collection as a Mesh master. The information of the Mesh Master is used to augment
the update query. At the moment of writing it is unknown if this information is
actually used by the JUIS service.

For non-DECT devices you can also supply a network address (IP address or DNS name)
and click the *Query* button. JuisCheck will then populate the device properties
from the *juis_boxinfo.xml* or *jason_boxinfo.xml* file retrieved from the device.

## Language selection

The primary user interface language is English. If addtional languages are installed
(*JuisCheck.resources.dll* files in per language subfolders) you can select the user
interface language in the settings panel. The default is automatic language selection.

Currently only an Italian localization is available (by *bovirus*).
<<<<<<< HEAD

## Portable mode

You can use JuisCheck in *portable mode* (e.g. to run it from a thumb drive) by renaming
the program to *JuisCheckPortable.exe*. Do not rename the *JuisCheck.resource.dll* files
in any per language subfolder! Portable mode has the following effects:

+ Settings are not saved when JuisCheck exits (leave no traces behind). All settings are
  default at every program start. As a result the last document directory, the last
  download directory and recent files will not be remembered across program sessions
  (but are remembered as long as the program is running).

+ Settings that have only effect after a program restart are disabled.

+ The user interface language is always selected automatically.

+ The default document directory and the default download directory will be the program
  directory instead of the Documents directory of the user.

+ A collection file *default.xml* will be loaded at startup if present in the program
  directory.
=======

## Portable mode

You can use JuisCheck in *portable mode* (e.g. to run it from a thumb drive) by renaming
the program to *JuisCheckPortable.exe*. Do not rename the *JuisCheck.resource.dll* files
in any per language subfolder! Portable mode has the following effects:

+ Settings are not saved when JuisCheck exits (leave no traces behind). All settings are
  default at every program start. As a result the last document directory, the last
  download directory and recent files will not be remembered across program sessions
  (but are remembered as long as the program is running).

+ Settings that have only effect after a program restart are disabled.

+ The user interface language is always selected automatically.

+ The default document directory and the default download directory will be the program
  directory instead of the Documents directory of the user.
>>>>>>> 40d2bc6790ffee00a5a34b81b426510fe9ef19a2


# Device collection files

Device collections are stored in XML files. The [examples](examples) folder contains
a number of device collections that are provided as a courtesy. Despite considerable
efforts from the author you must not assume that the data is either complete or fully
correct.


# Privacy

JuisCheck does not collect any information (neither personal nor otherwise) for
analysis by the author or third parties. The device information that you enter is
sent to the AVM update information services as part of update queries.
