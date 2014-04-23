/*! 
@file IpcClient.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief IpcClient Interface
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

A IpcClient Class.

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
    public sealed class IpcClient : ThreadEx, IpcClientInterface
    {
        /// <summary>
        /// connect wait time in milliseconds
        /// </summary>
        int m_connectWaitTimeInMillisec = 0;
        /// <summary>
        /// Pipe handle
        /// </summary>
        NamedPipeClientStream m_pipeHandle;
        /// <summary>
        /// flag whether the server is started
        /// </summary>
        bool m_connected;
        /// <summary>
        /// IPC client options
        /// </summary>
        IpcClientOps m_options;

        /// <summary>
        /// Write buffer queue
        /// </summary>
        Queue<PipeWriteElem> m_writeQueue=new Queue<PipeWriteElem>();
        /// <summary>
        /// Read buffer
        /// </summary>
        byte[] m_readBuffer;

        /// <summary>
        /// General lock object
        /// </summary>
        Object m_generalLock = new Object();
        
        /// <summary>
        /// Default Constructor
        /// </summary>
		public IpcClient()
        {
            m_readBuffer = null;
        }

        /// <summary>
        /// Default Destructor
        /// </summary>
		~IpcClient()
        {
            Disconnect();
        }

        /// <summary>
        /// Get the pipe name of server
        /// </summary>
        /// <returns> the pipe name in string</returns>
		public string GetFullPipeName()
        {
            return m_options.m_pipeName;
        }

		

        /// <summary>
        /// Connect to the server
        /// </summary>
        /// <param name="ops">the client options</param>
        /// <param name="waitTimeInMilliSec"> the wait time for connection in milli-second.</param>
		public void Connect(IpcClientOps ops, int waitTimeInMilliSec)
        {
            lock (m_generalLock)
            {
                if (IsConnected())
                    return;
            }

            if (ops == null)
                ops = IpcClientOps.defaultIpcClientOps;
            if (ops.m_callBackObj != null)
                throw new ArgumentException("callBackObj is null!");
            lock (m_generalLock)
            {
                m_options = ops;
                if (ops.m_numOfWriteBytes == 0)
                    m_options.m_numOfWriteBytes = IpcConf.DEFAULT_WRITE_BUF_SIZE;
                if (ops.m_numOfReadBytes == 0)
                    m_options.m_numOfReadBytes = IpcConf.DEFAULT_READ_BUF_SIZE;

            }
            m_readBuffer = new byte[m_options.m_numOfReadBytes];
            m_connectWaitTimeInMillisec = waitTimeInMilliSec;
            Start();

        }
        /// <summary>
        /// Actual connect function
        /// </summary>
        protected override void execute()
        {
            lock (m_generalLock)
            {
 
                try
                {
                    m_pipeHandle = new NamedPipeClientStream(
                    m_options.m_domain,   // pipe name 
                    m_options.m_pipeName,
                    PipeDirection.InOut,
                    PipeOptions.Asynchronous | PipeOptions.WriteThrough
                    );          // no template file 

                    // Break if the pipe handle is valid. 
                    m_pipeHandle.Connect(m_connectWaitTimeInMillisec);

                }
                catch (TimeoutException)
                {
                    m_options.m_callBackObj.OnConnected(this, ConnectStatus.FAIL_TIME_OUT);
                    return;
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message + " >" + ex.StackTrace);
                    m_options.m_callBackObj.OnConnected(this, ConnectStatus.FAIL_PIPE_OPEN_FAILED);
                    return;
                }


                try
                {
                    m_pipeHandle.ReadMode = PipeTransmissionMode.Message;
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message + " >" + ex.StackTrace);
                    m_options.m_callBackObj.OnConnected(this, ConnectStatus.FAIL_SET_READ_MODE_FAILED);
                    return;
                }

            }
            startReceive();
        }

        /// <summary>
        /// start receiving from pipe
        /// </summary>
        private void startReceive()
        {

            try
            {
                m_pipeHandle.BeginRead(m_readBuffer, 0, m_options.m_numOfReadBytes, OnReadComplete, this);
                m_connected = true;
                m_options.m_callBackObj.OnConnected(this, ConnectStatus.SUCCESS);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message + " >" + ex.StackTrace);
                Disconnect();
                m_options.m_callBackObj.OnConnected(this, ConnectStatus.FAIL_READ_FAILED);
            }
        }

        /// <summary>
        /// Stop the server
        /// </summary>
		public void Disconnect()
        {
            lock (m_generalLock)
            {
                if (!IsConnected())
                {
                    return;
                }
                m_pipeHandle.Close();
                m_pipeHandle = null;
                m_connected = false;
            }
            
            lock (m_writeQueue)
            {
                m_writeQueue.Clear();
            }
            Thread t = new Thread(delegate()
            {
                m_options.m_callBackObj.OnDisconnected(this);
            });
            t.Start();
        }

        /// <summary>
        /// Check if the client is connected to server
        /// </summary>
        /// <returns>true if the client is connected to server otherwise false</returns>
      
		public bool IsConnected()
        {
            return m_connected;
        }

        /// <summary>
        ///  Get the maximum write data byte size
        /// </summary>
        /// <returns>the maximum write data byte size</returns>
        public int GetMaxWriteDataByteSize()
        {
            return m_options.m_numOfWriteBytes;
        }

        /// <summary>
        /// Get the maximum read data byte size
        /// </summary>
        /// <returns>the maximum read data byte size</returns>
		public int GetMaxReadDataByteSize()
        {
            return m_options.m_numOfReadBytes;
        }


        /// <summary>
        /// Write data to the pipe
        /// </summary>
        /// <param name="data"> the data to write</param>
        /// <param name="offset">offset to start write from given data</param>
        /// <param name="dataByteSize">byte size of the data</param>
		public void Write(byte[] data,int offset,int dataByteSize)
        {
            if (dataByteSize > m_options.m_numOfWriteBytes)
                throw new ArgumentException();

	        PipeWriteElem elem=new PipeWriteElem(data,dataByteSize,offset);

	        lock(m_writeQueue)
            {
	            if(m_writeQueue.Count()>0)
	            {
		            m_writeQueue.Enqueue(elem);
	            }
	            else
	            {
                    try
                    {
                        m_pipeHandle.BeginWrite(elem.m_data,elem.m_offset,elem.m_dataSize,OnWriteComplete,this);
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex.Message + " >" + ex.StackTrace);
    	                Disconnect(); 
                    }
	            }
            }

        }
	
        /// <summary>
        /// Handles when Read is completed
        /// </summary>
        /// <param name="result">AsyncResult</param>
		private void OnReadComplete(IAsyncResult result)
        {
            IpcClient  pipeInst = (IpcClient ) result.AsyncState; 
            int readByte=0;
            byte[] readBuffer=null;
            try
            {
                readByte=pipeInst.m_pipeHandle.EndRead(result);
                readBuffer=m_readBuffer.ToArray();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message + " >" + ex.StackTrace);
                Disconnect();
                return;
            }
            try
            {
                pipeInst.m_pipeHandle.BeginRead(pipeInst.m_readBuffer, 0, pipeInst.m_options.m_numOfReadBytes, OnReadComplete, pipeInst);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message + " >" + ex.StackTrace);
                Disconnect();
            }

            pipeInst.m_options.m_callBackObj.OnReadComplete(pipeInst, readBuffer, readByte);
        }

        /// <summary>
        /// Handles when Write is completed
        /// </summary>
        /// <param name="result">AsyncResult</param>
        private void OnWriteComplete(IAsyncResult result)
        {
            IpcClient pipeInst = (IpcClient)result.AsyncState;
            
            try
            {
                pipeInst.m_pipeHandle.EndWrite(result);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message + " >" + ex.StackTrace);
                Disconnect();
                pipeInst.m_options.m_callBackObj.OnWriteComplete(pipeInst, WriteStatus.FAIL_WRITE_FAILED);
                return;
            }

            lock (pipeInst.m_writeQueue)
            {
                if (pipeInst.m_writeQueue.Count > 0)
                {
                    PipeWriteElem elem = pipeInst.m_writeQueue.Dequeue();
                    if (pipeInst.m_writeQueue.Count()>0)
                    {
                        PipeWriteElem nextElem = pipeInst.m_writeQueue.Dequeue();

                        try
                        {
                            m_pipeHandle.BeginWrite(nextElem.m_data, nextElem.m_offset, nextElem.m_dataSize, OnWriteComplete, this);
                        }
                        catch (System.Exception ex)
                        {
                            Console.WriteLine(ex.Message + " >" + ex.StackTrace);
                            pipeInst.m_options.m_callBackObj.OnWriteComplete(pipeInst, WriteStatus.SUCCESS);
                            Disconnect();
                            pipeInst.m_options.m_callBackObj.OnWriteComplete(pipeInst, WriteStatus.FAIL_WRITE_FAILED);
                            return;    
                        }
                    }
                    pipeInst.m_options.m_callBackObj.OnWriteComplete(pipeInst, WriteStatus.SUCCESS);
                }
            }
        }

    }
}
