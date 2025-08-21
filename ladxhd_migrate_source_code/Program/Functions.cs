using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LADXHD_Migrater
{
    internal class Functions
    {
        public static string appPath;
        public static string baseFolder;

        public static string orig_Content;
        public static string orig_Data;
        public static string update_Content;
        public static string update_Data;

        public static string xDeltaExe;
        public static string patches;

        public static Form_MainForm mainDialog;
        public static Form_OkayForm okayDialog; 

        public static void Initialize()
        {
            appPath        = Assembly.GetExecutingAssembly().Location;
            baseFolder     = Path.GetDirectoryName(appPath);
            xDeltaExe      = baseFolder + "\\ladxhd_game_source_code\\xdelta3.exe";
            patches        = baseFolder + "\\assets_patches";
            orig_Content   = baseFolder + "\\assets_original\\Content";
            orig_Data      = baseFolder + "\\assets_original\\Data";
            update_Content = baseFolder + "\\ladxhd_game_source_code\\Content";
            update_Data    = baseFolder + "\\ladxhd_game_source_code\\Data";

            mainDialog = new Form_MainForm();
            okayDialog = new Form_OkayForm();
        }

        public static bool XDeltaCheck()
        {
            if (!xDeltaExe.TestPath())
            {
                string Title = "XDelta3 Not Found";
                string Message = "This program requires \"xdelta3.exe\" to be present in the \"ladxhd_game_source_code\" path.";
                okayDialog.Display(Title, Message, 240, 40, 30, 16, 15);
                return false;
            }
            return true;
        }

        public static string GetXDeltaCreatePatchArguments(string OldFile, string NewFile, string PatchFile)
        {
		    string args = string.Empty;
		    args = string.Concat(new string[]
		    {
			    args,
			    " -f -s \"",
			    OldFile,
			    "\" \"",
			    NewFile,
			    "\" \"",
			    PatchFile,
			    "\""
		    });
            return args;
        }
        public static string GetXDeltaApplyPatchArguments(string Input, string PatchFile, string Output)
        {
		    string args = string.Empty;
		    args = string.Concat(new string[]
		    {
			    args,
			    " -d -f -s \"",
			    Input,
			    "\" \"",
			    PatchFile,
			    "\" \"",
			    Output,
			    "\""
		    });
            return args;
        }
        public static void RunXDelta(string arguments)
        {
            Process xDelta = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo 
            {
                WorkingDirectory = baseFolder,
                FileName = xDeltaExe,
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Normal
            };
            xDelta.StartInfo = startInfo;
            xDelta.Start();
            xDelta.WaitForExit();
        }

        public static void PatchCopyLoop(string orig, string update)
        {
            foreach (string file in orig.GetFiles("*", true))
            {
                FileItem fileItem = new FileItem(file);

                string patchFile = patches + "\\" + fileItem.Name + ".xdelta";
                string destPath  = update + fileItem.DirectoryName.Replace(orig, ""); 
                string destFile  = destPath + "\\" + fileItem.Name; 

                destPath.CreatePath(true);

                string arguments = GetXDeltaApplyPatchArguments(fileItem.FullName, patchFile, destFile);

                if (patchFile.TestPath())
                    RunXDelta(arguments);
                else
                    File.Copy(fileItem.FullName, destFile, true);
            }
        }
        public static void MigrateFiles()
        {
            mainDialog.ToggleDialog(false);

            PatchCopyLoop(orig_Content, update_Content);
            PatchCopyLoop(orig_Data, update_Data);

            string Title = "Finished Migration";
            string Message = "Updated Content/Data files to latest versions.";
            okayDialog.Display(Title, Message, 280, 40, 45, 26, 15);

            mainDialog.ToggleDialog(true);
        }

        public static string CalculateHash(string FilePath, string HashType)
        {
            HashAlgorithm Algorithm = HashAlgorithm.Create(HashType);
            byte[] ByteArray = File.ReadAllBytes(FilePath);
            return BitConverter.ToString(Algorithm.ComputeHash(ByteArray)).Replace("-", "");
        }
        public static void PatchCreateLoop(string orig, string update)
        {
            foreach (string file in update.GetFiles("*", true))
            {
                FileItem fileItem = new FileItem(file);
                string oldFile = orig + fileItem.DirectoryName.Replace(update, "") + "\\" + fileItem.Name;
                string oldHash = CalculateHash(oldFile, "MD5");
                string newHash = CalculateHash(fileItem.FullName, "MD5");
                string patchName = patches + "\\" + fileItem.Name + ".xdelta";
                string arguments = GetXDeltaCreatePatchArguments(oldFile, fileItem.FullName, patchName);

                patches.CreatePath(true);

                if (oldHash != newHash & fileItem.Extension != ".mgcontent" & fileItem.Extension != ".mgstats")
                    RunXDelta(arguments);
            }
        }
        public static void CreatePatches()
        {
            mainDialog.ToggleDialog(false);

            PatchCreateLoop(orig_Content, update_Content);
            PatchCreateLoop(orig_Data, update_Data);

            string Title = "Patches Created";
            string Message = "Finished creating xdelta patches from modified files. If any files were intentionally modifed, these can be shared as a new PR for the GitHub repository.";
            okayDialog.Display(Title, Message, 250, 40, 27, 9, 15);

            mainDialog.ToggleDialog(true);
        }
    }
}
