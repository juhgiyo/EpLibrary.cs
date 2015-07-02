/*! 
@file ThreadSafeErasableQueue.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief ThreadSafeErasableQueue Interface
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

A ThreadSafeErasableQueue Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace EpLibrary.cs
{
    /// <summary>
    /// A class for Thread Safe Erasable Queue.
    /// </summary>
    /// <typeparam name="T">the element type</typeparam>
    public class ThreadSafeErasableQueue<T> :ErasableQueue<T>, IQueue<T> where T : IComparable<T>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ThreadSafeErasableQueue():base()
        {

        }

        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="b">the object to copy from</param>
        public ThreadSafeErasableQueue(ThreadSafeErasableQueue<T> b):base(b)
        {
        }


        /// <summary>
         /// Check if the queue is empty.
        /// </summary>
         /// <returns>true if the queue is empty, otherwise false.</returns>
        public override bool IsEmpty()
        {
            lock(m_queueLock)
            {
                return base.IsEmpty();
            }
        }

        /// <summary>
        /// Check if the given obj exists in the queue.
        /// </summary>
        /// <param name="data">obj to check</param>
        /// <returns>true if exists, otherwise false.</returns>
        public override bool Contains(T data)
        {
            lock(m_queueLock)
            {
                return base.Contains(data);
            }
        }

       /// <summary>
        /// Return the size of the queue.
       /// </summary>
        public override int Count
        {
            get
            {
                lock (m_queueLock)
                {
                    return base.Count;
                }
            }
        }

        /// <summary>
        /// Return peek element
        /// </summary>
        /// <returns>the peek element of the queue </returns>
        public override T Peek()
        {
            return base.Peek();
        }

        /// <summary>
        /// Return the first item within the queue.
        /// </summary>
        /// <returns>the first element of the queue.</returns>
        public override T Front()
        {
            lock(m_queueLock)
            {
                return base.Front();
            }
        }

        /// <summary>
        /// Return the last item within the queue.
        /// </summary>
        /// <returns>the last element of the queue.</returns>
        public override T Back()
        {
            lock(m_queueLock)
            {
                return base.Back();
            }
        }

		/// <summary>
        /// Insert the new item into the queue.
		/// </summary>
        /// <param name="data">The inserting data.</param>
        public override void Enqueue(T data)
        {
            lock(m_queueLock)
            {
                base.Enqueue(data);
            }
        }

        /// <summary>
        /// Remove the first item from the queue.
        /// </summary>
        public override T Dequeue()
        {
            lock (m_queueLock)
            {
                return base.Dequeue();
            }
        }


        /// <summary>
        /// Erase the given item from the queue.
        /// </summary>
        /// <param name="data">The data to erase.</param>
        /// <returns>true if successful, otherwise false</returns>
        public override bool Erase(T data)
        {
            lock (m_queueLock)
            {
                return base.Erase(data);
            }

        }

        /// <summary>
        /// Clear the queue.
        /// </summary>
        public override void Clear()
        {
            lock (m_queueLock)
            {
                base.Clear();
            }
        }

        /// <summary>
        /// Return the actual queue structure
        /// </summary>
        /// <returns>the actual queue structure</returns>
        public override List<T> GetQueue()
        {
            lock (m_queueLock)
            {
                return base.GetQueue();
            }
        }

        /// <summary>
        /// lock
        /// </summary>
        protected Object m_queueLock = new Object();

    }
}
