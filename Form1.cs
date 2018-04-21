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
using DbHelper;
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
        public bool formClosing = false;
        private bool connected = false;
        private int exportedCount = 0;
        public string UserFolder { get; set; } = Environment.GetEnvironmentVariable("USERPROFILE");
        public static string Str = "hHAHAHHA";
        //private OnClosingDialog onClosingDialog;
        private string safeFileName = string.Empty;

        public Form1()
        {
            InitializeComponent();
            //new Form1().UserFolder = "dd";
        }
        /// <summary>
        ///  "abcde"
        /// </summary>
        /// <returns></returns>
        private List<string> getColumnName(string str)
        {
            List<string> list = new List<string>();
            string firstRow = getStrFileText().Substring(0, getStrFileText().IndexOf('\n'));
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
            setLabelColor();
            listBoxDatabase.SelectedIndex = 0;
            //setDbList();
            //DataBaseWriter.setProgressBar(toolStripProgressBar1);
            //DataBaseWriter.setStatusLabel(toolStripStatusLabel1);
        }
        private void createLogFile()
        {
            var filepath = $"{UserFolder}\\AppData\\Local\\CSVToDb\\main.log";
            if (!File.Exists(filepath))
            {
                File.Create(filepath);
            };
            FileStream fileStream = new FileStream(filepath, FileMode.Append);

            return;
        }
        private void setLabelColor()
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
                safeFileName = openFileDialog.SafeFileName;
                textBoxLog.AppendText($"Selected file {textBoxFilePath.Text} \n");
            }
        }
        private string getStrFileText()
        {
            return "";
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            if (!connected)
            {
                MessageBox.Show("Please connect to database server.");
                return;
            }
            if (!getDbList().Contains(comboBoxDbList.Text) || !getDtList().Contains(comboBoxDtList.Text))
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
                            ? $"Data Source={textBoxServer.Text};Initial Catalog=master;Persist Security Info=True;Integrated Security=True"
                            : $"Data Source={textBoxServer.Text};Initial Catalog=master;Persist Security Info=True;User ID={textBoxUId.Text};Password={textBoxPwd.Text}";
                        DataImporter.DoWriteToSqlServer(dbname, dtname, connectionString, textBoxFilePath.Text, checkBoxIgnoreFirstLine.Checked);
                    }
                    break;
                case 1:
                    {
                        connectionString = $"server={textBoxServer.Text};user id={textBoxUId.Text};Password={textBoxPwd.Text};database=mysql";
                        //DataImporter.ImportToMySqlAsync(dbname, dtname, connectionString, textBoxFilePath.Text, null, true);
                        DataImporter.ImportToMySql(dbname, dtname, connectionString, textBoxFilePath.Text,
                            checkBoxIgnoreFirstLine.Checked);
                    }
                    break;
            }
        }
        private void listBoxDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxLog.AppendText($"{listBoxDatabase.SelectedItem.ToString()} selected.\r\n");
            int selected = listBoxDatabase.SelectedIndex;
            switch (selected)
            {
                case 0: enableIntegratedSecurityChkbox(true); break;
                default: enableIntegratedSecurityChkbox(false); break;
            }
            connected = false;
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
            if (!connected)
                return;
            List<string> list = getDtList();
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
        private void setDbList()
        {
            comboBoxDbList.DataSource = getDbList();
        }

        private List<string> getDbList()
        {
            string queryString;
            List<string> list = new List<string>();
            DataTable table = null;
            #region Sql Server
            if (listBoxDatabase.SelectedIndex == 0)
            {
                queryString = $@"select name from master.dbo.sysdatabases where dbid > 7";
                if (checkBoxIntegratedSecurity.Checked)
                {
                    try
                    {
                        table = DbHelper.Helper.QuerySqlServer(queryString);
                        connected = true;
                    }
                    catch (Exception e)
                    {
                        textBoxLog.AppendText($"{textBoxLog.Text}\r\n");
                    }
                }

                else
                {
                    try
                    {
                        table = DbHelper.Helper.QuerySqlServer(
                            queryString,
                            $"Data Source=.;Initial Catalog=master;User ID={textBoxUId.Text};Password={textBoxPwd.Text}");
                        connected = true;
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
                    table = DbHelper.Helper.QueryMySql("show databases;",
                        $"server=localhost;user id={textBoxUId.Text};persistsecurityinfo=True;database=mysql;password={textBoxPwd.Text}");
                    connected = true;
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

        private List<string> getDtList()
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
                    $"Data Source=.;Initial Catalog=master;User ID={textBoxUId.Text};Password={textBoxPwd.Text}";
                try
                {

                    table = (checkBoxIntegratedSecurity.Checked)
                        ? DbHelper.Helper.QuerySqlServer(
                            $"use {comboBoxDbList.Text}; select name from sysobjects where xtype = 'U'")
                        : DbHelper.Helper.QuerySqlServer(
                            $"use {comboBoxDbList.Text}; select name from sysobjects where xtype = 'U'", connString);
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
            //
            //Mysql
            //
            else if (listBoxDatabase.SelectedIndex == 1)
            {
                connString = $"server={textBoxServer.Text};user id={textBoxUId.Text};persistsecurityinfo=False;database=mysql;password={textBoxPwd.Text}";
                try
                {

                    table = DbHelper.Helper.QueryMySql($@"use {comboBoxDbList.Text};show tables;", connString);
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
            var msgbox_show = MessageBox.Show("", "Warning", MessageBoxButtons.YesNo);
            textBoxLog.Text = msgbox_show.ToString();
            string connectionString = $"server={textBoxServer.Text};user id={textBoxUId.Text};password={textBoxPwd.Text}";
            //$"data source={textBoxServerAddress.Text};user id={textBoxUId.Text};password={textBoxPwd.Text};initial catalog=master";
            int count = Helper.GetRecordCount(comboBoxDbList.Text, comboBoxDtList.Text, connectionString);
            MessageBox.Show(count.ToString());
        }
        /// <summary>
        /// Event occurs when form closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.formClosing = true;
            if (DataExporter.thread != null)
            {
                DataExporter.thread.Suspend();
            }
            
            //MessageBox.Show($@"Max:{toolStripProgressBar1.Maximum}---Value:{toolStripProgressBar1.Value}");
            if (toolStripProgressBar1.Maximum != toolStripProgressBar1.Value)
                if (MessageBox.Show("Are you sure to exit?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    e.Cancel = false;
                }
                else
                {
                    formClosing = false;
                    e.Cancel = true;
                    if (DataExporter.thread.IsAlive)
                    {
                        DataExporter.thread.Resume();
                    }
                }
        }
        /// <summary>
        /// get the  boolean value that indicates whether the form is closing
        /// </summary>
        /// <returns></returns>
        private bool getformClosing()
        {
            return formClosing;
        }
        /// <summary>
        /// Get <value>textBoxLog</value> control
        /// </summary>
        /// <returns></returns>
        public TextBox gettextBoxLog()
        {
            return textBoxLog;
        }
        //
        private void exitXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataExporter.Dispose();
            
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
        private void diableChkBox(bool disable)
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
            setCheckbox();
        }

        private void comboBoxDtList_TextChanged(object sender, EventArgs e)
        {
            setCheckbox();
        }
        /// <summary>
        ///  Whether to disable checkBoxIgnoreFirstLine
        /// </summary>
        void setCheckbox()
        {
            if (!getDbList().Contains(comboBoxDbList.Text) || !getDtList().Contains(comboBoxDtList.Text))
            {
                diableChkBox(true);
            }
            else
            {
                diableChkBox(false);
            }
        }
        /// <summary>
        /// Enable <remarks>ddd</remarks>
        /// </summary>
        /// <param name="enable"></param>
        void enableIntegratedSecurityChkbox(bool enable)
        {
            if (enable)
                ;
            checkBoxIntegratedSecurity.Checked = enable;
            checkBoxIntegratedSecurity.Enabled = enable;
        }
        //
        private void btnCheckConnection_Click(object sender, EventArgs e)
        {
            if (textBoxServer.Text == string.Empty)
            {
                MessageBox.Show("Server not specified...");
                return;
            }
            if (!checkBoxIntegratedSecurity.Checked && (textBoxUId.Text == string.Empty || textBoxPwd.Text == string.Empty))
            {
                MessageBox.Show("Please login with userid and password.");
                return;
            }
            setDbList();
            //connected = true;
            textBoxLog.AppendText($"Connected to {textBoxServer.Text}.\r\n");
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
            Helper.Dispose();
            textBoxLog.AppendText("Disconnected.\r\n");
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Helper.Dispose();
            //DataExporter.thread.Abort();
            DataExporter.Dispose();
            
            //MessageBox.Show(toolStripProgressBar1.Value.ToString());
        }

        private async void btnExport_Click(object sender, EventArgs e)
        {
            if (!connected)
            {
                MessageBox.Show("No server connected.\r\n");
                return;
            }
            string filePath = "";
            DataSource source = DataSource.SqlServer;
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
            String connectionString = "";
            switch (listBoxDatabase.SelectedIndex)
            {
                case 0:
                    {
                        if (checkBoxIntegratedSecurity.Checked)
                        {
                            connectionString = checkBoxIntegratedSecurity.Checked
                                ? $"Data Source={textBoxServer.Text};Initial Catalog=master;Persist Security Info=True;Integrated Security=True"
                                : $"Data Source={textBoxServer.Text};Initial Catalog=master;Persist Security Info=True;User ID={textBoxUId.Text};Password={textBoxPwd.Text}";
                        }
                        else
                            source = DataSource.SqlServer;
                    }
                    break;
                case 1:
                    {
                        source = DataSource.MySql;
                        connectionString = $"server={textBoxServer.Text};user id={textBoxUId.Text};password={textBoxPwd.Text};persistsecurityinfo=False;database=mysql"; break;
                    }
            }

            string dbname = comboBoxDbList.Text;
            string dtname = comboBoxDtList.Text;
            //Task<int> task =Task.Run(()=>//DataExporter.ExportToCsv();
            DataExporter.ExportToCsv(dbname, dtname, connectionString, source, filePath);
            //
            //int result = await task;
            //textBoxLog.AppendText($"Exported {export.Result} records.\r\n");
        }

        private delegate void startExportDelegate();
        private void startExport()
        {

        }

        private void export(string dbname, string dtname, string connectionString, DataSource source, string filepath)
        {

        }
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

        private List<Control> getAllControls()
        {
            List<Control> list = new List<Control>();
            return list;
        }

        private List<Control> getTextBoxes()
        {
            List<Control> list = new List<Control>();
            return list;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (DataExporter.thread.ThreadState != ThreadState.Suspended)
            {
                DataExporter.thread.Suspend();
                btnPauseExport.Text = @"Resume";
            }
            else if (DataExporter.thread.ThreadState != ThreadState.Running)
            {
                DataExporter.thread.Resume();
                btnPauseExport.Text = @"Pause";
            }
            MessageBox.Show(DataExporter.thread.ThreadState.ToString());
        }

        private void btnStopExport_Click(object sender, EventArgs e)
        {
            DataExporter.thread.Abort();
            DataExporter.Dispose();
        }
    }
}