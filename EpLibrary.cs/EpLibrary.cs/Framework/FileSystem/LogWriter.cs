using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
            m_fileName = FolderHelper.GetModuleFileName();
            m_fileName.Replace(".exe", ".log");
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
		

        ~LogWriter()
        {
        }

		
    }
}
