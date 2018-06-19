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
        public static SqlConnection SqlConnection { get; set; }
        public static MySqlConnection MySqlConnection { get; set; }

        public Helper()
        {
            _connectionString = "Data Source=.;Initial Catalog=master;Integrated Security=True";
        }
        public Helper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Helper(SqlConnection connection)
        {
            SqlConnection = connection;
        }

        public Helper(MySqlConnection connection)
        {
            MySqlConnection = connection;
        }
        public DataTable QuerySqlServer(string str)
        {
            //sqlConnection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(str, SqlConnection);
            // try
            //{
            if (SqlConnection != null)
                if (SqlConnection.State == ConnectionState.Closed)
                    SqlConnection.Open();
            //}
            // catch (Exception e)
            //{
            //Program.getMainForm().textBoxLog.Text = $@"Cannot connect to Server with connectionstring {_connectionString}";
            //}
            //SqlDataReader sqlDataReader = command.ExecuteReader();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            sqlDataAdapter.Fill(table);
            sqlDataAdapter.Dispose();
            return table;
        }

        public DataTable QuerySqlServer(string queryString, string connectionString)
        {
            SqlConnection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(queryString, SqlConnection);
            if (SqlConnection != null && SqlConnection.State == ConnectionState.Closed)
                //try
                //{
                    SqlConnection.Open();
                //}
                //catch (Exception e)
                //{
                 //   Program.getMainForm().textBoxLog.Text = $@"Cannot connect to Server with connectionstring {connectionString}.\r\n";
                //}
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            sqlDataAdapter.Fill(table);
            sqlDataAdapter.Dispose();
            return table;
        }

        public DataTable QuerySqlServer(String queryString, SqlConnection connection)
        {
            return null;
        }

        public DataTable QueryMySql(String queryString, MySqlConnection connection)
        {
            return null;
        }
        /// <summary>
        /// Query datatable from MySql with query string and connection string
        /// </summary>
        /// <param name="queryString">query string</param>
        /// <param name="connectionString">connection string</param>
        /// <returns></returns>
        public DataTable QueryMySql(string queryString, string connectionString)
        {
            MySqlConnection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(queryString, MySqlConnection);
            //try
           // {

                MySqlConnection.Open();
            //}
            //catch (Exception e)
            //{
             //   Program.getMainForm().textBoxLog.Text =  $@"Cannot connect to Server with connectionstring {connectionString}.\r\n";
            //}
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            adapter.Dispose();
            return table;
        }
        public DataTable QueryMySql(string str)
        {
            MySqlConnection = new MySqlConnection(_connectionString);
            MySqlCommand command = new MySqlCommand(str, MySqlConnection);
            MySqlConnection.Open();
            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            sqlDataAdapter.Fill(table);
            sqlDataAdapter.Dispose();
            return table;
        }
        public static DataTable QueryMySqlAsync(string str)
        {
            MySqlConnection = new MySqlConnection(_connectionString);
            MySqlCommand command = new MySqlCommand(str, MySqlConnection);
            MySqlConnection.Open();
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
        public static void Dispose()
        {
            if (SqlConnection != null)
            {
                SqlConnection.Dispose();
                SqlConnection = null;
            }

            if (MySqlConnection != null)
            {
                MySqlConnection.Dispose();
                MySqlConnection = null;
            }
        }

        public int GetRecordCount(string dbname, string dtname)
        {
            string queryString = $"use {dbname};select count(*) as 'totalcount' from {dtname};";
            MySqlCommand command = new MySqlCommand(queryString, MySqlConnection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            adapter.Dispose();
            command.Dispose();
            return int.Parse(table.Rows[0][0].ToString());
        }
    }
}
