using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace EpLibrary.cs
{
    public class MutexEx:BaseLock
    {
        private Mutex m_mutex;
        private bool m_isInitialOwned;
        private String m_name;
        private bool m_isMutexAbandoned;
        public MutexEx(String mutexName = null)
            : base()
        {
	        m_isInitialOwned=false;
            m_name=mutexName;
            m_isMutexAbandoned = false;
            m_mutex=new Mutex(m_isInitialOwned,m_name);
        }

        public MutexEx(bool isInitialOwned, String mutexName = null)
        {
            m_isInitialOwned=isInitialOwned;
            m_name=mutexName;
            m_isMutexAbandoned = false;
            m_mutex=new Mutex(m_isInitialOwned,m_name);
        }

		public MutexEx(MutexEx b):base(b)
        {
            m_isInitialOwned=b.m_isInitialOwned;
            m_name=b.m_name;
            m_isMutexAbandoned = false;
            m_mutex=new Mutex(m_isInitialOwned,m_name);
        }

        ~MutexEx()
        {
        }

        public override bool Lock()
        {
            try
            {
                return m_mutex.WaitOne();
            }
            catch (AbandonedMutexException)
            {
                m_isMutexAbandoned = true;
                return false;
            }
        }
        public override bool TryLock()
        {
            try
            {
                return m_mutex.WaitOne(0);
            }
            catch (AbandonedMutexException)
            {
                m_isMutexAbandoned = true;
                return false;
            }
            
        }


        public override bool TryLockFor(int dwMilliSecond)
        {
            try
            {
                return m_mutex.WaitOne(dwMilliSecond);
            }
            catch (AbandonedMutexException)
            {
                m_isMutexAbandoned = true;
                return false;
            }
            
        }

		
		public override void Unlock()
        {
            m_mutex.ReleaseMutex();
        }

        public bool IsMutexAbandoned()
        {
            return m_isMutexAbandoned;
        }

        
    }
}
