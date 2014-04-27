using System;
using System.Windows;
using System.Threading;

namespace EpLibrary.cs
{
    public static class FrameworkElementEx
    {
        public static bool InvokeRequired(this FrameworkElement element)
        {
            return !element.Dispatcher.CheckAccess();
        }

        public static void Invoke(this FrameworkElement element, Delegate action, params object[] values)
        {
            if (element.InvokeRequired())
            {
                using (AutoResetEvent are = new AutoResetEvent(false))
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        action.DynamicInvoke(values);
                        are.Set();
                    });
                    are.WaitOne();
                }
            }
            else
                action.DynamicInvoke(values);
        }

    }
}
