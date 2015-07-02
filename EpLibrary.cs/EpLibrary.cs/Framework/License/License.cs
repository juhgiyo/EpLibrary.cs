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
        /// license crypt password
        /// </summary>
        private String m_password;
        /// <summary>
        /// license type
        /// </summary>
        private LicenseType m_licenseType;


        /// <summary>
        /// Default constructor
        /// </summary>
        public License(String password,LicenseType licenseType)
        {
            m_password = password;
            m_licenseType = licenseType;
        }
        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="b">the object to copy from</param>
        public License(License b)
        {
            m_password = b.m_password;
            m_licenseType = b.m_licenseType;
        }

        /// <summary>
        /// Create License and return as string
        /// </summary>
        /// <param name="macAddress">mac address which licensed</param>
        /// <param name="dateTime">expiration date</param>
        /// <returns></returns>
        public String CreateLicense(String macAddress=null, DateTime? expirationDate=null)
        {
            String licenseData = "";
            if ((m_licenseType & LicenseType.MacAddress) == LicenseType.MacAddress)
            {
                licenseData += Crypt.GetCrypt(CryptAlgo.Rijndael, "macaddress:" + macAddress, m_password, CryptType.Encrypt);
                licenseData += "\r\n";
            }
            if ((m_licenseType & LicenseType.ExpireDate) == LicenseType.ExpireDate && expirationDate.HasValue)
            {
                licenseData += Crypt.GetCrypt(CryptAlgo.Rijndael, "expirationdate:" + expirationDate.Value.ToString("d"), m_password, CryptType.Encrypt);
                licenseData += "\r\n";
            }
            return licenseData;
        }

        /// <summary>
        /// Check License with given license string
        /// </summary>
        /// <param name="licenseData">license string</param>
        /// <returns>result of checking</returns>
        public LicenseResult CheckLicense(String licenseData)
        {
            String licensedMacAddress = null;
            DateTime? expirationDate=null;
            String[] lines=Regex.Split(licenseData,"\r\n");
            foreach (String line in lines)
            {
                String decryptedData = Crypt.GetCrypt(CryptAlgo.Rijndael, line, m_password, CryptType.Decrypt);
                if (decryptedData.Contains("macaddress:"))
                {
                    decryptedData.Remove(0, "macaddress:".Length);
                    licensedMacAddress = decryptedData;
                }
                else if (decryptedData.Contains("expirationdate:"))
                {
                    decryptedData.Remove(0, "expirationdate:".Length);
                    expirationDate = Convert.ToDateTime(decryptedData);
                }
            }
            if ((m_licenseType & LicenseType.MacAddress) == LicenseType.MacAddress)
            {
                if (licensedMacAddress == null)
                    return LicenseResult.CorruptedLicense;
                if (!checkMacAddress(licensedMacAddress))
                    return LicenseResult.MacAddressMisMatch;
            }

            if ((m_licenseType & LicenseType.ExpireDate) == LicenseType.ExpireDate)
            {
                if (!expirationDate.HasValue)
                    return LicenseResult.CorruptedLicense;
                if (!checkDate(expirationDate.Value))
                    return LicenseResult.Expired;
            }
            return LicenseResult.Success;
        }

        /// <summary>
        /// Check if current mac address matches licensed mac address
        /// </summary>
        /// <param name="licenesedMacAddress">licensed mac address</param>
        /// <returns>true if matched otherwise false</returns>
        private bool checkMacAddress(String licenesedMacAddress)
        {
            List<String> macAddresses = new List<String>();

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
        private bool checkDate(DateTime expirationDate)
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
