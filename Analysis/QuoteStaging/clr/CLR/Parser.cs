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
    public interface IParser
    {
        List<string> ReadLine(TextReader reader);
    }
    public class ParserCSV : IParser
    {
        public char Delimiter = ',';
        public char TextQualifier = '"';
        public List<string> ReadLine(TextReader reader)
        {
            if (reader.Peek() == -1)
                return null;
            List<string> row = new List<string>();
            char item;
            bool isQuoteStarted = false;
            List<char> col = new List<char>(1000);
            int nextChar;
            while (true)
            {
                if (reader.Peek() == -1)
                {
                    if (col.Count > 0)
                        row.Add(new string(col.ToArray()));
                    return row;
                }
                item = (char)reader.Read();
                nextChar = reader.Peek();
                if ((item != Delimiter) || isQuoteStarted)
                {
                    if ((nextChar != -1) && ((item == '\r') || (item == '\n')) && !isQuoteStarted)
                    {
                        row.Add(new string(col.ToArray()));
                        while (((nextChar != -1) && ((char)nextChar == '\r' || ((char)nextChar == '\n'))))
                        {
                            nextChar = reader.Read();
                            nextChar = reader.Peek();
                        }
                        return row;
                    }
                    else if (item == TextQualifier)
                    {
                        if (!isQuoteStarted && (new string(col.ToArray()).Trim() == ""))
                        {
                            isQuoteStarted = true;
                        }
                        else if (isQuoteStarted)
                        {
                            if ((nextChar != -1) && ((char)nextChar == TextQualifier))
                            {
                                col.Add(TextQualifier);
                                reader.Read(); //skip next char
                            }
                            else
                            {
                                isQuoteStarted = false;
                            }
                        }
                    }
                    else
                    {
                        col.Add(item);
                    }
                }
                else
                {
                    row.Add(new string(col.ToArray()));
                    col.Clear();
                }
            }
        }
    }
    public class LineFileParser : IParser
    {
        public List<string> ReadLine(TextReader reader)
        {
            if (reader.Peek() == -1)
                return null;
            List<string> ret = new List<string>();
            ret.Add(reader.ReadLine());
            return ret;
        }
    }
    public class DelimiteredFileParser : IParser
    {
        Regex expression = null;
        public DelimiteredFileParser(string pattern)
        {
            expression = new Regex(pattern, RegexOptions.Compiled);
        }
        public List<string> ReadLine(TextReader reader)
        {
            if (reader.Peek() == -1)
                return null;
            string line = reader.ReadLine();
            List<string> ret = new List<string>();
            ret.AddRange(expression.Split(line));
            return ret;
        }
    }
    public class FixedLenghFileParser : IParser
    {
        int[][] format = null;
        public FixedLenghFileParser(XElement fmt)
        {
            format = fmt.Elements().Select(x => new int[2] { int.Parse(x.Attribute("Position").Value), int.Parse(x.Attribute("Length").Value) }).ToList().ToArray();
        }
        public List<string> ReadLine(TextReader reader)
        {
            if (reader.Peek() == -1)
                return null;
            List<string> ret = new List<string>();
            string str = reader.ReadLine();
            int len = str.Length;
            foreach (int[] f in format)
            {
                if (f[0] <= len && f[1] > 0)
                {
                    ret.Add(str.Substring(f[0] - 1, Math.Min(f[1], len - f[0] + 1)));
                }
                else
                {
                    ret.Add("");
                }
            }
            return ret;
        }
    }
    public class PeopleSoftCSV84 : IParser
    {
        public List<string> ReadLine(TextReader reader)
        {
            if (reader.Peek() == -1)
                return null;
            List<string> row = new List<string>();
            char item;
            List<char> col = new List<char>(1000);
            int nextChar;
            Action<List<char>> RearrangeColumn = (column) =>
            {
                if (column.Count > 1)
                {
                    if (column[0] == '"')
                    {
                        column.RemoveAt(0);
                    }
                    if (column[column.Count - 1] == '\r')
                        column.RemoveAt(column.Count - 1);
                    if (column[column.Count - 1] == '\n')
                        column.RemoveAt(column.Count - 1);
                    if (column[column.Count - 1] == '\n')
                        column.RemoveAt(column.Count - 1);
                    if (column[column.Count - 1] == '\r')
                        column.RemoveAt(column.Count - 1);
                    if (column[column.Count - 1] == '"')
                        column.RemoveAt(column.Count - 1);
                }
            };
            while (true)
            {
                if (reader.Peek() == -1)
                {
                    if (col.Count > 0)
                    {
                        RearrangeColumn(col);
                        row.Add(new string(col.ToArray()));
                    }
                    return row;
                }
                item = (char)reader.Read();
                nextChar = reader.Peek();
                col.Add(item);
                if (col.Count >= 2)
                {
                    if ((col[col.Count - 1] == ',') && (col[col.Count - 2] == '"') && ((char)nextChar == '"'))
                    {
                        col.RemoveAt(col.Count - 1);
                        RearrangeColumn(col);
                        row.Add(new string(col.ToArray()));
                        col.Clear();
                    }
                    else if ((col[col.Count - 1] == '"') && (col[col.Count - 2] == '\r') && ((char)nextChar == '"'))
                    {
                        col.RemoveAt(col.Count - 1);
                        RearrangeColumn(col);
                        row.Add(new string(col.ToArray()));
                        col.Clear();
                        return row;
                    }
                    else if ((col[col.Count - 1] == '"') && (col[col.Count - 2] == '\n') && ((char)nextChar == '"'))
                    {
                        col.RemoveAt(col.Count - 1);
                        RearrangeColumn(col);
                        row.Add(new string(col.ToArray()));
                        col.Clear();
                        return row;
                    }
                    else if ((col[col.Count - 2] == '"') && (col[col.Count - 1] == '\r') && ((char)nextChar == '"'))
                    {
                        col.RemoveAt(col.Count - 1);
                        RearrangeColumn(col);
                        row.Add(new string(col.ToArray()));
                        col.Clear();
                        return row;
                    }
                    else if ((col[col.Count - 2] == '"') && (col[col.Count - 1] == '\n') && ((char)nextChar == '"'))
                    {
                        col.RemoveAt(col.Count - 1);
                        RearrangeColumn(col);
                        row.Add(new string(col.ToArray()));
                        col.Clear();
                        return row;
                    }
                    else if (col.Count >= 3)
                    {
                        if (
                            ((col[col.Count - 3] == '"') && (col[col.Count - 1] == '\n') && (col[col.Count - 2] == '\r') && ((char)nextChar == '"'))
                            ||
                            ((col[col.Count - 3] == '"') && (col[col.Count - 2] == '\n') && (col[col.Count - 1] == '\r') && ((char)nextChar == '"'))


                            )
                        {
                            col.RemoveAt(col.Count - 1);
                            col.RemoveAt(col.Count - 1);
                            RearrangeColumn(col);
                            row.Add(new string(col.ToArray()));
                            col.Clear();
                            return row;
                        }

                    }
                }
            }
        }
    }
}
