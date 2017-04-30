using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

using Microsoft.SqlServer.Server;
namespace etl.sqlnotes.info
{
    public class FileListStructure
    {
        public string FileName;
        public bool IsFile;
        public DateTime CreationDate;
        public DateTime LastModifiedDate;
        public long Length;
    }
    public class Drive
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetDiskFreeSpaceEx(string lpDirectoryName, out ulong lpFreeBytesAvailable, out ulong lpTotalNumberOfBytes, out ulong lpTotalNumberOfFreeBytes);

        [SqlFunction(FillRowMethodName = "GetDiskFreeSpace_FillRow", TableDefinition = "TotalSize bigint, AvailableFreeSpace bigint, TotalFreeSpace bigint")]
        public static IEnumerable GetDiskFreeSpace(SqlString Path)
        {
            List<ulong[]> ret = new List<ulong[]>();
            if (Path.IsNull)
                return ret;
            ulong FreeBytesAvailable;
            ulong TotalNumberOfBytes;
            ulong TotalNumberOfFreeBytes;
            bool success = GetDiskFreeSpaceEx(Path.Value, out FreeBytesAvailable, out TotalNumberOfBytes, out TotalNumberOfFreeBytes);
            if (!success)
                return ret;
            ret.Add(new ulong[] { TotalNumberOfBytes, FreeBytesAvailable, TotalNumberOfFreeBytes });
            return ret;
        }
        public static void GetDiskFreeSpace_FillRow(object o, out SqlInt64 TotalSize, out SqlInt64 AvailableFreeSpace, out SqlInt64 TotalFreeSpace)
        {
            ulong[] d = (ulong[])o;
            TotalSize = new SqlInt64((long)(d[0]));
            AvailableFreeSpace = new SqlInt64((long)(d[1]));
            TotalFreeSpace = new SqlInt64((long)(d[2]));
        }
        [SqlFunction(FillRowMethodName = "ListDrive_FillRow", TableDefinition = "Name nvarchar(4000), DriveType nvarchar(200), Format nvarchar(200), IsReady bit, Label nvarchar(4000), RootDirectory nvarchar(4000), TotalSize bigint, AvailableFreeSpace bigint, TotalFreeSpace bigint")]
        public static IEnumerable ListDrive()
        {
            return DriveInfo.GetDrives();
        }
        public static void ListDrive_FillRow(object o, out SqlString Name, out SqlString DriveType, out SqlString Format, out SqlBoolean IsReady, out SqlString Label, out SqlString RootDirectory, out SqlInt64 TotalSize, out SqlInt64 AvailableFreeSpace, out SqlInt64 TotalFreeSpace)
        {
            DriveInfo d = (DriveInfo)o;
            Name = new SqlString(d.Name);
            DriveType = new SqlString(d.DriveType.ToString());
            Format = new SqlString(d.DriveFormat);
            IsReady = new SqlBoolean(d.IsReady);
            Label = new SqlString(d.VolumeLabel);
            RootDirectory = new SqlString(d.RootDirectory.FullName);
            TotalSize = new SqlInt64(d.TotalSize);
            AvailableFreeSpace = new SqlInt64(d.AvailableFreeSpace);
            TotalFreeSpace = new SqlInt64(d.TotalFreeSpace);
        }
    }
    public class FileAccess
    {
        [SqlFunction]
        public static SqlInt32 WriteBinaryToFile(SqlString FileName, SqlBytes Content)
        {
            using (BinaryWriter writer = new BinaryWriter(System.IO.File.Open(FileName.Value, FileMode.CreateNew)))
            {
                if (Content.IsNull)
                    return SqlInt32.Null;
                writer.Write(Content.Value);
                return new SqlInt32(Content.Value.Length);
            }
        }
        [SqlFunction(DataAccess = DataAccessKind.Read, SystemDataAccess = SystemDataAccessKind.Read)]
        public static SqlInt32 WriteBinaryResultToFile(SqlString FileName, SqlChars SQL)
        {
            int fileCount = 0;
            using (BinaryWriter writer = new BinaryWriter(System.IO.File.Open(FileName.Value, FileMode.CreateNew)))
            {
                if (SQL.IsNull)
                    return SqlInt32.Null;
                using (SqlConnection connection = new SqlConnection("context connection=true"))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandTimeout = 0;
                        command.CommandType = CommandType.Text;
                        command.CommandText = new string(SQL.Value);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader == null)
                                return new SqlInt32(-1);
                            do
                            {
                                while (reader.Read())
                                {
                                    writer.Write((byte[])reader.GetValue(0));
                                    fileCount++;
                                }
                            } while (reader.NextResult());
                            return new SqlInt32(fileCount);
                        }
                    }
                }
            }
        }
        [SqlFunction]
        public static SqlInt32 MoveFile(SqlString SourceFileName, SqlString TargetFileName)
        {
            System.IO.File.Move(SourceFileName.Value, TargetFileName.Value);
            return SqlInt32.Null;
        }
        [SqlFunction]
        public static SqlInt32 CopyFile(SqlString SourceFileName, SqlString TargetFileName)
        {
            System.IO.File.Copy(SourceFileName.Value, TargetFileName.Value, true);
            return SqlInt32.Null;
        }
        [SqlFunction]
        public static SqlInt32 DeleteFile(SqlString FileName)
        {
            System.IO.File.Delete(FileName.Value);
            return SqlInt32.Null;
        }
        [SqlFunction]
        public static SqlBytes GetFileContent(SqlString FileName)
        {
            if (FileName.IsNull)
                return SqlBytes.Null;
            try
            {
                return new SqlBytes(File.ReadAllBytes(FileName.Value));
            }
            catch
            {
                return SqlBytes.Null;
            }
        }
        [SqlFunction(FillRowMethodName = "ListFile_FillRow", TableDefinition = "FullFileName nvarchar(4000), FileName nvarchar(max), IsFile bit, CreationDate datetime, LastModifiedDate datetime, Length bigint")]
        public static IEnumerable ListFile(SqlString Path)
        {
            List<FileListStructure> list = new List<FileListStructure>();
            if (!Path.IsNull)
            {
                DirectoryInfo di = new DirectoryInfo(Path.Value);

                foreach (DirectoryInfo d in di.GetDirectories())
                {
                    //list.Add(new FileListStructure() { FileName = d.FullName, IsFile = false, CreationDate = d.CreationTime, LastModifiedDate = d.LastWriteTime, Length = 0 });
                    yield return new FileListStructure() { FileName = d.FullName, IsFile = false, CreationDate = d.CreationTime, LastModifiedDate = d.LastWriteTime, Length = 0 };
                }
                foreach (FileInfo f in di.GetFiles())
                {
                    //list.Add(new FileListStructure() { FileName = f.FullName, IsFile = true, CreationDate = f.CreationTime, LastModifiedDate = f.LastWriteTime, Length = f.Length });
                    yield return new FileListStructure() { FileName = f.FullName, IsFile = true, CreationDate = f.CreationTime, LastModifiedDate = f.LastWriteTime, Length = f.Length };
                }
                /*foreach (string str in Directory.GetDirectories(Path.Value))
                {
                    list.Add(new FileListStructure() { FileName = str, IsFile = false });
                }
                foreach (string str2 in Directory.GetFiles(Path.Value))
                {
                    list.Add(new FileListStructure() { FileName = str2, IsFile = true });
                }*/
            }
            //return list;
        }
        public static void ListFile_FillRow(object o, out SqlString FullFileName, out SqlString FileName, out SqlBoolean IsFile, out SqlDateTime CreationDate, out SqlDateTime LastModifiedDate, out SqlInt64 Length)
        {
            FileListStructure _structure = (FileListStructure)o;
            FullFileName = new SqlString(_structure.FileName);
            IsFile = new SqlBoolean(_structure.IsFile);
            FileName = new SqlString(Path.GetFileName(_structure.FileName));
            CreationDate = new SqlDateTime(_structure.CreationDate);
            LastModifiedDate = new SqlDateTime(_structure.LastModifiedDate);
            Length = new SqlInt64(_structure.Length);
            /*if (_structure.IsFile)
            {
                CreationDate = new SqlDateTime(File.GetCreationTime(_structure.FileName));
                LastModifiedDate = new SqlDateTime(File.GetLastWriteTime(_structure.FileName));
                Length = new SqlInt64((new FileInfo(_structure.FileName)).Length);
            }
            else
            {
                CreationDate = new SqlDateTime(Directory.GetCreationTime(_structure.FileName));
                LastModifiedDate = new SqlDateTime(Directory.GetLastWriteTime(_structure.FileName));
                Length = new SqlInt64(-1);
            }*/
        }
        [SqlFunction]
        public static SqlBoolean FileExists(SqlString FileName)
        {
            return new SqlBoolean(System.IO.File.Exists(FileName.Value));
        }
    }
    public class DirectoryAccess
    {
        [SqlFunction]
        public static SqlInt32 CreateDirectory(SqlString DirectoryName)
        {
            System.IO.Directory.CreateDirectory(DirectoryName.Value);
            return SqlInt32.Null;
        }
        [SqlFunction]
        public static SqlInt32 RemoveDirectory(SqlString DirectoryName, SqlBoolean Recursive)
        {
            System.IO.Directory.Delete(DirectoryName.Value, Recursive.Value);
            return SqlInt32.Null;
        }
        [SqlFunction]
        public static SqlBoolean DirectoryExists(SqlString DirectoryName)
        {
            return new SqlBoolean(System.IO.Directory.Exists(DirectoryName.Value));
        }
        [SqlFunction]
        public static SqlInt32 MoveDirectory(SqlString SourceDirectoryName, SqlString TargetDirectoryName)
        {
            System.IO.Directory.Move(SourceDirectoryName.Value, TargetDirectoryName.Value);
            return SqlInt32.Null;
        }
    }
    public class PathOperation
    {
        [SqlFunction]
        public static SqlString PathChangeExtension(SqlString Path, SqlString Extension)
        {
            return new SqlString(System.IO.Path.ChangeExtension(Path.Value, Extension.Value));
        }
        [SqlFunction]
        public static SqlString PathGetDirectoryName(SqlString Path)
        {
            return new SqlString(System.IO.Path.GetDirectoryName(Path.Value));
        }
        [SqlFunction]
        public static SqlString PathGetExtensionName(SqlString Path)
        {
            return new SqlString(System.IO.Path.GetExtension(Path.Value));
        }
        [SqlFunction]
        public static SqlString PathGetFileName(SqlString Path)
        {
            return new SqlString(System.IO.Path.GetFileName(Path.Value));
        }
        [SqlFunction]
        public static SqlString PathGetFileNameWithoutExtension(SqlString Path)
        {
            return new SqlString(System.IO.Path.GetFileNameWithoutExtension(Path.Value));
        }
        [SqlFunction]
        public static SqlString PathGetRoot(SqlString Path)
        {
            return new SqlString(System.IO.Path.GetPathRoot(Path.Value));
        }
        [SqlFunction]
        public static SqlString PathGetTempFileName()
        {
            return new SqlString(System.IO.Path.GetTempFileName());
        }
        [SqlFunction]
        public static SqlString PathGetTempPath()
        {
            return new SqlString(System.IO.Path.GetTempPath());
        }
        [SqlFunction]
        public static SqlBoolean PathHasExtension(SqlString Path)
        {
            return new SqlBoolean(System.IO.Path.HasExtension(Path.Value));
        }
        [SqlFunction]
        public static SqlBoolean PathIsPathRooted(SqlString Path)
        {
            return new SqlBoolean(System.IO.Path.IsPathRooted(Path.Value));
        }
        [SqlFunction]
        public static SqlString PathCombine(SqlString Path1, SqlString Path2)
        {
            return new SqlString(System.IO.Path.Combine(Path1.Value, Path2.Value));
        }
    }
}