using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using static LADXHD_Migrater.XDelta3;

namespace LADXHD_Migrater
{
    internal class Functions
    {
        // The top array is the name of all language files while the bottom is all dialog language files.
        private static string[] languageFiles  = new[] {        "esp.lng",        "fre.lng",        "ita.lng",        "por.lng",        "rus.lng" };
        private static string[] languageDialog = new[] { "dialog_esp.lng", "dialog_fre.lng", "dialog_ita.lng", "dialog_por.lng", "dialog_rus.lng" };

        // The top array holds files to generate from and the bottom holds the corresponding target file it creates.
        private static string[] specialFile   = new[] {    "menuBackground.png",       "npcs.png",       "items.png" };
        private static string[] specialTarget = new[] { "menuBackgroundAlt.png", "npcs_redux.png", "items_redux.png" };

        // This is all the fonts that the texture "smallFont" is the base of and all these are created from it.
        private static string[] smallFonts = new[] { "smallFont_redux.png", "smallFont_vwf.png", "smallFont_vwf_redux.png" };

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

        public static void HandleLanguagePatches(FileItem fileItem, string origPath, string updatePath)
        {
            var fileTargets = new Dictionary<string, string[]>
            {
                { "eng.lng", languageFiles },
                { "dialog_eng.lng", languageDialog }
            };
            if (!fileTargets.TryGetValue(fileItem.Name, out var target))
                return;

            foreach (string langFile in target)
            {
                string xdelta3File = Path.Combine(Config.patches, langFile + ".xdelta");
                string patchedFile = Path.Combine(updatePath + fileItem.DirectoryName.Replace(origPath, ""), langFile);
                XDelta3.Execute(Operation.Apply, fileItem.FullName, xdelta3File, patchedFile);
            }
        }

        private static void HandleSpecialUseCases(FileItem fileItem, string origPath, string updatePath)
        {
            if (specialFile.Contains(fileItem.Name))
            {
                int index = Array.IndexOf(specialFile, fileItem.Name);
                string specFile = specialTarget[index];

                string xdelta3File = Path.Combine(Config.patches, specFile + ".xdelta");
                string patchedFile = Path.Combine(updatePath + fileItem.DirectoryName.Replace(origPath, ""), specFile);
                XDelta3.Execute(Operation.Apply, fileItem.FullName, xdelta3File, patchedFile);
            }
        }

        private static void HandleSmallFontImages(FileItem fileItem, string origPath, string updatePath)
        {
            if (fileItem.Name != "smallFont.png")
                return;

            foreach (string sfontFile in smallFonts)
            {
                string xdelta3File = Path.Combine(Config.patches, sfontFile + ".xdelta");
                string patchedFile = Path.Combine(updatePath + fileItem.DirectoryName.Replace(origPath, ""), sfontFile);
                XDelta3.Execute(Operation.Apply, fileItem.FullName, xdelta3File, patchedFile);
            }
        }

        public static void MigrateCopyLoop(string origPath, string updatePath)
        {
            updatePath.RemovePath();

            foreach (string file in origPath.GetFiles("*", true))
            {
                FileItem fileItem = new FileItem(file);

                string xdelta3File = Path.Combine(Config.patches, fileItem.Name + ".xdelta");
                string patchedFile = Path.Combine((updatePath + fileItem.DirectoryName.Replace(origPath, "")).CreatePath(), fileItem.Name);

                if (xdelta3File.TestPath())
                    XDelta3.Execute(Operation.Apply, fileItem.FullName, xdelta3File, patchedFile);
                else
                    File.Copy(fileItem.FullName, patchedFile, true);

                HandleLanguagePatches(fileItem, origPath, updatePath);
                HandleSpecialUseCases(fileItem, origPath, updatePath);
                HandleSmallFontImages(fileItem, origPath, updatePath);
            }
        }

        public static void MigrateFiles()
        {
            if (!VerifyMigrate()) return;
            Forms.mainDialog.ToggleDialog(false);

            XDelta3.Create();
            MigrateCopyLoop(Config.orig_Content, Config.update_Content);
            MigrateCopyLoop(Config.orig_Data, Config.update_Data);
            XDelta3.Remove();

            Forms.okayDialog.Display("Finished Migration", 280, 40, 45, 26, 15, 
                "Updated Content/Data files to latest versions.");
            Forms.mainDialog.ToggleDialog(true);
        }

//------------------------------------------------------------------------------------------------------------------------------------------------

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

        private static string GetSpecialCases(FileItem fileItem, string origPath, string updatePath)
        {
            // Map special cases to their actual filenames
            var specialCases = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                {   "menuBackgroundAlt.png", "menuBackground.png" },
                {         "items_redux.png",          "items.png" },
                {          "npcs_redux.png",           "npcs.png" },
                {     "smallFont_redux.png",      "smallFont.png" },
                {       "smallFont_vwf.png",      "smallFont.png" },
                { "smallFont_vwf_redux.png",      "smallFont.png" }
            };
            if (specialCases.TryGetValue(fileItem.Name, out var targetName))
                return origPath + fileItem.DirectoryName.Replace(updatePath, "") + "\\" + targetName;

            return "";
        }

        public static void CreatePatchLoop(string origPath, string updatePath)
        {
            Config.patches.CreatePath(true);

            foreach (string file in updatePath.GetFiles("*", true))
            {
                FileItem fileItem = new FileItem(file);
                string oldFile = "";

                if (fileItem.DirectoryName.IndexOf("content\\bin", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    fileItem.DirectoryName.IndexOf("content\\obj", StringComparison.OrdinalIgnoreCase) >= 0)
                    continue;

                if (languageFiles.Contains(fileItem.Name)) 
                    oldFile = Path.Combine(origPath + fileItem.DirectoryName.Replace(updatePath, ""), "eng.lng");
                else if (languageDialog.Contains(fileItem.Name)) 
                    oldFile = Path.Combine(origPath + fileItem.DirectoryName.Replace(updatePath, ""), "dialog_eng.lng");
                else 
                    oldFile = Path.Combine(origPath + fileItem.DirectoryName.Replace(updatePath, ""), fileItem.Name);

                if (!oldFile.TestPath()) 
                    oldFile = GetSpecialCases(fileItem, origPath, updatePath);
                if (oldFile == "") continue;

                string oldHash = oldFile.CalculateHash("MD5");
                string newHash = fileItem.FullName.CalculateHash("MD5");

                if (oldHash != newHash)
                {
                    string patchName = Path.Combine(Config.patches, fileItem.Name + ".xdelta");
                    XDelta3.Execute(Operation.Create, oldFile, fileItem.FullName, patchName);
                }
            }
        }

        public static void CreatePatches()
        {
            if (!VerifyCreatePatch()) return;
            Forms.mainDialog.ToggleDialog(false);

            XDelta3.Create();
            CreatePatchLoop(Config.orig_Content, Config.update_Content);
            CreatePatchLoop(Config.orig_Data, Config.update_Data);
            XDelta3.Remove();

            Forms.okayDialog.Display("Patches Created", 250, 40, 27, 9, 15,
                "Finished creating xdelta patches from modified files. If any files were intentionally modifed, these can be shared as a new PR for the GitHub repository.");
            Forms.mainDialog.ToggleDialog(true);
        }

//------------------------------------------------------------------------------------------------------------------------------------------------

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

//------------------------------------------------------------------------------------------------------------------------------------------------

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
