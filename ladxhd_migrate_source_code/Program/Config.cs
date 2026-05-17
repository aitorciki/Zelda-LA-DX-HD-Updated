using System;
using System.IO;
using System.Runtime.InteropServices;

namespace LADXHD_Migrater
{
    internal class Config
    {
        public static string AppPath;
        public static string BaseFolder;
        public static string Patches;
        public static string Orig_Content;
        public static string Orig_Data;
        public static string Game_Source;
        public static string Launcher_Source;
        public static string Migrate_Source;
        public static string Patcher_Source;
        public static string VCDiff_Source;
        public static string Update_Content;
        public static string Update_Data;
        public static string Publish_Path;
        public static string Build_Path;

        public enum Platform { Windows, Android, Linux_x64, Linux_Arm64, MacOS_x64, MacOS_Arm64  }
        public static Platform SelectedPlatform;

        public enum GraphicsAPI { DirectX, OpenGL }
        public static GraphicsAPI SelectedGraphics;

        public static Platform GetNativePlatform()
        {
            bool isArm64 = RuntimeInformation.OSArchitecture == Architecture.Arm64;
            if (OperatingSystem.IsLinux()) return isArm64 ? Platform.Linux_Arm64 : Platform.Linux_x64;
            if (OperatingSystem.IsMacOS()) return isArm64 ? Platform.MacOS_Arm64 : Platform.MacOS_x64;
            return Platform.Windows;
        }

        public static void Initialize()
        {
            AppPath         = AppContext.BaseDirectory;

#if MACOS
            // When running inside a .app bundle the binary lives at
            // Foo.app/Contents/MacOS/ — navigate up to the folder containing
            // the .app so companion files are reachable.
            var contentsDir = Path.GetFullPath(Path.Combine(AppPath, ".."));
            if (Path.GetFileName(contentsDir) == "Contents" &&
                File.Exists(Path.Combine(contentsDir, "Info.plist")))
            {
                AppPath = Path.GetFullPath(Path.Combine(contentsDir, "..", "..")) + Path.DirectorySeparatorChar;
            }
#endif

            BaseFolder      = Path.GetDirectoryName(AppPath);
            Patches         = Path.Combine(BaseFolder, "assets_patches");
            Orig_Content    = Path.Combine(BaseFolder, "assets_original", "Content");
            Orig_Data       = Path.Combine(BaseFolder, "assets_original", "Data");
            Game_Source     = Path.Combine(BaseFolder, "ladxhd_game_source_code");
            Launcher_Source = Path.Combine(BaseFolder, "ladxhd_launcher_source_code");
            Migrate_Source  = Path.Combine(BaseFolder, "ladxhd_migrate_source_code");
            Patcher_Source  = Path.Combine(BaseFolder, "ladxhd_patcher_source_code");
            VCDiff_Source   = Path.Combine(BaseFolder, "vcdiff_source_code");
            Update_Content  = Path.Combine(Game_Source, "ProjectZ.Core", "Content");
            Update_Data     = Path.Combine(Game_Source, "ProjectZ.Core", "Data");
            Publish_Path    = Path.Combine(Game_Source, "~Publish");
        }
    }
}
