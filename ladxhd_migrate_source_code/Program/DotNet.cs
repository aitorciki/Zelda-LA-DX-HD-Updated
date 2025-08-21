using System;
using System.Diagnostics;

namespace LADXHD_Migrater
{
    internal class DotNet
    {
        public static bool BuildGame()
        {
            try
            {
                using (Process dotnet = new Process())
                {
                    dotnet.StartInfo = new ProcessStartInfo
                    {
                        WorkingDirectory = Config.game_source,
                        FileName = "dotnet",
                        Arguments = "publish -c Release -p:\"PublishProfile=FolderProfile\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };

                    dotnet.Start();

                    string output = dotnet.StandardOutput.ReadToEnd();
                    string error = dotnet.StandardError.ReadToEnd();

                    dotnet.WaitForExit();

                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        Forms.okayDialog.Display("Build Error", error, 250, 40, 27, 9, 15);
                    }
                }
            }
            catch (Exception ex)
            {
                Forms.okayDialog.Display("Exception Caught", "Exception: " + ex.Message, 250, 40, 27, 9, 15);
            }
            string GameExePath = Config.publish_Path + "\\Link's Awakening DX HD.exe";
            return GameExePath.TestPath();
        }
    }
}
