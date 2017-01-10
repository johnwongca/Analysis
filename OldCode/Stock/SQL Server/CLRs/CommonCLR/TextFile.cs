using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

namespace CommonCLR
{
	public class Common
	{
	}
	public class TextFile
	{
		
		[SqlFunction(Name = "TextFile_OpenFile")]
		public static SqlInt32 OpenFile(SqlString file)
		{
			int ret = TextFileBuffer.New(FInfo.CreateFile(file.ToString()));
			return ret;
		}

		[SqlFunction(Name = "TextFile_OpenString")]
		public static SqlInt32 OpenString(SqlString str)
		{
			int ret = TextFileBuffer.New(FInfo.CreateStringFile(str.ToString()));
			return ret;
		}
		
		[SqlFunction(Name = "TextFile_CloseFile")]
		public static SqlBoolean CloseFile(SqlInt32 handle)
		{
			TextFileBuffer.Remove(handle.Value);
			return SqlBoolean.True;
		}

		[
			SqlFunction(
				Name = "TextFile_ListHandles",
				FillRowMethodName = "ListHandlesRow",
				TableDefinition = "[Handle] int, [FileName] nvarchar(1024), [Size] bigint, [Type] nvarchar(10), [FileDate] datetime, [LoadDate] datetime"
				)
		]
		public static IEnumerable ListHandles()
		{
			return TextFileBuffer.Buffer;
		}
		public static void ListHandlesRow(Object obj, out SqlInt32 Handle, out SqlString FileName, out SqlInt64 Size, out SqlString Type, out SqlDateTime FileDate, out SqlDateTime LoadDate)
		{
			KeyValuePair<Int32, IFile> i = (KeyValuePair<Int32, IFile>)obj;
			Handle = i.Key;
			FileName = i.Value.FileInfo.Name;
			Size = i.Value.FileInfo.Size;
			Type = i.Value.FileInfo.Type.ToString();
			FileDate = i.Value.FileInfo.FileDate == DateTime.MinValue ? SqlDateTime.Null : i.Value.FileInfo.FileDate;
			LoadDate = i.Value.FileInfo.LoadDate == DateTime.MinValue ? SqlDateTime.Null : i.Value.FileInfo.LoadDate;
		}

		[SqlFunction(Name = "TextFile_SetDelimiter")]
		public static SqlBoolean SetDelimiter(SqlInt32 handle, SqlString delimiter)
		{
			TextFileBuffer.Get(handle.Value).FileInfo.Delimiter = delimiter.ToString();
			return SqlBoolean.True;
		}
		
		[SqlFunction(Name = "TextFile_GetText")]
		public static SqlString GetText(SqlInt32 handle)
		{
			IFile f = TextFileBuffer.Get(handle.Value);
			if (f.FileInfo.Body == "")
				f.Read();
			return f.FileInfo.Body;
		}
		
		[
			SqlFunction(
				Name = "TextFile_GetLines",
				DataAccess = DataAccessKind.Read, 
				FillRowMethodName = "LineTableRow",
				TableDefinition = "[LineID] int, [Value] NVARCHAR(MAX)"
				)
		]
		public static IEnumerable LineTable(SqlInt32 handle)
		{
			return TextFileBuffer.Get(handle.Value).FileInfo.Lines;
		}
		public static void LineTableRow(Object obj, out SqlInt32 LineID, out SqlString Value)
		{
			
			LineID = ((FLine)obj).LineID;
			Value = ((FLine)obj).Line;
		}
		
		[
			SqlFunction(
				Name = "TextFile_GetColumns",
				DataAccess = DataAccessKind.Read,
				FillRowMethodName = "ColumnTableRow",
				TableDefinition = "[LineID] int, [ColumnID] int, [Value] nvarchar(max)"
				)
		]
		public static IEnumerable ColumnTable(SqlInt32 handle)
		{
			return TextFileBuffer.Get(handle.Value).FileInfo.Table;
		}
		public static void ColumnTableRow(Object obj, out SqlInt32 LineID, out SqlInt32 ColumnID, out SqlString Value)
		{
			LineID = ((FLineColumn) obj).LineID;
			ColumnID = ((FLineColumn)obj).ColumnID;
			Value = ((FLineColumn)obj).Line;
		}

