using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public class FunctionRunSQL
{
    private static object RunSQL(string SQL)
    {
        if (SQL == null)
            return null;
        if (SQL == "")
            return null;
        object ret = null;
        SqlConnection conn = new SqlConnection("Context Connection = True;");
        SqlCommand cmd = conn.CreateCommand();
        SqlDataReader r = null;
        try
        {
            conn.Open();
            cmd.CommandTimeout = 0;
            cmd.CommandText = SQL;
            r = cmd.ExecuteReader();
            if (r != null)
                if (r.Read())
                    if (r.FieldCount > 0)
                        ret = r[0];

        }
        finally
        {
            if (r != null)
                if (!r.IsClosed)
                    r.Close();
            if (conn.State != ConnectionState.Closed)
                conn.Close();
            cmd = null;
        }
        return ret;
    }
    [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read, IsDeterministic = true, IsPrecise = true, SystemDataAccess = SystemDataAccessKind.Read)]
    public static SqlDouble fn_RunSQL_Float(string SQL)
    {
        if (SQL == null)
            return SqlDouble.Null;
        object ret = RunSQL(SQL);
        if ((ret == null) || (ret == DBNull.Value))
            return SqlDouble.Null;
        return new SqlDouble(Convert.ToDouble(ret));
    }
    [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read, IsDeterministic = true, IsPrecise = true, SystemDataAccess = SystemDataAccessKind.Read)]
    public static SqlString fn_RunSQL_Str(string SQL)
    {
        if (SQL == null)
            return SqlString.Null;
        object ret = RunSQL(SQL);
        if ((ret == null) || (ret == DBNull.Value))
            return SqlString.Null;
        return new SqlString(Convert.ToString(ret));
    }
    [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read, IsDeterministic = true, IsPrecise = true, SystemDataAccess = SystemDataAccessKind.Read)]
    public static SqlInt32 fn_RunSQL_Int(string SQL)
    {
        if (SQL == null)
            return SqlInt32.Null;
        object ret = RunSQL(SQL);
        if ((ret == null) || (ret == DBNull.Value))
            return SqlInt32.Null;
        return new SqlInt32(Convert.ToInt32(ret));
    }
    [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read, IsDeterministic = true, IsPrecise = true, SystemDataAccess = SystemDataAccessKind.Read)]
    public static SqlInt64 fn_RunSQL_BigInt(string SQL)
    {
        if (SQL == null)
            return SqlInt64.Null;
        object ret = RunSQL(SQL);
        if ((ret == null) || (ret == DBNull.Value))
            return SqlInt64.Null;
        return new SqlInt64(Convert.ToInt64(ret));
    }
    [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read, IsDeterministic = true, IsPrecise = true, SystemDataAccess = SystemDataAccessKind.Read)]
    public static SqlMoney fn_RunSQL_Money(string SQL)
    {
        if (SQL == null)
            return SqlMoney.Null;
        object ret = RunSQL(SQL);
        if ((ret == null) || (ret == DBNull.Value))
            return SqlMoney.Null;
        return new SqlMoney(Convert.ToDecimal(ret));
    }
    [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read, IsDeterministic = true, IsPrecise = true, SystemDataAccess = SystemDataAccessKind.Read)]
    public static object fn_RunSQL(string SQL)
    {
        if (SQL == null)
            return null;
        return RunSQL(SQL);
        
    }
    [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read, IsDeterministic = true, IsPrecise = true, SystemDataAccess = SystemDataAccessKind.Read)]
    public static SqlBoolean fn_RunSQL_Bit(string SQL)
    {
        if (SQL == null)
            return SqlBoolean.Null;
        return new SqlBoolean(Convert.ToBoolean(RunSQL(SQL)));

    }
    [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read, IsDeterministic = true, IsPrecise = true, SystemDataAccess = SystemDataAccessKind.Read)]
    public static SqlDateTime fn_RunSQL_DateTime(string SQL)
    {
        if (SQL == null)
            return SqlDateTime.Null;
        object ret = RunSQL(SQL);
        if ((ret == null) || (ret == DBNull.Value))
            return SqlDateTime.Null;
        return new SqlDateTime(Convert.ToDateTime(ret));
    }
};

