using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Microsoft.Win32;
using System.Collections;

namespace UWPDefaults
{
    internal class Program
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern int RegLoadAppKeyA(
        string lpFile,
        out SafeRegistryHandle phkResult,
        int samDesired,
        int dwOptions,
        int Reserved);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern int RegCloseKey(IntPtr hKey);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern int RegSetValueEx(
            IntPtr hKey,
            string lpValueName,
            int Reserved,
            uint dwType,
            byte[] lpData,
            uint cbData);

        [DllImport("advapi32.dll", CharSet = CharSet.Ansi, EntryPoint = "RegGetValueA")]
        public static extern int RegGetValue(
        IntPtr hKey,
        string lpSubKey,
        string lpValue,
        uint dwFlags,
        out uint lpType,
        System.Text.StringBuilder lpData,
        ref uint lpcbData);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern int RegUnLoadKey(IntPtr hKey, string lpSubKey);

        static void CheckSetting(string packageID, string valueName, string valueType, string valueData)
        {
            const string subKey = "LocalState";
            uint regBoolType = 0x5f5e10b;
            uint regStringType = 0x5f5e10c;
            uint regInt32Type = 0x5f5e104;
            SafeRegistryHandle hiveHandle;

            string hiveFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Packages", packageID, @"Settings\settings.dat");
            if (System.IO.File.Exists(hiveFile))
            {
                int result = RegLoadAppKeyA(hiveFile, out hiveHandle, 0x000F003F, 0, 0);
                if (result != 0)
                {
                    Console.WriteLine("Error loading hive: " + result);
                    return;
                }
                using (var baseKey = RegistryKey.FromHandle(hiveHandle))
                {
                    var sub1Key = baseKey.OpenSubKey(subKey,true);
                    IntPtr sub1KeyHandle = sub1Key.Handle.DangerousGetHandle();

                    uint lpType;
                    uint cbData=0;
                    result = RegGetValue(sub1KeyHandle, "", valueName, 0x0000ffff, out lpType, null, ref cbData);
                    if (result != 0)
                    {
                        result = 0;
                        if (valueType == "REG_BOOL")
                        {
                            byte[] binaryValue = new byte[9];
                            if (valueData == "True")
                            {
                                binaryValue[0] = 1;
                            }
                            else
                            {
                                binaryValue[0] = 0;
                            }
                            GetTimeStamp(binaryValue, 1);
                            result = RegSetValueEx(sub1KeyHandle, valueName, 0, regBoolType, binaryValue, (uint)binaryValue.Length);
                        }
                        else if (valueType == "REG_SZ")
                        {
                            byte[] stringBuffer = Encoding.Unicode.GetBytes(valueData + '\u0000');
                            byte[] binaryValue = new byte[stringBuffer.Length + 8];
                            Buffer.BlockCopy(stringBuffer, 0, binaryValue, 0, stringBuffer.Length);
                            GetTimeStamp(binaryValue, stringBuffer.Length);
                            result = RegSetValueEx(sub1KeyHandle, valueName, 0, regStringType, binaryValue, (uint)binaryValue.Length);
                        }
                        else if (valueType == "REG_DWORD")
                        {
                            if (valueData.Length == 8)
                            {
                                UInt32 x = Convert.ToUInt32("0x"+valueData, 16);
                                byte[] binaryValue = new byte[12];
                                BitConverter.GetBytes(x).CopyTo(binaryValue, 0);
                                GetTimeStamp(binaryValue, 4);
                                result = RegSetValueEx(sub1KeyHandle, valueName, 0, regInt32Type, binaryValue, (uint)binaryValue.Length);
                            }
                        }
                        else
                        {
                            //invalid Registry value type
                            result = -1;
                        }

                        if (result == 0)
                        {
                            Console.WriteLine("Value '{0}' successfully written to package '{1}'", valueName, packageID);
                        }
                        else
                        {
                            Console.WriteLine("Error 0x{0:X8} writing '{1}' to package '{2}'", result, valueName, packageID);
                        }

                    }
                    else
                    {
                        Console.WriteLine("Value '{0}' already exists in package '{1}'", valueName, packageID);
                    }
                }
            }
            else
            {
                Console.WriteLine("Settings file '{0}' doesn't exist.",hiveFile);
                return;
            }
        }

        private static void GetTimeStamp(byte[] binaryValue, int v)
        {
            DateTime now = DateTime.UtcNow;
            long fileTime = now.ToFileTime();
            byte[] byteArray = BitConverter.GetBytes(fileTime);
            Buffer.BlockCopy(byteArray, 0, binaryValue, v, 8);
        }

        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine(@"Syntax: UWPDefaults <UWP Settings File>");
                Console.WriteLine(@"Example: UWPDefaults c:\temp\UWPSettings.INI");
                return;
            }
            string settingsFile = args[0];

            if (System.IO.File.Exists(settingsFile))
            {
                string[] list=System.IO.File.ReadAllLines(settingsFile);
                foreach (string line in list)
                {
                    string[] a=line.Split('|');
                    if (a.Length == 4)
                    {
                        CheckSetting(a[0], a[1], a[2], a[3]);   
                    }
                    else
                    {
                        Console.WriteLine(@"Invalid file format");
                        Console.WriteLine(@"Microsoft.WindowsNotepad_8wekyb3d8bbwe|GhostFile|REG_BOOL|False");
                        return;
                    }
                }
            }
        }
    }

}
