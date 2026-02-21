using System;
using System.Windows.Forms;

namespace LADXHD_ModMaker
{
    internal static class Initialization
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

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
