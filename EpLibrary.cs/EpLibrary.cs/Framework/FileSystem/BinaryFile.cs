using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EpLibrary.cs
{
    public class BinaryFile
    {
        protected BinaryReader m_reader=null;
        protected BinaryWriter m_writer=null;
        protected MemoryStream m_stream=new MemoryStream();
        protected Object m_baseBinaryLock=new Object();
        /*!
		Default Constructor

		Initializes the Binary File 
		@param[in] lockPolicyType The lock policy
		*/
		public BinaryFile()
        {
        }

		/*!
		Default Copy Constructor

		Initializes the Binary File 
		@param[in] b the second object
		*/
        public BinaryFile(BinaryFile b)
        {
            lock (b.m_baseBinaryLock)
            {
                m_reader = b.m_reader;
                m_writer = b.m_writer;
            }
        }

		/*!
		Default Destructor

		Destroy the Binary File 
		*/
		~BinaryFile()
        {
        }

	
		/*!
		Save the list of the properties from the given file
		@param[in] filename the name of the file to save the list of properties
		@return true if successfully saved, otherwise false
		*/
        public bool SaveToFile(String filename)
        {
            lock(m_baseBinaryLock)
            {
                try
                {

                    m_writer = new BinaryWriter(File.Open(filename,FileMode.OpenOrCreate));
                    m_writer.Write(m_stream.ToArray());
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
            lock(m_baseBinaryLock)
            {
                try
                {

                    m_writer = new BinaryWriter(File.Open(filename,FileMode.Append));
                    m_writer.Write(m_stream.ToArray());
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
            lock(m_baseBinaryLock)
            {
                try
                {
                    m_reader = new BinaryReader(File.Open(filename,FileMode.OpenOrCreate));
                    FileInfo fInfo = new FileInfo(filename);
                    m_stream.SetLength(fInfo.Length);
                    m_reader.Read(m_stream.GetBuffer(),0,(int)fInfo.Length);
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
		Get the current stream
		@return the current stream
		*/
        public MemoryStream GetStream()
        {
            lock (m_baseBinaryLock)
            {
                return m_stream;
            }
        }

		

		/*!
		Set the stream as given stream
		@param[in] stream the stream to set
		*/
        public void SetStream(MemoryStream stream)
        {
            lock (m_baseBinaryLock)
            {
                m_stream = stream;
            }
        }
    }
}
