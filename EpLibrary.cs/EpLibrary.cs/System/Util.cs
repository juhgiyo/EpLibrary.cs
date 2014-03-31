using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace EpLibrary.cs
{
    /// <summary>
    /// Utility Class
    /// </summary>
    public class Util
    {
        /// <summary>
        /// Return number of cores during runtime.
        /// </summary>
        /// <returns>number of cores</returns>
        public static int GetNumberOfCores()
        {
            int coreCount = 0;
            foreach (var item in new ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                coreCount += int.Parse(item["NumberOfCores"].ToString());
            }
            return coreCount;
        }

    }
}
