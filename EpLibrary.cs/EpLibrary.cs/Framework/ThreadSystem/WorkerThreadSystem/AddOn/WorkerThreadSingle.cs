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
		
		~WorkerThreadSingle(){}

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
                    BaseJob jobPtr = m_workPool.Front();
                    m_workPool.Pop();
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
                    BaseJob jobPtr = m_workPool.Front();
                    m_workPool.Pop();
                    jobPtr.JobReport(JobStatus.INCOMPLETE);
                }
            }

        }
    }
}
