using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace EpLibrary.cs
{
   	/// Enumeration for Job Status
	public enum JobStatus{
		/// Job never entered to Job Processor
		NONE=0,
		/// Job is in the Job Processor's Queue
		IN_QUEUE,
		/// Job is in the Process
		IN_PROCESS,
		/// Job is finished by Job Processor
		DONE,
		/// Job is incomplete due to Thread Error or Job Processor Error
		INCOMPLETE,
		/// Job Processor is Timed Out
		JOB_PROCESSOR_TIMEOUT,
		/// Job is Timed out
		TIMEOUT,
		/// Job Processor and Job is in Pending State
		PENDING,
	};

    public class BaseJob: IComparable<BaseJob>
    {


		
		~BaseJob()
        {

        }

		public JobStatus GetStatus()
        {
            return m_status;
        }

        public ThreadPriority GetPriority()
        {
            return m_priority;
        }

        public void SetPriority(ThreadPriority newPrio)
        {
            m_priority = newPrio;
        }

		protected BaseJob(ThreadPriority priority=ThreadPriority.Normal)
        {
            m_status=JobStatus.NONE;
            m_priority=priority;
        }

		public BaseJob(BaseJob b)
		{
			m_status=b.m_status;
			m_priority=b.m_priority;
		}

        public virtual void handleReport(JobStatus status)
        {

        }

        public void JobReport(JobStatus status)
        {
            handleReport(status);
            m_status = status;
        }

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
        public int CompareTo(BaseJob obj)
        {
            return CompareTo(obj as object);
        }


		private JobStatus m_status;

		private ThreadPriority m_priority;
    }
}
