using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Transactions;
using System.Xml;
using Microsoft.SqlServer.Server;
using Sequence;


public partial class CLRSequence
{
    [SqlProcedure(Name = "Service")]
    public static void Service()
    {
        SequenceCommonDataArea.StartService();
    }
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void TerminateService()
    {
        SequenceCommonDataArea.TerminateServices();
    }
    [SqlFunction(DataAccess = DataAccessKind.Read, IsDeterministic = false)]
    public static SqlString CreateOrAlter(SqlString Name, SqlInt64 StartValue, SqlInt64 EndValue, SqlInt64 Step, SqlBoolean Cycle, SqlInt32 Cache, SqlInt64 CurrentValue)
    {
        Sequence.Sequence seq1 = SequenceCommonDataArea.GetSequence(Name.Value);
        Sequence.Sequence seq = null;
        if (seq1 == null)
        {
            seq = new Sequence.Sequence();
            seq.Name = Name.Value;
            if (!StartValue.IsNull) seq.StartValue = StartValue.Value;
            if (!EndValue.IsNull) seq.EndValue = EndValue.Value;
            if (!Step.IsNull) seq.Step = Step.Value;
            if (!Cycle.IsNull) seq.Cycle = Cycle.Value;
            if (!Cache.IsNull) seq.Cache = Cache.Value;
            if (!CurrentValue.IsNull) seq.mCurrentValue = CurrentValue.Value;
            seq.Validate();
            SequenceCommonDataArea.CreateOrUpdate(seq, true);
        }
        else
        {
            //seq = (Sequence.Sequence)seq1.Clone();
            seq = seq1;

            Sequence.Sequence.Validate(
                    StartValue.IsNull ? seq.StartValue : StartValue.Value,
                    EndValue.IsNull ? seq.EndValue : EndValue.Value,
                    Step.IsNull ? seq.Step : Step.Value,
                    Cycle.IsNull ? seq.Cycle : Cycle.Value,
                    Cache.IsNull ? seq.Cache : Cache.Value,
                    CurrentValue.IsNull ? seq.CurrentValue : CurrentValue.Value,
                    CurrentValue.IsNull ? true : seq.IsNew);
            seq.Name = Name.Value;
            if (!StartValue.IsNull) seq.StartValue = StartValue.Value;
            if (!EndValue.IsNull) seq.EndValue = EndValue.Value;
            if (!Step.IsNull) seq.Step = Step.Value;
            if (!Cycle.IsNull) seq.Cycle = Cycle.Value;
            if (!Cache.IsNull) seq.Cache = Cache.Value;
            if (!CurrentValue.IsNull)
            {
                seq.mCurrentValue = CurrentValue.Value;
                seq.IsNew = true;
            }
            seq.Validate();
            SequenceCommonDataArea.CreateOrUpdate(seq, false);
        }       
        return Name;
    }
    [SqlFunction(DataAccess = DataAccessKind.Read, IsDeterministic = false)]
    public static SqlString Remove(SqlString Name)
    {
        SequenceCommonDataArea.Remove(Name.Value);
        return Name;
    }
    [SqlFunction(DataAccess = DataAccessKind.Read, IsDeterministic = false)]
    public static SqlInt64 NextVal(SqlString Name)
    {
        Sequence.Sequence seq = SequenceCommonDataArea.GetSequence(Name.Value);
        if (seq == null)
            return SqlInt64.Null;
            //throw new Exception("Could not find Squence " + Name.Value + ".");
        return new SqlInt64(seq.NextValue);
    }
    [SqlFunction(DataAccess = DataAccessKind.Read, IsDeterministic = false)]
    public static SqlInt64 SessionNextVal(SqlString Name, SqlInt16 SPID)
    {
        if (Name.Value.Substring(0, 1) != "#")
            throw new Exception("SessionNextVal only supports session dependent sequence.");
        Sequence.Sequence seq = SequenceCommonDataArea.GetSessionSequence(Name.Value, SPID.Value);
        if (seq == null)
            return SqlInt64.Null;
        return new SqlInt64(seq.NextValue);
    }
    [SqlFunction(DataAccess = DataAccessKind.Read, FillRowMethodName = "List_FillRow", TableDefinition = "N bigint", IsDeterministic = false)]
    public static IEnumerable ListVal(SqlString Name, SqlInt32 N)
    {
        List<long> ret = new List<long>();
        Sequence.Sequence seq = SequenceCommonDataArea.GetSequence(Name.Value);
        if (seq == null)
            return ret;
            //throw new Exception("Could not find Squence " + Name.Value + ".");
        if (N.Value <= 0)
            throw new Exception("N must be >=0.");
        
        for (var i = 0; i < N.Value; i++) ret.Add(seq.NextValue);
        return ret;
    }
    public static void List_FillRow(object obj, out SqlInt64 N)
    {
        N = new SqlInt64((long)obj);
        return;
    }
    [SqlFunction(DataAccess = DataAccessKind.Read, IsDeterministic = false)]
    public static SqlInt64 CurrentVal(SqlString Name)
    {
        Sequence.Sequence seq = SequenceCommonDataArea.GetSequence(Name.Value);
        if (seq == null)
            return SqlInt64.Null;
        return new SqlInt64(seq.CurrentValue);
    }
    [SqlFunction(DataAccess = DataAccessKind.Read, FillRowMethodName = "ListSequence_FillRow", TableDefinition = "SPID int, Name nvarchar(128), CurrentValue bigint, StartValue bigint, EndValue bigint, Step bigint, Cycle bit, Cache int, CachedValue bigint", IsDeterministic = false)]
    public static IEnumerable ListSequence(SqlString type)
    {
        List<Sequence.Sequence> ret = new List<Sequence.Sequence>();        
        if (type.IsNull || type.Value.Trim().ToUpper() == "ALL")
        {
            if (SequenceCommonDataArea.StaticSequences != null)
                ret.AddRange(SequenceCommonDataArea.StaticSequences);
            if (SequenceCommonDataArea.SessionSequences != null)
            {
                foreach (var i1 in SequenceCommonDataArea.SessionSequences)
                    foreach (var i2 in i1.Sequences)
                        ret.Add(i2);
            }
        }
        else if (type.Value.Trim().ToUpper() == "ALLSTATIC")
        {
            if (SequenceCommonDataArea.StaticSequences != null)
                ret.AddRange(SequenceCommonDataArea.StaticSequences);
        }
        else if (type.Value.Trim().ToUpper() == "ALLSESSION")
        {
            if (SequenceCommonDataArea.SessionSequences != null)
            {
                foreach (var i1 in SequenceCommonDataArea.SessionSequences)
                    foreach (var i2 in i1.Sequences)
                        ret.Add(i2);
            }
        }
        else if (type.Value.Trim().ToUpper() == "THISSESSION")
        {
            if (SequenceCommonDataArea.SessionSequences != null)
            {
                int spid = SequenceCommonDataArea.GetCurrentSpid();
                var p = SequenceCommonDataArea.SessionSequences.Where(x=>x.SPID == spid);
                foreach (var i1 in p)
                    foreach (var i2 in i1.Sequences)
                        ret.Add(i2);
            }
        }
        return ret.OrderBy(x=> x.SPID).ThenBy(x=>x.Name);
    }
    public static void ListSequence_FillRow(object obj, out SqlInt32 SPID, out SqlChars Name, out SqlInt64 CurrentValue, out SqlInt64 StartValue, out SqlInt64 EndValue, out SqlInt64 Step, out SqlBoolean Cycle, out SqlInt32 Cache, out SqlInt64 CachedValue )
    {
        Sequence.Sequence seq = (Sequence.Sequence)obj;
        SPID = seq.SPID == 0? SqlInt32.Null : new SqlInt32(seq.SPID);
        Name = new SqlChars(seq.Name);
        CurrentValue = new SqlInt64(seq.IsNew ? seq.StartValue : seq.mCurrentValue);
        StartValue = new SqlInt64(seq.StartValue);
        EndValue = new SqlInt64(seq.EndValue);
        Step = new SqlInt64(seq.Step);
        Cycle = new SqlBoolean(seq.Cycle);
        CachedValue = new SqlInt64(seq.CachedValue);
        Cache = new SqlInt32(seq.Cache);
    }
    [SqlFunction(DataAccess=DataAccessKind.Read, IsDeterministic = false)]
    public static SqlInt32 SessionReset()
    {
        int spid = SequenceCommonDataArea.GetCurrentSpid();
        if (SequenceCommonDataArea.SessionSequences == null)
            return new SqlInt32(1);
        var sseq = SequenceCommonDataArea.SessionSequences.FirstOrDefault(x => x.SPID == spid);
        if(sseq!=null)
            SequenceCommonDataArea.SessionSequences.Remove(sseq);
        return new SqlInt32(1);
    }
    /*[SqlTrigger(Name="SequenceMonitorDataModification", Target = "Sequence", Event="FOR UPDATE")]*/
    public static void SequenceMonitorDataModification()
    {
        if(SequenceCommonDataArea.InternalSession!= SequenceCommonDataArea.GetCurrentSpid())
        {
            try
            {
                SqlContext.Pipe.Send("Table could only be changed by Sequence internal process.");
                Transaction.Current.Rollback();
            }
            catch
            { 
            }
        }
    }
};
