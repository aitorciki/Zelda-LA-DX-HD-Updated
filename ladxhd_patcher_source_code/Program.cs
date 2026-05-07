using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Avalonia;
using static LADXHD_Patcher.Config;

namespace LADXHD_Patcher
{
    internal class Program
    {
        // Windows-only console attachment for silent mode output.
        [SupportedOSPlatform("windows")]
        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);

        [SupportedOSPlatform("windows")]
        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        [SupportedOSPlatform("windows")]
        private static void TryAttachConsole() => AttachConsole(ATTACH_PARENT_PROCESS);

        [SupportedOSPlatform("windows")]
        private static void TryFreeConsole() => FreeConsole();

        private const int ATTACH_PARENT_PROCESS = -1;

        [STAThread]
        public static int Main(string[] args)
        {
            bool silentMode = args.Any(arg =>
                arg.Equals("--silent", StringComparison.OrdinalIgnoreCase) ||
                arg.Equals("-s", StringComparison.OrdinalIgnoreCase) ||
                arg.Equals("/silent", StringComparison.OrdinalIgnoreCase) ||
                arg.Equals("/s", StringComparison.OrdinalIgnoreCase));

            bool showHelp = args.Any(arg =>
                arg.Equals("--help", StringComparison.OrdinalIgnoreCase) ||
                arg.Equals("-h", StringComparison.OrdinalIgnoreCase) ||
                arg.Equals("/?", StringComparison.OrdinalIgnoreCase));

            if (silentMode || showHelp)
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    TryAttachConsole();

            if (showHelp)
            {
                Console.WriteLine();
                Console.WriteLine("LADXHD Patcher v" + Config.Version);
                Console.WriteLine();
                Console.WriteLine("Usage: LADXHD.Patcher.exe [options]");
                Console.WriteLine();
                Console.WriteLine("Options:");
                Console.WriteLine("  --silent, -s              Run in silent mode (no GUI, for automated installs)");
                Console.WriteLine("  --platform <value>        Target platform (default: windows)");
                Console.WriteLine("                            Values: windows, android, linux-x86, linux-arm64,");
                Console.WriteLine("                                    macos-x86, macos-arm64");
                Console.WriteLine("  --graphics <value>        Target graphics API");
                Console.WriteLine("                            Default: directx (windows), opengl (all others)");
                Console.WriteLine("                            Values: directx, opengl");
                Console.WriteLine("  --help, -h                Show this help message");
                Console.WriteLine();
                Console.WriteLine("Exit codes:");
                Console.WriteLine("  0  Success");
                Console.WriteLine("  1  Game executable not found");
                Console.WriteLine("  2  Patching failed");
                Console.WriteLine("  3  Invalid arguments");
                Console.WriteLine();
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    TryFreeConsole();
                return 0;
            }

            if (silentMode)
            {
                if (!TryParseTargetArgs(args, out Platform platform, out GraphicsAPI graphics))
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        TryFreeConsole();
                    return 3;
                }
                Config.Initialize();
                Config.SelectedPlatform = platform;
                Config.SelectedGraphics = graphics;

                int result = Functions.StartPatchingSilent();
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    TryFreeConsole();
                return result;
            }

            // GUI mode — Avalonia takes over from here.
            // Config.Initialize() is called in App.OnFrameworkInitializationCompleted().
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            return 0;
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();

        private static bool TryParseTargetArgs(string[] args, out Platform platform, out GraphicsAPI graphics)
        {
            platform = Platform.Windows;
            graphics = GraphicsAPI.DirectX;

            Platform? platformArg = ParsePlatformArg(args, out bool platformParseError);
            if (platformParseError)
            {
                Console.WriteLine("ERROR: Invalid --platform value. Valid values: windows, android, linux-x86, linux-arm64, macos-x86, macos-arm64");
                return false;
            }

            GraphicsAPI? graphicsArg = ParseGraphicsArg(args, out bool graphicsParseError);
            if (graphicsParseError)
            {
                Console.WriteLine("ERROR: Invalid --graphics value. Valid values: directx, opengl");
                return false;
            }

            platform = platformArg ?? Platform.Windows;

            if (graphicsArg.HasValue)
            {
                if (graphicsArg.Value == GraphicsAPI.DirectX && platform != Platform.Windows)
                {
                    Console.WriteLine("ERROR: --graphics directx is only supported on Windows. Use --graphics opengl for other platforms.");
                    return false;
                }
                graphics = graphicsArg.Value;
            }
            else
            {
                graphics = (platform == Platform.Windows) ? GraphicsAPI.DirectX : GraphicsAPI.OpenGL;
            }

            return true;
        }

        private static Platform? ParsePlatformArg(string[] args, out bool parseError)
        {
            parseError = false;
            for (int i = 0; i < args.Length; i++)
            {
                string value = null;
                if (args[i].StartsWith("--platform=", StringComparison.OrdinalIgnoreCase))
                    value = args[i].Substring("--platform=".Length);
                else if (args[i].Equals("--platform", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                    value = args[i + 1];

                if (value != null)
                {
                    switch (value.ToLowerInvariant())
                    {
                        case "windows":     return Platform.Windows;
                        case "android":     return Platform.Android;
                        case "linux-x86":   return Platform.Linux_x86;
                        case "linux-arm64": return Platform.Linux_Arm64;
                        case "macos-x86":   return Platform.MacOS_x86;
                        case "macos-arm64": return Platform.MacOS_Arm64;
                        default: parseError = true; return null;
                    }
                }
            }
            return null;
        }

        private static GraphicsAPI? ParseGraphicsArg(string[] args, out bool parseError)
        {
            parseError = false;
            for (int i = 0; i < args.Length; i++)
            {
                string value = null;
                if (args[i].StartsWith("--graphics=", StringComparison.OrdinalIgnoreCase))
                    value = args[i].Substring("--graphics=".Length);
                else if (args[i].Equals("--graphics", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                    value = args[i + 1];

                if (value != null)
                {
                    switch (value.ToLowerInvariant())
                    {
                        case "directx": return GraphicsAPI.DirectX;
                        case "opengl":  return GraphicsAPI.OpenGL;
                        default: parseError = true; return null;
                    }
                }
            }
            return null;
        }
    }
}