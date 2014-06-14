//------------------------------------------------------------------------------
// <copyright file="CSSqlClassFile.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;
namespace sqlnotes.info
{
    public class SessionStore<T> where T:new()
    {
        object SyncObject = new object();
        List<T> buffer = new List<T>();
        void SetSize(short size)
        {
            lock (SyncObject)
            {
                while (buffer.Count <= size + 50)
                {
                    buffer.Add(new T());
                }
            }
        }
        public T GetValue(short spid)
        {
            SetSize(spid);
            return buffer[spid];
        }
        public T SetValue(short spid, T value)
        {
            SetSize(spid);
            buffer[spid] = value;
            return value;
        }
    }
    public class SessionParameterItem
    {
        static Regex SQLParameterRegEx = new Regex(@"[()\.\s\,]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        SqlParameter mParameter = new SqlParameter();
        string mName = "";
        object mValue = DBNull.Value;
        public string Name
        {
            get { return mName; }
            set
            {
                PopulateParameter(value, mValue);
                mName = value;                
            }
        }
        public object Value
        {
            get { return mValue; }
            set
            {
                PopulateParameter(mName, value);
                mValue = value;
            }
        }
        public SqlParameter Parameter
        {
            get { return mParameter; }
        }
        void PopulateParameter(string name, object value)
        {
            
            string[] type = SQLParameterRegEx.Split(name).Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim()).ToArray();
            if (type.Length == 0)
                throw new Exception("Cannot parse parameter name and type: " + name);
            mParameter.ParameterName = type[0].Substring(0, 1) != "@" ? "@" + type[0] : type[0];
            if (type.Length == 1)
            {
                mParameter.Value = value;
                return;
            }
            mParameter.Value = value is DBNull ? DBNull.Value : value.GetType().GetProperty("Value").GetValue(value);
            mParameter.Direction = ParameterDirection.Input;
            if (type[type.Length - 1].ToLower().Trim() == "output")
            {
                mParameter.Direction = ParameterDirection.InputOutput;
            }
            SqlDbType dataType;
            if (!Enum.TryParse<SqlDbType>(type[1], true, out dataType))
            {
                throw new Exception("Cannot reganize type " + type[1]);
            }
            mParameter.SqlDbType = dataType;

            if (type.Length == 2)
                return;
            switch (dataType)
            {
                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.Binary:
                    mParameter.Size = int.Parse(type[2]);
                    break;
                case SqlDbType.Decimal:
                    mParameter.Precision = byte.Parse(type[2]);
                    if (type.Length >= 4)
                        mParameter.Scale = byte.Parse(type[3]);
                    break;
                case SqlDbType.DateTime2:
                case SqlDbType.DateTimeOffset:
                case SqlDbType.Time:
                    if (type.Length >= 3)
                        mParameter.Scale = byte.Parse(type[2]);
                    break;
                case SqlDbType.VarChar:
                case SqlDbType.NVarChar:
                case SqlDbType.VarBinary:
                    mParameter.Size = type[2].ToLower() == "max" ? -1 : int.Parse(type[2]);
                    break;
                default:
                    break;
            }
        }
        
