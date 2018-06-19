using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
//using DbHelper;
using MySql.Data.MySqlClient;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace CSVtoDatabase
{
    public partial class Form1 : Form
    {
        //private string connString = "";
        //private SqlConnection sqlConnection = null;
        //private MySqlConnection mySqlConnection = null;
        //private bool loaded = false;
        public bool _formClosing = false;
        private bool _connected = false;
        private int _exportedCount = 0;
        public string UserFolder { get; set; } = Environment.GetEnvironmentVariable("USERPROFILE");
        public static string Str = "hHAHAHHA";
        //private OnClosingDialog onClosingDialog;
        private string _safeFileName = string.Empty;
        private string _userId;

        internal DataExporter _dataExporter;
        internal DataImporter _dataImporter;
        private SqlConnection ThisSqlConnection { get; set; }
        private MySqlConnection ThisMySqlConnection { get; set; }
        private DataExporter ThisExporter { get; set; }

        public Form1()
        {
            InitializeComponent();
            //new Form1().UserFolder = "dd";
        }
        /// <summary>
        ///  "abcde"
        /// </summary>
        /// <returns></returns>
        private List<string> GetColumnName(string str)
        {
            List<string> list = new List<string>();
            string firstRow = GetStrFileText().Substring(0, GetStrFileText().IndexOf('\n'));
            Regex regex = new Regex("\"[^\"\"]{0,}\"");
            MatchCollection matchCollection = regex.Matches(firstRow);
            foreach (Match match in matchCollection)
            {
                string name = match.Value.Substring(1, match.Value.Length - 2);
                list.Add(name);
            }
            return list;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            textBoxFilePath.Text = Environment.GetEnvironmentVariable("USERPROFILE");
            SetLabelColor();
            listBoxDatabase.SelectedIndex = 0;
            //setDbList();
            //DataBaseWriter.setProgressBar(toolStripProgressBar1);
            //DataBaseWriter.setStatusLabel(toolStripStatusLabel1);
        }
        private void CreateLogFile()
        {
            var filepath = $"{UserFolder}\\AppData\\Local\\CSVToDb\\main.log";
            if (!File.Exists(filepath))
            {
                File.Create(filepath);
            };
            FileStream fileStream = new FileStream(filepath, FileMode.Append);

            return;
        }
        private void SetLabelColor()
        {
            List<Label> list = new List<Label>();
            foreach (object obj in Controls)
            {
                if (obj is Label)
                {
                    var label = obj as Label;
                    label.ForeColor = Color.Blue;
                }
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            #region
            //String connectionString = ConfigurationManager.ConnectionStrings["DBmaster"].ConnectionString;
            //textBoxFilePath.Text = connectionString;
            //SqlConnection connection = new SqlConnection(connectionString);
            //string strCommand = "use new_db;" +
            //    "create table new_dt(" +
            //    "myfield int null" +
            //    ");";
            //SqlCommand command = new SqlCommand(strCommand,connection);
            //connection.Open();
            //command.ExecuteNonQuery();
            //command.Dispose();
            //connection.Dispose();
            #endregion
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxFilePath.Text = openFileDialog.FileName;
                _safeFileName = openFileDialog.SafeFileName;
                textBoxLog.AppendText($"Selected file {textBoxFilePath.Text} \n");
            }
        }
        private string GetStrFileText()
        {
            return "";
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            if (!_connected)
            {
                MessageBox.Show(@"Please connect to database server.");
                return;
            }
            if (!GetDbList().Contains(comboBoxDbList.Text) || !GetDtList().Contains(comboBoxDtList.Text))
            {
                DialogResult dialogResult = MessageBox.Show(@"Do you want create with the specified name?",
                    @"Database or table not exists", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                    return;
            }
            string dbname = comboBoxDbList.Text == string.Empty ? "csv2db" : comboBoxDbList.Text;
            string dtname = comboBoxDtList.Text == string.Empty ? "tb_from_csv" : comboBoxDtList.Text;
            string connectionString = "";
            switch (listBoxDatabase.SelectedIndex)
            {
                case 0:
                    {
                        connectionString = checkBoxIntegratedSecurity.Checked
                            ? $"Data Source={Server};Initial Catalog=master;Persist Security Info=True;Integrated Security=True"
                            : $"Data Source={Server};Initial Catalog=master;Persist Security Info=True;User ID={UserId};Password={textBoxPwd.Text}";
                        _dataImporter.ImportToSqlServer(dbname, dtname, connectionString, textBoxFilePath.Text, checkBoxIgnoreFirstLine.Checked);
                    }
                    break;
                case 1:
                    {
                        connectionString = $"server={Server};user id={UserId};Password={textBoxPwd.Text};database=mysql";
                        //DataImporter.ImportToMySqlAsync(dbname, dtname, connectionString, textBoxFilePath.Text, null, true);
                        _dataImporter.ImportToMySql(dbname, dtname, connectionString, textBoxFilePath.Text,
                            checkBoxIgnoreFirstLine.Checked);
                    }
                    break;
            }
        }
        private void listBoxDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxLog.AppendText($"{listBoxDatabase.SelectedItem} selected.\r\n");
            int selected = listBoxDatabase.SelectedIndex;
            switch (selected)
            {
                case 0: EnableIntegratedSecurityChkbox(true); break;
                default: EnableIntegratedSecurityChkbox(false); break;
            }
            _connected = false;
            //setDbList();
        }
        private void preferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxLog.Copy();
        }
        private void comboBoxDbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_connected)
                return;
            List<string> list = GetDtList();
            if (list.Count == 0)
            {
                textBoxLog.AppendText("No table found...\r\n");
                comboBoxDtList.Text = "";
                return;
            }
            comboBoxDtList.DataSource = list;
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = $"{UserFolder}\\Documents";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxFilePath.Text = openFileDialog.FileName;
                textBoxLog.AppendText($"Selected file {textBoxFilePath.Text}");
            }
        }
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        private void SetDbList()
        {
            comboBoxDbList.DataSource = GetDbList();
        }
        /// <summary>
        /// Get list of database name
        /// </summary>
        /// <returns></returns>
        private List<string> GetDbList()
        {
            string queryString;
            List<string> list = new List<string>();
            DataTable table = null;
            #region Sql Server
            if (listBoxDatabase.SelectedIndex == 0)
            {
                queryString = $@"select name from master.dbo.sysdatabases ;";   //-- where dbid > 7
                // If use integrated security
                if (checkBoxIntegratedSecurity.Checked)
                {
                    ThisSqlConnection = new SqlConnection("Initial Catalog=master;Data Source=.;Integrated Security=True");
                    try
                    {
                        table = new Helper(ThisSqlConnection).QuerySqlServer(queryString);
                        _connected = true;
                    }
                    catch (Exception e)
                    {
                        textBoxLog.AppendText($"{e.Message}\r\n");
                        return new List<string>();
                    }
                }

                else
                {
                    ThisSqlConnection = new SqlConnection($"Initial Catalog=master;Data Source=.;User ID={UserId};Password={Password}");
                    try
                    {
                        table = new Helper(ThisSqlConnection).QuerySqlServer(queryString);
                        _connected = true;
                    }
                    catch (Exception e)
                    {
                        textBoxLog.AppendText(e.Message + "\r\n" + e.StackTrace + "\r\n");
                        return list;
                    }
                }
                foreach (DataRow row in table.Rows)
                {
                    list.Add(row["name"].ToString());
                }
            }
            #endregion

            #region MySql

            else
            {
                try
                {
                    //Get list of database
                    table = new Helper().QueryMySql("show databases;",
                        $"server=localhost;user id={UserId};persistsecurityinfo=True;database=mysql;password={textBoxPwd.Text}");
                    _connected = true;
                }
                catch (Exception e)
                {
                    textBoxLog.AppendText(e.StackTrace + "\r\n");
                    return list;
                }
                foreach (DataRow row in table.Rows)
                {
                    if (row[0].ToString() == "performance_schema" || row[0].ToString() == "information_schema" || row[0].ToString() == "sys")
                        continue;
                    list.Add(row[0].ToString());
                }
            }
            #endregion
            return list;
        }
        /// <summary>
        /// Get list of database name in specified database
        /// </summary>
        /// <returns></returns>
        private List<string> GetDtList()
        {
            List<string> list = new List<string>();
            DataTable table = null;
            //
            //Sql Server
            //
            String connString = "";
            if (listBoxDatabase.SelectedIndex == 0)
            {
                connString =
                    $"Data Source=.;Initial Catalog=master;User ID={UserId};Password={textBoxPwd.Text}";
                //try
                //{

                table = new Helper(ThisSqlConnection).QuerySqlServer(
                    $"use {comboBoxDbList.Text}; select name from sysobjects where xtype = 'U'");
                //}
                //catch (Exception e)
                //{
                //    textBoxLog.AppendText(e.StackTrace + "\r\n");
                //}
                foreach (DataRow row in table.Rows)
                {
                    list.Add(row[0].ToString());
                }
            }
            //
            //Mysql
            //
            else if (listBoxDatabase.SelectedIndex == 1)
            {
                connString = $"server={Server};user id={UserId};persistsecurityinfo=False;database=mysql;password={textBoxPwd.Text}";
                try
                {

                    table = new Helper().QueryMySql($@"use {comboBoxDbList.Text};show tables;", connString);
                }
                catch (Exception e)
                {
                    textBoxLog.AppendText(e.StackTrace + "\r\n");
                    return list;
                }
                foreach (DataRow row in table.Rows)
                {
                    list.Add(row[0].ToString());
                }
            }
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        private List<string> GetDbList(string connString)
        {
            switch (listBoxDatabase.SelectedIndex)
            {
                case 0:
                    {

                    }
                    break;
                case 1:
                    {

                    }
                    break;
            }
            return null;
        }
        //
        private void button1_Click_1(object sender, EventArgs e)
        {
            var msgboxShow = MessageBox.Show("", "Warning", MessageBoxButtons.YesNo);
            textBoxLog.Text = msgboxShow.ToString();
            string connectionString = $"server={Server};user id={UserId};password={Password}";
            //$"data source={textBoxServerAddress.Text};user id={UserId};password={textBoxPwd.Text};initial catalog=master";
            int count = new Helper(ThisSqlConnection).GetRecordCount(SelectDatabaseName,SelectDatatableName );
            MessageBox.Show(count.ToString());
        }
        /// <summary>
        /// Event occurs when form closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._formClosing = true;
            new DataExporter(ThisSqlConnection).Suspend();
            //MessageBox.Show($@"Max:{toolStripProgressBar1.Maximum}---Value:{toolStripProgressBar1.Value}");
            if (toolStripProgressBar1.Maximum != toolStripProgressBar1.Value)
                if (MessageBox.Show("Are you sure to exit?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    e.Cancel = false;
                }
                else
                {
                    _formClosing = false;
                    e.Cancel = true;
                }
        }
        /// <summary>
        /// get the  boolean value that indicates whether the form is closing
        /// </summary>
        /// <returns></returns>
        private bool GetformClosing()
        {
            return _formClosing;
        }
        /// <summary>
        /// Get <value>textBoxLog</value> control
        /// </summary>
        /// <returns></returns>
        public TextBox GettextBoxLog()
        {
            return textBoxLog;
        }
        //
        private void exitXToolStripMenuItem_Click(object sender, EventArgs e)
        {
           new  DataExporter().Dispose();

            Application.Exit();
        }
        //
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox.Show();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxDtList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Disable the checkbox indicates whether the first line contains column names.
        /// </summary>
        /// <param name="disable"></param>
        private void DiableChkBox(bool disable)
        {
            if (disable)
            {
                checkBoxIgnoreFirstLine.Checked = true;
            }
            checkBoxIgnoreFirstLine.Enabled = !disable;
        }

        private void comboBoxDbList_TextChanged(object sender, EventArgs e)
        {
            //if (!loaded)
            //{
            //    return;
            //}
            //MessageBox.Show(getDbList().Contains(comboBoxDbList.Text).ToString());
            SetCheckbox();
        }

        private void comboBoxDtList_TextChanged(object sender, EventArgs e)
        {
            SetCheckbox();
        }
        /// <summary>
        ///  Whether to disable checkBoxIgnoreFirstLine
        /// </summary>
        void SetCheckbox()
        {
            if (!GetDbList().Contains(comboBoxDbList.Text) || !GetDtList().Contains(comboBoxDtList.Text))
            {
                DiableChkBox(true);
            }
            else
            {
                DiableChkBox(false);
            }
        }
        /// <summary>
        /// Enable <remarks>ddd</remarks>
        /// </summary>
        /// <param name="enable"></param>
        private void EnableIntegratedSecurityChkbox(bool enable)
        {
            if (enable)
            {
            }

            checkBoxIntegratedSecurity.Checked = enable;
            checkBoxIntegratedSecurity.Enabled = enable;
        }
        //
        private void btnCheckConnection_Click(object sender, EventArgs e)
        {
            if (Server == string.Empty)
            {
                MessageBox.Show(@"Server not specified...");
                return;
            }
            if (!checkBoxIntegratedSecurity.Checked && (UserId == string.Empty || Password == string.Empty))
            {
                MessageBox.Show(@"Please login with userid and password.");
                return;
            }
            SetDbList();
            //connected = true;
            textBoxLog.AppendText($"Connected to {Server}.\r\n");
        }

        private void InitialDbConnection()
        {
            
        }

        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxLog.Clear();
        }

        private void textBoxFilePath_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            //Helper.Dispose();
            if (ThisSqlConnection == null && ThisMySqlConnection == null)
            {
                textBoxLog.AppendText("No connection.\r\n");
                return;
            }
            if (ThisSqlConnection != null && ThisSqlConnection.State != ConnectionState.Closed)
            {
                ThisSqlConnection.Dispose();
                textBoxLog.AppendText("Connection closed.\r\n");
            }

            if (ThisSqlConnection != null && ThisSqlConnection.State == ConnectionState.Closed)
            {
                textBoxLog.AppendText("Connection has already been closed.\r\n");
            }

            if (ThisMySqlConnection != null && ThisMySqlConnection.State != ConnectionState.Closed)
            {
                ThisMySqlConnection.Dispose();
                textBoxLog.AppendText("Connection closed.\r\n");
            }

            if (ThisMySqlConnection != null && ThisMySqlConnection.State == ConnectionState.Closed)
            {
                textBoxLog.AppendText("Connection has already been closed.\r\n");
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Helper.Dispose();
            _dataExporter.Dispose();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (ThisSqlConnection == null || ThisSqlConnection.State != ConnectionState.Connecting)
            {
                if (ThisMySqlConnection == null || ThisMySqlConnection.State != ConnectionState.Connecting)
                {
                    MessageBox.Show("No server connected.\r\n");
                    return;
                }
            }
            string filePath = "";
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = @"CSV Files|*.csv|All Files|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = saveFileDialog.FileName;
            }
            else
            {
                return;
            }

            string dbname = comboBoxDbList.Text;
            string dtname = comboBoxDtList.Text;
            //Task<int> task =Task.Run(()=>//DataExporter.ExportToCsv();
            _dataExporter.ExportToCsv(dbname, dtname, Source, filePath);
            //
            //int result = await task;
            //textBoxLog.AppendText($"Exported {export.Result} records.\r\n");
        }

        private void ExportFromSqlServer()
        {

        }
        private void InitialExporter()
        {
            switch (Source)
            {
                case DataSource.SqlServer: _dataExporter = new DataExporter(ThisSqlConnection);break;
                 case DataSource.MySql:_dataExporter = new DataExporter(ThisMySqlConnection);break;
            }
        }

        private void InitialImporter()
        {
            switch (Source)
            {
                case DataSource.SqlServer: _dataImporter = new DataImporter(ThisSqlConnection);break;
                case DataSource.MySql: _dataImporter = new DataImporter(ThisMySqlConnection);break;
            }
        }
        private delegate void StartExportDelegate();

        private void checkBoxIgnoreFirstLine_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void fileFToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string saveFilePath = "";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                saveFilePath = saveFileDialog.FileName;
            }
            FileStream fileStream = new FileStream(saveFilePath, FileMode.Create);
            StreamWriter writer = new StreamWriter(fileStream);
            writer.Write(textBoxLog.Text);
            writer.Dispose();
            fileStream.Dispose();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_dataExporter.ThisThread.ThreadState != ThreadState.Suspended)
            {
                _dataExporter.ThisThread.Suspend();
                btnPauseExport.Text = @"Resume";
            }
            else if (_dataExporter.ThisThread.ThreadState != ThreadState.Running)
            {
                _dataExporter.ThisThread.Resume();
                btnPauseExport.Text = @"Pause";
            }
            MessageBox.Show(_dataExporter.ThisThread.ThreadState.ToString());
        }

        private void btnStopExport_Click(object sender, EventArgs e)
        {
            _dataExporter.ThisThread.Abort();
            _dataExporter.Dispose();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        internal string UserId => textBoxUId.Text;
        internal string Password => textBoxPwd.Text;
        internal string Server => textBoxServer.Text;
        internal bool UseIntegratedSecurity => checkBoxIntegratedSecurity.Checked;
        internal string SourceFilePath => textBoxFilePath.Text;
        internal bool FirstLineContainsColumnNames => checkBoxIgnoreFirstLine.Checked;
        internal String SelectDatabaseName => comboBoxDbList.Text;
        internal String SelectDatatableName => comboBoxDbList.Text;
        internal DataSource Source
        {
            get
            {
                switch (listBoxDatabase.SelectedIndex)
                {
                    case 0: return DataSource.SqlServer;
                    case 1: return DataSource.MySql;
                    default: return DataSource.SqlServer;
                }
            }
        }
    }
}