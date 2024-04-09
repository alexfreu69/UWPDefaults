# UWPDefaults

UWPDefaults opens the settings.dat registry hive from %localappdata%\Packages\\<PackageID\>\Settings and sets a default value when a setting has not been defined yet.
The settings are initialized when the user opens the app the first time. To predefine some values, UWPDefaults can be executed before that, when the user logs in.

Syntax: UWPDefaults c:\settings\UWPDefaults.ini

The ini file looks like this:

Microsoft.WindowsNotepad_8wekyb3d8bbwe|GhostFile|REG_BOOL|False
Microsoft.ScreenSketch_8wekyb3d8bbwe|AutoSaveCaptures|REG_BOOL|False
Microsoft.WindowsCalculator_8wekyb3d8bbwe|SelectedAppTheme|REG_SZ|Dark
Microsoft.WindowsCalculator_8wekyb3d8bbwe|Mode|REG_DWORD|00000001
Microsoft.WindowsCalculator_8wekyb3d8bbwe|Test|REG_QWORD|FFFFFFFF00000001
Microsoft.WindowsCalculator_8wekyb3d8bbwe|Test2|REG_BINARY|25,00,53,00,79,00,73,00,74,00,65,00,6d,00,52,00,6f
Microsoft.WindowsCalculator_8wekyb3d8bbwe|Test3|REG_EXPAND_SZ|%SystemRoot%\system32\settings.dat
Microsoft.WindowsCalculator_8wekyb3d8bbwe|Test4|REG_WORD|0001
Microsoft.WindowsCalculator_8wekyb3d8bbwe|Test5|REG_DWORD_BIG_ENDIAN|87654321
Microsoft.WindowsCalculator_8wekyb3d8bbwe|Test6|REG_QWORD_BIG_ENDIAN|FEDCBA9876543210
Microsoft.WindowsCalculator_8wekyb3d8bbwe|Test7|0x5f5e105|00000000
microsoft.windowscommunicationsapps_8wekyb3d8bbwe|ConfigSettings\IsCalendarBetaDefaultOn|0x5f5e105|00000000

The syntax for the REG_DWORD format is the same as in reg files.
If you see these hexadecimal bytes at the beginning: 21 43 65 87, you should write 87654321 without 0x in the ini file.

REG_BOOL only knows True and False

All entries are case sensitive!

For Notepad: The app always starts with an empty file.

For Snipping Tool: The screenshot is not saved automatically.

For the Calculator:\
 The app uses the Dark theme. Other values are Default and Light\
 Scientific is the calculation mode

The Test values have no effect.

The undocumented registry value types are:

REG_SZ	0x5f5e10c\
REG_BOOL	0x5f5e10b\
REG_DWORD	0x5f5e104\
REG_QWORD   0x5f5e106\
REG_BINARY   0x5f5e10d\
REG_WORD     0x5f5e103

Unknown purpose:

0x5f5e105 (same as REG_DWORD)\
0x5f5e107 (same as REG_QWORD - maybe timestamp?)\
0x5f5e109 maybe big endian QWORD\
0x5f5e10e (same as REG_QWORD - maybe timestamp?)\
0x5f5e114\


All values have the latest change timestamp in 8 byte FILETIME format appended.

To identify the value name, you can copy the settings file from the packages folder to a temporary folder, open Regedit as administratator and select HKEY_LOCAL_MACHINE.
Select the menu entry File|Load Hive... and select the settings.dat. Assign a temporary name and look for the subkey LocalState.
Ignore the last 8 bytes and look at the first bytes.
Don't forget to unload the registry hive again.

REG_BOOL is one byte. 00 = False, 01 = True\
REG_STRING is a Unicode string with 00 00 appended.\
REG_DWORD are four bytes.\
REG_QWORD are eight bytes.\

