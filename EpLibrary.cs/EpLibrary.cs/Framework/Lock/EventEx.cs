using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;

namespace EpLibrary.cs
{
    /// <summary>
    /// A class that handles the event functionality.
    /// </summary>
    public sealed class EventEx:BaseLock
    {
        /// <summary>
        /// event
        /// </summary>
        private EventWaitHandle m_event;
        /// <summary>
        /// Flag for whether the event is raised on creation
        /// </summary>
		private bool m_isInitialRaised;
        /// <summary>
        /// EventResetMode
        /// </summary>
		private EventResetMode m_eventResetMode;
        /// <summary>
        /// event name
        /// </summary>
        private String m_name;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="eventName">name of the event to distinguish</param>
        public EventEx(String eventName = null)
            : base()
        {
            m_eventResetMode=EventResetMode.AutoReset;
	        m_isInitialRaised=true;
            m_name=eventName;
            m_event=new EventWaitHandle(m_isInitialRaised,m_eventResetMode,m_name);
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="isInitialRaised">flag to raise the event on creation</param>
        /// <param name="eventResetMode">EventResetMode</param>
        /// <param name="eventName">name of the event to distinguish</param>
        public EventEx(bool isInitialRaised, EventResetMode eventResetMode, String eventName = null):base()
        {
            m_eventResetMode=eventResetMode;
	        m_isInitialRaised=isInitialRaised;
            m_name=eventName;
            m_event=new EventWaitHandle(m_isInitialRaised,m_eventResetMode,m_name);
        }

        /// <summary>
        /// Default Copy Constructor
        /// </summary>
        /// <param name="b">the object to copy from</param>
		public EventEx(EventEx b):base(b)
        {
            m_isInitialRaised=b.m_isInitialRaised;
	        m_name=b.m_name;
            m_eventResetMode=b.m_eventResetMode;
            m_event=new EventWaitHandle(m_isInitialRaised,m_eventResetMode,m_name);
        }

		~EventEx()
        {
        }

        /// <summary>
        /// Locks the critical section
        /// </summary>
        /// <returns>true if locked, otherwise false</returns>
		public override bool Lock()
        {
            return m_event.WaitOne();
        }

        /// <summary>
        /// Try to lock the critical section
        /// 
        /// If other thread is already in the critical section, it just returns false and continue, otherwise obtain the ciritical section
        /// </summary>
        /// <returns>true if locked, otherwise false</returns>
		public override bool TryLock()
        {
            return m_event.WaitOne(0);
        }

        /// <summary>
        /// Try to lock the critical section for given time
        /// </summary>
        /// <param name="dwMilliSecond">the wait time</param>
        /// <returns>true if locked, otherwise false</returns>
		public override bool TryLockFor(int dwMilliSecond)
        {
            return m_event.WaitOne(dwMilliSecond);
        }

        /// <summary>
        /// Leave the critical section
        /// </summary>
		public override void Unlock()
        {
            m_event.Set();
        }

        /// <summary>
        /// Reset the event raised
        /// </summary>
        /// <returns>true if succeeded otherwise false</returns>
        /// <remarks>if event is not raised then no effect</remarks>
		public bool ResetEvent()
        {
            return m_event.Reset();
        }

        /// <summary>
        /// Set the event to be raised
        /// </summary>
        /// <returns>true if succeeded otherwise false</returns>
        /// <remarks>
        /// if event is already raised then no effect
        /// this function is same as unlock
        /// </remarks>
        public bool SetEvent()
        {
            return m_event.Set();
        }

        /// <summary>
        /// Returns the flag whether this event is resetting manually.
        /// </summary>
        /// <returns>EventResetMode</returns>
        public EventResetMode GetEventResetMode()
        {
            return m_eventResetMode;
        }

        /// <summary>
        /// Wait for the event raised for given time
        /// </summary>
        /// <param name="dwMilliSecond">the wait time in millisecond</param>
        /// <returns>true if the wait is succeeded, otherwise false</returns>
        public bool WaitForEvent(int dwMilliSecond = Timeout.Infinite)
        {
            return m_event.WaitOne(dwMilliSecond);
        }

        /// <summary>
        /// Get actual event
        /// </summary>
        /// <returns>actual event</returns>
        public EventWaitHandle GetEventHandle()
        {
            return m_event;
        }
	

        
    }
}
