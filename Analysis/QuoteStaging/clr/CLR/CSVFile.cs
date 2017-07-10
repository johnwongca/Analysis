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

using Microsoft.SqlServer.Server;

namespace etl.sqlnotes.info
{
    public class SCVFileOutput
    {
        public long RowID;
        public List<string> Data;
    }
    public class CSVFile
    {
        private static List<SCVFileOutput> ReadCSVInternal1(string FileName, string Separator)
        {
            long rowid = 0;
            ParserCSV parser = new ParserCSV() { Delimiter = Separator.ToCharArray()[0] };
            bool allEmpty = false;
            List<SCVFileOutput> ret = new List<SCVFileOutput>();
            using (var fileReader = new StreamReader(FileName))
            {
                while (true)
                {
                    rowid++;
                    List<string> data = parser.ReadLine(fileReader);
                    if (data == null)
                        break;
                    allEmpty = true;
                    data = data.Select(
                                x =>
                                {
                                    x = x.Trim();
                                    if (!string.IsNullOrEmpty(x))
                                        allEmpty = false;
                                    return x;
                                }).ToList<string>();
                    if (allEmpty)
                        continue;
                   ret.Add(new SCVFileOutput() { RowID = rowid, Data = data });

                }
            }
            return ret;
        }
        private static IEnumerable ReadCSVInternal(string FileName, string Separator)
        {
            long rowid = 0; 
            ParserCSV parser = new ParserCSV() { Delimiter = Separator.ToCharArray()[0] };
            bool allEmpty = false;
            using (var fileReader = new StreamReader(FileName))
            {
                while(true)
                {
                    rowid++;
                    List<string> data = parser.ReadLine(fileReader);
                    if (data == null)
                        break;
                    allEmpty = true;
                    data = data.Select(
                                x =>
                                {
                                    x = x.Trim();
                                    if (!string.IsNullOrEmpty(x))
                                        allEmpty = false;
                                    return x;
                                }).ToList<string>();
                    if (allEmpty)
                        continue;
                    yield return new SCVFileOutput() { RowID = rowid, Data = data };
                    
                }
            }
        }
        [SqlFunction(FillRowMethodName = "ReadCSV_FillRow", TableDefinition = "RowID bigint, C1 nvarchar(max), C2 nvarchar(max), C3 nvarchar(max), C4 nvarchar(max), C5 nvarchar(max), C6 nvarchar(max), C7 nvarchar(max), C8 nvarchar(max), C9 nvarchar(max), C10 nvarchar(max), C11 nvarchar(max), C12 nvarchar(max), C13 nvarchar(max), C14 nvarchar(max), C15 nvarchar(max), C16 nvarchar(max)")]
        public static IEnumerable ReadCSV(SqlString FileName, SqlString Separator)
        {
            if (!File.Exists(FileName.Value))
                return null;
            else
                return ReadCSVInternal(FileName.Value, Separator.Value);
        }
        public static void ReadCSV_FillRow(object o, out SqlInt64 RowID, out SqlString C1, out SqlString C2, out SqlString C3, out SqlString C4, out SqlString C5, out SqlString C6, out SqlString C7, out SqlString C8, out SqlString C9, out SqlString C10, out SqlString C11, out SqlString C12, out SqlString C13, out SqlString C14, out SqlString C15, out SqlString C16)
        {
            SCVFileOutput csv = (SCVFileOutput)o;
            RowID = new SqlInt64(csv.RowID);
            int c = csv.Data.Count;
            C1 = c < 1 ? SqlString.Null : string.IsNullOrEmpty(csv.Data[0]) ? SqlString.Null: new SqlString(csv.Data[0]);
            C2 = c < 2 ? SqlString.Null : string.IsNullOrEmpty(csv.Data[1]) ? SqlString.Null : new SqlString(csv.Data[1]);
            C3 = c < 3 ? SqlString.Null : string.IsNullOrEmpty(csv.Data[2]) ? SqlString.Null : new SqlString(csv.Data[2]);
            C4 = c < 4 ? SqlString.Null : string.IsNullOrEmpty(csv.Data[3]) ? SqlString.Null : new SqlString(csv.Data[3]);
            C5 = c < 5 ? SqlString.Null : string.IsNullOrEmpty(csv.Data[4]) ? SqlString.Null : new SqlString(csv.Data[4]);
            C6 = c < 6 ? SqlString.Null : string.IsNullOrEmpty(csv.Data[5]) ? SqlString.Null : new SqlString(csv.Data[5]);
            C7 = c < 7 ? SqlString.Null : string.IsNullOrEmpty(csv.Data[6]) ? SqlString.Null : new SqlString(csv.Data[6]);
            C8 = c < 8 ? SqlString.Null : string.IsNullOrEmpty(csv.Data[7]) ? SqlString.Null : new SqlString(csv.Data[7]);
            C9 = c < 9 ? SqlString.Null : string.IsNullOrEmpty(csv.Data[8]) ? SqlString.Null : new SqlString(csv.Data[8]);
            C10 = c < 10 ? SqlString.Null : string.IsNullOrEmpty(csv.Data[9]) ? SqlString.Null : new SqlString(csv.Data[9]);
            C11 = c < 11 ? SqlString.Null : string.IsNullOrEmpty(csv.Data[10]) ? SqlString.Null : new SqlString(csv.Data[10]);
            C12 = c < 12 ? SqlString.Null : string.IsNullOrEmpty(csv.Data[11]) ? SqlString.Null : new SqlString(csv.Data[11]);
            C13 = c < 13 ? SqlString.Null : string.IsNullOrEmpty(csv.Data[12]) ? SqlString.Null : new SqlString(csv.Data[12]);
            C14 = c < 14 ? SqlString.Null : string.IsNullOrEmpty(csv.Data[13]) ? SqlString.Null : new SqlString(csv.Data[13]);
            C15 = c < 15 ? SqlString.Null : string.IsNullOrEmpty(csv.Data[14]) ? SqlString.Null : new SqlString(csv.Data[14]);
            C16 = c < 16 ? SqlString.Null : string.IsNullOrEmpty(csv.Data[15]) ? SqlString.Null : new SqlString(csv.Data[15]);

        }
        
    }
}
