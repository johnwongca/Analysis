using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETL
{
    public enum FileType { Folder = 1, File = 2, String= 4, Unknown = -1 }
    public class TextFile
    {
        #region Private Fields
        private string mContent = null;
        private long mSize = -1;
        private DateTime mFileDate = DateTime.Today;
        private string mFileName = "";
        private FileType mFileType = FileType.Unknown;
        private List<TextFile> mFiles = null;
        #endregion

        #region Public Properties
        public string Content 
        {
            get { return mContent; }
        }
        public DateTime FileDate
        {
            get { return mFileDate; }
        }
        public long Size { get { return mSize; } }
        public string FileName 
        { 
            get { return mFileName; }
            set { mFileName = value; }
        }
        public FileType FileType { get { return mFileType; }}
        public List<TextFile> Files { get { return mFiles; } }
        #endregion
    }
}
