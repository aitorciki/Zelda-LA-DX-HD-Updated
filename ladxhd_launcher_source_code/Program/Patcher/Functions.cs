using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using static LADXHD_Launcher.VCDiff;

namespace LADXHD_Launcher
{
    internal class Patcher_Functions
    {
        private static int    _fileCount = 0;
        private static int    _totalCount = 0;

/*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        FILE MAPPING CODE : NOT ALL FILES AND PATCHES ARE 1:1 FROM ORIGINAL GAME VERSION. NEW FILES NEED A "BASE" TO BE CREATED FROM USING A PATCH
       
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        // SOME RESOURCES ARE USED TO CREATE MULTIPLE FILES OR PATCHES. EACH ARRAY BELOW HOLDS ALL VERSIONS OF A FILE THAT IS
        // BASED OFF OF ANOTHER FILE. THE "MASTER" FILE THAT CREATES THESE VERSIONS IS LINKED TO THEM IN THE DICTIONARY BELOW

        private static string[] langFiles  = new[] { "chn.lng", "deu.lng", "esp.lng", "fre.lng", "ind.lng", "ita.lng", "por.lng", "pte.lng", "rus.lng" };
        private static string[] langDialog = new[] { "dialog_chn.lng", "dialog_deu.lng", "dialog_esp.lng", "dialog_fre.lng", "dialog_ind.lng", "dialog_ita.lng", "dialog_por.lng", "dialog_pte.lng", "dialog_rus.lng" };
        private static string[] smallFonts = new[] { "smallFont_redux.xnb", "smallFont_vwf.xnb", "smallFont_vwf_redux.xnb", "smallFont_chn.xnb", "smallFont_chn_0.xnb", "smallFont_chn_redux.xnb", "smallFont_chn_redux_0.xnb" };
        private static string[] backGround = new[] { "menuBackgroundB.xnb", "menuBackgroundC.xnb", "sgb_border.xnb" };
        private static string[] lighting   = new[] { "mamuLight.xnb" };
        private static string[] linkImages = new[] { "link1.png", "weapons.png" };
        private static string[] npcImages  = new[] { "npcs_redux.png" };
        private static string[] itemImages = new[] { "items_chn.png", "items_deu.png", "items_esp.png", "items_fre.png", "items_ind.png", "items_ita.png", "items_por.png", "items_rus.png", "items_redux.png", 
                                                     "items_redux_chn.png", "items_redux_deu.png", "items_redux_esp.png", "items_redux_fre.png", "items_redux_ind.png", "items_redux_ita.png", "items_redux_por.png", "items_redux_rus.png" };
        private static string[] introImage = new[] { "intro_chn.png", "intro_deu.png", "intro_esp.png", "intro_fre.png", "intro_ind.png", "intro_ita.png", "intro_por.png", "intro_rus.png" };
        private static string[] introAtlas = new[] { "intro_chn.atlas" };
        private static string[] miniMapImg = new[] { "minimap_chn.png", "minimap_deu.png", "minimap_esp.png", "minimap_fre.png", "minimap_ind.png", "minimap_ita.png", "minimap_por.png", "minimap_rus.png" };
        private static string[] objectsImg = new[] { "objects_chn.png", "objects_deu.png", "objects_esp.png", "objects_fre.png", "objects_ind.png", "objects_ita.png", "objects_por.png", "objects_rus.png" };
        private static string[] photograph = new[] { "photos_chn.png", "photos_deu.png", "photos_esp.png", "photos_fre.png",  "photos_ind.png", "photos_ita.png", "photos_por.png", "photos_rus.png", "photos_redux.png", 
                                                     "photos_redux_chn.png", "photos_redux_deu.png", "photos_redux_esp.png", "photos_redux_fre.png", "photos_redux_ind.png", "photos_redux_ita.png", "photos_redux_por.png", "photos_redux_rus.png" };
        private static string[] uiImages   = new[] { "ui_chn.png", "ui_deu.png", "ui_esp.png", "ui_fre.png", "ui_ind.png", "ui_ita.png", "ui_por.png", "ui_rus.png" };
        private static string[] musicTile  = new[] { "musicOverworldClassic.data" };
        private static string[] dungeon3M  = new[] { "dungeon3.map" };
        private static string[] dungeon3D  = new[] { "dungeon3.map.data" };
        private static string[] bowwowanim = new[] { "bowwow_water.ani" };
        private static string[] dungeonani = new[] { "mapDungeon.ani", "mapManboPond.ani" };
        private static string[] boomerang  = new[] { "boomerangOrig.ani" };
        private static string[] shaderFile = new[] { "PixelGrid.xnb" };

        // THE "KEY" IS THE MASTER FILE THAT CREATES OTHER FILES FROM IT. THE "VALUE" IS THE STRING ARRAY THAT HOLDS THOSE FILES

        private static readonly Dictionary<string, string[]> fileTargets = new Dictionary<string, string[]>
        {
            { "eng.lng",              langFiles },
            { "dialog_eng.lng",      langDialog },
            { "smallFont.xnb",       smallFonts },
            { "menuBackground.xnb",  backGround },
            { "ligth room.xnb",        lighting },
            { "link0.png",           linkImages },
            { "npcs.png",             npcImages },
            { "items.png",           itemImages },
            { "intro.png",           introImage },
            { "intro.atlas",         introAtlas },
            { "minimap.png",         miniMapImg },
            { "objects.png",         objectsImg },
            { "photos.png",          photograph },
            { "ui.png",                uiImages },
            { "musicOverworld.data",  musicTile },
            { "dungeon3_1.map",       dungeon3M },
            { "dungeon3_1.map.data",  dungeon3D },
            { "BowWow.ani",          bowwowanim },
            { "mapPlayer.ani",       dungeonani },
            { "boomerang.ani",        boomerang },
            { "ShockEffect.xnb",     shaderFile }
        };

        private static readonly HashSet<string> _targetList = fileTargets.Values.SelectMany(v => v).ToHashSet(StringComparer.OrdinalIgnoreCase);

/*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        POST PATCHING CODE : LINUX / MACOS FINALIZATION SCRIPTS
       
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        // Sets the executable bit (chmod +x) on a file.
        private static void SetExecutableBit(string path)
        {
            if (!File.Exists(path)) return;
            if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
            {
                File.SetUnixFileMode(path, UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute |
                                           UnixFileMode.GroupRead | UnixFileMode.GroupExecute |
                                           UnixFileMode.OtherRead | UnixFileMode.OtherExecute);
            }
        }

        // Ad-hoc codesign an executable, dylib, or app bundle.
        private static void CodeSign(string path)
        {
            if (!OperatingSystem.IsMacOS() || (!File.Exists(path) && !Directory.Exists(path))) return;
            var psi = new ProcessStartInfo
            {
                FileName = "codesign",
                Arguments = $"--sign - --force --deep \"{path}\"",
                CreateNoWindow = true,
                UseShellExecute = false
            };
            Process.Start(psi)?.WaitForExit();
        }

        private static void CopyDirectory(string sourceDir, string destinationDir, bool recursive, string[]? excludeFolders = null)
        {
            var dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists) return;

            Directory.CreateDirectory(destinationDir);

            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath, true);
            }

            if (recursive)
            {
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    if (excludeFolders != null && excludeFolders.Contains(subDir.Name))
                        continue;

                    // Preserve symlinks rather than copying their contents.
                    if (subDir.Attributes.HasFlag(FileAttributes.ReparsePoint))
                    {
                        string? linkTarget = subDir.LinkTarget;
                        if (linkTarget != null)
                            Directory.CreateSymbolicLink(Path.Combine(destinationDir, subDir.Name), linkTarget);
                        continue;
                    }

                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true, excludeFolders);
                }
            }
        }

        private static void RunLinuxFinalization()
        {
            string executableName = Path.GetFileNameWithoutExtension(Config.ZeldaEXE);
            string exePath = Path.Combine(Config.RootPath, executableName);
            SetExecutableBit(exePath);

            string launcherPath = Path.Combine(Config.RootPath, "Launcher");
            if (File.Exists(launcherPath))
                SetExecutableBit(launcherPath);
        }

        // Finalizing for macOS requires:
        //   * setting the executable bit on the game binary
        //   * signing all binaries (executable and libraries)
        //   * bundling as an app bundle, and signing the bundle
        //   * repeating for the launcher if available
        private static void RunMacOSFinalization()
        {
            string name = Path.GetFileNameWithoutExtension(Config.ZeldaEXE);
            string baseFolder = Config.RootPath;
            string tempFolder = Config.TempPath;
            
            string bundle = Path.Combine(baseFolder, $"{name}.app");
            string bundleTmp = Path.Combine(tempFolder, $"{name}.app");

            // Clean slate
            if (Directory.Exists(bundleTmp))
                Directory.Delete(bundleTmp, true);

            // Set executable bit and codesign standalone binary
            string mainExe = Path.Combine(baseFolder, name);
            SetExecutableBit(mainExe);
            CodeSign(mainExe);

            // Codesign dylibs
            string[] gameDylibs = { "libopenal.dylib", "libSDL2-2.0.0.dylib" };
            foreach (var dylib in gameDylibs)
            {
                string dylibPath = Path.Combine(baseFolder, dylib);
                if (File.Exists(dylibPath))
                    CodeSign(dylibPath);
            }

            // Create bundle structure
            string contentsPath = Path.Combine(bundleTmp, "Contents");
            string macOSPath = Path.Combine(contentsPath, "MacOS");
            string resourcesPath = Path.Combine(contentsPath, "Resources");
            
            Directory.CreateDirectory(macOSPath);
            Directory.CreateDirectory(resourcesPath);

            // Copy binary and dylibs
            File.Copy(mainExe, Path.Combine(macOSPath, name), true);
            SetExecutableBit(Path.Combine(macOSPath, name));
            foreach (var dylib in gameDylibs)
            {
                string dylibPath = Path.Combine(baseFolder, dylib);
                if (File.Exists(dylibPath))
                    File.Copy(dylibPath, Path.Combine(macOSPath, dylib), true);
            }

            // Copy Data, Content, and Mods
            string dataSrc = Path.Combine(baseFolder, "Data");
            if (Directory.Exists(dataSrc))
                CopyDirectory(dataSrc, Path.Combine(macOSPath, "Data"), true, null);

            string contentSrc = Path.Combine(baseFolder, "Content");
            if (Directory.Exists(contentSrc))
                CopyDirectory(contentSrc, Path.Combine(macOSPath, "Content"), true, null);

            string modsSrc = Path.Combine(baseFolder, "Mods");
            if (Directory.Exists(modsSrc))
                CopyDirectory(modsSrc, Path.Combine(macOSPath, "Mods"), true, null);

            // Write Resources
            bool isArm64 = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture == System.Runtime.InteropServices.Architecture.Arm64;
            string arch = isArm64 ? "arm64" : "x86_64";
            File.WriteAllBytes(Path.Combine(resourcesPath, "Icon.icns"), Resources.GetBytes("Icon.icns"));

            string template = System.Text.Encoding.UTF8.GetString(Resources.GetBytes("Info.plist.template"));
            string plist = template
                .Replace("{EXECUTABLE}", name)
                .Replace("{VERSION}", Config.CurrentVersion)
                .Replace("{ARCH}", arch);
            File.WriteAllText(Path.Combine(contentsPath, "Info.plist"), plist, new System.Text.UTF8Encoding(false));

            // Create Content symlink
            File.CreateSymbolicLink(Path.Combine(resourcesPath, "Content"), "../MacOS/Content");

            // Codesign the app bundle
            CodeSign(bundleTmp);

            // Move completed bundle
            if (Directory.Exists(bundle))
                Directory.Delete(bundle, true);
            Directory.Move(bundleTmp, bundle);

            // Handle Launcher
            string launcherSrc = Path.Combine(baseFolder, "Launcher");
            if (File.Exists(launcherSrc))
            {
                SetExecutableBit(launcherSrc);
                CodeSign(launcherSrc);

                string[] launcherDylibs = { "libAvaloniaNative.dylib", "libHarfBuzzSharp.dylib", "libSkiaSharp.dylib" };
                foreach (var dylib in launcherDylibs)
                {
                    string dylibPath = Path.Combine(baseFolder, dylib);
                    if (File.Exists(dylibPath))
                        CodeSign(dylibPath);
                }

                string launcherBundle = Path.Combine(baseFolder, $"{name} Launcher.app");
                string launcherTmp = Path.Combine(tempFolder, $"{name} Launcher.app");

                if (Directory.Exists(launcherTmp))
                    Directory.Delete(launcherTmp, true);

                // Start with a copy of the game bundle
                CopyDirectory(bundle, launcherTmp, true);

                // Add launcher binary and its dylibs
                string launcherDestMacOS = Path.Combine(launcherTmp, "Contents", "MacOS");
                File.Copy(launcherSrc, Path.Combine(launcherDestMacOS, "Launcher"), true);
                SetExecutableBit(Path.Combine(launcherDestMacOS, "Launcher"));

                foreach (var dylib in launcherDylibs)
                {
                    string dylibPath = Path.Combine(baseFolder, dylib);
                    if (File.Exists(dylibPath))
                        File.Copy(dylibPath, Path.Combine(launcherDestMacOS, dylib), true);
                }

                // Update Info.plist for Launcher
                string launcherPlistPath = Path.Combine(launcherTmp, "Contents", "Info.plist");
                string launcherPlist = File.ReadAllText(launcherPlistPath)
                    .Replace($"<string>{name}</string>", "<string>Launcher</string>")
                    .Replace("com.projectz.game", "com.projectz.launcher");
                File.WriteAllText(launcherPlistPath, launcherPlist, new System.Text.UTF8Encoding(false));

                // Codesign the launcher bundle
                CodeSign(launcherTmp);

                if (Directory.Exists(launcherBundle))
                    Directory.Delete(launcherBundle, true);
                Directory.Move(launcherTmp, launcherBundle);
            }
        }

        private async static Task HostFinalizationFunctions()
        {
            if (OperatingSystem.IsLinux())
            {
                try { RunLinuxFinalization(); }
                catch {
                    await ShowWarning(
                        "Patching partially complete",
                        "The game was patched, but the Linux finalization steps failed to apply.\n" +
                        "Please check https://github.com/BigheadSMZ/Zelda-LA-DX-HD-Updated/blob/main/LINUX.md."
                    );
                }
            }
            else if (OperatingSystem.IsMacOS())
            {
                try { RunMacOSFinalization(); }
                catch {
                    await ShowWarning(
                        "Patching partially complete",
                        "The game was patched, but the macOS finalization steps failed to apply.\n" +
                        "Please check https://github.com/BigheadSMZ/Zelda-LA-DX-HD-Updated/blob/main/MACOS.md."
                    );
                }
            }
        }

/*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        PROGRESS CODE : TRACK AND UPDATE THE PROGRESS BAR BY KEEPING TRACK OF THE FILE COUNTS.
       
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        private async static Task ShowWarning(string title, string message, bool altSound = false)
        {
            await OkayWindow.ShowAsync(title, message, timeoutSeconds: 10, altSound: altSound);
        }

        private static void ResetProgress()
        {
            // Reset the variables.
            _fileCount = 0;
            _totalCount = 0;
            Config.ActiveWindow?.UpdateProgressBar(0);
        }

        private static async Task UpdateProgress()
        {
            // Update the file count.
            _fileCount++;

            // Update the progress bar and process UI events (GUI mode only).
            int progress = _totalCount > 0 ? (int)(_fileCount * 100.0 / _totalCount) : 0;
            Config.ActiveWindow?.UpdateProgressBar(progress);
        }

/*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        POST PATCHING CODE : ADDITIONAL FILE AND FOLDER HANDLING

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        private static void CopyNewFiles()
        {
            // Set up the path to the Icon.
            string iconPath = Path.Combine(Config.DataPath, "Icon").CreatePath();

            // Write the icon to the "Data\Icon" folder.
            string iconIcoFile = Path.Combine(iconPath, "Icon.ico");
            File.WriteAllBytes(iconIcoFile, Resources.GetBytes("Icon.ico"));

            // Write the bitmap icon to the "Data\Icon" folder.
            string iconBmpFile = Path.Combine(iconPath, "Icon.bmp");
            File.WriteAllBytes(iconBmpFile, Resources.GetBytes("Icon.bmp"));

            // Write the png icon to the the "Data\Icon" folder.
            string iconPngFile = Path.Combine(iconPath, "Icon.png");
            File.WriteAllBytes(iconPngFile, Resources.GetBytes("Icon.png"));

            // Write the svg icon to the the "Data\Icon" folder.
            string iconSvgFile = Path.Combine(iconPath, "Icon.svg");
            File.WriteAllBytes(iconSvgFile, Resources.GetBytes("Icon.svg"));
        }

        private static void RemoveBadBackupFiles()
        {
            // Because old versions of the patchers saved "new" files, we need to remove them or they will cause problems.
            string[][] list = { langFiles, langDialog, smallFonts, backGround, lighting, linkImages, npcImages, itemImages, introImage, introAtlas, 
                                miniMapImg, objectsImg, photograph, uiImages, musicTile, dungeon3M, dungeon3D, bowwowanim, dungeonani, boomerang };

            string[] remove = list.SelectMany(x => x).ToArray();

            // Loop through the files in the backup folder.
            foreach (string file in Config.BackupPath.GetFiles("*", true))
            {
                // Get the file as a file item which gives us some cool properties to reference.
                FileItem fileItem = new FileItem(file);

                // If the current array file exists then remove it.
                if (remove.Contains(fileItem.Name))
                    fileItem.FullName.RemovePath();
            }
        }

/*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        POST PATCHING CODE : MASTER FUNCTION : STUFF THAT IS DONE AFTER PATCHING HAS FINISHED.
       
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        private async static Task PostPatchingFunctions()
        {
            // We need the new FNT files for Chinese font, the new editor fonts, and a bitmap icon for OpenGL.
            CopyNewFiles();

            // They will probably be there again so remove them one more time.
            RemoveBadBackupFiles();

            // After migration, some map files are not needed.
            CleanUp.RemoveJunkMapFiles();
        }
/*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        UPDATE LAUNCHER: REPLACES THE LAUNCHER WITH A NEW VERSION DOWNLOADED FROM GITHUB.
       
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        private static void ReplaceLauncherWindows(string launcherZipPath)
        {
            string extractPath = Path.Combine(Config.TempPath, "launcher_new");
            if (Directory.Exists(extractPath))
                Directory.Delete(extractPath, true);
            ZipFile.ExtractToDirectory(launcherZipPath, extractPath);

            string newLauncher = Path.Combine(extractPath, Path.GetFileName(Config.AppPath));
            string oldLauncher = Config.AppPath;

            // Move old launcher to Content folder instead of leaving it visible
            string contentFolder = Path.Combine(Config.RootPath, "Content");
            Directory.CreateDirectory(contentFolder);
            string oldRenamed = Path.Combine(contentFolder, Path.GetFileNameWithoutExtension(Config.AppPath) + ".old");

            File.Move(oldLauncher, oldRenamed, overwrite: true);
            File.Copy(newLauncher, oldLauncher, overwrite: true);
        }

        private static void ReplaceLauncherUnix(string launcherZipPath)
        {
            string extractPath = Path.Combine(Config.TempPath, "launcher_new");
            if (Directory.Exists(extractPath))
                Directory.Delete(extractPath, true);
            ZipFile.ExtractToDirectory(launcherZipPath, extractPath);

            // Find the launcher binary in the extracted folder
            string newLauncher = Directory.GetFiles(extractPath, "Launcher*", SearchOption.AllDirectories)
                .FirstOrDefault(f => !f.EndsWith(".zip"));

            if (newLauncher == null)
                throw new Exception("Could not find launcher binary in zip.");

            // Copy over the old launcher
            File.Copy(newLauncher, Config.AppPath, overwrite: true);

            // Ensure it's executable
            File.SetUnixFileMode(Config.AppPath,
                UnixFileMode.UserRead    | UnixFileMode.UserWrite  | UnixFileMode.UserExecute  |
                UnixFileMode.GroupRead   | UnixFileMode.GroupExecute |
                UnixFileMode.OtherRead   | UnixFileMode.OtherExecute);
        }

        public static void ReplaceLauncher(string launcherZipPath)
        {
            #if WINDOWS
                ReplaceLauncherWindows(launcherZipPath);
            #else
                ReplaceLauncherUnix(launcherZipPath);
            #endif
        }

/*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        PATCHING CODE : PATCH FILES USING VCDIFF PATCHES TO UPDATE TO THE LATEST VERSION.
       
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        private static Dictionary<string, string> _gameFileLookup;

        private static void BuildGameFileLookup()
        {
            // Create a dictionary to store both file name (key) and the path to it (value).
            _gameFileLookup = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // Loop through all files in the base folder.
            foreach (string file in Directory.EnumerateFiles(Config.RootPath, "*", SearchOption.AllDirectories))
            {
                // Get a file item of the file.
                var fileItem = new FileItem(file);

                // Always skip files in the mods, backup, and temp folders.
                if (fileItem.IsInFolder("Mods") || fileItem.IsInFolder("Backup") || fileItem.IsInFolder("~temp"))
                    continue;

                // Get the name of the file for the key.
                string name = Path.GetFileName(file);

                // Store the key if it doesn't exist and set the value to the full path.
                if (!_gameFileLookup.ContainsKey(name))
                    _gameFileLookup[name] = file;

                // If there is a duplicate key, add a hashtag that will be trimmed later.
                else
                    _gameFileLookup[name + "#"] = file;
            }
        }

        private static void HandleMultiFilePatches(string filePath)
        {
            // Use the input path to get a file item.
            FileItem fileItem = new FileItem(filePath);

            // Use the file name to get the files that it creates.
            if (!fileTargets.TryGetValue(fileItem.Name, out var targets))
                return;

            // Loop through the target file names.
            foreach (string newFile in targets)
            {
                // Set up the path to the patch.
                string vcdiffFile = Path.Combine(Config.PatchesPath, newFile + ".vcdiff");

                // Make sure a patch exists.
                if (!vcdiffFile.TestPath())
                    continue;

                // Where the patched file will be temporarily held.
                string patchedFile = Path.Combine(Config.PatchedPath, newFile);

                // The path to move the file. 
                string newFilePath = Path.Combine(fileItem.DirectoryName, newFile);

                // Apply the patch to the file.
                VCDiff.Execute(Operation.Apply, fileItem.FullName, vcdiffFile, patchedFile, newFilePath);
            }
        }

        private static void RestoreOriginalDungeon3Map()
        {
            // Get all possible locations to find the dungeon 3 map and data files.
            string mapsPath  = Path.Combine(Config.DataPath, "Maps");
            string[] d3Files = { "dungeon3.map", "dungeon3_1.map", "dungeon3.map.data", "dungeon3_1.map.data" };

            // Delete all versions of these files.
            foreach (string file in d3Files)
            {
                Path.Combine(mapsPath, file).RemovePath();
                Path.Combine(Config.BackupPath, file).RemovePath();
            }
            // Write the v1.0.0 "dungeon3_1.map" and "dungeon3_1.map.data" files in the "Data\Maps" folder.
            File.WriteAllBytes(Path.Combine(mapsPath, "dungeon3_1.map"), Resources.GetBytes("d3map"));
            File.WriteAllBytes(Path.Combine(mapsPath, "dungeon3_1.map.data"), Resources.GetBytes("d3mapdata"));
        }

        private static void PrepareZeldaLAExecutable()
        {
            // The executable was resolved at initialization.
            Config.ZeldaEXE.RemovePath();

            // Set the v1.0.0 executable from the backup path.
            string backupEXE = Path.Combine(Config.BackupPath, "Link's Awakening DX HD.exe");

            // If it doesn't exist then it most likely had the extension removed.
            if (!backupEXE.TestPath())
                backupEXE = Path.Combine(Config.BackupPath, "Link's Awakening DX HD");

            // Move the backup executable to the base folder.
            backupEXE.MovePath(Config.ZeldaEXE, true);
        }

        public static async Task PatchGameFiles()
        {
            await Task.Run(async () =>
            {
                // The v1.0.0 executable must be in the base path.
                PrepareZeldaLAExecutable();

                // Restore v1.0.0 version of "dungeon3_1.map" and "dungeon3_1.map.data".
                RestoreOriginalDungeon3Map();

                // Build fast lookup of game files (name -> full path).
                BuildGameFileLookup();

                // Get a count of how many files there are.
                _totalCount = _gameFileLookup.Count;

                // Loop through all files in the collection.
                foreach (var kvp in _gameFileLookup)
                {
                    // We don't need a perfect representation of progress just a rough idea of the time left.
                    await UpdateProgress();

                    // Get the filename by key. Trim hashtag character from duplicates (see "BuildGameFileLookup").
                    string fileName = kvp.Key.TrimEnd('#');
                    string fullPath = kvp.Value;

                    // If it's a file that relies on a file with a different name, skip it.
                    if (_targetList.Contains(fileName))
                        continue;

                    // Get the file as a file item which gives us some cool properties to reference.
                    FileItem fileItem = new FileItem(fullPath);

                    // Get the backup path to test for existing backups and create new ones to it.
                    string backupPath = Path.Combine(Config.BackupPath, fileName);
                    string vcDiffFile = Path.Combine(Config.PatchesPath, fileName + ".vcdiff");
                    bool patchExists = vcDiffFile.TestPath();

                    // Default both input and output to the file item path.
                    string inputFile = fullPath;
                    string outputFile = fullPath;

                    // If a patch file exists.
                    if (patchExists)
                    {
                        // If a backup doesn't exist, create one. If one does exist, overwrite the current file with it.
                        if (!backupPath.TestPath())
                            fullPath.CopyPath(backupPath, true);
                        else
                            backupPath.CopyPath(fullPath, true);
                    }

                    // If this file creates other files do so now.
                    if (fileTargets.TryGetValue(fileName, out _))
                        HandleMultiFilePatches(inputFile);

                    // If this file is not patched directly then move on to the next.
                    if (!patchExists)
                        continue;

                    // Patch the file.
                    string patchedFile = Path.Combine(Config.PatchedPath, fileName);
                    VCDiff.Execute(Operation.Apply, inputFile, vcDiffFile, patchedFile, outputFile);
                }
                // There's stuff to do after patching. This function just gathers it all into one method.
                await PostPatchingFunctions();
            });
        }

        public async static Task StartPatching()
        {
            // Reset the progress bar.
            ResetProgress();

            // Start the patching process.
            await PatchGameFiles();

            // Update the game version for finalization functions.
            Config.CurrentVersion = Config.GithubVersion;

            // Run the finalization scripts.
            await HostFinalizationFunctions();
        }
    }
}