        public SessionParameterItem(string name, object value)
        {
            PopulateParameter(name, value);
            mName = name;
            mValue = value;
        }
    }
    public class SessionParameter
    {
        static Regex SQLParameterRegEx = new Regex(@"[()\.\s\,]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        static SessionStore<SessionParameter> SessionParameterStore = new SessionStore<SessionParameter>();
        SqlParameter ParseParameterDefinition(string definition, object value)
        {
            SqlParameter ret = new SqlParameter();
            string[] type = SQLParameterRegEx.Split(definition).Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim()).ToArray();
            if (type.Length == 0)
                throw new Exception("Cannot parse parameter name and type: " + definition);
            ret.ParameterName = type[0].Substring(0, 1) != "@" ? "@" + type[0] : type[0];
            if (type.Length == 1)
            {
                ret.Value = value;
                return ret;
            }
            ret.Value = value is DBNull ? DBNull.Value : value.GetType().GetProperty("Value").GetValue(value);
            ret.Direction = ParameterDirection.Input;
            if (type[type.Length - 1].ToLower().Trim() == "output")
            {
                ret.Direction = ParameterDirection.InputOutput;
            }
            SqlDbType dataType;
            if (!Enum.TryParse<SqlDbType>(type[1], true, out dataType))
            {
                throw new Exception("Cannot reganize type " + type[1]);
            }
            ret.SqlDbType = dataType;

            if (type.Length == 2)
                return ret;
            switch (dataType)
            {
                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.Binary:
                    ret.Size = int.Parse(type[2]);
                    break;
                case SqlDbType.Decimal:
                    ret.Precision = byte.Parse(type[2]);
                    if (type.Length >= 4)
                        ret.Scale = byte.Parse(type[3]);
                    break;
                case SqlDbType.DateTime2:
                case SqlDbType.DateTimeOffset:
                case SqlDbType.Time:
                    if (type.Length >= 3)
                        ret.Scale = byte.Parse(type[2]);
                    break;
                case SqlDbType.VarChar:
                case SqlDbType.NVarChar:
                case SqlDbType.VarBinary:
                    ret.Size = type[2].ToLower() == "max" ? -1 : int.Parse(type[2]);
                    break;
                default:
                    break;
            }
            return ret;
        }
        List<SqlParameter> list = new List<SqlParameter>();
        public static SessionParameter GetParameter(short sessionid)
        {
            return SessionParameterStore.GetValue(sessionid);
        }
        public SqlParameter SetValue(string definition, object value)
        {
            definition = definition.Trim();
            SqlParameter param = ParseParameterDefinition(definition, value);
            SqlParameter existingItem = list.FirstOrDefault(x => x.ParameterName.ToUpper() == param.ParameterName.ToUpper());
            if (existingItem == default(SqlParameter))
            {
                list.Add(param);
                return param;
            }
            existingItem.Value = value;
            existingItem.SqlDbType = param.SqlDbType;
            existingItem.Direction = param.Direction;
            existingItem.Size = param.Size;
            existingItem.Scale = param.Scale;
            existingItem.Precision = param.Precision;
            return existingItem;
        }
        public SqlParameter SetValue(SqlParameter p)
        {
            SqlParameter existingItem = list.FirstOrDefault(x => x.ParameterName.ToUpper() == p.ParameterName.ToUpper());
            if (existingItem == default(SqlParameter))
            {
                list.Add(p);
                return p;
            }
            existingItem.Value = p.Value;
            existingItem.SqlDbType = p.SqlDbType;
            existingItem.Direction = p.Direction;
            existingItem.Size = p.Size;
            existingItem.Scale = p.Scale;
            existingItem.Precision = p.Precision;
            return existingItem;
        }
        public void Clear()
        {
            list.Clear();
        }
        public bool Remove(string name)
        {
            SqlParameter existingItem = list.FirstOrDefault(x => x.ParameterName.ToUpper() == name.ToUpper());
            if (existingItem != default(SqlParameter))
            {
                list.Remove(existingItem);
                return true;
            }
            return false;
        }
        public SqlParameter GetItem(string name)
        {
            name = name.Trim().ToUpper();
            return list.FirstOrDefault(x => x.ParameterName.ToUpper() == name);
        }
        public SqlParameter[] GetParameters()
        {
            return list.Where(x => x.ParameterName.Substring(0, 4) != "@___").ToArray();
        }
        [SqlFunction]
        public static object SessionParameterGetValue(SqlInt16 SPID, SqlString ParameterName)
        {
            SqlParameter value = SessionParameterStore.GetValue(SPID.Value).GetItem(ParameterName.Value);
            return value == null ? DBNull.Value : value.Value;
        }
        [SqlFunction]
        public static SqlInt16 SessionParameterSetValue(SqlInt16 SPID, SqlString Definition, object Value)
        {
            SessionParameterStore.GetValue(SPID.Value).SetValue(Definition.Value, Value);
            return SPID;
        }
        [SqlFunction]
        public static SqlChars SessionParameterGetValueVarcharMax(SqlInt16 SPID, SqlString ParameterName)
        {
            SqlParameter value = SessionParameterStore.GetValue(SPID.Value).GetItem(ParameterName.Value);
            return value == null ? SqlChars.Null: (SqlChars)(value.SqlValue);
        }
        [SqlFunction]
        public static SqlInt16 SessionParameterSetValueVarcharMax(SqlInt16 SPID, SqlString Definition, SqlChars Value)
        {
            SqlParameter p = SessionParameterStore.GetValue(SPID.Value).SetValue(Definition.Value, new SqlChars(Value.Value));
            p.SqlDbType = SqlDbType.NVarChar;
            p.Size = -1;
            return SPID;
        }
        [SqlFunction]
        public static SqlBytes SessionParameterGetValueVarbinaryMax(SqlInt16 SPID, SqlString ParameterName)
        {
            SqlParameter value = SessionParameterStore.GetValue(SPID.Value).GetItem(ParameterName.Value);
            return value == null ? SqlBytes.Null : (SqlBytes)(value.SqlValue);
        }

