/*! 
@file LogWorker.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief Log Worker Interface
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

A LogWorker Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Threading;

namespace EpLibrary.cs
{
    /// <summary>
    /// Log Worker
    /// </summary>
    public class LogWorker : BaseTextFile, IDisposable
    {
        /// <summary>
        /// Loggin stop event
        /// </summary>
        EventEx m_threadStopEvent = new EventEx(false, EventResetMode.ManualReset);
        /// <summary>
        /// Lock for log
        /// </summary>
        Object m_logLock = new Object();
        /// <summary>
        /// Lock for Thread
        /// </summary>
        Object m_threadLock = new Object();
        /// <summary>
        ///  Name of the log file
        /// </summary>
        string m_fileName;
        /// <summary>
        ///  Log String
        /// </summary>
        string m_logString;
        /// <summary>
        ///  Log Queue
        /// </summary>
        Queue<string> m_logQueue = new Queue<string>();
        /// <summary>
        /// Thread
        /// </summary>
        ThreadEx m_thread;

        /// <summary>
        /// Name of Log File
        /// </summary>
        public String FileName
        {
            get
            {
                return m_fileName;
            }
        }
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="fileName">name of log file</param>
        /// <param name="encodingType">encoding type</param>
        public LogWorker(string fileName, Encoding encodingType = null)
            : base(encodingType)
        {
            m_fileName = fileName;
            m_thread = new ThreadEx(this.execute, ThreadPriority.Normal);
            m_thread.Start();
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~LogWorker()
        {
            Dispose(false);
        }

        /// <summary>
        /// Stop the logging
        /// </summary>
        public void Stop()
        {
            stop();
        }

        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="b">the copying object</param>
        public LogWorker(LogWorker b)
            : base(b)
        {
            m_fileName = b.FileName;
            m_thread.Start();
        }

        /// <summary>
        /// Start the log worker
        /// </summary>
        void execute()
        {
            while (!m_threadStopEvent.WaitForEvent(0))
            {
                Thread.Sleep(1);

                m_logString = "";


                lock (m_logLock)
                {
                    while (m_logQueue.Count != 0)
                    {
                        string logString = m_logQueue.Peek();
                        m_logQueue.Dequeue();

                        DateTime curTime = DateTime.Now;
                        m_logString += "[" + curTime.ToString("yyyy-MM-dd HH:mm:ss-fff") + "] : " + logString + "\r\n";
                    }
                }

                if (m_logString.Length > 0)
                    AppendToFile(m_fileName);
            }
        }
        /// <summary>
        /// Stop the log worker
        /// </summary>
        void stop()
        {
            if (m_thread.GetStatus() != ThreadStatus.TERMINATED)
            {
                m_threadStopEvent.SetEvent();
                m_thread.WaitFor();
            }
        }

        /// <summary>
        /// Writer given message to the log with current time.
        /// </summary>
        /// <param name="pMsg">the message to print to the log file.</param>
        public void WriteLog(string pMsg)
        {
            // write error or other information into log file
            lock (m_logLock)
            {
                m_logQueue.Enqueue(pMsg);
            }
        }
        /// <summary>
        /// Loop Function that writes to the file.
        /// </summary>
        protected override void writeLoop()
        {
            writeToFile(m_logString);
        }

        /// <summary>
        /// Actual load Function that loads values from the file.
        /// </summary>
        /// <param name="stream"></param>
        protected override void loadFromFile(StreamReader stream)
        {
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
        private void Dispose(bool disposing)
        {
            if (m_disposed)
                return;

            stop();
            if (disposing)
            {
                // Free any other managed objects here.
                if (m_threadStopEvent != null)
                {
                    m_threadStopEvent.Dispose();
                    m_threadStopEvent = null;
                }
            }

            // Free any unmanaged objects here.
            m_disposed = true;
        }
    }
}
