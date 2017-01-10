using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Base
{
    namespace Data
    {
        public enum CursorLocation{Client, Server}
        public enum CursorType {Dynamic, KeySet, Static, ForwardOnly}
        public static class CommonProcedures
        {
            public static string SqlParameterToDefinition(SqlParameter param)
            {
                if (param.ParameterName == "")
                    throw new APICursorParameterException();
                if (param.ParameterName.Substring(0, 1).CompareTo("@") != 0)
                    throw new APICursorParameterException();
                if (param.SqlDbType == SqlDbType.Udt)
                    throw new APICursorParameterException("Does not support User Defined Data Type.");

                string ret = param.ParameterName + " " + param.SqlDbType.ToString().ToUpper();
                if (param.SqlDbType == SqlDbType.Decimal)
                    ret = ret + "(" + param.Precision.ToString() + "," + param.Scale.ToString() + ")";
                else if (param.SqlDbType == SqlDbType.Binary ||
                        param.SqlDbType == SqlDbType.Char ||
                        param.SqlDbType == SqlDbType.NChar ||
                        param.SqlDbType == SqlDbType.NText ||
                        param.SqlDbType == SqlDbType.NVarChar ||
                        param.SqlDbType == SqlDbType.VarBinary ||
                        param.SqlDbType == SqlDbType.VarChar)
                    ret = ret + "(" + param.Size.ToString() + ")";
                if (param.Direction == ParameterDirection.Output || param.Direction == ParameterDirection.InputOutput)
                    ret = ret + " OUTPUT";
                return ret;
            }
            public static string SqlParameterToParam(SqlParameter param)
            {
                if (param.ParameterName == "")
                    throw new APICursorParameterException();
                if (param.ParameterName.Substring(0, 1).CompareTo("@") != 0)
                    throw new APICursorParameterException();
                if (param.SqlDbType == SqlDbType.Udt)
                    throw new APICursorParameterException("Does not support User Defined Data Type.");
                string ret = param.ParameterName + "=" + param.ParameterName;
                if (param.Direction == ParameterDirection.Output || param.Direction == ParameterDirection.InputOutput)
                    ret = ret + " OUTPUT";
                return ret;
            }
            public static SqlDataReader RunSqlWithDataReader(SqlConnection connection, int timeout, string sql)
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandTimeout = timeout;
                return cmd.ExecuteReader();
            }
            public static SqlDataReader RunSqlWithDataReader(SqlConnection connection, int timeout, StringBuilder sql)
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql.ToString();
                cmd.CommandTimeout = timeout;
                return cmd.ExecuteReader();
            }
            public static SqlDataReader RunSqlWithDataReader(SqlConnection connection, string sql)
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                return cmd.ExecuteReader();
            }
            public static SqlDataReader RunSqlWithDataReader(SqlConnection connection, StringBuilder sql)
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql.ToString();
                return cmd.ExecuteReader();
            }
            public static DataTable RunSqlWithDataTable(SqlConnection connection, int timeout, string sql)
            {
                DataTable ret = new DataTable();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandTimeout = timeout;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ret);
                return ret;
            }
            public static DataTable RunSqlWithDataTable(SqlConnection connection, int timeout, StringBuilder sql)
            {
                DataTable ret = new DataTable();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql.ToString();
                cmd.CommandTimeout = timeout;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ret);
                return ret;
            }
            public static DataTable RunSqlWithDataTable(SqlConnection connection, string sql)
            {
                DataTable ret = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(sql, connection);
                da.Fill(ret);
                return ret;
            }
            public static DataTable RunSqlWithDataTable(SqlConnection connection, StringBuilder sql)
            {
                DataTable ret = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(sql.ToString(), connection);
                da.Fill(ret);
                return ret;
            }
            public static DataSet RunSqlWithDataSet(SqlConnection connection, int timeout, string sql)
            {
                DataSet ret = new DataSet();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandTimeout = timeout;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ret);
                return ret;
            }
            public static DataSet RunSqlWithDataSet(SqlConnection connection, int timeout, StringBuilder sql)
            {
                DataSet ret = new DataSet();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql.ToString();
                cmd.CommandTimeout = timeout;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ret);
                return ret;
            }
            public static DataSet RunSqlWithDataSet(SqlConnection connection, string sql)
            {
                DataSet ret = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(sql, connection);
                da.Fill(ret);
                return ret;
            }
            public static DataSet RunSqlWithDataSet(SqlConnection connection, StringBuilder sql)
            {
                DataSet ret = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(sql.ToString(), connection);
                da.Fill(ret);
                return ret;
            }
            public static void RunSql(SqlConnection connection, int timeout, string sql)
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandTimeout = timeout;
                cmd.ExecuteNonQuery();
                return;
            }
            public static void RunSql(SqlConnection connection, int timeout, StringBuilder sql)
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql.ToString();
                cmd.CommandTimeout = timeout;
                cmd.ExecuteNonQuery();
                return;
            }
            public static void RunSql(SqlConnection connection, string sql)
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                return;
            }
            public static void RunSql(SqlConnection connection, StringBuilder sql)
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql.ToString();
                cmd.ExecuteNonQuery();
                return;
            }
            public static string QuoteSqlColumn(string column)
            {
                if (column.Contains("["))
                    return column;
                if (column.Contains("]"))
                    return column;
                return "[" + column + "]";
            }
        }
        public enum APICursorScrollOption : int
        {
            KeySetCursor = 0x0001,
            DynamicCursor = 0x0002,
            ForwardOnlyCursor = 0x0004,
            FastForwardOnlyCursor = 0x0010,
            ParameterizedQuery = 0x1000,
            AutoFetch = 0x2000,
            AutoClose = 0x4000,
            CheckAcceptableTypes = 0x8000,
            KeysetAcceptable = 0x10000,
            DynamicAcceptable = 0x20000,
            ForwardAcceptable = 0x40000,
            StaticAcceptable = 0x80000,
            FastForwardAcceptable = 0x100000
        }
        public enum APICursorConcurrencyOption : int
        {
            ReadOnly = 0x0001,
            ScrollLocks = 0x0002,
            OptimisticTimeStamps = 0x0004,
            OptimisticValues = 0x0008,
            OpenOnAnySql = 0x2000,
            UpdateKeySet = 0x4000,
            ReadOnlyAcceptable = 0x10000,
            LocksAcceptable = 0x20000,
            OptimisticAcceptable = 0x40000
        }
        public enum APICursorFetchType : int
        {
            First = 0x0001,
            Next = 0x0002,
            Previous = 0x0004,
            Last = 0x0008,
            AbsoluteRowIndex = 0x0010,
            RelativeRowIndex = 0x0020,
            //ByValue                 = 0x0040,
            Refresh = 0x0080,
            //ResultSetInfo           = 0x0100,
            //PreviousNoAdjust        = 0x0200,
            //SkipUpdateConcurrency   = 0x0400
        }
        public enum APICursorOption : int
        {
            ReturnTextPTR = 1,
            SetCursorName = 2
        }
        public enum APICursor : int
        {
            //Update1                 = 1,
            Insert = 4,
            Update = 33,
            Delete = 34
        }
        public class APICursorException : Exception
        {
            public APICursorException() : base("Could not open cursor.") { }
            public APICursorException(string message) : base(message) { }
        }
        public class APICursorParameterException : Exception
        {
            public APICursorParameterException() : base("Parameters must be named") { }
            public APICursorParameterException(string message) : base(message) { }
        }
        public class APICursorExecutionEngine
        {
            bool isRunning;
            SqlConnection connection;
            StateChangeEventHandler connectionStateChangeHandler;
            APICursorScrollOption cursorScrollOption;
            APICursorConcurrencyOption cursorConcurrencyOption;
            SqlTransaction transaction;
            string commandText;
            int commandTimeout;
            CommandType commandType;
            SqlParameterCollection parameters;
            SqlCommand parametersCommand;
            int cursorHandle;
            string cursorName;
            string updateTable;
            int rowCount, batchSize;
            DataTable schema, cursor, cursorTables, cursorColumns, data;
            long identity;
            public SqlConnection Connecton
            {
                get { return connection; }
                set
                {
                    if (isRunning)
                        throw new APICursorException("Could not change Connection before cursor is closed.");
                    if (value == connection)
                        return;
                    if (connection != null)
                        connection.StateChange -= connectionStateChangeHandler;
                    connection = value;
                    if (connection != null)
                        connection.StateChange += connectionStateChangeHandler;

                }
            }
            public APICursorScrollOption CursorScrollOption
            {
                get { return cursorScrollOption; }
                set
                {
                    if (isRunning)
                        throw new APICursorException("Could not change CursorScrollOption before cursor is closed.");
                    cursorScrollOption = value;
                }
            }
            public APICursorConcurrencyOption CursorConcurrencyOption
            {
                get { return cursorConcurrencyOption; }
                set
                {
                    if (isRunning)
                        throw new APICursorException("Could not change CursorConcurrencyOption before cursor is closed.");
                    cursorConcurrencyOption = value;
                }
            }
            public SqlTransaction Transaction
            {
                get { return transaction; }
                set { transaction = value; }
            }
            public string CommandText
            {
                get { return commandText; }
                set
                {
                    if (isRunning)
                        throw new APICursorException("Could not change CommandText before cursor is closed.");
                    commandText = value;
                }
            }
            public int CommandTimeout
            {
                get { return commandTimeout; }
                set { commandTimeout = value; }
            }
            public CommandType CommandType
            {
                get { return commandType; }
                set
                {
                    if (isRunning)
                        throw new APICursorException("Could not change CommandType before cursor is closed.");
                    commandType = value;
                }
            }
            public SqlParameterCollection Parameters
            {
                get { return parameters; }
            }
            public int CursorHandle
            {
                get { return cursorHandle; }
            }
            public string CursorName
            {
                get
                {
                    return cursorName;
                }
                set
                {
                    cursorName = value;
                    if (!IsActive())
                        return;
                    CommonProcedures.RunSql(connection, "exec sp_cursoroption " + cursorHandle.ToString() + ", 2, N'" + cursorName.Replace("'", "''") + "'");
                }
            }
            public string UpdateTable
            {
                get { return updateTable; }
                set { updateTable = value; }
            }
            public bool Active
            {
                get { return IsActive(); }
                set
                {
                    if (isRunning && !value)
                        Close();
                    else if (!isRunning && value)
                        Open();
                }
            }
            public int RowCount
            {
                get { return rowCount; }
            }
            public int BatchSize
            {
                get { return batchSize; }
                set
                {
                    batchSize = value < 1 ? 1 : value;
                }
            }
            public DataTable Sechema
            {
                get { return schema; }
            }
            public DataTable Cursor
            {
                get { return cursor; }
            }
            public DataTable CursorTables
            {
                get { return cursorTables; }
            }
            public DataTable CursorColumns
            {
                get { return cursorColumns; }
            }
            public DataTable Data
            {
                get { return data; }
            }
            public long Identity
            {
                get { return identity; }
            }
            private string ParameterToSqlDefinition()
            {
                throw new Exception("ParameterToSqlDefinition has not been implement yet");
            }
            private bool IsActive()
            {
                return isRunning && cursorHandle != 0 && connection != null;
            }
            private void ConnectionStateChange(object sender, StateChangeEventArgs e)
            {
                if (e.CurrentState == ConnectionState.Closed || e.CurrentState == ConnectionState.Broken)
                    Close();
            }
            public APICursorExecutionEngine()
            {
                isRunning = false;
                cursorScrollOption = APICursorScrollOption.DynamicCursor;
                cursorConcurrencyOption = APICursorConcurrencyOption.OptimisticValues;
                transaction = null;
                batchSize = 20;
                rowCount = 0;
                data = null;
                identity = -1;
                commandTimeout = 30;
                commandType = CommandType.Text;
                connectionStateChangeHandler = new System.Data.StateChangeEventHandler(this.ConnectionStateChange);
                connection = null;
                parametersCommand = new SqlCommand();
                parameters = parametersCommand.Parameters;
            }
            public APICursorExecutionEngine(SqlConnection connection)
                : this()
            {
                Connecton = connection;
            }
            public APICursorExecutionEngine(string sql, SqlConnection connection)
                : this(connection)
            {
                commandText = sql;
            }
            ~APICursorExecutionEngine()
            {
                Close();
            }
            public DataTable Fetch(APICursorFetchType fetchType, int rownum, int nrows)
            {
                data = CommonProcedures.RunSqlWithDataTable(connection, commandTimeout, "exec sp_cursorfetch " + cursorHandle.ToString() + ", " + ((int)fetchType).ToString() + ", " + rownum.ToString() + ", " + nrows.ToString());
                return data;
            }
            public void MoveTo(APICursorFetchType fetchType, int rownum)
            {
                if (fetchType == APICursorFetchType.AbsoluteRowIndex || fetchType == APICursorFetchType.RelativeRowIndex)
                    CommonProcedures.RunSql(connection, commandTimeout, "exec sp_cursorfetch " + cursorHandle.ToString() + ", " + ((int)fetchType).ToString() + ", " + rownum.ToString() + ", 0");
                return;
            }
            public void CursorOperation(APICursor op, SqlParameterCollection param, SqlTransaction trans)
            {
                CursorOperation(op, (IList)param, trans);
            }
            public void CursorOperation(APICursor op, IList param, SqlTransaction trans)
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sb1 = new StringBuilder();
                string s;
                SqlParameter p;
                SqlCommand cmd = connection.CreateCommand();
                if (trans != null)
                    cmd.Transaction = trans;
                cmd.CommandTimeout = commandTimeout;
                switch (op)
                {
                    case APICursor.Insert:
                        sb.Append("insert into " + updateTable + " (");
                        for (int i = 0; i < param.Count; i++)
                        {
                            p = (SqlParameter)param[i];
                            s = p.ParameterName;
                            if (s.Substring(0, 1) == "@")
                                s.Remove(0, 1);
                            s = CommonProcedures.QuoteSqlColumn(s);
                            sb.Append(s + ",");
                            if (param == null)
                            {
                                sb1.Append("null,");
                            }
                            else
                            {
                                sb1.Append("@P" + i.ToString() + ",");
                                SqlParameter p1 = new SqlParameter("@P" + i.ToString(), p.SqlDbType, p.Size, ParameterDirection.Input, p.Precision, p.Scale, p.SourceColumn, p.SourceVersion, p.SourceColumnNullMapping, p.Value, p.XmlSchemaCollectionDatabase, p.XmlSchemaCollectionOwningSchema, p.XmlSchemaCollectionOwningSchema);
                                p1.Value = p.Value;
                                cmd.Parameters.Add(p1);
                            }
                        }

                        sb.Remove(sb.Length - 1, 1);
                        sb1.Remove(sb1.Length - 1, 1);
                        sb.Append(") values (");
                        sb.Append(sb1.ToString());
                        sb.Append(")\n");
                        sb.Append("select @id = scope_identity()");
                        p = new SqlParameter("@id", SqlDbType.BigInt);
                        p.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(p);
                        cmd.CommandText = sb.ToString();
                        cmd.ExecuteNonQuery();
                        identity = (long)cmd.Parameters[cmd.Parameters.Count - 1].Value;
                        break;
                    case APICursor.Delete:
                        cmd.CommandText = "delete from  " + updateTable + " where current of " + CursorName;
                        cmd.ExecuteNonQuery();
                        break;
                    case APICursor.Update:
                        sb.Append("update " + updateTable + " set ");
                        for (int i = 0; i < param.Count; i++)
                        {
                            p = (SqlParameter)param[i];
                            s = p.ParameterName;
                            if (s.Substring(0, 1) == "@")
                                s.Remove(0, 1);
                            s = CommonProcedures.QuoteSqlColumn(s);
                            sb.Append(s + " = ");
                            if (param == null)
                            {
                                sb.Append("null,");
                            }
                            else
                            {
                                sb.Append("@P" + i.ToString() + ",");
                                SqlParameter p1 = new SqlParameter("@P" + i.ToString(), p.SqlDbType, p.Size, ParameterDirection.Input, p.Precision, p.Scale, p.SourceColumn, p.SourceVersion, p.SourceColumnNullMapping, p.Value, p.XmlSchemaCollectionDatabase, p.XmlSchemaCollectionOwningSchema, p.XmlSchemaCollectionOwningSchema);
                                p1.Value = p.Value;
                                cmd.Parameters.Add(p1);
                            }
                        }

                        sb.Remove(sb.Length - 1, 1);
                        sb.Append(" where current of " + CursorName);
                        cmd.CommandText = sb.ToString();
                        cmd.ExecuteNonQuery();
                        break;
                    default:
                        break;
                }
                return;
            }
            public void Open()
            {
                if (isRunning || CommandText == "")
                    return;
                if (connection.State == ConnectionState.Broken || connection.State == ConnectionState.Closed)
                    throw new APICursorException("Could not open a cursor on a disconnected connection.");
                StringBuilder sql = new StringBuilder();
                sql.Append("declare @cursor int, @scrollopt int, @ccopt int, @rowcount int, @returnvalue int \n");
                sql.Append("select @scrollopt=" + ((int)cursorScrollOption).ToString() + ",");
                sql.Append("@ccopt=" + ((int)cursorConcurrencyOption).ToString() + ",");
                sql.Append("@rowcount=" + batchSize.ToString());
                sql.Append("\nexec @returnvalue = sp_cursoropen @cursor output,N'");
                if (CommandType == CommandType.TableDirect)
                {
                    sql.Append("select * from " + commandText.Replace("'", "''"));
                }
                else if (CommandType == CommandType.StoredProcedure)
                {
                    sql.Append("exec @ReturnValue=" + commandText.Replace("'", "''"));
                }
                else
                {
                    sql.Append(CommandText.Replace("'", "''"));
                }
                sql.Append("',@scrollopt output, @ccopt output, @rowcount output \n");

                if (Parameters.Count > 0)
                {
                    StringBuilder paramDef = new StringBuilder();
                    StringBuilder param = new StringBuilder();
                    paramDef.Append("N'");
                    foreach (SqlParameter p in parameters)
                    {
                        paramDef.Append(CommonProcedures.SqlParameterToDefinition(p));
                        paramDef.Append(",");
                        param.Append(",");
                        param.Append(CommonProcedures.SqlParameterToParam(p));
                    }
                    paramDef.Remove(paramDef.Length - 1, 1);
                    paramDef.Append("'");
                    sql.Append(",");
                    sql.Append(paramDef);
                    sql.Append(param);
                }
                sql.Append("\nselect @cursor as [cursor], @scrollopt as scrollopt, @ccopt as ccopt, @rowcount as [rowcount], @returnvalue returnvalue\n");

                parametersCommand.Connection = connection;
                parametersCommand.CommandText = sql.ToString();
                parametersCommand.CommandTimeout = CommandTimeout;
                SqlDataReader r = parametersCommand.ExecuteReader();
                schema = r.GetSchemaTable();
                if (!r.NextResult())
                    throw new APICursorException();
                try
                {
                    if (!r.Read())
                        throw new APICursorException();
                    if ((int)r[4] != 0)
                        throw new APICursorException();
                    cursorHandle = (int)r[0];
                    if (cursorHandle == 0)
                        throw new APICursorException();
                    cursorScrollOption = (APICursorScrollOption)((int)r[1]);
                    cursorConcurrencyOption = (APICursorConcurrencyOption)((int)r[2]);
                    rowCount = (int)r[3];
                    isRunning = true;
                    r.Close();
                    r = CommonProcedures.RunSqlWithDataReader(connection, commandTimeout, "select 'C'+rtrim(cast(host_id() as varchar(100))) + '_'+ cast(db_id() as varchar(20))+'_'+ cast(@@spid as varchar(20))+ '_'+replace(cast(newid() as varchar(50)),'-','') as cursor_name");
                    if (!r.Read())
                        throw new APICursorException("Could not get cursor name.");
                    cursorName = r[0].ToString().Trim();
                    r.Close();
                    CursorName = cursorName;
                    DescribeCursor();
                    DescribeCursorTables();
                    DescribeCursorColumns();
                    isRunning = true;
                }
                finally
                {
                    if (!r.IsClosed)
                        r.Close();
                }
            }
            public void Close()
            {
                try
                {
                    if (!IsActive())
                        return;
                    CommonProcedures.RunSql(connection, commandTimeout, "close " + cursorName + "\n deallocate " + cursorName);
                }
                finally
                {
                    if (schema != null)
                        schema.Dispose();
                    if (cursor != null)
                        cursor.Dispose();
                    if (cursorTables != null)
                        cursorTables.Dispose();
                    if (cursorColumns != null)
                        cursorColumns.Dispose();
                    if (data != null)
                        data.Dispose();
                    data = null;
                    schema = null;
                    cursor = null;
                    cursorTables = null;
                    cursorColumns = null;
                    isRunning = false;
                    cursorHandle = 0;
                    cursorName = "";
                    GC.Collect();
                }
                return;
            }
            public DataTable First(int rows)
            {
                return Fetch(APICursorFetchType.First, rows, rows); ;
            }
            public DataTable First()
            {
                return First(batchSize);
            }
            public DataTable Next(int rows)
            {
                return Fetch(APICursorFetchType.Next, rows, rows); ;
            }
            public DataTable Next()
            {
                return Next(batchSize);
            }
            public DataTable Previous(int rows)
            {
                return Fetch(APICursorFetchType.Previous, rows, rows); ;
            }
            public DataTable Previous()
            {
                return Previous(batchSize);
            }
            public DataTable Last(int rows)
            {
                return Fetch(APICursorFetchType.Last, rows, rows); ;
            }
            public DataTable Last()
            {
                return Last(batchSize);
            }
            public DataTable Goto(int index, int rows)
            {
                return Fetch(APICursorFetchType.RelativeRowIndex, index, rows);
            }
            public DataTable Goto(int index)
            {
                return Goto(index, batchSize);
            }
            public DataTable GotoAbsolute(int index, int rows)
            {
                return Fetch(APICursorFetchType.AbsoluteRowIndex, index, rows);
            }
            public DataTable GotoAbsolute(int index)
            {
                return GotoAbsolute(index, batchSize);
            }
            public void Insert(IList param, SqlTransaction trans)
            {
                CursorOperation(APICursor.Insert, param, trans);
            }
            public void Insert(SqlParameterCollection param, SqlTransaction trans)
            {
                CursorOperation(APICursor.Insert, param, trans);
            }
            public void Delete(SqlTransaction trans)
            {
                CursorOperation(APICursor.Delete, (IList)null, trans);
            }
            public void Update(IList param, SqlTransaction trans)
            {
                CursorOperation(APICursor.Update, param, trans);
            }
            public void Update(SqlParameterCollection param, SqlTransaction trans)
            {
                CursorOperation(APICursor.Update, param, trans);
            }
            public void Refresh()
            {
                Fetch(APICursorFetchType.Refresh, 0, 0);
            }
            public DataTable DescribeCursor()
            {
                if (!IsActive())
                {
                    cursor = null;
                    return null;
                }
                DataTable ret = APICursorExecutionEngine.DescribeCursor(connection, commandTimeout, cursorName);
                cursor = ret;
                return ret;
            }
            public DataTable DescribeCursorTables()
            {
                if (!IsActive())
                {
                    cursorTables = null;
                    return null;
                }
                DataTable ret = APICursorExecutionEngine.DescribeCursorTables(connection, commandTimeout, cursorName);
                cursorTables = ret;
                return ret;
            }
            public DataTable DescribeCursorColumns()
            {
                if (!IsActive())
                {
                    cursorColumns = null;
                    return null;
                }
                DataTable ret = APICursorExecutionEngine.DescribeCursorColumns(connection, commandTimeout, cursorName);
                cursorColumns = ret;
                return ret;
            }


            public static DataTable ListCursors(SqlConnection connection)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("declare @cursor cursor");
                sb.AppendLine("declare @cursors table(reference_name nvarchar(128), cursor_name nvarchar(128), cursor_scope tinyint, status int, model tinyint, concurrency tinyint, scrollable tinyint, open_status tinyint, cursor_rows decimal(10, 0), fetch_status smallint, column_count smallint, row_count decimal(10, 0), last_operation tinyint, cursor_handle int)");
                sb.AppendLine("declare @cursor_name nvarchar(128), @reference_name nvarchar(128), @cursor_scope tinyint, @status int, @model tinyint, @concurrency tinyint, @scrollable tinyint, @open_status tinyint, @cursor_rows decimal(10, 0), @fetch_status smallint, @column_count smallint, @row_count decimal(10, 0), @last_operation tinyint, @cursor_handle int");
                sb.AppendLine("exec master.dbo.sp_cursor_list @cursor_return = @cursor output, @cursor_scope = 3");
                sb.AppendLine("fetch next from @cursor into @reference_name, @cursor_name, @cursor_scope, @status, @model, @concurrency, @scrollable, @open_status, @cursor_rows, @fetch_status, @column_count, @row_count, @last_operation, @cursor_handle");
                sb.AppendLine("while (@@fetch_status = 0)");
                sb.AppendLine("begin");
                sb.AppendLine("	insert into @cursors(reference_name, cursor_name, cursor_scope, status, model, concurrency, scrollable, open_status, cursor_rows, fetch_status, column_count, row_count, last_operation, cursor_handle)");
                sb.AppendLine("		values(@reference_name, @cursor_name, @cursor_scope, @status, @model, @concurrency, @scrollable, @open_status, @cursor_rows, @fetch_status, @column_count, @row_count, @last_operation, @cursor_handle)");
                sb.AppendLine("	fetch next from @cursor into @reference_name, @cursor_name, @cursor_scope, @status, @model, @concurrency, @scrollable, @open_status, @cursor_rows, @fetch_status, @column_count, @row_count, @last_operation, @cursor_handle");
                sb.AppendLine("end");
                sb.AppendLine("select reference_name, cursor_name, cursor_scope, case cursor_scope when 1 then 'local' else 'global' end as cursor_scope_str, status, case status when 1 then 'rows' when 0 then 'norows' when -1 then 'closed' when -2 then 'nocursor' else 'notexists' end as status_str, model, case model when 1 then 'static' when 2 then 'keyset' when 3 then 'dynamic' else 'fastforward' end as mode_str, concurrency, case concurrency when 1 then 'readonly' when 2 then 'scrolllocks' else 'optimistic' end as concurrency_str, scrollable, case scrollable when 0 then 'forwardonly' else 'scrollable' end as scrollable_str,open_status, case open_status when 0 then 'closed' else 'open' end as open_status_str, cursor_rows, fetch_status, case fetch_status when 0 then 'successful' when -1 then 'failed' when '-1' then 'missing' else 'norecord' end as fetch_status_str, column_count, row_count, last_operation, case last_operation when 0 then 'nothing' when 1 then 'open' when 2 then 'fetch' when 3 then 'insert' when 4 then 'update' when 5 then 'delete' when 6 then 'close' else 'deallocate' end last_operation_str, cursor_handle from @cursors a");
                return CommonProcedures.RunSqlWithDataTable(connection, sb);
            }
            public static DataTable DescribeCursor(SqlConnection connection, int timeout, string cursorName)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("declare @cursor cursor");
                sb.AppendLine("declare @cursors table(reference_name nvarchar(128), cursor_name nvarchar(128), cursor_scope tinyint, status int, model tinyint, concurrency tinyint, scrollable tinyint, open_status tinyint, cursor_rows decimal(10, 0), fetch_status smallint, column_count smallint, row_count decimal(10, 0), last_operation tinyint, cursor_handle int)");
                sb.AppendLine("declare @cursor_name nvarchar(128),@reference_name nvarchar(128), @cursor_scope tinyint, @status int, @model tinyint, @concurrency tinyint, @scrollable tinyint, @open_status tinyint, @cursor_rows decimal(10, 0), @fetch_status smallint, @column_count smallint, @row_count decimal(10, 0), @last_operation tinyint, @cursor_handle int");
                sb.AppendLine("exec sp_describe_cursor @cursor_return = @cursor output, @cursor_source = 'global', @cursor_identity = N'" + cursorName.Replace("'", "''") + "'");
                sb.AppendLine("fetch next from @cursor into @reference_name, @cursor_name, @cursor_scope, @status, @model, @concurrency, @scrollable, @open_status, @cursor_rows, @fetch_status, @column_count, @row_count, @last_operation, @cursor_handle");
                sb.AppendLine("while (@@fetch_status = 0)");
                sb.AppendLine("begin");
                sb.AppendLine("	insert into @cursors(reference_name, cursor_name, cursor_scope, status, model, concurrency, scrollable, open_status, cursor_rows, fetch_status, column_count, row_count, last_operation, cursor_handle)");
                sb.AppendLine("		values(@reference_name, @cursor_name, @cursor_scope, @status, @model, @concurrency, @scrollable, @open_status, @cursor_rows, @fetch_status, @column_count, @row_count, @last_operation, @cursor_handle)");
                sb.AppendLine("	fetch next from @cursor into @reference_name, @cursor_name, @cursor_scope, @status, @model, @concurrency, @scrollable, @open_status, @cursor_rows, @fetch_status, @column_count, @row_count, @last_operation, @cursor_handle");
                sb.AppendLine("end");
                sb.AppendLine("select reference_name, cursor_name, cursor_scope, case cursor_scope when 1 then 'local' else 'global' end as cursor_scope_str, status, case status when 1 then 'rows' when 0 then 'norows' when -1 then 'closed' when -2 then 'nocursor' else 'notexists' end as status_str, model, case model when 1 then 'static' when 2 then 'keyset' when 3 then 'dynamic' else 'fastforward' end as mode_str, concurrency, case concurrency when 1 then 'readonly' when 2 then 'scrolllocks' else 'optimistic' end as concurrency_str, scrollable, case scrollable when 0 then 'forwardonly' else 'scrollable' end as scrollable_str,open_status, case open_status when 0 then 'closed' else 'open' end as open_status_str, cursor_rows, fetch_status, case fetch_status when 0 then 'successful' when -1 then 'failed' when '-1' then 'missing' else 'norecord' end as fetch_status_str, column_count, row_count, last_operation, case last_operation when 0 then 'nothing' when 1 then 'open' when 2 then 'fetch' when 3 then 'insert' when 4 then 'update' when 5 then 'delete' when 6 then 'close' else 'deallocate' end last_operation_str, cursor_handle from @cursors a");
                return CommonProcedures.RunSqlWithDataTable(connection, timeout, sb);
            }
            public static DataTable DescribeCursor(SqlConnection connection, string cursorName)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("declare @cursor cursor");
                sb.AppendLine("declare @cursors table(reference_name nvarchar(128), cursor_name nvarchar(128), cursor_scope tinyint, status int, model tinyint, concurrency tinyint, scrollable tinyint, open_status tinyint, cursor_rows decimal(10, 0), fetch_status smallint, column_count smallint, row_count decimal(10, 0), last_operation tinyint, cursor_handle int)");
                sb.AppendLine("declare @cursor_name nvarchar(128), @reference_name nvarchar(128), @cursor_scope tinyint, @status int, @model tinyint, @concurrency tinyint, @scrollable tinyint, @open_status tinyint, @cursor_rows decimal(10, 0), @fetch_status smallint, @column_count smallint, @row_count decimal(10, 0), @last_operation tinyint, @cursor_handle int");
                sb.AppendLine("exec sp_describe_cursor @cursor_return = @cursor output, @cursor_source = 'global', @cursor_identity = N'" + cursorName.Replace("'", "''") + "'");
                sb.AppendLine("fetch next from @cursor into @reference_name, @cursor_name, @cursor_scope, @status, @model, @concurrency, @scrollable, @open_status, @cursor_rows, @fetch_status, @column_count, @row_count, @last_operation, @cursor_handle");
                sb.AppendLine("while (@@fetch_status = 0)");
                sb.AppendLine("begin");
                sb.AppendLine("	insert into @cursors(reference_name, cursor_name, cursor_scope, status, model, concurrency, scrollable, open_status, cursor_rows, fetch_status, column_count, row_count, last_operation, cursor_handle)");
                sb.AppendLine("		values(@reference_name, @cursor_name, @cursor_scope, @status, @model, @concurrency, @scrollable, @open_status, @cursor_rows, @fetch_status, @column_count, @row_count, @last_operation, @cursor_handle)");
                sb.AppendLine("	fetch next from @cursor into @reference_name, @cursor_name, @cursor_scope, @status, @model, @concurrency, @scrollable, @open_status, @cursor_rows, @fetch_status, @column_count, @row_count, @last_operation, @cursor_handle");
                sb.AppendLine("end");
                sb.AppendLine("select reference_name, cursor_name, cursor_scope, case cursor_scope when 1 then 'local' else 'global' end as cursor_scope_str, status, case status when 1 then 'rows' when 0 then 'norows' when -1 then 'closed' when -2 then 'nocursor' else 'notexists' end as status_str, model, case model when 1 then 'static' when 2 then 'keyset' when 3 then 'dynamic' else 'fastforward' end as mode_str, concurrency, case concurrency when 1 then 'readonly' when 2 then 'scrolllocks' else 'optimistic' end as concurrency_str, scrollable, case scrollable when 0 then 'forwardonly' else 'scrollable' end as scrollable_str,open_status, case open_status when 0 then 'closed' else 'open' end as open_status_str, cursor_rows, fetch_status, case fetch_status when 0 then 'successful' when -1 then 'failed' when '-1' then 'missing' else 'norecord' end as fetch_status_str, column_count, row_count, last_operation, case last_operation when 0 then 'nothing' when 1 then 'open' when 2 then 'fetch' when 3 then 'insert' when 4 then 'update' when 5 then 'delete' when 6 then 'close' else 'deallocate' end last_operation_str, cursor_handle from @cursors a");
                return CommonProcedures.RunSqlWithDataTable(connection, sb);
            }
            public static DataTable DescribeCursorTables(SqlConnection connection, int timeout, string cursorName)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("declare @cursor cursor");
                sb.AppendLine("declare @tables table (table_owner nvarchar(128), table_name nvarchar(128), optimizer_hint smallint, lock_type smallint, server_name nvarchar(128), objectid int, dbid int, dbname nvarchar(128))");
                sb.AppendLine("declare @table_owner nvarchar(128), @table_name nvarchar(128), @optimizer_hint smallint, @lock_type smallint, @server_name nvarchar(128), @objectid int, @dbid int, @dbname nvarchar(128)");
                sb.AppendLine("exec master.dbo.sp_describe_cursor_tables @cursor_return = @cursor output, @cursor_source = 'global', @cursor_identity = N'" + cursorName.Replace("'", "''") + "'");
                sb.AppendLine("fetch next from @cursor into @table_owner, @table_name, @optimizer_hint, @lock_type, @server_name, @objectid, @dbid, @dbname");
                sb.AppendLine("while (@@fetch_status = 0)");
                sb.AppendLine("begin");
                sb.AppendLine("	insert into @tables (table_owner, table_name, optimizer_hint, lock_type, server_name, objectid, dbid, dbname)");
                sb.AppendLine("		values(@table_owner, @table_name, @optimizer_hint, @lock_type, @server_name, @objectid, @dbid, @dbname)");
                sb.AppendLine("	fetch next from @cursor into @table_owner, @table_name, @optimizer_hint, @lock_type, @server_name, @objectid, @dbid, @dbname");
                sb.AppendLine("end");
                sb.AppendLine("close @cursor");
                sb.AppendLine("deallocate @cursor");
                sb.AppendLine("select * from @tables");
                return CommonProcedures.RunSqlWithDataTable(connection, timeout, sb);
            }
            public static DataTable DescribeCursorTables(SqlConnection connection, string cursorName)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("declare @cursor cursor");
                sb.AppendLine("declare @tables table (table_owner nvarchar(128), table_name nvarchar(128), optimizer_hint smallint, lock_type smallint, server_name nvarchar(128), objectid int, dbid int, dbname nvarchar(128))");
                sb.AppendLine("declare @table_owner nvarchar(128), @table_name nvarchar(128), @optimizer_hint smallint, @lock_type smallint, @server_name nvarchar(128), @objectid int, @dbid int, @dbname nvarchar(128)");
                sb.AppendLine("exec master.dbo.sp_describe_cursor_tables @cursor_return = @cursor output, @cursor_source = 'global', @cursor_identity = N'" + cursorName.Replace("'", "''") + "'");
                sb.AppendLine("fetch next from @cursor into @table_owner, @table_name, @optimizer_hint, @lock_type, @server_name, @objectid, @dbid, @dbname");
                sb.AppendLine("while (@@fetch_status =0)");
                sb.AppendLine("begin");
                sb.AppendLine("	insert into @tables (table_owner, table_name, optimizer_hint, lock_type, server_name, objectid, dbid, dbname)");
                sb.AppendLine("		values(@table_owner, @table_name, @optimizer_hint, @lock_type, @server_name, @objectid, @dbid, @dbname)");
                sb.AppendLine("	fetch next from @cursor into @table_owner, @table_name, @optimizer_hint, @lock_type, @server_name, @objectid, @dbid, @dbname");
                sb.AppendLine("end");
                sb.AppendLine("close @cursor");
                sb.AppendLine("deallocate @cursor");
                sb.AppendLine("select * from @tables");
                return CommonProcedures.RunSqlWithDataTable(connection, sb);
            }
            public static DataTable DescribeCursorColumns(SqlConnection connection, int timeout, string cursorName)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("declare @cursor cursor");
                sb.AppendLine("declare @table_columns table (column_name nvarchar(128), ordinal_position int, column_characteristics_flags int, column_size int, data_type_sql smallint, column_precision tinyint, column_scale tinyint, order_position int, order_direction nvarchar, hidden_column smallint, columnid int, objectid int, dbid int, dbname nvarchar(128))");
                sb.AppendLine("declare @table_owner nvarchar(128), @table_name nvarchar(128), @optimizer_hint smallint, @lock_type smallint, @server_name nvarchar(128)");
                sb.AppendLine("declare @curosr_source nvarchar(30), @column_name nvarchar(128), @ordinal_position int, @column_characteristics_flags int, @column_size int, @data_type_sql smallint, @column_precision tinyint, @column_scale tinyint, @order_position int, @order_direction nvarchar, @hidden_column smallint, @columnid int, @objectid int, @dbid int, @dbname nvarchar(128)");
                sb.AppendLine("select @curosr_source = case when databaseproperty(db_name(), 'islocalcursorsdefault') = 1 then 'local' else 'global' end");
                sb.AppendLine("exec master.dbo.sp_describe_cursor_columns   @cursor_return = @cursor output,  @cursor_source = @curosr_source, @cursor_identity =  N'" + cursorName.Replace("'", "''") + "'");
                sb.AppendLine("fetch next from @cursor into @column_name, @ordinal_position, @column_characteristics_flags, @column_size, @data_type_sql, @column_precision, @column_scale, @order_position, @order_direction, @hidden_column, @columnid, @objectid, @dbid, @dbname");
                sb.AppendLine("while (@@fetch_status =0)");
                sb.AppendLine("begin");
                sb.AppendLine("	insert into @table_columns(column_name, ordinal_position, column_characteristics_flags, column_size, data_type_sql, column_precision, column_scale, order_position, order_direction, hidden_column, columnid, objectid, dbid, dbname)");
                sb.AppendLine("		values(@column_name, @ordinal_position, @column_characteristics_flags, @column_size, @data_type_sql, @column_precision, @column_scale, @order_position, @order_direction, @hidden_column, @columnid, @objectid, @dbid, @dbname)");
                sb.AppendLine("	fetch next from @cursor into @column_name, @ordinal_position, @column_characteristics_flags, @column_size, @data_type_sql, @column_precision, @column_scale, @order_position, @order_direction, @hidden_column, @columnid, @objectid, @dbid, @dbname");
                sb.AppendLine("end");
                sb.AppendLine("close @cursor");
                sb.AppendLine("deallocate @cursor");
                sb.AppendLine("select a.column_name, a.ordinal_position, a.column_characteristics_flags, a.column_size, a.data_type_sql, b.name data_type_sql_str, a.column_precision, a.column_scale, a.order_position, a.order_direction, case lower(a.order_direction) when 'a' then 'asc' when 'd' then 'desc' else 'nothing' end order_direction_str, a.hidden_column, a.columnid, a.objectid, a.dbid, a.dbname from @table_columns a	left outer join systypes b on a.data_type_sql = b.xusertype");
                return CommonProcedures.RunSqlWithDataTable(connection, timeout, sb);
            }
            public static DataTable DescribeCursorColumns(SqlConnection connection, string cursorName)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("declare @cursor cursor");
                sb.AppendLine("declare @table_columns table (column_name nvarchar(128), ordinal_position int, column_characteristics_flags int, column_size int, data_type_sql smallint, column_precision tinyint, column_scale tinyint, order_position int, order_direction nvarchar, hidden_column smallint, columnid int, objectid int, dbid int, dbname nvarchar(128))");
                sb.AppendLine("declare @table_owner nvarchar(128), @table_name nvarchar(128), @optimizer_hint smallint, @lock_type smallint, @server_name nvarchar(128)");
                sb.AppendLine("declare @curosr_source nvarchar(30), @column_name nvarchar(128), @ordinal_position int, @column_characteristics_flags int, @column_size int, @data_type_sql smallint, @column_precision tinyint, @column_scale tinyint, @order_position int, @order_direction nvarchar, @hidden_column smallint, @columnid int, @objectid int, @dbid int, @dbname nvarchar(128)");
                sb.AppendLine("select @curosr_source = case when databaseproperty(db_name(), 'islocalcursorsdefault') = 1 then 'local' else 'global' end");
                sb.AppendLine("exec master.dbo.sp_describe_cursor_columns   @cursor_return = @cursor output,  @cursor_source = @curosr_source, @cursor_identity =  N'" + cursorName.Replace("'", "''") + "'");
                sb.AppendLine("fetch next from @cursor into @column_name, @ordinal_position, @column_characteristics_flags, @column_size, @data_type_sql, @column_precision, @column_scale, @order_position, @order_direction, @hidden_column, @columnid, @objectid, @dbid, @dbname");
                sb.AppendLine("while (@@fetch_status =0)");
                sb.AppendLine("begin");
                sb.AppendLine("	insert into @table_columns(column_name, ordinal_position, column_characteristics_flags, column_size, data_type_sql, column_precision, column_scale, order_position, order_direction, hidden_column, columnid, objectid, dbid, dbname)");
                sb.AppendLine("		values(@column_name, @ordinal_position, @column_characteristics_flags, @column_size, @data_type_sql, @column_precision, @column_scale, @order_position, @order_direction, @hidden_column, @columnid, @objectid, @dbid, @dbname)");
                sb.AppendLine("	fetch next from @cursor into @column_name, @ordinal_position, @column_characteristics_flags, @column_size, @data_type_sql, @column_precision, @column_scale, @order_position, @order_direction, @hidden_column, @columnid, @objectid, @dbid, @dbname");
                sb.AppendLine("end");
                sb.AppendLine("close @cursor");
                sb.AppendLine("deallocate @cursor");
                sb.AppendLine("select a.column_name, a.ordinal_position, a.column_characteristics_flags, a.column_size, a.data_type_sql, b.name data_type_sql_str, a.column_precision, a.column_scale, a.order_position, a.order_direction, case lower(a.order_direction) when 'a' then 'asc' when 'd' then 'desc' else 'nothing' end order_direction_str, a.hidden_column, a.columnid, a.objectid, a.dbid, a.dbname from @table_columns a	left outer join systypes b on a.data_type_sql = b.xusertype");
                return CommonProcedures.RunSqlWithDataTable(connection, sb);
            }
        }
        
    }

}
