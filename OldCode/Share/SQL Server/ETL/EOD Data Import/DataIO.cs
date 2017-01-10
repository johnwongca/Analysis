using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FTPLib;

namespace DataIO
{
    interface IFile
    {
        FInfo FileInfo { get; }
        FInfo Read();
        void ReadFileInfo();
        System.Collections.Generic.List<IFile> ReadFolder(bool includeBody);
        System.Collections.Generic.List<IFile> ReadFolder();
    }
    enum FileType { Folder, File, String }
    class FLine
    {
        public int LineID;
        public string Line;
        public FLine(int id, string s)
        {
            LineID = id; Line = s;
        }
    }
    class FLineColumn
    {
        public int LineID;
        public int ColumnID;
        public string Line;
        public FLineColumn(int id, int c, string s)
        {
            LineID = id; ColumnID = c; Line = s;
        }
    }
    class FInfo
    {
        private string body, delimiter;
        private ArrayList lines;
        private ArrayList table;
        private int tableColumns;
        public DateTime FileDate, LoadDate;
        public FileType Type;
        public long Size;
        public string Name;
        public string FileName
        {
            get
            {
                return Path.GetFileName(Name);
            }
        }
        public string Body
        {
            get
            {
                return body;
            }
            set
            {
                body = value;
                LoadDate = DateTime.Now;
                if (Type == FileType.String)
                {
                    Size = body.Length;
                    FileDate = LoadDate;
                }
                lines.Clear();
                ClearTable();
            }
        }
        public string Delimiter
        {
            get
            {
                return delimiter;
            }
            set
            {
                if (delimiter != value)
                {
                    delimiter = value;
                    ClearTable();
                }
            }
        }
        private void ClearTable()
        {
            table.Clear();
            tableColumns = 0;
        }
        public FInfo()
        {
            Name = ""; Type = FileType.File; body = ""; Size = -1; FileDate = DateTime.MinValue; LoadDate = DateTime.MinValue;
            lines = new ArrayList();
            table = new ArrayList();
        }
        public ArrayList Lines
        {
            get
            {
                if (lines.Count == 0)
                {
                    int i = 0;
                    StringReader r = new StringReader(body);
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        i++;
                        lines.Add(new FLine(i, line));
                    }
                }
                return lines;
            }
        }
        public ArrayList Table
        {
            get
            {
                if (table.Count == 0)
                {
                    ArrayList f = Lines;
                    int i = 0;
                    foreach (FLine item in f)
                    {
                        i = 0;
                        foreach (string s in (item.Line.Split(new string[] { Delimiter }, StringSplitOptions.None)))
                        {
                            i++;
                            table.Add(new FLineColumn(item.LineID, i, s));
                            if (tableColumns < i)
                                tableColumns = i;
                        }
                    }
                }
                return table;
            }
        }
        public int ColumnCount
        {
            get
            {
                return tableColumns;
            }
        }
        public override string ToString()
        {
            StringBuilder ss = new StringBuilder();
            if (Body != "")
            {
                ss.AppendLine("File Body: ");
                ss.AppendLine(Body);
            }
            ss.AppendLine("File Full Name: " + Name);
            ss.AppendLine("File Name: " + FileName);
            ss.AppendLine("File Type: " + Type.ToString());
            ss.AppendLine("File Size: " + Size.ToString());
            ss.AppendLine("File Date: " + FileDate.ToString());
            ss.AppendLine("File Loaded Date: " + LoadDate.ToString());
            ss.AppendLine();
            return ss.ToString();
        }

