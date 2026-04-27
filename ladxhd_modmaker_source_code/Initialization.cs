using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LADXHD_ModMaker
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
            Config.Initialize();
            Forms.Initialize();

            // Show the appropriate form.
            if (Config.PatchMode)
                Forms.ModDialog.ShowDialog();
            else
                Forms.MainDialog.ShowDialog();
        }
    }
}
