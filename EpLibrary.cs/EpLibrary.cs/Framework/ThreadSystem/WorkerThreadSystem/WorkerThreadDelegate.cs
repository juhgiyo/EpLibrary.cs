using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpLibrary.cs
{
    /// <summary>
    /// A interface for Worker Thread Delegate.
    /// </summary>
    public interface WorkerThreadDelegate
    {
        /// <summary>
        /// Call Back Function.
        /// </summary>
        /// <param name="p">the argument for call back function.</param>
        void CallBackFunc(BaseWorkerThread p);
    }
}
