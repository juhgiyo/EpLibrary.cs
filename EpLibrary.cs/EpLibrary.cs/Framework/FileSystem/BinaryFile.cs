/*! 
@file BinaryFile.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief BinaryFile Interface
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

A BinaryFile Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace EpLibrary.cs
{
    /// <summary>
    /// A class for Binary File.
    /// </summary>
    public class BinaryFile: IDisposable
    {
        /// <summary>
        /// stream
        /// </summary>
        protected MemoryStream m_stream=new MemoryStream();
        /// <summary>
        /// lock
        /// </summary>
        protected Object m_baseBinaryLock=new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
		public BinaryFile()
        {
        }

        /// <summary>
        /// Default Copy Constructor
        /// </summary>
        /// <param name="b">the object to copy from</param>
        public BinaryFile(BinaryFile b)
        {
            lock (b.m_baseBinaryLock)
            {
                m_stream = b.m_stream;
            }
        }

	
        /// <summary>
        /// Save the binary to the given file
        /// </summary>
        /// <param name="filename">the name of the file to save</param>
        /// <returns>true if successfully saved, otherwise false</returns>
        public bool SaveToFile(String filename)
        {
            lock(m_baseBinaryLock)
            {
                try
                {

                    using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
                    {
                        writer.Write(m_stream.ToArray());
                        writer.Flush();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + " >" + ex.StackTrace);
                    return false;
                }
            }
        }

        /// <summary>
        /// Append the binary to the given file
        /// </summary>
        /// <param name="filename">the name of the file to append</param>
        /// <returns>true if successfully saved, otherwise false</returns>
        public bool AppendToFile(String filename)
        {
            lock(m_baseBinaryLock)
            {
                try
                {

                    using(BinaryWriter writer = new BinaryWriter(File.Open(filename,FileMode.Append))){
                        writer.Write(m_stream.ToArray());
                        writer.Flush();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + " >" + ex.StackTrace);
                    return false;
                }
            }
        }
		

        /// <summary>
        /// Load the list of the properties from the given file
        /// </summary>
        /// <param name="filename">the name of the file to load</param>
        /// <returns>true if successfully loaded, otherwise false</returns>
        public bool LoadFromFile(String filename)
        {
            lock(m_baseBinaryLock)
            {
                try
                {
                    using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
                    {
                        FileInfo fInfo = new FileInfo(filename);
                        m_stream.SetLength(fInfo.Length);
                        reader.Read(m_stream.GetBuffer(), 0, (int)fInfo.Length);
                        m_stream.Seek(0, SeekOrigin.Begin);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + " >" + ex.StackTrace);
                    return false;
                }
            }
        }

        /// <summary>
        /// Get the current stream
        /// </summary>
        /// <returns>the current stream</returns>
        public MemoryStream GetStream()
        {
            lock (m_baseBinaryLock)
            {
                return m_stream;
            }
        }

		
        /// <summary>
        /// Set the stream as given stream
        /// </summary>
        /// <param name="stream">the stream to set</param>
        public void SetStream(MemoryStream stream)
        {
            lock (m_baseBinaryLock)
            {
                m_stream = stream;
            }
        }

          bool m_disposed = false;

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (m_disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                if (m_stream != null)
                {
                    m_stream.Dispose();
                    m_stream = null;
                }
            }

            // Free any unmanaged objects here.
            m_disposed = true;
        }

        ~BinaryFile() { Dispose(false); }
    }
}
