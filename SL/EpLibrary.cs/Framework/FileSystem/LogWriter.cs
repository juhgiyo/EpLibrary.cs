/*! 
@file LogWriter.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief LogWriter Interface
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

A LogWriter Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Reflection;

namespace EpLibrary.cs
{
    /// <summary>
    /// Log Writer Singleton Instance
    /// </summary>
    public sealed class LogWriter:BaseTextFile
    {
        /// <summary>
        /// Name of the Log File
        /// </summary>
        private String m_fileName;

        /// <summary>
        /// Log string builder
        /// </summary>
        private StringBuilder m_logString;

        /// <summary>
        /// lock
        /// </summary>
        private Object m_logLock = new Object();

        /// <summary>
        /// Writer given message to the log with current time.
        /// </summary>
        /// <param name="pMsg">the message to print to the log file.</param>
        public void WriteLog(String pMsg)
        {
            lock (m_logLock)
            {
                DateTime curTime = DateTime.Now;
                m_logString = new StringBuilder();
                m_logString.AppendFormat("{0}/{1}/{2}, {3}:{4}:{5}.{6}  :  {7}\n", curTime.Month, curTime.Day, curTime.Year, curTime.Hour, curTime.Minute, curTime.Second, curTime.Millisecond, pMsg);
                AppendToFile(m_fileName);
            }
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public LogWriter()
        {
            var callingAssembly = Assembly.GetCallingAssembly();
            var assemblyProductAttribute = ((AssemblyProductAttribute[])callingAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false)).Single();
            m_fileName = assemblyProductAttribute.Product+".log";
        }
		
        /// <summary>
        /// Default Copy Constructor
        /// </summary>
        /// <param name="b">the object to copy from</param>
        public LogWriter(LogWriter b)
        {
            m_fileName = b.m_fileName;
        }

        /// <summary>
        /// Loop Function that writes to the file.
        /// </summary>
        protected override void writeLoop()
        {
            writeToFile(m_logString.ToString());
        }


        /// <summary>
        /// Actual load Function that loads values from the file.
        /// </summary>
        /// <param name="stream">stream from the file</param>
        protected override void loadFromFile(StreamReader stream)
        {
        }
		


		
    }
}
