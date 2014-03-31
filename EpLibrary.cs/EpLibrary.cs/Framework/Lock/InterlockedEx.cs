using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace EpLibrary.cs
{
    public sealed class InterlockedEx:BaseLock
    {
        /// <summary>
        /// lock
        /// </summary>
        private int m_interLock;
        /// <summary>
        /// Default constructor
        /// </summary>
        public InterlockedEx():base()
        {
            m_interLock = 0;
        }

        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="b">the object to copy from</param>
        public InterlockedEx(NoLock b)
            : base(b)
        {
            m_interLock = 0;
        }

        ~InterlockedEx()
        {
        }

        /// <summary>
        /// Locks the critical section
        /// </summary>
        /// <returns>true if locked, otherwise false</returns>
        public override bool Lock()
        {
            while (Interlocked.Exchange(ref m_interLock, 1) != 0)
            {
                Thread.Sleep(0);
            }
            return true;
        }

        /// <summary>
        /// Try to lock the critical section
        /// 
        /// If other thread is already in the critical section, it just returns false and continue, otherwise obtain the ciritical section
        /// </summary>
        /// <returns>true if locked, otherwise false</returns>
        public override bool TryLock()
        {
            bool ret = false;
            if (Interlocked.Exchange(ref m_interLock, 1) == 0)
            {
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// Try to lock the critical section for given time
        /// </summary>
        /// <param name="dwMilliSecond">the wait time</param>
        /// <returns>true if locked, otherwise false</returns>
        public override bool TryLockFor(int dwMilliSecond)
        {
            bool ret=false;
	        
	        DateTime startTime;
	        double timeUsed;
	        double waitTime=(double)dwMilliSecond;
	        startTime=DateTimeHelper.GetCurrentDateTime();
	
	        do
	        {
		        if(Interlocked.Exchange(ref m_interLock, 1) != 0)
		        {
			        Thread.Sleep(0);
			        timeUsed=DateTimeHelper.AbsDiffInMilliSec(DateTimeHelper.GetCurrentDateTime(),startTime);
			        waitTime=waitTime-timeUsed;
			        startTime=DateTimeHelper.GetCurrentDateTime();
		        }
		        else
		        {
			        ret=true;
		        }
	        }while(waitTime>0.0);
            return ret;
        }

        /// <summary>
        /// Leave the critical section
        /// </summary>
        public override void Unlock()
        {
            Interlocked.Exchange(ref m_interLock, 0);
        }
    }
}
