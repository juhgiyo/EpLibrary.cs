/*! 
@file PriorityQueue.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief PriorityQueue Interface
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

A PriorityQueue Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace EpLibrary.cs
{
    /// <summary>
    /// A template PriorityQueue class
    /// </summary>
    /// <typeparam name="T">Queue element type</typeparam>
    public sealed class PriorityQueue<T> where T : IComparable
    {
        private List<T> m_data;

        public PriorityQueue()
        {
            this.m_data = new List<T>();
        }

        public PriorityQueue(PriorityQueue<T> b)
        {
            m_data = new List<T>(b.m_data);
        }

        /// <summary>
        /// Insert the new item into the queue.
        /// </summary>
        /// <param name="queueItem">The inserting item.</param>
        public void Enqueue(T queueItem)
        {
            m_data.Add(queueItem);
            m_data.Sort();
        }

        /// <summary>
        /// Clear the queue
        /// </summary>
        public void Clear()
        {
            m_data.Clear();
        }

        /// <summary>
        /// Remove the first item from the queue.
        /// </summary>
        /// <returns>removed item</returns>
        public T Dequeue()
        {
            T frontItem = m_data[0];
            m_data.RemoveAt(0);
            return frontItem;
        }

        /// <summary>
        /// Return the first item within the queue.
        /// </summary>
        /// <returns>the first element of the queue.</returns>
        public T Peek()
        {
            T frontItem = m_data[0];
            return frontItem;
        }

        /// <summary>
        /// Check if the given item exists in the queue.
        /// </summary>
        /// <param name="queueItem">item to check</param>
        /// <returns>true if exists, otherwise false</returns>
        public bool Contains(T queueItem)
        {
            return m_data.Contains(queueItem);
        }

        /// <summary>
        /// Return the number of element in the queue.
        /// </summary>
        public int Count
        {
            get
            {
                return m_data.Count;
            }
        }
    }

}