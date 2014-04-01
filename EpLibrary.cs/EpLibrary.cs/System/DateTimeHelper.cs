/*! 
@file DateTimeHelper.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief DateTimeHelper Interface
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

A DateTimeHelper Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpLibrary.cs
{
    /// <summary>
    /// This is a class for DateTime Helping Class
    /// </summary>
    public class DateTimeHelper
    {
        /// <summary>
        /// Return the current date time
        /// </summary>
        /// <returns>the current date time.</returns>
        public static DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
        
        /// <summary>
        /// Find the difference between two time and return the difference
        /// </summary>
        /// <param name="time1">the first time to find the difference</param>
        /// <param name="time2">the second time to find the difference</param>
        /// <returns>the difference between two given time in TimeSpan</returns>
        public static TimeSpan Diff(DateTime time1, DateTime time2)
        {
            return time1.Subtract(time2);
        }

        /// <summary>
        /// Find the absolute difference between two time and return the difference in millisecond 
        /// </summary>
        /// <param name="time1">the first time to find the difference</param>
        /// <param name="time2">the second time to find the difference</param>
        /// <returns>the difference between two given time in millisecond</returns>
        /// <remarks>result = time1 - time2</remarks>
        public static double AbsDiffInMilliSec(DateTime time1, DateTime time2)
        {
            return Math.Abs(time1.Subtract(time2).TotalMilliseconds);
        }

        /// <summary>
        /// Find the difference between two time and return the difference in millisecond 
        /// </summary>
        /// <param name="time1">the first time to find the difference</param>
        /// <param name="time2">the second time to find the difference</param>
        /// <returns>the difference between two given time in millisecond</returns>
        /// <remarks>result = time1 - time2</remarks>
        public static double DiffInMilliSec(DateTime time1, DateTime time2)
        {
            return time1.Subtract(time2).TotalMilliseconds;
        }

        /// <summary>
        /// Find the absolute difference between two time and return the difference in second 
        /// </summary>
        /// <param name="time1">the first time to find the difference</param>
        /// <param name="time2">the second time to find the difference</param>
        /// <returns>the difference between two given time in second</returns>
        /// <remarks>result = time1 - time2</remarks>
        public static double AbsDiffInSec(DateTime time1, DateTime time2)
        {
            return Math.Abs(time1.Subtract(time2).TotalSeconds);
        }

        /// <summary>
        /// Find the difference between two time and return the difference in second 
        /// </summary>
        /// <param name="time1">the first time to find the difference</param>
        /// <param name="time2">the second time to find the difference</param>
        /// <returns>the difference between two given time in second</returns>
        /// <remarks>result = time1 - time2</remarks>
        public static double DiffInSec(DateTime time1, DateTime time2)
        {
            return time1.Subtract(time2).TotalSeconds;
        }

        /// <summary>
        /// Find the absolute difference between two time and return the difference in minute 
        /// </summary>
        /// <param name="time1">the first time to find the difference</param>
        /// <param name="time2">the second time to find the difference</param>
        /// <returns>the difference between two given time in minute</returns>
        /// <remarks>result = time1 - time2</remarks>
        public static double AbsDiffInMin(DateTime time1, DateTime time2)
        {
            return Math.Abs(time1.Subtract(time2).TotalMinutes);
        }

        /// <summary>
        /// Find the difference between two time and return the difference in minute 
        /// </summary>
        /// <param name="time1">the first time to find the difference</param>
        /// <param name="time2">the second time to find the difference</param>
        /// <returns>the difference between two given time in minute</returns>
        /// <remarks>result = time1 - time2</remarks>
        public static double DiffInMin(DateTime time1, DateTime time2)
        {
            return time1.Subtract(time2).TotalMinutes;
        }

        /// <summary>
        /// Find the absolute difference between two time and return the difference in hour 
        /// </summary>
        /// <param name="time1">the first time to find the difference</param>
        /// <param name="time2">the second time to find the difference</param>
        /// <returns>the difference between two given time in hour</returns>
        /// <remarks>result = time1 - time2</remarks>
        public static double AbsDiffInHour(DateTime time1, DateTime time2)
        {
            return Math.Abs(time1.Subtract(time2).TotalHours);
        }

        /// <summary>
        /// Find the difference between two time and return the difference in hour 
        /// </summary>
        /// <param name="time1">the first time to find the difference</param>
        /// <param name="time2">the second time to find the difference</param>
        /// <returns>the difference between two given time in hour</returns>
        /// <remarks>result = time1 - time2</remarks>
        public static double DiffInHour(DateTime time1, DateTime time2)
        {
            return time1.Subtract(time2).TotalHours;
        }

        /// <summary>
        /// Find the absolute difference between two time and return the difference in day
        /// </summary>
        /// <param name="time1">the first time to find the difference</param>
        /// <param name="time2">the second time to find the difference</param>
        /// <returns>the difference between two given time in day</returns>
        /// <remarks>result = time1 - time2</remarks>
        public static double AbsDiffInDay(DateTime time1, DateTime time2)
        {
            return Math.Abs(time1.Subtract(time2).TotalDays);
        }

        /// <summary>
        /// Find the difference between two time and return the difference in day
        /// </summary>
        /// <param name="time1">the first time to find the difference</param>
        /// <param name="time2">the second time to find the difference</param>
        /// <returns>the difference between two given time in day</returns>
        /// <remarks>result = time1 - time2</remarks>
        public static double DiffInDay(DateTime time1, DateTime time2)
        {
            return time1.Subtract(time2).TotalDays;
        }
    }
}
