using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace EpLibrary.cs
{
    public sealed class TextFile:BaseTextFile
    {
        String m_text;
        		/*!
		Default Constructor

		Initializes the Properties File 
		@param[in] encodingType the encoding type for this file
		@param[in] lockPolicyType The lock policy
		*/
		public TextFile(Encoding encoding=null):base(encoding)
        {
            m_text="";
        }

		/*!
		Default Copy Constructor

		Initializes the PropertiesFile 
		@param[in] b the second object
		*/
		public TextFile(TextFile b):base(b)
        {
            lock(m_baseTextLock)
            {
                m_text=b.m_text;
            }
        }

	
		/*!
		Default Destructor

		Destroy the Properties File 
		*/
		~TextFile()
        {
        }

		/*!
		Set the text with the given text
		@param[in] val the text value
		*/
		void SetText(String val)
        {
            lock(m_baseTextLock)
            {
                m_text=val;
            }
        }

		/*!
		Get the value of the textfile
		@return text value holding
		*/
		String GetText()
        {
            lock(m_baseTextLock)
            {
                return m_text;
            }
        }

	
		/*!
		Clear the text
		*/
		void Clear()
        {
            lock(m_baseTextLock)
            {
             m_text="";
            }
        }

		/*!
		Loop Function that writes to the file.
		@remark Sub classes should implement this function
		*/
		protected override void writeLoop()
        {
            writeToFile(m_text);
        }

		/*!
		Actual Load Function that loads values from the file.
		@remark Sub classes should implement this function
		@param[in] lines the all data from the file
		*/
        protected override void loadFromFile(StreamReader stream)
        {
            m_text=stream.ReadToEnd();
        }

    }
}
