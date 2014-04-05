/*! 
@file WorkerThreadSingle.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief WorkerThreadSingle Interface
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

A WorkerThreadSingle Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace EpLibrary.cs
{
    /// <summary>
    /// A class that implements single-job Worker Thread Class.
    /// </summary>
    public sealed class WorkerThreadSingle:BaseWorkerThread
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="policy">the life policy of this worker thread.</param>
        public WorkerThreadSingle(ThreadLifePolicy policy):base(policy)
		{}

        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="b">the object to copy from</param>
        public WorkerThreadSingle(WorkerThreadSingle b)
            : base(b)
		{}
		

        /// <summary>
        /// Actual single-job Thread Code.
        /// </summary>
        protected override void execute()
        {
            try
            {
                while (true)
                {
                    if (m_workPool.IsEmpty())
                        break;
                    Debug.Assert(m_jobProcessor != null, "Job Processor is NULL!");
                    if (m_jobProcessor == null)
                        break;
                    BaseJob jobPtr = m_workPool.Dequeue();
                    jobPtr.JobReport(JobStatus.IN_PROCESS);
                    m_jobProcessor.DoJob(this, jobPtr);
                    jobPtr.JobReport(JobStatus.DONE);
                }
                callCallBack();
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
    }
}
