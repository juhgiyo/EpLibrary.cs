using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpLibrary.cs
{
    public sealed class NoLock:BaseLock
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public NoLock():base()
        {
        }

        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="b">the object to copy from</param>
        public NoLock(NoLock b)
            : base(b)
        {
        }

        ~NoLock()
        {
        }

        /// <summary>
        /// Locks the critical section
        /// </summary>
        /// <returns>true if locked, otherwise false</returns>
        public override bool Lock()
        {
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
            return true;
        }

        /// <summary>
        /// Try to lock the critical section for given time
        /// </summary>
        /// <param name="dwMilliSecond">the wait time</param>
        /// <returns>true if locked, otherwise false</returns>
        public override bool TryLockFor(int dwMilliSecond)
        {
            return true;
        }

        /// <summary>
        /// Leave the critical section
        /// </summary>
        public override void Unlock()
        {
        }
    }

}
