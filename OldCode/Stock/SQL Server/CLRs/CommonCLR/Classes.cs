using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace CommonCLR
{
	enum FileType { Folder, File, String }
	internal class FLine
	{
		public int LineID;
		public string Line;
		public FLine(int id, string s)
		{
			LineID = id; Line = s;
		}
	}
	internal class FLineColumn
	{
		public int LineID;
		public int ColumnID;
		public string Line;
		public FLineColumn(int id, int c, string s)
		{
			LineID = id; ColumnID = c; Line = s;
		}
	}
	internal class FInfo
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
					int i=0;
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
						foreach(string s in (item.Line.Split(new string[] { Delimiter }, StringSplitOptions.None)))
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
	internal class FtpFile : CommonCLR.IFile 
	{
		FInfo fileInfo;
		public FInfo FileInfo
		{
			get { return fileInfo; }
		}
		public FtpFile(string address)
		{
			fileInfo = new FInfo();
			fileInfo.Name = address;
			fileInfo.Size = -1;
			ServicePointManager.MaxServicePointIdleTime = 10000;
			ServicePointManager.MaxServicePoints = 1;
			try
			{
				ReadFileSize();
				fileInfo.Type = FileType.File;
				ReadFileDate();
			}
			catch
			{
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
		private void ReadFileSize()
		{
			if (fileInfo.Size != -1) return;
			FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(fileInfo.Name);
			req.KeepAlive = true;
			req.Method = WebRequestMethods.Ftp.GetFileSize;
			FtpWebResponse res = (FtpWebResponse)req.GetResponse();
			fileInfo.Size = res.ContentLength;
			res.Close();
			return;
		}
		private void ReadFileDate()
		{
			if (fileInfo.FileDate != DateTime.MinValue) return;
			FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(fileInfo.Name);
			req.KeepAlive = true;
			req.Method = WebRequestMethods.Ftp.GetDateTimestamp;
			FtpWebResponse res = (FtpWebResponse)req.GetResponse();
			fileInfo.FileDate = res.LastModified;
			res.Close();
			return;
		}
		public FInfo Read()
		{
			if (fileInfo.Type != FileType.File) return fileInfo;
			StreamReader r = null;
			try
			{
				FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(fileInfo.Name);
				req.KeepAlive = true;
				req.Method = WebRequestMethods.Ftp.DownloadFile;
				FtpWebResponse res = (FtpWebResponse)req.GetResponse();
				r = new StreamReader(res.GetResponseStream());
				fileInfo.Body = r.ReadToEnd();
				res.Close();
			}
			finally
			{
				if (r != null)
					r.Close();
			}
			return fileInfo;
		}
		public List<IFile> ReadFolder()
		{
			return ReadFolder(false);
		}
		public List<IFile> ReadFolder(bool includeBody)
		{
			List<IFile> ret = new List<IFile>();
			StreamReader r = null;
			FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(fileInfo.Name);
			req.KeepAlive = true;
			req.Method = WebRequestMethods.Ftp.ListDirectory;
			try
			{
				FtpWebResponse res = (FtpWebResponse)req.GetResponse();
				r = new StreamReader(res.GetResponseStream());
				string s = r.ReadLine();
				while (s != null)
				{
					FtpFile item = new FtpFile(fileInfo.Name+s);
					if (includeBody) item.Read();
					ret.Add(item);
					s = r.ReadLine();
				}
				res.Close();
			}
			finally
			{
				if (r != null)
					r.Close();
			}
			return ret;
		}
	}
	internal class SystemFile : CommonCLR.IFile
	{
		FInfo fileInfo;
		public FInfo FileInfo
		{
			get { return fileInfo; }
		}
		public SystemFile(string address)
		{
			fileInfo = new FInfo();
			fileInfo.Name = address;
			fileInfo.Size = -1;
			fileInfo.Type = (System.IO.File.GetAttributes(address) & FileAttributes.Directory) == FileAttributes.Directory ? FileType.Folder: FileType.File;
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
	internal class StringFile : CommonCLR.IFile
	{
		FInfo fileInfo;
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
	internal static class TextFileBuffer
	{
		private static int counter= 0;
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
		public static Dictionary<Int32, IFile> Buffer = new Dictionary<int,IFile>();
	}
}
