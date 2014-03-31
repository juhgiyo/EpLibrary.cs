using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpLibrary.cs
{
    /// <summary>
    /// A class that calculates Prime Number.
    /// </summary>
    public class PrimeNum
    {
        /// <summary>
        /// Check if the given number is a prime number
        /// </summary>
        /// <param name="x">the value to check if it is a prime number.</param>
        /// <returns>if x is a prime number, false otherwise</returns>
        public static bool IsPrime(uint x)
        {
            if (x <= 1)
                return false;
            if (x == 2 || x == 3)
                return true;
            if (x % 2 == 0 || x % 3 == 0)
                return false;
            uint sqrtValue =(uint) Math.Sqrt(x);
            uint trav = 1;
            uint multP1 = (trav * 6) + 1;
            uint multP2 = (trav * 6) - 1;
            while (multP2 <= sqrtValue)
            {
                if (x % multP1 == 0 || x % multP2 == 0)
                    return false;
                trav++;
                multP1 = (trav * 6) + 1;
                multP2 = (trav * 6) - 1;
            }
            if (multP1 <= sqrtValue && x % multP1 == 0)
                return false;
            return true;
        }
        /// <summary>
        /// Find the first prime number larger than given number.
        /// </summary>
        /// <param name="x">the value to find a first prime number larger than x.</param>
        /// <returns>first prime number larger than x</returns>
        public static uint NextPrime(uint x)
        {
            uint multIdx=x/6;
	        uint check1;
	        uint check2;
	        while(true)
	        {
		        check1=multIdx*6-1;
		        if(check1>x && PrimeNum.IsPrime(check1))
		        {
			        return check1;
		        }
		        check2=multIdx*6+1;
		        if(check2>x && PrimeNum.IsPrime(check2))
			        return check2;
		        multIdx++;
	        }
        }
    }
}
