using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace EpLibrary.cs
{
    /// Enumerator for Thread Operation Code
    public enum ThreadOpCode
    {
        /// The thread is started when it is created.
        CREATE_START = 0,
        /// The thread is suspended when it is created.
        CREATE_SUSPEND
    };

    /// Enumerator for Thread Status
    public enum ThreadStatus
    {
        /// The thread is started and running.
        STARTED = 0,
        /// The thread is suspended.
        SUSPENDED,
        /// The thread is terminated.
        TERMINATED
    };

    /// Enumerator for Thread Terminate Result
    public enum TerminateResult
    {
        /// Failed to terminate the thread
        FAILED = 0,
        /// The thread terminated gracefully
        GRACEFULLY_TERMINATED,
        /// The thread terminated forcefully
        FORCEFULLY_TERMINATE,
        /// The thread was not running
        NOT_ON_RUNNING,
    };
    public class ThreadEx
    {
        private Thread m_threadHandle;
        		
		/// ThreadPriority
		private ThreadPriority m_threadPriority;
		
		/// Parent Thread Handle
		private Thread m_parentThreadHandle;
		
		/// Thread Status
		private ThreadStatus m_status;

        /// thread Func
        private Action m_threadFunc;

		/// Lock
        private Object m_threadLock = new Object();
		/// exit code
		private ulong m_exitCode;

        private static Action dummyThreadFunc=null;


 		public ThreadEx(ThreadPriority priority=ThreadPriority.Normal)
        {
            m_threadHandle=null;
            m_threadPriority=priority;
            m_parentThreadHandle=null;
            m_status=ThreadStatus.TERMINATED;
            m_exitCode=0;
            m_threadFunc = dummyThreadFunc;
        }

		public ThreadEx(Action threadFunc, ThreadPriority priority=ThreadPriority.Normal)
        {
            m_threadHandle = null;
            m_threadPriority = priority;
            m_parentThreadHandle = null;
            m_status = ThreadStatus.TERMINATED;
            m_exitCode = 0;
            m_threadFunc = threadFunc;


	        m_parentThreadHandle=Thread.CurrentThread;
	        m_threadHandle=new Thread(ThreadEx.entryPoint);
            m_threadHandle.Priority=m_threadPriority;
            m_threadHandle.Start(this);
            m_status=ThreadStatus.STARTED; 
                  

        }

        public ThreadEx(ThreadEx b)
        {
            m_threadFunc=b.m_threadFunc;
	        if(m_threadFunc!=dummyThreadFunc)
	        {
		        m_parentThreadHandle=b.m_parentThreadHandle;
		        m_threadHandle=b.m_threadHandle;
		        m_threadPriority=b.m_threadPriority;
		        m_status=b.m_status;
		        m_exitCode=b.m_exitCode;

		        b.m_parentThreadHandle=null;
		        b.m_threadHandle=null;
		        b.m_status=ThreadStatus.TERMINATED;
		        b.m_exitCode=0;
            }
	        else
	        {
		        m_threadHandle=null;
		        m_threadPriority=b.m_threadPriority;
		        m_parentThreadHandle=null;
		        m_exitCode=0;

                m_status = ThreadStatus.TERMINATED;
	        }
        }

        ~ThreadEx()
        {
            resetThread();
        }
		
		public bool Start(ThreadOpCode opCode=ThreadOpCode.CREATE_START, int stackSize=0)
        {
            lock(m_threadLock)
            {
                m_parentThreadHandle=Thread.CurrentThread;
                if(m_status==ThreadStatus.TERMINATED&& m_threadHandle==null)
                {
                    m_threadHandle=new Thread(ThreadEx.entryPoint,stackSize);
                    if (m_threadHandle != null)
                    {
                        m_threadHandle.Priority = m_threadPriority;
                        if (opCode == ThreadOpCode.CREATE_START)
                        {
                            m_threadHandle.Start(this);
                            m_status = ThreadStatus.STARTED;
                        }
                        else
                            m_status = ThreadStatus.SUSPENDED;
                        return true;
                    }

                }
                //	System::OutputDebugString(_T("The thread (%x): Thread already exists!\r\n"),m_threadId);
	                return false;
            }
        }


        public bool Resume()
        {
            lock (m_threadLock)
            {
                if (m_status == ThreadStatus.SUSPENDED && m_threadHandle != null)
                {
                    m_threadHandle.Resume();
                    m_status = ThreadStatus.STARTED;
                    return true;
                }
            }
            //	System::OutputDebugString(_T("The thread (%x): Thread must be in suspended state in order to resume!\r\n"),m_threadId);
            return false;
        }

        public bool Suspend()
        {

            if(m_status==ThreadStatus.STARTED && m_threadHandle!=null)
            {
                lock(m_threadLock)
                {
                    m_status=ThreadStatus.SUSPENDED;
                }
                m_threadHandle.Suspend();
                return true;
            }
            //	System::OutputDebugString(_T("The thread (%x): Thread must be in running state in order to suspend!\r\n"),m_threadId);
            return false;
            
        }

        public bool Terminate()
        {
            Debug.Assert(m_threadHandle != Thread.CurrentThread, "Exception : Thread should not terminate self.");

            if (m_status != ThreadStatus.TERMINATED && m_threadHandle != null)
            {
                lock (m_threadLock)
                {
                    m_status = ThreadStatus.TERMINATED;
                    m_exitCode = 1;
                    m_threadHandle.Abort();
                    m_threadHandle = null;
                    m_parentThreadHandle = null;
                }
                ulong exitCode = m_exitCode;
                onTerminated(exitCode);
                return true;
            }
            return true;
        }


        public bool WaitFor(int tMilliseconds = Timeout.Infinite)
        {
            if(m_status!=ThreadStatus.TERMINATED && m_threadHandle!=null)
	        {
		        return m_threadHandle.Join(tMilliseconds);
	        }
	        else
	        {
                //	System::OutputDebugString(_T("The thread (%x): Thread is not started!\r\n"),m_threadId);
		        return false;
	        }
        }

        public void Join()
        {
            if (m_status != ThreadStatus.TERMINATED && m_threadHandle != null)
            {
                m_threadHandle.Join();
            }
        }


        public bool Joinable()
        {
            return (m_status != ThreadStatus.TERMINATED && m_threadHandle != null);
        }


        public void Detach()
        {
            Debug.Assert(Joinable() == true);
            lock (m_threadLock)
            {
                m_status = ThreadStatus.TERMINATED;
                m_threadHandle = null;
                m_parentThreadHandle = null;
                m_exitCode = 0;
            }
        }

        public TerminateResult TerminateAfter(int tMilliseconds)
        {
           	if(m_status!=ThreadStatus.TERMINATED && m_threadHandle!=null)
	        {
		        bool status=m_threadHandle.Join(tMilliseconds);
                if(status)
                {
                    return TerminateResult.GRACEFULLY_TERMINATED;
                }
                else
                {
                    if (Terminate())
                        return TerminateResult.FORCEFULLY_TERMINATE;
                    return TerminateResult.FAILED;
                }
	        }
	        else
	        {
		        //System::OutputDebugString(_T("The thread (%x): Thread is not started!\r\n"),m_threadId);
		        return TerminateResult.NOT_ON_RUNNING;
	        }
        }


		/*!
		Return the parent's Thread Handle.
		@return the parent's Thread Handle.
		*/
		public Thread GetParentThreadHandle()
		{
			return m_parentThreadHandle;
		}

		/*!
		Return the Thread Status.
		@return the current thread status.
		*/
		public ThreadStatus GetStatus()
		{
			return m_status;
		}

		/*!
		Return the Thread Exit Code.
		@return the thread exit code.
		@remark 0 means successful termination, 1 means unsafe termination.
		*/
		public ulong GetExitCode()
		{
			return m_exitCode;
		}


        public ThreadPriority GetPriority()
        {
            return m_threadPriority;
        }

        public bool SetPriority(ThreadPriority priority)
        {
            m_threadPriority = priority;
            m_threadHandle.Priority = priority;
            return true;
        }

		protected Thread getHandle()
		{
			return m_threadHandle;
		}



        protected virtual void execute()
        {
            m_threadFunc();
        }

        protected virtual void onTerminated(ulong exitCode, bool isInDeletion = false)
        {
        }


        private void successTerminate()
        {
            lock (m_threadLock)
            {
                m_status = ThreadStatus.TERMINATED;
                m_threadHandle = null;
                m_parentThreadHandle = null;
                m_exitCode = 0;
            }
            
            onTerminated(m_exitCode);
        }

        private int run()
        {
            execute();
            successTerminate();
            return 0;
        }
        private void resetThread()
        {
            if(m_status!=ThreadStatus.TERMINATED)
	        {
		        m_exitCode=1;
		        m_threadHandle.Abort();
		        onTerminated(m_exitCode,true);
	        }

	        m_threadHandle=null;
	        m_parentThreadHandle=null;
	        m_exitCode=0;
            m_status = ThreadStatus.TERMINATED;
        }
        private static void entryPoint(object pThis)
        {
            ThreadEx pt = (ThreadEx)pThis;
            pt.run();
        }


    }
}
