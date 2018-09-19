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
    public partial class MainForm : Form
    {
        //private string connString = "";
        //private SqlConnection sqlConnection = null;
        //private MySqlConnection mySqlConnection = null;
        //private bool loaded = false;
        private bool _isImporting;
        private bool _isExporting;
        public bool _formClosing;
        private bool _connected;
        private int _exportedCount = 0;
        public string UserFolder { get; set; } = Environment.GetEnvironmentVariable("USERPROFILE") ;
        //private OnClosingDialog onClosingDialog;
        private string _safeFileName = string.Empty;
        internal DataExporter _dataExporter { get; set; }
        internal DataImporter _dataImporter;
        private SqlConnection _sqlConnection { get; set; }
        private MySqlConnection _mySqlConnection { get; set; }
        private Thread _thread { get; set; }

        public MainForm()
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
            textBoxFilePath.Text = Environment.GetEnvironmentVariable("USERPROFILE")+ "\\Documents";
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
            openFileDialog.Filter = "CSV Files | *.csv| All Files | *.*";
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
            InitialImporter();
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
            //string connectionString = "";
            switch (listBoxDatabase.SelectedIndex)
            {
                case 0:
                    {
                        //connectionString = checkBoxIntegratedSecurity.Checked
                        //    ? $"Data Source={Server};Initial Catalog=master;Persist Security Info=True;Integrated Security=True"
                        //    : $"Data Source={Server};Initial Catalog=master;Persist Security Info=True;User ID={UserId};Password={textBoxPwd.Text}";
                        _dataImporter.ImportToSqlServer(dbname, dtname, textBoxFilePath.Text, checkBoxIgnoreFirstLine.Checked);
                    }
                    break;
                case 1:
                    {
                        //connectionString = $"server={Server};user id={UserId};Password={textBoxPwd.Text};database=mysql";
                        //DataImporter.ImportToMySqlAsync(dbname, dtname, connectionString, textBoxFilePath.Text, null, true);
                        _dataImporter.ImportToMySql(dbname, dtname, textBoxFilePath.Text,
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
                case 0:
                    EnableIntegratedSecurityChkbox(true);
                    textBoxUId.Text = "sa"; break;
                default: EnableIntegratedSecurityChkbox(false);
                    textBoxUId.Text = "root"; break;
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
                    _sqlConnection = new SqlConnection("Initial Catalog=master;Data Source=.;Integrated Security=True");
                    try
                    {
                        table = new Helper(_sqlConnection).QuerySqlServer(queryString);
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
                    _sqlConnection = new SqlConnection($"Initial Catalog=master;Data Source=.;User ID={UserId};Password={Password}");
                    try
                    {
                        table = new Helper(_sqlConnection).QuerySqlServer(queryString);
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
                    _mySqlConnection = new MySqlConnection($"server=localhost;user id={UserId};persistsecurityinfo=True;database=mysql;password={Password}");
                    //Get list of database
                    table = new Helper(_mySqlConnection).QueryMySql("show databases;");
                    _connected = true;
                }
                catch (Exception e)
                {
                    textBoxLog.AppendText(e.Message + "\r\n");
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

                table = new Helper(_sqlConnection).QuerySqlServer(
                    $"use {comboBoxDbList.Text}; select name from sysobjects where xtype = 'U'");
                //}
                //catch (Exception e)
                //{
                //    textBoxLog.AppendText(e.StackTrace + "\r\n");
                //}
                foreach (DataRow row in table.Rows)
                    list.Add(row[0].ToString());
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
                    list.Add(row[0].ToString());
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
            int count = new Helper(_sqlConnection).GetRecordCount(SelectDatabaseName,SelectDatatableName );
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
            new DataExporter(_sqlConnection).Suspend();
            //MessageBox.Show($@"Max:{toolStripProgressBar1.Maximum}---Value:{toolStripProgressBar1.Value}");
            if (toolStripProgressBar1.Maximum != toolStripProgressBar1.Value)
                if (MessageBox.Show("Are you sure to exit?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    e.Cancel = false;
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
                checkBoxIgnoreFirstLine.Checked = true;
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
                DiableChkBox(true);
            else
                DiableChkBox(false);
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
            if (_sqlConnection == null && _mySqlConnection == null)
            {
                textBoxLog.AppendText("No connection.\r\n");
                return;
            }
            else if (_sqlConnection != null && _sqlConnection.State != ConnectionState.Closed)
            {
                _sqlConnection.Dispose();
                textBoxLog.AppendText("Connection closed.\r\n");
            }

           else  if (_sqlConnection != null && _sqlConnection.State == ConnectionState.Closed)
            {
                textBoxLog.AppendText("Connection has already been closed.\r\n");
            }

           else  if (_mySqlConnection != null && _mySqlConnection.State != ConnectionState.Closed)
            {
                _mySqlConnection.Close();
                textBoxLog.AppendText("Connection closed.\r\n");
            }

           else  if (_mySqlConnection != null && _mySqlConnection.State != ConnectionState.Closed)
            {
                textBoxLog.AppendText("Connection has already been closed.\r\n");
            }

            comboBoxDbList.DataSource = new List<string>();
            comboBoxDbList.Text = string.Empty;
            comboBoxDtList.DataSource = new List<String>();
            comboBoxDtList.Text = string.Empty;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Helper.Dispose();
            _dataExporter?.Dispose();
            _dataImporter?.Dispose();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            string filePath;
            if (_sqlConnection == null && _mySqlConnection==null)
            {
                textBoxLog.AppendText("No connection.\r\n");
            }
            else if(_sqlConnection!=null&&_sqlConnection.State!=ConnectionState.Open)
            {
                textBoxLog.AppendText("Connection not opened.\r\n");
            }
            else if(_mySqlConnection!=null&&_mySqlConnection.State!=ConnectionState.Open)
            {
                textBoxLog.AppendText("Connection not opened.\r\n");
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = @"CSV Files|*.csv|All Files|*.*"
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = saveFileDialog.FileName;
            }
            else
            {
                return;
            }
            InitialExporter();
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
                case DataSource.SqlServer: _dataExporter = new DataExporter(_sqlConnection);break;
                 case DataSource.MySql:_dataExporter = new DataExporter(_mySqlConnection);break;
            }
        }

        private void InitialImporter()
        {
            switch (Source)
            {
                case DataSource.SqlServer: _dataImporter = new DataImporter(_sqlConnection);break;
                case DataSource.MySql: _dataImporter = new DataImporter(_mySqlConnection);break;
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
            if (_dataExporter._thread != null && _dataExporter._thread.ThreadState == ThreadState.Running)
            {
                _dataExporter._thread.Suspend();
                textBoxLog.AppendText("Paused.\r\n");
            }
            if (_dataImporter._thread != null && _dataImporter._thread.ThreadState == ThreadState.Running)
            {
                _dataExporter._thread.Suspend();
                textBoxLog.AppendText("Paused.\r\n");
            }
            //MessageBox.Show(_dataExporter._thread.ThreadState.ToString());
        }

        private void btnStopExport_Click(object sender, EventArgs e)
        {
            if(_dataExporter!=null)
                if(_dataExporter._thread!=null)
                    if(_dataExporter._thread.ThreadState != ThreadState.Stopped)
                        _dataExporter._thread.Abort();
            _dataExporter.Dispose();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        public void Suspend()
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

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            MessageBox.Show("YEP");
        }

        private void textBoxFilePath_DragOver(object sender, DragEventArgs e)
        {
            MessageBox.Show("Dropped.");
        }

        private void textBoxFilePath_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void Form1_DragEnter_1(object sender, DragEventArgs e)
        {

        }

        private void textBoxFilePath_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[]) e.Data.GetData(DataFormats.FileDrop);
            textBoxFilePath.Text = files[0];
        }

        private void btnGenerateScripts_Click(object sender, EventArgs e)
        {
            
        }
    }
}