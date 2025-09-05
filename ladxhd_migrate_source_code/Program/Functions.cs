using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace LADXHD_Migrater
{
    internal class Functions
    {
        private static string[] languageFiles  = new[] { "esp", "fre", "ita", "por", "rus" };
        private static string[] languageDialog = new[] { "dialog_esp", "dialog_fre", "dialog_ita", "dialog_por", "dialog_rus" };

        public static string CalculateHash(string FilePath, string HashType)
        {
            HashAlgorithm Algorithm = HashAlgorithm.Create(HashType);
            byte[] ByteArray = File.ReadAllBytes(FilePath);
            return BitConverter.ToString(Algorithm.ComputeHash(ByteArray)).Replace("-", "");
        }

        public static bool VerifyMigrate()
        {
            if (!Config.orig_Content.TestPath() || !Config.orig_Data.TestPath())
            {
                Forms.okayDialog.Display("Error: Assets Missing", 250, 40, 26, 16, 15,
                    "Either the original \"Content\" folder, \"Data\" folder, or both are missing from the \"assets_original\" folder.");
                return false;
            }
            bool verify = Forms.yesNoDialog.Display("Confirm Migration", 250, 40, 31, 16, true, 
                "Are you sure you wish to migrate assets? This will apply current patches and overwrite your assets!");
            return verify;
        }

        public static void LanguagePatches(string input, string orig, string update)
        {
            FileItem fileItem = new FileItem(input);

            string[] target = null;
            if (fileItem.Name == "eng.lng")
                target = languageFiles;
            else if (fileItem.Name == "dialog_eng.lng")
                target = languageDialog;

            if (target == null) return;

            foreach (string lang in target)
            {
                string patchFile = Config.patches + "\\" + lang + ".lng.xdelta";
                string destFile = update + fileItem.DirectoryName.Replace(orig, "") + "\\" + lang + ".lng";

                if (!patchFile.TestPath()) continue;

                XDelta3.Args = XDelta3.GetApplyArguments(fileItem.FullName, patchFile, destFile);
                XDelta3.Start();
            }
        }

        public static void MigrateCopyLoop(string orig, string update)
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

                if (fileItem.Name == "eng.lng" || fileItem.Name == "dialog_eng.lng")
                    LanguagePatches(fileItem.FullName, orig, update);
            }
        }

        public static void MigrateFiles()
        {
            if (!VerifyMigrate()) return;
            Forms.mainDialog.ToggleDialog(false);

            MigrateCopyLoop(Config.orig_Content, Config.update_Content);
            MigrateCopyLoop(Config.orig_Data, Config.update_Data);

            Forms.okayDialog.Display("Finished Migration", 280, 40, 45, 26, 15, 
                "Updated Content/Data files to latest versions.");
            Forms.mainDialog.ToggleDialog(true);
        }

        public static bool VerifyCreatePatch()
        {
            if (!Config.orig_Content.TestPath() || !Config.orig_Data.TestPath())
            {
                Forms.okayDialog.Display("Error: Assets Missing", 250, 40, 26, 16, 15,
                    "Either the original \"Content\" folder, \"Data\" folder, or both are missing from the \"assets_original\" folder.");
                return false;
            }
            if (!Config.update_Content.TestPath() || !Config.update_Data.TestPath())
            {
                Forms.okayDialog.Display("Assets Missing", 250, 40, 34, 16, 15,
                    "Either the \"Content\" folder, \"Data\" folder, or both are missing from \"ladxhd_game_source_code\".");
                return false;
            }
            bool verify = Forms.yesNoDialog.Display("Confirm Create Patches", 250, 40, 31, 16, true, 
                "Are you sure you wish to create patches? This will overwrite all current patches with recent changes!");
            return verify;
        }

        private static string GetSpecialCases(FileItem fileItem, string orig, string update)
        {
            if (fileItem.Name == "menuBackgroundAlt.png")
                return orig + fileItem.DirectoryName.Replace(update, "") + "\\menuBackground.png";
            if (fileItem.Name == "items_redux.png")
                return orig + fileItem.DirectoryName.Replace(update, "") + "\\items.png";
            if (fileItem.Name == "npcs_redux.png")
                return orig + fileItem.DirectoryName.Replace(update, "") + "\\npcs.png";
            if (fileItem.Name == "smallFont_redux.png")
                return orig + fileItem.DirectoryName.Replace(update, "") + "\\smallFont.png";
            if (fileItem.Name == "smallFont_vwf.png")
                return orig + fileItem.DirectoryName.Replace(update, "") + "\\smallFont.png";
            if (fileItem.Name == "smallFont_vwf_redux.png")
                return orig + fileItem.DirectoryName.Replace(update, "") + "\\smallFont.png";
            return "";
        }

        public static void CreatePatchLoop(string orig, string update)
        {
            // Create the folder if it does not exist.
            Config.patches.CreatePath(true);

            foreach (string file in update.GetFiles("*", true))
            {
                FileItem fileItem = new FileItem(file);
                string oldFile = "";

                // Do not get files in the "obj" or "bin" folders. Ignore 
                if (fileItem.DirectoryName.IndexOf("content\\bin", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    fileItem.DirectoryName.IndexOf("content\\obj", StringComparison.OrdinalIgnoreCase) >= 0)
                    continue;

                // Hack to derive non-english langues files from english language files.
                if (languageFiles.Contains(fileItem.BaseName))
                    oldFile = orig + fileItem.DirectoryName.Replace(update, "") + "\\eng.lng";
                else if (languageDialog.Contains(fileItem.BaseName))
                    oldFile = orig + fileItem.DirectoryName.Replace(update, "") + "\\dialog_eng.lng";
                else
                    oldFile = orig + fileItem.DirectoryName.Replace(update, "") + "\\" + fileItem.Name;

                // Special cases are files that don't have an original but are derived from other files.
                if (!oldFile.TestPath()) 
                    oldFile = GetSpecialCases(fileItem, orig, update);

                if (oldFile == "") continue;
 
                // Get the file hashes.
                string oldHash = CalculateHash(oldFile, "MD5");
                string newHash = CalculateHash(fileItem.FullName, "MD5");

                // Set up the patch name and run the patcher.
                string patchName = Config.patches + "\\" + fileItem.Name + ".xdelta";
                XDelta3.Args = XDelta3.GetCreateArguments(oldFile, fileItem.FullName, patchName);
                if (oldHash != newHash) XDelta3.Start();
            }
        }

        public static void CreatePatches()
        {
            if (!VerifyCreatePatch()) return;
            Forms.mainDialog.ToggleDialog(false);

            CreatePatchLoop(Config.orig_Content, Config.update_Content);
            CreatePatchLoop(Config.orig_Data, Config.update_Data);

            Forms.okayDialog.Display("Patches Created", 250, 40, 27, 9, 15,
                "Finished creating xdelta patches from modified files. If any files were intentionally modifed, these can be shared as a new PR for the GitHub repository.");
            Forms.mainDialog.ToggleDialog(true);
        }

        public static bool VerifyCleanFiles()
        {
            bool verify = Forms.yesNoDialog.Display("Clean Build Files", 250, 40, 29, 9, true, 
                "Are you sure you wish to clean build files? This will remove all instances of \'obj\', \'bin\', \'Publish\', and \'zelda_ladxhd_build\' folders if they currently exist.");
            return verify;
        }

        public static void CleanBuildFiles()
        {
            if (!VerifyCleanFiles()) return;
            Forms.mainDialog.ToggleDialog(false);

            (Config.game_source + "\\bin").RemovePath();
            (Config.game_source + "\\obj").RemovePath();
            (Config.game_source + "\\Content\\bin").RemovePath();
            (Config.game_source + "\\Content\\obj").RemovePath();
            (Config.game_source + "\\Publish").RemovePath();
            (Config.migrate_source + "\\bin").RemovePath();
            (Config.migrate_source + "\\obj").RemovePath();
            (Config.patcher_source + "\\bin").RemovePath();
            (Config.patcher_source + "\\obj").RemovePath();
            (Config.baseFolder + "\\zelda_ladxhd_build").RemovePath();

            Forms.okayDialog.Display("Finished", 260, 40, 26, 26, 15,
                "Finished cleaning build files (obj/bin/Publish folders).");
            Forms.mainDialog.ToggleDialog(true);
        }

        public static void CreateBuild()
        {
            Forms.mainDialog.ToggleDialog(false);

            if (DotNet.BuildGame())
            {
                string MoveDestination = Config.baseFolder + "\\zelda_ladxhd_build";
                Config.publish_Path.MovePath(MoveDestination, true);

                Forms.okayDialog.Display("Finished", 250, 40, 28, 16, 15,
                    "Finished build process. If the build was successful, it can be found in the \"zelda_ladxhd_build\" folder.");
            }
            Forms.mainDialog.ToggleDialog(true);
        }
    }
}
