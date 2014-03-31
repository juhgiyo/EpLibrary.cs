using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace EpLibrary.cs
{
    /// <summary>
    /// A factory class that returns the Worker Thread objects.
    /// </summary>
    public class WorkerThreadFactory
    {
        /// <summary>
        /// Return the new worker thread object with given life policy.
        /// </summary>
        /// <param name="policy">the life policy of the thread to create.</param>
        /// <returns>the new worker thread object with given life policy.</returns>
        public static BaseWorkerThread GetWorkerThread(ThreadLifePolicy policy = ThreadLifePolicy.INFINITE)
        {
           	if(policy==ThreadLifePolicy.INFINITE)
		        return new WorkerThreadInfinite(policy) as BaseWorkerThread;
	        else if(policy== ThreadLifePolicy.TERMINATE_AFTER_WORK)
		        return new WorkerThreadSingle(policy) as BaseWorkerThread;
	        else if(policy== ThreadLifePolicy.SUSPEND_AFTER_WORK)
		        return new WorkerThreadInfinite(policy) as BaseWorkerThread;
	        else
	        {
                String outputString="Unknown Thread Life Policy Input! Thread Life Policy Input : ";
                outputString+=policy.ToString();
		        Debug.Assert(false,outputString as string);
	        }
	        return null;
        }
    }
}
