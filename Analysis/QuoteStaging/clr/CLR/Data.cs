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
using System.Data.OleDb;


using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

using Microsoft.SqlServer.Server;
namespace etl.sqlnotes.info
{
    public class Data
    {
        [SqlProcedure]
        public static void ForceCollectMemory()
        {
            for (int i = GC.MaxGeneration; i >= 0; i--)
            {
                GC.Collect(i, GCCollectionMode.Forced);
            }
        }

        [SqlFunction]
        public static SqlString GetSQLName(SqlString Name)
        {
            if(Name.IsNull)
                return SqlString.Null;
            return new SqlString(Name.Value.GetSQLName());
        }
        
        [SqlFunction(DataAccess=DataAccessKind.Read)]
        public static SqlChars ExecuteSQLScalarString(SqlString ConnectionString, SqlChars CommandText)
        {
            using(SqlConnection connection = new SqlConnection(ConnectionString.Value))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = new string(CommandText.Value);
                command.CommandTimeout = 0;
                var ret = command.ExecuteScalar();
                return ret == null ? SqlChars.Null : new SqlChars(ret.ToString());
            }
        }
        [SqlFunction(DataAccess = DataAccessKind.Read)]
        public static object ExecuteOLEDBScalarObject(SqlBoolean IsSourceOLEDBConnection, SqlString ConnectionString, SqlChars CommandText)
        {
            using (DbConnection connection = IsSourceOLEDBConnection.Value ? (DbConnection)(new OleDbConnection(ConnectionString.Value)) : (DbConnection)(new SqlConnection(ConnectionString.Value)))
            {
                connection.Open();
                DbCommand command = connection.CreateCommand();
                command.CommandText = new string(CommandText.Value);
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 0;
                return command.ExecuteScalar();
            }
        }
        static DataColumnDefinition GetOracleDefinition(DataRow row)
        {
            DataColumnDefinition ret = new DataColumnDefinition()
            {
                Name = (string)row[SchemaTableColumn.ColumnName],
                Ordinal = (int)row[SchemaTableColumn.ColumnOrdinal],
                Size = (int)row[SchemaTableColumn.ColumnSize],
                Precision = (short)row[SchemaTableColumn.NumericPrecision],
                Scale = (short)row[SchemaTableColumn.NumericScale],
                Nullable = (bool)row[SchemaTableColumn.AllowDBNull],
                DataTypeName = OracleTypeConversion((Type)row[SchemaTableColumn.DataType])
            };
            if (ret.DataTypeName == "decimal" && ret.Scale > 38)
                ret.Scale = 0;
            if (ret.DataTypeName == "datetime2")
                ret.Scale = 7;
            return ret;
        }
        static string OracleTypeConversion(Type type)
        {
            if (type == typeof(byte[]))
                return "varbinary";
            if (type == typeof(string))
                return "nvarchar";
            if (type == typeof(DateTime))
                return "datetime";
            if (type == typeof(decimal))
                return "decimal";
            if (type == typeof(int))
                return "int";
            return "nvarchar";
        }
        [SqlFunction(DataAccess = DataAccessKind.Read, SystemDataAccess = SystemDataAccessKind.Read,
            FillRowMethodName = "GetOLEDBQuerySchemaInternal_FillRow",
            TableDefinition = "Name nvarchar(128), Ordinal int, DataType nvarchar(128), Size int, Precision smallint, Scale smallint, Nullable bit, IsLong bit"
            )]
        public static IEnumerable GetOLEDBQuerySchemaInternal(SqlBoolean IsSourceOLEDBConnection, SqlString ConnectionString, SqlChars CommandText)
        {
            using (DbConnection connection = IsSourceOLEDBConnection.Value ? (DbConnection)(new OleDbConnection(ConnectionString.Value)) : (DbConnection)(new SqlConnection(ConnectionString.Value)))
            {
                connection.Open();
                DbCommand command = connection.CreateCommand();
                command.CommandText = new string(CommandText.Value);
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 0;

                DbDataReader reader = command.ExecuteReader();
                DataTable t = reader.GetSchemaTable();
                reader.Read();
                command.Cancel();
                reader.Close();
                connection.Close();
                foreach (DataRow x in t.Rows)
                {
                    yield return
                        t.Columns.Contains("DataTypeName") ?

                        new DataColumnDefinition()
                        {
                            Name = (string)x[SchemaTableColumn.ColumnName],
                            Ordinal = (int)x[SchemaTableColumn.ColumnOrdinal],
                            Size = (int)x[SchemaTableColumn.ColumnSize],
                            Precision = (short)x[SchemaTableColumn.NumericPrecision],
                            Scale = (short)x[SchemaTableColumn.NumericScale],
                            Nullable = (bool)x[SchemaTableColumn.AllowDBNull],
                            DataTypeName =  (string)x["DataTypeName"].ToString()
                        }
                        :
                        GetOracleDefinition(x);
                    
                }
            }
        }
        public static void GetOLEDBQuerySchemaInternal_FillRow(object o, out SqlString Name, out SqlInt32 Ordinal, out SqlString DataType, out SqlInt32 Size, out SqlInt16 Precision, out SqlInt16 Scale, out SqlBoolean Nullable, out SqlBoolean IsLong)
        {
            DataColumnDefinition d = (DataColumnDefinition)o;
            Name = new SqlString(d.Name);
            Ordinal = new SqlInt32(d.Ordinal);
            DataType = new SqlString(d.SQLDBType.ToString());
            Size = new SqlInt32(d.Size);
            Precision = new SqlInt16(d.Precision);
            Scale = new SqlInt16(d.Scale);
            Nullable = new SqlBoolean(d.Nullable);
            IsLong = new SqlBoolean(d.IsLong);
        }
        [SqlProcedure]
        public static void ExecuteSQLTimeout(SqlString ConnectionString, SqlChars CommandText, SqlInt32 Timeout)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString.Value))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = new string(CommandText.Value);
                command.CommandTimeout = Timeout.IsNull ? 0 : Timeout.Value;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader == null)
                        return;
                    SqlContext.Pipe.Send(reader);
                }
            }
        }
        [SqlProcedure]
        public static void ExecuteOLEDBSQL(SqlBoolean IsSourceOLEDBConnection, SqlString ConnectionString, SqlChars CommandText)
        {
            using (DbConnection connection = IsSourceOLEDBConnection.Value ? (DbConnection)(new OleDbConnection(ConnectionString.Value)) : (DbConnection)(new SqlConnection(ConnectionString.Value)))
            {
                connection.Open();
                DbCommand command = connection.CreateCommand();
                command.CommandText = new string(CommandText.Value);
                command.CommandTimeout = 0;
                using (DbDataReader reader = command.ExecuteReader())
                {
                    if (reader == null)
                        return;
                    if(reader is SqlDataReader)
                        SqlContext.Pipe.Send((SqlDataReader)reader);
                    else
                    {
                        SqlDataRecord record;
                        //DataColumnDefinition d;
                        List<DataColumnDefinition> m = new List<DataColumnDefinition>();
                        DataTable t = reader.GetSchemaTable();
                        foreach (DataRow x in t.Rows)
                        {
                            m.Add(GetOracleDefinition(x));
                        }
                        record = new SqlDataRecord(m.Select(x => x.SqlMetaData).ToArray());
                        SqlContext.Pipe.SendResultsStart(record);
                        object[] values = new object[record.FieldCount];
                        while(reader.Read())
                        {
                            reader.GetValues(values);
                            record.SetValues(values);
                            SqlContext.Pipe.SendResultsRow(record);
                        }
                        SqlContext.Pipe.SendResultsEnd();

                    }
                }
            }
        }
        [SqlProcedure]
        public static void ExecuteSQL(SqlString ConnectionString, SqlChars CommandText)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString.Value))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = new string(CommandText.Value);
                command.CommandTimeout = 0;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader == null)
                        return;
                    SqlContext.Pipe.Send(reader);
                }
            }
        }
        [SqlFunction(DataAccess = DataAccessKind.Read,
            FillRowMethodName = "ExecuteSQLWithInfo_FillRow",
            TableDefinition = "ID int, ErrorNumber int, SeverityLevel smallint, State smallint, ProcedureName nvarchar(128), LineNumber int, Message nvarchar(max), Type nvarchar(9)"
            )]
        public static IEnumerable ExecuteSQLWithInfo(SqlString ConnectionString, SqlBoolean IsCommandTextProcedure,SqlChars CommandText)
        {
            List<Tuple<int, int, short, short, string, int, string, Tuple<string>>> ret = new List<Tuple<int, int, short, short, string, int, string, Tuple<string>>>();
            int index = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.Value))
                {
                    connection.FireInfoMessageEventOnUserErrors = true;
                    connection.InfoMessage += (sender, e) =>
                        {
                            for (int i = 0; i < e.Errors.Count; i++)
                            {
                                index++;
                                ret.Add(new Tuple<int, int, short, short, string, int, string, Tuple<string>>
                                 (
                                    index,
                                    e.Errors[i].Number,
                                    e.Errors[i].Class,
                                    e.Errors[i].State,
                                    e.Errors[i].Procedure,
                                    e.Errors[i].LineNumber,
                                    e.Errors[i].Message,
                                    new Tuple<string>(e.Errors[i].Class > 10 ? "Error" : "Info")
                                 ));
                            }
                        };
                    connection.Open();
                    
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = new string(CommandText.Value);
                    command.CommandType = (IsCommandTextProcedure.IsNull || IsCommandTextProcedure.IsFalse) ? CommandType.Text : CommandType.StoredProcedure;
                    command.CommandTimeout = 0;
                    command.Parameters.Add("@___ReturnValue___", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                    var r = command.ExecuteScalar();
                    if ((r != null) || ((command.Parameters[0] != null) && (Convert.ToInt32(command.Parameters[0].Value) !=0)))
                    {
                        index++;
                        ret.Add(new Tuple<int, int, short, short, string, int, string, Tuple<string>>
                                    (
                                       index,
                                       (command.Parameters[0] != null) ? Convert.ToInt32(command.Parameters[0].Value) : int.MinValue,
                                       short.MinValue,
                                       short.MinValue,
                                       string.Empty,
                                       int.MinValue,
                                       r != null ? r.ToString(): string.Empty,
                                       new Tuple<string>("Return")
                                    )
                                    );
                    }
                    return ret;
                }
            }catch (Exception ee)
            {
                if (ee is SqlException)
                {
                    SqlException se = (SqlException)ee;

                    index++;
                    ret.Add(new Tuple<int, int, short, short, string, int, string, Tuple<string>>
                                    (
                                       index,
                                       se.Number,
                                       se.Class,
                                       se.State,
                                       se.Procedure,
                                       se.LineNumber,
                                       se.Message,
                                       new Tuple<string>("Exception")
                                    )
                                    );
                }
                else
                {
                    index++;
                    ret.Add(new Tuple<int, int, short, short, string, int, string, Tuple<string>>
                                    (
                                       index,
                                       int.MinValue,
                                       short.MinValue,
                                       short.MinValue,
                                       string.Empty,
                                       int.MinValue,
                                       ee.Message,
                                       new Tuple<string>("Exception")
                                    )
                                    );
                }
                return ret;
            }
        }
        public static void ExecuteSQLWithInfo_FillRow(object o, out SqlInt32 ID, out SqlInt32 ErrorNumber, out SqlInt16 SeverityLevel, out SqlInt16 State, out SqlString ProcedureName, out SqlInt32 LineNumber, out SqlString Message, out SqlString Type)
         {
             Tuple<int, int, short, short, string, int, string, Tuple<string>> d = (Tuple<int, int, short, short, string, int, string, Tuple<string>>)o;
             ID = d.Item1 == int.MinValue ? SqlInt32.Null : new SqlInt32(d.Item1);
             ErrorNumber = d.Item2 == int.MinValue ? SqlInt32.Null : new SqlInt32(d.Item2);
             SeverityLevel = d.Item3 == short.MinValue ? SqlInt16.Null : new SqlInt16(d.Item3);
             State = d.Item4 == short.MinValue ? SqlInt16.Null : new SqlInt16(d.Item4);
             ProcedureName = string.IsNullOrEmpty(d.Item5) ? SqlString.Null : new SqlString(d.Item5);
             LineNumber = d.Item6 == int.MinValue ? SqlInt32.Null : new SqlInt32(d.Item6);
             Message = string.IsNullOrEmpty(d.Item7) ? SqlString.Null : new SqlString(d.Item7);
             Type = new SqlString(d.Rest.Item1);
         }


        [SqlFunction(DataAccess = DataAccessKind.Read, 
            FillRowMethodName = "GetReturnSchema_FillRow",
            TableDefinition = "ColumnID int, ColumnName nvarchar(128), DataType nvarchar(128), DataLength int, Precision smallint, Scale smallint"
            )]
        public static IEnumerable GetReturnSchema(SqlString ConnectionString, SqlChars CommandText)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString.Value))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = new string(CommandText.Value);
                command.CommandTimeout = 0;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    DataRowCollection ret;
                    if (reader == null)
                        ret = (new DataTable()).Rows;
                    else
                        ret = reader.GetSchemaTable().Rows;
                    command.Cancel();
                    return ret;
                }
            }
        }
        public static void GetReturnSchema_FillRow(object o, out SqlInt32 ColumnID, out SqlString ColumnName, out SqlString DataType, out SqlInt32 DataLength, out SqlInt16 Precision, out SqlInt16 Scale)
        {
            DataRow x = (DataRow)o;
            ColumnID = new SqlInt32((int)x[SchemaTableColumn.ColumnOrdinal] + 1);
            ColumnName = new SqlString((string)x[SchemaTableColumn.ColumnName]);
            DataLength = new SqlInt32((int)x[SchemaTableColumn.ColumnSize] > 8000 ? -1 : (int)x[SchemaTableColumn.ColumnSize]);
            Precision = new SqlInt16( (short)x[SchemaTableColumn.NumericPrecision]);
            Scale = new SqlInt16((short)x[SchemaTableColumn.NumericScale]);
            DataType = new SqlString(x["DataTypeName"].ToString());
        }

        [SqlProcedure]
        public static SqlInt32 BulkCopy(SqlBoolean IsSourceOLEDBConnection, SqlString SourceConnection, SqlChars CommandText, SqlString TargetConnection, SqlString TargetTable, SqlInt32 BatchSize, SqlXml Mapping, SqlBoolean UseFieldNameMapping, SqlBoolean KeepIdentity, SqlBoolean CheckConstraints, SqlBoolean TableLock, SqlBoolean KeepNulls, SqlBoolean FireTriggers)
        {
            long rowcount = 0;
            SqlBulkCopyOptions options = SqlBulkCopyOptions.Default;
            if ((!KeepIdentity.IsNull) && KeepIdentity.Value)
                options = options | SqlBulkCopyOptions.KeepIdentity;
            if ((!CheckConstraints.IsNull) && CheckConstraints.Value)
                options = options | SqlBulkCopyOptions.CheckConstraints;
            if ((!KeepNulls.IsNull) && KeepNulls.Value)
                options = options | SqlBulkCopyOptions.KeepNulls;
            if ((!FireTriggers.IsNull) && FireTriggers.Value)
                options = options | SqlBulkCopyOptions.FireTriggers;

            using (DbConnection connection = IsSourceOLEDBConnection.Value ? (DbConnection)(new OleDbConnection(SourceConnection.Value)) : (DbConnection)(new SqlConnection(SourceConnection.Value)))
            {
                connection.Open();
                DbCommand command = connection.CreateCommand();
                command.CommandText = new string(CommandText.Value);
                command.CommandTimeout = 0;
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader == null)
                        return new SqlInt32(0);
                    using (SqlBulkCopy bc = new SqlBulkCopy(TargetConnection.Value, options))
                    {
                        bc.BatchSize = BatchSize.Value;
                        bc.EnableStreaming = true;
                        bc.NotifyAfter = 1;
                        bc.BulkCopyTimeout = 0;
                        bc.DestinationTableName = TargetTable.Value;
                        bc.SqlRowsCopied += (obj, r) => rowcount = r.RowsCopied;
                        using (SqlConnection targetConnection = new SqlConnection(TargetConnection.Value))
                        {
                            targetConnection.Open();
                            SqlCommand targetCommand = targetConnection.CreateCommand();
                            targetCommand.CommandText = "select top 0 * from " + TargetTable.ToString();
                            using (SqlDataReader targetReader = targetCommand.ExecuteReader())
                            {
                                using (DataTable TargetSchema = targetReader.GetSchemaTable())
                                {
                                    if (UseFieldNameMapping.Value && Mapping.IsNull)
                                    {
                                        foreach (DataRow row in TargetSchema.Rows)
                                        {


                                            foreach (DataRow x in reader.GetSchemaTable().Rows)
                                            {
                                                if (x["ColumnName"].ToString().ToUpper() == row["ColumnName"].ToString().ToUpper())
                                                {
                                                    bc.ColumnMappings.Add(x["ColumnName"].ToString(), row["ColumnName"].ToString());
                                                    break;
                                                }
                                            }

                                        }
                                    }
                                    else if (!Mapping.IsNull)
                                    {
                                        string sourceColumnName, targetColumnName;
                                        bool foundInSource, foundInTarget;
                                        XElement map = XElement.Parse(Mapping.Value);
                                        foreach (var item in map.Elements())
                                        {

                                            sourceColumnName = item.Attribute("Source").Value;
                                            targetColumnName = item.Attribute("Target").Value;
                                            foundInSource = false;
                                            foreach (DataRow s in reader.GetSchemaTable().Rows)
                                            {
                                                if (s["ColumnName"].ToString().ToUpper() == sourceColumnName.ToUpper())
                                                {
                                                    sourceColumnName = s["ColumnName"].ToString();
                                                    foundInSource = true;
                                                    break;
                                                }
                                            }
                                            foundInTarget = false;
                                            foreach (DataRow s in TargetSchema.Rows)
                                            {
                                                if (s["ColumnName"].ToString().ToUpper() == targetColumnName.ToUpper())
                                                {
                                                    targetColumnName = s["ColumnName"].ToString();
                                                    foundInTarget = true;
                                                    break;
                                                }
                                            }

                                            if (foundInSource && foundInTarget)
                                            {
                                                bc.ColumnMappings.Add(sourceColumnName, targetColumnName);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        bc.WriteToServer(reader);
                        bc.Close();
                    }
                    return new SqlInt32(Convert.ToInt32(rowcount));
                }
            }
        }

        

        [SqlFunction(DataAccess = DataAccessKind.Read,
            FillRowMethodName = "GetTableSchema_FillRow",
            TableDefinition = "ObjectID int, ColumnID int, ColumnName nvarchar(128), DataType nvarchar(128), Nullable bit, ReadOnly bit, CollationName nvarchar(128), PrimaryKeyOrder int"
            )]
        public static IEnumerable GetTableSchema(SqlString ConnectionString, SqlString TableName)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString.Value))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"select cast(c.object_id as int) ObjectID, c.name ColumnName, cast(c.column_id as int) ColumnID, cast(isnull(ic.key_ordinal,0) as int) PrimaryKeyOrder, c.max_length DataLength,
	case when type_name(c.system_type_id) = 'timestamp' then 'binary(8)' else type_name(c.system_type_id) end +
	case 
		when type_name(c.system_type_id) in ('binary', 'char', 'nchar') then '('+cast(c.max_length as varchar(20))+')'
		when type_name(c.system_type_id) in ('nchar') then '('+cast(c.max_length/2 as varchar(20))+')'
		when type_name(c.system_type_id) in ('datetime2', 'datetimeoffset', 'time') then '('+cast(c.scale as varchar(20))+')'
		when type_name(c.system_type_id) in ('decimal', 'numeric') then '('+cast(c.precision as varchar(20))+','+cast(c.scale as varchar(20))+')'
		when type_name(c.system_type_id) in ('varchar') then '('+case when c.max_length = -1 then 'max' else cast(c.max_length as varchar(20)) end+')'
		when type_name(c.system_type_id) in ('nvarchar') then '('+case when c.max_length = -1 then 'max' else cast(c.max_length/2 as varchar(20)) end+')'
		when type_name(c.system_type_id) in ('varbinary') then '('+case when c.max_length = -1 then 'max' else cast(c.max_length as varchar(20)) end+')'
		else ''
	end DataType,
	cast(c.is_nullable as bit) Nullable,
	cast(case when c.is_identity = 1 or c.is_computed = 1 or type_name(c.system_type_id) = 'timestamp' then 1 else 0 end as bit) ReadOnly,
	c.collation_name as CollationName
from sys.all_columns c 
	left outer join sys.indexes i on i.object_id = c.object_id and i.is_primary_key = 1
	left outer join sys.index_columns ic on ic.object_id = c.object_id and ic.index_id = i.index_id and ic.column_id = c.column_id
where c.object_id = object_id(@ObjectName)";
                command.CommandTimeout = 0;
                command.Parameters.Add("@ObjectName", SqlDbType.NVarChar, 128).Value = TableName.Value;
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable ret = new DataTable();
                da.Fill(ret);
                return ret.Rows;
            }
        }
        public static void GetTableSchema_FillRow(object o, out SqlInt32 ObjectID, out SqlInt32 ColumnID, out SqlString ColumnName, out SqlString DataType, out SqlBoolean Nullable, out SqlBoolean ReadOnly, out SqlString CollationName, out SqlInt32 PrimaryKeyOrder)
        {
            DataRow x = (DataRow)o;
            ObjectID = new SqlInt32((int)x["ObjectID"]);
            ColumnID = new SqlInt32((int)x["ColumnID"]);
            ColumnName = new SqlString((string)x["ColumnName"]);
            DataType = new SqlString(x["DataType"].ToString());
            Nullable = new SqlBoolean((bool)x["Nullable"]);
            ReadOnly = new SqlBoolean((bool)x["ReadOnly"]);
            CollationName = new SqlString(x["CollationName"].ToString());
            PrimaryKeyOrder = new SqlInt32((int)x["PrimaryKeyOrder"]);
        }
        [SqlProcedure]
        public static void GetDependency(SqlString ConnectionString, SqlString ObjectName)
        {
            SqlDataRecord record = new SqlDataRecord(
                new SqlMetaData("SchemaName", SqlDbType.NVarChar, 128),
                new SqlMetaData("ObjectName", SqlDbType.NVarChar, 128),
                new SqlMetaData("ObjectType", SqlDbType.NVarChar, 128),
                new SqlMetaData("ObjectTypeDescription", SqlDbType.NVarChar, 128),
                new SqlMetaData("ReferencedServerName", SqlDbType.NVarChar, 128),
                new SqlMetaData("ReferencedDatabaseName", SqlDbType.NVarChar, 128),
                new SqlMetaData("ReferencedSchemaName", SqlDbType.NVarChar, 128),
                new SqlMetaData("ReferencedObjectName", SqlDbType.NVarChar, 128),
                new SqlMetaData("ReferencedColumnName", SqlDbType.NVarChar, 128),
                new SqlMetaData("Error", SqlDbType.NVarChar, 128)
                );
            using (SqlConnection connection = new SqlConnection(ConnectionString.Value))
            {
                connection.Open();
                using(SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"select object_schema_name(o.object_id) SchemaName, object_name(o.object_id) ObjectName, rtrim(type) ObjectType, type_desc ObjectTypeDescription, 
isnull(stuff(replicate(',default', (select count(*) from sys.parameters p where p.object_id = o.object_id)), 1, 1, ''), '' )Parameters,
quotename(object_schema_name(o.object_id)) + '.' + quotename(object_name(o.object_id)) FullName
from sys.all_objects o
where o.type not in ('AF', 'D', 'F', 'IT', 'PC', 'PK', 'S', 'SN', 'U', 'UQ', 'X', 'TR', 'SQ', 'FS') 
	and object_schema_name(o.object_id) not in('sys', 'INFORMATION_SCHEMA')
	and (@ObjectName is null or o.object_id = object_id(@ObjectName) )";
                    cmd.Parameters.Add("@ObjectName", SqlDbType.NVarChar, 128).Value = ObjectName;
                    DataTable t = new DataTable();
                    (new SqlDataAdapter(cmd)).Fill(t);
                    cmd.Parameters.Clear();
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "SET SHOWPLAN_XML ON";
                    cmd.ExecuteNonQuery();

                    SqlContext.Pipe.SendResultsStart(record);
                    foreach(DataRow row in t.Rows)
                    {
                        
                        cmd.CommandText = "";
                        record.SetValue(0, row[0]);
                        record.SetValue(1, row[1]);
                        record.SetValue(2, row[2]);
                        record.SetValue(3, row[3]);
                        record.SetValue(4,DBNull.Value); //ReferencedServerName
                        record.SetValue(5,DBNull.Value); //ReferencedDatabaseName
                        record.SetValue(6,DBNull.Value); //ReferencedSchemaName
                        record.SetValue(7,DBNull.Value); //ReferencedObjectName
                        record.SetValue(8,DBNull.Value); //ReferencedColumnName
                        record.SetValue(9,DBNull.Value); //Error
                        switch(row[2].ToString().ToUpper())
                        {
                            case "FN"://SQL_SCALAR_FUNCTION
                                cmd.CommandText = "select " + row["FullName"].ToString() + "(" + row["Parameters"].ToString() + ")";
                                break;
                            case "IF"://SQL_INLINE_TABLE_VALUED_FUNCTION
                                cmd.CommandText = "select * from " + row["FullName"].ToString() + "(" + row["Parameters"].ToString() + ") a";
                                break;
                            case "P": //SQL_STORED_PROCEDURE
                                cmd.CommandText = "exec " + row["FullName"].ToString() + " " + row["Parameters"].ToString();
                                break;
                            case "TF"://SQL_TABLE_VALUED_FUNCTION
                                cmd.CommandText = "select * from " + row["FullName"].ToString() + "(" + row["Parameters"].ToString() + ") a";
                                break;
                            case "V"://VIEW
                                cmd.CommandText = "select * from " + row["FullName"].ToString() + " a";
                                break;
                        }
                        try
                        {
                            Action<XElement> traverse = null;
                            
                            traverse = (x) =>
                            {
                                if(x.Name=="ColumnReference")
                                {
                                    record.SetValue(4, DBNull.Value); //ReferencedServerName
                                    record.SetValue(5, DBNull.Value); //ReferencedDatabaseName
                                    record.SetValue(6, DBNull.Value); //ReferencedSchemaName
                                    record.SetValue(7, DBNull.Value); //ReferencedObjectName
                                    record.SetValue(8, DBNull.Value); //ReferencedColumnName
                                    record.SetValue(9, DBNull.Value); //Error
                                    bool hasTableName = false;
                                     //<ColumnReference Server="[SDASQLPROD]" Database="[VerificationManager]" Schema="[dbo]" Table="[Audit]" Column="ID" />
                                    foreach(XAttribute a in x.Attributes())
                                    {
                                        if (a.Name == "Server")
                                            record.SetValue(4, a.Value);
                                        if (a.Name == "Database")
                                            record.SetValue(5, a.Value);
                                        if (a.Name == "Schema")
                                            record.SetValue(6, a.Value);
                                        if (a.Name == "Table")
                                        {
                                            record.SetValue(7, a.Value);
                                            hasTableName = true;
                                        }
                                        if (a.Name == "Column")
                                            record.SetValue(8, a.Value);
                                    }
                                    if (hasTableName)
                                        SqlContext.Pipe.SendResultsRow(record);
                                }
                                foreach(XElement x1 in x.Elements())
                                {
                                    traverse(x1);
                                }
                            };
                            traverse(XElement.Parse(cmd.ExecuteScalar().ToString().Replace("xmlns=\"http://schemas.microsoft.com/sqlserver/2004/07/showplan\"", "")));
                            
                        }
                        catch(Exception e)
                        {
                            record.SetValue(9, e.Message);
                            SqlContext.Pipe.SendResultsRow(record);
                        }
                        
                    }
                    SqlContext.Pipe.SendResultsEnd();
                }
            }
        } 
        [SqlFunction(DataAccess=DataAccessKind.Read)]
        public static SqlChars GetQueryPlan(SqlString ConnectionString, SqlString Query)
        {
            using(SqlConnection connection = new SqlConnection(ConnectionString.Value))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "SET SHOWPLAN_XML ON";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = Query.Value;
                    return new SqlChars(cmd.ExecuteScalar().ToString().ToCharArray());
                }
                catch
                {
                    return SqlChars.Null;
                }
            }
        }
        public static List<string[]> GetReferencedColumnsInternal(XElement queryPlanElement, List<string[]> columns = null)
        {
            if(columns == null)
            {
                columns = new List<string[]>();
            }
            if (queryPlanElement.Name == "ColumnReference")
            {
                string[] item = new string[6]{"","","","","",""};
                //ReferencedServerName
                //ReferencedDatabaseName
                //ReferencedSchemaName
                //ReferencedObjectName
                //ReferencedColumnName
                //Error
                bool hasTableName = false;
                foreach (XAttribute a in queryPlanElement.Attributes())
                {
                    if (a.Name == "Server")
                        item[0] = a.Value;
                    if (a.Name == "Database")
                        item[1] = a.Value;
                    if (a.Name == "Schema")
                        item[2] = a.Value;
                    if (a.Name == "Table")
                    {
                        item[3] = a.Value;
                        hasTableName = true;
                    }
                    if (a.Name == "Column")
                        item[4] = a.Value;
                }
                if (hasTableName)
                    columns.Add(item);
            }
            foreach (XElement x1 in queryPlanElement.Elements())
            {
                GetReferencedColumnsInternal(x1, columns);
            }
            return columns;
        }

        [SqlFunction(DataAccess = DataAccessKind.None, 
            FillRowMethodName = "GetReferencedColumns_FillRow", 
            TableDefinition = "ServerName nvarchar(128), DatabaseName nvarchar(128), SchemaName nvarchar(128), TableName nvarchar(128), ColumnName nvarchar(128)"
            )]
        public static IEnumerable GetReferencedColumns(SqlChars QueryPlan)
        {
            return GetReferencedColumnsInternal(XElement.Parse((new string(QueryPlan.Value)).Replace("xmlns=\"http://schemas.microsoft.com/sqlserver/2004/07/showplan\"", "")));
        }
        public static void GetReferencedColumns_FillRow (object obj, out SqlString ServerName, out SqlString DatabaseName, out SqlString SchemaName,out SqlString TableName, out SqlString ColumnName)
        {
            string[] columns = (string[])obj;
            ServerName      = columns[0].Trim() == "" ? SqlString.Null : new SqlString(columns[0]);
            DatabaseName    = columns[1].Trim() == "" ? SqlString.Null : new SqlString(columns[1]);
            SchemaName      = columns[2].Trim() == "" ? SqlString.Null : new SqlString(columns[2]);
            TableName       = columns[3].Trim() == "" ? SqlString.Null : new SqlString(columns[3]);
            ColumnName      = columns[4].Trim() == "" ? SqlString.Null : new SqlString(columns[4]);
        }
    }
}
