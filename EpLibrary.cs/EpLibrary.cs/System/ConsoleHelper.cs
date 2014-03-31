using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace EpLibrary.cs
{
    public class ConsoleHelper
    {
        /*!
		Execute the given command to the console and return the result

		** waitStruct is ignored when isWaitForTerminate is false.
		@param[in] command the command to execute
		@param[in] isDosCommand flag whether the command is standard DOS command or not
		@param[in] isWaitForTerminate flag for waiting for process to terminate or not
		@param[in] isShowWindow flag for whether to show console window
		@param[in] useStdOutput flag for whether console to print to console window or to pipe.<br/>
		                        true to print to pipe; false to print to console.
		@param[in] priority the priority of the process executing
		@param[out] retProcessHandle the handle to the process created
		@return the result of the console command
		@remark retProcessHandle will be NULL when the function exits.
		        This can be used when isWaitForTerminate is true, and you need to terminate the process while waiting.
				Terminate the process from the other thread using the given handle pointer.
		*/
        public static String ExecuteConsoleCommand(String command,String arguments, bool isDosCommand = false, bool isWaitForTerminate = true, bool isShowWindow = false, bool redirectStdOutput = true, ThreadPriority priority = ThreadPriority.Normal)
        {
            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                ProcessStartInfo procStartInfo=null;
                if (isDosCommand)
                    procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command +" "+ arguments);
                else
                    procStartInfo = new ProcessStartInfo(command, arguments);


                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = redirectStdOutput;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = isShowWindow;
                // Now we create a process, assign its ProcessStartInfo and start it
                Process proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                if(isWaitForTerminate)
                    return proc.StandardOutput.ReadToEnd();
                return null;
            }
            catch (Exception)
            {
                // Log the exception
                return null;
            }
        }

		/*!
		Execute the given executable file
		@param[in] execFilePath the program file path to execute
		@param[in] parameters the parameter variables for executing file
		*/
        public static void ExecuteProgram(String execFilePath, String parameters = null)
        {
            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo(execFilePath, parameters);

                procStartInfo.UseShellExecute = true;
                // Now we create a process, assign its ProcessStartInfo and start it
                Process proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
            }
            catch (Exception)
            {
            }
        }
    }
}
