using System;

namespace LADXHD_ModMaker
{
    internal class Forms
    {
        public static Form_MainForm  MainDialog;
        public static Form_ModForm   ModDialog;
        public static Form_OkayForm  OkayDialog; 
        public static Form_YesNoForm YesNoDialog; 

        public static void Initialize()
        {
            if (Config.PatchMode)
            {
                ModDialog = new Form_ModForm();
                ModDialog.Text = Config.ModName;
                ModDialog.SetInformation();
            }
            else
            {
                MainDialog = new Form_MainForm();
                MainDialog.Text = "LADXHD Mod Maker v" + Config.Version;
            }
            OkayDialog  = new Form_OkayForm();
            YesNoDialog = new Form_YesNoForm();
        }

        public static string CreateFolderSelectDialog(string inputPath)
        {
            // Create a new openfolder dialog.
            FolderSelectDialog folderDialog = new FolderSelectDialog();
            folderDialog.InitialDirectory = inputPath;
            folderDialog.Title = "";
            folderDialog.Show();

            // Store the file that was returned.
            string recievedFolder = folderDialog.FileName;

            // Make sure the folder has been set.
            if (recievedFolder != "")
                return recievedFolder;

            // Default to empty text.
            return "";
        }
    }
}