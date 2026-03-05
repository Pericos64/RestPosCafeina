using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Finisar.SQLite;
//using System.Data.SQLite;

namespace RestPOS 
{
	//////SQLite Edition
    class DataAccess
    {
	    
	    // Connection String for  SQlite Edition
        static string _ConnectionString = @"Data Source=RestPOS.db;Version=3;New=False;Compress=True";
							//Data Source=DemoT.db;Version=3;New=False;Compress=True;
	    
	    // Use for RestPOS.exe.config file 
	    //   static string _ConnectionString = RestPOS.Properties.Settings.Default.restConnectionString1;

	    //Its absolute Connection String for MS SQL Server 2008 - Upto
        //static string _ConnectionString = "Data Source=(local);Initial Catalog=restpos; User ID=sa;Password=sapass!";
	    

	    //This is Mysql Database Access        
        // static string _ConnectionString = "server=localhost; database=restpos; uid=root; PASSWORD=1234";


	
  
        static SQLiteConnection _Connection = null;
        public static SQLiteConnection Connection
        {
            get
            {
                if (_Connection == null)
                {
                    _Connection = new SQLiteConnection(_ConnectionString);
                    _Connection.Open();

                    return _Connection;
                }
                else if (_Connection.State != System.Data.ConnectionState.Open)
                {
                    _Connection.Open();

                    return _Connection;
                }
                else
                {
                    return _Connection;
                }
            }
        }

        public static DataSet GetDataSet(string sql)
        {
            SQLiteCommand cmd = new SQLiteCommand(sql, Connection);
            SQLiteDataAdapter adp = new SQLiteDataAdapter(cmd);

            DataSet ds = new DataSet();
            adp.Fill(ds);
            Connection.Close();

            return ds;
        }

        public static DataTable GetDataTable(string sql)
        {
            Console.WriteLine(sql);
            DataSet ds = GetDataSet(sql);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
          //  Connection.Close(); //New add
            return null;
        }

        public static int ExecuteSQL(string sql)
        {
            SQLiteCommand cmd = new SQLiteCommand(sql, Connection);
            return cmd.ExecuteNonQuery();
        }
    }

}
