using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace EODDataService
{
    public enum DownloadTaskStatus{Pending, Running, Complete}
    public class DownloadTask :IDisposable
    {
        static object SyncObject = new object();
        static List<DownloadTask> RunningTasks = new List<DownloadTask>();
        static void AddToList(DownloadTask downloadTask)
        {
            lock (SyncObject)
            {
                RunningTasks.Add(downloadTask);
            }
        }
        static void RemoveFromList(DownloadTask downloadTask)
        {
            lock (SyncObject)
            {

                RunningTasks.Remove(downloadTask);
            }
        }
        
        public static int TaskCount
        {
            get 
            {
                lock (SyncObject)
                    return RunningTasks.Count;
            }
        }
        public static void WaitAll()
        {
            while (TaskCount != 0)
                Thread.Sleep(10);
        }

        
        SqlConnection connection = new SqlConnection(G.ConnectionString);
        public Task task;
        public List<Exception> Exceptions = new List<Exception>();
        public SqlString ExceptionInSqlFormat
        {
            get 
            {
                return Exceptions.Count == 0? SqlString.Null:new SqlString(string.Join("\n", Exceptions.Select(x => x.Message).ToArray()));
            }
        }

        void StatusUpdate()
        {
            using (SqlConnection connection = new SqlConnection(G.ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "[EODData].[SetSessionStatus]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ManagementThreadID", SqlDbType.Int).Value = Thread.CurrentThread.ManagedThreadId;
                cmd.Parameters.Add("@TaskSessionID", SqlDbType.SmallInt).Value = SessionID;
                cmd.Parameters.Add("@BulkCopySessionID", SqlDbType.SmallInt).Value = BulkCopySessionID;
                cmd.Parameters.Add("@TaskID", SqlDbType.Int).Value = TaskID;
                cmd.Parameters.Add("@PoolID", SqlDbType.SmallInt).Value = PoolID;
                cmd.Parameters.Add("@MethodName", SqlDbType.VarChar, 50).Value = MethodName;
                cmd.Parameters.Add("@IntervalID", SqlDbType.TinyInt).Value = (byte)Interval;
                cmd.Parameters.Add("@Exchange", SqlDbType.VarChar, 10).Value = Exchange;
                cmd.Parameters.Add("@DateFrom", SqlDbType.Date).Value = DateFrom;
                cmd.Parameters.Add("@Status", SqlDbType.VarChar, 50).Value = Status.ToString();
                cmd.Parameters.Add("@Error", SqlDbType.VarChar, -1).Value = ExceptionInSqlFormat;
                cmd.Parameters.Add("@Rows", SqlDbType.Int).Value = Rows;
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        DownloadTaskStatus mStatus = DownloadTaskStatus.Pending;
        short mBulkCopySessionID = -1;
        short mSessionID = -1;
        short mPoolID = -1;
        int mTaskID = -1;
        string mMethodName = "";
        string mExchange = "";
        int mRows = 0;
        EODDataInterval mInterval = EODDataInterval.None;
        DateTime mDateFrom = DateTime.MinValue;

        public DownloadTaskStatus Status { get { return mStatus; } set { mStatus = value; StatusUpdate(); } }
        public short BulkCopySessionID { get { return mBulkCopySessionID; } set { mBulkCopySessionID = value; StatusUpdate(); } }
        public short SessionID { get { return mSessionID; } set { mSessionID = value; StatusUpdate(); } }
        public short PoolID { get { return mPoolID; } set { mPoolID = value; StatusUpdate(); } }
        public int TaskID { get { return mTaskID; } set { mTaskID = value; StatusUpdate(); } }
        public int Rows { get { return mRows; } set { mRows = value; StatusUpdate(); } }
        public string MethodName { get { return mMethodName; } set { mMethodName = value; StatusUpdate(); } }
        public string Exchange { get { return mExchange; } set { mExchange = value; StatusUpdate(); } }
        public EODDataInterval Interval { get { return mInterval; } set { mInterval = value; StatusUpdate(); } }
        public DateTime DateFrom  { get { return mDateFrom; } set { mDateFrom = value; StatusUpdate(); } }
        public DownloadTask()
        {
            AddToList(this);
            task = Task.Factory.StartNew(TaskBody, TaskCreationOptions.LongRunning);
            
        }

        bool OpenConnection()
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "set xact_abort off; begin transaction;select @@spid";
                SessionID = (short)cmd.ExecuteScalar();
                return true;
            }
            catch(Exception e)
            {
                Exceptions.Add(e);
                return false;
            }
        }
        void CloseConnection()
        {
            try
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "while @@trancount > 0 commit;";
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                Exceptions.Add(e);
            }
        }
        void ExecuteTask()
        {
            DateTime downloadStartDate = DateTime.Now;
            try
            {
                if (MethodName.ToUpper() == "GetCountries".ToUpper())
                    G.EODDataConnection.GetCountries().WriteToServer(this);
                if (MethodName.ToUpper() == "GetExchanges".ToUpper())
                    G.EODDataConnection.GetExchanges().WriteToServer(this);
                if (MethodName.ToUpper() == "GetFundamentals".ToUpper())
                    G.EODDataConnection.GetFundamentals(Exchange).WriteToServer(Exchange, this);
                if (MethodName.ToUpper() == "GetQuotes".ToUpper())
                    G.EODDataConnection.GetQuotes(Exchange, DateFrom, Interval).WriteToServer(Exchange, Interval, this);
                if (MethodName.ToUpper() == "GetSplits".ToUpper())
                    G.EODDataConnection.GetSplits(Exchange).WriteToServer(Exchange, this);
                if (MethodName.ToUpper() == "GetSymbols".ToUpper())
                    G.EODDataConnection.GetSymbols(Exchange).WriteToServer(Exchange, this);
                if (MethodName.ToUpper() == "GetSymbolChanges".ToUpper())
                    G.EODDataConnection.GetSymbolChanges(Exchange).WriteToServer(Exchange, this);
            }
            catch(Exception e)
            {
                Exceptions.Add(e);
            }
            finally
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.CommandText = "[EODData].[EndTask]";
                cmd.Parameters.Add("@PoolID", SqlDbType.SmallInt).Value = PoolID;
                cmd.Parameters.Add("@TaskID", SqlDbType.Int).Value = TaskID;
                cmd.Parameters.Add("@Error", SqlDbType.VarChar, -1).Value = ExceptionInSqlFormat;
                cmd.Parameters.Add("@Rows", SqlDbType.Int).Value = Rows;
                cmd.Parameters.Add("@DownloadStartDate", SqlDbType.DateTime).Value = downloadStartDate;
                cmd.ExecuteNonQuery();
            }
        }
        bool ReadTask()
        {
            try
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "EODData.GetTask";
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    if (r == null)
                        return false;
                    if(r.Read())
                    {
                        mPoolID = r.IsDBNull(0) ? (short)-1 : r.GetInt16(0);
                        mTaskID = r.IsDBNull(1) ? -1 : r.GetInt32(1);
                        mMethodName = r.IsDBNull(2) ? "" : r.GetString(2);
                        mExchange = r.IsDBNull(3) ? "" : r.GetString(3);
                        mInterval = r.IsDBNull(4) ? EODDataInterval.None : (EODDataInterval)r.GetByte(4);
                        DateFrom = r.IsDBNull(5) ? DateTime.MinValue : r.GetDateTime(5);
                        return true;
                    }
                    r.Close();
                }
                return false;
            }
            catch(Exception e)
            {
                Exceptions.Add(e);
                return false;
            }
        }
        void TaskBody()
        {
            try
            {
                StatusUpdate();
                if (!OpenConnection())
                    return;
                if (!ReadTask())
                    return;
                Status = DownloadTaskStatus.Running;
                ExecuteTask();
                lock(SyncObject)
                    G.LastTaskExecutionTime = DateTime.Now;
            }
            finally
            {
                Status = DownloadTaskStatus.Complete;
                CloseConnection();
                RemoveFromList(this);
            }
        }

        public void Dispose()
        {
            try 
            { 
                connection.Close(); 
                
            }
            catch { }
            Exceptions.Clear();

        }
    }
}
