/*! 
@file WebRequestEx.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief WebRequestEx Interface
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

A WebRequestEx Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Net;
using System.Threading;

namespace EpLibrary.cs
{

    /// <summary>
    /// Download file status
    /// </summary>
    public enum DownloadFileStatus
    {
        /// <summary>
        /// Success
        /// </summary>
        SUCCESS,
        /// <summary>
        /// Failed
        /// </summary>
        FAILED
    }

    /// <summary>
    /// WebRequestEx class
    /// </summary>
    public class WebRequestEx
    {
        /// <summary>
        /// Get response from the given uri with given credentials
        /// </summary>
        /// <param name="uri">uri</param>
        /// <param name="credentials">credentials</param>
        /// <param name="waitTimeInMilliSec">wait time in milliseconds</param>
        /// <returns></returns>
        public static String GetResponse(String uri, ICredentials credentials = null, int waitTimeInMilliSec = Timeout.Infinite)
        {
            if (uri == null || uri.Length == 0)
                throw new Exception("Must supply valid URI!");

            WebClient client = new WebClient(); WebRequest.Create(uri);
            string result = null;
            if (credentials != null)
                client.Credentials = credentials;
            EventEx doneEvent = new EventEx(false, EventResetMode.AutoReset);
            client.DownloadStringCompleted += (s, e) =>
            {
               result=e.Result;
               doneEvent.SetEvent();
            };
            client.DownloadStringAsync(new Uri(uri, UriKind.Absolute));
            doneEvent.WaitForEvent(waitTimeInMilliSec);
            return result;
        }

        /// <summary>
        /// Get response from the given uri with given credentials
        /// </summary>
        /// <param name="uri">uri</param>
        /// <param name="credentials">credentials</param>
        /// <param name="callbackFunc">callback function</param>
        public static void GetResponseAsync(String uri, Action<String> callbackFunc, ICredentials credentials=null)
        {
            if (uri == null || uri.Length == 0)
                throw new Exception("Must supply valid URI!");

            WebClient client = new WebClient(); WebRequest.Create(uri);
            if (credentials != null)
                client.Credentials = credentials;
            client.DownloadStringCompleted += (s, e) =>
            {
                callbackFunc(e.Result);
            }; 
            client.DownloadStringAsync(new Uri(uri, UriKind.Absolute));
        }

        /// <summary>
        /// Download file from given uri to given filepath
        /// </summary>
        /// <param name="uri">uri</param>
        /// <param name="filepath">filepath</param>
        /// <param name="waitTimeInMilliSec">wait time in milliseconds</param>
        public static void DownloadFile(String uri, String filepath, int waitTimeInMilliSec = Timeout.Infinite)
        {
            if (uri == null || uri.Length == 0)
                throw new Exception("Must supply valid URI!");
            if (filepath == null || filepath.Length == 0)
                throw new Exception("Must supply valid filepath!");
            WebClient webClient = new WebClient();
            EventEx doneEvent=new EventEx(false,EventResetMode.AutoReset);
            webClient.OpenReadCompleted += (s, e) =>
            {
                try
                {
                    using ( FileStream stream = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                    {
                        e.Result.CopyTo(stream);
                        stream.Flush();
                        stream.Close();
                        doneEvent.SetEvent();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + " >" + ex.StackTrace);
                }
            };
            webClient.OpenReadAsync(new Uri(uri, UriKind.Absolute));
            doneEvent.WaitForEvent(waitTimeInMilliSec);
        }

        /// <summary>
        /// Download data from given uri and call callback
        /// </summary>
        /// <param name="uri">uri</param>
        /// <param name="callbackFunc">callback function</param>
        public static void DownloadDataAsync(String uri, Action<Stream> callbackFunc)
        {
            if (uri == null || uri.Length == 0)
                throw new Exception("Must supply valid URI!");
            WebClient webClient = new WebClient();
            webClient.OpenReadCompleted += (s, e) =>
            {
                try
                {
                    callbackFunc(e.Result);
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + " >" + ex.StackTrace);
                }
            };
            webClient.OpenReadAsync(new Uri(uri, UriKind.Absolute));
        }

        /// <summary>
        /// Download file from given uri to given filepath and call callback
        /// </summary>
        /// <param name="uri">uril</param>
        /// <param name="filepath">filepath</param>
        /// <param name="callbackFunc">callback function</param>
        public static void DownloadFileAsync(String uri, String filepath, Action<DownloadFileStatus> callbackFunc)
        {
            if (uri == null || uri.Length == 0)
                throw new Exception("Must supply valid URI!");
            if (filepath == null || filepath.Length == 0)
                throw new Exception("Must supply valid filepath!");
            WebClient webClient = new WebClient();
            webClient.OpenReadCompleted += (s, e) =>
            {
                try
                {
                    using (FileStream stream = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                    {
                        e.Result.CopyTo(stream);
                        stream.Flush();
                        stream.Close();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + " >" + ex.StackTrace);
                }
            };
            webClient.OpenReadAsync(new Uri(uri, UriKind.Absolute));
        }

      

    }
}
