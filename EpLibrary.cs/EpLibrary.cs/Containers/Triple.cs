/*! 
@file Triple.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eppathfinding>
@date July 16, 2013
@brief Triple Interface
@version 2.0

@section LICENSE

The MIT License (MIT)

Copyright (c) 2013 Woong Gyu La <juhgiyo@gmail.com>

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

An Interface for the Triple Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpLibrary.cs
{
    /// <summary>
    /// A Triple class.
    /// </summary>
    /// <typeparam name="T">first object type</typeparam>
    /// <typeparam name="U">second object type</typeparam>
    /// <typeparam name="V">third object type</typeparam>
    public sealed class Triple<T,U,V>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Triple()
        {
        }
        /// <summary>
        /// Default copy constructor
        /// </summary>
        /// <param name="b">the object to copy from</param>
        public Triple(Triple<T,U,V> b)
        {
            first = b.first;
            second = b.second;
            third = b.third;
        }

        /// <summary>
        /// Constructor to set with given values
        /// </summary>
        /// <param name="iFirst">first object</param>
        /// <param name="iSecond">second object</param>
        /// <param name="iThird">third object</param>
        public Triple(T iFirst, U iSecond,V iThird)
        {
            this.first = iFirst;
            this.second = iSecond;
            this.third = iThird;
        }
        /// <summary>
        /// first object
        /// </summary>
        public T first { get; set; }
        /// <summary>
        /// second object
        /// </summary>
        public U second { get; set; }
        /// <summary>
        /// third object
        /// </summary>
        public V third { get; set; }
    }
}
