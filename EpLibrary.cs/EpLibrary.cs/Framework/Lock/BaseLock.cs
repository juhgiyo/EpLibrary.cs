using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace EpLibrary.cs
{
    /// <summary>
    /// A class that handles the virtual base lock.
    /// </summary>
    public abstract class BaseLock
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseLock()
        {
        }
        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="b">the object to copy from</param>
        public BaseLock(BaseLock b)
        {
        }
		~BaseLock()
        {
        }

        /// <summary>
        /// Locks the critical section
        /// </summary>
        /// <returns>true if locked, otherwise false</returns>
		public abstract bool Lock();

        /// <summary>
        /// Try to lock the critical section
        /// 
        /// If other thread is already in the critical section, it just returns false and continue, otherwise obtain the ciritical section
        /// </summary>
        /// <returns>true if locked, otherwise false</returns>
        public abstract bool TryLock();

        /// <summary>
        /// Try to lock the critical section for given time
        /// </summary>
        /// <param name="dwMilliSecond">the wait time</param>
        /// <returns>true if locked, otherwise false</returns>
        public abstract bool TryLockFor(int dwMilliSecond);


        /// <summary>
        /// Leave the critical section
        /// </summary>
        public abstract void Unlock();

        /// <summary>
        /// A class that handles the lock.
        /// </summary>
		public class BaseLockObj
		{
            /// <summary>
            /// Default Constructor
            /// </summary>
            /// <param name="iLock">the lock to lock.</param>
			public BaseLockObj(BaseLock iLock)
            {
                Debug.Assert(iLock != null, "Lock is null!");
                m_lock = iLock;
                if (m_lock!=null)
                    m_lock.Lock();
            }

            /// <summary>
            /// Default Destructor
            /// 
            /// Unlock when this object destroyed.
            /// </summary>
			~BaseLockObj()
            {
                if (m_lock != null)
                {
                    m_lock.Unlock();
                }
            }

            /// <summary>
            /// Default Constructor
            /// </summary>
            /// <remarks>Cannot be used</remarks>
			private BaseLockObj()
            {
                m_lock = null;
            }

            /// <summary>
            /// lock
            /// </summary>
			BaseLock m_lock;
		};

    }
}
