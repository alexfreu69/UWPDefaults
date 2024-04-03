# UWPDefaults

UWPDefaults opens the settings.dat registry hive from %localappdata%\Packages\\<PackageID\>\Settings and sets a default value when a setting has not been defined yet.
The settings are initialized when the user opens the app the first time. To predefine some values, UWPDefaults can be executed before that, when the user logs in.

Syntax: UWPDefaults c:\settings\UWPDefaults.ini

The ini file looks like this:

Microsoft.WindowsNotepad_8wekyb3d8bbwe|GhostFile|REG_BOOL|False
Microsoft.ScreenSketch_8wekyb3d8bbwe|AutoSaveCaptures|REG_BOOL|False


For Notepad: The app always starts with an empty file.

For Snipping Tool: The screenshot is not saved automatically.

Currently only REG_BOOL is supported.

The undocumented registry value types are:

REG_SZ	0x5f5e10c

REG_BOOL	0x5f5e10b

REG_DWORD	0x5f5e104

All values have the latest change timestamp in 8 byte FILETIME format appended.
