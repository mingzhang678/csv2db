using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CSVtoDatabase
{
    class DataExporter
    {
        private static Form1 mainForm = Program.getMainForm();
        private static SqlConnection sqlConnection { get; set; }

        private static MySqlConnection mySqlConnection { get; set; }
        internal static Thread thread { get; set; }
        private static FileStream fileStream { get; set; }
        private static StreamWriter writer { get; set; }
        public static int ExportToCsv(string dbname, string dtname, string connectionString, DataSource source,
            string filepath)
        {
            mainForm.textBoxLog.AppendText($"Exporting to {filepath}...\r\n");
            // Total count of records
            int recordCount = getRecordCount(dbname, dtname, connectionString, source);
            int exportedCount = 0;
            mainForm.toolStripStatusLabel1.Text = $@"Export progxress: {exportedCount}/{recordCount}";
            // Names of columns
            List<string> columnNames = GetColumnNames(dbname, dtname, connectionString, source);
            // Set progressbar max value to recordCount
            mainForm.toolStripProgressBar1.Maximum = recordCount;
            DataTable table = null;
            string _queryString = "";
            // Datasource
            switch (source)
            {
                // Sql Server
                case DataSource.SqlServer:
                    {
                        _queryString = $"select * from {dbname}.dbo.{dtname}";
                    }
                    break;
                // MySql
                case DataSource.MySql:
                    {
                        _queryString = $"use {dbname};select * from {dtname};";
                    }
                    break;
            }
            int fieldCount = columnNames.Count;
            table = GetDataTable(_queryString, connectionString, source);
            if (File.Exists(filepath))
                File.Delete(filepath);
            fileStream = new FileStream(filepath, FileMode.Create);
            writer = new StreamWriter(fileStream);
            string lineString = "";
            for (int i = 0; i < columnNames.Count; i++)
            {
                if (i == columnNames.Count - 1)
                {
                    lineString += $"\"{columnNames[i]}\"";
                }
                else
                {
                    lineString += $"\"{columnNames[i]}\",";
                }
            }

            mainForm.textBoxLog.AppendText($"Start export.\r\n");
            int success = -1;
            success = WriteToCsv(table, fieldCount, recordCount, writer);
            return exportedCount;
        }
        public static int WriteToCsv(DataTable table, int fieldCount, int recordCount, StreamWriter writer)
        {
            thread = new Thread(() => WriteRecordsThread(table, fieldCount, recordCount, writer))
            {
                IsBackground = false
            };
            thread.Start();
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fieldCount"></param>
        /// <param name="recordCount"></param>
        /// <param name="write"></param>
        public delegate void WriteRecordsDelegate(DataTable table, int fieldCount, int recordCount, StreamWriter write);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        private delegate void setStatusLabelTextDelegate(string text);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        private delegate void setProgressBarValueDelegate(int value);

        internal delegate void appendTextBoxTextDeleget(string text);
        private static int WriteRecordsThread(DataTable table, int fieldCount, int recordCount, StreamWriter writer)
        {
            int exportedCount = 0;
            string lineString = "";
            writer.WriteLine(lineString);
            foreach (DataRow row in table.Rows)
            {
                lineString = "";
                for (int i = 0; i < fieldCount; i++)
                {
                    if (i == fieldCount - 1)
                        lineString += $"\"{row[i]}\"";
                    //writer.WriteLine($"\"{row[i]}\"");
                    else
                        lineString += $"\"{row[i]}\",";
                    //writer.Write($"\"{row[i]}\",");
                }
                writer.WriteLine(lineString);
                exportedCount++;
                mainForm.toolStripStatusLabel1.GetCurrentParent().Invoke(new OperateControls.setStatusLabelTextDelegate(OperateControls.setStatusLabelText), $"{exportedCount} / {recordCount}");
                mainForm.toolStripProgressBar1.GetCurrentParent().Invoke(new OperateControls.setProgressBarValueDelegate(OperateControls.setProgressBar), exportedCount);
                if (recordCount == exportedCount)
                {
                    mainForm.textBoxLog.Invoke(new OperateControls.appendTextBoxTextDelegate(OperateControls.appendTextBoxText), $"Exported {exportedCount} records.\r\n");
                    //fileStream.Dispose();
                    Dispose();
                }
            }
            return 0;
        }

        /// <summary>
        /// Release resources
        /// </summary>
        internal static void Dispose()
        {
            writer?.Dispose();
            fileStream?.Dispose();
            if (thread != null)
            {
                //if (thread.IsAlive)
                thread.Resume();
                    thread.Abort();
            }
        }

        public static DataTable GetDataTable(string queryString, string connectionString, DataSource source)
        {
            DataTable table = null;
            switch (source)
            {
                case DataSource.SqlServer:
                    {
                        table = new Helper().QuerySqlServer(queryString, connectionString);
                    }
                    break;
                case DataSource.MySql:
                    {
                        table = new Helper().QueryMySql(queryString, connectionString);
                    }
                    break;
            }
            return table;
        }

        public static List<string> GetColumnNames(string dbname, string dtname, string connectionString, DataSource source)
        {
            List<string> list = new List<string>();
            DataTable table = new DataTable();
            string _connectionString = connectionString;
            string _queryString = "";
            switch (source)
            {
                case DataSource.SqlServer:
                    {

                        _queryString = $"SELECT COLUMN_NAME FROM {dbname}.INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='{dtname}'";
                        sqlConnection = new SqlConnection(_connectionString);
                        //try
                        //{
                        sqlConnection.Open();
                        //}
                        //catch (Exception e)
                        //{
                        //}
                        SqlCommand command = new SqlCommand(_queryString, sqlConnection);
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(table);
                        command.Dispose();
                        adapter.Dispose();
                    }
                    ; break;
                case DataSource.MySql:
                    {
                        _queryString = $"USE {dbname};SELECT column_name FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{dtname}'";
                        mySqlConnection = new MySqlConnection(_connectionString);
                        mySqlConnection.Open();
                        MySqlCommand command = new MySqlCommand(_queryString, mySqlConnection);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        adapter.Fill(table);
                        command.Dispose();
                        adapter.Dispose();
                    }
                    break;
            }

            foreach (DataRow row in table.Rows)
            {
                list.Add(row["COLUMN_NAME"].ToString());
            }
            return list;
        }
        public static int getRecordCount(string dbname, string dtname, string connectionString, DataSource source)
        {
            string queryString = $"USE {dbname};SELECT COUNT(*) as 'totalcount' FROM {dtname}";
            object count = 0;
            switch (source)
            {
                case DataSource.SqlServer:
                    {
                        sqlConnection = new SqlConnection(connectionString);
                        sqlConnection.Open();
                        SqlCommand command = new SqlCommand(queryString, sqlConnection);
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        count = table.Rows[0][0];
                    }
                    break;
                case DataSource.MySql:
                    {
                        mySqlConnection = new MySqlConnection(connectionString);
                        mySqlConnection.Open();
                        MySqlCommand commmand = new MySqlCommand(queryString, mySqlConnection);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(commmand);
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        count = table.Rows[0][0];
                    }
                    break;
            }
            return Convert.ToInt32(count);
        }

    }
}
