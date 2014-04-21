using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace EpLibrary.cs
{
    /// <summary>
    /// Crypt type
    /// </summary>
    public enum CryptType
    {
        /// <summary>
        /// Encryption
        /// </summary>
        Encrypt,
        /// <summary>
        /// Decryption
        /// </summary>
        Decrypt
    }

    /// <summary>
    /// Crypt algorithm type
    /// </summary>
    public enum CryptAlgo
    {
        /// <summary>
        /// Rijndael Algorithm
        /// </summary>
        Rijndael,
        /// <summary>
        ///  AES Algorithm
        /// </summary>
        Aes,

    }

    /// <summary>
    /// This is a class for Crypt Class
    /// </summary>
    public class Crypt
    {
                /// <summary>
        /// Encrypt/Decypt the given cryptData string with the given password
        /// </summary>
        /// <param name="cryptData">string data to encrypt</param>
        /// <param name="cryptPwd">password string</param>
        /// <param name="cryptType">crypt type</param>
        /// <returns>encrypted/decrypted data</returns>
        public static string GetCrypt(CryptAlgo algoType,string cryptData, string cryptPwd, CryptType cryptType)
        {
            string retString = null;
            switch (algoType)
            {
                case CryptAlgo.Aes:
                    retString = AesCrypt.GetCrypt(cryptData, cryptPwd, null, cryptType);
                    break;
                case CryptAlgo.Rijndael:
                    retString = RijndaelCrypt.GetCrypt(cryptData, cryptPwd, null, cryptType);
                    break;
            }
            return retString;
        }

        /// <summary>
        /// Encrypt/Decypt the given cryptData string with the given password
        /// </summary>
        /// <param name="cryptData">string data to encrypt</param>
        /// <param name="cryptPwd">password string</param>
        /// <param name="cryptType">crypt type</param>
        /// <returns>encrypted/decrypted data</returns>
        /// <remarks>if keySalt is null, then default keySalt is used</remarks>
        public static string GetCrypt(CryptAlgo algoType, string cryptData, string cryptPwd, byte[] keySalt, CryptType cryptType)
        {
            string retString = null;
            switch (algoType)
            {
                case CryptAlgo.Aes:
                    retString = AesCrypt.GetCrypt(cryptData, cryptPwd, keySalt, cryptType);
                    break;
                case CryptAlgo.Rijndael:
                    retString = RijndaelCrypt.GetCrypt(cryptData, cryptPwd, keySalt, cryptType);
                    break;
            }
            return retString;
        }

        /// <summary>
        /// Encrypt/Decypt the given cryptData with the given password
        /// </summary>
        /// <param name="cryptData">data to crypt</param>
        /// <param name="cryptPwd">password string</param>
        /// <param name="cryptType">crypt type</param>
        /// <returns>encrypted/decrypted data</returns>
        public static byte[] GetCrypt(CryptAlgo algoType, byte[] cryptData, string cryptPwd, CryptType cryptType)
        {
            byte[] retBytes = null;
            switch (algoType)
            {
                case CryptAlgo.Aes:
                    retBytes = AesCrypt.GetCrypt(cryptData, cryptPwd, null, cryptType);
                    break;
                case CryptAlgo.Rijndael:
                    retBytes = RijndaelCrypt.GetCrypt(cryptData, cryptPwd, null, cryptType);
                    break;
            }
            return retBytes;
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
        public static byte[] GetCrypt(CryptAlgo algoType, byte[] cryptData, string cryptPwd, byte[] keySalt, CryptType cryptType)
        {
            byte[] retBytes = null;
            switch(algoType)
            {
                case CryptAlgo.Aes:
                retBytes= AesCrypt.GetCrypt(cryptData,cryptPwd,keySalt,cryptType);
                break;
                case CryptAlgo.Rijndael:
                retBytes= RijndaelCrypt.GetCrypt(cryptData,cryptPwd,keySalt,cryptType);
                break;
            }
            return retBytes;
        }
        /// <summary>
        /// Create random salt with given length
        /// </summary>
        /// <param name="length">length of salt bytes</param>
        /// <returns>randomly created salt</returns>
        public static byte[] CreateRandomSalt(int length)
        {
            // Create a buffer 
            byte[] randBytes;

            if (length >= 1)
            {
                randBytes = new byte[length];
            }
            else
            {
                randBytes = new byte[1];
            }

            // Create a new RNGCryptoServiceProvider.
            RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();

            // Fill the buffer with random bytes.
            rand.GetBytes(randBytes);

            // return the bytes. 
            return randBytes;
        }
    }
}
