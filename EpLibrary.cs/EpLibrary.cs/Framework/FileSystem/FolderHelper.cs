using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        /// Show the Choose Folder Dialog and return true if successfully folder is chosen with chosen folder path.
        /// </summary>
        /// <param name="title">the title of the choose folder dialog</param>
        /// <returns>selected folder path</returns>
        public static String ChooseFolder(String title)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.Description = title;
            folderBrowserDialog1.ShowNewFolderButton = true;
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                return folderBrowserDialog1.SelectedPath;
            }
            return null;
        }
        /// <summary>
        /// Show the Choose Folder Dialog and return true if successfully folder is chosen with chosen folder path.
        /// </summary>
        /// <param name="title">the title of the choose folder dialog</param>
        /// <param name="rootFolder">the root folder</param>
        /// <returns>selected folder path</returns>
        public static String ChooseFolder(String title, Environment.SpecialFolder rootFolder)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.Description = title;
            folderBrowserDialog1.ShowNewFolderButton = true;
            folderBrowserDialog1.RootFolder = rootFolder;
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                return folderBrowserDialog1.SelectedPath;
            }
            return null;
        }

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
            catch
            {
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
            catch
            {
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
            catch
            {
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
                    retString.Remove(stringTrav, 1);
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
                    tmpString.Remove(0, stringTrav + 1);
                    retString = tmpString;
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
                    tmpString.Remove(stringTrav,tmpString.Length- stringTrav);
                    break;
                }
            }

            strLength = tmpString.Length;
            for (int stringTrav = strLength - 1; stringTrav >= 0; stringTrav--)
            {
                if (tmpString[stringTrav].CompareTo('\\')==0)
                {
                    tmpString.Remove(0, stringTrav + 1);
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
                    tmpString.Remove(0, stringTrav + 1);
                    retString = tmpString;
                    break;
                }
            }
            return retString;
        }

        [DllImport("coredll.dll", SetLastError = true)]
        private static extern int GetModuleFileName(IntPtr hModule, StringBuilder lpFilename, int nSize); 

        /// <summary>
        /// Return the full path with file name for the current executable file
        /// </summary>
        /// <returns>the full path of the current module</returns>
        public static String GetModuleFileName()
        {
            StringBuilder exePath = new StringBuilder(FolderHelper.MAX_PATH);
            int exePathLen = GetModuleFileName((IntPtr)0, exePath, exePath.Capacity);
            return exePath.ToString();
        }
        
        /// <summary>
        /// Return only the directory ending with "\" which contains the current executable file
        /// </summary>
        /// <returns>the directory ending with "\", which contains the current module</returns>
        public static String GetModuleFileDirectory()
        {
            String retString = GetModuleFileName();
            return GetPathOnly(retString);
        }
    }




    /// <summary>
    /// his is a class for Open File Dialog
    /// </summary>
	public class OpenFileDialogEx
    {
	    private OpenFileDialog openFileDialog=new OpenFileDialog();
        private IWin32Window owner = null;
		
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="title">the title of the dialog</param>
        /// <param name="defaultExt">the default extension</param>
        /// <param name="defaultDir">the default directory</param>
        /// <param name="filter">the file extension filter</param>
        /// <param name="owner">the parent window</param>
		public OpenFileDialogEx(String title=null, String defaultExt=null,String defaultDir=null,String filter=null, IWin32Window owner=null)
        {
            openFileDialog.Title=title;
            openFileDialog.DefaultExt=defaultExt;
            openFileDialog.InitialDirectory=defaultDir;
            openFileDialog.Filter=filter;
            openFileDialog.Multiselect=false;
            this.owner = owner;

        }

	
		~OpenFileDialogEx()
        {
        }

	
        /// <summary>
        /// Return full path and filename
        /// </summary>
        /// <returns>full path and filename</returns>
		public String GetPathName()
        {
            return openFileDialog.FileName;
        }

        /// <summary>
        /// Return only filename
        /// </summary>
        /// <returns>filename</returns>
		public String GetFileName()
        {
            return FolderHelper.GetFileName(openFileDialog.FileName);
        }

        /// <summary>
        /// Return only ext
        /// </summary>
        /// <returns>ext</returns>
		public String GetFileExt()
        {
            return FolderHelper.GetFileExtension(openFileDialog.FileName);
        }

        /// <summary>
        /// Return file title
        /// </summary>
        /// <returns>file title</returns>
		public String GetFileTitle()
        {
            return FolderHelper.GetFileTitle(openFileDialog.FileName);
        }


        /// <summary>
        /// Show dialog and return DialogResult
        /// </summary>
        /// <returns>DialogResult</returns>
        public virtual DialogResult ShowDialog()
        {
            if(owner!=null)
                return openFileDialog.ShowDialog(owner);
            return openFileDialog.ShowDialog();
        }
	}


    /// <summary>
    /// This is a class for Open Multi-File Dialog
    /// </summary>
	public class OpenMultiFileDialog{
		private OpenFileDialog openFileDialog=new OpenFileDialog();
        private IWin32Window owner = null;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="title">the title of the dialog</param>
        /// <param name="defaultExt">The default extension</param>
        /// <param name="defaultDir">The default directory</param>
        /// <param name="filter">The file extension filter</param>
        /// <param name="owner">the parent window</param>
		public OpenMultiFileDialog(String title=null, String defaultExt=null,String defaultDir=null,String filter=null, IWin32Window owner=null)
        {
            openFileDialog.Title=title;
            openFileDialog.DefaultExt=defaultExt;
            openFileDialog.InitialDirectory=defaultDir;
            openFileDialog.Filter=filter;
            openFileDialog.Multiselect=true;
            this.owner = owner;
        }

		~OpenMultiFileDialog()
        {
        }


        /// <summary>
        /// Return the list of selected file names
        /// </summary>
        /// <returns>the list of selected file names</returns>
		public String[] GetFileNames()
        {
            return openFileDialog.FileNames;
        }

        /// <summary>
        /// Show dialog and return DialogResult
        /// </summary>
        /// <returns>DialogResult</returns>
        public virtual DialogResult ShowDialog()
        {
            if (owner != null)
                return openFileDialog.ShowDialog(owner);
            return openFileDialog.ShowDialog();
        }
	}

    /// <summary>
    /// This is a class for Open Folder Dialog
    /// </summary>
	public class OpenFolderDialog
    {
	    private FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        private IWin32Window owner = null;
		
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="title">the title of the dialog</param>
        /// <param name="showNewFolderButton">flag to show new folder button or not</param>
        /// <param name="owner">the parent window</param>
		public OpenFolderDialog(String title=null, bool showNewFolderButton=true, IWin32Window owner=null)
        {
              
            folderBrowserDialog.Description = title;
            folderBrowserDialog.ShowNewFolderButton = showNewFolderButton;
            this.owner = owner;
        }


		~OpenFolderDialog()
        {}


        /// <summary>
        /// Return full directory path
        /// </summary>
        /// <returns>full directory path</returns>
		public String GetPathName() 
        {
            return folderBrowserDialog.SelectedPath;
        }

        /// <summary>
        /// Show dialog and return DialogResult
        /// </summary>
        /// <returns>DialogResult</returns>
        public virtual DialogResult ShowDialog()
        {
            if (owner != null)
                return folderBrowserDialog.ShowDialog(owner);
            return folderBrowserDialog.ShowDialog();
        }
	}


    /// <summary>
    /// This is a class for Save File Dialog
    /// </summary>
	public class SaveFileDialogEx
    {
	    private SaveFileDialog saveFileDialog=new SaveFileDialog();
        private IWin32Window owner = null;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="title">the title of the dialog</param>
        /// <param name="defaultExt">The default extension</param>
        /// <param name="defaultDir">The default directory</param>
        /// <param name="filter">The file extension filter</param>
        /// <param name="overwritePrompt">flag to prompt when overwrite</param>
        /// <param name="owner">The parent window</param>
        public SaveFileDialogEx(String title = null, String defaultExt = null, String defaultDir = null, String filter = null, bool overwritePrompt = true, IWin32Window owner = null)
        {
            saveFileDialog.Title=title;
            saveFileDialog.DefaultExt=defaultExt;
            saveFileDialog.InitialDirectory=defaultDir;
            saveFileDialog.Filter=filter;
            saveFileDialog.OverwritePrompt=overwritePrompt;
            this.owner = owner;
        }

		~SaveFileDialogEx()
        {}
		

        /// <summary>
        /// Return full path and filename
        /// </summary>
        /// <returns>full path and filename</returns>
        public String GetPathName()
        {
            return saveFileDialog.FileName;
        }
		
        /// <summary>
        /// Return only filename
        /// </summary>
        /// <returns>filename</returns>
        public String GetFileName()
        {
            return FolderHelper.GetFileName(saveFileDialog.FileName);
        }


        /// <summary>
        /// Return only ext
        /// </summary>
        /// <returns>ext</returns>
        public String GetFileExt()
        {
            return FolderHelper.GetFileExtension(saveFileDialog.FileName);
        }

        /// <summary>
        /// Return file title
        /// </summary>
        /// <returns>file title</returns>
        public String GetFileTitle()
        {
            return FolderHelper.GetFileTitle(saveFileDialog.FileName);
        }

        /// <summary>
        /// Show dialog and return DialogResult
        /// </summary>
        /// <returns>DialogResult</returns>
        public virtual DialogResult ShowDialog()
        {
            if (owner != null)
                return saveFileDialog.ShowDialog(owner);
            return saveFileDialog.ShowDialog();
        }

	};
}
