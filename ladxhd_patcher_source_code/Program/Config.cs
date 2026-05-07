using System;
using System.IO;
using System.Reflection;
using static LADXHD_Patcher.Functions;

namespace LADXHD_Patcher
{
    public class Config
    {
        public const string Version = "1.8.0";

        public static string BaseFolder;
        public static string TempFolder;
        public static string ZeldaEXE;
        public static string BackupPath;

        public static string ModsPath;
        public static string AnimationMods;
        public static string DungeonMods;
        public static string GraphicsMods;
        public static string MusicMods;
        public static string LanguageMods;
        public static string MapsMods;
        public static string SoundsMods;
        public static string LAHDModPath;

        public static string Launcher;
        public static string WLauncher;

        public static string ApkSign;
        public static string ZipAlign;
        public static string KeyStore;
        public static string JavaExe;
        public static string SevenZip;

        public static bool AndroidMods;

        public static IProgressWindow? ActiveWindow { get; set; }

        public enum Platform { Windows, Android, Linux_x86, Linux_Arm64, MacOS_x86, MacOS_Arm64 }
        public static Platform SelectedPlatform;

        public enum GraphicsAPI { DirectX, OpenGL }
        public static GraphicsAPI SelectedGraphics;

        public static void Initialize()
        {
            BaseFolder    = AppContext.BaseDirectory;
            TempFolder    = Path.Combine(BaseFolder, "~temp");
            ZeldaEXE      = Path.Combine(BaseFolder, "Link's Awakening DX HD.exe");
            BackupPath    = Path.Combine(BaseFolder, "Data", "Backup");

            ModsPath      = Path.Combine(BaseFolder, "Mods");
            AnimationMods = Path.Combine(ModsPath, "Animations");
            DungeonMods   = Path.Combine(ModsPath, "Dungeon");
            GraphicsMods  = Path.Combine(ModsPath, "Graphics");
            MusicMods     = Path.Combine(ModsPath, "Music");
            LanguageMods  = Path.Combine(ModsPath, "Languages");
            MapsMods      = Path.Combine(ModsPath, "Maps");
            SoundsMods    = Path.Combine(ModsPath, "SoundEffects");
            LAHDModPath   = Path.Combine(ModsPath, "LAHDMods");

            Launcher      = Path.Combine(BaseFolder, "Launcher");
            WLauncher     = Path.Combine(BaseFolder, "Launcher.exe");

            ApkSign       = Path.Combine(TempFolder, "android", "apksigner.jar");
            ZipAlign      = Path.Combine(TempFolder, "android", "zipalign.exe");
            KeyStore      = Path.Combine(TempFolder, "android", "keystore.jks");
            JavaExe       = Path.Combine(TempFolder, "android", "java", "bin", "java.exe");
            SevenZip      = Path.Combine(TempFolder, "7z.exe");

            CleanUp.Init();
        }
    }
}