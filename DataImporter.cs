using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace CSVtoDatabase
{
    public class DataImporter
    {
        private static Form1 mainForm = Program.getMainForm();
        private static Thread thread = null;
        static MySqlConnection mySqlConnection = null;
        static SqlConnection sqlConnection = null;
        static FileStream fileStream = null;
        static StreamReader streamReader = null;
        /// <summary>
        /// Import data from text file to Sql server
        /// </summary>
        /// <param name="dbname">Database name</param>
        /// <param name="filepath">Filepath</param>
        /// <param name="dtname">Datatable name</param>
        /// <param name="controlSet"></param>
        /// <param name="firstLineColNames">Whether ignore first line, default value is <value>false</value></param>
        /// <returns></returns>
        public static async Task<int> WriteToSqlServer(string dbname, string dtname, string filepath, ControlSet controlSet, bool firstLineColNames)
        {
            if (string.IsNullOrWhiteSpace(filepath))
                return -1;
            string connectionString = @"Data Source=.;Initial Catalog=master;Integrated Security=True";//ConfigurationManager.ConnectionStrings["DBmaster"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
            streamReader = new StreamReader(fileStream);
            int importedRecordCount = 0;
            if (!File.Exists(filepath))
            {
                MessageBox.Show($@"File {filepath} no found.");
                return -1;
            }
            try
            {
                sqlConnection.Open();
            }
            catch (Exception e)
            {
                controlSet.textBoxLog.AppendText($"{e.Message}\r\n");
            }
            int currentLineNumber = 0;
            long lineCount = getLineCount(streamReader, false);
            mainForm.toolStripProgressBar1.GetCurrentParent().Invoke(
                new OperateControls.setProgressBarValueDelegate(OperateControls.setProgressBar), (int)lineCount);
            streamReader.BaseStream.Position = 0;
            while (!streamReader.EndOfStream)
            {
                currentLineNumber++;
                string str = streamReader.ReadLine();
                List<string> splited = GetSplitedStrings(str);
                if (splited == null)
                {
                    continue;
                }

                if (currentLineNumber == 1)
                {
                    if (firstLineColNames)
                    {
                        string fields = "";
                        for (int i = 0; i < splited.Count; i++)
                        {
                            fields += $"[{splited[i]}] varchar(255) null,";
                        }
                        string createDbCommandString = $"IF(DB_ID('{dbname}') IS NULL) CREATE DATABASE [{dbname}];";
                        string createDtCommandString = $"USE {dbname}; IF NOT EXISTS(SELECT [NAME] FROM SYS.TABLES WHERE [NAME] = '{dtname}') CREATE TABLE {dtname}({fields});";
                        SqlCommand createDbCommand = new SqlCommand(createDbCommandString, sqlConnection);
                        SqlCommand createDtCommand = new SqlCommand(createDtCommandString, sqlConnection);
                        createDbCommand.ExecuteNonQuery();
                        createDtCommand.ExecuteNonQuery();
                        createDtCommand.Dispose();
                        createDbCommand.Dispose();
                    }
                }
                else
                {
                    string valueString = "";
                    for (int i = 0; i < splited.Count; i++)
                    {
                        if (i == splited.Count - 1)
                        {
                            valueString += $"'{splited[i]}'";
                            break;
                        }
                        else
                        {
                            string str2 = "";
                            if (splited[i].Contains("'"))
                                str2 = splited[i].Replace("'", "''");
                            else
                                str2 = splited[i];
                            valueString += $"'{str2}',";
                        }
                    }
                    string insertCommandString = $"USE {dbname};INSERT INTO {dtname} VALUES({valueString});";
                    SqlCommand insertCommand = new SqlCommand(insertCommandString, sqlConnection);
                    try
                    {
                        await insertCommand.ExecuteNonQueryAsync();
                    }
                    catch (Exception e)
                    {
                        mainForm.textBoxLog.Invoke(
                            new OperateControls.appendTextBoxTextDelegate(OperateControls.appendTextBoxText),
                            $"\nExecute \"{insertCommandString}\" failed.\n" + $"{e.Message} \n");
                        var dialogResult = MessageBox.Show("Continue ?", "", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.OK)
                        {

                        }
                        else
                        {
                            break;
                        }
                    }
                    controlSet.toolStripStatusLabeL.Text = $@"Progress : {currentLineNumber}/{lineCount}";
                    controlSet.toolStripProgressBar.Value = currentLineNumber;
                    insertCommand.Dispose();
                    importedRecordCount++;
                }
            }
            MessageBox.Show($@"Imported {importedRecordCount} record.");
            sqlConnection.Dispose();
            streamReader.Dispose();
            fileStream.Dispose();
            return importedRecordCount;
        }

        public static int DoWriteToSqlServer(string dbname, string dtname, string connectionString, string filepath, bool firstLineColNames)
        {
            thread = new Thread(() => WriteToSqlServer(dbname, dtname, connectionString, filepath, firstLineColNames));
            thread.Start();
            return 0;
        }

        public delegate int WriteToSqlServerDelegate(string dbname, string dtname, string connectionString, string filepath, bool firstLineColNames);
        public static int WriteToSqlServer(string dbname, string dtname, string connectionString, string filepath, bool firstLineColNames)
        {
            if (!File.Exists(filepath))
            {
                mainForm.textBoxLog.AppendText($"File not found:{filepath}.\r\n");
                return -1;
            }
            if (string.IsNullOrWhiteSpace(filepath))
                return -1;
            string _connectionString = connectionString;
            sqlConnection = new SqlConnection(_connectionString);
            fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
            streamReader = new StreamReader(fileStream);
            int importedRecordCount = 0;
            if (!File.Exists(filepath))
            {
                MessageBox.Show($@"File {filepath} no found.");
                return 0;
            }
            try
            {
                sqlConnection.Open();
            }
            catch (Exception e)
            {
                mainForm.textBoxLog.AppendText($"{e.Message}\r\n");
            }
            int currentLineNumber = 0;
            long lineCount = getLineCount(streamReader, false);
            mainForm.toolStripProgressBar1.GetCurrentParent().Invoke(
                new OperateControls.setProgressBarMaxDelegate(OperateControls.SetProgressBarMax), (int)lineCount);
            streamReader.BaseStream.Position = 0;
            while (!streamReader.EndOfStream)
            {
                currentLineNumber++;
                string str = streamReader.ReadLine();
                List<string> splited = GetSplitedStrings(str);
                if (splited == null)
                {
                    continue;
                }
                if (currentLineNumber == 1)
                {
                    if (firstLineColNames)
                    {
                        string fields = "";
                        for (int i = 0; i < splited.Count; i++)
                        {
                            fields += $"[{splited[i]}] varchar(255) null,";
                        }
                        string createDbCommandString = $"IF(DB_ID('{dbname}') IS NULL) CREATE DATABASE [{dbname}];";
                        string createDtCommandString = $"USE {dbname}; IF NOT EXISTS(SELECT [NAME] FROM SYS.TABLES WHERE [NAME] = '{dtname}') CREATE TABLE [{dtname}]({fields});";
                        SqlCommand createDbCommand = new SqlCommand(createDbCommandString, sqlConnection);
                        SqlCommand createDtCommand = new SqlCommand(createDtCommandString, sqlConnection);
                        createDbCommand.ExecuteNonQuery();
                        createDtCommand.ExecuteNonQuery();
                        createDtCommand.Dispose();
                        createDbCommand.Dispose();
                    }
                    else
                    {
                        string valueString = "";
                        for (int i = 0; i < splited.Count; i++)
                        {
                            if (i == splited.Count - 1)
                            {
                                valueString += $"'{splited[i]}'";
                                break;
                            }
                            else
                            {
                                string str2 = "";
                                if (splited[i].Contains("'"))
                                    str2 = splited[i].Replace("'", "''");
                                else
                                    str2 = splited[i];
                                valueString += $"'{str2}',";
                            }
                        }
                        string insertCommandString = $"USE {dbname};INSERT INTO [{dtname}] VALUES({valueString});";
                        SqlCommand insertCommand = new SqlCommand(insertCommandString, sqlConnection);
                        try
                        {
                            insertCommand.ExecuteNonQueryAsync();
                        }
                        catch (Exception e)
                        {
                            mainForm.textBoxLog.Invoke(
                                new OperateControls.appendTextBoxTextDelegate(OperateControls.appendTextBoxText), $"\nExecute \"{insertCommandString}\" failed.\n" + $"{e.Message} \n");
                            var dialogResult = MessageBox.Show(@"Continue ?", "", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.OK)
                            {

                            }
                            else
                            {
                                break;
                            }
                        }
                        mainForm.toolStripStatusLabel1.GetCurrentParent().Invoke(
                            new OperateControls.setStatusLabelTextDelegate(OperateControls.setStatusLabelText), $@"Progress : {currentLineNumber}/{lineCount}");
                        mainForm.toolStripProgressBar1.GetCurrentParent().Invoke(
                            new OperateControls.setProgressBarValueDelegate(OperateControls.setProgressBar),
                            currentLineNumber);
                        insertCommand.Dispose();
                        importedRecordCount++;
                    }

                }
                else
                {
                    string valueString = "";
                    for (int i = 0; i < splited.Count; i++)
                    {
                        if (i == splited.Count - 1)
                        {
                            valueString += $"'{splited[i]}'";
                            break;
                        }
                        else
                        {
                            string str2 = "";
                            if (splited[i].Contains("'"))
                                str2 = splited[i].Replace("'", "''");
                            else
                                str2 = splited[i];
                            valueString += $"'{str2}',";
                        }
                    }
                    string insertCommandString = $"USE {dbname};INSERT INTO [{dtname}] VALUES({valueString});";
                    SqlCommand insertCommand = new SqlCommand(insertCommandString, sqlConnection);
                    try
                    {
                        insertCommand.ExecuteNonQueryAsync();
                    }
                    catch (Exception e)
                    {
                        mainForm.textBoxLog.Invoke(
                            new OperateControls.appendTextBoxTextDelegate(OperateControls.appendTextBoxText),
                            $"\nExecute \"{insertCommandString}\" failed.\n" + $"{e.Message} \n");
                        var dialogResult = MessageBox.Show("Continue ?", "", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.OK)
                        {

                        }
                        else
                        {
                            break;
                        }
                    }
                    mainForm.toolStripStatusLabel1.GetCurrentParent().Invoke(
                        new OperateControls.setStatusLabelTextDelegate(OperateControls.setStatusLabelText), $@"Progress : {currentLineNumber}/{lineCount}");
                    mainForm.toolStripProgressBar1.GetCurrentParent().Invoke(
                        new OperateControls.setProgressBarValueDelegate(OperateControls.setProgressBar),
                        currentLineNumber);
                    insertCommand.Dispose();
                    importedRecordCount++;
                }
            }
            MessageBox.Show($@"Imported {importedRecordCount} record.");
            sqlConnection.Dispose();
            streamReader.Dispose();
            fileStream.Dispose();
            return importedRecordCount;
        }

        public static List<string> GetSplitedStrings(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;
            List<string> list = new List<string>();
            string pattern = "\"[^\"\"]{0,}\"";
            Regex regex = new Regex(pattern);
            MatchCollection matchCollection = regex.Matches(text);
            foreach (Match m in matchCollection)
            {
                list.Add(m.Value.Substring(1, m.Value.Length - 2));
            }
            return list;
        }

        public static long getLineCount(StreamReader reader, bool ignoreFirstLine)
        {
            long count = 0;
            while (!reader.EndOfStream)
            {
                string s = reader.ReadLine();
                if (s == "\n")
                    continue;
                count++;
            }

            return ignoreFirstLine ? count - 1 : count;
        }

        public static void setProgressBar(ToolStripProgressBar s)
        {
            s.Value = 80;
        }

        public static void setProgressBarValue(ToolStripProgressBar o, int value)
        {
            o.Value = value;
        }

        public static void setProgressBarMaxValue(ToolStripProgressBar o, int value)
        {
            o.Maximum = value;
        }

        public static void setStatusLabel(ToolStripStatusLabel t)
        {
            //t.Text = @"1000000/1000000   Error occurs.";
        }

        public static async void Pause()
        {
            bool formClosing = Program.getMainForm().formClosing;
            while (formClosing)
            {
                formClosing = Program.getMainForm().formClosing;
            }
        }

        public static async Task<int> ImportToMySqlAsync(string dbname, string dtname, string connectionString, string filepath, ControlSet controlSet, bool firstLineColNames)
        {
            if (string.IsNullOrWhiteSpace(filepath))
                return -1;
            string _connectionString = connectionString;// @"server=localhost;user id=root;persistsecurityinfo=False;database=sakila";
            MySqlConnection mySqlConnection;
            mySqlConnection = new MySqlConnection(_connectionString);
            FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);

            StreamReader streamReader = new StreamReader(fileStream);
            int importedRecordCount = 0;
            if (!File.Exists(filepath))
            {
                MessageBox.Show($@"File {filepath} no found.");
                return 0;
            }
            //try
            //{
            mySqlConnection.Open();
            //}
            //catch (Exception e)
            //{
            //    controlSet.textBoxLog.AppendText($"{e.Message}\r\n");
            //}            
            int currentLineNumber = 0;
            long lineCount = getLineCount(streamReader, false);
            setProgressBarMaxValue(controlSet.toolStripProgressBar, (int)lineCount);
            streamReader.BaseStream.Position = 0;
            while (!streamReader.EndOfStream)
            {
                currentLineNumber++;
                string str = streamReader.ReadLine();
                List<string> splited = GetSplitedStrings(str);
                if (splited == null)
                {
                    continue;
                }
                // When current line number is 1
                if (currentLineNumber == 1)
                {
                    // If the first line contains column name
                    if (firstLineColNames)
                    {
                        string fields = "";
                        //if(string.IsNullOrWhiteSpace(splited))
                        for (int i = 0; i < splited.Count; i++)
                        {
                            if (i == splited.Count - 1)
                                fields += $"`{splited[i]}` varchar(255) null";
                            else
                                fields += $"`{splited[i]}` varchar(255) null,";
                        }
                        //string checkDbExistsString = "IF (DB_ID) is "
                        string createDbCommandString = $"CREATE DATABASE IF NOT EXISTS {dbname};";
                        string createDtCommandString = $"USE {dbname} ; CREATE TABLE IF NOT EXISTS {dtname}({fields});";
                        //controlSet.textBoxLog.AppendText(createDtCommandString);
                        MySqlCommand createDbCommand = new MySqlCommand(createDbCommandString, mySqlConnection);
                        controlSet.textBoxLog.AppendText($"Executed {createDbCommandString} successfully.\r\n");
                        MySqlCommand createDtCommand = new MySqlCommand(createDtCommandString, mySqlConnection);
                        controlSet.textBoxLog.AppendText($"Executed {createDtCommandString} successfully.\r\n");
                        createDbCommand.ExecuteNonQuery();
                        createDtCommand.ExecuteNonQuery();
                        createDtCommand.Dispose();
                        createDbCommand.Dispose();
                    }
                    else
                    {
                        string valueString = "";
                        for (int i = 0; i < splited.Count; i++)
                        {
                            if (i == splited.Count - 1)
                            {
                                valueString += $"'{splited[i]}'";
                                break;
                            }
                            string str2 = "";
                            if (splited[i].Contains("'"))
                                str2 = splited[i].Replace("'", "''");
                            else
                                str2 = splited[i];
                            valueString += $"'{str2}',";
                        }
                        string insertCommandString = $"USE {dbname};INSERT INTO {dtname} VALUES({valueString});";
                        //try
                        //{

                        MySqlCommand insertCommand = new MySqlCommand(insertCommandString, mySqlConnection);
                        try
                        {
                            await insertCommand.ExecuteNonQueryAsync();
                        }
                        catch (Exception e)
                        {
                            //controlSet.toolStripStatusLabeL.Text += @"  Text format incorrect.";
                            Program.getMainForm().gettextBoxLog().AppendText($"\nExecute \"{insertCommandString}\" failed.\n" + $"{e.Message} \n");
                            var dialogResult = MessageBox.Show("Continue ?", "", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.OK)
                            {

                            }
                            else
                            {
                                break;
                            }
                        }
                        controlSet.toolStripStatusLabeL.Text = $@"Progress : {currentLineNumber}/{lineCount}";
                        controlSet.toolStripProgressBar.Value = currentLineNumber;
                        insertCommand.Dispose();
                        importedRecordCount++;
                    }
                }
                else
                {
                    string valueString = "";
                    for (int i = 0; i < splited.Count; i++)
                    {
                        if (i == splited.Count - 1)
                        {
                            valueString += $"'{splited[i]}'";
                            break;
                        }
                        else
                        {
                            string str2 = "";
                            if (splited[i].Contains("'"))
                                str2 = splited[i].Replace("'", "''");
                            else
                                str2 = splited[i];
                            valueString += $"'{str2}',";
                        }
                    }
                    string insertCommandString = $"USE {dbname};INSERT INTO {dtname} VALUES({valueString});";
                    //try
                    //{

                    MySqlCommand insertCommand = new MySqlCommand(insertCommandString, mySqlConnection);
                    try
                    {
                        controlSet.textBoxLog.AppendText($"Execute {insertCommandString}.\r\n");
                        await insertCommand.ExecuteNonQueryAsync();
                    }
                    catch (Exception e)
                    {
                        //controlSet.toolStripStatusLabeL.Text += @"  Text format incorrect.";
                        Program.getMainForm().gettextBoxLog().AppendText($"\nExecute \"{insertCommandString}\" failed.\n" + $"{e.Message} \n");
                        var dialogResult = MessageBox.Show(@"Continue ?", "", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            continue;
                        }
                        break;
                    }
                    controlSet.toolStripStatusLabeL.Text = $@"Progress : {currentLineNumber}/{lineCount}";
                    controlSet.toolStripProgressBar.Value = currentLineNumber;
                    insertCommand.Dispose();
                    importedRecordCount++;
                }
            }
            MessageBox.Show($@"Imported {importedRecordCount} record.");
            mySqlConnection.Dispose();
            streamReader.Dispose();
            fileStream.Dispose();
            return importedRecordCount;
        }

        public static int DoImportToMySql()
        {
            //thread = new Thread(()=>ImportToMySql());
            return 0;
        }
        public static int ImportToMySql(string dbname, string dtname, string connectionString, string filepath, bool firstLineColName)
        {
            thread = new Thread(() =>
                ImportToMySqlThread(dbname, dtname, connectionString, filepath, firstLineColName));
            thread.Start();
            return 0;
        }
        public delegate int ImpportToMySqlDelegate(string dbname, string dtname, string connectionString, string filepath, bool firstLineColNames);
        public static int ImportToMySqlThread(string dbname, string dtname, string connectionString, string filepath, bool firstLineColNames)
        {
            if (string.IsNullOrWhiteSpace(filepath))
                return -1;
            string _connectionString = connectionString;

            mySqlConnection = new MySqlConnection(_connectionString);
            FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);

            StreamReader streamReader = new StreamReader(fileStream);
            int importedRecordCount = 0;
            if (!File.Exists(filepath))
            {
                MessageBox.Show($@"File {filepath} no found.");
                return 0;
            }
            mySqlConnection.Open();
            int currentLineNumber = 0;
            long lineCount = getLineCount(streamReader, false);
            mainForm.toolStripProgressBar1.GetCurrentParent()
                .Invoke(new OperateControls.setProgressBarMaxDelegate(OperateControls.SetProgressBarMax), (int)lineCount);
            streamReader.BaseStream.Position = 0;
            while (!streamReader.EndOfStream)
            {
                currentLineNumber++;
                string str = streamReader.ReadLine();
                List<string> splited = GetSplitedStrings(str);
                if (splited == null)
                {
                    continue;
                }
                // When current line number is 1
                if (currentLineNumber == 1)
                {
                    // If the first line contains column name
                    if (firstLineColNames)
                    {
                        string fields = "";
                        for (int i = 0; i < splited.Count; i++)
                        {
                            if (i == splited.Count - 1)
                                fields += $"`{splited[i]}` varchar(255) null";
                            else
                                fields += $"`{splited[i]}` varchar(255) null,";
                        }
                        string createDbCommandString = $"CREATE DATABASE IF NOT EXISTS {dbname};";
                        string createDtCommandString = $"USE {dbname} ; CREATE TABLE IF NOT EXISTS {dtname}({fields});";
                        MySqlCommand createDbCommand = new MySqlCommand(createDbCommandString, mySqlConnection);
                        mainForm.textBoxLog.Invoke(
                            new OperateControls.appendTextBoxTextDelegate(OperateControls.appendTextBoxText),
                            $"Executed {createDbCommandString} successfully.\r\n");
                        MySqlCommand createDtCommand = new MySqlCommand(createDtCommandString, mySqlConnection);
                        mainForm.textBoxLog.Invoke(
                            new OperateControls.appendTextBoxTextDelegate(OperateControls.appendTextBoxText),
                            $"Executed {createDbCommandString} successfully.\r\n");
                        createDbCommand.ExecuteNonQuery();
                        createDtCommand.ExecuteNonQuery();
                        createDtCommand.Dispose();
                        createDbCommand.Dispose();
                    }
                    else
                    {
                        string valueString = "";
                        for (int i = 0; i < splited.Count; i++)
                        {
                            if (i == splited.Count - 1)
                            {
                                valueString += $"'{splited[i]}'";
                                break;
                            }
                            string str2 = "";
                            if (splited[i].Contains("'"))
                                str2 = splited[i].Replace("'", "''");
                            else
                                str2 = splited[i];
                            valueString += $"'{str2}',";
                        }
                        string insertCommandString = $"USE {dbname};INSERT INTO {dtname} VALUES({valueString});";
                        MySqlCommand insertCommand = new MySqlCommand(insertCommandString, mySqlConnection);
                        try
                        {
                            insertCommand.ExecuteNonQueryAsync();
                        }
                        catch (Exception e)
                        {
                            Program.getMainForm().gettextBoxLog().AppendText($"\nExecute \"{insertCommandString}\" failed.\n" + $"{e.Message} \n");
                            mainForm.textBoxLog.Invoke(
                                new OperateControls.appendTextBoxTextDelegate(OperateControls.appendTextBoxText),
                                $"\nExecute \"{insertCommandString}\" failed.\n" + $"{e.Message} \n");
                            var dialogResult = MessageBox.Show("Continue ?", "", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.OK)
                            {
                                continue;
                            }
                            break;
                        }
                        mainForm.toolStripStatusLabel1.GetCurrentParent().Invoke(
                            new OperateControls.setStatusLabelTextDelegate(OperateControls.setStatusLabelText), $@"Progress : {currentLineNumber}/{lineCount}");
                        mainForm.toolStripStatusLabel1.GetCurrentParent().Invoke(
                            new OperateControls.setProgressBarValueDelegate(OperateControls.setProgressBar),
                            currentLineNumber);
                        insertCommand.Dispose();
                        importedRecordCount++;
                    }
                }
                else
                {
                    string valueString = "";
                    for (int i = 0; i < splited.Count; i++)
                    {
                        if (i == splited.Count - 1)
                        {
                            valueString += $"'{splited[i]}'";
                            break;
                        }
                        else
                        {
                            string str2 = "";
                            if (splited[i].Contains("'"))
                                str2 = splited[i].Replace("'", "''");
                            else
                                str2 = splited[i];
                            valueString += $"'{str2}',";
                        }
                    }
                    string insertCommandString = $"USE {dbname};INSERT INTO {dtname} VALUES({valueString});";
                    MySqlCommand insertCommand = new MySqlCommand(insertCommandString, mySqlConnection);
                    try
                    {
                        mainForm.textBoxLog.Invoke(
                            new OperateControls.appendTextBoxTextDelegate(OperateControls.appendTextBoxText), $"Execute {insertCommandString}.\r\n");
                        insertCommand.ExecuteNonQueryAsync();
                    }
                    catch (Exception e)
                    {
                        mainForm.textBoxLog.Invoke(
                            new OperateControls.appendTextBoxTextDelegate(OperateControls.appendTextBoxText),
                            $"\nExecute \"{insertCommandString}\" failed.\n" + $"{e.Message} \n");
                        var dialogResult = MessageBox.Show(@"Continue ?", "", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            continue;
                        }
                        break;
                    }
                    mainForm.toolStripStatusLabel1.GetCurrentParent().Invoke(
                        new OperateControls.setStatusLabelTextDelegate(OperateControls.setStatusLabelText),
                        $@"Progress : {currentLineNumber}/{lineCount}");
                    mainForm.toolStripProgressBar1.GetCurrentParent().Invoke(
                        new OperateControls.setProgressBarValueDelegate(OperateControls.setProgressBar),
                        currentLineNumber);
                    insertCommand.Dispose();
                    importedRecordCount++;
                }
            }
            MessageBox.Show($@"Imported {importedRecordCount} record.");
            mySqlConnection.Dispose();
            streamReader.Dispose();
            fileStream.Dispose();
            return importedRecordCount;
        }
        internal static void Dispose()
        {
            if (thread != null)
                thread.Abort();
        }
    }
}
