using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EpLibrary.cs
{
    public sealed class LogWriter:BaseTextFile
    {
        /// Name of the Log File
        String m_fileName;

        /// Log String
        StringBuilder m_logString;

        private Object m_logLock = new Object();

        /*!
		Writer given message to the log with current time.
		@param[in] pMsg the message to print to the log file.
		*/
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

		/*!
		Default Constructor

		Initializes the Log Writer
		@param[in] lockPolicyType The lock policy
		*/
        private LogWriter()
        {
            m_fileName = FolderHelper.GetModuleFileName();
            m_fileName.Replace(".exe", ".log");
        }
		
		/*!
		Default Copy Constructor

		Initializes the LogWriter 
		@param[in] b the second object
		*/
        private LogWriter(LogWriter b)
        {
            m_fileName = b.m_fileName;
        }

		/*!
		Loop Function that writes to the file.
		@remark Sub classes should implement this function
		*/
        protected override void writeLoop()
        {
            writeToFile(m_logString.ToString());
        }

		/*!
		Actual load Function that loads values from the file.
		@remark Sub classes should implement this function
		@param[in] lines the all data from the file
		*/
        protected override void loadFromFile(StreamReader stream)
        {
        }
		
		/*!
		Default Destructor

		Destroy the Log Writer
		*/
        ~LogWriter()
        {
        }

		
    }
}
