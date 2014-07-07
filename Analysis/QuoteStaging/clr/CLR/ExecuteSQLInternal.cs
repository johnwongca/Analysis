//------------------------------------------------------------------------------
// <copyright file="CSSqlClassFile.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Text;
using System.Threading;
using Microsoft.SqlServer.Server;

namespace sqlnotes.info
{
    public class ExecuteSQLInternal
    {
        static string GetConnectionString(short sessionid)
        {
            return SessionParameter.GetParameter(sessionid).GetItem("@___ConnectionString___").Value.ToString();            
        }
        static string GetLoopbackConnectionString()
        {
            using(SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "select clr.GetLoopbackConnectionString()";
                cmd.CommandType = CommandType.Text;
                return cmd.ExecuteScalar().ToString();
            }
        }
        static KeyValuePair<short,SqlCommand> SetCommand(SqlString CommandType, SqlChars Command, SqlInt16 sessionID)
        {
            short currentSessionID = sessionID.Value;
            SqlConnection connection = new SqlConnection(GetConnectionString(currentSessionID));
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = new string(Command.Value);
            command.CommandType = (CommandType)(Enum.Parse(typeof(CommandType), CommandType.Value));
            command.Parameters.Clear();
            command.CommandTimeout = 0;
            command.Parameters.AddRange(SessionParameter.GetParameter(currentSessionID).GetParameters());
            if (command.CommandType == System.Data.CommandType.StoredProcedure)
            {
                SqlParameter p = SessionParameter.GetParameter(currentSessionID).SetValue(new SqlParameter("@___ReturnValue___", SqlDbType.Int));
                p.Direction = ParameterDirection.ReturnValue;
                p = command.Parameters.Add(p);

            }
            return new KeyValuePair<short,SqlCommand>(currentSessionID, command);
        }
        [SqlProcedure]
        public static SqlInt32 ExecuteSQL(SqlInt16 SessionID, SqlString CommandType, SqlChars Command)
        {
            SqlCommand command = SetCommand(CommandType, Command, SessionID).Value;
            try
            {
                using (SqlDataReader r = command.ExecuteReader())
                {
                    do
                    {
                        SqlContext.Pipe.Send(r);
                    } while (r.NextResult());
                }
                return new SqlInt32(0);
            }
            finally
            {
                try { command.Connection.Close(); }
                catch { }
            }
        }
        [SqlFunction(DataAccess = DataAccessKind.Read)]
        public static SqlInt32 ExecuteSQLOneRow(SqlInt16 SessionID, SqlString CommandType, SqlChars Command)
        {
            var p = SetCommand(CommandType, Command, SessionID);
            SqlCommand command = p.Value;
            try
            {
                using (SqlDataReader r = command.ExecuteReader())
                {

                    if (r.Read())
                    {
                        for (int i = 0; i < r.FieldCount; i++)
                        {
                            SessionInfo.SessionInfoSetValue(p.Key, r.GetName(i), r[i]);
                        }
                    }
                    return new SqlInt32(0);
                }
            }
            finally
            {
                try { command.Connection.Close(); }
                catch { }
            }
        }
        class kv { 
            public SqlInt32 SetNumber;
            public SqlInt32 RowNumber;
            public SqlString ColumnName;
            public SqlInt32 ColumnOrdinal;
            public object Value;
        }
        [SqlFunction(DataAccess = DataAccessKind.Read, FillRowMethodName = "ExecuteSQLAll_FillRow", TableDefinition = "SetNumber int, RowNumber int, ColumnOrdinal int, ColumnName nvarchar(128), Value sql_variant", SystemDataAccess = SystemDataAccessKind.Read)]
        public static IEnumerable ExecuteSQLAll(SqlInt16 SessionID, SqlString CommandType, SqlChars Command)
        {
            var p = SetCommand(CommandType, Command, SessionID);
            SqlCommand command = p.Value;
            int setNumber = 0, rowNumber = 0;
            DataTable schemaTable;
            List<kv> ret = new List<kv>();
            try
            {
                using (SqlDataReader r = p.Value.ExecuteReader())
                {
                    if (r != null)
                    {
                        do
                        {
                            setNumber++;
                            rowNumber = 0;
                            schemaTable = r.GetSchemaTable();
                            while (r.Read())
                            {
                                rowNumber++;
                                for (int i = 0; i < r.FieldCount; i++)
                                {
                                    if ((bool)(schemaTable.Rows[i][SchemaTableColumn.IsLong]))
                                        continue;

                                    ret.Add(new kv()
                                    {
                                        SetNumber = new SqlInt32(setNumber),
                                        ColumnName = new SqlString(schemaTable.Rows[i][SchemaTableColumn.ColumnName].ToString()),
                                        RowNumber = new SqlInt32(rowNumber),
                                        Value = r.GetValue(i),
                                        ColumnOrdinal = new SqlInt32(i+1)
                                    });
                                }
                                
                            }
                        } while (r.NextResult());
                    }
                }
                return ret;
            }
            finally
            {
                try { p.Value.Connection.Close(); }
                catch { }
            }
        }
        public static void ExecuteSQLAll_FillRow(object o, out SqlInt32 SetNumber, out SqlInt32 RowNumber, out SqlInt32 ColumnOrdinal, out SqlString ColumnName, out object Value)
        {
            kv data = (kv)o;
            SetNumber = data.SetNumber;
            RowNumber = data.RowNumber;
            ColumnName = data.ColumnName;
            Value = data.Value;
            ColumnOrdinal = data.ColumnOrdinal;
        }        
        [SqlFunction(DataAccess = DataAccessKind.Read, SystemDataAccess = SystemDataAccessKind.Read)]
        public static SqlInt64 BulkCopy(SqlInt16 SessionID, SqlString CommandType, SqlChars Command, SqlString TargetTable, SqlInt32 BatchSize, SqlBoolean AutoColumnMapping)
        {
            DataTable targetSchemaTable, sourceSchemaTable;
            string sourceColumnName, targetColumnName;
            long ret = 0;
            using (SqlConnection targetConnection = new SqlConnection(GetLoopbackConnectionString()))
            {
                targetConnection.Open();

                SqlCommand targetCommand = targetConnection.CreateCommand();
                targetCommand.CommandType = System.Data.CommandType.Text;
                targetCommand.CommandText = "select top 0 * from " + TargetTable.Value;
                using(SqlDataReader r0 = targetCommand.ExecuteReader())
                {
                    targetSchemaTable = r0.GetSchemaTable();
                    r0.Close();
                }

                SqlCommand sourceCommand = SetCommand(CommandType, Command, SessionID).Value;
                try
                {
                    using (SqlDataReader r = sourceCommand.ExecuteReader())
                    {
                        sourceSchemaTable = r.GetSchemaTable();
                        using(SqlBulkCopy bulkCopy = new SqlBulkCopy(targetConnection.ConnectionString, SqlBulkCopyOptions.FireTriggers))
                        {
                            bulkCopy.BulkCopyTimeout = 60;
                            bulkCopy.BatchSize = BatchSize.Value;
                            bulkCopy.DestinationTableName = TargetTable.Value;
                            bulkCopy.NotifyAfter = BatchSize.Value == 0 ? 10000 : BatchSize.Value;
                            bulkCopy.SqlRowsCopied += (sender, e) =>
                                {
                                    ret = e.RowsCopied;
                                    Thread.Sleep(0);
                                    SessionInfo.SessionInfoSetValue(SessionID.Value, "___BulkCopyRowsCopied___", new SqlInt64(ret));
                                };
                            if (AutoColumnMapping.Value)
                            {
                                foreach (DataRow sourceRow in sourceSchemaTable.Rows)
                                {
                                    sourceColumnName = sourceRow[SchemaTableColumn.ColumnName].ToString();
                                    foreach (DataRow targetRow in targetSchemaTable.Rows)
                                    {
                                        targetColumnName = targetRow[SchemaTableColumn.ColumnName].ToString();
                                        if (sourceColumnName.ToUpper().Trim() == targetColumnName.ToUpper().Trim())
                                        {
                                            bulkCopy.ColumnMappings.Add(sourceColumnName, targetColumnName);
                                            break;
                                        }
                                    }
                                }
                            }
                            bulkCopy.EnableStreaming = true;
                            bulkCopy.WriteToServer(r);
                            return new SqlInt64(ret);
                        }
                        
                    }
                    
                }
                finally
                {
                    try { sourceCommand.Connection.Close(); }
                    catch { }
                    
                }
                
            }
            
        }

    }
}
