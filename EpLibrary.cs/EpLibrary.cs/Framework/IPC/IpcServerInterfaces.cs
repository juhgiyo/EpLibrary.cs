/*! 
@file IpcServerInterface.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief IpcServerInterface Interface
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

A IpcServerInterface Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EpLibrary.cs
{
    /// <summary>
    /// Server option class
    /// </summary>
	public class IpcServerOps{
        /// <summary>
        /// Callback Object
        /// </summary>
		public IpcServerCallbackInterface m_callBackObj;
		
		/// <summary>
        /// Name of the pipe
		/// </summary>
        public string m_pipeName;
		
        /// <summary>
        /// The maximum possible number of pipe instances
        /// </summary>
        public int m_maximumInstances;
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
        public IpcServerOps(string pipeName, IpcServerCallbackInterface callBackObj, int maximumInstance = IpcConf.DEFAULT_PIPE_INSTANCES, int numOfReadyBytes = IpcConf.DEFAULT_READ_BUF_SIZE, int numOfWriteBytes = IpcConf.DEFAULT_WRITE_BUF_SIZE)
        {
            m_pipeName=pipeName;
            m_maximumInstances=maximumInstance;
            m_numOfReadBytes = numOfReadyBytes;
            m_numOfWriteBytes=numOfWriteBytes;
            m_callBackObj=callBackObj;
        }
        /// <summary>
        /// Default Constructor
        /// </summary>
		public IpcServerOps()
		{
            m_callBackObj = null;
            m_pipeName = null;
            m_maximumInstances = IpcConf.DEFAULT_PIPE_INSTANCES;
            m_numOfReadBytes = IpcConf.DEFAULT_READ_BUF_SIZE;
            m_numOfWriteBytes = IpcConf.DEFAULT_WRITE_BUF_SIZE;

		}

		/// <summary>
        /// Default IPC Server options
		/// </summary>
		public static IpcServerOps defaultIpcServerOps;
	};

    /// <summary>
    /// IPC Server Interface
    /// </summary>
	public interface IpcServerInterface{

        /// <summary>
        /// Get the pipe name of server
        /// </summary>
        /// <returns>the pipe name in string</returns>
		string GetFullPipeName();


        /// <summary>
        /// Get the Maximum Instances of server
        /// </summary>
        /// <returns>the Maximum Instances</returns>
		int GetMaximumInstances();


        /// <summary>
        /// Start the server
        /// </summary>
        /// <param name="ops">the server options</param>
		void StartServer(IpcServerOps ops);
        /// <summary>
        /// Stop the server
        /// </summary>
		void StopServer();

        /// <summary>
        /// Check if the server is started
        /// </summary>
        /// <returns>true if the server is started otherwise false</returns>
		bool IsServerStarted();

        /// <summary>
        /// Terminate all clients' socket connected.
        /// </summary>
		void ShutdownAllClient();

        /// <summary>
        /// Get the maximum write data byte size
        /// </summary>
        /// <returns>the maximum write data byte size</returns>
		int GetMaxWriteDataByteSize();
        /// <summary>
        /// Get the maximum read data byte size
        /// </summary>
        /// <returns>the maximum read data byte size</returns>
		int GetMaxReadDataByteSize();

        /// <summary>
        /// Return the number of pipe connected
        /// </summary>
        /// <returns>number of pipe connected</returns>
        int GetPipeCount();
	}

    /// <summary>
    /// IPC Interface
    /// </summary>
	public interface IpcInterface{

        /// <summary>
        /// Write data to the pipe
        /// </summary>
        /// <param name="data">the data to write</param>
        /// <param name="offset">offset to start write from given data</param>
        /// <param name="dataByteSize">byte size of the data to write</param>
		void Write(byte[] data, int offset ,int dataByteSize);

        /// <summary>
        /// Check if the connection is alive
        /// </summary>
        /// <returns>true if the connection is alive otherwise false</returns>
		bool IsConnectionAlive();

        /// <summary>
        /// Kill the connection
        /// </summary>
		void KillConnection();

        /// <summary>
        /// Kill current connection and wait for other connection
        /// </summary>
        void Reconnect();
	}

    /// <summary>
    /// Server Callback Interface
    /// </summary>
    public interface IpcServerCallbackInterface
    {
        /// <summary>
        /// When server started
        /// </summary>
        /// <param name="server">the server</param>
        /// <param name="status">started status</param>
        void OnServerStarted(IpcServerInterface server, IpcStartStatus status);

        /// <summary>
        /// When accepted client tries to make connection.
        /// </summary>
        /// <param name="server">the server</param>
        /// <param name="pipe">the pipe connected</param>
        /// <param name="status">status of connection</param>
        /// <remarks>When this function calls, it is right before making connection,
        /// so user can configure the pipe before the connection is actually made.</remarks>
        void OnNewConnection(IpcServerInterface server, IpcInterface pipe, IpcConnectStatus status);

        /// <summary>
        /// Received the data from the client.
        /// </summary>
        /// <param name="server">the server</param>
        /// <param name="pipe">the pipe which received the packet</param>
        /// <param name="receivedData">the received data</param>
        /// <param name="receivedDataByteSize">the received data byte size</param>
        void OnReadComplete(IpcServerInterface server, IpcInterface pipe, byte[] receivedData, int receivedDataByteSize);

        /// <summary>
        /// When write completed to pipe
        /// </summary>
        /// <param name="server">the server</param>
        /// <param name="pipe">the pipe which wrote the packet</param>
        /// <param name="status">the status of write</param>
        void OnWriteComplete(IpcServerInterface server, IpcInterface pipe, IpcWriteStatus status);

        /// <summary>
        /// when pipe is disconnected
        /// </summary>
        /// <param name="server">the server</param>
        /// <param name="pipe">the pipe, disconnected</param>
        void OnDisconnected(IpcServerInterface server, IpcInterface pipe);
    };

}
