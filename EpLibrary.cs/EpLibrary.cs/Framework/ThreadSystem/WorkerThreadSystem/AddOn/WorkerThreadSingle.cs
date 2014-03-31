using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace EpLibrary.cs
{
    public sealed class WorkerThreadSingle:BaseWorkerThread
    {
        public WorkerThreadSingle(ThreadLifePolicy policy):base(policy)
		{}

        public WorkerThreadSingle(WorkerThreadSingle b)
            : base(b)
		{}
		
		~WorkerThreadSingle(){}

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
