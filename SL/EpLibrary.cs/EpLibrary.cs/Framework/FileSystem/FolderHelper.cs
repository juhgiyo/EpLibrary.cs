/*! 
@file FolderHelper.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eplibrary.cs>
@date April 01, 2014
@brief FolderHelper Interface
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

A FolderHelper Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Runtime.InteropServices;

namespace EpLibrary.cs
{
    /// <summary>
    /// This is a class for Folder Processing Class
    /// </summary>
    public class FolderHelper
    {
        /// <summary>
        /// Max path size
        /// </summary>
        public const int MAX_PATH = 260;

        /// <summary>
        /// Create given folder path from file system
        /// </summary>
        /// <param name="path">the file path to create</param>
        /// <returns>true if the folder is created successfully, otherwise false</returns>
        public static DirectoryInfo CreateFolder(String path)
        {
            try
            {
                return Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " >" + ex.StackTrace);
                return null;
            }
       }

        /// <summary>
        /// Delete given folder path from file system
        /// </summary>
        /// <param name="path"> the file path to delete</param>
        public static void DeleteFolder(String path)
        {
            try
            {
                Directory.Delete(path, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " >" + ex.StackTrace);
            } 
        }

        /// <summary>
        /// Check if the given path exists
        /// </summary>
        /// <param name="path">the file path to check</param>
        /// <returns>true if the folder exists, otherwise false</returns>
        public static bool IsPathExist(String path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Copy the source file to the destination file
        /// </summary>
        /// <param name="fromFile">the source file path</param>
        /// <param name="toFile">the destination file path</param>
        /// <param name="overWrite">overwrite if exist</param>
        /// <returns>true if the copied successfully, otherwise false</returns>
        public static bool CopyFile(String fromFile, String toFile, bool overWrite = true)
        {
            try
            {
                File.Copy(fromFile, toFile, overWrite);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " >" + ex.StackTrace);
                return false;
            }
        }
        /// <summary>
        /// Return only the directory of given file path with file name
        /// </summary>
        /// <param name="filePath">the full path of the file with file name</param>
        /// <returns>the directory ending with "\", which contains the given file</returns>
        public static String GetPathOnly(String filePath)
        {
            String retString = filePath;
            int strLength = retString.Length;

            for (int stringTrav = strLength - 1; stringTrav >= 0; stringTrav--)
            {
                if (retString[stringTrav].CompareTo('\\')!=0)
                    retString=retString.Remove(stringTrav, 1);
                else
                    return retString;
            }
            return retString;
        }

        /// <summary>
        /// Return only the extension of given file path
        /// </summary>
        /// <param name="filePath">the full path of the file with file name</param>
        /// <returns>the extension of given file path</returns>
        public static String GetFileExtension(String filePath)
        {
            String tmpString = filePath;
            //size_t strLength=System::TcsLen(filePath);
            int strLength = tmpString.Length;
            String retString = "";
            for (int stringTrav = strLength - 1; stringTrav >= 0; stringTrav--)
            {
                if (tmpString[stringTrav].CompareTo('.')==0)
                {
                    retString=tmpString.Remove(0, stringTrav + 1);
                    break;
                }
            }
            return retString;
        }

        /// <summary>
        /// Return the simple name of the file without extension
        /// </summary>
        /// <param name="filePath">the full path of the file with file name</param>
        /// <returns>the simple name of given file path without extension</returns>
        public static String GetFileTitle(String filePath)
        {
            String tmpString = filePath;
            String retString = "";
            int strLength = tmpString.Length;
            for (int stringTrav = strLength - 1; stringTrav >= 0; stringTrav--)
            {
                if (tmpString[stringTrav].CompareTo('.')==0)
                {
                    tmpString=tmpString.Remove(stringTrav,tmpString.Length- stringTrav);
                    break;
                }
            }

            strLength = tmpString.Length;
            for (int stringTrav = strLength - 1; stringTrav >= 0; stringTrav--)
            {
                if (tmpString[stringTrav].CompareTo('\\')==0)
                {
                    tmpString = tmpString.Remove(0, stringTrav + 1);
                    retString = tmpString;
                    break;
                }
            }
            return retString;
        }

        /// <summary>
        /// Return the simple name of the file
        /// </summary>
        /// <param name="filePath">the full path of the file with file name</param>
        /// <returns>the simple name of given file path</returns>
        public static String GetFileName(String filePath)
        {
            String tmpString = filePath;
            String retString = "";
            int strLength = tmpString.Length;
            for (int stringTrav = strLength - 1; stringTrav >= 0; stringTrav--)
            {
                if (tmpString[stringTrav].CompareTo('\\')==0)
                {
                    tmpString=tmpString.Remove(0, stringTrav + 1);
                    retString = tmpString;
                    break;
                }
            }
            return retString;
        }
    }


}
