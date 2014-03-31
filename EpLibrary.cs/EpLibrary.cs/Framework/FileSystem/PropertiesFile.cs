using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace EpLibrary.cs
{
    public sealed class PropertiesFile:BaseTextFile
    {
        private Dictionary<String, String> m_propertyList=new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);

        	/*!
		Default Constructor

		Initializes the Properties File 
		@param[in] encodingType the encoding type for this file
		@param[in] lockPolicyType The lock policy
		*/
		public PropertiesFile(Encoding encoding=null):base(encoding)
        {
        }

		/*!
		Default Copy Constructor

		Initializes the PropertiesFile 
		@param[in] b the second object
		*/
        public PropertiesFile(PropertiesFile b)
            : base(b)
        {
            lock(m_baseTextLock)
            {
                m_propertyList = new Dictionary<string, string>(b.m_propertyList, StringComparer.OrdinalIgnoreCase);
            }
        }
		

		/*!
		Default Destructor

		Destroy the Properties File 
		*/
		~PropertiesFile()
        {
        }

		/*!
		Set the property with the given key with the value given
		@param[in] key the key of the property to change the value
		@param[in] val the value to change the property
		@return true if changed, otherwise false
		*/
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

		/*!
		Get the value of the property with the given key
		@param[in] key the key of the property to get the value
		@param[in] retVal the value of the property of given key
		@return true if found, otherwise false
		*/
        public bool GetProperty(String key, ref String retVal)
        {
            lock(m_baseTextLock)
            {
                String opKey=key.Trim();
                opKey+="=";
                if(m_propertyList.ContainsKey(key))
                {
                    retVal=m_propertyList[opKey];
                    return true;
                }
                return false;
            }
        }

		/*!
		Get the value of the property with the given key
		@param[in] key the key of the property to get the value
		@remark raises exception when key does not exists 
		@return the value of the property of given key
		*/
        public String GetProperty(String key)
        {
            lock(m_baseTextLock)
            {
                String opKey=key.Trim();
                opKey+="=";
                return m_propertyList[opKey];
            }
        }

		/*!
		Add new property with the given key and value
		@param[in] key the key of the property to add
		@param[in] val the value of the new property
		@return true if successfully added, otherwise false
		*/
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

		/*!
		Remove the property with the given key
		@param[in] key the key of the property to remove
		@return true if successfully removed, otherwise false
		*/
        public bool RemoveProperty(String key)
        {
            lock(m_baseTextLock)
            {
                String opKey=key.Trim();
                opKey+="=";
                return m_propertyList.Remove(opKey);
            }
        }

		/*!
		Clear the list of the properties
		*/
        public void Clear()
        {
            lock(m_baseTextLock)
            {
                m_propertyList.Clear();
            }
        }

		/*!
		If given key exists, then return the value, and 
		if given key does not exist, then create key and 
		return the reference to empty value.
		@param[in] key the key of the property to find/create
		@return value of the given key.
		*/
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


		/*!
		Loop Function that writes to the file.
		@remark Sub classes should implement this function
		*/
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

		/*!
		Actual Load Function that loads values from the file.
		@remark Sub classes should implement this function
		@param[in] lines the all data from the file
		*/
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

		/*!
		Parse the key and value from the line buffer
		@param[in] buf the buffer that holds a line
		@param[out] retKey the key part of the given line
		@param[out] retVal the value part of the given line
		@return true if successfully parsed the key and value, otherwise false
		*/
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
            retVal.Remove(0, bufTrav);

            return true;
        }

    }
}
