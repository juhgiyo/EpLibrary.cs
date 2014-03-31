using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace EpLibrary.cs
{
    /// <summary>
    /// This is a class for Registry Processing Class
    /// </summary>
    public class RegistryHelper
    {
        /// <summary>
        /// Set the given registry string data to given registry name
        /// </summary>
        /// <param name="key">the registry hive ex. (HKEY_LOCAL_MACHINE)</param>
        /// <param name="subKey">the subkey within the registry mode ex. ("SOFTWARE\\WINDOWS\\")</param>
        /// <param name="regName">the name of the registry to write the data</param>
        /// <param name="regData">the string data to write</param>
        /// <returns>true if successful, otherwise false</returns>
		public static bool SetRegistryData(RegistryHive key,String subKey,String regName,Object regData)
        {
            try
            {
                RegistryKey registry = RegistryKey.OpenBaseKey(key, RegistryView.Default);
                registry = registry.CreateSubKey(subKey);
                registry.SetValue(regName, regData);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///  Get the given registry string data of given registry name
        /// </summary>
        /// <param name="key">the registry hive ex. (HKEY_LOCAL_MACHINE)</param>
        /// <param name="subKey">the subkey within the registry hive ex. ("SOFTWARE\\WINDOWS\\")</param>
        /// <param name="regName">the name of the registry to read the data</param>
        /// <param name="retRegData">the data read</param>
        /// <returns>true if successful, otherwise false</returns>
        public static bool GetRegistryData(RegistryHive key, String subKey, String regName, ref Object retRegData)
        {
            try
            {
                RegistryKey registry = RegistryKey.OpenBaseKey(key, RegistryView.Default);
                registry = registry.OpenSubKey(subKey);
                retRegData = registry.GetValue(regName);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Delete the given registry value
        /// </summary>
        /// <param name="key">the registry hive</param>
        /// <param name="subkey">the subkey within the registry hive</param>
        /// <param name="regName">the registry value to be deleted</param>
        public static void DeleteRegistryValue(RegistryHive key, String subkey, String regName)
        {
            try
            {
                RegistryKey registry = RegistryKey.OpenBaseKey(key, RegistryView.Default);
                registry = registry.OpenSubKey(subkey);
                registry.DeleteValue(regName);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Delete the given registry key
        /// </summary>
        /// <param name="key">the registry hive</param>
        /// <param name="subkey">the subkey within the registry hive to be deleted</param>
        public static void DeleteRegistryKey(RegistryHive key, String subkey)
        {
            try
            {
                RegistryKey registry = RegistryKey.OpenBaseKey(key, RegistryView.Default);
                registry = registry.OpenSubKey(subkey);
                registry.DeleteSubKeyTree(subkey);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Return the given registry's RegistryValueKind
        /// </summary>
        /// <param name="key">the registry hive</param>
        /// <param name="subkey">the subkey within the registry hive</param>
        /// <param name="regName">the name of the registry to get the RegistryValueType</param>
        /// <returns></returns>
        public static RegistryValueKind GetRegistryValueKind(RegistryHive key, String subkey, String regName)
        {
            try
            {
                RegistryKey registry = RegistryKey.OpenBaseKey(key, RegistryView.Default);
                registry = registry.OpenSubKey(subkey);
                return registry.GetValueKind(regName);
            }
            catch
            {
                return RegistryValueKind.Unknown;
            }
        }
    }
}
