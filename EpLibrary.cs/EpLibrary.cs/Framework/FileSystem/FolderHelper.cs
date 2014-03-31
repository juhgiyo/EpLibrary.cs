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
    
    public class FolderHelper
    {
        public const int MAX_PATH = 260;

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

        public static bool IsPathExist(String path)
        {
            return Directory.Exists(path);
        }

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

        public static String GetModuleFileName()
        {
            StringBuilder exePath = new StringBuilder(FolderHelper.MAX_PATH);
            int exePathLen = GetModuleFileName((IntPtr)0, exePath, exePath.Capacity);
            return exePath.ToString();
        }
        
        public static String GetModuleFileDirectory()
        {
            String retString = GetModuleFileName();
            return GetPathOnly(retString);
        }
    }




    /*! 
	@class OpenFileDialog epFolderHelper.h
	@brief This is a class for Open File Dialog

	Implements the Open File Dialog.
	*/
	public class OpenFileDialogEx{
	private OpenFileDialog openFileDialog=new OpenFileDialog();
    private IWin32Window owner = null;
		/*!
		Default Constructor

		Initializes the Open File Dialog
		@param[in] title the title of the dialog
		@param[in] defaultExt The default extension
		@param[in] defaultDir The default directory
		@param[in] filter The file extension filter
		@param[in] pParentWnd The parent window
		*/
		public OpenFileDialogEx(String title=null, String defaultExt=null,String defaultDir=null,String filter=null, IWin32Window owner=null)
        {
            openFileDialog.Title=title;
            openFileDialog.DefaultExt=defaultExt;
            openFileDialog.InitialDirectory=defaultDir;
            openFileDialog.Filter=filter;
            openFileDialog.Multiselect=false;
            this.owner = owner;

        }

		/*!
		Default Destructor

		Destroy the Open File Dialog
		*/
		~OpenFileDialogEx()
        {
        }

		/*! 
		Return full path and filename
		@return full path and filename
		*/
		public String GetPathName()
        {
            return openFileDialog.FileName;
        }
		
		/*! 
		Return only filename
		@return filename
		*/
		public String GetFileName()
        {
            return FolderHelper.GetFileName(openFileDialog.FileName);
        }

		public String GetFileExt()
        {
            return FolderHelper.GetFileExtension(openFileDialog.FileName);
        }

		/*!
		Return file title
		@return file title
		*/
		public String GetFileTitle()
        {
            return FolderHelper.GetFileTitle(openFileDialog.FileName);
        }

		/*! 
		Do modal and return Modal result
		@return Modal result
		*/
        public virtual DialogResult ShowDialog()
        {
            if(owner!=null)
                return openFileDialog.ShowDialog(owner);
            return openFileDialog.ShowDialog();
        }
	}


	/*! 
	@class OpenMultiFileDialog epFolderHelper.h
	@brief This is a class for Open Multi-File Dialog

	Implements the Open Multi-File Dialog.
	*/
	public class OpenMultiFileDialog{
		private OpenFileDialog openFileDialog=new OpenFileDialog();
        private IWin32Window owner = null;
		/*!
		Default Constructor

		Initializes the Open Multi-File Dialog
		@param[in] title the title of the dialog
		@param[in] defaultExt The default extension
		@param[in] defaultDir The default directory
		@param[in] filter The file extension filter
		*/
		public OpenMultiFileDialog(String title=null, String defaultExt=null,String defaultDir=null,String filter=null, IWin32Window owner=null)
        {
            openFileDialog.Title=title;
            openFileDialog.DefaultExt=defaultExt;
            openFileDialog.InitialDirectory=defaultDir;
            openFileDialog.Filter=filter;
            openFileDialog.Multiselect=true;
            this.owner = owner;
        }
		/*!
		Default Destructor

		Destroy the Open Multi-File Dialog
		*/
		~OpenMultiFileDialog()
        {
        }

		/*! 
		Return next path name
		@return the next path name
		@remark when this function is first called, it returns the first path name.
		*/
		public String[] GetFileNames()
        {
            return openFileDialog.FileNames;
        }

		/*! 
		Do modal and return Modal result
		@return Modal result
		*/
        public virtual DialogResult ShowDialog()
        {
            if (owner != null)
                return openFileDialog.ShowDialog(owner);
            return openFileDialog.ShowDialog();
        }
	}

	/*! 
	@class OpenFolderDialog epFolderHelper.h
	@brief This is a class for Open Folder Dialog

	Implements the Open Folder Dialog.
	*/
	public class OpenFolderDialog{
	private FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
    private IWin32Window owner = null;
		/*!
		Default Constructor

		Initializes the Open Folder Dialog
		@param[in] hParent the parent window's handle
		@param[in] title the title of the dialog
		*/
		public OpenFolderDialog(String title=null, bool showNewFolderButton=true, IWin32Window owner=null)
        {
              
            folderBrowserDialog.Description = title;
            folderBrowserDialog.ShowNewFolderButton = showNewFolderButton;
            this.owner = owner;
        }

		/*!
		Default Destructor

		Destroy the Open Folder Dialog
		*/
		~OpenFolderDialog()
        {}

		/*! 
		Return full directory path
		@return full directory path
		*/
		public String GetPathName() 
        {
            return folderBrowserDialog.SelectedPath;
        }

		/*! 
		Do modal and return Modal result
		@return Modal result
		*/
        public virtual DialogResult ShowDialog()
        {
            if (owner != null)
                return folderBrowserDialog.ShowDialog(owner);
            return folderBrowserDialog.ShowDialog();
        }
	}

	/*! 
	@class SaveFileDialog epFolderHelper.h
	@brief This is a class for Save File Dialog

	Implements the Save File Dialog.
	*/
	public class SaveFileDialogEx{
	    private SaveFileDialog saveFileDialog=new SaveFileDialog();
        private IWin32Window owner = null;
		/*!
		Default Constructor

		Initializes the Save File Dialog
		@param[in] defaultExt The default extension
		@param[in] defaultDir The default directory
		@param[in] filter The file extension filter
		@param[in] pParentWnd The parent window
		*/
        public SaveFileDialogEx(String title = null, String defaultExt = null, String defaultDir = null, String filter = null, bool overwritePrompt = true, IWin32Window owner = null)
        {
            saveFileDialog.Title=title;
            saveFileDialog.DefaultExt=defaultExt;
            saveFileDialog.InitialDirectory=defaultDir;
            saveFileDialog.Filter=filter;
            saveFileDialog.OverwritePrompt=overwritePrompt;
            this.owner = owner;
        }
		/*!
		Default Destructor

		Destroy the Save File Dialog
		*/
		~SaveFileDialogEx()
        {}
		
		/*!
		Return full path and filename
		@return full path and filename
		*/
        public String GetPathName()
        {
            return saveFileDialog.FileName;
        }
		
		/*!
		Return only filename
		@return filename
		*/
        public String GetFileName()
        {
            return FolderHelper.GetFileName(saveFileDialog.FileName);
        }

		/*! 
		Return only ext
		@return ext
		*/
        public String GetFileExt()
        {
            return FolderHelper.GetFileExtension(saveFileDialog.FileName);
        }

		/*! 
		Return file title
		@return file title
		*/
        public String GetFileTitle()
        {
            return FolderHelper.GetFileTitle(saveFileDialog.FileName);
        }

		/*! 
		Do modal and return Modal result
		@return Modal result
		*/
        public virtual DialogResult ShowDialog()
        {
            if (owner != null)
                return saveFileDialog.ShowDialog(owner);
            return saveFileDialog.ShowDialog();
        }

	};
}
