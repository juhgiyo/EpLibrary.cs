using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace EpLibrary.cs
{
    /// <summary>
    /// Enumerator for Thread Life Policy
    /// </summary>
    public enum ThreadLifePolicy
    {
        /// <summary>
        /// The thread is infinitely looping.
        /// </summary>
        INFINITE = 0,
        /// <summary>
        /// The thread suspends if work pool is empty.
        /// </summary>
        SUSPEND_AFTER_WORK,
        /// <summary>
        /// The thread terminates if work pool is empty.
        /// </summary>
        TERMINATE_AFTER_WORK,
    };

    /// <summary>
    /// A class that implements Base Worker Thread Class.
    /// </summary>
    public abstract class BaseWorkerThread:ThreadEx
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="policy">the life policy of this worker thread.</param>
        public BaseWorkerThread(ThreadLifePolicy policy):base(ThreadPriority.Normal)
        {
            m_lifePolicy=policy;
            m_callBackClass=null;

        }

        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="b">the object to copy from</param>
		public BaseWorkerThread(BaseWorkerThread b):base(b)
        {
            lock(b.m_callBackLock)
            {
                m_lifePolicy=b.m_lifePolicy;
	            m_callBackClass=b.m_callBackClass;
	            m_jobProcessor=b.m_jobProcessor;
	            m_workPool=b.m_workPool;
            }
            
        }

        ~BaseWorkerThread()
        {
            while(!m_workPool.IsEmpty())
	        {
		        m_workPool.Front().JobReport(JobStatus.INCOMPLETE);
		        m_workPool.Pop();
	        }
        }

		/// <summary>
        /// Push in the new work to the work pool.
		/// </summary>
        /// <param name="work">the new work to put into the work pool.</param>
		public void Push(BaseJob  work)
        {
            m_workPool.Push(work);
            if(m_lifePolicy==ThreadLifePolicy.SUSPEND_AFTER_WORK)
                Resume();
        }

		/// <summary>
        /// Pop a work from the work pool.
		/// </summary>
		public void Pop()
        {
            m_workPool.Pop();
        }

        /// <summary>
        /// Get First Job in the Job Queue.
        /// </summary>
        /// <returns>first job</returns>
		public BaseJob Front()
        {
            return m_workPool.Front();
        }
        /// <summary>
        /// Erase the given work from the work pool.
        /// </summary>
        /// <param name="work">the work to erase from the work pool</param>
        /// <returns>true if successful, otherwise false.</returns>
		public bool Erase(BaseJob work)
        {
            return m_workPool.Erase(work);
        }

        /// <summary>
        /// Return the life policy of this worker thread.
        /// </summary>
        /// <returns>the life policy of this worker thread.</returns>
		public ThreadLifePolicy GetLifePolicy()
		{
			return m_lifePolicy;
		}

		/// <summary>
        /// Set call back class to call when work is done.
		/// </summary>
        /// <param name="callBackClass">the call back class.</param>
		public virtual void SetCallBackClass(WorkerThreadDelegate callBackClass)
        {
            lock(m_callBackLock)
            {
                m_callBackClass=callBackClass;
            }
        }

        /// <summary>
        /// Get job count in work pool.
        /// </summary>
        /// <returns>the job count in work pool.</returns>
        public int GetJobCount()
        {
            return m_workPool.Count;
        }

        /// <summary>
        /// Set new Job Processor.
        /// </summary>
        /// <param name="jobProcessor">set new Job Processor for this thread.</param>
        public void SetJobProcessor(BaseJobProcessor jobProcessor)
        {
            m_jobProcessor = jobProcessor;
        }

        /// <summary>
        /// Get Job Processor.
        /// </summary>
        /// <returns>the Job Processor for this thread.</returns>
        public BaseJobProcessor GetJobProcessor()
        {
            return m_jobProcessor;
        }

        /// <summary>
        /// Wait for worker thread to terminate, and if not terminated, then Terminate.
        /// </summary>
        /// <param name="waitTimeInMilliSec">the time-out interval, in milliseconds.</param>
        /// <returns>the terminate result of the thread</returns>
        public virtual TerminateResult TerminateWorker(int waitTimeInMilliSec = Timeout.Infinite)
        {
            return TerminateAfter(waitTimeInMilliSec);
        }

	
        /// <summary>
        /// Pure Worker Thread Code.
        /// </summary>
		protected new abstract void execute();

        /// <summary>
        /// Call the Call Back Class if callback class is assigned.
        /// </summary>
        protected void callCallBack()
        {
            lock (m_callBackLock)
            {
                if (m_callBackClass != null)
                    m_callBackClass.CallBackFunc(this);
                m_callBackClass = null;
            }
        }

        /// <summary>
        /// the work list
        /// </summary>
        protected JobScheduleQueue m_workPool;
        /// <summary>
        /// the life policy of the thread
        /// </summary>
        protected ThreadLifePolicy m_lifePolicy;
        /// <summary>
        /// the call back class
        /// </summary>
        protected WorkerThreadDelegate m_callBackClass;

        /// <summary>
        /// callback Lock
        /// </summary>
        protected Object m_callBackLock = new Object();
        /// <summary>
        /// Job Processor
        /// </summary>
        protected BaseJobProcessor m_jobProcessor;
    }
}
