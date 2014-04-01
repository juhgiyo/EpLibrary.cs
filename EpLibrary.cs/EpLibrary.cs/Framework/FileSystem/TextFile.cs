/*! 
@file TextFile.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief TextFile Interface
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

A TextFile Class.

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
    /// A class for Text File.
    /// </summary>
    public sealed class TextFile:BaseTextFile
    {
        /// <summary>
        /// the text
        /// </summary>
        private String m_text;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="encoding">the encoding type for this file</param>
		public TextFile(Encoding encoding=null):base(encoding)
        {
            m_text="";
        }

        /// <summary>
        /// Default Copy Constructor
        /// </summary>
        /// <param name="b">the object to copy from</param>
		public TextFile(TextFile b):base(b)
        {
            lock(m_baseTextLock)
            {
                m_text=b.m_text;
            }
        }

		~TextFile()
        {
        }


        /// <summary>
        /// Set the text with the given text
        /// </summary>
        /// <param name="val">the text value</param>
		void SetText(String val)
        {
            lock(m_baseTextLock)
            {
                m_text=val;
            }
        }


        /// <summary>
        /// Get the value of the text
        /// </summary>
        /// <returns>text value holding</returns>
		String GetText()
        {
            lock(m_baseTextLock)
            {
                return m_text;
            }
        }

	
        /// <summary>
        /// Clear the text
        /// </summary>
		void Clear()
        {
            lock(m_baseTextLock)
            {
             m_text="";
            }
        }

	
        /// <summary>
        /// Loop Function that writes to the file.
        /// </summary>
		protected override void writeLoop()
        {
            writeToFile(m_text);
        }

        /// <summary>
        /// Actual Load Function that loads values from the file.
        /// </summary>
        /// <param name="stream">the stream from the file</param>
        protected override void loadFromFile(StreamReader stream)
        {
            m_text=stream.ReadToEnd();
        }

    }
}
