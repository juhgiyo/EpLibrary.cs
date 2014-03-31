using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpLibrary.cs
{
    /// <summary>
    /// A class for Thread Safe Queue.
    /// </summary>
    /// <typeparam name="DataType">the element type</typeparam>
    public class ThreadSafeQueue<DataType>
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
		public ThreadSafeQueue(ThreadSafeQueue<DataType> b)
        {
            m_queue=b.GetQueue();
        }

		 ~ThreadSafeQueue()
         {
             lock(m_queueLock)
             {
                 m_queue.Clear();
             }
         }

        /// <summary>
         /// Check if the queue is empty.
        /// </summary>
         /// <returns>true if the queue is empty, otherwise false.</returns>
		public bool IsEmpty()
        {
            lock(m_queueLock)
            {
                return m_queue.Count==0;
            }
        }

        /// <summary>
        /// Check if the given obj exists in the queue.
        /// </summary>
        /// <param name="data">obj to check</param>
        /// <returns>true if exists, otherwise false.</returns>
		public bool IsExist(DataType data)
        {
            lock(m_queueLock)
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
        /// Return the first item within the queue.
        /// </summary>
        /// <returns>the first element of the queue.</returns>
		public DataType Front()
        {
            lock(m_queueLock)
            {
                return m_queue.First();
            }
        }

        /// <summary>
        /// Return the last item within the queue.
        /// </summary>
        /// <returns>the last element of the queue.</returns>
		public DataType Back()
        {
            lock(m_queueLock)
            {
                return m_queue.Last();
            }
        }

		/// <summary>
        /// Insert the new item into the queue.
		/// </summary>
        /// <param name="data">The inserting data.</param>
		public virtual void Push(DataType data)
        {
            lock(m_queueLock)
            {
                m_queue.Add(data);
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
                if (m_queue.Contains(data))
                {
                    for (int idx = m_queue.Count - 1; idx >= 0; idx--)
                    {
                        if (m_queue[idx].Equals(data))
                        {
                            m_queue.RemoveAt(idx);
                            return true;
                        }
                    }
                }
                return false;

            }
            
        }

        /// <summary>
        /// Remove the first item from the queue.
        /// </summary>
        public virtual void Pop()
        {
            lock (m_queueLock)
            {
                m_queue.RemoveAt(0);
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
		protected List<DataType> m_queue;

        /// <summary>
        /// lock
        /// </summary>
        protected Object m_queueLock = new Object();

    }
}
