/*! 
@file IpcConf.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief IpcConf Interface
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

A IpcConf Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpLibrary.cs
{
    /// <summary>
    /// Server start status
    /// </summary>
    public enum StartStatus
    {
        /// <summary>
        /// Success
        /// </summary>
        SUCCESS=0,
        /// <summary>
        /// Failed to create pipe
        /// </summary>
        FAIL_PIPE_CREATE_FAILED,
    }
	/// <summary>
    /// Connect Status
	/// </summary>
	public enum ConnectStatus{
		/// <summary>
        /// Success
		/// </summary>
		SUCCESS=0,
        /// <summary>
        /// Failed to wait for connection
        /// </summary>
        FAIL_WAIT_FOR_CONNECTION_FAILED,
		/// <summary>
        /// Pipe open failed
		/// </summary>
		FAIL_PIPE_OPEN_FAILED,
		/// <summary>
        /// ReadMode Set failed
		/// </summary>
		FAIL_SET_READ_MODE_FAILED,
		/// <summary>
        /// Read failed
		/// </summary>
		FAIL_READ_FAILED,
		/// <summary>
        /// Timed Out
		/// </summary>
		FAIL_TIME_OUT,
	}


	/// <summary>
    /// Write Status
	/// </summary>
	public enum WriteStatus{
		/// <summary>
        /// Success
		/// </summary>
		SUCCESS=0,
		/// <summary>
        /// Send failed
		/// </summary>
		FAIL_WRITE_FAILED,
	}

    /// <summary>
    /// Pipe write element
    /// </summary>
	public class PipeWriteElem{
        /// <summary>
        /// offset of start of data
        /// </summary>
        public int m_offset;
        /// <summary>
        /// /// Byte size of the data
        /// </summary>
        public int m_dataSize;
        /// <summary>
        /// Data buffer
        /// </summary>
        public byte[] m_data;

        /// <summary>
        /// Default constructor
        /// </summary>
		public PipeWriteElem()
        {
            m_dataSize=0;
            m_offset = 0;
            m_data=null;
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="data">the byte size of the data</param>
        /// <param name="offset">offset of the byte to start write</param>
        /// <param name="dataSize">byte size of the data to write</param>
        public PipeWriteElem(byte[] data, int offset,int dataSize)
        {
            m_offset = offset;
            m_dataSize=dataSize;
            m_data = data;
        }

	}
    /// <summary>
    /// IPC configuration class
    /// </summary>
    public class IpcConf
    {
        /// <summary>
        /// Unlimited instance of pipe
        /// </summary>
        public const int DEFAULT_PIPE_INSTANCES = 255;
        /// <summary>
        /// Default write buffer size
        /// </summary>
        public const int DEFAULT_WRITE_BUF_SIZE = 4096;
        /// <summary>
        /// Default read buffer size
        /// </summary>
        public const int DEFAULT_READ_BUF_SIZE = 4096;
    }
}
