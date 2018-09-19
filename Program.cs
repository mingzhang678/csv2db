using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSVtoDatabase
{
    static class Program
    {
        private static MainForm form1;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form1 = new MainForm();
            Application.Run(form1);
        }
        public static MainForm GetMainForm()
        {
            return form1;
        }
    }
}
