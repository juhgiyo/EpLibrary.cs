using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace EpLibrary.cs
{
    public class Endian
    {
        public static double Swap(double val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            return BitConverter.ToDouble(bytes, 0);
        }

        public static float Swap(float val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            return BitConverter.ToSingle(bytes, 0);
        }
        public static int Swap(int val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
        public static uint Swap(uint val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }
        public static long Swap(long val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            return BitConverter.ToInt64(bytes, 0);
        }
        public static ulong Swap(ulong val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            return BitConverter.ToUInt64(bytes, 0);
        }
        public static short Swap(short val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            return BitConverter.ToInt16(bytes, 0);
        }
        public static ushort Swap(ushort val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            return BitConverter.ToUInt16(bytes, 0);
        }

        public static short HostToNetWorkOrder(short host)
        {
            return IPAddress.HostToNetworkOrder(host);
        }

        public static int HostToNetWorkOrder(int host)
        {
            return IPAddress.HostToNetworkOrder(host);
        }

        public static long HostToNetWorkOrder(long host)
        {
            return IPAddress.HostToNetworkOrder(host);
        }

        public static short NetworkToHostOrder(short host)
        {
            return IPAddress.NetworkToHostOrder(host);
        }

        public static int NetworkToHostOrder(int host)
        {
            return IPAddress.NetworkToHostOrder(host);
        }

        public static long NetworkToHostOrder(long host)
        {
            return IPAddress.NetworkToHostOrder(host);
        }

        public static bool IsLittleEndian()
        {
            return BitConverter.IsLittleEndian;
        }

        public static bool IsBigEndian()
        {
            return !BitConverter.IsLittleEndian;
        }
    }
}
