/*! 
@file InterlockedEx.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief InterlockedEx Interface
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

A InterlockedEx Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace EpLibrary.cs
{
    public sealed class InterlockedEx:BaseLock
    {
        /// <summary>
        /// lock
        /// </summary>
        private int m_interLock;
        /// <summary>
        /// Default constructor
        /// </summary>
        public InterlockedEx():base()
        {
            m_interLock = 0;
        }

        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="b">the object to copy from</param>
        public InterlockedEx(NoLock b)
            : base(b)
        {
            m_interLock = 0;
        }

        ~InterlockedEx()
        {
        }

        /// <summary>
        /// Locks the critical section
        /// </summary>
        /// <returns>true if locked, otherwise false</returns>
        public override bool Lock()
        {
            while (Interlocked.Exchange(ref m_interLock, 1) != 0)
            {
                Thread.Sleep(0);
            }
            return true;
        }

        /// <summary>
        /// Try to lock the critical section
        /// 
        /// If other thread is already in the critical section, it just returns false and continue, otherwise obtain the ciritical section
        /// </summary>
        /// <returns>true if locked, otherwise false</returns>
        public override bool TryLock()
        {
            bool ret = false;
            if (Interlocked.Exchange(ref m_interLock, 1) == 0)
            {
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// Try to lock the critical section for given time
        /// </summary>
        /// <param name="dwMilliSecond">the wait time</param>
        /// <returns>true if locked, otherwise false</returns>
        public override bool TryLockFor(int dwMilliSecond)
        {
            bool ret=false;
	        
	        DateTime startTime;
	        double timeUsed;
	        double waitTime=(double)dwMilliSecond;
	        startTime=DateTimeHelper.GetCurrentDateTime();
	
	        do
	        {
		        if(Interlocked.Exchange(ref m_interLock, 1) != 0)
		        {
			        Thread.Sleep(0);
			        timeUsed=DateTimeHelper.AbsDiffInMilliSec(DateTimeHelper.GetCurrentDateTime(),startTime);
			        waitTime=waitTime-timeUsed;
			        startTime=DateTimeHelper.GetCurrentDateTime();
		        }
		        else
		        {
			        ret=true;
		        }
	        }while(waitTime>0.0);
            return ret;
        }

        /// <summary>
        /// Leave the critical section
        /// </summary>
        public override void Unlock()
        {
            Interlocked.Exchange(ref m_interLock, 0);
        }
    }
}
