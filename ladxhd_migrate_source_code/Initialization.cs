using System;
using System.Drawing;
using System.IO;
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
            try
            {
                Application.SetDefaultFont(new Font(new FontFamily("Microsoft Sans Serif"), 8.25f));
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Initialize DPI scaling.
                SetProcessDPIAware();
                DPI.Init();

                // Initialize the classes.
                Forms.Initialize();
                Config.Initialize();

                // Show the dialog.
                Forms.MainDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                // If an error happens, try to show a message box and create a crash log.
                MessageBox.Show(ex.ToString(), "Startup Crash", MessageBoxButtons.OK, MessageBoxIcon.Error);
                File.WriteAllText("crash.log", ex.ToString());
            }
        }
    }
}
