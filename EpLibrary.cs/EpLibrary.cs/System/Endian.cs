using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace EpLibrary.cs
{
    /// <summary>
    /// A class for Endian.
    /// </summary>
    public class Endian
    {
        /// <summary>
        /// Swap double endianness and return the swapped value.
        /// </summary>
        /// <param name="val">the value to swap.</param>
        /// <returns>the swapped value</returns>
        public static double Swap(double val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            return BitConverter.ToDouble(bytes, 0);
        }

        /// <summary>
        /// Swap float endianness and return the swapped value.
        /// </summary>
        /// <param name="val">the value to swap.</param>
        /// <returns>the swapped value</returns>
        public static float Swap(float val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            return BitConverter.ToSingle(bytes, 0);
        }

        /// <summary>
        /// Swap int endianness and return the swapped value.
        /// </summary>
        /// <param name="val">the value to swap.</param>
        /// <returns>the swapped value</returns>
        public static int Swap(int val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Swap uint endianness and return the swapped value.
        /// </summary>
        /// <param name="val">the value to swap.</param>
        /// <returns>the swapped value</returns>
        public static uint Swap(uint val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        /// <summary>
        /// Swap long endianness and return the swapped value.
        /// </summary>
        /// <param name="val">the value to swap.</param>
        /// <returns>the swapped value</returns>
        public static long Swap(long val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            return BitConverter.ToInt64(bytes, 0);
        }

        /// <summary>
        /// Swap ulong endianness and return the swapped value.
        /// </summary>
        /// <param name="val">the value to swap.</param>
        /// <returns>the swapped value</returns>
        public static ulong Swap(ulong val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            return BitConverter.ToUInt64(bytes, 0);
        }

        /// <summary>
        /// Swap short endianness and return the swapped value.
        /// </summary>
        /// <param name="val">the value to swap.</param>
        /// <returns>the swapped value</returns>
        public static short Swap(short val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            return BitConverter.ToInt16(bytes, 0);
        }

        /// <summary>
        /// Swap ushort endianness and return the swapped value.
        /// </summary>
        /// <param name="val">the value to swap.</param>
        /// <returns>the swapped value</returns>
        public static ushort Swap(ushort val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            return BitConverter.ToUInt16(bytes, 0);
        }

        /// <summary>
        /// Change given short value in network order and return the changed value
        /// </summary>
        /// <param name="host">the value to change order</param>
        /// <returns>value in network order</returns>
        public static short HostToNetWorkOrder(short host)
        {
            return IPAddress.HostToNetworkOrder(host);
        }

        /// <summary>
        /// Change given int value in network order and return the changed value
        /// </summary>
        /// <param name="host">the value to change order</param>
        /// <returns>value in network order</returns>
        public static int HostToNetWorkOrder(int host)
        {
            return IPAddress.HostToNetworkOrder(host);
        }

        /// <summary>
        /// Change given long value in network order and return the changed value
        /// </summary>
        /// <param name="host">the value to change order</param>
        /// <returns>value in network order</returns>
        public static long HostToNetWorkOrder(long host)
        {
            return IPAddress.HostToNetworkOrder(host);
        }

        /// <summary>
        /// Change given short value in host order and return the changed value
        /// </summary>
        /// <param name="host">the value to change order</param>
        /// <returns>value in host order</returns>
        public static short NetworkToHostOrder(short host)
        {
            return IPAddress.NetworkToHostOrder(host);
        }

        /// <summary>
        /// Change given int value in host order and return the changed value
        /// </summary>
        /// <param name="host">the value to change order</param>
        /// <returns>value in host order</returns>
        public static int NetworkToHostOrder(int host)
        {
            return IPAddress.NetworkToHostOrder(host);
        }

        /// <summary>
        /// Change given long value in host order and return the changed value
        /// </summary>
        /// <param name="host">the value to change order</param>
        /// <returns>value in host order</returns>
        public static long NetworkToHostOrder(long host)
        {
            return IPAddress.NetworkToHostOrder(host);
        }

        /// <summary>
        /// Return if current machine is in little endian
        /// </summary>
        /// <returns>true if current machine is in little endian, otherwise false</returns>
        public static bool IsLittleEndian()
        {
            return BitConverter.IsLittleEndian;
        }

        /// <summary>
        /// Return if current machine is in big endian
        /// </summary>
        /// <returns>true if current machine is in big endian, otherwise false</returns>
        public static bool IsBigEndian()
        {
            return !BitConverter.IsLittleEndian;
        }
    }
}
