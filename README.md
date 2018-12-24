# JuisCheck for Windows

JuisCheck for Windows is a tool to query the AVM update information services
(JUIS and CATI) for device firmware updates. JuisCheck for Windows supports
all types of AVM devices (FRITZ!Box, FRITZ!DECT, FRITZ!Powerline and FRITZ!WLAN).

## Using JuisCheck for Windows

### Starting JuisCheck

+ Run *JuisCheck.exe* in the *bin\Release* folder.
+ Add your devices.
+ Select one or more devices and click *Find Updates*.

### Ribbon menu buttons

Devices section

+ *Add Device*. Add a FRITZ!Box, FRITZ!Powerline or FRITZ!WLAN device. Firmware
  update information for these devices is retrieved from the AVM JUIS service.
+ *Add DECT*. Add a FRITZ!DECT device. Firmware update information for DECT devices
  is retrieved from the AVM CATI service.
+ *Delete*. Remove the selected devices from the device collection.
+ *Edit*. Edit the selected device.
+ *Select All*. Select all devices in the device collection.
+ *Select None*. Unselect all devices in the device collection.

Firmware Update section

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

### Backstage menu buttons

+ *Save* (Ctrl+S). Save the device collection to the file from which it was loaded.
+ *Save As...* (Ctrl+Alt+S). Save the device collection to a different file.
+ *Open...* (Ctrl+O). Load a device collection from a file.
+ *Close* (Ctrl+W). Close the device collection. If the data is modified you will
  be prompted if you want to save the data.
+ *Recent*. Opens the Recent Files panel.
+ *Settings*. Open the Settings panel. Among others you can set default
  values for some device properties to facilitate data entry.
+ *About*. Display program information.
+ *Exit*. Exit the program. If the data is modified you will be prompted if you
  want to save the data.

## Device dialogs

These dialogs open when you click the *Add Device*, *Add DECT* or *Edit* button.
Input fields are checked for valid input. An input field with invalid input will
have a red border. In this case the input field has a tooltip that explains the
reason for this. If any field has invalid input, you cannot click the *OK* button.

If the device is a device that uses the JUIS service (all devices except DECT),
you can supply a network address (IP address or DNS name) and click the *Query*
button. JuisCheck for Windows will then populate the device properties from the
*juis_boxinfo.xml* file retrieved from the device.

## Device collection files

Device collections are stored in XML files. The *examples* folder contains a number
of device collections that are provided as a courtasy. Despite considerable efforts
from the author you must not assume that the data is either complete or fully correct.

## Miscellaneous

### Compatibility

JuisCheck for Windows has been tested on Windows 7 and Windows 10. The tool will
probably also run on older Windows versions with .NET Framework 4.

### Privacy

JuisCheck for Windows does not collect any information (neither personal nor otherwise).

### Rebuilding from source

JuisCheck for Windows has been built using Visual Studio 2017. The Vitevic Assembly
Embedder extension is used to embed 3rd party library DLLs as resources into the
executable.

If you want/need to build the solution without the Vitevic Assembly Embedder extension,
you must set *Copy Local* to *True* for the following references:

+ ControlzEx
+ Fluent
+ Muon.DotNetExtensions
+ Muon.Windows
+ Muon.Windows.Controls
