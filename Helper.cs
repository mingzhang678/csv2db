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
        public static SqlConnection _sqlConnection { get; set; }
        public static MySqlConnection _mySqlConnection { get; set; }

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
            _sqlConnection = connection;
        }

        public Helper(MySqlConnection connection)
        {
            _mySqlConnection = connection;
        }
        public DataTable QuerySqlServer(string str)
        {
            //sqlConnection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(str, _sqlConnection);
            // try
            //{
            if (_sqlConnection != null)
                if (_sqlConnection.State == ConnectionState.Closed)
                    _sqlConnection.Open();
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
            _sqlConnection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(queryString, _sqlConnection);
            if (_sqlConnection != null && _sqlConnection.State == ConnectionState.Closed)
                //try
                //{
                    _sqlConnection.Open();
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
            _mySqlConnection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(queryString, _mySqlConnection);
            //try
           // {

                _mySqlConnection.Open();
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
            if(_mySqlConnection==null)
                _mySqlConnection = new MySqlConnection(_connectionString);
            MySqlCommand command = new MySqlCommand(str, _mySqlConnection);
            if(_mySqlConnection.State==ConnectionState.Closed)
                _mySqlConnection.Open();
            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            sqlDataAdapter.Fill(table);
            sqlDataAdapter.Dispose();
            return table;
        }
        public static DataTable QueryMySqlAsync(string str)
        {
            _mySqlConnection = new MySqlConnection(_connectionString);
            MySqlCommand command = new MySqlCommand(str, _mySqlConnection);
            _mySqlConnection.Open();
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
            if (_sqlConnection != null)
            {
                _sqlConnection.Dispose();
                _sqlConnection = null;
            }

            if (_mySqlConnection != null)
            {
                _mySqlConnection.Dispose();
                _mySqlConnection = null;
            }
        }

        public int GetRecordCount(string dbname, string dtname)
        {
            string queryString = $"use {dbname};select count(*) as 'totalcount' from {dtname};";
            MySqlCommand command = new MySqlCommand(queryString, _mySqlConnection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            adapter.Dispose();
            command.Dispose();
            return int.Parse(table.Rows[0][0].ToString());
        }
    }
}
