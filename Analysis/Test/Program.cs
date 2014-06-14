using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static Regex SQLParameterRegEx = new Regex(@"[()\.\s\,]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        static void Main(string[] args)
        {
            SqlParameter ret = check("@abc int",  new SqlString("5"));
            Console.WriteLine(ret);
            Console.WriteLine("Done");
            Console.ReadKey();
        }
        static SqlParameter check(string name, object value)
        {
            SqlParameter ret = new SqlParameter();
            string[] type = SQLParameterRegEx.Split(name).Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim()).ToArray();
            if (type.Length == 0)
                throw new Exception("Cannot parse parameter name and type: " + name);
            ret.ParameterName = type[0].Substring(0, 1) != "@" ? "@" + type[0] : type[0];
            if (type.Length == 1)
            {
                ret.Value = value;
                return ret;
            }

            //set value
            ret.Value = value is DBNull ? DBNull.Value : value.GetType().GetProperty("Value").GetValue(value);
            ret.SqlValue = value;
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
    }
}
