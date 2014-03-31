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
