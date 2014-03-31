using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpLibrary.cs
{
    public sealed class NoLock:BaseLock
    {
        public NoLock():base()
        {
        }

        public NoLock(NoLock b)
            : base(b)
        {
        }

        ~NoLock()
        {
        }

        public override bool Lock()
        {
            return true;
        }

        public override bool TryLock()
        {
            return true;
        }

        public override bool TryLockFor(int dwMilliSecond)
        {
            return true;
        }

        public override void Unlock()
        {
        }
    }

}
