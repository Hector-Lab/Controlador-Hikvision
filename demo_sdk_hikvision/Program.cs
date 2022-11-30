using System;
using System.Windows.Forms;

namespace demo_sdk_hikvision
{
    static class Program
    {
        /// <summary>
        /// DEVELOPED BY PHURSKA
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Control());
            //Application.Run(new Form1());
        }
    }
}
