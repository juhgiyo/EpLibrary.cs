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
using System.Threading.Tasks;
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
        /// <returns></returns>
        public static String GetResponse(String uri=null, ICredentials credentials=null)
        {
            if (uri == null || uri.Length == 0)
                throw new Exception("Must supply valid URI!");
            ICredentials useCredentials = CredentialCache.DefaultCredentials;
            WebRequest request;
            if (credentials != null)
                useCredentials = credentials;


            try
            {
                request = WebRequest.Create(uri);
                request.Credentials = useCredentials;
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                String responseFromServer = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return responseFromServer;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Get response from the given uri with given credentials
        /// </summary>
        /// <param name="uri">uri</param>
        /// <param name="credentials">credentials</param>
        /// <param name="callbackFunc">callback function</param>
        public static void GetResponseAsync(String uri, ICredentials credentials, Action<String> callbackFunc)
        {
            if (uri == null || uri.Length == 0)
                throw new Exception("Must supply valid URI!");
            ICredentials useCredentials = CredentialCache.DefaultCredentials;
            WebRequest request;
            if (credentials != null)
                useCredentials = credentials;


            try
            {
                request = WebRequest.Create(uri);
                request.Credentials = useCredentials;
                WebResponse response = request.GetResponse();
                RequestTranporter transporter = new RequestTranporter(request, callbackFunc);
                request.BeginGetResponse(new AsyncCallback(RespCallback), transporter);
            }
            catch
            {
                if (callbackFunc != null)
                    callbackFunc(null);
            }
        }

        /// <summary>
        /// Download data from given uri
        /// </summary>
        /// <param name="uri">uri</param>
        /// <returns>downloaded data</returns>
        public static byte[] DownloadData(String uri)
        {
            WebClient webClient = new WebClient();
            byte[] myDataBuffer = webClient.DownloadData(uri);
            return myDataBuffer;
        }
        /// <summary>
        /// Download file from given uri to given filepath
        /// </summary>
        /// <param name="uri">uri</param>
        /// <param name="filepath">filepath</param>
        public static void DownloadFile(String uri, String filepath)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(uri, filepath);
        }

        /// <summary>
        /// Download data from given uri and call callback
        /// </summary>
        /// <param name="uri">uri</param>
        /// <param name="callbackFunc">callback function</param>
        public static void DownloadDataAsync(String uri, Action<byte[]> callbackFunc)
        {
            if (uri == null || uri.Length == 0)
                throw new Exception("Must supply Valid URI!");
            ThreadEx thread = new ThreadEx(downloadData, new DownloadDataTranporter(uri, callbackFunc));
            thread.Start();
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
            ThreadEx thread = new ThreadEx(downloadFile, new DownloadFileTranporter(uri, filepath, callbackFunc));
            thread.Start();
        }

        /// <summary>
        /// Request Transporter class
        /// </summary>
        private class RequestTranporter
        {
            /// <summary>
            /// WebRequest callback object
            /// </summary>
            public Action<String> m_callbackFunc;
            /// <summary>
            /// WebRequest object
            /// </summary>
            public WebRequest m_request;
            
            /// <summary>
            /// Default constructor
            /// </summary>
            /// <param name="request">request object</param>
            /// <param name="callbackFunc">callback function</param>
            public RequestTranporter(WebRequest request, Action<String> callbackFunc)
            {
                m_callbackFunc = callbackFunc;
                m_request = request;
            }
        }
        /// <summary>
        /// Request callback function
        /// </summary>
        /// <param name="asynchronousResult">RequestTransporter object</param>
        private static void RespCallback(IAsyncResult asynchronousResult)
        {
            RequestTranporter webRequestEx = null;
            try
            {
                // State of request is asynchronous.
                webRequestEx = (RequestTranporter)asynchronousResult.AsyncState;
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)webRequestEx.m_request;
                WebResponse response = (HttpWebResponse)myHttpWebRequest.EndGetResponse(asynchronousResult);

                // Read the response into a Stream object.
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                String responseFromServer = reader.ReadToEnd();
                reader.Close();
                response.Close();
                if(webRequestEx.m_callbackFunc!=null)
                    webRequestEx.m_callbackFunc(responseFromServer);
                return;
            }
            catch (Exception)
            {
                if (webRequestEx.m_callbackFunc != null)
                    webRequestEx.m_callbackFunc(null);
            }
        }



        /// <summary>
        /// download data thread funtion
        /// </summary>
        /// <param name="param">download data transporter object</param>
        private static void downloadData(object param)
        {
            DownloadDataTranporter tranporter = (DownloadDataTranporter)param;
            WebClient webClient = new WebClient();
            try
            {
                byte[] myDataBuffer = webClient.DownloadData(tranporter.m_uri);
                if(tranporter.m_callbackFunc!=null)
                    tranporter.m_callbackFunc(myDataBuffer);
            }
            catch
            {
                if (tranporter.m_callbackFunc != null)
                    tranporter.m_callbackFunc(null);
            }
            
        }
        /// <summary>
        /// Download data tranporter class
        /// </summary>
        private class DownloadDataTranporter
        {
            /// <summary>
            /// uri
            /// </summary>
            public String m_uri;
            /// <summary>
            /// callback function
            /// </summary>
            public Action<byte[]> m_callbackFunc;
            /// <summary>
            /// Default constructor
            /// </summary>
            /// <param name="uri">uri</param>
            /// <param name="callbackFunc">callback function</param>
            public DownloadDataTranporter(String uri, Action<byte[]> callbackFunc)
            {
                m_uri = uri;
                m_callbackFunc = callbackFunc;
            }
        }

        /// <summary>
        /// download file thread function
        /// </summary>
        /// <param name="param">download file tranporter object</param>
        private static void downloadFile(object param)
        {
            DownloadFileTranporter tranporter = (DownloadFileTranporter)param;
            WebClient webClient = new WebClient();
            try
            {
               webClient.DownloadFile(tranporter.m_uri,tranporter.m_filepath);
                if (tranporter.m_callbackFunc != null)
                    tranporter.m_callbackFunc(DownloadFileStatus.SUCCESS);
            }
            catch
            {
                if (tranporter.m_callbackFunc != null)
                    tranporter.m_callbackFunc(DownloadFileStatus.FAILED);
            }

        }
        /// <summary>
        /// Download file tranporter class
        /// </summary>
        private class DownloadFileTranporter
        {
            /// <summary>
            /// uri
            /// </summary>
            public String m_uri;
            /// <summary>
            /// filepath
            /// </summary>
            public String m_filepath;
            /// <summary>
            /// m_callbackFunc
            /// </summary>
            public Action<DownloadFileStatus> m_callbackFunc;
            /// <summary>
            /// Default constructor
            /// </summary>
            /// <param name="uri">uri</param>
            /// <param name="filepath">filepath</param>
            /// <param name="callbackFunc">callback function</param>
            public DownloadFileTranporter(String uri, String filepath, Action<DownloadFileStatus> callbackFunc)
            {
                m_uri = uri;
                m_filepath = filepath;
                m_callbackFunc = callbackFunc;
            }
        }

    }
}
