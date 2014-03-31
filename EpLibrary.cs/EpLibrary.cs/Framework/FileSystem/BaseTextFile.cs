using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EpLibrary.cs
{
    public abstract class BaseTextFile
    {
        protected StreamReader m_reader=null;
        protected StreamWriter m_writer = null;
        protected Encoding m_encoding = Encoding.Unicode;
        protected Object m_baseTextLock = new Object();
		/*!
		Default Constructor

		Initializes the Base File 
		@param[in] encodingType the encoding type for this file
		@param[in] lockPolicyType The lock policy
		*/
		public BaseTextFile(Encoding encoding=null)
        {
            if (encoding != null)
            {
                m_encoding = encoding;
            }
        }

		/*!
		Default Copy Constructor

		Initializes the Base File 
		@param[in] b the second object
		*/
        public BaseTextFile(BaseTextFile b)
        {
            lock (b.m_baseTextLock)
            {
                m_encoding = b.m_encoding;
                m_reader = b.m_reader;
                m_writer = b.m_writer;
            }
        }
		
		/*!
		Default Destructor

		Destroy the Base File 
		*/
		~BaseTextFile()
        {
        }

        public Encoding GetEncoding()
        {
            lock(m_baseTextLock)
            {
	            return m_encoding;
            }
        }
        public void SetEncoding(Encoding encoding)
        {
            lock(m_baseTextLock)
            {
	            m_encoding=encoding;
            }
        }

		/*!
		Save the list of the properties from the given file
		@param[in] filename the name of the file to save the list of properties
		@return true if successfully saved, otherwise false
		*/
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
                catch
                {
                    return false;
                }
            }
            
        }

		/*!
		Append the list of the properties from the given file
		@param[in] filename the name of the file to append the list of properties
		@return true if successfully saved, otherwise false
		*/
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
                catch
                {
                    return false;
                }
            }

        }
		
		/*!
		Load the list of the properties from the given file
		@param[in] filename the name of the file to load the list of properties
		@return true if successfully loaded, otherwise false
		*/
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
                catch
                {
                    return false;
                }
            }
        }


		/*!
		Write the given string to the file
		@param[in] toFileString the string to write to the file
		*/
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
                catch
                {
                }
            }
        }

		/*!
		Loop Function that writes to the file.
		@remark Sub classes should implement this function
		*/
		protected abstract void writeLoop();

		/*!
		Actual load Function that loads values from the file.
		@remark Sub classes should implement this function
		@param[in] lines the all data from the file
		*/
		protected abstract void loadFromFile(StreamReader stream);
    }
}