		[
			SqlFunction(
				Name = "TextFile_GetFileInfo",
				DataAccess = DataAccessKind.Read,
				FillRowMethodName = "InfoRow",
				TableDefinition = "[LineID] int, [Name] nvarchar(30), [Value] nvarchar(max)"
				)
		]
		public static IEnumerable Info(SqlInt32 handle)
		{
			FInfo f = TextFileBuffer.Get(handle.Value).FileInfo;
			List<string[]> ret = new List<string[]>();
			ret.Add(new string[]{"1", "Name", f.Name });
			ret.Add(new string[]{"2", "File Name", f.FileName });
			ret.Add(new string[]{"3", "Size", f.Size.ToString() });
			ret.Add(new string[]{"4", "Type", f.Type.ToString() });
			ret.Add(new string[]{"5", "File Date", f.FileDate.ToString("yyyy-MM-dd HH:mm:ss") } );
			ret.Add(new string[]{"6", "Loaded Date", f.LoadDate.ToString("yyyy-MM-dd HH:mm:ss") });
			ret.Add(new string[] { "7", "body", f.Body });
			return ret;
		}
		public static void InfoRow(Object obj, out SqlInt32 LineID, out SqlString Name, out SqlString Value)
		{
			string[] s = (string[])obj;
			LineID = Int32.Parse(s[0]);
			Name = s[1];
			Value = s[2];
		}

		[
			SqlFunction(
				Name = "TextFile_GetDirectory",
				DataAccess = DataAccessKind.Read,
				FillRowMethodName = "ReadDirectoryRow",
				TableDefinition = "[FullName] nvarchar(max), [FileName] nvarchar(256), [Size] bigint, [Type] nvarchar(10), [FileDate] datetime"
				)
		]
		public static IEnumerable ReadDirectory(SqlString path)
		{
			IFile handle = FInfo.CreateFile(path.Value);
			List<IFile> ret = handle.ReadFolder();
			return ret;
		}
		public static void ReadDirectoryRow(Object obj, out SqlString FullName, out SqlString FileName, out SqlInt64 Size, out SqlString Type, out SqlDateTime FileDate)
		{
			FullName = ((IFile)obj).FileInfo.Name;
			FileName = ((IFile)obj).FileInfo.FileName;
			Size = ((IFile)obj).FileInfo.Size;
			Type = ((IFile)obj).FileInfo.Type.ToString();
			FileDate = ((IFile)obj).FileInfo.FileDate == DateTime.MinValue ? SqlDateTime.Null : ((IFile)obj).FileInfo.FileDate;
		}
		[SqlProcedure(Name = "TextFile_GetTable")]
		public static void GetTable(SqlInt32 handle, SqlInt32 Columns)
		{
			IFile f = TextFileBuffer.Get(handle.Value);
			int cols = Columns.Value;
			if(f.FileInfo.Body=="")
				f.Read();
			ArrayList t = f.FileInfo.Table;
			
			ArrayList metaData = new ArrayList();
			metaData.Add(new SqlMetaData("LineID", SqlDbType.Int));
			for (int i = 0; i < cols; i++)
				metaData.Add(new SqlMetaData("Column"+(i+1).ToString(), SqlDbType.VarChar,8000));
			SqlDataRecord rec = new SqlDataRecord((SqlMetaData[])metaData.ToArray(typeof(SqlMetaData)));
			SqlContext.Pipe.SendResultsStart(rec);
			if (f.FileInfo.Table.Count > 0)
			{
				foreach (FLineColumn c in f.FileInfo.Table)
				{
					if (cols >= c.ColumnID)
					{
						if (c.ColumnID == 1 && c.LineID > 1)
						{
							SqlContext.Pipe.SendResultsRow(rec);
						}
						rec.SetValue(0, c.LineID);
						rec.SetValue(c.ColumnID, c.Line);
					}
				}
				SqlContext.Pipe.SendResultsRow(rec);
			}
			SqlContext.Pipe.SendResultsEnd();
			return;
		}

	};

}