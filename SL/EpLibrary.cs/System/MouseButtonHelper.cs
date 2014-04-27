using System;
using System.Windows;
using System.Windows.Input;

namespace EpLibrary.cs
{
    public static class MouseButtonHelper
    {
        private const long k_DoubleClickSpeed = 500;
        private const double k_MaxMoveDistance = 10;

        private static long m_LastClickTicks = 0;
        private static Point m_LastPosition;
        private static object m_LastSender;

        public static bool IsDoubleClick(object sender, MouseButtonEventArgs e)
        {
            bool senderMatch = sender.Equals(m_LastSender);
            long clickTicks = DateTime.Now.Ticks;
            Point position = e.GetPosition(null);
            long elapsedTicks = clickTicks - m_LastClickTicks;
            long elapsedTime = elapsedTicks / TimeSpan.TicksPerMillisecond;
            double distance = position.Distance(m_LastPosition);

            if (elapsedTime > k_DoubleClickSpeed)
            { // if overdue, no doubleclick
                m_LastClickTicks = clickTicks;
                m_LastPosition = position;
                m_LastSender = sender;
                return false;
            }
            else if (senderMatch && elapsedTime <= k_DoubleClickSpeed && distance <= k_MaxMoveDistance)
            {
                // Double click!
                m_LastClickTicks = 0;
                m_LastSender = null;
                return true;
            }

            // Not a double click
            m_LastClickTicks = clickTicks;
            m_LastPosition = position;
            return false;
        }

        private static double Distance(this Point pointA, Point pointB)
        {
            double x = pointA.X - pointB.X;
            double y = pointA.Y - pointB.Y;
            return Math.Sqrt(x * x + y * y);
        }
    }
}
