using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace EpLibrary.cs
{
    /// <summary>
    /// A class for Thread Safe Priority Queue.
    /// </summary>
    /// <typeparam name="DataType">the element type</typeparam>
    public class ThreadSafePQueue<DataType>:ThreadSafeQueue<DataType> where DataType : IComparable<DataType>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ThreadSafePQueue():base()
        {
            
        }

        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="b">object to copy from</param>
		public ThreadSafePQueue(ThreadSafePQueue<DataType> b):base(b)
        {

        }

        ~ThreadSafePQueue() { }

        /// <summary>
        /// Insert the new item into the priority queue.
        /// </summary>
        /// <param name="data">The inserting data.</param>
        public override void Push(DataType data)
        {
            lock(m_queueLock)
            {
		        if(m_queue.Count>0)
		        {
			        int retIdx=m_queue.BinarySearch(data);
                    if (retIdx < 0)
                        m_queue.Insert(~retIdx, data);
                    else
                        Debug.Assert(false, "Same Object already in the queue!");
		        }
		        else
		        {
			        m_queue.Add(data);
		        }
            }
		    
        }
    }
}
