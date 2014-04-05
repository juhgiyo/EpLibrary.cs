/*! 
@file BaseTextFile.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief BaseTextFile Interface
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

A BaseTextFile Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EpLibrary.cs
{
    /// <summary>
    /// A class for Base File.
    /// </summary>
    public abstract class BaseTextFile
    {
        /// <summary>
        /// reader stream
        /// </summary>
        protected StreamReader m_reader=null;
        /// <summary>
        ///  writer stream
        /// </summary>
        protected StreamWriter m_writer = null;
        /// <summary>
        /// Encoding
        /// </summary>
        protected Encoding m_encoding = Encoding.Unicode;
        /// <summary>
        /// lock
        /// </summary>
        protected Object m_baseTextLock = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="encoding">the encoding type for this file</param>
		public BaseTextFile(Encoding encoding=null)
        {
            if (encoding != null)
            {
                m_encoding = encoding;
            }
        }

        /// <summary>
        /// Default Copy Constructor
        /// </summary>
        /// <param name="b">the second object</param>
        public BaseTextFile(BaseTextFile b)
        {
            lock (b.m_baseTextLock)
            {
                m_encoding = b.m_encoding;
                m_reader = b.m_reader;
                m_writer = b.m_writer;
            }
        }

        /// <summary>
        /// Get current encoding
        /// </summary>
        /// <returns>current encoding</returns>
        public Encoding GetEncoding()
        {
            lock(m_baseTextLock)
            {
	            return m_encoding;
            }
        }
        /// <summary>
        /// Set encoding as given encoding
        /// </summary>
        /// <param name="encoding">encoding to set</param>
        public void SetEncoding(Encoding encoding)
        {
            lock(m_baseTextLock)
            {
	            m_encoding=encoding;
            }
        }

        /// <summary>
        /// Save the text to the given file
        /// </summary>
        /// <param name="filename">the name of the file to save</param>
        /// <returns>true if successfully saved, otherwise false</returns>
        public bool SaveToFile(String filename)
        {
            lock(m_baseTextLock)
            {
                try
                {

                    m_writer = new StreamWriter(filename, false, m_encoding);
                    writeLoop();
                    m_writer.Close();
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
        /// Append the text from the given file
        /// </summary>
        /// <param name="filename">the name of the file to append</param>
        /// <returns>true if successfully saved, otherwise false</returns>
        public bool AppendToFile(String filename)
        {
            lock(m_baseTextLock)
            {
                try
                {

                    m_writer = new StreamWriter(filename, true, m_encoding);
                    writeLoop();
                    m_writer.Close();
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
            lock(m_baseTextLock)
            {
                try
                {
                    m_reader=new StreamReader(filename,m_encoding,true);
                    loadFromFile(m_reader);
                    m_reader.Close();
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
        /// Write the given string to the file
        /// </summary>
        /// <param name="toFileString">the string to write to the file</param>
        protected void writeToFile(String toFileString)
        {
            lock (m_baseTextLock)
            {
                try
                {

                    if (m_writer != null)
                    {
                        m_writer.Write(toFileString);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + " >" + ex.StackTrace);
                }
            }
        }


        /// <summary>
        /// Loop Function that writes to the file.
        /// </summary>
        /// <remarks>Sub classes should implement this function</remarks>
		protected abstract void writeLoop();

        /// <summary>
        /// Actual load Function that loads values from the file.
        /// </summary>
        /// <param name="stream">stream from the file</param>
        /// <remarks>Sub classes should implement this function</remarks>
		protected abstract void loadFromFile(StreamReader stream);
    }
}
