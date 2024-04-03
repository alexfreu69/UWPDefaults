# UWPDefaults

UWPDefaults opens the settings.dat registry hive from %localappdata%\Packages\\<PackageID\>\Settings and sets a default value when a setting has not been defined yet.
The settings are initialized when the user opens the app the first time. To predefine some values, UWPDefaults can be executed before that, when the user logs in.

Syntax: UWPDefaults c:\settings\UWPDefaults.ini

The ini file looks like this:

Microsoft.WindowsNotepad_8wekyb3d8bbwe|GhostFile|REG_BOOL|False
Microsoft.ScreenSketch_8wekyb3d8bbwe|AutoSaveCaptures|REG_BOOL|False
Microsoft.WindowsCalculator_8wekyb3d8bbwe|SelectedAppTheme|REG_SZ|Dark
Microsoft.WindowsCalculator_8wekyb3d8bbwe|Mode|REG_DWORD|00000001

The syntax for the REG_DWORD format is the same as in reg files.
If you see these hexadecimal bytes at the beginning: 21 43 65 87, you should write 87654321 without 0x in the ini file.

REG_BOOL only knows True and False

All entries are case sensitive!

For Notepad: The app always starts with an empty file.

For Snipping Tool: The screenshot is not saved automatically.

For the Calculator:\
 The app uses the Dark theme. Other values are Default and Light\
 Scientific is the calculation mode

The undocumented registry value types are:

REG_SZ	0x5f5e10c\
REG_BOOL	0x5f5e10b\
REG_DWORD	0x5f5e104

All values have the latest change timestamp in 8 byte FILETIME format appended.

To identify the value name, you can copy the settings file from the packages folder to a temporary folder, open Regedit as administratator and select HKEY_LOCAL_MACHINE.
Select the menu entry File|Load Hive... and select the settings.dat. Assign a temporary name and look for the subkey LocalState.
Ignore the last 8 bytes and look at the first bytes.
Don't forget to unload the registry hive again.

REG_BOOL is one byte. 00 = False, 01 = True
REG_STRING is a Unicode string with 00 00 appended.
REG_DWORD are four bytes.

