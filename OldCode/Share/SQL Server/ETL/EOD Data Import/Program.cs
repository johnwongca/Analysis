using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using FTPLib;
using DataIO;

namespace EOD_Data_Import
{
     
    class EODDataImport
    {
        private SqlConnection connection;// = new SqlConnection("Data Source=(local);Initial Catalog=Stock;Integrated Security=SSPI;");
        private SqlCommand cmdCanImport, cmdImport, cmdError;
        static void Test()
        {
            //FTPSite ftp = new FTPSite("ftp://u38107435-readonly:rfvTGB456$%^@test.ultradyna.com:21/modules/dblog/");
            //FTPSite ftp = new FTPSite("ftp.EODData.com", 21, "jhuang", "euphoria");
            FTPSite ftp = new FTPSite("ftp://jhuang:euphoria@ftp.EODData.com");
            ftp.Connect();
            foreach (var f in ftp.Current.Files)
            {
                Console.WriteLine("{0}{1}---{2}----{3}", f.Path, f.Name, f.Size, f.Date);
                //Console.Read();
                //Console.WriteLine(f.DataString);
                //f.FreeData();
                //Console.Read();
                break;
            }
            /*FTPSite ftp = new FTPSite("ftp://u38107435-readonly:rfvTGB456$%^@test.ultradyna.com:21/modules/dblog/dblog-rtl.css");
            ftp.Connect();
            if (!ftp.Current.IsFolder)
            {
                var f = ftp.Current;
                Console.WriteLine("{0}{1}---{2}-------------------", f.Path, f.Name, f.Size);
                //Console.Read();
                Console.WriteLine(f.DataString);
                f.FreeData();
            }*/

            ftp.Disconnect();
            /*System.Uri u = new Uri("ftp://u38107435-readonly:rfvTGB456$%^@test.ultradyna.com:/a/b/c.dfa/");
            Console.WriteLine(u.UserInfo.Substring(0, u.UserInfo.IndexOf(":")));
            Console.WriteLine(u.UserInfo.Substring(u.UserInfo.IndexOf(":") + 1, u.UserInfo.Length - u.UserInfo.IndexOf(":") -1));
            Cosole.WriteLine(u);*/
            Console.WriteLine("Done");
            Console.Read();
            return;
        }
        static void Main(string[] args)
        {
            Test();
            Console.WriteLine("done...");
            Console.ReadKey();
            //string serverName = "(local)";
            //string databaseName = "Stock";
            //if (args.Length == 2)
            //{
            //    serverName = args[0];
            //    databaseName = args[1];
            //}

            //Console.WriteLine("Start importing data to {0}.{1}......", serverName, databaseName);
            //(new EODDataImport(serverName, databaseName)).Import();
            //Console.WriteLine("Finished......");
        }
        public EODDataImport(string serverName, string databaseName)
        {
            Initialization(serverName, databaseName);
            
        }
        private void Initialization(string serverName, string databaseName)
        {

            connection = new SqlConnection("Data Source=" + serverName + ";Initial Catalog=" + databaseName + ";Integrated Security=SSPI;");
            cmdCanImport = connection.CreateCommand();
            cmdCanImport.CommandType = CommandType.StoredProcedure;
            cmdCanImport.CommandText = "imp.Importable";
            cmdCanImport.Parameters.Add(new SqlParameter("@LocationID", SqlDbType.Int));
            cmdCanImport.Parameters.Add(new SqlParameter("@FileName", SqlDbType.NVarChar, 256));
            cmdCanImport.Parameters.Add(new SqlParameter("@FileSize", SqlDbType.BigInt));
            cmdCanImport.Parameters.Add(new SqlParameter("@FileDate", SqlDbType.DateTime));
            cmdCanImport.Parameters.Add(new SqlParameter("@Importable", SqlDbType.Int)).Direction = ParameterDirection.Output;

            cmdImport = connection.CreateCommand();
            cmdImport.CommandType = CommandType.StoredProcedure;
            cmdImport.CommandText = "imp.Import";
            cmdImport.Parameters.Add(new SqlParameter("@LocationID", SqlDbType.Int));
            cmdImport.Parameters.Add(new SqlParameter("@FileName", SqlDbType.NVarChar, 256));
            cmdImport.Parameters.Add(new SqlParameter("@FileSize", SqlDbType.BigInt));
            cmdImport.Parameters.Add(new SqlParameter("@FileDate", SqlDbType.DateTime));
            cmdImport.Parameters.Add(new SqlParameter("@Text", SqlDbType.VarChar, -1));
            cmdImport.Parameters.Add(new SqlParameter("@Error", SqlDbType.VarChar, -1));

            cmdError = connection.CreateCommand();
            cmdError.CommandType = CommandType.StoredProcedure;
            cmdError.CommandText = "imp.ImportError";
            cmdError.Parameters.Add(new SqlParameter("@LocationID", SqlDbType.Int));
            cmdError.Parameters.Add(new SqlParameter("@Message", SqlDbType.VarChar, -1));
            return;
        }
        private void OpenConnection()
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
        }
        private void Import()
        {

            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "imp.Cleanup";
            OpenConnection();
            cmd.ExecuteNonQuery();
            cmd.CommandText = "imp.GetLocation";
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            foreach (DataRow r in dt.Rows)
            {
                ImportLocation((int)r["LocationID"], r["SourceFolder"].ToString());
            }
        }
        private void ImportLocation(int locationID, string path)
        {
            
            DateTime fileDate;
            int retries = 0;
            Exception ee = null;
            try
            {
                Console.WriteLine("Connecting location id {0}, path = {1}", locationID, path);
                List<IFile> files = FInfo.CreateFile(path).ReadFolder();
                foreach (IFile f in files)
                {
                    retries = 0;
                    while (retries < 3)
                    {
                        try
                        {
                            f.ReadFileInfo();
                            retries = 10;
                        }
                        catch 
                        {
                            retries++;
                            Thread.Sleep(1000);
                        }
                    }
                    if(retries!=10)
                        f.ReadFileInfo();
                    fileDate = f.FileInfo.FileDate == DateTime.MinValue ? new DateTime(1901, 1, 1) : f.FileInfo.FileDate;
                    if (CanImport(locationID, f.FileInfo.FileName, f.FileInfo.Size, fileDate) == 1)
                    {
                        Console.WriteLine("Downloading {0}, Size = {1}", f.FileInfo.FileName, f.FileInfo.Size);
                        retries = 0;
                        while (retries < 3)
                        {
                            try
                            {
                                ee = null;
                                f.Read();
                                retries = 10;
                                ee = null;
                            }
                            catch (Exception e)
                            {
                                retries++;
                                ee = e;
                                Thread.Sleep(1000);
                            }
                        }
                        if(ee != null)
                            Console.WriteLine("File {0} is downloaded with error {1}", f.FileInfo.FileName, ee.Message);
                        Import(locationID, f.FileInfo.FileName, f.FileInfo.Size, fileDate, f.FileInfo.Body, ee == null ? "" : "Downloading file " + f.FileInfo.FileName + "\n" + ee.Message);
                    }
                }
            }
            catch(Exception e2)
            {
                ImportError(locationID, e2.ToString());
                Console.WriteLine("Error reading path...");
                Console.WriteLine(path);
                Console.WriteLine(e2);
            }
        }
        private void Quit()
        {
            connection.Close();
        }
        private int CanImport(int locationID, string fileName, long size, DateTime fileDate)
        {
            /*return 1 when it's importable*/
            if (connection.State != ConnectionState.Open)
                connection.Open();
            cmdCanImport.Parameters["@LocationID"].Value = locationID;
            cmdCanImport.Parameters["@FileName"].Value = fileName;
            cmdCanImport.Parameters["@FileSize"].Value = size;
            cmdCanImport.Parameters["@FileDate"].Value = fileDate;
            cmdCanImport.ExecuteNonQuery();
            return (int)(cmdCanImport.Parameters["@Importable"].Value);
        }
        private void ImportError(int locationID, string message)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            cmdError.Parameters["@LocationID"].Value = locationID;
            cmdError.Parameters["@Message"].Value = message;
            cmdError.ExecuteNonQuery();
            return;
        }
        private void Import(int locationID, string fileName, long size, DateTime fileDate, string text, string error)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            cmdImport.Parameters["@LocationID"].Value = locationID;
            cmdImport.Parameters["@FileName"].Value = fileName;
            cmdImport.Parameters["@FileSize"].Value = size;
            cmdImport.Parameters["@FileDate"].Value = fileDate;
            cmdImport.Parameters["@Text"].Value = text;
            cmdImport.Parameters["@Error"].Value = error;
            cmdImport.ExecuteNonQuery();
            return;
        }
        
    }
    
}
