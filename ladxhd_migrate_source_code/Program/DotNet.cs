using System;
using System.IO;
using System.Diagnostics;
using static LADXHD_Migrater.Config;

namespace LADXHD_Migrater
{
    internal class DotNet
    {
        public static bool BuildGame()
        {
            // If the game path is invalid just cancel.
            if (!Config.Game_Source.TestPath()) return false;

            string arguments = "";

            // Build the game for the correct platform and graphics API.
            if (Config.SelectedPlatform == Platform.Windows)
            {
                if (Config.SelectedGraphics == GraphicsAPI.DirectX)
                {
                    Config.Build_Path = Path.Combine(Config.Publish_Path, "Windows-DX");
                    arguments = "publish ProjectZ.Desktop\\ProjectZ.Desktop.csproj -c Release -f net8.0-windows -r win-x64 -p:PublishProfile=FolderProfile_DX";
                }
                else if (Config.SelectedGraphics == GraphicsAPI.OpenGL)
                {
                    Config.Build_Path = Path.Combine(Config.Publish_Path, "Windows-GL");
                    arguments = "publish ProjectZ.Desktop\\ProjectZ.Desktop.csproj -c Release -f net8.0 -r win-x64 -p:PublishProfile=FolderProfile_GL";
                }
            }
            else if (Config.SelectedPlatform == Platform.Android)
            {
                Config.Build_Path = Path.Combine(Config.Publish_Path, "Android");
                arguments = "publish ProjectZ.Android\\ProjectZ.Android.csproj -c Release -f net8.0-android -p:PublishProfile=FolderProfile_Android";
            }

            try
            {
                using (Process dotnet = new Process())
                {
                    dotnet.StartInfo = new ProcessStartInfo
                    {
                        WorkingDirectory = Config.Game_Source,
                        FileName = "dotnet",
                        Arguments = arguments,
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
                        Forms.OkayDialog.Display("Build Error", 250, 40, 27, 9, 15, error);
                    }
                }
            }

            // If something didn't work out.
            catch (Exception ex)
            {
                Forms.OkayDialog.Display("Exception Caught", 250, 40, 27, 9, 15, "Exception: " + ex.Message);
            }

            // Return whether or not it actually succeeded.
            if (Config.SelectedPlatform == Platform.Windows)
            {
                string exePath = Path.Combine(Config.Build_Path, "Link's Awakening DX HD.exe");
                return exePath.TestPath();
            }
            if (Config.SelectedPlatform == Platform.Android)
            {
                string apkPath = Path.Combine(Config.Build_Path, "com.zelda.ladxhd-Signed.apk");
                return apkPath.TestPath();
            }
            return false;
        }
    }
}
