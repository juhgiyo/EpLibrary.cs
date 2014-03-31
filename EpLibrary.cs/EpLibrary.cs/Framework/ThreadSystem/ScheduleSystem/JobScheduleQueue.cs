using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        ~JobScheduleQueue() { }

        /// <summary>
        /// Insert the new item into the schedule queue.
        /// </summary>
        /// <param name="data">data The inserting data</param>
        /// <param name="status">the status to set for the data</param>
		public void Push(BaseJob data,JobStatus status=JobStatus.IN_QUEUE)
        {
            base.Push(data);
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
