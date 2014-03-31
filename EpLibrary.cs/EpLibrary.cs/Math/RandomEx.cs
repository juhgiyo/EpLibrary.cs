using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpLibrary.cs
{
    /// <summary>
    /// A class that generate the random number.
    /// </summary>
    public class RandomEx
    {
        private static Random m_random = new Random();
        /// <summary>
        /// Return a random number
        /// </summary>
        /// <returns>generated random number</returns>
        public static float Random()
        {
            var result = m_random.NextDouble();
            return (float)result;
        }
    }
}
