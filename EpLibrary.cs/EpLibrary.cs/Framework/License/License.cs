/*! 
@file License.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief License Interface
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

A License Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace EpLibrary.cs
{
    /// <summary>
    /// License type
    /// </summary>
    [Flags]
    public enum LicenseType
    {
        /// <summary>
        /// Mac Address Checking
        /// </summary>
        MacAddress=0x1,
        /// <summary>
        /// Date Checking
        /// </summary>
        ExpireDate=0x2
    }

    /// <summary>
    /// License Result
    /// </summary>
    public enum LicenseResult
    {
        /// <summary>
        /// Success
        /// </summary>
        Success,
        /// <summary>
        /// Date Expired
        /// </summary>
        Expired,
        /// <summary>
        /// Mac Address Mismatch
        /// </summary>
        MacAddressMisMatch,
        /// <summary>
        /// License Corrupted
        /// </summary>
        CorruptedLicense
    }
    /// <summary>
    /// A class that handles the license.
    /// </summary>
    public class License
    {
        /// <summary>
        /// Create License and return as string
        /// </summary>
        /// <param name="macAddress">mac address which licensed</param>
        /// <param name="dateTime">expiration date</param>
        /// <returns></returns>
        public static String CreateLicense(String password,LicenseType licenseType,String macAddress=null, DateTime? expirationDate=null)
        {
            String licenseData = "";
            if ((licenseType & LicenseType.MacAddress) == LicenseType.MacAddress)
            {
                licenseData += Crypt.GetCrypt(CryptAlgo.Rijndael, "macaddress:" + macAddress, password, CryptType.Encrypt);
                licenseData += "\r\n";
            }
            if ((licenseType & LicenseType.ExpireDate) == LicenseType.ExpireDate && expirationDate.HasValue)
            {
                licenseData += Crypt.GetCrypt(CryptAlgo.Rijndael, "expirationdate:" + expirationDate.Value.ToString("d"), password, CryptType.Encrypt);
                licenseData += "\r\n";
            }
            return licenseData;
        }

        /// <summary>
        /// Return encrypted Mac Address
        /// </summary>
        /// <param name="password">password</param>
        /// <param name="licenseData">license data</param>
        /// <returns>decrypted Mac Address</returns>
        public static String GetDecryptedMacAddr(String password, String licenseData)
        {
            String[] lines = Regex.Split(licenseData, "\r\n");
            String licensedMacAddress = null;
            foreach (String line in lines)
            {
                String decryptedData = Crypt.GetCrypt(CryptAlgo.Rijndael, line, password, CryptType.Decrypt);
                if (decryptedData!=null && decryptedData.Contains("macaddress:"))
                {
                    decryptedData=decryptedData.Remove(0, "macaddress:".Length);
                    licensedMacAddress = decryptedData;
                }
            }
            return licensedMacAddress;
        }

        public static DateTime? GetDecryptedExpirationDate(String password, String licenseData)
        {
            DateTime? expirationDate = null;
            String[] lines = Regex.Split(licenseData, "\r\n");
            foreach (String line in lines)
            {
                String decryptedData = Crypt.GetCrypt(CryptAlgo.Rijndael, line, password, CryptType.Decrypt);
                if (decryptedData != null && decryptedData.Contains("expirationdate:"))
                {
                    decryptedData = decryptedData.Remove(0, "expirationdate:".Length);
                    expirationDate = Convert.ToDateTime(decryptedData);
                }
            }
            return expirationDate;
        }


        /// <summary>
        /// Check License with given license string
        /// </summary>
        /// <param name="licenseData">license string</param>
        /// <returns>result of checking</returns>
        public static LicenseResult CheckLicense(String password, LicenseType licenseType, String licenseData)
        {
            String licensedMacAddress = null;
            DateTime? expirationDate=null;
            String[] lines=Regex.Split(licenseData,"\r\n");
            foreach (String line in lines)
            {
                String decryptedData = Crypt.GetCrypt(CryptAlgo.Rijndael, line, password, CryptType.Decrypt);
                if (decryptedData != null && decryptedData.Contains("macaddress:"))
                {
                    decryptedData = decryptedData.Remove(0, "macaddress:".Length);
                    licensedMacAddress = decryptedData;
                }
                else if (decryptedData != null && decryptedData.Contains("expirationdate:"))
                {
                    decryptedData = decryptedData.Remove(0, "expirationdate:".Length);
                    expirationDate = Convert.ToDateTime(decryptedData);
                }
            }
            if ((licenseType & LicenseType.MacAddress) == LicenseType.MacAddress)
            {
                if (licensedMacAddress == null)
                    return LicenseResult.CorruptedLicense;
                if (!checkMacAddress(licensedMacAddress))
                    return LicenseResult.MacAddressMisMatch;
            }

            if ((licenseType & LicenseType.ExpireDate) == LicenseType.ExpireDate)
            {
                if (!expirationDate.HasValue)
                    return LicenseResult.CorruptedLicense;
                if (!checkDate(expirationDate.Value))
                    return LicenseResult.Expired;
            }
            return LicenseResult.Success;
        }

        /// <summary>
        /// Return the list of MAC Addresses
        /// </summary>
        /// <returns>the list of MAC Addresses</returns>
        public static List<String> GetMacAddress()
        {
            List<String> macAddresses = new List<String>();

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if(nic.GetPhysicalAddress().ToString().Length>0)
                    macAddresses.Add(nic.GetPhysicalAddress().ToString());
            }
            return macAddresses;
        }

        /// <summary>
        /// Check if current mac address matches licensed mac address
        /// </summary>
        /// <param name="licenesedMacAddress">licensed mac address</param>
        /// <returns>true if matched otherwise false</returns>
        private static bool checkMacAddress(String licenesedMacAddress)
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (licenesedMacAddress.CompareTo(nic.GetPhysicalAddress().ToString()) == 0)
                    return true;
            }
            return false;
        }



        /// <summary>
        /// Check if license is expired with current date
        /// </summary>
        /// <param name="expirationDate">expiration date</param>
        /// <returns>true if not expired otherwise false</returns>
        private static bool checkDate(DateTime expirationDate)
        {

            DateTime curTime= DateTime.Now;
            if (curTime.Year > expirationDate.Year)
	        {
		        return false;
	        }
            if (curTime.Year == expirationDate.Year)
	        {
                if (curTime.Month > expirationDate.Month)
                    return false;
                if (curTime.Month == expirationDate.Month)
		        {
                    if (curTime.Day > expirationDate.Day)
                        return false;
		        }
	        }
            return true;
        }

    }
}
