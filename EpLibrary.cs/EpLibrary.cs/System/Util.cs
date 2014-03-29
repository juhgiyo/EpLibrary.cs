using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace EpLibrary.cs
{
    public class Util
    {
        public static int GetNumberOfCores()
        {
            int coreCount = 0;
            foreach (var item in new ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                coreCount += int.Parse(item["NumberOfCores"].ToString());
            }
            return coreCount;
        }

        private static Random m_random = new Random();
        public static float Random()
        {
            var result = m_random.NextDouble();
            return (float)result;
        }
    }
}
