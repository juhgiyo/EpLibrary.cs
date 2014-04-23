/*! 
@file PrimeNum.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief PrimeNum Interface
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

A PrimeNum Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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
