/*! 
@file BaseJobProcessor.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief BaseJobProcessor Interface
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

A BaseJobProcessor Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpLibrary.cs
{
    /// <summary>
    /// Enumeration for Job Processor Status
    /// </summary>
    public enum JobProcessorStatus
    {
        /// <summary>
        /// Job Processor never entered to the scheduler
        /// </summary>
        NONE = 0,
        /// <summary>
        /// Job Processor is in the Pending State
        /// </summary>
        PENDING,
        /// <summary>
        /// Job Processor is in the Process
        /// </summary>
        IN_PROCESS,
        /// <summary>
        /// Job Processor is finished working
        /// </summary>
        DONE,
        /// <summary>
        /// Job Processor is suspended by other processor
        /// </summary>
        SUSPENDED,
        /// <summary>
        /// Job Processor terminated due to Thread Error or Internal Error
        /// </summary>
        INCOMPLETE,
        /// <summary>
        /// Job PRocessor TImed Out
        /// </summary>
        TIMEOUT,
    };

    /// <summary>
    /// A base class for Job Processing Objects.
    /// </summary>
    public abstract class BaseJobProcessor
    {


        /// <summary>
        /// Process the job given, subclasses must implement this function.
        /// </summary>
        /// <param name="workerThread">The worker thread which called the DoJob.</param>
        /// <param name="data">The job given to this object.</param>
        public abstract void DoJob(BaseWorkerThread workerThread, BaseJob data);

        /// <summary>
        /// Handles when Job Status Changed
        /// </summary>
        /// <param name="status">The Status of the Job</param>
        /// <remarks>Subclass should overwrite this function!!</remarks>
		protected virtual void handleReport(JobProcessorStatus status)
        {

        }

        /// <summary>
        /// Default constructor
        /// </summary>
        protected BaseJobProcessor()
        {
            m_status = JobProcessorStatus.NONE;
        }

        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="b">the object to copy from</param>
		protected BaseJobProcessor(BaseJobProcessor b)
		{
			m_status=b.m_status;
		}

        /// <summary>
        /// Call Back Function When Job's Status Changed.
        /// </summary>
        /// <param name="status">The Status of the Job</param>
        private void JobProcessorReport(JobProcessorStatus status)
        {
            handleReport(status);
            m_status = status;
        }


        /// <summary>
        /// current Job Processor Status
        /// </summary>
		private JobProcessorStatus m_status;
    }
}
