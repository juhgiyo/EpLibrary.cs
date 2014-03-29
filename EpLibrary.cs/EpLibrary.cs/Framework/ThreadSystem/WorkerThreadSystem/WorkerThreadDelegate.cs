using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpLibrary.cs
{
    public interface WorkerThreadDelegate
    {
        void CallBackFunc(BaseWorkerThread p);
    }
}
