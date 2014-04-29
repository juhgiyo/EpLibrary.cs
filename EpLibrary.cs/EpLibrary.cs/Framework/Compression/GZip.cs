/*! 
@file GZip.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief GZip Interface
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

A GZip Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace EpLibrary.cs
{
    public class GZip
    {

        /// <summary>
        /// Compress the given gzipData string
        /// </summary>
        /// <param name="gzipData">string data to compress</param>
        /// <returns>compressed data</returns>
        public static string Compress(string gzipData)
        {
            try
            {
                byte[] cryptBytes = Encoding.Unicode.GetBytes(gzipData); ;
                byte[] retData = Compress(cryptBytes);
                return Convert.ToBase64String(retData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " >" + ex.StackTrace);
            }
            return null;
        }

        /// <summary>
        /// Decompress the given gzipData string
        /// </summary>
        /// <param name="gzipData">string data to decompress</param>
        /// <returns>decompressed data</returns>
        public static string Decompress(string cryptData)
        {
            try
            {
                byte[] cryptBytes = Convert.FromBase64String(cryptData);
                byte[] retData = Decompress(cryptBytes);
                return Encoding.Unicode.GetString(retData);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " >" + ex.StackTrace);
            }
            return null;
        }

        /// <summary>
        /// Decompress the given byte array
        /// </summary>
        /// <param name="gzip">byte array to decompress</param>
        /// <returns>decompressed byte array</returns>
        public static byte[] Decompress(byte[] gzip)
        {
            // Create a GZIP stream with decompression mode.
            // ... Then create a buffer and write into while reading from the GZIP stream.
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }

        /// <summary>
        /// Compress the given byte array
        /// </summary>
        /// <param name="raw">byte array to compress</param>
        /// <returns>compressed byte array</returns>
        public static byte[] Compress(byte[] raw)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                return memory.ToArray();
            }
        }
        /// <summary>
        /// Decompress the given byte array
        /// </summary>
        /// <param name="gzip">byte array to decompress</param>
        /// <param name="offset">index to start decompress</param>
        /// <param name="count">size of given byte</param>
        /// <returns>decompressed data</returns>
        public static byte[] Decompress(byte[] gzip, int offset, int count)
        {
            // Create a GZIP stream with decompression mode.
            // ... Then create a buffer and write into while reading from the GZIP stream.
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip, offset, count), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int tCount = 0;
                    do
                    {
                        tCount = stream.Read(buffer, 0, size);
                        if (tCount > 0)
                        {
                            memory.Write(buffer, 0, tCount);
                        }
                    }
                    while (tCount > 0);
                    return memory.ToArray();
                }
            }
        }

        /// <summary>
        /// Compress the given byte array
        /// </summary>
        /// <param name="raw">byte array to compress</param>
        /// <param name="offset">index to start compress</param>
        /// <param name="count">size of given byte</param>
        /// <returns>compressed byte array</returns>
        public static byte[] Compress(byte[] raw, int offset, int count)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    gzip.Write(raw, offset, count);
                }
                return memory.ToArray();
            }
        }
    }
}
