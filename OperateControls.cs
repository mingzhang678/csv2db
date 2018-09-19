using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVtoDatabase
{

    public class OperateControls
    {
        private static MainForm mainForm = Program.GetMainForm();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        internal delegate void setStatusLabelTextDelegate(string text);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        internal delegate void setProgressBarValueDelegate(int value);

        internal delegate void appendTextBoxTextDelegate(string text);

        internal delegate void setProgressBarMaxDelegate(int value);
        internal static void SetStatusLabelText(string text)
        {
            mainForm.toolStripStatusLabel1.Text = text;
            return;
        }

        internal static void SetProgressBar(int value)
        {
            mainForm.toolStripProgressBar1.Value = value;
            return;
        }

        internal static void SetProgressBarMax(int value)
        {
            mainForm.toolStripProgressBar1.Maximum = value;
        }
        internal static void AppendTextBoxText(string text)
        {
            mainForm.textBoxLog.AppendText(text);
        }
    }
}
