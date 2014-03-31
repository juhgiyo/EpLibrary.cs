using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace EpLibrary.cs
{
    public abstract class BaseLock
    {
        public BaseLock()
        {
        }
        public BaseLock(BaseLock b)
        {
        }
		~BaseLock()
        {
        }

		public abstract bool Lock();

        public abstract bool TryLock();

        public abstract bool TryLockFor(int dwMilliSecond);
        public abstract void Unlock();

		public class BaseLockObj
		{
			public BaseLockObj(BaseLock iLock)
            {
                Debug.Assert(iLock != null, "Lock is null!");
                m_lock = iLock;
                if (m_lock!=null)
                    m_lock.Lock();
            }

			~BaseLockObj()
            {
                if (m_lock != null)
                {
                    m_lock.Unlock();
                }
            }
			private BaseLockObj()
            {
                m_lock = null;
            }

			BaseLock m_lock;
		};

    }
}
