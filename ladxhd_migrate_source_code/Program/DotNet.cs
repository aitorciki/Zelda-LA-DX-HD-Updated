using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static LADXHD_Migrater.Config;

namespace LADXHD_Migrater
{
    internal class DotNet
    {
        private static string WslPrefix(string wslPath) => $"export MGFXC_WINE_PATH={WinePath} && cd {wslPath} && dotnet";

        private static string ToWslPath(string windowsPath)
        {
            string full = Path.GetFullPath(windowsPath);
            string drive = full.Substring(0, 1).ToLower();
            string rest = full.Substring(2).Replace('\\', '/');
            return $"/mnt/{drive}{rest}";
        }
        private static string WinePath = GetWslWinePath();

        private static string GetWslWinePath()
        {
            // Try to get the path to Wine which includes the username in WSL.
            try
            {
                var p = Process.Start(new ProcessStartInfo("wsl", "echo $HOME")
                {
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
                });
                string? home = p?.StandardOutput.ReadLine()?.Trim();
                p?.WaitForExit();
                if (!string.IsNullOrEmpty(home))
                    return home + "/.wine-mgfxc";
            }
            catch { }

            // Fallback to lowercased Windows username if nothing is found.
            return "/home/" + Environment.UserName.ToLower() + "/.wine-mgfxc";
        }