        [SqlFunction]
        public static SqlInt16 SessionParameterSetValueVarbinaryMax(SqlInt16 SPID, SqlString Definition, SqlBytes Value)
        {
            SqlParameter p = SessionParameterStore.GetValue(SPID.Value).SetValue(Definition.Value, new SqlBytes(Value.Value));
            p.SqlDbType = SqlDbType.VarBinary;
            p.Size = -1;
            return SPID;
        }
        [SqlFunction]
        public static SqlInt16 SessionParameterClear(SqlInt16 SPID)
        {
            SessionParameterStore.GetValue(SPID.Value).Clear();
            return SPID;
        }
        [SqlFunction]
        public static SqlInt16 SessionParameterRemove(SqlInt16 SPID, SqlString ParameterName)
        {
            SessionParameterStore.GetValue(SPID.Value).Remove(ParameterName.Value);
            return SPID;
        }
        

        [SqlFunction(FillRowMethodName = "SessionParameterListDefinition_FillRow", TableDefinition = "ParameterID int, ParameterName nvarchar(4000), Type nvarchar(128), Size int, Precision tinyint, Scale tinyint, Direction nvarchar(12)")]
        public static IEnumerable SessionParameterListDefinition(SqlInt16 SPID)
        {
            var list = SessionParameterStore.GetValue(SPID.Value);
            for (int i = 0; i < list.list.Count; i++)
            {
                yield return new KeyValuePair<int, SqlParameter>(i + 1,list.list[i]);
            }
        }
        public static void SessionParameterListDefinition_FillRow(object o, out SqlInt32 ParameterID, out SqlString ParameterName, out SqlString Type, out SqlInt32 Size, out SqlByte Precision, out SqlByte Scale, out SqlString Direction)//, out object Value)
        {
            KeyValuePair<int, SqlParameter> data = (KeyValuePair<int, SqlParameter>)o;
            ParameterID = new SqlInt32(data.Key);
            ParameterName = new SqlString(data.Value.ParameterName);
            Type = new SqlString(data.Value.SqlDbType.ToString());
            Size = new SqlInt32(data.Value.Size);
            Precision = new SqlByte(data.Value.Precision);
            Scale = new SqlByte(data.Value.Scale);
            Direction = new SqlString(data.Value.Direction.ToString());
            //Value = data.Value.Value;
        }
    }

    public class SessionInfoItem
    {
        public string Name;
        public object Value;
    }
    public class SessionInfo
    {
        List<SessionInfoItem> list = new List<SessionInfoItem>();

