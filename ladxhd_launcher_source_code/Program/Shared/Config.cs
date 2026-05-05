using System;
using System.Collections.Generic;
using System.IO;
using static LADXHD_Launcher.ModMaker_Functions;

namespace LADXHD_Launcher
{
    internal class Config
    {
        public const string Version = "1.0.0";

        public static string AppPath = "";
        public static string AppName = "";
        public static string BaseFolder = "";
        public static string ZeldaEXE = "";

        public static string ModName = "";
        public static string Description = "";

        public static string DataPath = "";
        public static string BackupPath = "";

        public static string ImagePath = "";
        public static string TempFolder = "";
        public static string OutputPath = "";

        public static string AnimationMods = "";
        public static string DungeonMods = "";
        public static string GraphicsMods = "";
        public static string MusicMods = "";
        public static string LanguageMods = "";
        public static string MapsMods = "";
        public static string SoundsMods = "";
        public static string LAHDModPath = "";
        public static string ZScripts = "";

        public static string OutAnimationMods = "";
        public static string OutDungeonMods = "";
        public static string OutGraphicsMods = "";
        public static string OutMusicMods = "";
        public static string OutLanguageMods = "";
        public static string OutMapsMods = "";
        public static string OutSoundsMods = "";
        public static string OutLAHDModPath = "";
        public static string OutZScripts = "";

        public static IProgressWindow? ActiveWindow { get; set; }

        public static string LauncherConfig => Path.Combine(
            File.Exists(Path.Combine(BaseFolder, "portable.txt"))
                ? BaseFolder
                : Path.Combine(Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData), "Zelda_LA"), "launcher.cfg");

        private static Dictionary<string, object> resources = ResourceHelper.GetAllResources();

        public static void Initialize()
        {
            BaseFolder = AppContext.BaseDirectory;
            DataPath   = Path.Combine(BaseFolder, "Data");
            BackupPath = Path.Combine(DataPath, "Backup");

            #if WINDOWS
                AppPath  = Path.Combine(BaseFolder, "LADXHD_Launcher.exe");
                ZeldaEXE = Path.Combine(BaseFolder, "Link's Awakening DX HD.exe");
            #elif LINUX
                AppPath  = Path.Combine(BaseFolder, "LADXHD_Launcher");
                ZeldaEXE = Path.Combine(BaseFolder, "Link's Awakening DX HD");
            #elif MACOS
                AppPath  = Path.Combine(BaseFolder, "LADXHD_Launcher");
                ZeldaEXE = Path.Combine(BaseFolder, "Link's Awakening DX HD");
            #endif

            SetModCreatePaths();
            CreateDefaultFiles();
        }

        public static string GetGameExecutable(string path)
        {
            var exe = Path.Combine(path, "Link's Awakening DX HD.exe");
            if (File.Exists(exe))
                return exe;
            exe = Path.Combine(path, "Link's Awakening DX HD");
            if (File.Exists(exe))
                return exe;
            return "";
        }

        private static Dictionary<string, string> LoadRawValues(string path)
        {
            // Preserve the user's stored values in a dictionary.
            var values = new Dictionary<string, string>();
            string currentSection = null;

            // Read through each line of the "advanced" file.
            foreach (string raw in File.ReadAllLines(path))
            {
                // Trim whitespace and get the current section.
                string line = raw.Trim();
                if (line.StartsWith("[") && line.EndsWith("]"))
                    currentSection = line[1..^1];

                // Look for a line with a key-value pair that is not a comment and is part of a section.
                if (line.Contains('=') && !line.StartsWith("//") && currentSection != null)
                {
                    // Crop out the key and value.
                    int eq     = line.IndexOf('=');
                    string key = line[..eq].Trim();
                    string val = line[(eq + 1)..].Trim();

                    // Store the current key-value pair.
                    if (!string.IsNullOrEmpty(key))
                        values[$"{currentSection}|{key}"] = val;
                }
            }
            // Return the dictionary.
            return values;
        }

