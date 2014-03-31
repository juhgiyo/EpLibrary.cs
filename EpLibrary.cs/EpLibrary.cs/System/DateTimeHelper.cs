using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpLibrary.cs
{
    public class DateTimeHelper
    {

        public static DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
        
        public static TimeSpan Diff(DateTime time1, DateTime time2)
        {
            return time1.Subtract(time2);
        }

        public static double AbsDiffInMilliSec(DateTime time1, DateTime time2)
        {
            return Math.Abs(time1.Subtract(time2).TotalMilliseconds);
        }

        public static double DiffInMilliSec(DateTime time1, DateTime time2)
        {
            return time1.Subtract(time2).TotalMilliseconds;
        }

        public static double AbsDiffInSec(DateTime time1, DateTime time2)
        {
            return Math.Abs(time1.Subtract(time2).TotalSeconds);
        }

        public static double DiffInSec(DateTime time1, DateTime time2)
        {
            return time1.Subtract(time2).TotalSeconds;
        }
        public static double AbsDiffInMin(DateTime time1, DateTime time2)
        {
            return Math.Abs(time1.Subtract(time2).TotalMinutes);
        }

        public static double DiffInMin(DateTime time1, DateTime time2)
        {
            return time1.Subtract(time2).TotalMinutes;
        }

        public static double AbsDiffInHour(DateTime time1, DateTime time2)
        {
            return Math.Abs(time1.Subtract(time2).TotalHours);
        }

        public static double DiffInHour(DateTime time1, DateTime time2)
        {
            return time1.Subtract(time2).TotalHours;
        }

        public static double AbsDiffInDay(DateTime time1, DateTime time2)
        {
            return Math.Abs(time1.Subtract(time2).TotalDays);
        }

        public static double DiffInDay(DateTime time1, DateTime time2)
        {
            return time1.Subtract(time2).TotalDays;
        }
    }
}
