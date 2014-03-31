using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace EpLibrary.cs
{
    /// <summary>
    /// A class that implements infinite-looping Worker Thread Class.
    /// </summary>
    public sealed class WorkerThreadInfinite:BaseWorkerThread
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="policy">the life policy of this worker thread.</param>
        public WorkerThreadInfinite(ThreadLifePolicy policy):base(policy)
        {
            m_terminateEvent=new EventEx(false,EventResetMode.AutoReset);
        }

        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="b">the object to copy from</param>
		public WorkerThreadInfinite(WorkerThreadInfinite  b):base(b)
        {
            m_terminateEvent=new EventEx(false,EventResetMode.AutoReset);
        }

        ~WorkerThreadInfinite(){}

        /// <summary>
        /// Wait for worker thread to terminate, and if not terminated, then Terminate.
        /// </summary>
        /// <param name="waitTimeInMilliSec">the time-out interval, in milliseconds.</param>
        /// <returns>the terminate result of the thread</returns>
        public new TerminateResult TerminateWorker(int waitTimeInMilliSec= Timeout.Infinite)
        {
            m_terminateEvent.SetEvent();
	        Resume();
	        return TerminateAfter(waitTimeInMilliSec);
        }

        /// <summary>
        /// Actual infinite-looping Thread Code.
        /// </summary>
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

        /// <summary>
        /// Terminate Signal Event
        /// </summary>
		private EventEx m_terminateEvent;
    }
}