        private static void MergeValuesIntoFile(string path, Dictionary<string, string> oldValues)
        {
            // Read the new file and create a list to merge old and new.
            var lines  = File.ReadAllLines(path);
            var output = new List<string>();
            string currentSection = null;

            // Loop through each line in the new file.
            foreach (string raw in lines)
            {
                // Trim white space and find current section.
                string line = raw.Trim();
                if (line.StartsWith("[") && line.EndsWith("]"))
                    currentSection = line[1..^1];

                // Look for a line with a key-value pair that is not a comment and is part of a section.
                if (line.Contains('=') && !line.StartsWith("//") && currentSection != null)
                {
                    // Crop out the key and value.
                    int eq      = line.IndexOf('=');
                    string key  = line[..eq].Trim();
                    string newVal = line[(eq + 1)..].Trim();
                    string vk   = $"{currentSection}|{key}";

                    // Get the old value from the key in the current section.
                    if (oldValues.TryGetValue(vk, out string oldVal))
                    {
                        // Variable names may remain the same but value types might change (ex: int to float).
                        bool oldIsBool  = oldVal.Equals("true", StringComparison.OrdinalIgnoreCase) || oldVal.Equals("false", StringComparison.OrdinalIgnoreCase);
                        bool newIsBool  = newVal.Equals("true", StringComparison.OrdinalIgnoreCase) || newVal.Equals("false", StringComparison.OrdinalIgnoreCase);
                        bool oldIsFloat = !oldIsBool && oldVal.Contains('.');
                        bool newIsFloat = !newIsBool && newVal.Contains('.');

                        // Only restore the old value if types match.
                        if (oldIsBool == newIsBool && oldIsFloat == newIsFloat)
                        {
                            int rawEq  = raw.IndexOf('=');
                            string lhs = raw[..rawEq];
                            output.Add($"{lhs}= {oldVal}");
                            continue;
                        }
                    }
                }
                // Add the line the output.
                output.Add(raw);
            }
            // Write the merged file to the output.
            File.WriteAllLines(path, output);
        }

