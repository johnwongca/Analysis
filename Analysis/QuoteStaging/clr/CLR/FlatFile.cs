using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

using Microsoft.SqlServer.Server;

namespace etl.sqlnotes.info
{
    public enum FlatFileType { CSV, Delimitered, FixedLength, Line, PeopleSoftCSV84 };
    public class FlatFile : ADataReader
    {
        public string Content;
        public bool FirstRowContainColumnName = false, ContentIsPath = true, IncludeRowID = true;
        public int SkipTopLines = 0, SkipTopRows = 0, MaxDataColumns = 30000;
        public long RetrieveTopRows = 0;
        public FlatFileType Type = FlatFileType.CSV;
        //For Delimitered File
        public string DelimiterRegularExpression = ",";
        //for fixed length
        public XElement FixLengthFormat = null;
        //For CSV
        public char CSVDelimiter = ',';
        public char CSVTextQualifier = '"';

        TextReader Reader = null;
        IParser Parser = null;
        List<string> Line = null;
        long LineID = 0;
        DataSetDefinition setDefinition = new DataSetDefinition();
        public override bool InitializeDataSource()
        {
            Reader = ContentIsPath ? new StreamReader(Content, Encoding.GetEncoding(1252)) : new StreamReader(new MemoryStream(System.Text.ASCIIEncoding.Unicode.GetBytes(Content)), System.Text.Encoding.Unicode);
            switch (Type)
            {
                case FlatFileType.CSV:
                    Parser = new ParserCSV() { Delimiter = CSVDelimiter, TextQualifier = CSVTextQualifier };
                    break;
                case FlatFileType.Delimitered:
                    Parser = new DelimiteredFileParser(DelimiterRegularExpression);
                    break;
                case FlatFileType.FixedLength:
                    Parser = new FixedLenghFileParser(FixLengthFormat);
                    break;
                case FlatFileType.Line:
                    Parser = new LineFileParser();
                    break;
                case FlatFileType.PeopleSoftCSV84:
                    Parser = new PeopleSoftCSV84();
                    break;
                default:
                    Parser = null;
                    break;
            }

            for (int i = 0; i < SkipTopLines; i++)//Skip top lines
            {
                Reader.ReadLine();
            }

            Line = Parser.ReadLine(Reader); //First Line
            if (Line == null)
            {
                return false;
            }

            DataColumnDefinition col;
            if (IncludeRowID)
            {
                col = new DataColumnDefinition() { Name = "___RowID___", DataTypeName = "bigint", Table = setDefinition };
                setDefinition.Columns.Add(col);
            }

            //Line = Parser.ReadLine(Reader);
            for (int i = 0; i < Line.Count && i < MaxDataColumns; i++)
            {
                col = new DataColumnDefinition() { Size = -1, DataTypeName = "nvarchar", Table = setDefinition };

                if (!FirstRowContainColumnName)
                    col.Name = "C" + (i + 1).ToString();
                else
                {
                    Line[i] = Line[i].Trim();
                    if (Line[i] == "")
                        Line[i] = "C" + (i + 1).ToString();
                    if (setDefinition.Columns.Exists(x => x.Name.ToLower() == Line[i].ToLower()))
                        throw new Exception("Duplicated Column Name Found, " + Line[i]);
                    col.Name = Line[i];
                }
                setDefinition.Columns.Add(col);
                Thread.Sleep(0);
            }

            setDefinition.ResetColumnOrdinal();
            PopulateMetadataTable(setDefinition);
            if (FirstRowContainColumnName)
                Line = Parser.ReadLine(Reader);
            LineID = 1;
            while (LineID <= SkipTopRows) //skip rows
            {
                if (Line == null)
                    return false;
                LineID++;
                Line = Parser.ReadLine(Reader);
            }
            return true;
        }
        bool ReachTotalRows()
        {
            if (RetrieveTopRows >= 0)
            {
                if (RetrieveTopRows < LineID)
                {
                    return true;
                }
            }
            return false;
        }
        public override bool ReadOneFromDataSource(bool stop = false)
        {
            if (stop)
                return false;
            if (ReachTotalRows())
                return false;
            int k, j;
            bool IsAllEmpty = true;
            if (Line != null)
            {
                k = 0;
                object[] row = new object[setDefinition.ColumnCount];
                if (IncludeRowID)
                {
                    row[0] = LineID;
                    k++;
                }
                j = 0;
                IsAllEmpty = true;
                while (k < setDefinition.ColumnCount)
                {
                    row[k] = DBNull.Value;
                    if (Line.Count > j)
                    {
                        if (Line[j] != "")
                        {
                            row[k] = Line[j];
                            if (IsAllEmpty) IsAllEmpty = false;
                        }
                    }
                    k++; j++;
                }
                if (!IsAllEmpty)
                {
                    if (!PushRecord(row))
                    {
                        return !ReachTotalRows();
                    }
                }
                else
                {
                    return (ReadOneFromDataSource(stop)) && (!ReachTotalRows());
                }

                LineID++;
                Line = Parser.ReadLine(Reader);
                return true;
            }
            return false;
        }
        public override void CompleteDataSource()
        {
            base.CompleteDataSource();
            try { Reader.Close(); }
            catch { }
        }


        [SqlProcedure]
        public static void ReadFile(
            SqlString FileType, SqlChars ContentOrFileName,
            SqlBoolean IsContent, SqlString DelimiterRegularExpression, SqlXml FixLengthFormat,
            SqlBoolean FirstRowContainColumnName, SqlInt32 SkipTopLines, SqlInt32 SkipTopRows, SqlInt64 RetrieveTopRows
            )
        {
            using (FlatFile flatFile = new FlatFile())
            {
                if (!FileType.IsNull)
                    flatFile.Type = (FlatFileType)Enum.Parse(typeof(FlatFileType), FileType.Value, true);
                if (ContentOrFileName.IsNull)
                    throw new Exception("Content or File Path should be not be null.");
                flatFile.Content = new string(ContentOrFileName.Value);
                if (!IsContent.IsNull)
                    flatFile.ContentIsPath = !IsContent.Value;
                if (!DelimiterRegularExpression.IsNull)
                    flatFile.DelimiterRegularExpression = DelimiterRegularExpression.Value;
                if (!FixLengthFormat.IsNull)
                    flatFile.FixLengthFormat = XElement.Parse(FixLengthFormat.Value);
                if (!FirstRowContainColumnName.IsNull)
                    flatFile.FirstRowContainColumnName = FirstRowContainColumnName.Value;
                if (!SkipTopLines.IsNull)
                    flatFile.SkipTopLines = SkipTopLines.Value;
                if (!SkipTopRows.IsNull)
                    flatFile.SkipTopRows = SkipTopRows.Value;
                if (!RetrieveTopRows.IsNull)
                    flatFile.RetrieveTopRows = RetrieveTopRows.Value;

                flatFile.InitializeDataSource();
                SqlDataRecord record = flatFile.SetDefinition.GetDataRecord();
                SqlContext.Pipe.SendResultsStart(record);
                while (flatFile.Read())
                {
                    record.SetValues(flatFile.CurrentRow);
                    SqlContext.Pipe.SendResultsRow(record);
                }
                SqlContext.Pipe.SendResultsEnd();
            }
        }
    }

}