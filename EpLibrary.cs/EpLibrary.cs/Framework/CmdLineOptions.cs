using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpLibrary.cs
{

    public class CmdLineOptions: Dictionary<String,CmdLineOptions.CmdArgs>
    {

        public class CmdArgs
        {
            public CmdArgs()
            {

            }
            public List<String> m_args = new List<String>();
        }

        public CmdLineOptions(): base(StringComparer.OrdinalIgnoreCase)
        {

        }

        public CmdLineOptions(CmdLineOptions b):base(b)
        {

        }

        ~CmdLineOptions()
        {

        }

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

		/*!
		Check if CmdLineOptions contain the given option
		@param[in] option the option string to check
		@return true if exists otherwise false
		*/
		bool HasOption(String option)
        {
            return (ContainsKey(option));
        }

		/*!
		Get argument of given option at given index.
		@param[in] option the option string to get argument
		@param[in] idx the index of the arguments of given option
		@param[in] defaultArg the default argument string if not found 
		@return the argument string found.
		@remark if the argument does not exist then return given default argument string.
		*/
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
		String GetArgument(String option,int idx)
        {
            String value=this[option].m_args[idx];
            return value;
        }

		/*!
		Get the number of arguments of given option
		@param[in] option the option string to get the number of arguments
		@return the number of arguments of given option
		@remark if the option does not exist then return -1
		*/
		int GetArgumentCount(String option)
        {
            int retCount=-1;
            if(ContainsKey(option))
            {
                return this[option].m_args.Count;
            }
            return retCount;
        }

		/*!
		Check if given option is an option (starts with '-')
		@param[in] option the option string to check
		@return true if given option string is an option otherwise false.
		*/
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
