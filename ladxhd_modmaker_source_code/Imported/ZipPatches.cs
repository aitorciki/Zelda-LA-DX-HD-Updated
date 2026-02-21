using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace LADXHD_ModMaker
{
    public class ZipPatches
    {
        private static Dictionary<string, object> resources = ResourceHelper.GetAllResources();

        public static void ExtractZipFile()
        {
            // Set the patches and zipfile paths.
            string patchesPath = (Config.TempPath + "\\patches").CreatePath();
            string patchedPath = (Config.TempPath + "\\patchedFiles").CreatePath();
            string zipFilePath = Path.Combine(Config.TempPath, "patches.zip");

            // Write the zipfile, extract it, then delete it.
            File.WriteAllBytes(zipFilePath, (byte[])resources["patches.zip"]);
            ZipFile.ExtractToDirectory(zipFilePath, patchesPath);
            zipFilePath.RemovePath();
        }
    }
}
