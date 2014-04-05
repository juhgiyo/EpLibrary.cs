/*! 
@file MutexEx.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief MutexEx Interface
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

A MutexEx Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace EpLibrary.cs
{
    /// <summary>
    /// A class that handles the mutex functionality
    /// </summary>
    public class MutexEx:BaseLock
    {
        /// <summary>
        /// lock
        /// </summary>
        private Mutex m_mutex;
        /// <summary>
        /// the flag whether the mutex is owned on creation
        /// </summary>
        private bool m_isInitialOwned;
        /// <summary>
        /// name of the mutex
        /// </summary>
        private String m_name;
        /// <summary>
        /// the flag whether the mutex is abandoned
        /// </summary>
        private bool m_isMutexAbandoned;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="mutexName">name of the mutex</param>
        public MutexEx(String mutexName = null)
            : base()
        {
	        m_isInitialOwned=false;
            m_name=mutexName;
            m_isMutexAbandoned = false;
            m_mutex=new Mutex(m_isInitialOwned,m_name);
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="isInitialOwned">flag to own the mutex on creation</param>
        /// <param name="mutexName">name of the mutex</param>
        public MutexEx(bool isInitialOwned, String mutexName = null)
        {
            m_isInitialOwned=isInitialOwned;
            m_name=mutexName;
            m_isMutexAbandoned = false;
            m_mutex=new Mutex(m_isInitialOwned,m_name);
        }

        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="b">the object to copy from</param>
		public MutexEx(MutexEx b):base(b)
        {
            m_isInitialOwned=b.m_isInitialOwned;
            m_name=b.m_name;
            m_isMutexAbandoned = false;
            m_mutex=new Mutex(m_isInitialOwned,m_name);
        }


        /// <summary>
        /// Locks the critical section
        /// </summary>
        /// <returns>true if locked, otherwise false</returns>
        public override bool Lock()
        {
            try
            {
                return m_mutex.WaitOne();
            }
            catch (AbandonedMutexException ex)
            {
                Console.WriteLine(ex.Message+" >"+ex.StackTrace);
                m_isMutexAbandoned = true;
                return false;
            }
        }

        /// <summary>
        /// Try to lock the critical section
        /// 
        /// If other thread is already in the critical section, it just returns false and continue, otherwise obtain the ciritical section
        /// </summary>
        /// <returns>true if locked, otherwise false</returns>
        public override bool TryLock()
        {
            try
            {
                return m_mutex.WaitOne(0);
            }
            catch (AbandonedMutexException ex)
            {
                Console.WriteLine(ex.Message+" >"+ex.StackTrace);
                m_isMutexAbandoned = true;
                return false;
            }
            
        }

        /// <summary>
        /// Try to lock the critical section for given time
        /// </summary>
        /// <param name="dwMilliSecond">the wait time</param>
        /// <returns>true if locked, otherwise false</returns>
        public override bool TryLockFor(int dwMilliSecond)
        {
            try
            {
                return m_mutex.WaitOne(dwMilliSecond);
            }
            catch (AbandonedMutexException ex)
            {
                Console.WriteLine(ex.Message+" >"+ex.StackTrace);
                m_isMutexAbandoned = true;
                return false;
            }
            
        }

        /// <summary>
        /// Leave the critical section
        /// </summary>
		public override void Unlock()
        {
            m_mutex.ReleaseMutex();
        }

        /// <summary>
        /// Returns the flag whether this mutex is abandoned or not.
        /// </summary>
        /// <returns>true if the mutex is abandoned, otherwise false</returns>
        public bool IsMutexAbandoned()
        {
            return m_isMutexAbandoned;
        }

        
    }
}
