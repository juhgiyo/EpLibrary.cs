using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace EpLibrary.cs
{
    public class ThreadSafePQueue<DataType>:ThreadSafeQueue<DataType> where DataType : IComparable<DataType>
    {
        
        public ThreadSafePQueue():base()
        {
            
        }

		public ThreadSafePQueue(ThreadSafePQueue<DataType> b):base(b)
        {

        }

        ~ThreadSafePQueue() { }


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
