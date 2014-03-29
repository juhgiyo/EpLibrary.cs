using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpLibrary.cs
{
    public class JobScheduleQueue:ThreadSafePQueue<BaseJob>
    {
       	public JobScheduleQueue()
        {
        }
		
		public JobScheduleQueue(JobScheduleQueue b):base(b)
		{
		}

        ~JobScheduleQueue() { }

        public override void Push(BaseJob data)
        {
            Push(data, JobStatus.IN_QUEUE);
        }
		public void Push(BaseJob data,JobStatus status)
        {
            base.Push(data);
            if (status != JobStatus.NONE)
                data.JobReport(status);
        }

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
