using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.SqlServer.Server;

namespace SqlRemoteExecute
{
	public enum Parameters_ConnectionType
	{
		CurrentContext,
		NewConnection,
		NoConnection
	}
	public enum Parameters_CommandError
	{
		None,
		Raise
	}
	public enum Parameter_TransactionType
	{
		CurrentContext,
		NewTransaction,
		NoTransaction
	}
	public enum Parameters_TransactionAction
	{
		NoAction,
		CommitTransaction,
		RollBackTransaction
	}
	public enum Parameters_ReturnType
	{
		Console,
		Event,
		None
	}
	public enum Parameters_EventError
	{
		IgnoreError,
		StopProcess
	}
	public class Parameters_Connection
	{
		public Parameters_ConnectionType ConnectionType;
		public string ConnectionString;
		public int CommandTimeout;
		public void Validate()
		{
			ConnectionString = ConnectionString.Trim();
			switch (ConnectionType)
			{
				case Parameters_ConnectionType.CurrentContext:
                    ConnectionString = "Context Connection=true;";
					break;
				case Parameters_ConnectionType.NewConnection:
					if (ConnectionString == "")
						throw new Exception("Connection string is not specified.");
					break;
				default:
					ConnectionString = "";
					break;
			}
		}
		#region Parameters_Connection Constructure
		public Parameters_Connection(SqlString ConnectionType,
			SqlString ConnectionString,
			SqlInt32 CommandTimeout)
		{
			this.ConnectionType = (Parameters_ConnectionType)Enum.Parse(typeof(Parameters_ConnectionType), ConnectionType.Value.Trim(), true);
			this.ConnectionString = ConnectionString.ToString().Trim();
			if (this.ConnectionString.LastIndexOf(";") != this.ConnectionString.Length - 1)
				this.ConnectionString = this.ConnectionString + ";";
			this.CommandTimeout = CommandTimeout.Value;
		}
		#endregion
	}
	public class Parameters_Transaction
	{
		public Parameter_TransactionType Type;
		public Parameters_TransactionAction Success;
		public Parameters_TransactionAction Failure;
		public void Validate()
		{
			switch (Type)
			{				
				case Parameter_TransactionType.NewTransaction:
					if ((Success == Parameters_TransactionAction.NoAction) &&
						(Failure == Parameters_TransactionAction.NoAction))
					{
						Success = Parameters_TransactionAction.CommitTransaction;
						Failure = Parameters_TransactionAction.RollBackTransaction;
					}
					if (
						(Success == Parameters_TransactionAction.NoAction) ||
						(Failure == Parameters_TransactionAction.NoAction)
						)
						throw new Exception("TransactionType is set to NewTransaction, Seccess and Failure must be set.");
					break;
				default:
					Success = Parameters_TransactionAction.NoAction;
					Failure = Parameters_TransactionAction.NoAction;
					break;
			}
		}
		#region Parameters_Transaction Constructure 
		public Parameters_Transaction(SqlString Type,
			SqlString Success,
			SqlString Failure)
		{
			this.Type = (Parameter_TransactionType)Enum.Parse(typeof(Parameter_TransactionType), Type.Value.Trim(), true);
			this.Success = (Parameters_TransactionAction)Enum.Parse(typeof(Parameters_TransactionAction), Success.Value.Trim(), true);
			this.Failure = (Parameters_TransactionAction)Enum.Parse(typeof(Parameters_TransactionAction), Failure.Value.Trim(), true);
		}
		#endregion
	}
	public class Parameters_Destination
	{
		public Parameters_Connection Connection;
		public string SQL;
		public Parameters_CommandError CommandError;
		public Parameters_Transaction Transaction;
		public void Validate()
		{
			try
			{
				Connection.Validate();
				Transaction.Validate();
				if (Transaction.Type == Parameter_TransactionType.NewTransaction)
					Connection.ConnectionString = Connection.ConnectionString + "Enlist=false;";
				/*if(Transaction.Type == Parameter_TransactionType.CurrentContext) 
					Connection.ConnectionString = Connection.ConnectionString + "Enlist=true;";*/
			}
			catch (Exception e)
			{
				throw new Exception("Error occurred while validaing Destination: " + e.Message);
			}
		}
		#region Parameters_Destination Constructure
		public Parameters_Destination(SqlString Destination_Connection_ConnectionType,
			SqlString Destination_Connection_ConnectionString,
			SqlInt32 Destination_Connection_CommandTimeout,
			SqlString Destination_SQL,
			SqlString Destination_CommandError,
			SqlString Destination_Transaction_Type,
			SqlString Destination_Transaction_Success,
			SqlString Destination_Transaction_Failure)
		{
			this.Connection = new Parameters_Connection(Destination_Connection_ConnectionType,
					Destination_Connection_ConnectionString,
					Destination_Connection_CommandTimeout);
			this.Transaction = new Parameters_Transaction(Destination_Transaction_Type,
					Destination_Transaction_Success,
					Destination_Transaction_Failure);
			this.SQL = Destination_SQL.Value;
			this.CommandError = (Parameters_CommandError)Enum.Parse(typeof(Parameters_CommandError), Destination_CommandError.Value.Trim(), true);
		}
		#endregion
	}
	public class Parameters_Message
	{
		public Parameters_ReturnType ReturnType;
		public Parameters_Connection Connection;
		public string OnMessage;
		public Parameters_EventError OnEventError;
		public void Validate()
		{
			if (
				(ReturnType == Parameters_ReturnType.Console)||
				(ReturnType == Parameters_ReturnType.None)
				)
				return;
			if (Connection.ConnectionType == Parameters_ConnectionType.NoConnection)
				throw new Exception("ConnectionType must not be NoConnection when ReturnType is Event.");
			Connection.Validate();
		}
		#region Parameters_Message Constructure
		public Parameters_Message(SqlString ReturnType,
			SqlString Connection_ConnectionType,
			SqlString Connection_ConnectionString,
			SqlInt32 Connection_CommandTimeout,
			SqlString OnMessage,
			SqlString OnEventError)
		{
			this.ReturnType = (Parameters_ReturnType)Enum.Parse(typeof(Parameters_ReturnType), ReturnType.Value.Trim(), true);
			this.Connection = new Parameters_Connection(Connection_ConnectionType,
					Connection_ConnectionString,
					Connection_CommandTimeout);
			this.OnMessage = OnMessage.ToString();
			this.OnEventError = (Parameters_EventError)Enum.Parse(typeof(Parameters_EventError), OnEventError.Value.Trim(), true);
		}
		#endregion
	}
	public class Parameters_ResultSets
	{
		public Parameters_ReturnType ReturnType;
		public Parameters_Connection Connection;
		public string OnResult;
		public Parameters_EventError OnEventError;
		public void Validate()
		{
			if (
				(ReturnType == Parameters_ReturnType.Console) ||
				(ReturnType == Parameters_ReturnType.None)
				)
				return;
			if (Connection.ConnectionType == Parameters_ConnectionType.NoConnection)
				throw new Exception("ConnectionType must not be NoConnection when ReturnType is Event.");
			Connection.Validate();
		}
		#region Parameters_ResultSets Constructure
		public Parameters_ResultSets(SqlString ReturnType,
			SqlString Connection_ConnectionType,
			SqlString Connection_ConnectionString,
			SqlInt32 Connection_CommandTimeout,
			SqlString OnResult,
			SqlString OnEventError)
		{
			this.ReturnType = (Parameters_ReturnType)Enum.Parse(typeof(Parameters_ReturnType), ReturnType.Value.Trim(), true);
			this.Connection = new Parameters_Connection(Connection_ConnectionType,
					Connection_ConnectionString,
					Connection_CommandTimeout);
			this.OnResult = OnResult.ToString();
			this.OnEventError = (Parameters_EventError)Enum.Parse(typeof(Parameters_EventError), OnEventError.Value.Trim(), true);
		}
		#endregion
	}
	public class Parameters
	{
		public string ID;
		public bool Message_IncludeDetail;
		public Parameters_Destination Destination;
		public Parameters_Message InfoMessage;
		public Parameters_Message ErrorMessage;
		public Parameters_ResultSets ResultSets;
		public void Validate()
		{
			Destination.Validate();
			InfoMessage.Validate();
			ErrorMessage.Validate();
			ResultSets.Validate();
		}
		#region Parameters Constructure
		public Parameters(
			SqlString ID,
			SqlString Destination_Connection_ConnectionType,
			SqlString Destination_Connection_ConnectionString,
			SqlInt32 Destination_Connection_CommandTimeout,
			SqlString Destination_SQL,
			SqlString Destination_CommandError,
			SqlString Destination_Transaction_Type,
			SqlString Destination_Transaction_Success,
			SqlString Destination_Transaction_Failure,

			SqlString InfoMessage_IncludeDetail,
			SqlString InfoMessage_ReturnType,
			SqlString InfoMessage_Connection_ConnectionType,
			SqlString InfoMessage_Connection_ConnectionString,
			SqlInt32 InfoMessage_Connection_CommandTimeout,
			SqlString InfoMessage_OnMessage,
			SqlString InfoMessage_OnEventError,

			SqlString ErrorMessage_ReturnType,
			SqlString ErrorMessage_Connection_ConnectionType,
			SqlString ErrorMessage_Connection_ConnectionString,
			SqlInt32 ErrorMessage_Connection_CommandTimeout,
			SqlString ErrorMessage_OnMessage,
			SqlString ErrorMessage_OnEventError,

			SqlString ResultSets_ReturnType,
			SqlString ResultSets_Connection_ConnectionType,
			SqlString ResultSets_Connection_ConnectionString,
			SqlInt32 ResultSets_Connection_CommandTimeout,
			SqlString ResultSets_OnResult,
			SqlString ResultSets_OnEventError
			)
		{
			this.ID = ID.ToString();
			this.Message_IncludeDetail = InfoMessage_IncludeDetail.ToString().ToLower().Trim() != "false";
			this.Destination = new Parameters_Destination(Destination_Connection_ConnectionType,
					Destination_Connection_ConnectionString,
					Destination_Connection_CommandTimeout,
					Destination_SQL,
					Destination_CommandError,
					Destination_Transaction_Type,
					Destination_Transaction_Success,
					Destination_Transaction_Failure);
			this.InfoMessage = new Parameters_Message(InfoMessage_ReturnType,
					InfoMessage_Connection_ConnectionType,
					InfoMessage_Connection_ConnectionString,
					InfoMessage_Connection_CommandTimeout,
					InfoMessage_OnMessage,
					InfoMessage_OnEventError);
			this.ErrorMessage = new Parameters_Message(ErrorMessage_ReturnType,
					ErrorMessage_Connection_ConnectionType,
					ErrorMessage_Connection_ConnectionString,
					ErrorMessage_Connection_CommandTimeout,
					ErrorMessage_OnMessage,
					ErrorMessage_OnEventError);
			this.ResultSets = new Parameters_ResultSets(ResultSets_ReturnType,
					ResultSets_Connection_ConnectionType,
					ResultSets_Connection_ConnectionString,
					ResultSets_Connection_CommandTimeout,
					ResultSets_OnResult,
					ResultSets_OnEventError);
		}
		#endregion
	}
	public class ExecutionEngine
	{
		private Parameters parameters;
		private SqlConnection msgConnection;
		private SqlConnection errConnection;
		private SqlConnection retConnection;
		private SqlConnection contextConnection;
		private bool error = false;
		private void CreateAuxiliaryConnections()
		{
			contextConnection = new SqlConnection("Context Connection=true;");
			msgConnection = contextConnection; errConnection = contextConnection; retConnection = contextConnection;
			if (
					(parameters.InfoMessage.ReturnType == Parameters_ReturnType.Event)&&
					(parameters.InfoMessage.Connection.ConnectionType == Parameters_ConnectionType.NewConnection)
				)
				msgConnection = new SqlConnection(parameters.InfoMessage.Connection.ConnectionString);

			if (
					(parameters.ErrorMessage.ReturnType == Parameters_ReturnType.Event)&&
					(parameters.ErrorMessage.Connection.ConnectionType == Parameters_ConnectionType.NewConnection)
				)
			{
				if (parameters.ErrorMessage.Connection.ConnectionString == "Context Connection=true;")
					errConnection = contextConnection;
				else if (parameters.ErrorMessage.Connection.ConnectionString == parameters.InfoMessage.Connection.ConnectionString)
						errConnection = msgConnection;
				else
					errConnection = new SqlConnection(parameters.ErrorMessage.Connection.ConnectionString);
			}
			if (
					(parameters.ResultSets.ReturnType == Parameters_ReturnType.Event)&&
					(parameters.ResultSets.Connection.ConnectionType == Parameters_ConnectionType.NewConnection)
				)
			{
				if (parameters.ResultSets.Connection.ConnectionString == "Context Connection=true;")
					retConnection = contextConnection;
				else if (parameters.ResultSets.Connection.ConnectionString == parameters.InfoMessage.Connection.ConnectionString)
						retConnection = msgConnection;
				else if (parameters.ResultSets.Connection.ConnectionString == parameters.ErrorMessage.Connection.ConnectionString)
						retConnection = errConnection;
				else
					retConnection = new SqlConnection(parameters.ResultSets.Connection.ConnectionString);
			}
		}