        private static void CreateDefaultFiles()
        {
            // Get the target path. User may store saves locally with portable.txt file.
            string portable  = Path.Combine(BaseFolder, "portable.txt");
            string targetDir = File.Exists(portable)
                ? BaseFolder
                : Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData), "Zelda_LA");

            // Make sure the directory exists.
            Directory.CreateDirectory(targetDir);

            // Set the paths to the two save files.
            string settingsPath = Path.Combine(targetDir, "settings");
            string advancedPath = Path.Combine(targetDir, "advanced");

            // Create "settings" file if it is missing.
            if (!File.Exists(settingsPath) && resources.ContainsKey("settings"))
                File.WriteAllBytes(settingsPath, (byte[])resources["settings"]);

            // Create "advanced" if missing or merge new with old if it exists.
            if (resources.ContainsKey("advanced"))
            {
                // It doesn't exist so simply create a new one.
                if (!File.Exists(advancedPath))
                {
                    File.WriteAllBytes(advancedPath, (byte[])resources["advanced"]);
                }
                // It does exist so it's about to get complicated...
                else
                {
                    // Load the user's stored values and merge them into the new file.
                    var oldValues = LoadRawValues(advancedPath);
                    File.WriteAllBytes(advancedPath, (byte[])resources["advanced"]);
                    MergeValuesIntoFile(advancedPath, oldValues);
                }
            }
        }

        public static void SaveLauncherConfig()
        {
            // Saves the launcher's values to the config file.
            File.WriteAllLines(LauncherConfig, new[]
            {
                $"SoundEnabled={SoundPlayer.Enabled}",
                $"WindowHeight={App.MainWindowInstance?.Height ?? 768}"
            });
            // On Linux/Mac, ensure the file is not marked as executable
            if (!OperatingSystem.IsWindows())
            {
                File.SetUnixFileMode(LauncherConfig,
                    UnixFileMode.UserRead  | 
                    UnixFileMode.UserWrite |
                    UnixFileMode.GroupRead |
                    UnixFileMode.OtherRead);
            }
        }

        public static void LoadLauncherConfig()
        {
            // Migrate old "launcher" file to "launcher.cfg" if it exists.
            string oldConfig = LauncherConfig.Replace(".cfg", "");
            if (File.Exists(oldConfig))
            {
                // Copy the old path temporarily so we can read it.
                string oldPath = oldConfig;

                // Read old values first.
                foreach (string line in File.ReadAllLines(oldPath))
                {
                    int eq = line.IndexOf('=');
                    if (eq < 0) continue;
                    string key   = line[..eq].Trim();
                    string value = line[(eq + 1)..].Trim();

                    switch (key)
                    {
                        case "SoundEnabled":
                            SoundPlayer.Enabled = value.Equals("True", StringComparison.OrdinalIgnoreCase);
                            break;
                        case "WindowHeight":
                            if (double.TryParse(value, out double h))
                                App.SavedWindowHeight = h;
                            break;
                    }
                }
                // Delete old file and save as new format.
                File.Delete(oldPath);
                SaveLauncherConfig();
                return;
            }

            // Normal load from new .cfg file.
            if (!File.Exists(LauncherConfig)) return;

            foreach (string line in File.ReadAllLines(LauncherConfig))
            {
                int eq = line.IndexOf('=');
                if (eq < 0) continue;
                string key   = line[..eq].Trim();
                string value = line[(eq + 1)..].Trim();

                switch (key)
                {
                    case "SoundEnabled":
                        SoundPlayer.Enabled = value.Equals("True", StringComparison.OrdinalIgnoreCase);
                        break;
                    case "WindowHeight":
                        if (double.TryParse(value, out double h))
                            App.SavedWindowHeight = h;
                        break;
                }
            }
        }

        public static void SetModCreatePaths()
        {
            DataPath      = Path.Combine(BaseFolder, "Data");
            BackupPath    = Path.Combine(DataPath, "Backup");
            AnimationMods = Path.Combine(BaseFolder, "Mods", "Animations");
            DungeonMods   = Path.Combine(BaseFolder, "Mods", "Dungeon");
            GraphicsMods  = Path.Combine(BaseFolder, "Mods", "Graphics");
            MusicMods     = Path.Combine(BaseFolder, "Mods", "Music");
            LanguageMods  = Path.Combine(BaseFolder, "Mods", "Languages");
            MapsMods      = Path.Combine(BaseFolder, "Mods", "Maps");
            SoundsMods    = Path.Combine(BaseFolder, "Mods", "SoundEffects");
            LAHDModPath   = Path.Combine(BaseFolder, "Mods", "LAHDMods");
            ZScripts      = Path.Combine(BaseFolder, "Mods", "scripts.zScript");
        }

        public static void UpdateOutputPaths(string output)
        {
            // The paths when creating patches.
            OutputPath       = Path.Combine(output, "~ModOutput");
            OutAnimationMods = Path.Combine(OutputPath, "Mods", "Animations");
            OutDungeonMods   = Path.Combine(OutputPath, "Mods", "Dungeon");
            OutGraphicsMods  = Path.Combine(OutputPath, "Mods", "Graphics");
            OutMusicMods     = Path.Combine(OutputPath, "Mods", "Music");
            OutLanguageMods  = Path.Combine(OutputPath, "Mods", "Languages");
            OutMapsMods      = Path.Combine(OutputPath, "Mods", "Maps");
            OutSoundsMods    = Path.Combine(OutputPath, "Mods", "SoundEffects");
            OutLAHDModPath   = Path.Combine(OutputPath, "Mods", "LAHDMods");
            OutZScripts      = Path.Combine(OutputPath, "Mods", "scripts.zScript");
        }

        public static void UpdateOutputPaths_ApplyPatches()
        {
            // The paths when applying patches.
            OutputPath       = Path.Combine(BaseFolder, "Mods", "Graphics");
            OutAnimationMods = Path.Combine(TempFolder, "Mods", "Animations");
            OutDungeonMods   = Path.Combine(TempFolder, "Mods", "Dungeon");
            OutGraphicsMods  = Path.Combine(TempFolder, "Mods", "Graphics");
            OutMusicMods     = Path.Combine(TempFolder, "Mods", "Music");
            OutLanguageMods  = Path.Combine(TempFolder, "Mods", "Languages");
            OutMapsMods      = Path.Combine(TempFolder, "Mods", "Maps");
            OutSoundsMods    = Path.Combine(TempFolder, "Mods", "SoundEffects");
            OutLAHDModPath   = Path.Combine(TempFolder, "Mods", "LAHDMods");
            OutZScripts      = Path.Combine(TempFolder, "Mods", "scripts.zScript");
        }
    }
}
