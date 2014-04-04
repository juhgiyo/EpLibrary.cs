/*! 
@file RegistryHelper.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief RegistryHelper Interface
@version 2.0

@section LICENSE

The MIT License (MIT)

Copyright (c) 2014 Woong Gyu La <juhgiyo@gmail.com>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

@section DESCRIPTION

A RegistryHelper Class.

*/
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " >" + ex.StackTrace);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " >" + ex.StackTrace);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " >" + ex.StackTrace);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " >" + ex.StackTrace);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " >" + ex.StackTrace);
                return RegistryValueKind.Unknown;
            }
        }
    }
}
