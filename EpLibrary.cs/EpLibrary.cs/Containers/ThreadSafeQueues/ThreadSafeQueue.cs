using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpLibrary.cs
{
    public class ThreadSafeQueue<DataType>
    {
        public ThreadSafeQueue()
        {

        }

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


		public bool IsEmpty()
        {
            lock(m_queueLock)
            {
                return m_queue.Count==0;
            }
        }

		public bool IsExist(DataType data)
        {
            lock(m_queueLock)
            {
                return m_queue.Contains(data);
            }
        }

		public int Size()
        {
            lock(m_queueLock)
            {
                return m_queue.Count;
            }
        }

		public DataType Front()
        {
            lock(m_queueLock)
            {
                return m_queue.First();
            }
        }

		public DataType Back()
        {
            lock(m_queueLock)
            {
                return m_queue.Last();
            }
        }

		
		public virtual void Push(DataType data)
        {
            lock(m_queueLock)
            {
                m_queue.Add(data);
            }
        }

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

		/*!
		Remove the first item from the queue.
		*/
        public virtual void Pop()
        {
            lock (m_queueLock)
            {
                m_queue.RemoveAt(0);
            }
        }

		/*!
		Clear the queue.
		*/
		public void Clear()
        {
            lock (m_queueLock)
            {
                m_queue.Clear();
            }
        }

        public List<DataType> GetQueue()
        {
            lock (m_queueLock)
            {
                return new List<DataType>(m_queue);
            }
        }

		/// Actual queue structure
		protected List<DataType> m_queue;

       	/// callback Lock
        protected Object m_queueLock = new Object();

    }
}