        public static bool IsFtp(string address)
        {
            return address.Trim().Substring(0, 3).ToUpper() == "FTP";
        }
        public static IFile CreateFile(string address)
        {
            if (IsFtp(address))
                return new FtpFile(address);
            return new SystemFile(address);
        }
        public static IFile CreateStringFile(string text)
        {
            return new StringFile(text);
        }
    }
    class FtpFile : IFile
    {
        FInfo fileInfo;
        //FtpWebRequest ftpReq = null;
        public FInfo FileInfo
        {
            get { return fileInfo; }
        }
        public FtpFile(string address)
        {
            fileInfo = new FInfo();
            fileInfo.Name = address;
            fileInfo.Size = -1;
            try
            {
                fileInfo.Type = FileType.File;

            }
            catch//(Exception e)
            {
                //Console.WriteLine(e);
                fileInfo.Type = FileType.Folder;
            }
            finally
            {
                fileInfo.Name = fileInfo.Name.Trim();
                if (fileInfo.Type == FileType.Folder)
                {
                    if (fileInfo.Name.Substring(fileInfo.Name.Length - 1, 1) != @"/")
                        fileInfo.Name = fileInfo.Name + @"/";
                }
            }
        }
        public void ReadFileInfo()
        {
            ReadFileSize();
            ReadFileDate();
        }
        private void ReadFileSize()
        {
            fileInfo.Size = FTPConnection.GetFile(fileInfo.Name).Current.Size;
            return;
        }
        private void ReadFileDate()
        {
            fileInfo.FileDate = FTPConnection.GetFile(fileInfo.Name).Current.Date;
            return;
        }
        public FInfo Read()
        {
            if (fileInfo.Type != FileType.File) return fileInfo;
            fileInfo.Body = FTPConnection.GetFile(fileInfo.Name).Current.DataString;
            return fileInfo;
        }
        public List<IFile> ReadFolder()
        {
            return ReadFolder(false);
        }
        private FtpFile ReadFolder_NewFile(string name)
        {
            FtpFile item = null;
            if (fileInfo.Name.Substring(fileInfo.Name.Length - 1, 1) != @"/")
                item = new FtpFile(fileInfo.Name + @"/" + name);
            else
                item = new FtpFile(fileInfo.Name + name);
            return item;
        }
        public List<IFile> ReadFolder(bool includeBody)
        {
            List<IFile> ret = new List<IFile>();
            FtpFile item = null;
            FTPSite c = FTPConnection.GetFile(fileInfo.Name);
            if (!c.Current.IsFolder)
            {
                item = ReadFolder_NewFile(c.Current.Name);
                if (includeBody) item.Read();
                ret.Add(item);
            }
            else
            {
                foreach (var a in c.Current.Folders)
                {
                    item = ReadFolder_NewFile(a.Name);
                    if (includeBody) item.Read();
                    ret.Add(item);
                }
                foreach (var a in c.Current.Files)
                {
                    item = ReadFolder_NewFile(a.Name);
                    if (includeBody) item.Read();
                    ret.Add(item);
                }
            }
            return ret;
        }
    }
    class SystemFile : IFile
    {
        FInfo fileInfo;
        public void ReadFileInfo()
        {
        }
        public FInfo FileInfo
        {
            get { return fileInfo; }
        }
        public SystemFile(string address)
        {
            fileInfo = new FInfo();
            fileInfo.Name = address;
            fileInfo.Size = -1;
            fileInfo.Type = (System.IO.File.GetAttributes(address) & FileAttributes.Directory) == FileAttributes.Directory ? FileType.Folder : FileType.File;
            if (fileInfo.Type == FileType.Folder)
            {
                fileInfo.Name = fileInfo.Name.Trim();
                if (fileInfo.Name.Substring(fileInfo.Name.Length - 1, 1) != @"\")
                    fileInfo.Name = fileInfo.Name + @"\";
            }
            else
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(fileInfo.Name);
                fileInfo.FileDate = fi.LastWriteTime;
                fileInfo.Size = fi.Length;
            }
        }
        public FInfo Read()
        {
            if (fileInfo.Type != FileType.File) return fileInfo;
            fileInfo.Body = System.IO.File.ReadAllText(fileInfo.Name);
            return fileInfo;
        }
        public List<IFile> ReadFolder()
        {
            return ReadFolder(false);
        }
        public List<IFile> ReadFolder(bool includeBody)
        {
            List<IFile> ret = new List<IFile>();
            if (fileInfo.Type == FileType.Folder)
            {
                DirectoryInfo di = new DirectoryInfo(fileInfo.Name);
                foreach (System.IO.FileSystemInfo fi in di.GetFileSystemInfos())
                {
                    SystemFile item = new SystemFile(fileInfo.Name + fi.Name);
                    if (includeBody) item.Read();
                    ret.Add(item);
                }
            }
            else
            {
                SystemFile item = new SystemFile(fileInfo.Name);
                if (includeBody) item.Read();
                ret.Add(item);
            }
            return ret;
        }
    }
    class StringFile : IFile
    {
        FInfo fileInfo;
        public void ReadFileInfo()
        {
        }
        public FInfo FileInfo
        {
            get { return fileInfo; }
        }
        public StringFile(string text)
        {
            fileInfo = new FInfo();
            fileInfo.Type = FileType.String;
            fileInfo.Body = text;
        }
        public FInfo Read()
        {
            return fileInfo;
        }
        public List<IFile> ReadFolder(bool includeBody)
        {
            return new List<IFile>();
        }
        public List<IFile> ReadFolder()
        {
            return new List<IFile>();
        }
    }
    static class TextFileBuffer
    {
        private static int counter = 0;
        public static int New(IFile file)
        {
            counter = Buffer.Count == 0 ? 0 : counter;
            counter++;
            Buffer.Add(counter, file);
            return counter;
        }
        public static IFile Set(int key, IFile value)
        {
            Buffer[key] = value;
            return value;
        }
        public static IFile Get(int key)
        {
            return Buffer[key];
        }
        public static void Remove(int key)
        {
            if (Buffer[key] != null) Buffer[key] = null;
            Buffer.Remove(key);
            GC.Collect();
        }
        public static Dictionary<Int32, IFile> Buffer = new Dictionary<int, IFile>();
    }
    static class FTPConnection
    {
        internal static List<FTPSite> Sites = new List<FTPSite>();
        public static FTPSite GetFile(string fullName)
        {
            return GetFile(new Uri(fullName));
        }
        public static FTPSite GetFile(Uri uri)
        {
            FTPSite p = Sites.Where(x => x.RequestUri.Host == uri.Host).FirstOrDefault();
            if (p == null)
            {
                p = new FTPSite(uri);
                p.Connect();
                Sites.Add(p);
            }
            p.ChangeCurrent(uri.Segments);
            return p;
        }
        public static void RemoveFile(string fullName)
        {
            RemoveFile(new Uri(fullName));
        }
        public static void RemoveFile(Uri uri)
        {
            FTPSite p = Sites.Where(x => x.RequestUri.Host == uri.Host).FirstOrDefault();
            if (p != null)
            {
                p.Disconnect();
                Sites.Remove(p);
            }
        }
        public static void RemoveAll()
        {
            foreach (var p in Sites) p.Disconnect();
            Sites.Clear();
        }
    }
}
