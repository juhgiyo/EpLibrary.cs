/*! 
@file PropertiesFile.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief PropertiesFile Interface
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

A PropertiesFile Class.

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
    /// A class for Properties File.
    /// </summary>
    public sealed class PropertiesFile:BaseTextFile
    {
        /// <summary>
        /// The list of the properties
        /// </summary>
        private Dictionary<String, String> m_propertyList=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="encoding">the encoding type for this file</param>
		public PropertiesFile(Encoding encoding=null):base(encoding)
        {
        }

        /// <summary>
        /// Default Copy Constructor
        /// </summary>
        /// <param name="b">the object to copy from</param>
        public PropertiesFile(PropertiesFile b)
            : base(b)
        {
            lock(m_baseTextLock)
            {
                m_propertyList = new Dictionary<string, string>(b.m_propertyList, StringComparer.OrdinalIgnoreCase);
            }
        }
		

		~PropertiesFile()
        {
        }

        /// <summary>
        /// Set the property with the given key with the value given
        /// </summary>
        /// <param name="key">the key of the property to change the value</param>
        /// <param name="val">the value to change the property</param>
        /// <returns></returns>
        public bool SetProperty(String key, String val)
        {
            lock(m_baseTextLock)
            {
                String opKey=key.Trim();
                opKey+="=";
                if(m_propertyList.ContainsKey(opKey))
                {
                    m_propertyList[opKey]=val.Trim();
                    return true;
                }
                return false;
            }

        }


        /// <summary>
        /// Get the value of the property with the given key
        /// </summary>
        /// <param name="key">the key of the property to get the value</param>
        /// <param name="retVal">the value of the property of given key</param>
        /// <returns>true if found, otherwise false</returns>
        public bool GetProperty(String key, ref String retVal)
        {
            lock(m_baseTextLock)
            {
                String opKey=key.Trim();
                opKey+="=";
                if (m_propertyList.ContainsKey(opKey))
                {
                    retVal=m_propertyList[opKey];
                    return true;
                }
                return false;
            }
        }


        /// <summary>
        /// Get the value of the property with the given key
        /// </summary>
        /// <param name="key">the key of the property to get the value</param>
        /// <returns>the value of the property of given key</returns>
        /// <remarks>raises exception when key does not exists</remarks>
        public String GetProperty(String key)
        {
            lock(m_baseTextLock)
            {
                String opKey=key.Trim();
                opKey+="=";
                return m_propertyList[opKey];
            }
        }

        /// <summary>
        /// Add new property with the given key and value
        /// </summary>
        /// <param name="key">the key of the property to add</param>
        /// <param name="val">the value of the new property</param>
        /// <returns>true if successfully added, otherwise false</returns>
        public bool AddProperty(String key, String val)
        {
            lock(m_baseTextLock)
            {
                String opKey=key.Trim();
                opKey+="=";
                if(m_propertyList.ContainsKey(opKey))
                    return false;
                m_propertyList.Add(opKey,val.Trim());
                return true;
            }
        }


        /// <summary>
        /// Remove the property with the given key
        /// </summary>
        /// <param name="key">the key of the property to remove</param>
        /// <returns>true if successfully removed, otherwise false</returns>
        public bool RemoveProperty(String key)
        {
            lock(m_baseTextLock)
            {
                String opKey=key.Trim();
                opKey+="=";
                return m_propertyList.Remove(opKey);
            }
        }

        /// <summary>
        /// Clear the list of the properties
        /// </summary>
        public void Clear()
        {
            lock(m_baseTextLock)
            {
                m_propertyList.Clear();
            }
        }

        /// <summary>
        /// If given key exists, then return the value, and if given key does not exist, then create key and return the reference to empty value.
        /// </summary>
        /// <param name="key">the key of the property to find/create</param>
        /// <returns>value of the given key.</returns>
		public  String this[String key]
        {
            get{
                 lock(m_baseTextLock)
                {
                    String opKey=key.Trim();
                    opKey+="=";
                     if(m_propertyList.ContainsKey(opKey))
                        return m_propertyList[opKey];
                     m_propertyList.Add(opKey,"");
                     return m_propertyList[opKey];
                }
            }
            set
            {
                lock(m_baseTextLock)
                {
                    String opKey=key.Trim();
                    opKey+="=";
                    m_propertyList[opKey]=value;
                }
            }
        }


        /// <summary>
        /// Loop Function that writes to the file.
        /// </summary>
		protected override void writeLoop()
        {
            StringBuilder toFileString=new StringBuilder();
            foreach(KeyValuePair<String,String> entry in m_propertyList)
            {
                toFileString.Clear();
                toFileString.Append(entry.Key);
                toFileString.Append(entry.Value);
                toFileString.Append("\n");
                writeToFile(toFileString.ToString());
            }
        }

        /// <summary>
        /// Actual Load Function that loads values from the file.
        /// </summary>
        /// <param name="stream">the stream from the file</param>
		protected override void loadFromFile(StreamReader stream)
        {
            m_propertyList.Clear();
            String line ="";
            line=stream.ReadLine();
            while(line!=null)
            {
                String key="";
		        String val="";
		        if(getValueKeyFromLine(line,ref key,ref val))
		        {
			        key.Trim();
                    val.Trim();
			        m_propertyList.Add(key,val);
		        }
		        else
		        {
			        m_propertyList.Add(line,"");
		        }
                line = stream.ReadLine();

            }
        }

        /// <summary>
        /// Parse the key and value from the line buffer
        /// </summary>
        /// <param name="buf">the buffer that holds a line</param>
        /// <param name="retKey">the key part of the given line</param>
        /// <param name="retVal">the value part of the given line</param>
        /// <returns>true if successfully parsed the key and value, otherwise false</returns>
        private bool getValueKeyFromLine(String buf, ref String retKey, ref String retVal)
        {
            char splitChar='\0';
            int bufTrav = 0;
            if (buf.Length <= 0)
                return false;

            retKey = "";
            retVal = "";
            StringBuilder builder = new StringBuilder();
            

            for (int testTrav = 0; testTrav < buf.Length; testTrav++)
            {
                if (buf[testTrav]== '#')
                    return false;
            }

            while (splitChar != '=' && bufTrav < buf.Length)
            {
                splitChar = buf[bufTrav];
                builder.Append(splitChar);
                bufTrav++;
            }
            retKey=builder.ToString();
            retVal = buf;
            retVal=retVal.Remove(0, bufTrav);

            return true;
        }

    }
}
