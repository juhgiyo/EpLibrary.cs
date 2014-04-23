/*! 
@file WorkerThreadFactory.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief WorkerThreadFactory Interface
@version 2.0

@section LICENSE

The MIT License (MIT)

Copyright (c) 2014 Woong Gyu La <juhgiyo@gmail.com>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

@section DESCRIPTION

A WorkerThreadFactory Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
