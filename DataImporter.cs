using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
using MySql.Data.MySqlClient;

namespace CSVtoDatabase
{
    public class DataImporter
    {
        private static readonly Form1 mainForm = Program.GetMainForm();
        private static Thread _thread = null;
        MySqlConnection _mySqlConnection = null;
        SqlConnection _sqlConnection;
        FileStream _fileStream = null;
        StreamReader _streamReader = null;

        public DataImporter()
        {

        }

        public DataImporter(SqlConnection connection)
        {
            _sqlConnection = connection;
        }
        public DataImporter(MySqlConnection connection)
        {
            _mySqlConnection = connection;
        }
        /// <summary>
        /// Import data from text file to Sql server
        /// </summary>
        /// <param name="dbname">Database name</param>
        /// <param name="filepath">Filepath</param>
        /// <param name="dtname">Datatable name</param>
        /// <param name="controlSet"></param>
        /// <param name="firstLineColNames">Whether ignore first line, default value is <value>false</value></param>
        /// <returns></returns>
        public async Task<int> WriteToSqlServer(string dbname, string dtname, string filepath, ControlSet controlSet, bool firstLineColNames)
        {
            if (string.IsNullOrWhiteSpace(filepath))
                return -1;
            string connectionString = @"Data Source=.;Initial Catalog=master;Integrated Security=True";//ConfigurationManager.ConnectionStrings["DBmaster"].ConnectionString;
            _sqlConnection = new SqlConnection(connectionString);
            _fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
            _streamReader = new StreamReader(_fileStream);
            int importedRecordCount = 0;
            if (!File.Exists(filepath))
            {
                MessageBox.Show($@"File {filepath} no found.");
                return -1;
            }
            try
            {
                _sqlConnection.Open();
            }
            catch (Exception e)
            {
                controlSet.textBoxLog.AppendText($"{e.Message}\r\n");
            }
            int currentLineNumber = 0;
            long lineCount = GetLineCount(_streamReader, false);
            mainForm.toolStripProgressBar1.GetCurrentParent().Invoke(
                new OperateControls.setProgressBarValueDelegate(OperateControls.setProgressBar), (int)lineCount);
            _streamReader.BaseStream.Position = 0;
            while (!_streamReader.EndOfStream)
            {
                currentLineNumber++;
                string str = _streamReader.ReadLine();
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
                        SqlCommand createDbCommand = new SqlCommand(createDbCommandString, _sqlConnection);
                        SqlCommand createDtCommand = new SqlCommand(createDtCommandString, _sqlConnection);
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
                            string str2;
                            if (splited[i].Contains("'"))
                                str2 = splited[i].Replace("'", "''");
                            else
                                str2 = splited[i];
                            valueString += $"'{str2}',";
                        }
                    }
                    string insertCommandString = $"USE {dbname};INSERT INTO {dtname} VALUES({valueString});";
                    SqlCommand insertCommand = new SqlCommand(insertCommandString, _sqlConnection);
                    try
                    {
                        await insertCommand.ExecuteNonQueryAsync();
                    }
                    catch (Exception e)
                    {
                        mainForm.textBoxLog.Invoke(
                            new OperateControls.appendTextBoxTextDelegate(OperateControls.appendTextBoxText),
                            $"\nExecute \"{insertCommandString}\" failed.\n" + $"{e.Message} \n");
                        var dialogResult = MessageBox.Show(@"Continue ?", "", MessageBoxButtons.YesNo);
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
            _sqlConnection.Dispose();
            _streamReader.Dispose();
            _fileStream.Dispose();
            return importedRecordCount;
        }

        public int ImportToSqlServer(string dbname, string dtname, string connectionString, string filepath, bool firstLineColNames)
        {
            _thread = new Thread(() => WriteToSqlServer(dbname, dtname, connectionString, filepath, firstLineColNames));
            _thread.Start();
            return 0;
        }

