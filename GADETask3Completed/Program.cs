using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GADETask1
{
    static class Program
    {
       
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static Form1 UI = new Form1();
        [STAThread]

        static void Main()
        {
            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(UI);
        }
    }
}