		#region Read Data
		private DataSet schemaTables, dataTables;
		private SqlCommand SchemaTableToSchemaCommand(int index, string tableName)
		{
			StringBuilder SQL = new StringBuilder();
			SQL.Append("CREATE TABLE [" + tableName + "](\n");
			foreach(DataRow r in schemaTables.Tables[index].Rows)
				SQL.Append("[" + r["ConvertedColumnName"].ToString() + "] " + r["ConvertedColumnType"].ToString()+", \n");
			SQL.Append("[Primary Key Field - " + tableName + "] BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY)");
			return new SqlCommand(SQL.ToString());
		}
		private SqlCommand SchemaTableToInsertCommand(int index, string tableName)
		{
			StringBuilder part1 = new StringBuilder();
			StringBuilder part2 = new StringBuilder();
			SqlCommand cmd = new SqlCommand();
			part1.Append("INSERT INTO [" + tableName + "] (");
			part2.Append("Values(");
			foreach (DataRow r in schemaTables.Tables[index].Rows)
			{
				part1.Append("[" + r["ConvertedColumnName"] + "],");
				part2.Append(r["ParamName"] + ",");
				cmd.Parameters.Add((SqlParameter)r["Parameter"]);
			}
			part1.Remove(part1.Length - 1, 1);
			part1.Append(")");
			part2.Remove(part2.Length - 1, 1);
			part2.Append(")");
			cmd.CommandText = part1.ToString() + part2.ToString();
			return cmd;
		}
		private DataTable SchemaTableToDataTable(DataTable sc)
		{
			DataTable dt = new DataTable();
			foreach(DataRow r in sc.Rows)
				dt.Columns.Add(new DataColumn(r["ParamName"].ToString(), Type.GetType(r["DataType"].ToString())));
			return dt;
		}
		private void SchemaTableConvert(DataTable dt)
		{
			string originalColumnName = "", newColumnName = "";
			int i=0; SqlParameter p;
			ArrayList listCols = new ArrayList();
			if(dt.Columns.IndexOf("ConvertedColumnName") == -1)
				dt.Columns.Add("ConvertedColumnName", typeof(String));
			if(dt.Columns.IndexOf("ConvertedColumnType") == -1)
				dt.Columns.Add("ConvertedColumnType", typeof(String));
			if(dt.Columns.IndexOf("ParamName") == -1)
				dt.Columns.Add("ParamName", typeof(String));
			if(dt.Columns.IndexOf("Parameter") == -1)
				dt.Columns.Add("Parameter", typeof(Object));
			foreach(DataRow r in dt.Rows)
			{
				/*Create new column name for the result set*/
				originalColumnName = r["ColumnName"].ToString();
				if (originalColumnName.Trim() == "")
					originalColumnName = "No Column Name";			
				i = 0;
				newColumnName = originalColumnName;
				while (listCols.Contains(newColumnName))
				{
					newColumnName = originalColumnName+"_"+i.ToString();
					i++;
				}
				listCols.Add(newColumnName);
				r["ConvertedColumnName"] = newColumnName;
				/*Create parameter and SQL type for the result set*/
				r["ParamName"] = "@P"+r["ColumnOrdinal"];
				p = new SqlParameter();
				p.ParameterName = r["ParamName"].ToString();
				if ((r["DataTypeName"].ToString().ToLower() == "bigint") ||
					(r["DataTypeName"].ToString().ToLower() == "bit") ||
					(r["DataTypeName"].ToString().ToLower() == "datetime") ||
					(r["DataTypeName"].ToString().ToLower() == "float") ||
					(r["DataTypeName"].ToString().ToLower() == "image") ||
					(r["DataTypeName"].ToString().ToLower() == "integer") ||
					(r["DataTypeName"].ToString().ToLower() == "money") ||
					(r["DataTypeName"].ToString().ToLower() == "ntext") ||
					(r["DataTypeName"].ToString().ToLower() == "real") ||
					(r["DataTypeName"].ToString().ToLower() == "smalldatetime") ||
					(r["DataTypeName"].ToString().ToLower() == "smallint") ||
					(r["DataTypeName"].ToString().ToLower() == "smallmoney") ||
					(r["DataTypeName"].ToString().ToLower() == "text") ||
					(r["DataTypeName"].ToString().ToLower() == "tinyint") ||
					(r["DataTypeName"].ToString().ToLower() == "uniqueidentifier") ||
					(r["DataTypeName"].ToString().ToLower() == "xml"))
				{
					r["ConvertedColumnType"] = r["DataTypeName"].ToString().ToUpper();
					p.SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), r["DataTypeName"].ToString(), true);
				}
				else if(r["DataTypeName"].ToString().ToLower() == "sql_variant")
				{
					r["ConvertedColumnType"] = r["DataTypeName"].ToString().ToUpper();
					p.SqlDbType = SqlDbType.Variant;
				}
				else if (r["DataTypeName"].ToString().ToLower() == "timestamp")
				{
					r["ConvertedColumnType"] = "VARBINARY(10)";
					p.SqlDbType = SqlDbType.VarBinary;
					p.Size = 10;
				}
				else if ((r["DataTypeName"].ToString().ToLower() == "binary") ||
					(r["DataTypeName"].ToString().ToLower() == "char") ||
					(r["DataTypeName"].ToString().ToLower() == "nchar") ||
					(r["DataTypeName"].ToString().ToLower() == "nvarchar") ||
					(r["DataTypeName"].ToString().ToLower() == "varbinary") ||
					(r["DataTypeName"].ToString().ToLower() == "varchar"))
				{
					if (Int32.Parse(r["ColumnSize"].ToString()) <= 8000)
						r["ConvertedColumnType"] = r["DataTypeName"].ToString().ToUpper() + "(" + r["ColumnSize"].ToString() + ")";
					else
						r["ConvertedColumnType"] = r["DataTypeName"].ToString().ToUpper() + "(MAX)";
					p.SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), r["DataTypeName"].ToString(), true);
					p.Size = Int32.Parse(r["ColumnSize"].ToString());
				}
				else if ((r["DataTypeName"].ToString().ToLower() == "decimal") ||
					(r["DataTypeName"].ToString().ToLower() == "numeric"))
				{
					r["ConvertedColumnType"] = r["DataTypeName"].ToString().ToUpper() + "(" + r["NumericPrecision"].ToString() + "," + r["NumericScale"].ToString() + ")";
					p.SqlDbType = SqlDbType.Decimal;
					p.Precision = Byte.Parse(r["NumericPrecision"].ToString());
					p.Scale = Byte.Parse(r["NumericScale"].ToString());
				}
				else
				{
					r["ConvertedColumnType"] = "VARCHAR(8000)";
					p.Size = 8000;
				}
				r["Parameter"] = p;
			}
			return;
		}
		private void ReadData(SqlDataReader r)
		{
			DataTable dt; DataRow dr;
			schemaTables.Tables.Clear();
			dataTables.Tables.Clear();
			if (r.GetSchemaTable() == null) return;
			while (true)
			{
				dt = r.GetSchemaTable();
                dt.TableName = Guid.NewGuid().ToString();
				SchemaTableConvert(dt);
				schemaTables.Tables.Add(dt);
				dt = SchemaTableToDataTable(dt);
                dt.TableName = Guid.NewGuid().ToString();
				dataTables.Tables.Add(dt);
				while (r.Read())
				{
					dr = dt.NewRow();
					for (int i = 0; i < r.FieldCount; i++)
						dr[i] = r[i];
					dt.Rows.Add(dr);
				}
				if (!r.NextResult())
					break;
			}
		}
		#endregion

		private string GetSQLErrorMessage(SqlError er)
		{
			return "Message: " + er.Message +
						" LineNumber: " + er.LineNumber +
						" Class:" + er.Class +
						" State: " + er.State +
						" Procedure: " + er.Procedure+
						" Source: " + er.Source;
			
		}
		private void WriteConsole(Exception e)
		{
			SqlContext.Pipe.Send("Error (" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ")");
			if (e is SqlException)
				foreach(SqlError er in ((SqlException)e).Errors)
					SqlContext.Pipe.Send("	" + GetSQLErrorMessage(er));
			else
				SqlContext.Pipe.Send("	" + e.Message);
		}
		private void WriteConsole(SqlErrorCollection e)
		{
			SqlContext.Pipe.Send("Error (" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ")");
			foreach (SqlError er in e)
				SqlContext.Pipe.Send("	" + GetSQLErrorMessage(er));
		}
		private void WriteConsole(string message)
		{
			SqlContext.Pipe.Send("Message (" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ") " + message);
		}
		private void WriteConsole(SqlDataReader data, bool raiseError)
		{
			try
			{
				if (data.GetSchemaTable() == null) return;
				SqlContext.Pipe.Send(data);
			}
			catch
			{
				if (raiseError)
					throw;
			}
		}
		private void WriteTrigger(SqlConnection conn, string trigger, Exception e, bool raiseError)
		{
			StringBuilder msg = new StringBuilder();
			if (e is SqlException)
				foreach (SqlError er in ((SqlException)e).Errors)
					msg.Append(GetSQLErrorMessage(er));
			else
				msg.Append(e.Message);
			WriteTrigger(conn, trigger, msg.ToString(), raiseError);
            msg = null;
		}
		private void WriteTrigger(SqlConnection conn, string trigger, SqlErrorCollection e, bool raiseError)
		{
			StringBuilder msg = new StringBuilder();
			foreach (SqlError er in e)
				msg.Append(GetSQLErrorMessage(er));
			WriteTrigger(conn, trigger, msg.ToString(), raiseError);
            msg = null;
		}
		private void WriteTrigger(SqlConnection conn, string trigger, string message, bool raiseError)
		{
			conn.Close();
            SqlCommand cmd = conn.CreateCommand();
			try
			{
				conn.Open();
				cmd.CommandText = trigger;
				cmd.Parameters.Add("@_ID", SqlDbType.VarChar, 1000);
				cmd.Parameters.Add("@_Message", SqlDbType.Text);
				cmd.Parameters[0].Value = parameters.ID;
				cmd.Parameters[1].Value = message;
				cmd.ExecuteNonQuery();
			}
			catch 
			{
				if (raiseError)
					throw;
			}
			finally
			{
                cmd = null;
				conn.Close();
			}
		}
		private void WriteTrigger(SqlConnection conn, string trigger, bool raiseError)
		{
			string tableName = "";
			conn.Close();
			try
			{
				conn.Open();
				for (int i = 0; i < schemaTables.Tables.Count; i++)
				{
					tableName = "__" + parameters.ID + " - " +Guid.NewGuid().ToString()+"__";
					/*Creates Table*/
					SqlCommand cmd = SchemaTableToSchemaCommand(i, tableName);
					cmd.Connection = conn;
					cmd.ExecuteNonQuery();
					cmd = null;
					/*Write Data*/
					cmd = SchemaTableToInsertCommand(i, tableName);
					cmd.Connection = conn;
					foreach (DataRow r in dataTables.Tables[i].Rows)
					{
						foreach (DataRow p in schemaTables.Tables[i].Rows)
							cmd.Parameters[p["ParamName"].ToString()].Value = r[p["ParamName"].ToString()];
						cmd.ExecuteNonQuery();
					}
					/*fire trigger*/
					cmd = null;
					cmd = conn.CreateCommand();
					cmd.CommandText = trigger;
					cmd.Parameters.Add("@_ID", SqlDbType.NVarChar, 1000);
					cmd.Parameters.Add("@_Index", SqlDbType.Int);
					cmd.Parameters.Add("@_TableName", SqlDbType.NVarChar, 1000);
					cmd.Parameters[0].Value = parameters.ID;
					cmd.Parameters[1].Value = i;
					cmd.Parameters[2].Value = tableName;
					cmd.ExecuteNonQuery();
                    cmd = null;
				}
			}
			catch
			{
				if (raiseError)
					throw;
			}
			finally
			{
				conn.Close();
			}
		}		
		private void WriteInfomation(string msg)
		{
			if(parameters.InfoMessage.ReturnType == Parameters_ReturnType.None)
				return;
			if (parameters.InfoMessage.ReturnType == Parameters_ReturnType.Console)
			{
				WriteConsole(msg);
				return;
			}
			WriteTrigger(msgConnection,
				parameters.InfoMessage.OnMessage,
				msg,
				parameters.InfoMessage.OnEventError == Parameters_EventError.StopProcess);
		}
		private void WriteError(Exception err)
		{
			if (parameters.ErrorMessage.ReturnType == Parameters_ReturnType.None)
				return;
			if (parameters.ErrorMessage.ReturnType == Parameters_ReturnType.Console)
			{
				WriteConsole(err);
				return;
			}
			WriteTrigger(errConnection,
				parameters.ErrorMessage.OnMessage,
				err,
				parameters.ErrorMessage.OnEventError == Parameters_EventError.StopProcess);
		}
		private void WriteError(SqlErrorCollection err)
		{
			if (parameters.ErrorMessage.ReturnType == Parameters_ReturnType.None)
				return;
			if (parameters.ErrorMessage.ReturnType == Parameters_ReturnType.Console)
			{
				WriteConsole(err);
				return;
			}
			WriteTrigger(errConnection,
				parameters.ErrorMessage.OnMessage,
				err,
				parameters.ErrorMessage.OnEventError == Parameters_EventError.StopProcess);
		}
		private void WriteResult(SqlDataReader data)
		{
			if (parameters.ResultSets.ReturnType == Parameters_ReturnType.None)
				return;
			if (parameters.ResultSets.ReturnType == Parameters_ReturnType.Console)
			{
				WriteConsole(data,parameters.ResultSets.OnEventError == Parameters_EventError.StopProcess);
				return;
			}
			ReadData(data);
			WriteTrigger(retConnection,
				parameters.ResultSets.OnResult,
				parameters.ResultSets.OnEventError == Parameters_EventError.StopProcess);
		}
		private void onSession_InfoMessage(object sender, SqlInfoMessageEventArgs e)
		{
			WriteInfomation(e.Message);
		}
		public void Run()
		{
			bool isTranStarted = false;
			SqlConnection session = new SqlConnection(parameters.Destination.Connection.ConnectionString);
			SqlTransaction tran = null;
            SqlCommand cmd = null;
            SqlDataReader data = null;
			session.InfoMessage += new SqlInfoMessageEventHandler(onSession_InfoMessage);
			session.FireInfoMessageEventOnUserErrors = false;
			try
			{
				if(parameters.Message_IncludeDetail)
					WriteInfomation("Connecting " + session.DataSource + " ...");
				session.Open();
				if (parameters.Message_IncludeDetail)
					WriteInfomation("Server " + session.DataSource + " connected...");
				cmd = session.CreateCommand();
				if (parameters.Destination.Transaction.Type == Parameter_TransactionType.NewTransaction)
				{
					tran = session.BeginTransaction();
					isTranStarted = true;
					cmd.Transaction = tran;
					if (parameters.Message_IncludeDetail)
						WriteInfomation("New transaction is strated ...");
				}
				cmd.CommandTimeout = parameters.Destination.Connection.CommandTimeout;
				cmd.CommandText = parameters.Destination.SQL;
				if (parameters.Message_IncludeDetail)
					WriteInfomation("Starting command ...");
				if (parameters.ResultSets.ReturnType != Parameters_ReturnType.None)
				{
					data = cmd.ExecuteReader();
					if (parameters.Message_IncludeDetail)
						WriteInfomation("Retrieving result sets ...");
					WriteResult(data);
					if (parameters.Message_IncludeDetail)
						WriteInfomation("Result sets retrieved ...");
						
					data.Close();
				}
				else
				{
					if (parameters.Message_IncludeDetail)
						WriteInfomation("Result sets are being ignored ...");
					cmd.ExecuteNonQuery();
				}
				if (parameters.Message_IncludeDetail)
					WriteInfomation("Command finished ...");
				if ((parameters.Destination.Transaction.Type == Parameter_TransactionType.NewTransaction))
				{
					if ((!error)&&(parameters.Destination.Transaction.Success == Parameters_TransactionAction.CommitTransaction))
					{
						tran.Commit();
						if (parameters.Message_IncludeDetail)
							WriteInfomation("Transaction is committed ...");
					}
					else
					{
						tran.Rollback();
						if (parameters.Message_IncludeDetail)
							WriteInfomation("Transaction is rolled back ...");
					}
				}
			}
			catch (Exception er)
			{
				error = true;
				WriteError(er);
				if (isTranStarted && (parameters.Destination.Transaction.Type == Parameter_TransactionType.NewTransaction))
				{
					if ((parameters.Destination.Transaction.Failure == Parameters_TransactionAction.CommitTransaction))
					{
                        try
                        {
                            if (data != null)
                                if (!data.IsClosed)
                                    data.Close();
                            tran.Commit();
                        }
                        catch (Exception er1)
                        {
                            WriteInfomation(er1.Message);
                        }
						if (parameters.Message_IncludeDetail)
							WriteInfomation("Transaction is committed ...");
					}
					else
					{
                        try
                        {
                            if (data != null)
                                if (!data.IsClosed)
                                    data.Close();
                            tran.Rollback();
                        }
                        catch (Exception er1)
                        {
                            WriteInfomation(er1.Message);
                        }
						if (parameters.Message_IncludeDetail)
							WriteInfomation("Transaction is rolledback ...");
					}
				}
				if (parameters.Destination.CommandError == Parameters_CommandError.Raise)
					throw;
			}
			finally
			{
				session.Close();
				if (parameters.Message_IncludeDetail)
					WriteInfomation("Connection closed ....");
				if (error)
				{
					if (parameters.Message_IncludeDetail)
						WriteInfomation("Finished with error ....");
				}
				else
				{
					if (parameters.Message_IncludeDetail)
						WriteInfomation("Finished....");
				}
                cmd = null;
                session = null;
                tran = null;
			}
		}
		#region ExecutionEngine Constructure
		public ExecutionEngine(
			SqlString ID,
			SqlString Destination_Connection_ConnectionType,
			SqlString Destination_Connection_ConnectionString,
			SqlInt32  Destination_Connection_CommandTimeout,
			SqlString Destination_SQL,
			SqlString Destination_CommandError,
			SqlString Destination_Transaction_Type,
			SqlString Destination_Transaction_Success,
			SqlString Destination_Transaction_Failure,

			SqlString InfoMessage_IncludeDetail,
			SqlString InfoMessage_ReturnType,
			SqlString InfoMessage_Connection_ConnectionType,
			SqlString InfoMessage_Connection_ConnectionString,
			SqlInt32  InfoMessage_Connection_CommandTimeout,
			SqlString InfoMessage_OnMessage,
			SqlString InfoMessage_OnEventError,

			SqlString ErrorMessage_ReturnType,
			SqlString ErrorMessage_Connection_ConnectionType,
			SqlString ErrorMessage_Connection_ConnectionString,
			SqlInt32  ErrorMessage_Connection_CommandTimeout,
			SqlString ErrorMessage_OnMessage,
			SqlString ErrorMessage_OnEventError,

			SqlString ResultSets_ReturnType,
			SqlString ResultSets_Connection_ConnectionType,
			SqlString ResultSets_Connection_ConnectionString,
			SqlInt32  ResultSets_Connection_CommandTimeout,
			SqlString ResultSets_OnResult,
			SqlString ResultSets_OnEventError
			)
		{
			parameters = new Parameters(ID, 
				Destination_Connection_ConnectionType,
				Destination_Connection_ConnectionString,
				Destination_Connection_CommandTimeout,
				Destination_SQL,
				Destination_CommandError,
				Destination_Transaction_Type,
				Destination_Transaction_Success,
				Destination_Transaction_Failure,

				InfoMessage_IncludeDetail,
				InfoMessage_ReturnType,
				InfoMessage_Connection_ConnectionType,
				InfoMessage_Connection_ConnectionString,
				InfoMessage_Connection_CommandTimeout,
				InfoMessage_OnMessage,
				InfoMessage_OnEventError,

				ErrorMessage_ReturnType,
				ErrorMessage_Connection_ConnectionType,
				ErrorMessage_Connection_ConnectionString,
				ErrorMessage_Connection_CommandTimeout,
				ErrorMessage_OnMessage,
				ErrorMessage_OnEventError,

				ResultSets_ReturnType,
				ResultSets_Connection_ConnectionType,
				ResultSets_Connection_ConnectionString,
				ResultSets_Connection_CommandTimeout,
				ResultSets_OnResult,
				ResultSets_OnEventError
				);
			schemaTables = new DataSet();
			dataTables = new DataSet();
			parameters.Validate();
			CreateAuxiliaryConnections();
		}
			#endregion
        public void memCleanup()
        {
            parameters = null;
            if (msgConnection != null)
            {
                msgConnection.Close();
                msgConnection = null;
            }
            if (errConnection != null)
            {
                errConnection.Close();
                errConnection = null;
            }
            if (retConnection != null)
            {
                retConnection.Close();
                retConnection = null;
            }
            if (contextConnection != null)
            {
                contextConnection.Close();
                contextConnection = null;
            }
            if (schemaTables != null)
            {
                schemaTables.Clear();
                schemaTables = null;
            }
            if (dataTables != null)
            {
                dataTables.Clear();
                dataTables = null;
            }
        }
	}
    public partial class SQLRExecute
    {
        [Microsoft.SqlServer.Server.SqlProcedure]
        public static void sp_CLRRemoteExecute(
            SqlString ID,
            SqlString Destination_Connection_ConnectionType,
            SqlString Destination_Connection_ConnectionString,
            SqlInt32 Destination_Connection_CommandTimeout,
            SqlString Destination_SQL,
            SqlString Destination_CommandError,
            SqlString Destination_Transaction_Type,
            SqlString Destination_Transaction_Success,
            SqlString Destination_Transaction_Failure,

            SqlString InfoMessage_IncludeDetail,
            SqlString InfoMessage_ReturnType,
            SqlString InfoMessage_Connection_ConnectionType,
            SqlString InfoMessage_Connection_ConnectionString,
            SqlInt32 InfoMessage_Connection_CommandTimeout,
            SqlString InfoMessage_OnMessage,
            SqlString InfoMessage_OnEventError,

            SqlString ErrorMessage_ReturnType,
            SqlString ErrorMessage_Connection_ConnectionType,
            SqlString ErrorMessage_Connection_ConnectionString,
            SqlInt32 ErrorMessage_Connection_CommandTimeout,
            SqlString ErrorMessage_OnMessage,
            SqlString ErrorMessage_OnEventError,

            SqlString ResultSets_ReturnType,
            SqlString ResultSets_Connection_ConnectionType,
            SqlString ResultSets_Connection_ConnectionString,
            SqlInt32 ResultSets_Connection_CommandTimeout,
            SqlString ResultSets_OnResult,
            SqlString ResultSets_OnEventError
            )
        {
            ExecutionEngine executionEngine = new ExecutionEngine(ID,
                Destination_Connection_ConnectionType,
                Destination_Connection_ConnectionString,
                Destination_Connection_CommandTimeout,
                Destination_SQL,
                Destination_CommandError,
                Destination_Transaction_Type,
                Destination_Transaction_Success,
                Destination_Transaction_Failure,

                InfoMessage_IncludeDetail,
                InfoMessage_ReturnType,
                InfoMessage_Connection_ConnectionType,
                InfoMessage_Connection_ConnectionString,
                InfoMessage_Connection_CommandTimeout,
                InfoMessage_OnMessage,
                InfoMessage_OnEventError,

                ErrorMessage_ReturnType,
                ErrorMessage_Connection_ConnectionType,
                ErrorMessage_Connection_ConnectionString,
                ErrorMessage_Connection_CommandTimeout,
                ErrorMessage_OnMessage,
                ErrorMessage_OnEventError,

                ResultSets_ReturnType,
                ResultSets_Connection_ConnectionType,
                ResultSets_Connection_ConnectionString,
                ResultSets_Connection_CommandTimeout,
                ResultSets_OnResult,
                ResultSets_OnEventError);
            try
            {
                executionEngine.Run();
            }
            finally
            {
                executionEngine.memCleanup();
                executionEngine = null;
                //GC.Collect();
            }
        }
    }
}

