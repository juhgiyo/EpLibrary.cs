/*! 
@file JobScheduleQueue.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief JobScheduleQueue Interface
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

A JobScheduleQueue Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace EpLibrary.cs
{
    /// <summary>
    /// A class for Thread Safe Priority Queue.
    /// </summary>
    public sealed class JobScheduleQueue:ThreadSafePQueue<BaseJob>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
       	public JobScheduleQueue()
        {
        }
        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="b">the object to copy from</param>
		public JobScheduleQueue(JobScheduleQueue b):base(b)
		{
		}


        /// <summary>
        /// Insert the new item into the schedule queue.
        /// </summary>
        /// <param name="data">data The inserting data</param>
        /// <param name="status">the status to set for the data</param>
		public void Enqueue(BaseJob data,JobStatus status=JobStatus.IN_QUEUE)
        {
            base.Enqueue(data);
            if (status != JobStatus.NONE)
                data.JobReport(status);
        }

        /// <summary>
        /// Erase the element with given schedule policy holder
        /// </summary>
        /// <param name="data">the schedule policy holder to erase</param>
        /// <returns>true if successful, otherwise false.</returns>
		public new bool Erase(BaseJob data)
        {
            lock(m_queueLock)
            {
	            if(m_queue.Count==0)
		            return false;
	            for(int idx=m_queue.Count-1;idx>=0;idx--)
                {
                    if(m_queue[idx].Equals(data))
                    {
                        data.JobReport(JobStatus.TIMEOUT);
                        m_queue.RemoveAt(idx);
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Make Report to the all element in the queue as given status
        /// </summary>
        /// <param name="status">the status to give to all element in the queue</param>
        public void ReportAllJob(JobStatus status)
        {
            List<BaseJob> queue=null;
            lock(m_queueLock)
            {
                queue=new List<BaseJob>(m_queue);
            }
            foreach(BaseJob job in queue)
            {
                job.JobReport(JobStatus.INCOMPLETE);
            }
        }
    }
}
