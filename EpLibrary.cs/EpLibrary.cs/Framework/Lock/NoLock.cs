/*! 
@file NoLock.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief NoLock Interface
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

A NoLock Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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
