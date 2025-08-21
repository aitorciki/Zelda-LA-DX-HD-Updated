using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using ClairObscurConfig;

namespace LADXHD_Patcher
{
    internal class Functions
    {
        private const string oldHash = "F4ADFBA864B852908705EA6A18A48F18";
        private const string newHash = "0FF64AD6F66A828719E0295E8A2D8354";

        public static string appPath;
        public static string baseFolder;
        public static string workFolder;
        public static string xDeltaExe;
        public static string zeldaEXE;

        public static void Initialize()
        {
            appPath = Assembly.GetExecutingAssembly().Location;
            baseFolder = Path.GetDirectoryName(appPath);
            workFolder = baseFolder + "\\~temp";
            xDeltaExe = workFolder + "\\xdelta3.exe";
            zeldaEXE = baseFolder + "\\Link's Awakening DX HD.exe";
        }

        public static string XDeltaBuildArguments(string Input, string PatchFile, string Output)
        {
            // Getting this string just right was a nightmare scenario. I ended up decompiling the XDelta GUI
            // on the Romhacking forums to get this because no matter what I tried, it just didn't want to work.
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

        public static Process XDeltaGetProcess(string Arguments)
        {
            Process p = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo 
            {
                WorkingDirectory = workFolder,
                FileName = xDeltaExe,
                Arguments = Arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Normal
            };
            p.StartInfo = startInfo;
            return p;
        }

        public static void XDeltaPatchFile(FileItem fileItem)
        {
            string patchDir = (workFolder + "\\patches").CreatePath();
            string resultDir = (workFolder + "\\patchedFiles").CreatePath();

            var resources = ResourceHelper.GetAllResources();
            byte[] patchRes = (byte[])resources[fileItem.Name];

            string patchPath = patchDir + "\\" + fileItem.Name + ".xdelta";
            string resultPath = resultDir + "\\" + fileItem.Name;

            if (!patchPath.TestPath())
                File.WriteAllBytes(patchPath, patchRes);

            string arguments = XDeltaBuildArguments(fileItem.FullName, patchPath, resultPath);

            Process xDelta = XDeltaGetProcess(arguments);
            xDelta.Start();
            xDelta.WaitForExit();

            resultPath.MovePath(fileItem.FullName, true);
        }

        public static string CalculateHash(string FilePath, string HashType)
        {
            HashAlgorithm Algorithm = HashAlgorithm.Create(HashType);
            byte[] ByteArray = File.ReadAllBytes(FilePath);
            return BitConverter.ToString(Algorithm.ComputeHash(ByteArray)).Replace("-", "");
        }

        public static void StartPatching()
        {
            Form_OkayForm OkayDialog = new Form_OkayForm();

            if (!zeldaEXE.TestPath())
            {
                string Title = "Game Executable Not Found";
                string Message = "Could not find \"Link's Awakening DX HD.exe\" to patch. Copy this patcher executable to the folder of the original release of v1.0.0 and run it from there.";
                OkayDialog.Display(Title, Message, 250, 40, 27, 10, 15);
                return;
            }

            string md5Hash = CalculateHash(zeldaEXE, "MD5");

            // We need not update the game as it has already been updated.
            if (md5Hash == newHash)
            {
                string Title = "Already Patched";
                string Message = "The game is already at v1.1.0 so no patching is needed. Close this patcher and launch the game!";
                OkayDialog.Display(Title, Message, 260, 40, 30, 16, 10);
                return;
            }

            // The game has not been updated so patch it.
            if (md5Hash == oldHash)
            {
                var resources = ResourceHelper.GetAllResources();
                byte[] xdeltaRes = (byte[])resources["xdelta3.exe"];

                workFolder.CreatePath(true);
                File.WriteAllBytes(xDeltaExe, xdeltaRes);

                foreach (string file in baseFolder.GetFiles("*", true))
                {
                    FileItem fileItem = new FileItem(file);

                    if (fileItem.Name == "xdelta3.exe")
                        continue;

                    if (resources.ContainsKey(fileItem.Name))
                    {
                        Console.WriteLine(fileItem.Name);
                        XDeltaPatchFile(fileItem);
                    }
                }
                string Title = "Patching Complete";
                string Message = "Patching Link's Awakening DX HD v1.0.0 was successful. The game was updated to v1.1.0.";
                OkayDialog.Display(Title, Message, 260, 40, 34, 16, 10);

                workFolder.RemovePath();
            }
        }
    }
}
