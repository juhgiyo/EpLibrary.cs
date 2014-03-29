using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpLibrary.cs
{
    /// Enumeration for Job Processor Status
    public enum JobProcessorStatus
    {
        /// Job Processor never entered to the scheduler
        NONE = 0,
        /// Job Processor is in the Pending State
        PENDING,
        /// Job Processor is in the Process
        IN_PROCESS,
        /// Job Processor is finished working
        DONE,
        /// Job Processor is suspended by other processor
        SUSPENDED,
        /// Job Processor terminated due to Thread Error or Internal Error
        INCOMPLETE,
        /// Job PRocessor TImed Out
        TIMEOUT,
    };

    public abstract class BaseJobProcessor
    {
		~BaseJobProcessor()
        {

        }


        public abstract void DoJob(BaseWorkerThread workerThread, BaseJob data);

		protected virtual void handleReport(JobProcessorStatus status)
        {

        }

        protected BaseJobProcessor()
        {
            m_status = JobProcessorStatus.NONE;
        }

		protected BaseJobProcessor(BaseJobProcessor b)
		{
			m_status=b.m_status;
		}


        private void JobProcessorReport(JobProcessorStatus status)
        {
            handleReport(status);
            m_status = status;
        }


		/// current Job Processor Status
		private JobProcessorStatus m_status;
    }
}
