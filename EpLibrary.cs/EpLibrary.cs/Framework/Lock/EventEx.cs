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
    public sealed class EventEx:BaseLock
    {
        private EventWaitHandle m_event;
		private bool m_isInitialRaised;
		private EventResetMode m_eventResetMode;
        private String m_name;

        public EventEx(String eventName = null)
            : base()
        {
            m_eventResetMode=EventResetMode.AutoReset;
	        m_isInitialRaised=true;
            m_name=eventName;
            m_event=new EventWaitHandle(m_isInitialRaised,m_eventResetMode,m_name);
        }

        public EventEx(bool isInitialRaised, EventResetMode eventResetMode, String eventName = null):base()
        {
            m_eventResetMode=eventResetMode;
	        m_isInitialRaised=isInitialRaised;
            m_name=eventName;
            m_event=new EventWaitHandle(m_isInitialRaised,m_eventResetMode,m_name);
        }

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

		public override bool Lock()
        {
            return m_event.WaitOne();
        }
		public override bool TryLock()
        {
            return m_event.WaitOne(0);
        }

	
		public override bool TryLockFor(int dwMilliSecond)
        {
            return m_event.WaitOne(dwMilliSecond);
        }

		
		public override void Unlock()
        {
            m_event.Set();
        }

		public bool ResetEvent()
        {
            return m_event.Reset();
        }

        public bool SetEvent()
        {
            return m_event.Set();
        }

        public EventResetMode GetEventResetMode()
        {
            return m_eventResetMode;
        }

        public bool WaitForEvent(int dwMilliSecond = Timeout.Infinite)
        {
            return m_event.WaitOne(dwMilliSecond);
        }

        public EventWaitHandle GetEventHandle()
        {
            return m_event;
        }
	

        
    }
}
