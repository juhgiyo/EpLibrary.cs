using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpLibrary.cs
{
    public class PrimeNum
    {
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
