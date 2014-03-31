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
		~BaseJobProcessor()
        {

        }

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
