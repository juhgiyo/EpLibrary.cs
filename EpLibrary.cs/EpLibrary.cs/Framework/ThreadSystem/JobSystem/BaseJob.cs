/*! 
@file BaseJob.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief BaseJob Interface
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

A BaseJob Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

namespace EpLibrary.cs
{
    /// <summary>
    /// Enumeration for Job Status
    /// </summary>
	public enum JobStatus{
        /// <summary>
        /// Job never entered to Job Processor
        /// </summary>
		NONE=0,
        /// <summary>
        /// Job is in the Job Processor's Queue
        /// </summary>
		IN_QUEUE,
        /// <summary>
        /// Job is in the Process
        /// </summary>
		IN_PROCESS,
        /// <summary>
        /// Job is finished by Job Processor
        /// </summary>
		DONE,
        /// <summary>
        /// Job is incomplete due to Thread Error or Job Processor Error
        /// </summary>
		INCOMPLETE,
        /// <summary>
        /// Job Processor is Timed Out
        /// </summary>
		JOB_PROCESSOR_TIMEOUT,
        /// <summary>
        /// Job is Timed out
        /// </summary>
		TIMEOUT,
        /// <summary>
        /// Job Processor and Job is in Pending State
        /// </summary>
		PENDING,
	};

    /// <summary>
    /// A base class for Job Objects.
    /// </summary>
    public class BaseJob: IComparable<BaseJob>
    {

        /// <summary>
        /// current Job Status
        /// </summary>
        private JobStatus m_status;
        /// <summary>
        /// priority of the Job
        /// </summary>
        private ThreadPriority m_priority;


        /// <summary>
        /// Return the current Job Status.
        /// </summary>
        /// <returns>the current Job Status</returns>
		public JobStatus GetStatus()
        {
            return m_status;
        }

        /// <summary>
        /// Return the priority of this job
        /// </summary>
        /// <returns>the priority of this Job</returns>
        public ThreadPriority GetPriority()
        {
            return m_priority;
        }

        /// <summary>
        /// Set the priority of this job
        /// </summary>
        /// <param name="newPrio">new priority of this job</param>
        public void SetPriority(ThreadPriority newPrio)
        {
            m_priority = newPrio;
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="priority">the priority of the job</param>
		protected BaseJob(ThreadPriority priority=ThreadPriority.Normal)
        {
            m_status=JobStatus.NONE;
            m_priority=priority;
        }

        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="b">the object to copy from</param>
		public BaseJob(BaseJob b)
		{
			m_status=b.m_status;
			m_priority=b.m_priority;
		}

        /// <summary>
        /// Handles when Job Status Changed
        /// </summary>
        /// <param name="status">The Status of the Job</param>
        /// <remarks>Subclass should overwrite this function!!</remarks>
        public virtual void handleReport(JobStatus status)
        {

        }

        /// <summary>
        /// Call Back Function When Job's Status Changed.
        /// </summary>
        /// <param name="status">The Status of the Job</param>
        public void JobReport(JobStatus status)
        {
            handleReport(status);
            m_status = status;
        }

        /// <summary>
        /// Compares Job with obj
        /// </summary>
        /// <param name="obj">another Job object.</param>
        /// <returns>the Result of Comparison</returns>
        public int CompareTo(object obj)
        {
            if (obj == null) return -1;
            BaseJob _b = obj as BaseJob;
            if (GetPriority() > _b.GetPriority())
                return -1;
            else if (GetPriority() < _b.GetPriority())
                return 1;
            return 1;
        }
        /// <summary>
        /// Compares Job with obj
        /// </summary>
        /// <param name="obj">another Job object.</param>
        /// <returns>the Result of Comparison</returns>
        public int CompareTo(BaseJob obj)
        {
            return CompareTo(obj as object);
        }

    }
}
