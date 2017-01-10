using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.SqlServer.Server;

namespace Sequence
{
    public enum SequenceProcessType { Update, Delete }
    public class Sequence : ICloneable
    {
        public int Cache = 100, SPID = 0;
        public string Name = "";
        public long mCurrentValue = 1, CachedValue = 1, StartValue = 1, EndValue = 9223372036854775807, Step = 1;
        public bool Cycle = true, IsNew = true;
        public void Validate()
        {
            Sequence.Validate(StartValue, EndValue, Step, Cycle, Cache, mCurrentValue, IsNew);
            return;
        }
        public static void Validate(long StartValue, long EndValue, long Step, bool Cycle, int Cache, long mCurrentValue, bool IsNew)
        {
            if (Step > 0 && StartValue > EndValue)
                throw new Exception("EndValue must be greater than StartValue when Step is greater than 0.");
            if (Step < 0 && StartValue < EndValue)
                throw new Exception("StartValue must be greater than EndValue when Step is less than 0.");
            if (Step == 0)
                throw new Exception("Step must not be 0.");
            if (Cache <= 0)
                throw new Exception("Cache must be greater than 0.");
            if (!IsNew)
            {
                if (!mCurrentValue.Between(StartValue, EndValue))
                    throw new Exception("Invalid Current Value " + mCurrentValue.ToString());
            }
            if ((EndValue - StartValue).Abs() / Step.Abs() < Cache)
                throw new Exception("Cache is too large.");
            if ((EndValue - StartValue).Abs() < Step.Abs())
                throw new Exception("Step is too big");
            return;
        }
        private void SetNextCacheValue()
        {
            if (IsNew)
            {
                IsNew = false;
                CachedValue = StartValue;
            }
            if ((EndValue - CachedValue).Abs() >= (Cache * Step).Abs())
            {
                CachedValue = CachedValue + Cache * Step;
            }
            else
            {
                CachedValue = EndValue;
            }
            if (SPID != 0)
                return;

            SequenceProcess req = SequenceCommonDataArea.RunCommand(this, SequenceProcessType.Update, false);
            if (req.Error != null)
                throw req.Error;
        }
        public long CurrentValue
        {
            get
            {
                return IsNew ? NextValue : mCurrentValue;
            }
        }
        public long NextValue
        {
            get
            {
                lock (this)
                {
                    if (IsNew)
                    {
                        mCurrentValue = StartValue;
                        SetNextCacheValue();
                        return mCurrentValue;
                    }
                    long ret = mCurrentValue;
                    if ((EndValue - mCurrentValue).Abs() >= Step.Abs())
                    {
                        ret = mCurrentValue + Step;
                        if (ret >= CachedValue && Step > 0 || ret <= CachedValue && Step < 0)
                        {
                            SetNextCacheValue();
                        }
                    }
                    else
                    {
                        if (Cycle)
                        {
                            if (Step > 0)
                                ret = StartValue + (Step.Abs() - (EndValue - mCurrentValue).Abs() - 1);
                            else
                                ret = StartValue - (Step.Abs() - (EndValue - mCurrentValue).Abs() - 1);

                            CachedValue = StartValue;
                            SetNextCacheValue();
                        }
                        else
                        {
                            throw new Exception("Sequence is reached it maximum value.");
                        }
                    }
                    mCurrentValue = ret;
                    return mCurrentValue;
                }
            }
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public bool IsTemporary
        {
            get { return Name.Substring(0, 1) == "#"; }
        }
    }
    public class SequenceProcess
    {
        public Sequence Sequence= null;
        public SequenceProcessType Type = SequenceProcessType.Update;
        public ManualResetEvent ClientEvent = new ManualResetEvent(false);
        public Exception Error = null;
        public bool TerminateServiceProcess = false;
        public int SPID = SequenceCommonDataArea.GetCurrentSpid();
    }
    public class SessionSequence
    {
        public int SPID = 0;
        public List<Sequence> Sequences = new List<Sequence>();
        public void Clear()
        {
            Sequences.Clear();
        }
        public SessionSequence(int spid)
        {
            SPID = spid;
        }
    }
    public static class SequenceCommonDataArea
    {
        #region Global Static Variables
        public static ManualResetEvent ServiceEvent = null;
        public static Queue<SequenceProcess> SequenceQueue = null;
        public static List<Sequence> StaticSequences = null;
        public static List<SessionSequence> SessionSequences = null;
        public static int InternalSession = 0;
        private static SqlConnection Connection = null;
        private static SqlCommand Command = null;
        #endregion

