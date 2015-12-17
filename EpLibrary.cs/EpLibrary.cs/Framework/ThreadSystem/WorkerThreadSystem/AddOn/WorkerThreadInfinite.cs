/*! 
@file WorkerThreadInfinite.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief WorkerThreadInfinite Interface
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

A WorkerThreadInfinite Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Diagnostics;

namespace EpLibrary.cs
{
    /// <summary>
    /// A class that implements infinite-looping Worker Thread Class.
    /// </summary>
    public sealed class WorkerThreadInfinite:BaseWorkerThread, IDisposable
    {

        /// <summary>
        /// Terminate Signal Event
        /// </summary>
        private EventEx m_terminateEvent;


        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="policy">the life policy of this worker thread.</param>
        public WorkerThreadInfinite(ThreadLifePolicy policy):base(policy)
        {
            m_terminateEvent=new EventEx(false,EventResetMode.AutoReset);
        }

        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="b">the object to copy from</param>
		public WorkerThreadInfinite(WorkerThreadInfinite  b):base(b)
        {
            m_terminateEvent=new EventEx(false,EventResetMode.AutoReset);
        }


        /// <summary>
        /// Wait for worker thread to terminate, and if not terminated, then Terminate.
        /// </summary>
        /// <param name="waitTimeInMilliSec">the time-out interval, in milliseconds.</param>
        /// <returns>the terminate result of the thread</returns>
        public new TerminateResult TerminateWorker(int waitTimeInMilliSec= Timeout.Infinite)
        {
            m_terminateEvent.SetEvent();
	        Resume();
	        return TerminateAfter(waitTimeInMilliSec);
        }

        /// <summary>
        /// Actual infinite-looping Thread Code.
        /// </summary>
        protected override void execute()
        {
            try
            {
                while (true)
                {
                    if (m_terminateEvent.WaitForEvent(0))
                    {
                        return;
                    }
                    if (m_workPool.IsEmpty())
                    {
                        if (m_lifePolicy == ThreadLifePolicy.SUSPEND_AFTER_WORK)
                        {
                            callCallBack();
                            Suspend();
                            continue;
                        }
                        callCallBack();
                        Thread.Sleep(0);
                        continue;
                    }
                    Debug.Assert(m_jobProcessor != null,"Job Processor is NULL!");
                    if (m_jobProcessor == null)
                        break;
                    BaseJob jobPtr = m_workPool.Dequeue();
                    jobPtr.JobReport(JobStatus.IN_PROCESS);
                    m_jobProcessor.DoJob(this, jobPtr);
                    jobPtr.JobReport(JobStatus.DONE);

                }
            }
            catch (ThreadAbortException)
            {
                while (m_workPool.IsEmpty())
                {
                    BaseJob jobPtr = m_workPool.Dequeue();
                    jobPtr.JobReport(JobStatus.INCOMPLETE);
                }
            }
        }

        bool m_disposed = false;

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        private void Dispose(bool disposing)
        {
            if (m_disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                if (m_terminateEvent != null)
                {
                    m_terminateEvent.Dispose();
                    m_terminateEvent = null;
                }
            }

            // Free any unmanaged objects here.
            m_disposed = true;
        }

    }
}
