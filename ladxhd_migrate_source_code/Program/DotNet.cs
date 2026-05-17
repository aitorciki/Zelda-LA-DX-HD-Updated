using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using static LADXHD_Migrater.Config;

namespace LADXHD_Migrater
{
    internal class DotNet
    {
        public async static Task<bool> BuildGame()
        {
            if (!Config.Game_Source.TestPath()) return false;

            try
            {
                (string? buildFolder, string? csproj, string? framework, string? rid, string? profile, string? extra, string? exe) =
                    (Config.SelectedPlatform, Config.SelectedGraphics) switch
                    {
                        (Platform.Windows, GraphicsAPI.DirectX) => ("Windows-DX", "ProjectZ.Desktop/ProjectZ.Desktop.csproj", "net8.0-windows", "win-x64", "FolderProfile_DX", "-p:EnableWindowsTargeting=true", "Link's Awakening DX HD.exe"),
                        (Platform.Windows, GraphicsAPI.OpenGL) => ("Windows-GL", "ProjectZ.Desktop/ProjectZ.Desktop.csproj", "net8.0", "win-x64", "FolderProfile_GL", "-p:EnableWindowsTargeting=true", "Link's Awakening DX HD.exe"),
                        (Platform.Android, _) => ("Android", "ProjectZ.Android/ProjectZ.Android.csproj", "net8.0-android", null, "FolderProfile_Android", null, "com.zelda.ladxhd-Signed.apk"),
                        (Platform.Linux_x64, _) => ("Linux-x86_64","ProjectZ.Linux/ProjectZ.Linux.csproj", "net8.0", "linux-x64", "FolderProfile_Linux", null, "Link's Awakening DX HD"),
                        (Platform.Linux_Arm64, _) => ("Linux-Arm64", "ProjectZ.Linux/ProjectZ.Linux.csproj", "net8.0", "linux-arm64", "FolderProfile_Linux_Arm", null, "Link's Awakening DX HD"),
                        (Platform.MacOS_Arm64, _) => ("MacOS-Arm64", "ProjectZ.MacOS/ProjectZ.MacOS.csproj", "net8.0", "osx-arm64", "FolderProfile_MacOS", null, "Link's Awakening DX HD"),
                        (Platform.MacOS_x64, _) => ("MacOS-x86_64", "ProjectZ.MacOS/ProjectZ.MacOS.csproj", "net8.0", "osx-x64", "FolderProfile_MacOS_x64", null, "Link's Awakening DX HD"),
                        _ => (null, null, null, null, null, null, null)
                    };

                if (buildFolder == null) return false;

                Config.Build_Path = Path.Combine(Config.Publish_Path, buildFolder);

                string args = $"publish {csproj} -c Release -f {framework}";
                if (!string.IsNullOrEmpty(rid)) args += $" -r {rid}";
                if (!string.IsNullOrEmpty(extra)) args += $" {extra}";
                args += $" -p:PublishProfile={profile}";

                if (!await RunProcess("dotnet", args, "Build Error")) return false;

                string exePath = Path.Combine(Config.Build_Path, exe!);
                return exePath.TestPath();
            }
            catch (Exception ex)
            {
                await Functions.Notify("Exception Caught", "Exception: " + ex.Message, 10);
                return false;
            }
        }

        public async static Task<bool> BuildLauncher()
        {
            if (!Config.Launcher_Source.TestPath()) return false;

            (string rid,string profile) = Config.SelectedPlatform switch
            {
                Platform.Windows     => ("win-x64","Windows"),
                Platform.Linux_x64   => ("linux-x64","Linux-x64"),
                Platform.Linux_Arm64 => ("linux-arm64","Linux-arm64"),
                Platform.MacOS_x64   => ("osx-x64", "macOS-x64"),
                Platform.MacOS_Arm64 => ("osx-arm64","macOS-arm64"),
                _                    => ("","")
            };

            if (string.IsNullOrEmpty(rid) || string.IsNullOrEmpty(profile))
                return true;

            string args = $"publish LADXHD_Launcher.csproj -r {rid} -p:PublishProfile={profile}";
            return await RunProcess("dotnet", args, "Launcher Build Error", Config.Launcher_Source);
        }

        private async static Task<bool> RunProcess(string executable, string arguments, string errorTitle, string workingDirectory = "")
        {
            using (Process process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = string.IsNullOrEmpty(workingDirectory) ? Config.Game_Source : workingDirectory,
                    FileName = executable,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = !Functions.HeadlessMode,
                    RedirectStandardError = !Functions.HeadlessMode,
                    CreateNoWindow = true
                };

                process.Start();

                if (!Functions.HeadlessMode)
                {
                    string output = await process.StandardOutput.ReadToEndAsync();
                    string error = await process.StandardError.ReadToEndAsync();
                    await process.WaitForExitAsync();

                    if (process.ExitCode != 0)
                    {
                        string message = string.IsNullOrWhiteSpace(error) ? output : error;
                        if (message.Length > 500)
                            message = message.Substring(message.Length - 500);
                        await Functions.Notify(errorTitle, message, 10);
                        return false;
                    }
                }
                else
                {
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                        return false;
                }
            }
            return true;
        }
    }
}