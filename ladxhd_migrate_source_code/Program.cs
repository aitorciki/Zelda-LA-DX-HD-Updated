using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Avalonia;
using Mono.Options;

namespace LADXHD_Migrater
{
    internal class Program
    {
        [SupportedOSPlatform("windows")]
        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);

        [SupportedOSPlatform("windows")]
        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        private const int ATTACH_PARENT_PROCESS = -1;

        private static void ExitHeadless(int exitCode)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                FreeConsole();
            Environment.Exit(exitCode);
        }

        [STAThread]
        public static async Task<int> Main(string[] args)
        {
            bool showHelp = false;
            bool migrateAssets = false;
            bool createPatches = false;
            bool cleanBuild = false;
            bool buildRequested = false;
            string? buildPlatform = null;
            string? graphicsStr = null;

            var opts = new OptionSet {
                { "migrate-assets|m",  "Run asset migration in headless mode.",      _ => migrateAssets = true },
                { "create-patches|p",  "Create vcdiff patches from modified files.", _ => createPatches = true },
                { "clean-build|c",     "Remove all bin/obj/Publish folders.",        _ => cleanBuild = true },
                { "build|b:",          "Build for platform [windows|android|linux-x86|linux-arm64|macos-x86|macos-arm64]. Default: current platform.", v => { buildRequested = true; buildPlatform = v; } },
                { "graphics=",         "Target graphics API: directx, opengl. Default: directx (windows), opengl (others).", v => graphicsStr = v },
                { "headless",          "Deprecated. Use --migrate-assets instead.",  _ => showHelp = true },
                { "h|?|help",          "Show this help message.",                    _ => showHelp = true },
            };

            opts.Parse(args);

            if (buildRequested && buildPlatform == null)
            {
                Config.SelectedPlatform = Config.GetNativePlatform();
                Config.SelectedGraphics = (Config.SelectedPlatform == Config.Platform.Windows)
                    ? Config.GraphicsAPI.DirectX
                    : Config.GraphicsAPI.OpenGL;
            }

            if (showHelp)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    AttachConsole(ATTACH_PARENT_PROCESS);

                Console.WriteLine();
                Console.WriteLine("LADXHD Migrater");
                Console.WriteLine("Usage: Migrater [options]");
                Console.WriteLine();
                Console.WriteLine("Options:");
                opts.WriteOptionDescriptions(Console.Out);
                Console.WriteLine();
                Console.WriteLine("Exit codes: 0=Success, 1=Source assets missing, 2=Operation failed, 3=Invalid arguments");
                Console.WriteLine();

                ExitHeadless(0);
            }

            if (migrateAssets || createPatches || cleanBuild || buildRequested)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    AttachConsole(ATTACH_PARENT_PROCESS);

                Functions.HeadlessMode = true;
                Config.Initialize();
                BuildAvaloniaApp().SetupWithoutStarting();

                if (buildRequested && buildPlatform != null)
                {
                    try
                    {
                        Config.Platform platform = buildPlatform.ToLowerInvariant() switch
                        {
                            "windows"     => Config.Platform.Windows,
                            "android"     => Config.Platform.Android,
                            "linux-x86"   => Config.Platform.Linux_x64,
                            "linux-arm64" => Config.Platform.Linux_Arm64,
                            "macos-x86"   => Config.Platform.MacOS_x64,
                            "macos-arm64" => Config.Platform.MacOS_Arm64,
                            _ => throw new ArgumentException($"Invalid --build value '{buildPlatform}'. Valid values: windows, android, linux-x86, linux-arm64, macos-x86, macos-arm64")
                        };

                        Config.GraphicsAPI graphics;
                        if (graphicsStr != null)
                        {
                            graphics = graphicsStr.ToLowerInvariant() switch
                            {
                                "directx" => Config.GraphicsAPI.DirectX,
                                "opengl"  => Config.GraphicsAPI.OpenGL,
                                _ => throw new ArgumentException($"Invalid --graphics value '{graphicsStr}'. Valid values: directx, opengl")
                            };
                            if (graphics == Config.GraphicsAPI.DirectX && platform != Config.Platform.Windows)
                                throw new ArgumentException("--graphics directx is only supported on Windows. Use --graphics opengl for other platforms.");
                        }
                        else
                        {
                            graphics = (platform == Config.Platform.Windows) ? Config.GraphicsAPI.DirectX : Config.GraphicsAPI.OpenGL;
                        }

                        Config.SelectedPlatform = platform;
                        Config.SelectedGraphics = graphics;
                    }
                    catch (ArgumentException ex)
                    {
                        Console.Error.WriteLine("ERROR: " + ex.Message);
                        ExitHeadless(3);
                    }
                }

                if (migrateAssets)
                {
                    if (!Config.Orig_Content.TestPath() || !Config.Orig_Data.TestPath())
                    {
                        Console.Error.WriteLine("ERROR: assets_original/Content or assets_original/Data is missing.");
                        ExitHeadless(1);
                    }

                    try
                    {
                        Functions.MigrateFiles();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("ERROR: " + ex.Message);
                        ExitHeadless(2);
                    }

                    Console.WriteLine("Migration complete.");
                    ExitHeadless(0);
                }

                if (createPatches)
                {
                    if (!Config.Orig_Content.TestPath() || !Config.Orig_Data.TestPath())
                    {
                        Console.Error.WriteLine("ERROR: assets_original/Content or assets_original/Data is missing.");
                        ExitHeadless(1);
                    }
                    if (!Config.Update_Content.TestPath() || !Config.Update_Data.TestPath())
                    {
                        Console.Error.WriteLine("ERROR: ladxhd_game_source_code Content or Data is missing.");
                        ExitHeadless(1);
                    }

                    try
                    {
                        Functions.CreatePatches();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("ERROR: " + ex.Message);
                        ExitHeadless(2);
                    }

                    Console.WriteLine("Patches created.");
                    ExitHeadless(0);
                }

                if (cleanBuild)
                {
                    try
                    {
                        Functions.CleanBuildFiles();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("ERROR: " + ex.Message);
                        ExitHeadless(2);
                    }

                    Console.WriteLine("Build files cleaned.");
                    ExitHeadless(0);
                }

                if (buildRequested)
                {
                    try
                    {
                        bool success = await Functions.CreateBuild();
                        if (!success)
                        {
                            Console.Error.WriteLine("ERROR: Build failed.");
                            ExitHeadless(2);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("ERROR: " + ex.Message);
                        ExitHeadless(2);
                    }

                    Console.WriteLine("Build complete.");
                    ExitHeadless(0);
                }
            }

            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            return 0;
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}