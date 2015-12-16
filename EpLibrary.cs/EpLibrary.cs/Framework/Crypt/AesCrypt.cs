/*! 
@file AesCrypt.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief AES Crypt Interface
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

A AesCrypt Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace EpLibrary.cs
{

    /// <summary>
    /// This is a class for AES Crypt Class
    /// </summary>
    public class AesCrypt
    {
        /// <summary>
        /// Encrypt/Decypt the given cryptData string with the given password
        /// </summary>
        /// <param name="cryptData">string data to encrypt</param>
        /// <param name="cryptPwd">password string</param>
        /// <param name="cryptType">crypt type</param>
        /// <returns>encrypted/decrypted data</returns>
        public static string GetCrypt(string cryptData, string cryptPwd, CryptType cryptType)
        {
            return GetCrypt(cryptData, cryptPwd, null, cryptType);
        }

        /// <summary>
        /// Encrypt/Decypt the given cryptData string with the given password
        /// </summary>
        /// <param name="cryptData">string data to encrypt</param>
        /// <param name="cryptPwd">password string</param>
        /// <param name="cryptType">crypt type</param>
        /// <returns>encrypted/decrypted data</returns>
        /// <remarks>if keySalt is null, then default keySalt is used</remarks>
        public static string GetCrypt(string cryptData, string cryptPwd, byte[] keySalt, CryptType cryptType)
        {
            try
            {
                byte[] cryptBytes = null;
                switch (cryptType)
                {
                    case CryptType.Encrypt:
                        cryptBytes = Encoding.Unicode.GetBytes(cryptData);
                        break;
                    case CryptType.Decrypt:
                        cryptBytes = Convert.FromBase64String(cryptData);
                        break;
                }

                byte[] retData = GetCrypt(cryptBytes, cryptPwd, keySalt, cryptType);
                switch (cryptType)
                {
                    case CryptType.Encrypt:
                        return Convert.ToBase64String(retData);
                    case CryptType.Decrypt:
                        return Encoding.Unicode.GetString(retData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " >" + ex.StackTrace);
            }
            return null;
        }

        /// <summary>
        /// Encrypt/Decypt the given cryptData with the given password
        /// </summary>
        /// <param name="cryptData">data to crypt</param>
        /// <param name="cryptPwd">password string</param>
        /// <param name="cryptType">crypt type</param>
        /// <returns>encrypted/decrypted data</returns>
        public static byte[] GetCrypt(byte[] cryptData, string cryptPwd, CryptType cryptType)
        {
            return GetCrypt(cryptData, cryptPwd, null, cryptType);
        }

        /// <summary>
        /// Encrypt/Decypt the given cryptData with the given password
        /// </summary>
        /// <param name="cryptData">data to crypt</param>
        /// <param name="offset">offset of cryptData for crypt</param>
        /// <param name="count">length for crypt</param>
        /// <param name="cryptPwd">password string</param>
        /// <param name="cryptType">crypt type</param>
        /// <returns>encrypted/decrypted data</returns>
        public static byte[] GetCrypt(byte[] cryptData, int offset, int count, string cryptPwd, CryptType cryptType)
        {
            return GetCrypt(cryptData,offset,count, cryptPwd, null, cryptType);
        }

        /// <summary>
        /// Encrypt/Decypt the given cryptData with the given password
        /// </summary>
        /// <param name="cryptData">data to crypt</param>
        /// <param name="cryptPwd">password string</param>
        /// <param name="keySalt">salt string</param>
        /// <param name="cryptType">crypt type</param>
        /// <returns>encrypted/decrypted data</returns>
        /// <remarks>if keySalt is null, then default keySalt is used</remarks>
        public static byte[] GetCrypt(byte[] cryptData, string cryptPwd, byte[] keySalt, CryptType cryptType)
        {
            return GetCrypt(cryptData, 0, cryptData.Length, cryptPwd, keySalt, cryptType);
        }


        /// <summary>
        /// Encrypt/Decypt the given cryptData with the given password
        /// </summary>
        /// <param name="cryptData">data to crypt</param>
        /// <param name="offset">offset of cryptData for crypt</param>
        /// <param name="count">length for crypt</param>
        /// <param name="cryptPwd">password string</param>
        /// <param name="keySalt">salt string</param>
        /// <param name="cryptType">crypt type</param>
        /// <returns>encrypted/decrypted data</returns>
        /// <remarks>if keySalt is null, then default keySalt is used</remarks>
        public static byte[] GetCrypt(byte[] cryptData,int offset, int count, string cryptPwd, byte[] keySalt, CryptType cryptType)
        {
            if (keySalt == null)
                keySalt = new byte[] { 0x54, 0x81, 0x45, 0x4A, 0x3B, 0x5E, 0x52, 0x15, 0x86, 0x5A, 0x40, 0x3B, 0xB4 };
            //PasswordDeriveBytes pwdBytes = new PasswordDeriveBytes(cryptPwd, keySalt);
            Rfc2898DeriveBytes pwdBytes = new Rfc2898DeriveBytes(cryptPwd, keySalt);

            AesManaged crypAlg = new AesManaged();
            crypAlg.Key = pwdBytes.GetBytes(32);
            crypAlg.IV = pwdBytes.GetBytes(16);

            try
            {
                ICryptoTransform cryptoTranform = (cryptType == CryptType.Encrypt) ? crypAlg.CreateEncryptor() : crypAlg.CreateDecryptor();

                using (MemoryStream memStream = new MemoryStream())
                {
                    using (CryptoStream cryptStream = new CryptoStream(memStream, cryptoTranform, CryptoStreamMode.Write))
                    {
                        cryptStream.Write(cryptData, offset, count);
                        cryptStream.FlushFinalBlock();
                        return memStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " >" + ex.StackTrace);
            }
            return null;
        }

   
    }
}
