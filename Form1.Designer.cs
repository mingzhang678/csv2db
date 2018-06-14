namespace CSVtoDatabase
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxFilePath = new System.Windows.Forms.TextBox();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.btnChooseFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnImport = new System.Windows.Forms.Button();
            this.listBoxDatabase = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBoxIgnoreFirstLine = new System.Windows.Forms.CheckBox();
            this.comboBoxDbList = new System.Windows.Forms.ComboBox();
            this.comboBoxDtList = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.textBoxUId = new System.Windows.Forms.TextBox();
            this.textBoxPwd = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBoxIntegratedSecurity = new System.Windows.Forms.CheckBox();
            this.btnCheckConnection = new System.Windows.Forms.Button();
            this.textBoxServer = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPauseExport = new System.Windows.Forms.Button();
            this.btnStopExport = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxFilePath
            // 
            this.textBoxFilePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxFilePath.Location = new System.Drawing.Point(16, 81);
            this.textBoxFilePath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxFilePath.Name = "textBoxFilePath";
            this.textBoxFilePath.Size = new System.Drawing.Size(695, 30);
            this.textBoxFilePath.TabIndex = 0;
            this.textBoxFilePath.Text = "C:\\Users\\lrz\\Desktop\\FTP\\IP2LOCATION-LITE-DB11-3345.CSV";
            this.textBoxFilePath.TextChanged += new System.EventHandler(this.textBoxFilePath_TextChanged);
            // 
            // textBoxLog
            // 
            this.textBoxLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLog.Location = new System.Drawing.Point(0, 325);
            this.textBoxLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLog.Size = new System.Drawing.Size(1305, 693);
            this.textBoxLog.TabIndex = 1;
            this.textBoxLog.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // btnChooseFile
            // 
            this.btnChooseFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChooseFile.Location = new System.Drawing.Point(739, 81);
            this.btnChooseFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnChooseFile.Name = "btnChooseFile";
            this.btnChooseFile.Size = new System.Drawing.Size(111, 32);
            this.btnChooseFile.TabIndex = 2;
            this.btnChooseFile.Text = "Choose file";
            this.btnChooseFile.UseVisualStyleBackColor = true;
            this.btnChooseFile.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.LightSeaGreen;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(11, 39);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 27);
            this.label1.TabIndex = 3;
            this.label1.Text = "CSV file";
            // 
            // btnImport
            // 
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.Location = new System.Drawing.Point(833, 241);
            this.btnImport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(139, 39);
            this.btnImport.TabIndex = 4;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // listBoxDatabase
            // 
            this.listBoxDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxDatabase.FormattingEnabled = true;
            this.listBoxDatabase.ItemHeight = 20;
            this.listBoxDatabase.Items.AddRange(new object[] {
            "SQL Server",
            "MySQL"});
            this.listBoxDatabase.Location = new System.Drawing.Point(16, 167);
            this.listBoxDatabase.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listBoxDatabase.Name = "listBoxDatabase";
            this.listBoxDatabase.Size = new System.Drawing.Size(203, 44);
            this.listBoxDatabase.TabIndex = 5;
            this.listBoxDatabase.SelectedIndexChanged += new System.EventHandler(this.listBoxDatabase_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.LightSeaGreen;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 137);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 27);
            this.label2.TabIndex = 7;
            this.label2.Text = "Source";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.LightSeaGreen;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(393, 255);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 24);
            this.label3.TabIndex = 9;
            this.label3.Text = "Database name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.LightSeaGreen;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(395, 290);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(140, 24);
            this.label4.TabIndex = 10;
            this.label4.Text = "Datatable name";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileFToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1307, 28);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileFToolStripMenuItem
            // 
            this.fileFToolStripMenuItem.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.fileFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveLogToolStripMenuItem,
            this.exitXToolStripMenuItem});
            this.fileFToolStripMenuItem.Name = "fileFToolStripMenuItem";
            this.fileFToolStripMenuItem.Size = new System.Drawing.Size(61, 24);
            this.fileFToolStripMenuItem.Text = "File(&F)";
            this.fileFToolStripMenuItem.Click += new System.EventHandler(this.fileFToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(159, 26);
            this.openToolStripMenuItem.Text = "Open(&O)";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveLogToolStripMenuItem
            // 
            this.saveLogToolStripMenuItem.Name = "saveLogToolStripMenuItem";
            this.saveLogToolStripMenuItem.Size = new System.Drawing.Size(159, 26);
            this.saveLogToolStripMenuItem.Text = "Save log(&S)";
            this.saveLogToolStripMenuItem.Click += new System.EventHandler(this.saveLogToolStripMenuItem_Click);
            // 
            // exitXToolStripMenuItem
            // 
            this.exitXToolStripMenuItem.Name = "exitXToolStripMenuItem";
            this.exitXToolStripMenuItem.Size = new System.Drawing.Size(159, 26);
            this.exitXToolStripMenuItem.Text = "Exit(&X)";
            this.exitXToolStripMenuItem.Click += new System.EventHandler(this.exitXToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.cutToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.clearLogToolStripMenuItem,
            this.preferenceToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(65, 24);
            this.editToolStripMenuItem.Text = "Edit(&E)";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(212, 26);
            this.copyToolStripMenuItem.Text = "Copy             Ctrl+C";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(212, 26);
            this.cutToolStripMenuItem.Text = "Cut                Ctrl+X";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(212, 26);
            this.pasteToolStripMenuItem.Text = "Paste             Ctrl+V";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // clearLogToolStripMenuItem
            // 
            this.clearLogToolStripMenuItem.Name = "clearLogToolStripMenuItem";
            this.clearLogToolStripMenuItem.Size = new System.Drawing.Size(212, 26);
            this.clearLogToolStripMenuItem.Text = "Clear log";
            this.clearLogToolStripMenuItem.Click += new System.EventHandler(this.clearLogToolStripMenuItem_Click);
            // 
            // preferenceToolStripMenuItem
            // 
            this.preferenceToolStripMenuItem.Name = "preferenceToolStripMenuItem";
            this.preferenceToolStripMenuItem.Size = new System.Drawing.Size(212, 26);
            this.preferenceToolStripMenuItem.Text = "Preference";
            this.preferenceToolStripMenuItem.Visible = false;
            this.preferenceToolStripMenuItem.Click += new System.EventHandler(this.preferenceToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.helpToolStripMenuItem.Text = "Help(&H)";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(125, 26);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // checkBoxIgnoreFirstLine
            // 
            this.checkBoxIgnoreFirstLine.AutoSize = true;
            this.checkBoxIgnoreFirstLine.BackColor = System.Drawing.Color.LightSeaGreen;
            this.checkBoxIgnoreFirstLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxIgnoreFirstLine.Location = new System.Drawing.Point(920, 85);
            this.checkBoxIgnoreFirstLine.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxIgnoreFirstLine.Name = "checkBoxIgnoreFirstLine";
            this.checkBoxIgnoreFirstLine.Size = new System.Drawing.Size(307, 28);
            this.checkBoxIgnoreFirstLine.TabIndex = 12;
            this.checkBoxIgnoreFirstLine.Text = "First line contains column names";
            this.checkBoxIgnoreFirstLine.UseVisualStyleBackColor = false;
            this.checkBoxIgnoreFirstLine.CheckedChanged += new System.EventHandler(this.checkBoxIgnoreFirstLine_CheckedChanged);
            // 
            // comboBoxDbList
            // 
            this.comboBoxDbList.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxDbList.FormattingEnabled = true;
            this.comboBoxDbList.Location = new System.Drawing.Point(551, 250);
            this.comboBoxDbList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxDbList.Name = "comboBoxDbList";
            this.comboBoxDbList.Size = new System.Drawing.Size(273, 32);
            this.comboBoxDbList.TabIndex = 13;
            this.comboBoxDbList.SelectedIndexChanged += new System.EventHandler(this.comboBoxDbList_SelectedIndexChanged);
            this.comboBoxDbList.TextChanged += new System.EventHandler(this.comboBoxDbList_TextChanged);
            // 
            // comboBoxDtList
            // 
            this.comboBoxDtList.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxDtList.FormattingEnabled = true;
            this.comboBoxDtList.Location = new System.Drawing.Point(551, 287);
            this.comboBoxDtList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxDtList.Name = "comboBoxDtList";
            this.comboBoxDtList.Size = new System.Drawing.Size(273, 32);
            this.comboBoxDtList.TabIndex = 14;
            this.comboBoxDtList.SelectedIndexChanged += new System.EventHandler(this.comboBoxDtList_SelectedIndexChanged);
            this.comboBoxDtList.TextChanged += new System.EventHandler(this.comboBoxDtList_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(993, 154);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 17;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.LightSeaGreen;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1020);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1307, 31);
            this.statusStrip1.TabIndex = 19;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.BackColor = System.Drawing.Color.LightGreen;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(250, 26);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Maximum = 0;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(933, 25);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.AutoSize = false;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(1500, 15);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // textBoxUId
            // 
            this.textBoxUId.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxUId.Location = new System.Drawing.Point(123, 251);
            this.textBoxUId.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxUId.Name = "textBoxUId";
            this.textBoxUId.Size = new System.Drawing.Size(219, 29);
            this.textBoxUId.TabIndex = 20;
            this.textBoxUId.Text = "root";
            // 
            // textBoxPwd
            // 
            this.textBoxPwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPwd.Location = new System.Drawing.Point(123, 288);
            this.textBoxPwd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxPwd.Name = "textBoxPwd";
            this.textBoxPwd.PasswordChar = '*';
            this.textBoxPwd.Size = new System.Drawing.Size(219, 29);
            this.textBoxPwd.TabIndex = 21;
            this.textBoxPwd.Text = "000000";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.LightSeaGreen;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(17, 258);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 24);
            this.label5.TabIndex = 22;
            this.label5.Text = "Username";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.LightSeaGreen;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(17, 292);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 24);
            this.label6.TabIndex = 22;
            this.label6.Text = "Password";
            // 
            // checkBoxIntegratedSecurity
            // 
            this.checkBoxIntegratedSecurity.AutoSize = true;
            this.checkBoxIntegratedSecurity.BackColor = System.Drawing.Color.Aquamarine;
            this.checkBoxIntegratedSecurity.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxIntegratedSecurity.Location = new System.Drawing.Point(253, 207);
            this.checkBoxIntegratedSecurity.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxIntegratedSecurity.Name = "checkBoxIntegratedSecurity";
            this.checkBoxIntegratedSecurity.Size = new System.Drawing.Size(327, 28);
            this.checkBoxIntegratedSecurity.TabIndex = 23;
            this.checkBoxIntegratedSecurity.Text = "Integrated security(Sql Server Only)";
            this.checkBoxIntegratedSecurity.UseVisualStyleBackColor = false;
            // 
            // btnCheckConnection
            // 
            this.btnCheckConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckConnection.Location = new System.Drawing.Point(797, 145);
            this.btnCheckConnection.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCheckConnection.Name = "btnCheckConnection";
            this.btnCheckConnection.Size = new System.Drawing.Size(139, 43);
            this.btnCheckConnection.TabIndex = 24;
            this.btnCheckConnection.Text = "Connect";
            this.btnCheckConnection.UseVisualStyleBackColor = true;
            this.btnCheckConnection.Click += new System.EventHandler(this.btnCheckConnection_Click);
            // 
            // textBoxServer
            // 
            this.textBoxServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxServer.Location = new System.Drawing.Point(253, 167);
            this.textBoxServer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxServer.Name = "textBoxServer";
            this.textBoxServer.Size = new System.Drawing.Size(391, 29);
            this.textBoxServer.TabIndex = 25;
            this.textBoxServer.Text = "localhost";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.LightSeaGreen;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(253, 137);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(179, 26);
            this.label7.TabIndex = 26;
            this.label7.Text = "Server address";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisconnect.Location = new System.Drawing.Point(797, 192);
            this.btnDisconnect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(139, 42);
            this.btnDisconnect.TabIndex = 27;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(833, 283);
            this.btnExport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(139, 38);
            this.btnExport.TabIndex = 28;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnPauseExport
            // 
            this.btnPauseExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPauseExport.Location = new System.Drawing.Point(980, 284);
            this.btnPauseExport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPauseExport.Name = "btnPauseExport";
            this.btnPauseExport.Size = new System.Drawing.Size(116, 38);
            this.btnPauseExport.TabIndex = 29;
            this.btnPauseExport.Text = "Pause";
            this.btnPauseExport.UseVisualStyleBackColor = true;
            this.btnPauseExport.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnStopExport
            // 
            this.btnStopExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopExport.Location = new System.Drawing.Point(1104, 284);
            this.btnStopExport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStopExport.Name = "btnStopExport";
            this.btnStopExport.Size = new System.Drawing.Size(107, 38);
            this.btnStopExport.TabIndex = 30;
            this.btnStopExport.Text = "Stop";
            this.btnStopExport.UseVisualStyleBackColor = true;
            this.btnStopExport.Click += new System.EventHandler(this.btnStopExport_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkCyan;
            this.ClientSize = new System.Drawing.Size(1307, 1051);
            this.Controls.Add(this.btnStopExport);
            this.Controls.Add(this.btnPauseExport);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxServer);
            this.Controls.Add(this.btnCheckConnection);
            this.Controls.Add(this.checkBoxIntegratedSecurity);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxPwd);
            this.Controls.Add(this.textBoxUId);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBoxDtList);
            this.Controls.Add(this.comboBoxDbList);
            this.Controls.Add(this.checkBoxIgnoreFirstLine);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listBoxDatabase);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnChooseFile);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.textBoxFilePath);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Csv2Db";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        internal System.Windows.Forms.TextBox textBoxFilePath;
        internal System.Windows.Forms.TextBox textBoxLog;
        internal System.Windows.Forms.Button btnChooseFile;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button btnImport;
        internal System.Windows.Forms.ListBox listBoxDatabase;
        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.Label label4;
        internal System.Windows.Forms.MenuStrip menuStrip1;
        internal System.Windows.Forms.ToolStripMenuItem fileFToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem exitXToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem preferenceToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        internal System.Windows.Forms.CheckBox checkBoxIgnoreFirstLine;
        internal System.Windows.Forms.ComboBox comboBoxDbList;
        internal System.Windows.Forms.ComboBox comboBoxDtList;
        internal System.Windows.Forms.Button button1;
        internal System.Windows.Forms.StatusStrip statusStrip1;
        internal System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        internal System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        internal System.Windows.Forms.TextBox textBoxUId;
        internal System.Windows.Forms.TextBox textBoxPwd;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.Label label6;
        internal System.Windows.Forms.CheckBox checkBoxIntegratedSecurity;
        internal System.Windows.Forms.Button btnCheckConnection;
        internal System.Windows.Forms.TextBox textBoxServer;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.ToolStripMenuItem clearLogToolStripMenuItem;
        internal System.Windows.Forms.Button btnDisconnect;
        internal System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.ToolStripMenuItem saveLogToolStripMenuItem;
        private System.Windows.Forms.Button btnPauseExport;
        private System.Windows.Forms.Button btnStopExport;
    }
}

