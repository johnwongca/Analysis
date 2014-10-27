using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;
using System.Xml;
using System.Xml.Linq;

namespace Algorithm.Core
{
    //public
    public class ChartList
    {
        public static List<ChartList> Charts = new List<ChartList>();
        public static void LoadCharts()
        {
            using(SqlConnection connection = Methods.GetConnection())
            {
                Charts.Clear();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "q.GetChart";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            Charts.Add(new ChartList() { Name = reader[0].ToString(), Definition = XElement.Parse(reader[1].ToString()) });
                    }
                }
            }

        }
        public string Name;
        public XElement Definition;
        public void Save()
        {
            if (Definition == null)
                return;
            using (SqlConnection connection = Methods.GetConnection())
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "q.UpdateChart";
                    cmd.Parameters.Add("@ChartName", SqlDbType.VarChar, 128).Value = Name;
                    cmd.Parameters.Add("@ChartDefinition", SqlDbType.Xml).Value = Definition.ToString();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
