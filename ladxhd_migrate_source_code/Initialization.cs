using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LADXHD_Migrater
{
    internal static class Initialization
    {
        // Import to make the application DPI aware for high DPI displays.
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialize DPI scaling.
            SetProcessDPIAware();
            DPI.Init();

            // Initialize the classes.
            Forms.Initialize();
            Config.Initialize();
            XDelta3.Initialize();

            // Only run if "xdelta3.exe" is found.
            Forms.MainDialog.ShowDialog();
        }
    }
}
