using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
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

        [STAThread]
        public static int Main(string[] args)
        {
            bool showHelp = false;
            bool migrateAssets = false;

            var opts = new OptionSet {
                { "migrate-assets|m",  "Run asset migration in headless mode.",     _ => migrateAssets = true },
                { "headless",          "Deprecated. Use --migrate-assets instead.", _ => showHelp = true },
                { "h|?|help",          "Show this help message.",                   _ => showHelp = true },
            };

            opts.Parse(args);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                AttachConsole(ATTACH_PARENT_PROCESS);

            if (showHelp)
            {
                Console.WriteLine();
                Console.WriteLine("LADXHD Migrater");
                Console.WriteLine("Usage: Migrater [options]");
                Console.WriteLine();
                Console.WriteLine("Options:");
                opts.WriteOptionDescriptions(Console.Out);
                Console.WriteLine();
                Console.WriteLine("Exit codes: 0=Success, 1=Source assets missing, 2=Migration failed");
                Console.WriteLine();
                return 0;
            }

            if (migrateAssets)
            {
                Config.Initialize();
                BuildAvaloniaApp().SetupWithoutStarting();
                Functions.HeadlessMode = true;

                if (!Config.Orig_Content.TestPath() || !Config.Orig_Data.TestPath())
                {
                    Console.Error.WriteLine("ERROR: assets_original/Content or assets_original/Data is missing.");
                    return 1;
                }

                try
                {
                    Functions.MigrateFiles();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("ERROR: " + ex.Message);
                    return 2;
                }

                Console.WriteLine("Migration complete.");
                return 0;
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