using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace EpLibrary.cs
{
    /// <summary>
    /// A class for Thread Safe Queue.
    /// </summary>
    /// <typeparam name="T">the element type</typeparam>
    public class ThreadSafeQueue<T>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ThreadSafeQueue()
        {

        }

        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="b">the object to copy from</param>
        public ThreadSafeQueue(ThreadSafeQueue<T> b)
        {
            m_queue = new Queue<T>(b.GetQueue());
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
        public bool Contains(T data)
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
        public T Peek()
        {
            return Front();
        }

        /// <summary>
        /// Return the first item within the queue.
        /// </summary>
        /// <returns>the first element of the queue.</returns>
        public T Front()
        {
            lock (m_queueLock)
            {
                return m_queue.First();
            }
        }

        /// <summary>
        /// Return the last item within the queue.
        /// </summary>
        /// <returns>the last element of the queue.</returns>
        public T Back()
        {
            lock (m_queueLock)
            {
                return m_queue.Last();
            }
        }

        /// <summary>
        /// Insert the new item into the queue.
        /// </summary>
        /// <param name="data">The inserting data.</param>
        public virtual void Enqueue(T data)
        {
            lock (m_queueLock)
            {
                m_queue.Enqueue(data);
            }
        }


        /// <summary>
        /// Remove the first item from the queue.
        /// </summary>
        public virtual T Dequeue()
        {
            lock (m_queueLock)
            {
                return m_queue.Dequeue();
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
        public List<T> GetQueue()
        {
            lock (m_queueLock)
            {
                return new List<T>(m_queue);
            }
        }

        /// <summary>
        /// Actual queue structure
        /// </summary>
        protected Queue<T> m_queue = new Queue<T>();

        /// <summary>
        /// lock
        /// </summary>
        protected Object m_queueLock = new Object();

    }
}
