using System;
using System.Drawing;
using System.Windows.Forms;

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
                ModDialog.Size = DPI.Scale(new Size(384, 534));
                ScaleControls(ModDialog);
            }
            else
            {
                MainDialog = new Form_MainForm();
                MainDialog.Text = "LADXHD Mod Maker v" + Config.Version;
                MainDialog.Size = DPI.Scale(new Size(384, 682));
                ScaleControls(MainDialog);
            }
            OkayDialog  = new Form_OkayForm();
            YesNoDialog = new Form_YesNoForm();

            // The size of forms needs to be manually scaled.
            OkayDialog.Size = DPI.Scale(new Size(320, 135));
            YesNoDialog.Size = DPI.Scale(new Size(320, 135));

            // Scale the controls through a loop.
            ScaleControls(OkayDialog);
            ScaleControls(YesNoDialog);
        }

        static void ScaleControls(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                ctrl.Size = DPI.Scale(ctrl.Size);

                if (ctrl.GetType() == typeof(TextBox))
                    ctrl.Location = DPI.Scale(ctrl.Location, AddY:1);
                else
                    ctrl.Location = DPI.Scale(ctrl.Location);

                if (ctrl.HasChildren)
                    ScaleControls(ctrl);
            }
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