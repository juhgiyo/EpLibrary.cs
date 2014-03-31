using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpLibrary.cs
{
    /// <summary>
    /// A CmdLine Options class.
    /// </summary>
    public class CmdLineOptions: Dictionary<String,CmdLineOptions.CmdArgs>
    {
        /// <summary>
        /// A CmdLine Argument List class.
        /// </summary>
        public class CmdArgs
        {
            public CmdArgs()
            {

            }
            /// <summary>
            /// List of arguments
            /// </summary>
            public List<String> m_args = new List<String>();
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public CmdLineOptions(): base(StringComparer.OrdinalIgnoreCase)
        {

        }

        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="b">the object to copy from</param>
        public CmdLineOptions(CmdLineOptions b):base(b)
        {

        }

        ~CmdLineOptions()
        {

        }

        /// <summary>
        /// Parse the Command Line Argument with given.
        /// </summary>
        /// <param name="argv">the array of argument strings.</param>
        /// <returns>the number of CmdLine Options parsed</returns>
        int Parse(String[] argv)
        {
            Clear();
            String curSector;
            for(int argTrav=1;argTrav<argv.Length;argTrav++)
            {
                if(isOption(argv[argTrav]))
                {
                    curSector=argv[argTrav];
                    CmdArgs args=new CmdArgs();
                    while(argTrav+1<argv.Length && !isOption(argv[argTrav+1]))
                    {
                        args.m_args.Add(argv[argTrav+1]);
                        argTrav++;
                    }
                    Add(curSector,args);

                }
            }
            return Count;
        }

        /// <summary>
        /// Check if CmdLineOptions contain the given option
        /// </summary>
        /// <param name="option">the option string to check</param>
        /// <returns>true if exists otherwise false</returns>
		bool HasOption(String option)
        {
            return (ContainsKey(option));
        }

        /// <summary>
        /// Get argument of given option at given index.
        /// </summary>
        /// <param name="option">the option string to get argument</param>
        /// <param name="idx">the index of the arguments of given option</param>
        /// <param name="defaultArg">the default argument string if not found </param>
        /// <returns>the argument string found.</returns>
        /// <remarks>if the argument does not exist then return given default argument string.</remarks>
		String GetArgument(String option,int idx, String defaultArg)
        {
            try{
                String value=this[option].m_args[idx];
                return value;
            }
            catch
            {
                return defaultArg;
            }
        }

		/*!
		Get argument of given option at given index.
		@param[in] option the option string to get argument
		@param[in] idx the index of the arguments of given option
		@return the argument string found.
		@remark if the argument does not exist then throws exception 0.
		*/
        /// <summary>
        /// Get argument of given option at given index.
        /// </summary>
        /// <param name="option">the option string to get argument</param>
        /// <param name="idx">the index of the arguments of given option</param>
        /// <returns>the argument string found.</returns>
        /// <remarks>if the argument does not exist then throws exception</remarks>
		String GetArgument(String option,int idx)
        {
            String value=this[option].m_args[idx];
            return value;
        }

        /// <summary>
        /// Get the number of arguments of given option
        /// </summary>
        /// <param name="option">the option string to get the number of arguments</param>
        /// <returns>the number of arguments of given option</returns>
        /// <remarks>if the option does not exist then return -1</remarks>
		int GetArgumentCount(String option)
        {
            int retCount=-1;
            if(ContainsKey(option))
            {
                return this[option].m_args.Count;
            }
            return retCount;
        }

        /// <summary>
        /// Check if given option is an option (starts with '-')
        /// </summary>
        /// <param name="option">the option string to check</param>
        /// <returns>true if given option string is an option otherwise false.</returns>
		protected bool isOption(String option)
        {
            if(option==null)
		        return false;

	        String checkSectorString=option;
	        checkSectorString=checkSectorString.Trim();

	        if(checkSectorString.Length>1)
	        {
		        if(checkSectorString[0]=='-')
		        {
			        return !char.IsDigit(checkSectorString[1]);
		        }
	        }
	        return false;
        }
    }
}
