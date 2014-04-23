/*! 
@file ConsoleHelper.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief ConsoleHelper Interface
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

A ConsoleHelper Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices.Automation;

namespace EpLibrary.cs
{
    /// <summary>
    /// This is a class for Console Processing Class
    /// </summary>
    public class ConsoleHelper
    {
        /// <summary>
        /// Execute the given executable file
        /// </summary>
        /// <param name="execFilePath">the program file path to execute</param>
        /// <param name="parameters">the parameter variables for executing file</param>
        public static void ExecuteProgram(String execFilePath, String parameters = null, bool bWaitOnReturn=false)
        {
            try
            {
                string strCommand = execFilePath;
                if (parameters != null && parameters.Length > 0)
                    strCommand += " " + parameters;
                dynamic cmd = AutomationFactory.CreateObject("WScript.Shell");
                cmd.Run(strCommand, 1, bWaitOnReturn);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " >" + ex.StackTrace);
            }
        }
    }
}
