using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace EpLibrary.cs
{
    public enum ThreadLifePolicy
    {
        /// The thread is infinitely looping.
        INFINITE = 0,
        /// The thread suspends if work pool is empty.
        SUSPEND_AFTER_WORK,
        /// The thread terminates if work pool is empty.
        TERMINATE_AFTER_WORK,
    };

    public abstract class BaseWorkerThread:ThreadEx
    {
        public BaseWorkerThread(ThreadLifePolicy policy):base(ThreadPriority.Normal)
        {
            m_lifePolicy=policy;
            m_callBackClass=null;

        }
		
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

		
		public void Push(BaseJob  work)
        {
            m_workPool.Push(work);
            if(m_lifePolicy==ThreadLifePolicy.SUSPEND_AFTER_WORK)
                Resume();
        }

		
		public void Pop()
        {
            m_workPool.Pop();
        }

		public BaseJob Front()
        {
            return m_workPool.Front();
        }

		public bool Erase(BaseJob work)
        {
            return m_workPool.Erase(work);
        }

		public ThreadLifePolicy GetLifePolicy()
		{
			return m_lifePolicy;
		}

		
		public virtual void SetCallBackClass(WorkerThreadDelegate callBackClass)
        {
            lock(m_callBackLock)
            {
                m_callBackClass=callBackClass;
            }
        }

        public int GetJobCount()
        {
            return m_workPool.Size();
        }

        public void SetJobProcessor(BaseJobProcessor jobProcessor)
        {
            m_jobProcessor = jobProcessor;
        }

        public BaseJobProcessor GetJobProcessor()
        {
            return m_jobProcessor;
        }

        public virtual TerminateResult TerminateWorker(int waitTimeInMilliSec = Timeout.Infinite)
        {
            return TerminateAfter(waitTimeInMilliSec);
        }

	

		protected new abstract void execute();

        protected void callCallBack()
        {
            lock (m_callBackLock)
            {
                if (m_callBackClass != null)
                    m_callBackClass.CallBackFunc(this);
                m_callBackClass = null;
            }
        }

		/// the work list
        protected JobScheduleQueue m_workPool;
		/// the life policy of the thread
        protected ThreadLifePolicy m_lifePolicy;
		/// the call back class
        protected WorkerThreadDelegate m_callBackClass;

		/// callback Lock
        protected Object m_callBackLock = new Object();
		/// Job Processor
        protected BaseJobProcessor m_jobProcessor;
    }
}
