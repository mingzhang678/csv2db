using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using MySql.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSVtoDatabase;
using MySql.Data.MySqlClient;

namespace CSVtoDatabase
{
    public class Helper
    {
        private static String _connectionString;// = "Data Source=.;Initial Catalog=master;Integrated Security=True"; //ConfigurationManager.ConnectionStrings["DBmaster"].ConnectionString;
        public static SqlConnection sqlConnection = null;
        public static MySqlConnection mySqlConnection = null;
        public Helper(string connectionString)
        {
            _connectionString = connectionString;
        }
        public static DataTable QuerySqlServer(string str)
        {
            sqlConnection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(str, sqlConnection);
            try
            {

                sqlConnection.Open();
            }
            catch (Exception e)
            {
                Program.getMainForm().textBoxLog.Text = $@"Cannot connect to Server with connectionstring {_connectionString}";
            }
            //SqlDataReader sqlDataReader = command.ExecuteReader();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            sqlDataAdapter.Fill(table);
            sqlDataAdapter.Dispose();
            return table;
        }

        public static DataTable QuerySqlServer(string queryString, string connectionString)
        {
            sqlConnection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(queryString, sqlConnection);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception e)
            {
                Program.getMainForm().textBoxLog.Text =
                    $@"Cannot connect to Server with connectionstring {connectionString}.\r\n";
            }
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            sqlDataAdapter.Fill(table);
            sqlDataAdapter.Dispose();
            return table;
        }

        public static DataTable QuerySqlServer(String queryString, SqlConnection connection)
        {
            return null;
        }

        public static DataTable QueryMySql(String queryString, MySqlConnection connection)
        {
            return null;
        }
        /// <summary>
        /// Query datatable from MySql with query string and connection string
        /// </summary>
        /// <param name="queryString">query string</param>
        /// <param name="connectionString">connection string</param>
        /// <returns></returns>
        public static DataTable QueryMySql(string queryString, string connectionString)
        {
            mySqlConnection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(queryString, mySqlConnection);
            try
            {

                mySqlConnection.Open();
            }
            catch (Exception e)
            {
                Program.getMainForm().textBoxLog.Text =
                    $@"Cannot connect to Server with connectionstring {connectionString}.\r\n";
            }
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            adapter.Dispose();
            return table;
        }
        public static DataTable QueryMySql(string str)
        {
            mySqlConnection = new MySqlConnection(_connectionString);
            MySqlCommand command = new MySqlCommand(str, mySqlConnection);
            mySqlConnection.Open();
            //SqlDataReader sqlDataReader = command.ExecuteReader();
            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            sqlDataAdapter.Fill(table);
            sqlDataAdapter.Dispose();
            return table;
        }
        public static DataTable QueryMySqlAsync(string str)
        {
            mySqlConnection = new MySqlConnection(_connectionString);
            MySqlCommand command = new MySqlCommand(str, mySqlConnection);
            mySqlConnection.Open();
            //SqlDataReader sqlDataReader = command.ExecuteReader();
            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            sqlDataAdapter.Fill(table);
            sqlDataAdapter.Dispose();
            return table;
        }
        //public static DataTable QueryMySql(string queryStr, string connectionString)
        //{

        //}
        /// <summary>
        /// Dispose resources
        /// </summary>
        public static void  Dispose()
        {
            if (sqlConnection != null)
            {
                sqlConnection.Dispose();
                sqlConnection = null;
            }

            if (mySqlConnection != null)
            {
                mySqlConnection.Dispose();
                mySqlConnection = null;
            }
        }

        public static int GetRecordCount(string dbname,string dtname , string connectionString)
        {
            string queryString = $"use {dbname};select count(*) as 'totalcount' from {dtname};";
            //sqlConnection = new SqlConnection(connectionString);
            //sqlConnection.Open();
            //SqlCommand command = new SqlCommand(queryString, sqlConnection);
            //SqlDataAdapter adapter = new SqlDataAdapter(command);
            mySqlConnection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(queryString, mySqlConnection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            adapter.Dispose();
            command.Dispose();
            return int.Parse(table.Rows[0][0].ToString());
        }
    }
}
