using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace EpLibrary.cs
{
    /// <summary>
    /// This is a class for Console Processing Class
    /// </summary>
    public class ConsoleHelper
    {

        /// <summary>
        /// Execute the given command to the console and return the result
        /// 
        /// ** waitStruct is ignored when isWaitForTerminate is false.
        /// </summary>
        /// <param name="command">the command to execute</param>
        /// <param name="arguments">the argument for the command</param>
        /// <param name="isDosCommand">flag whether the command is standard DOS command or not</param>
        /// <param name="isWaitForTerminate">flag for waiting for process to terminate or not</param>
        /// <param name="isShowWindow">flag for whether to show console window</param>
        /// <param name="redirectStdOutput">flag for whether console to print to console window or to pipe.
        /// true to print to pipe; false to print to console.
        /// </param>
        /// <returns>the result of the console command</returns>
        /// <remarks>
        /// retProcessHandle will be NULL when the function exits.
        /// This can be used when isWaitForTerminate is true, and you need to terminate the process while waiting.
        /// Terminate the process from the other thread using the given handle pointer.
        /// </remarks>
        public static String ExecuteConsoleCommand(String command,String arguments, bool isDosCommand = false, bool isWaitForTerminate = true, bool isShowWindow = false, bool redirectStdOutput = true)
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

        /// <summary>
        /// Execute the given executable file
        /// </summary>
        /// <param name="execFilePath">the program file path to execute</param>
        /// <param name="parameters">the parameter variables for executing file</param>
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