        #region Data Access
        private static SqlCommand GetCommand()
        {
            return GetCommand(CommandType.Text);
        }
        private static SqlCommand GetCommand(CommandType type)
        {
            if(Connection == null)
                Connection = new SqlConnection("context connection=true");
            if(Connection.State == ConnectionState.Closed||Connection.State == ConnectionState.Broken)
                Connection.Open();
            if(Command == null)
                Command = Connection.CreateCommand();
            Command.CommandType = type;
            Command.CommandTimeout = 0;
            Command.Parameters.Clear();
            return Command;
        }
        public static int GetCurrentSpid()
        {
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                int ret;
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "select @@spid";
                ret = System.Int32.Parse(cmd.ExecuteScalar().ToString());
                connection.Close();
                return ret;
            }
        }
        private static void SetInternalSessionID()
        {
            SqlCommand cmd = GetCommand();
            cmd.CommandText = "select @@spid";
            InternalSession = System.Int16.Parse(cmd.ExecuteScalar().ToString());
        }
        private static Sequence DataReaderToSequence(SqlDataReader r)
        {
            Sequence ret = new Sequence();
            ret.Name = r.GetString(0);
            ret.mCurrentValue = r.IsDBNull(1) ? r.GetInt64(2) : r.GetInt64(1);
            ret.StartValue = r.GetInt64(2);
            ret.EndValue = r.GetInt64(3);
            ret.Step = r.GetInt64(4);
            ret.Cycle = r.GetBoolean(5);
            ret.Cache = r.GetInt32(6);
            ret.CachedValue = r.GetInt64(7);
            ret.IsNew = r.IsDBNull(1);
            return ret;
        }
        public static List<Sequence> ReadSequence()
        {
            return ReadSequence("");
        }
        public static List<Sequence> ReadSequence(string name)
        {
            List<Sequence> ret = new List<Sequence>();
            Sequence seq = null;
            SqlCommand cmd = GetCommand(CommandType.StoredProcedure);
            cmd.CommandText = "Sequence.InternalRead";
            cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 128).Value = name;
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                try
                {
                    seq = DataReaderToSequence(r);
                    seq.Validate();
                    ret.Add(seq);
                }
                catch (Exception e)
                {
                    WriteError(seq, e.Message);
                }
            }
            r.Close();
            r = null;
            return ret;
        }
        private static void WriteError(Sequence seq, string error)
        {
            try
            {
                SqlCommand cmd = GetCommand(CommandType.StoredProcedure);
                cmd.CommandText = "Sequence.InternalUpdateError";
                cmd.Parameters.Add("@Error", SqlDbType.VarChar, -1).Value = error == "" ? null : error;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 128).Value = seq.Name;
                cmd.ExecuteNonQuery();
            }
            catch
            { 
            }
        }
        private static void DeleteSequence(Sequence seq)
        {
            SqlCommand cmd = GetCommand(CommandType.StoredProcedure);
            cmd.CommandText = "Sequence.InternalDelete";
            cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 128).Value = seq.Name;
            cmd.ExecuteNonQuery();
        }
        private static void UpdateSequence(Sequence seq)
        {
            SqlCommand cmd = GetCommand(CommandType.StoredProcedure);
            cmd.CommandText = "Sequence.InternalUpdate";
            cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 128).Value = seq.Name;
            if(seq.IsNew)
                cmd.Parameters.Add("@CurrentValue", SqlDbType.BigInt).Value = DBNull.Value;
            else 
                cmd.Parameters.Add("@CurrentValue", SqlDbType.BigInt).Value = seq.CurrentValue;
            cmd.Parameters.Add("@StartValue", SqlDbType.BigInt).Value = seq.StartValue;
	        cmd.Parameters.Add("@EndValue", SqlDbType.BigInt).Value = seq.EndValue;
	        cmd.Parameters.Add("@Step", SqlDbType.BigInt).Value = seq.Step;
	        cmd.Parameters.Add("@Cycle", SqlDbType.Bit).Value = seq.Cycle;
            cmd.Parameters.Add("@Cache", SqlDbType.Int).Value = seq.Cache;
            cmd.Parameters.Add("@CachedValue", SqlDbType.BigInt).Value = seq.CachedValue;
            cmd.ExecuteNonQuery();
        }
        #endregion

        #region Sequence management
        public static void CreateOrUpdate(Sequence seq, bool addToQueue)
        {
            SequenceCommonDataArea.RunCommand(seq, SequenceProcessType.Update, false);
            if (addToQueue)
                StaticSequences.Add(seq);
        }
        public static void Remove(string name)
        {
            
            Sequence seq = null;
            if (name.Substring(0, 1) == "#")
            {
                int spid = GetCurrentSpid();
                var a1 = SequenceCommonDataArea.SessionSequences.Where(x => x.SPID == spid).FirstOrDefault();
                if (a1 != null)
                {
                    seq = a1.Sequences.Where(x => x.Name == name).FirstOrDefault();
                    if (seq != null)
                    {
                        a1.Sequences.Remove(seq);
                        return;
                    }
                }
            }
            else
            {
                SequenceCommonDataArea.CheckService();
                seq = SequenceCommonDataArea.StaticSequences.Where(x => (x.Name == name)).FirstOrDefault();
                if (seq != null)
                {
                    SequenceCommonDataArea.RunCommand(seq, SequenceProcessType.Delete, false);
                    StaticSequences.Remove(seq);
                    return;
                }
            }
            throw new Exception("Could not find Squence " + name + ".");
        }
        public static Sequence GetSessionSequence(string name, int spid)
        {
            Sequence ret = null;
            if (SessionSequences == null)
                SessionSequences = new List<SessionSequence>();
            SessionSequence session = SessionSequences.Where(x => x.SPID == spid).FirstOrDefault();
            if (session == null)
            {
                session = new SessionSequence(spid);
                SessionSequences.Add(session);
            }
            ret = session.Sequences.Where(x => x.Name == name).FirstOrDefault();
            if (ret == null)
            {
                ret = new Sequence();
                ret.SPID = spid;
                ret.Name = name;
                session.Sequences.Add(ret);
            }
            
            return ret;
        }
        public static Sequence GetSequence(string name)
        {
            int spid = 0;
            if (name.Substring(0, 1) == "#")
            {
                spid = GetCurrentSpid();
                return GetSessionSequence(name, spid);
            }
            CheckService();
            Sequence seq = SequenceCommonDataArea.StaticSequences.Where(x => ((x.Name == name) && (x.SPID == spid))).FirstOrDefault();
            return seq;
        }
        #endregion

        #region Service
        private static void GlobalVariableCleanup()
        {
            Connection = null;
            Command = null;
            if (StaticSequences != null)
                StaticSequences.Clear();
            StaticSequences = null;
            if (SessionSequences != null)
            {
                foreach (var x in SessionSequences) x.Clear();
                SessionSequences.Clear();
            }
            SessionSequences = null;
            InternalSession = 0;
            SequenceQueue = null;
            ServiceEvent = null;
        }
        public static void CheckService()
        {
            if (SequenceCommonDataArea.SequenceQueue == null || SequenceCommonDataArea.ServiceEvent == null)
                throw new Exception("Service process is not running.");
        }
        public static SequenceProcess RunCommand(Sequence seq, SequenceProcessType Type, bool TerminateServiceProcess)
        {
            SequenceProcess req = new SequenceProcess();
            req.Sequence = seq;
            req.Type = Type;
            req.TerminateServiceProcess = TerminateServiceProcess;
            CheckService();
            lock (SequenceQueue)
            {
                SequenceQueue.Enqueue(req);
            }
            ServiceEvent.Set();
            req.ClientEvent.WaitOne();
            if (req.Error != null)
                throw req.Error;
            return req;
        }
        public static void StartService()
        {
            GlobalVariableCleanup();
            StaticSequences = ReadSequence();
            SetInternalSessionID();
            ServiceEvent = new ManualResetEvent(false);
            SequenceQueue = new Queue<SequenceProcess>();
            SequenceProcess req = null;
            SqlContext.Pipe.Send("Sequence Service is started at session " + InternalSession.ToString() + ".");
            while (true)
            {
                ServiceEvent.WaitOne();
                lock (SequenceQueue)
                {
                    while (SequenceQueue.Count > 0)
                    {
                        req = null;
                        req = SequenceQueue.Dequeue();
                        if (req.TerminateServiceProcess)
                        {
                            SqlContext.Pipe.Send("A termination signal sent by session " + req.SPID.ToString() + " is received.");
                            req.ClientEvent.Set();
                            ServiceEvent.Reset();
                            
                            SqlContext.Pipe.Send("Sequence Service is being terminated...");
                            while (SequenceQueue.Count > 0)
                            {
                                req = SequenceQueue.Dequeue();
                                req.ClientEvent.Set();
                            }
                            GlobalVariableCleanup();
                            SqlContext.Pipe.Send("Sequence Service is stopped.");
                            return;
                        }
                        if (req == null) continue;
                        try
                        {
                            switch (req.Type)
                            {
                                case SequenceProcessType.Delete:
                                    DeleteSequence(req.Sequence);
                                    break;
                                case SequenceProcessType.Update:
                                    UpdateSequence(req.Sequence);
                                    break;
                                default:
                                    break;
                            }
                        }
                        catch (Exception e)
                        {
                            req.Error = e;
                        }
                        finally
                        {
                            req.ClientEvent.Set();
                        }
                    }
                    ServiceEvent.Reset();
                }
            }
        }
        public static void TerminateServices()
        {
            try
            {
                SequenceCommonDataArea.RunCommand(null, SequenceProcessType.Update, true);
            }
            catch (Exception e)
            {
                SqlContext.Pipe.Send(e.Message);
            }
        }
        #endregion

        #region Math
        public static long Max(this long val1, long val2)
        {
            return Math.Max(val1, val2);
        }
        public static long Min(this long val1, long val2)
        {
            return Math.Min(val1, val2);
        }
        public static bool Between(this long val, long val1, long val2)
        {
            return val >= val1.Min(val2) && val <= val2.Max(val1);
        }
        public static long Abs(this long val)
        {
            return Math.Abs(val);
        }
        #endregion
    }
}