        public delegate int WriteToSqlServerDelegate(string dbname, string dtname, string connectionString, string filepath, bool firstLineColNames);
        public int WriteToSqlServer(string dbname, string dtname, string connectionString, string filepath, bool firstLineColNames)
        {
            if (!File.Exists(filepath))
            {
                mainForm.textBoxLog.AppendText($"File not found:{filepath}.\r\n");
                return -1;
            }
            if (string.IsNullOrWhiteSpace(filepath))
                return -1;
            _sqlConnection = new SqlConnection(connectionString);
            _fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
            _streamReader = new StreamReader(_fileStream);
            int importedRecordCount = 0;
            if (!File.Exists(filepath))
            {
                MessageBox.Show($@"File {filepath} no found.");
                return 0;
            }
            try
            {
                _sqlConnection.Open();
            }
            catch (Exception e)
            {
                mainForm.textBoxLog.AppendText($"{e.Message}\r\n");
            }
            int currentLineNumber = 0;
            long lineCount = GetLineCount(_streamReader, false);
            mainForm.toolStripProgressBar1.GetCurrentParent().Invoke(
                new OperateControls.setProgressBarMaxDelegate(OperateControls.SetProgressBarMax), (int)lineCount);
            _streamReader.BaseStream.Position = 0;
            while (!_streamReader.EndOfStream)
            {
                currentLineNumber++;
                string str = _streamReader.ReadLine();
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
                        SqlCommand createDbCommand = new SqlCommand(createDbCommandString, _sqlConnection);
                        SqlCommand createDtCommand = new SqlCommand(createDtCommandString, _sqlConnection);
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
                            string str2;
                            str2 = splited[i].Contains("'") ? splited[i].Replace("'", "''") : splited[i];
                            valueString += $"'{str2}',";
                        }
                        string insertCommandString = $"USE {dbname};INSERT INTO [{dtname}] VALUES({valueString});";
                        SqlCommand insertCommand = new SqlCommand(insertCommandString, _sqlConnection);
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
                        string str2;
                        if (splited[i].Contains("'"))
                            str2 = splited[i].Replace("'", "''");
                        else
                            str2 = splited[i];
                        valueString += $"'{str2}',";
                    }
                    string insertCommandString = $"USE {dbname};INSERT INTO [{dtname}] VALUES({valueString});";
                    SqlCommand insertCommand = new SqlCommand(insertCommandString, _sqlConnection);
                    try
                    {
                        insertCommand.ExecuteNonQueryAsync();
                    }
                    catch (Exception e)
                    {
                        mainForm.textBoxLog.Invoke(
                            new OperateControls.appendTextBoxTextDelegate(OperateControls.appendTextBoxText),
                            $"\nExecute \"{insertCommandString}\" failed.\n" + $"{e.Message} \n");
                        var dialogResult = MessageBox.Show(@"Continue ?", "", buttons: MessageBoxButtons.YesNo);
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
            _sqlConnection.Dispose();
            _streamReader.Dispose();
            _fileStream.Dispose();
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

        public static long GetLineCount(StreamReader reader, bool ignoreFirstLine)
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

        public void SetProgressBar(ToolStripProgressBar s)
        {
            s.Value = 80;
        }

        public void SetProgressBarValue(ToolStripProgressBar o, int value)
        {
            o.Value = value;
        }

        public void SetProgressBarMaxValue(ToolStripProgressBar o, int value)
        {
            o.Maximum = value;
        }

        public void SetStatusLabel(ToolStripStatusLabel t)
        {
            //t.Text = @"1000000/1000000   Error occurs.";
        }

        public void Pause()
        {
            bool formClosing = Program.GetMainForm()._formClosing;
            while (formClosing)
            {
                formClosing = Program.GetMainForm()._formClosing;
            }
        }

        public async Task<int> ImportToMySqlAsync(string dbname, string dtname, string connectionString, string filepath, ControlSet controlSet, bool firstLineColNames)
        {
            if (string.IsNullOrWhiteSpace(filepath))
                return -1;
            // @"server=localhost;user id=root;persistsecurityinfo=False;database=sakila";
            MySqlConnection mySqlConnection;
            mySqlConnection = new MySqlConnection(connectionString);
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
            long lineCount = GetLineCount(streamReader, false);
            SetProgressBarMaxValue(controlSet.toolStripProgressBar, (int)lineCount);
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
                            string str2;
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
                            Program.GetMainForm().GettextBoxLog().AppendText($"\nExecute \"{insertCommandString}\" failed.\n" + $"{e.Message} \n");
                            var dialogResult = MessageBox.Show(@"Continue ?", "", MessageBoxButtons.YesNo);
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
                            string str2;
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
                        Program.GetMainForm().GettextBoxLog().AppendText($"\nExecute \"{insertCommandString}\" failed.\n" + $"{e.Message} \n");
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
        public int ImportToMySql(string dbname, string dtname, string connectionString, string filepath, bool firstLineColName)
        {
            _thread = new Thread(() =>
                ImportToMySqlThread(dbname, dtname, connectionString, filepath, firstLineColName));
            _thread.Start();
            return 0;
        }
        public delegate int ImpportToMySqlDelegate(string dbname, string dtname, string connectionString, string filepath, bool firstLineColNames);
        public int ImportToMySqlThread(string dbname, string dtname, string connectionString, string filepath, bool firstLineColNames)
        {
            if (string.IsNullOrWhiteSpace(filepath))
                return -1;
            _mySqlConnection = new MySqlConnection(connectionString);
            FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);

            StreamReader streamReader = new StreamReader(fileStream);
            int importedRecordCount = 0;
            if (!File.Exists(filepath))
            {
                MessageBox.Show($@"File {filepath} no found.");
                return 0;
            }
            _mySqlConnection.Open();
            int currentLineNumber = 0;
            long lineCount = GetLineCount(streamReader, false);
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
                        MySqlCommand createDbCommand = new MySqlCommand(createDbCommandString, _mySqlConnection);
                        mainForm.textBoxLog.Invoke(
                            new OperateControls.appendTextBoxTextDelegate(OperateControls.appendTextBoxText),
                            $"Executed {createDbCommandString} successfully.\r\n");
                        MySqlCommand createDtCommand = new MySqlCommand(createDtCommandString, _mySqlConnection);
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
                            string str2;
                            if (splited[i].Contains("'"))
                                str2 = splited[i].Replace("'", "''");
                            else
                                str2 = splited[i];
                            valueString += $"'{str2}',";
                        }
                        string insertCommandString = $"USE {dbname};INSERT INTO {dtname} VALUES({valueString});";
                        MySqlCommand insertCommand = new MySqlCommand(insertCommandString, _mySqlConnection);
                        try
                        {
                            insertCommand.ExecuteNonQueryAsync();
                        }
                        catch (Exception e)
                        {
                            Program.GetMainForm().GettextBoxLog().AppendText($"\nExecute \"{insertCommandString}\" failed.\n" + $"{e.Message} \n");
                            mainForm.textBoxLog.Invoke(
                                new OperateControls.appendTextBoxTextDelegate(OperateControls.appendTextBoxText),
                                $"\nExecute \"{insertCommandString}\" failed.\n" + $"{e.Message} \n");
                            var dialogResult = MessageBox.Show(@"Continue ?", "", MessageBoxButtons.YesNo);
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
                            string str2;
                            if (splited[i].Contains("'"))
                                str2 = splited[i].Replace("'", "''");
                            else
                                str2 = splited[i];
                            valueString += $"'{str2}',";
                    }
                    string insertCommandString = $"USE {dbname};INSERT INTO {dtname} VALUES({valueString});";
                    MySqlCommand insertCommand = new MySqlCommand(insertCommandString, _mySqlConnection);
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
            _mySqlConnection.Dispose();
            streamReader.Dispose();
            fileStream.Dispose();
            return importedRecordCount;
        }
        internal void Dispose()
        {
            _thread?.Abort();
        }
    }
}
