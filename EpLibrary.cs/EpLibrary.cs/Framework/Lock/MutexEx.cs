using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace EpLibrary.cs
{
    /// <summary>
    /// A class that handles the mutex functionality
    /// </summary>
    public class MutexEx:BaseLock
    {
        /// <summary>
        /// lock
        /// </summary>
        private Mutex m_mutex;
        /// <summary>
        /// the flag whether the mutex is owned on creation
        /// </summary>
        private bool m_isInitialOwned;
        /// <summary>
        /// name of the mutex
        /// </summary>
        private String m_name;
        /// <summary>
        /// the flag whether the mutex is abandoned
        /// </summary>
        private bool m_isMutexAbandoned;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="mutexName">name of the mutex</param>
        public MutexEx(String mutexName = null)
            : base()
        {
	        m_isInitialOwned=false;
            m_name=mutexName;
            m_isMutexAbandoned = false;
            m_mutex=new Mutex(m_isInitialOwned,m_name);
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="isInitialOwned">flag to own the mutex on creation</param>
        /// <param name="mutexName">name of the mutex</param>
        public MutexEx(bool isInitialOwned, String mutexName = null)
        {
            m_isInitialOwned=isInitialOwned;
            m_name=mutexName;
            m_isMutexAbandoned = false;
            m_mutex=new Mutex(m_isInitialOwned,m_name);
        }

        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="b">the object to copy from</param>
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

        /// <summary>
        /// Locks the critical section
        /// </summary>
        /// <returns>true if locked, otherwise false</returns>
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

        /// <summary>
        /// Try to lock the critical section
        /// 
        /// If other thread is already in the critical section, it just returns false and continue, otherwise obtain the ciritical section
        /// </summary>
        /// <returns>true if locked, otherwise false</returns>
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

        /// <summary>
        /// Try to lock the critical section for given time
        /// </summary>
        /// <param name="dwMilliSecond">the wait time</param>
        /// <returns>true if locked, otherwise false</returns>
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

        /// <summary>
        /// Leave the critical section
        /// </summary>
		public override void Unlock()
        {
            m_mutex.ReleaseMutex();
        }

        /// <summary>
        /// Returns the flag whether this mutex is abandoned or not.
        /// </summary>
        /// <returns>true if the mutex is abandoned, otherwise false</returns>
        public bool IsMutexAbandoned()
        {
            return m_isMutexAbandoned;
        }

        
    }
}
