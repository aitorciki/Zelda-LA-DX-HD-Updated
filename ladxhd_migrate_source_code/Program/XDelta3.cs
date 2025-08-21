using System.Diagnostics;

namespace LADXHD_Migrater
{
    internal class XDelta3
    {
        public static string Exe;
        public static string Args;

        public static void Initialize()
        {
            XDelta3.Exe = Config.baseFolder + "\\ladxhd_game_source_code\\xdelta3.exe";
        }

        public static bool Exists()
        {
            if (!XDelta3.Exe.TestPath())
            {
                string Title = "XDelta3 Not Found";
                string Message = "This program requires \"xdelta3.exe\" to be present in the \"ladxhd_game_source_code\" path.";
                Forms.okayDialog.Display(Title, Message, 240, 40, 30, 16, 15);
                return false;
            }
            return true;
        }

        public static string GetCreateArguments(string OldFile, string NewFile, string PatchFile)
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

        public static string GetApplyArguments(string OldFile, string PatchFile, string NewFile)
        {
		    string args = string.Empty;
		    args = string.Concat(new string[]
		    {
			    args,
			    " -d -f -s \"",
			    OldFile,
			    "\" \"",
			    PatchFile,
			    "\" \"",
			    NewFile,
			    "\""
		    });
            return args;
        }

        public static void Start()
        {
            Process xDelta = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo 
            {
                WorkingDirectory = Config.baseFolder,
                FileName = XDelta3.Exe,
                Arguments = XDelta3.Args,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Normal
            };
            xDelta.StartInfo = startInfo;
            xDelta.Start();
            xDelta.WaitForExit();
        }
    }
}
