using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EpLibrary.cs
{
    /// <summary>
    /// A class for Binary File.
    /// </summary>
    public class BinaryFile
    {
        /// <summary>
        /// Binary stream reader
        /// </summary>
        protected BinaryReader m_reader=null;
        /// <summary>
        /// Binary stream writer
        /// </summary>
        protected BinaryWriter m_writer=null;
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
                m_reader = b.m_reader;
                m_writer = b.m_writer;
            }
        }

		~BinaryFile()
        {
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
    }
}