        static SessionStore<SessionInfo> SessionInfoStore = new SessionStore<SessionInfo>();
        public SessionInfoItem SetValue(string name, object value)
        {
            SessionInfoItem param = new SessionInfoItem() { Name = name.Trim(), Value = value };
            SessionInfoItem existingItem = list.FirstOrDefault(x => x.Name.ToUpper() == param.Name.ToUpper());
            if (existingItem == default(SessionInfoItem))
            {
                list.Add(param);
                return param;
            }
            existingItem.Value = value;
            return existingItem;
        }
        public SessionInfoItem SetValue(SessionInfoItem p)
        {
            SessionInfoItem existingItem = list.FirstOrDefault(x => x.Name.ToUpper() == p.Name.ToUpper());
            if (existingItem == default(SessionInfoItem))
            {
                list.Add(p);
                return p;
            }
            existingItem.Value = p.Value;
            return existingItem;
        }
        public void Clear()
        {
            list.Clear();
        }
        public bool Remove(string name)
        {
            SessionInfoItem existingItem = list.FirstOrDefault(x => x.Name.ToUpper() == name.ToUpper());
            if (existingItem != default(SessionInfoItem))
            {
                list.Remove(existingItem);
                return true;
            }
            return false;
        }
        public SessionInfoItem GetItem(string name)
        {
            name = name.Trim().ToUpper();
            return list.FirstOrDefault(x => x.Name.ToUpper() == name);
        }

        [SqlFunction]
        public static object SessionInfoGetValue(SqlInt16 SPID, SqlString Name)
        {
            SessionInfoItem value = SessionInfoStore.GetValue(SPID.Value).GetItem(Name.Value);
            return value == null ? DBNull.Value : value.Value;
        }
        [SqlFunction]
        public static SqlInt16 SessionInfoSetValue(SqlInt16 SPID, SqlString Name, object Value)
        {
            SessionInfoStore.GetValue(SPID.Value).SetValue(Name.Value, Value);
            return SPID;
        }
        [SqlFunction]
        public static SqlChars SessionInfoGetValueVarcharMax(SqlInt16 SPID, SqlString Name)
        {
            SessionInfoItem value = SessionInfoStore.GetValue(SPID.Value).GetItem(Name.Value);
            return value == null ? SqlChars.Null : (SqlChars)(value.Value);
        }
        [SqlFunction]
        public static SqlInt16 SessionInfoSetValueVarcharMax(SqlInt16 SPID, SqlString Name, SqlChars Value)
        {
            SessionInfoStore.GetValue(SPID.Value).SetValue(Name.Value, new SqlChars(Value.Value));
            return SPID;
        }
        [SqlFunction]
        public static SqlBytes SessionInfoGetValueVarbinaryMax(SqlInt16 SPID, SqlString Name)
        {
            SessionInfoItem value = SessionInfoStore.GetValue(SPID.Value).GetItem(Name.Value);
            return value == null ? SqlBytes.Null : (SqlBytes)(value.Value);
        }

        [SqlFunction]
        public static SqlInt16 SessionInfoSetValueVarbinaryMax(SqlInt16 SPID, SqlString Name, SqlBytes Value)
        {
            SessionInfoStore.GetValue(SPID.Value).SetValue(Name.Value, new SqlBytes(Value.Value));
            return SPID;
        }
        [SqlFunction]
        public static SqlInt16 SessionInfoClear(SqlInt16 SPID)
        {
            SessionInfoStore.GetValue(SPID.Value).Clear();
            return SPID;
        }
        [SqlFunction]
        public static SqlInt16 SessionInfoRemove(SqlInt16 SPID, SqlString Name)
        {
            SessionInfoStore.GetValue(SPID.Value).Remove(Name.Value);
            return SPID;
        }


        [SqlFunction(FillRowMethodName = "SessionInfoListDefinition_FillRow", TableDefinition = "ID int, Name nvarchar(4000), Type nvarchar(128)")]
        public static IEnumerable SessionInfoListDefinition(SqlInt16 SPID)
        {
            var list = SessionInfoStore.GetValue(SPID.Value);
            for (int i = 0; i < list.list.Count; i++)
            {
                yield return new KeyValuePair<int, SessionInfoItem>(i + 1, list.list[i]);
            }
        }
        public static void SessionInfoListDefinition_FillRow(object o, out SqlInt32 ID, out SqlString Name, out SqlString Type)
        {
            KeyValuePair<int, SessionInfoItem> data = (KeyValuePair<int, SessionInfoItem>)o;
            ID = new SqlInt32(data.Key);
            Name = new SqlString(data.Value.Name);
            Type = new SqlString(data.Value.Value == null? "Unknown" : data.Value.Value.GetType().ToString());

        }

    }
    
}
