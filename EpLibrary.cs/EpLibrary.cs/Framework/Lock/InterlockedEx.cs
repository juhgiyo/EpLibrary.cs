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
        private int m_interLock;
        public InterlockedEx():base()
        {
            m_interLock = 0;
        }

        public InterlockedEx(NoLock b)
            : base(b)
        {
            m_interLock = 0;
        }

        ~InterlockedEx()
        {
        }

        public override bool Lock()
        {
            while (Interlocked.Exchange(ref m_interLock, 1) != 0)
            {
                Thread.Sleep(0);
            }
            return true;
        }

        public override bool TryLock()
        {
            bool ret = false;
            if (Interlocked.Exchange(ref m_interLock, 1) == 0)
            {
                ret = true;
            }
            return ret;
        }

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

        public override void Unlock()
        {
            Interlocked.Exchange(ref m_interLock, 0);
        }
    }
}
