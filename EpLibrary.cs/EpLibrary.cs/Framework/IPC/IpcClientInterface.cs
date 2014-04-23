/*! 
@file IpcClientInterface.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief IpcClientInterface
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

A IpcClientInterface.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Pipes;
using System.Threading;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Diagnostics;

namespace EpLibrary.cs
{
    /// <summary>
    /// IPC Pipe Options
    /// </summary>
    public class IpcClientOps
    {
        /// <summary>
        /// Callback Object
        /// </summary>
        public IpcClientCallbackInterface m_callBackObj;

        /// <summary>
        /// Domain
        /// </summary>
        public string m_domain;
        /// <summary>
        /// Name of the pipe
        /// </summary>
        public string m_pipeName;

        /// <summary>
        /// read byte size
        /// </summary>
        public int m_numOfReadBytes;
        /// <summary>
        /// write byte size
        /// </summary>
        public int m_numOfWriteBytes;

          /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="pipeName">the name of the pipe</param>
        /// <param name="maximumInstance">maximum number of pipe instance</param>
        /// <param name="numOfReadyBytes">maximum read buffer size</param>
        /// <param name="numOfWriteBytes">maximum write buffer size</param>
        /// <param name="callBackObj">callback object</param>
        public IpcClientOps(string domain, string pipeName, IpcClientCallbackInterface callBackObj, int numOfReadyBytes = IpcConf.DEFAULT_READ_BUF_SIZE, int numOfWriteBytes = IpcConf.DEFAULT_WRITE_BUF_SIZE)
        {
            m_domain = domain;
            m_pipeName = pipeName;
            m_callBackObj = callBackObj;
            m_numOfReadBytes = numOfReadyBytes;
            m_numOfWriteBytes = numOfWriteBytes;
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public IpcClientOps()
        {
            m_domain = ".";
            m_pipeName = null;
            m_callBackObj = null;
            m_numOfReadBytes = IpcConf.DEFAULT_READ_BUF_SIZE;
            m_numOfWriteBytes = IpcConf.DEFAULT_WRITE_BUF_SIZE;


        }

        /// <summary>
        /// Default IPC Pipe options
        /// </summary>
        public static IpcClientOps defaultIpcClientOps = new IpcClientOps();
    }
  
    /// <summary>
    /// IPC Client Interface
    /// </summary>
    public interface IpcClientInterface
    {
        /// <summary>
        /// Get the pipe name of server
        /// </summary>
        /// <returns>the pipe name in string</returns>
        string GetFullPipeName();

        /// <summary>
        ///  Connect to the server
        /// </summary>
        /// <param name="ops">the client options</param>
        /// <param name="waitTimeInMilliSec">connect wait time in milliseconds</param>
        void Connect(IpcClientOps ops, int waitTimeInMilliSec=Timeout.Infinite);

        /// <summary>
        /// Disconnect from the server
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Check if the client is connected to server
        /// </summary>
        /// <returns>true if the client is connected to server otherwise false</returns>
        bool IsConnected();

        /// <summary>
        ///  Get the maximum write data byte size
        /// </summary>
        /// <returns>the maximum write data byte size</returns>
        int GetMaxWriteDataByteSize();

        /// <summary>
        /// Get the maximum read data byte size
        /// </summary>
        /// <returns>the maximum read data byte size</returns>
        int GetMaxReadDataByteSize();

        /// <summary>
        /// Write data to the pipe
        /// </summary>
        /// <param name="data"> the data to write</param>
        /// <param name="offset">offset to start write from given data</param>
        /// <param name="dataByteSize">byte size of the data</param>
        void Write(byte[] data, int offset, int dataByteSize);
    }

    /// <summary>
    /// Client Callback Interface
    /// </summary>
    public interface IpcClientCallbackInterface
    {
        /// <summary>
        ///  When connected to client
        /// </summary>
        /// <param name="pipe">the pipe</param>
        /// <param name="status">status of connect</param>
        void OnConnected(IpcClientInterface pipe, IpcConnectStatus status);
        /// <summary>
        /// Received the data from the client.
        /// </summary>
        /// <param name="pipe">the pipe which received the packet</param>
        /// <param name="receivedData">the received data</param>
        /// <param name="receivedDataByteSize">the received data byte size</param>
        void OnReadComplete(IpcClientInterface pipe, byte[] receivedData, int bytes);
        /// <summary>
        /// Received the packet from the client.
        /// </summary>
        /// <param name="pipe">the pipe which wrote the packet</param>
        /// <param name="status">the status of write</param>
        void OnWriteComplete(IpcClientInterface pipe, IpcWriteStatus status);
        /// <summary>
        ///  The pipe is disconnected.
        /// </summary>
        /// <param name="pipe">the pipe, disconnected.</param>
        void OnDisconnected(IpcClientInterface pipe);
    }
}
