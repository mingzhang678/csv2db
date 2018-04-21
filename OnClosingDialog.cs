using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSVtoDatabase
{
    public partial class OnClosingDialog : Form
    {
        public OnClosingDialog()
        {
            InitializeComponent();
        }

        public OnClosingDialog(string title, string message)
        {
            InitializeComponent();
            this.Text = title;
            this.label1.Text = message;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(Program.getMainForm().UserFolder);
            this.DialogResult = DialogResult.OK;
            //Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            //this.DialogResult = DialogResult.Retry;
        }
        
    }
}
