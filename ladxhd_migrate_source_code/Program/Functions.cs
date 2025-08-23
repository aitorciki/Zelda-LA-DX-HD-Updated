using System;
using System.IO;
using System.Security.Cryptography;

namespace LADXHD_Migrater
{
    internal class Functions
    {
        public static string CalculateHash(string FilePath, string HashType)
        {
            HashAlgorithm Algorithm = HashAlgorithm.Create(HashType);
            byte[] ByteArray = File.ReadAllBytes(FilePath);
            return BitConverter.ToString(Algorithm.ComputeHash(ByteArray)).Replace("-", "");
        }

        public static void PatchCopyLoop(string orig, string update)
        {
            foreach (string file in orig.GetFiles("*", true))
            {
                FileItem fileItem = new FileItem(file);

                string patchFile = Config.patches + "\\" + fileItem.Name + ".xdelta";
                string destPath  = update + fileItem.DirectoryName.Replace(orig, ""); 
                string destFile  = destPath + "\\" + fileItem.Name; 

                destPath.CreatePath(true);

                XDelta3.Args = XDelta3.GetApplyArguments(fileItem.FullName, patchFile, destFile);

                if (patchFile.TestPath())
                    XDelta3.Start();
                else
                    File.Copy(fileItem.FullName, destFile, true);
            }
        }
        public static void MigrateFiles()
        {
            Forms.mainDialog.ToggleDialog(false);

            PatchCopyLoop(Config.orig_Content, Config.update_Content);
            PatchCopyLoop(Config.orig_Data, Config.update_Data);

            string Title = "Finished Migration";
            string Message = "Updated Content/Data files to latest versions.";
            Forms.okayDialog.Display(Title, Message, 280, 40, 45, 26, 15);

            Forms.mainDialog.ToggleDialog(true);
        }

        public static void PatchCreateLoop(string orig, string update)
        {
            foreach (string file in update.GetFiles("*", true))
            {
                FileItem fileItem = new FileItem(file);

                string oldFile = orig + fileItem.DirectoryName.Replace(update, "") + "\\" + fileItem.Name;
                string patchName = Config.patches + "\\" + fileItem.Name + ".xdelta";
                XDelta3.Args = XDelta3.GetCreateArguments(oldFile, fileItem.FullName, patchName);

                string oldHash = CalculateHash(oldFile, "MD5");
                string newHash = CalculateHash(fileItem.FullName, "MD5");

                Config.patches.CreatePath(true);

                if (oldHash != newHash & fileItem.Extension != ".mgcontent" & fileItem.Extension != ".mgstats")
                    XDelta3.Start();
            }
        }
        public static void CreatePatches()
        {
            Forms.mainDialog.ToggleDialog(false);

            PatchCreateLoop(Config.orig_Content, Config.update_Content);
            PatchCreateLoop(Config.orig_Data, Config.update_Data);

            string Title = "Patches Created";
            string Message = "Finished creating xdelta patches from modified files. If any files were intentionally modifed, these can be shared as a new PR for the GitHub repository.";
            Forms.okayDialog.Display(Title, Message, 250, 40, 27, 9, 15);

            Forms.mainDialog.ToggleDialog(true);
        }

        public static void CleanBuildFiles()
        {
            Forms.mainDialog.ToggleDialog(false);

            (Config.game_source + "\\bin").RemovePath();
            (Config.game_source + "\\obj").RemovePath();
            (Config.game_source + "\\Content\\bin").RemovePath();
            (Config.game_source + "\\Content\\obj").RemovePath();
            (Config.game_source + "\\Publish").RemovePath();

            string Title = "Finished";
            string Message = "Finished cleaning build files (obj/bin/Publish folders).";
            Forms.okayDialog.Display(Title, Message, 260, 40, 26, 26, 15);

            Forms.mainDialog.ToggleDialog(true);
        }

        public static void CreateBuild()
        {
            Forms.mainDialog.ToggleDialog(false);

            if (DotNet.BuildGame())
            {
                string MoveDestination = Config.baseFolder + "\\zelda_ladxhd_build";
                Config.publish_Path.MovePath(MoveDestination, true);

                string Title = "Finished";
                string Message = "Finished build process. If the build was successful, it can be found in the \"zelda_ladxhd_build\" folder.";
                Forms.okayDialog.Display(Title, Message, 250, 40, 28, 16, 15);
            }
            Forms.mainDialog.ToggleDialog(true);
        }
    }
}
