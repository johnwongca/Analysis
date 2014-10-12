using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Core
{
    public class IndicatorWriter
    {
        public string DatabaseName;
        public IndicatorReader Reader;
        public string TableName;
        string TableDefinition()
        {
            string ret;
            ret = string.Format(@"if object_id('[{0}]') is not null
    drop table [{0}]
create table [{0}](", TableName);
            for (int i = 0; i < Reader.DataSetDefinition.ColumnCount; i++)
            {
               ret = ret  + Reader.DataSetDefinition.Columns[i].SQLDefinition +",";
            }
            ret = ret + " primary key (___RowNumber___))";
            return ret;
        }
        public void WriteToServer()
        {
            using (SqlConnection connection = Methods.GetConnection())
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "use " + DatabaseName;
                command.ExecuteNonQuery();
                command.CommandText = TableDefinition();
                command.ExecuteNonQuery();
                using (SqlBulkCopy bulkcopy = new SqlBulkCopy(connection))
                {
                    bulkcopy.DestinationTableName = "["+TableName+"]";
                    bulkcopy.WriteToServer(Reader);
                }
            }
        }
    }
}
