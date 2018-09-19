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
    public class DataExporter
    {
        private readonly Form1 MainForm = Program.GetMainForm();
        private SqlConnection _sqlConnection;
        private MySqlConnection _mySqlConnection;
        public Thread _thread;
        private FileStream _fileStream;
        private StreamWriter _streamWriter;

        public DataExporter()
        {

        }
        /// <summary>
        /// dd
        /// </summary>
        /// <param name="connection"></param>
        public DataExporter(SqlConnection connection)
        {
            _sqlConnection = connection;
        }
        public DataExporter(MySqlConnection connection)
        {
            _mySqlConnection = connection;
        }
        public int GenerateScripts(string dbname, string dtname, DataSource source, string filepath)
        {
            MainForm.textBoxLog.AppendText($"Exporting to {filepath}...\r\n");
            // Total count of records
            int recordCount = GetRecordCount(dbname, dtname, source);
            // Count of records exported
            int exportedCount = 0;
            MainForm.toolStripStatusLabel1.Text = $@"Export progxress: {exportedCount}/{recordCount}";
            // Names of columns
            List<string> columnNames = GetColumnNames(dbname, dtname, source);
            // Set progressbar max value to recordCount
            MainForm.toolStripProgressBar1.Maximum = recordCount;
            DataTable table;
            string queryString = "";
            // Datasource
            switch (source)
            {
                // Sql Server
                case DataSource.SqlServer:
                    {
                        queryString = $"select * from {dbname}.dbo.{dtname}";
                    }
                    break;
                // MySql
                case DataSource.MySql:
                    {
                        queryString = $"use {dbname};select * from {dtname};";
                    }
                    break;
            }
            // the quantity of field
            int fieldCount = columnNames.Count;
            table = GetDataTable(queryString, source);
            if (File.Exists(filepath))
                File.Delete(filepath);
            _fileStream = new FileStream(filepath, FileMode.Create);
            _streamWriter = new StreamWriter(_fileStream);
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

            MainForm.textBoxLog.AppendText($"Start export.\r\n");
            int success = -1;
            success = WriteToCsv(table, fieldCount, recordCount, _streamWriter);
            return exportedCount;
        }

        public int ExportToCsv(string dbname, string dtname, DataSource source, string filepath)
        {
            MainForm.textBoxLog.AppendText($"Exporting to {filepath}...\r\n");
            // Total count of records
            int recordCount = GetRecordCount(dbname, dtname, source);
            // Count of records exported
            int exportedCount = 0;
            MainForm.toolStripStatusLabel1.Text = $@"Export progxress: {exportedCount}/{recordCount}";
            // Names of columns
            List<string> columnNames = GetColumnNames(dbname, dtname, source);
            // Set progressbar max value to recordCount
            MainForm.toolStripProgressBar1.Maximum = recordCount;
            DataTable table;
            string queryString = "";
            // Datasource
            switch (source)
            {
                // Sql Server
                case DataSource.SqlServer:
                    {
                        queryString = $"select * from {dbname}.dbo.{dtname}";
                    }
                    break;
                // MySql
                case DataSource.MySql:
                    {
                        queryString = $"use {dbname};select * from {dtname};";
                    }
                    break;
            }
            // the quantity of field
            int fieldCount = columnNames.Count;
            table = GetDataTable(queryString, source);
            if (File.Exists(filepath))
                File.Delete(filepath);
            _fileStream = new FileStream(filepath, FileMode.Create);
            _streamWriter = new StreamWriter(_fileStream);
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

            MainForm.textBoxLog.AppendText($"Start export.\r\n");
            int success = -1;
            success = WriteToCsv(table, fieldCount, recordCount, _streamWriter);
            return exportedCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbname"></param>
        /// <param name="dtname"></param>
        /// <param name="connectionString"></param>
        /// <param name="source"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public int ExportToCsv(string dbname, string dtname, string connectionString, DataSource source, string filepath)
        {
            MainForm.textBoxLog.AppendText($"Exporting to {filepath}...\r\n");
            // Total count of records
            int recordCount = GetRecordCount(dbname, dtname, connectionString, source);
            // Count of records exported
            int exportedCount = 0;
            MainForm.toolStripStatusLabel1.Text = $@"Export progxress: {exportedCount}/{recordCount}";
            // Names of columns
            List<string> columnNames = GetColumnNames(dbname, dtname, connectionString, source);
            // Set progressbar max value to recordCount
            MainForm.toolStripProgressBar1.Maximum = recordCount;
            DataTable table;
            string queryString = "";
            // Datasource
            switch (source)
            {
                // Sql Server
                case DataSource.SqlServer:
                    {
                        queryString = $"select * from {dbname}.dbo.{dtname}";
                    }
                    break;
                // MySql
                case DataSource.MySql:
                    {
                        queryString = $"use {dbname};select * from {dtname};";
                    }
                    break;
            }
            // the quantity of field
            int fieldCount = columnNames.Count;
            table = GetDataTable(queryString, connectionString, source);
            if (File.Exists(filepath))
                File.Delete(filepath);
            _fileStream = new FileStream(filepath, FileMode.Create);
            _streamWriter = new StreamWriter(_fileStream);
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

            MainForm.textBoxLog.AppendText($"Start export.\r\n");
            int success = -1;
            success = WriteToCsv(table, fieldCount, recordCount, _streamWriter);
            return exportedCount;
        }
        public int WriteToCsv(DataTable table, int fieldCount, int recordCount, StreamWriter writer)
        {
            _thread = new Thread(() => WriteRecordsThread(table, fieldCount, recordCount, writer))
            {
                IsBackground = false
            };
            _thread.Start();
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
        private delegate void SetStatusLabelTextDelegate(string text);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        private delegate void SetProgressBarValueDelegate(int value);

        internal delegate void AppendTextBoxTextDeleget(string text);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fieldCount"></param>
        /// <param name="recordCount"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        private int WriteRecordsThread(DataTable table, int fieldCount, int recordCount, StreamWriter writer)
        {
            int exportedCount = 0;
            string lineString = "";
            //writer.WriteLine(lineString);
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
                MainForm.toolStripStatusLabel1.GetCurrentParent().Invoke(new OperateControls.setStatusLabelTextDelegate(OperateControls.SetStatusLabelText), $"{exportedCount} / {recordCount}");
                MainForm.toolStripProgressBar1.GetCurrentParent().Invoke(new OperateControls.setProgressBarValueDelegate(OperateControls.SetProgressBar), exportedCount);
                if (recordCount == exportedCount)
                {
                    MainForm.textBoxLog.Invoke(new OperateControls.appendTextBoxTextDelegate(OperateControls.AppendTextBoxText), $"Exported {exportedCount} records.\r\n");
                    Dispose();
                }
            }
            return 0;
        }

        /// <summary>
        /// Release resources
        /// </summary>
        internal void Dispose()
        {
            _streamWriter?.Dispose();
            _fileStream?.Dispose();
            //if (thread.IsAlive)
            _thread?.Abort();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string queryString, DataSource source)
        {
            DataTable table = null;
            switch (source)
            {
                case DataSource.SqlServer:
                    {
                        table = new Helper(_sqlConnection).QuerySqlServer(queryString);
                    }
                    break;
                case DataSource.MySql:
                    {
                        table = new Helper(_mySqlConnection).QueryMySql(queryString);
                    }
                    break;
            }
            return table;
        }

        public DataTable GetDataTable(string queryString, string connectionString, DataSource source)
        {
            DataTable table = null;
            switch (source)
            {
                case DataSource.SqlServer:
                    {
                        table = new Helper(_sqlConnection).QuerySqlServer(queryString);
                    }
                    break;
                case DataSource.MySql:
                    {
                        table = new Helper(_mySqlConnection).QueryMySql(queryString);
                    }
                    break;
            }
            return table;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbname"></param>
        /// <param name="dtname"></param>
        /// <param name="connectionString"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public List<string> GetColumnNames(string dbname, string dtname, DataSource source)
        {
            List<string> list = new List<string>();
            DataTable table = new DataTable();
            string queryString = "";
            switch (source)
            {
                case DataSource.SqlServer:
                    {

                        //queryString = $"SELECT COLUMN_NAME FROM {dbname}.INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='{dtname}'";
                        queryString =
                            $"SELECT `COLUMN_NAME` FROM `INFORMATION_SCHEMA`.`COLUMNS` WHERE `TABLE_SCHEMA`=\'{dbname}\' AND `TABLE_NAME`=\'{dtname}\';";
                        //try
                        //{
                        if (_sqlConnection.State == ConnectionState.Closed)
                            _sqlConnection.Open();
                        //}
                        //catch (Exception e)
                        //{
                        //}
                        SqlCommand command = new SqlCommand(queryString, _sqlConnection);
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(table);
                        command.Dispose();
                        adapter.Dispose();
                    }
                    ; break;
                case DataSource.MySql:
                    {
                        queryString = $"USE {dbname};SELECT column_name FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{dtname}'";
                        if (_mySqlConnection.State == ConnectionState.Closed)
                            _mySqlConnection.Open();
                        MySqlCommand command = new MySqlCommand(queryString, _mySqlConnection);
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

        public List<string> GetColumnNames(string dbname, string dtname, string connectionString, DataSource source)
        {
            List<string> list = new List<string>();
            DataTable table = new DataTable();
            string queryString = "";
            switch (source)
            {
                case DataSource.SqlServer:
                    {

                        queryString =
                            $"SELECT COLUMN_NAME FROM {dbname}.INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='{dtname}'";
                        _sqlConnection = new SqlConnection(connectionString);
                        //try
                        //{
                        _sqlConnection.Open();
                        //}
                        //catch (Exception e)
                        //{
                        //}
                        SqlCommand command = new SqlCommand(queryString, _sqlConnection);
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(table);
                        command.Dispose();
                        adapter.Dispose();
                    }
                    ;
                    break;
                case DataSource.MySql:
                    {
                        queryString =
                            $"USE {dbname};SELECT column_name FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{dtname}'";
                        _mySqlConnection = new MySqlConnection(connectionString);
                        _mySqlConnection.Open();
                        MySqlCommand command = new MySqlCommand(queryString, _mySqlConnection);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        adapter.Fill(table);
                        command.Dispose();
                        adapter.Dispose();
                    }
                    break;
            }

            foreach (DataRow row in table.Rows)
                list.Add(row["COLUMN_NAME"].ToString());
            return list;
        }

        public int GetRecordCount(string dbname, string dtname, DataSource source)
        {
            string queryString = $"USE {dbname};SELECT COUNT(*) as 'totalcount' FROM {dtname}";
            object count = 0;
            switch (source)
            {
                case DataSource.SqlServer:
                    {
                        if (_sqlConnection.State == ConnectionState.Closed)
                            _sqlConnection.Open();
                        SqlCommand command = new SqlCommand(queryString, _sqlConnection);
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        count = table.Rows[0][0];
                    }
                    break;
                case DataSource.MySql:
                    {
                        if (_mySqlConnection.State == ConnectionState.Closed)
                            _mySqlConnection.Open();
                        MySqlCommand commmand = new MySqlCommand(queryString, _mySqlConnection);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(commmand);
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        count = table.Rows[0][0];
                    }
                    break;
            }
            return Convert.ToInt32(count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbname"></param>
        /// <param name="dtname"></param>
        /// <param name="connectionString"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public int GetRecordCount(string dbname, string dtname, string connectionString, DataSource source)
        {
            string queryString = $"USE {dbname};SELECT COUNT(*) as 'totalcount' FROM {dtname}";
            object count = 0;
            switch (source)
            {
                case DataSource.SqlServer:
                    {
                        _sqlConnection = new SqlConnection(connectionString);
                        _sqlConnection.Open();
                        SqlCommand command = new SqlCommand(queryString, _sqlConnection);
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        count = table.Rows[0][0];
                    }
                    break;
                case DataSource.MySql:
                    {
                        _mySqlConnection = new MySqlConnection(connectionString);
                        _mySqlConnection.Open();
                        MySqlCommand commmand = new MySqlCommand(queryString, _mySqlConnection);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(commmand);
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        count = table.Rows[0][0];
                    }
                    break;
            }
            return Convert.ToInt32(count);
        }

        public void Suspend()
        {
            if (_thread != null)
            {
                if(_thread.ThreadState==ThreadState.Running)
                    _thread.Suspend();
            }
        }

        public void Resume()
        {
            if (_thread != null)
            {
                if(_thread.ThreadState==ThreadState.Suspended)
                _thread.Resume();
            }
        }
        ~DataExporter()
        {
            _streamWriter?.Dispose();
            _mySqlConnection?.Dispose();
            _fileStream?.Dispose();
            _thread?.Abort();
        }
    }
}