        public async static Task<bool> BuildGame()
        {
            string restoreArgs;
            string publishArgs;

            if (!Config.Game_Source.TestPath()) return false;

            try
            {
                if (Config.SelectedPlatform == Platform.Windows)
                {
                    if (!await RunProcess("dotnet", "restore", false, "Restore Error")) return false;

                    if (Config.SelectedGraphics == GraphicsAPI.DirectX)
                    {
                        Config.Build_Path = Path.Combine(Config.Publish_Path, "Windows-DX");
                        restoreArgs = "build ProjectZ.Desktop\\ProjectZ.Desktop.csproj -c Release -f net8.0-windows -r win-x64 --no-restore";
                        publishArgs = "publish ProjectZ.Desktop\\ProjectZ.Desktop.csproj -c Release -f net8.0-windows -r win-x64 --no-restore -p:PublishProfile=FolderProfile_DX";
                        if (!await RunProcess("dotnet", restoreArgs, false, "Build Error")) return false;
                        if (!await RunProcess("dotnet", publishArgs, false, "Build Error")) return false;
                    }
                    else if (Config.SelectedGraphics == GraphicsAPI.OpenGL)
                    {
                        Config.Build_Path = Path.Combine(Config.Publish_Path, "Windows-GL");
                        restoreArgs = "build ProjectZ.Desktop\\ProjectZ.Desktop.csproj -c Release -f net8.0 -r win-x64 --no-restore";
                        publishArgs = "publish ProjectZ.Desktop\\ProjectZ.Desktop.csproj -c Release -f net8.0 -r win-x64 --no-restore -p:PublishProfile=FolderProfile_GL";
                        if (!await RunProcess("dotnet", restoreArgs, false, "Build Error")) return false;
                        if (!await RunProcess("dotnet", publishArgs, false, "Build Error")) return false;
                    }
                }
                else if (Config.SelectedPlatform == Platform.Android)
                {
                    Config.Build_Path = Path.Combine(Config.Publish_Path, "Android");
                    if (!await RunProcess("dotnet", "restore", false, "Restore Error")) return false;
                    publishArgs = "publish ProjectZ.Android\\ProjectZ.Android.csproj -c Release -f net8.0-android --no-restore -p:PublishProfile=FolderProfile_Android";
                    if (!await RunProcess("dotnet", publishArgs, false, "Build Error")) return false;
                }
                else if (Config.SelectedPlatform == Platform.Linux_x64)
                {
                    Config.Build_Path = Path.Combine(Config.Publish_Path, "Linux-x86_64");
                #if WINDOWS
                    string prefix = WslPrefix(ToWslPath(Config.Game_Source));
                    restoreArgs = $"bash -c \"{prefix} restore ProjectZ.Linux/ProjectZ.Linux.csproj\"";
                    publishArgs = $"bash -c \"{prefix} publish ProjectZ.Linux/ProjectZ.Linux.csproj -c Release -f net8.0 -r linux-x64 --no-restore -p:PublishProfile=FolderProfile_Linux\"";
                    if (!await RunProcess("wsl", restoreArgs, true, "Restore Error")) return false;
                    if (!await RunProcess("wsl", publishArgs, true, "Build Error")) return false;
                #else
                    restoreArgs = "restore ProjectZ.Linux/ProjectZ.Linux.csproj";
                    publishArgs = "publish ProjectZ.Linux/ProjectZ.Linux.csproj -c Release -f net8.0 -r linux-x64 --no-restore -p:PublishProfile=FolderProfile_Linux";
                    if (!await RunProcess("dotnet", restoreArgs, false, "Restore Error")) return false;
                    if (!await RunProcess("dotnet", publishArgs, false, "Build Error")) return false;
                #endif
                }
                else if (Config.SelectedPlatform == Platform.Linux_Arm64)
                {
                    Config.Build_Path = Path.Combine(Config.Publish_Path, "Linux-Arm64");
                #if WINDOWS
                    string prefix = WslPrefix(ToWslPath(Config.Game_Source));
                    restoreArgs = $"bash -c \"{prefix} restore ProjectZ.Linux/ProjectZ.Linux.csproj\"";
                    publishArgs = $"bash -c \"{prefix} publish ProjectZ.Linux/ProjectZ.Linux.csproj -c Release -f net8.0 -r linux-arm64 --no-restore -p:PublishProfile=FolderProfile_Linux_Arm\"";
                    if (!await RunProcess("wsl", restoreArgs, true, "Restore Error")) return false;
                    if (!await RunProcess("wsl", publishArgs, true, "Build Error")) return false;
                #else
                    restoreArgs = "restore ProjectZ.Linux/ProjectZ.Linux.csproj";
                    publishArgs = "publish ProjectZ.Linux/ProjectZ.Linux.csproj -c Release -f net8.0 -r linux-arm64 --no-restore -p:PublishProfile=FolderProfile_Linux_Arm";
                    if (!await RunProcess("dotnet", restoreArgs, false, "Restore Error")) return false;
                    if (!await RunProcess("dotnet", publishArgs, false, "Build Error")) return false;
                #endif
                }
                else if (Config.SelectedPlatform == Platform.MacOS_Arm64)
                {
                    Config.Build_Path = Path.Combine(Config.Publish_Path, "MacOS-Arm64");
                #if WINDOWS
                    string prefix = WslPrefix(ToWslPath(Config.Game_Source));
                    restoreArgs = $"bash -c \"{prefix} restore ProjectZ.MacOS/ProjectZ.MacOS.csproj\"";
                    publishArgs = $"bash -c \"{prefix} publish ProjectZ.MacOS/ProjectZ.MacOS.csproj -c Release -f net8.0 -r osx-arm64 --no-restore -p:PublishProfile=FolderProfile_MacOS\"";
                    if (!await RunProcess("wsl", restoreArgs, true, "Restore Error")) return false;
                    if (!await RunProcess("wsl", publishArgs, true, "Build Error")) return false;
                #else
                    restoreArgs = "restore ProjectZ.MacOS/ProjectZ.MacOS.csproj";
                    publishArgs = "publish ProjectZ.MacOS/ProjectZ.MacOS.csproj -c Release -f net8.0 -r osx-arm64 --no-restore -p:PublishProfile=FolderProfile_MacOS";
                    if (!await RunProcess("dotnet", restoreArgs, false, "Restore Error")) return false;
                    if (!await RunProcess("dotnet", publishArgs, false, "Build Error")) return false;
                #endif
                }
                else if (Config.SelectedPlatform == Platform.MacOS_x64)
                {
                    Config.Build_Path = Path.Combine(Config.Publish_Path, "MacOS-x86_64");
                #if WINDOWS
                    string prefix = WslPrefix(ToWslPath(Config.Game_Source));
                    restoreArgs = $"bash -c \"{prefix} restore ProjectZ.MacOS/ProjectZ.MacOS.csproj\"";
                    publishArgs = $"bash -c \"{prefix} publish ProjectZ.MacOS/ProjectZ.MacOS.csproj -c Release -f net8.0 -r osx-x64 --no-restore -p:PublishProfile=FolderProfile_MacOS_x64\"";
                    if (!await RunProcess("wsl", restoreArgs, true, "Restore Error")) return false;
                    if (!await RunProcess("wsl", publishArgs, true, "Build Error")) return false;
                #else
                    restoreArgs = "restore ProjectZ.MacOS/ProjectZ.MacOS.csproj";
                    publishArgs = "publish ProjectZ.MacOS/ProjectZ.MacOS.csproj -c Release -f net8.0 -r osx-x64 --no-restore -p:PublishProfile=FolderProfile_MacOS_x64";
                    if (!await RunProcess("dotnet", restoreArgs, false, "Restore Error")) return false;
                    if (!await RunProcess("dotnet", publishArgs, false, "Build Error")) return false;
                #endif
                }
            }
            catch (Exception ex)
            {
                await Functions.Notify("Exception Caught", "Exception: " + ex.Message, 10);
            }

            if (Config.SelectedPlatform == Platform.Windows)
            {
                string exePath = Path.Combine(Config.Build_Path, "Link's Awakening DX HD.exe");
                return exePath.TestPath();
            }
            else if (Config.SelectedPlatform == Platform.Android)
            {
                string apkPath = Path.Combine(Config.Build_Path, "com.zelda.ladxhd-Signed.apk");
                return apkPath.TestPath();
            }
            else if (Config.SelectedPlatform == Platform.Linux_x64 || Config.SelectedPlatform == Platform.Linux_Arm64)
            {
                string linuxBinPath = Path.Combine(Config.Build_Path, "Link's Awakening DX HD");
                return linuxBinPath.TestPath();
            }
            else if (Config.SelectedPlatform == Platform.MacOS_Arm64 || Config.SelectedPlatform == Platform.MacOS_x64)
            {
                string macBinPath = Path.Combine(Config.Build_Path, "Link's Awakening DX HD");
                return macBinPath.TestPath();
            }
            return false;
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

            string args = $"publish LADXHD_Launcher.csproj -r {rid} /p:PublishProfile={profile}";
            return await RunProcess("dotnet", args, false, "Launcher Build Error", Config.Launcher_Source);
        }

        private async static Task<bool> RunProcess(string executable, string arguments, bool useWsl, string errorTitle, string workingDirectory = "")
        {
            using (Process process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    // Use explicit workingDirectory if provided, otherwise fall back to existing logic.
                    WorkingDirectory = useWsl ? "" : (string.IsNullOrEmpty(workingDirectory) ? Config.Game_Source : workingDirectory),
                    FileName = executable,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                process.Start();

                string output = await process.StandardOutput.ReadToEndAsync();
                string error  = await process.StandardError.ReadToEndAsync();

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
            return true;
        }
    }
}