/*! 
@file ThreadSafePQueue.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief ThreadSafePQueue Interface
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

A ThreadSafePQueue Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace EpLibrary.cs
{
    /// <summary>
    /// A class for Thread Safe Priority Queue.
    /// </summary>
    /// <typeparam name="DataType">the element type</typeparam>
    public class ThreadSafePQueue<DataType> where DataType : IComparable<DataType>
    {
        /// <summary>
        /// Reverse order comparer
        /// </summary>
        public class ReverseOrderClass : IComparer<DataType>
        {

            // Calls CaseInsensitiveComparer.Compare with the parameters reversed. 
            int IComparer<DataType>.Compare(DataType x, DataType y)
            {
                return x.CompareTo(y) * -1;
            }
        }
        IComparer<DataType> pQueueComparer = new ReverseOrderClass();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ThreadSafePQueue()
        {
            
        }

        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="b">object to copy from</param>
		public ThreadSafePQueue(ThreadSafePQueue<DataType> b)
        {
            m_queue = new List<DataType>(b.GetQueue());
        }


        /// <summary>
        /// Check if the queue is empty.
        /// </summary>
        /// <returns>true if the queue is empty, otherwise false.</returns>
        public bool IsEmpty()
        {
            lock (m_queueLock)
            {
                return m_queue.Count == 0;
            }
        }


        /// <summary>
        /// Check if the given obj exists in the queue.
        /// </summary>
        /// <param name="data">obj to check</param>
        /// <returns>true if exists, otherwise false.</returns>
        public bool Contains(DataType data)
        {
            lock (m_queueLock)
            {
                return m_queue.Contains(data);
            }
        }

        /// <summary>
        /// Return the size of the queue.
        /// </summary>
        public int Count
        {
            get
            {
                lock (m_queueLock)
                {
                    return m_queue.Count;
                }
            }
        }

        /// <summary>
        /// Return peek element
        /// </summary>
        /// <returns>the peek element of the queue </returns>
        public DataType Peek()
        {
            return Front();
        }

        /// <summary>
        /// Return the first item within the queue.
        /// </summary>
        /// <returns>the first element of the queue.</returns>
        public DataType Front()
        {
            lock (m_queueLock)
            {
                return m_queue.Last();
            }
        }

        /// <summary>
        /// Return the last item within the queue.
        /// </summary>
        /// <returns>the last element of the queue.</returns>
        public DataType Back()
        {
            lock (m_queueLock)
            {
                return m_queue.First();
            }
        }
        /// <summary>
        /// Insert the new item into the priority queue.
        /// </summary>
        /// <param name="data">The inserting data.</param>
        public void Enqueue(DataType data)
        {
            lock(m_queueLock)
            {
                m_queue.Add(data);
                m_queue.Sort(pQueueComparer);
            }
		    
        }

        /// <summary>
        /// Remove the first item from the queue.
        /// </summary>
        public virtual DataType Dequeue()
        {
            lock (m_queueLock)
            {
                DataType data = m_queue[m_queue.Count-1];
                m_queue.RemoveAt(m_queue.Count - 1);
                return data;
            }
        }

        /// <summary>
        /// Erase the given item from the queue.
        /// </summary>
        /// <param name="data">The data to erase.</param>
        /// <returns>true if successful, otherwise false</returns>
        public bool Erase(DataType data)
        {
            lock (m_queueLock)
            {
                int idx = m_queue.BinarySearch(data, pQueueComparer);
                if (idx >= 0)
                {
                    m_queue.RemoveAt(idx);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Clear the queue.
        /// </summary>
        public void Clear()
        {
            lock (m_queueLock)
            {
                m_queue.Clear();
            }
        }

        /// <summary>
        /// Return the actual queue structure
        /// </summary>
        /// <returns>the actual queue structure</returns>
        public List<DataType> GetQueue()
        {
            lock (m_queueLock)
            {
                return new List<DataType>(m_queue);
            }
        }

        /// <summary>
        /// Actual queue structure
        /// </summary>
        protected List<DataType> m_queue = new List<DataType>();

        /// <summary>
        /// lock
        /// </summary>
        protected Object m_queueLock = new Object();

    }
}
