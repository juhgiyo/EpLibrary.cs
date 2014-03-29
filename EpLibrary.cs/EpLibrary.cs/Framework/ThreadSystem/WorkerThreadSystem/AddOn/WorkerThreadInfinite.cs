using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace EpLibrary.cs
{
    public class WorkerThreadInfinite:BaseWorkerThread
    {
        public WorkerThreadInfinite(ThreadLifePolicy policy):base(policy)
        {
            m_terminateEvent=new EventEx(false,EventResetMode.AutoReset);
        }
		public WorkerThreadInfinite(WorkerThreadInfinite  b):base(b)
        {
            m_terminateEvent=new EventEx(false,EventResetMode.AutoReset);
        }

        ~WorkerThreadInfinite(){}

        public new TerminateResult TerminateWorker(int waitTimeInMilliSec= Timeout.Infinite)
        {
            m_terminateEvent.SetEvent();
	        Resume();
	        return TerminateAfter(waitTimeInMilliSec);
        }


        protected override void execute()
        {
            try
            {
                while (true)
                {
                    if (m_terminateEvent.WaitForEvent(0))
                    {
                        return;
                    }
                    if (m_workPool.IsEmpty())
                    {
                        if (m_lifePolicy == ThreadLifePolicy.SUSPEND_AFTER_WORK)
                        {
                            callCallBack();
                            Suspend();
                            continue;
                        }
                        callCallBack();
                        Thread.Sleep(0);
                        continue;
                    }
                    Debug.Assert(m_jobProcessor != null,"Job Processor is NULL!");
                    if (m_jobProcessor == null)
                        break;
                    BaseJob jobPtr = m_workPool.Front();
                    m_workPool.Pop();
                    jobPtr.JobReport(JobStatus.IN_PROCESS);
                    m_jobProcessor.DoJob(this, jobPtr);
                    jobPtr.JobReport(JobStatus.DONE);

                }
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

		/// Terminate Signal Event
		private EventEx m_terminateEvent;
    }
}
