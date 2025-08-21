using System.IO;
using System.Reflection;

namespace LADXHD_Migrater
{
    internal class Config
    {
        public static string appPath;
        public static string baseFolder;
        public static string patches;
        public static string orig_Content;
        public static string orig_Data;
        public static string game_source;
        public static string update_Content;
        public static string update_Data;
        public static string publish_Path;

        public static void Initialize()
        {
            appPath        = Assembly.GetExecutingAssembly().Location;
            baseFolder     = Path.GetDirectoryName(appPath);
            patches        = baseFolder + "\\assets_patches";
            orig_Content   = baseFolder + "\\assets_original\\Content";
            orig_Data      = baseFolder + "\\assets_original\\Data";
            game_source    = baseFolder + "\\ladxhd_game_source_code";
            update_Content = game_source + "\\Content";
            update_Data    = game_source + "\\Data";
            publish_Path   = game_source + "\\Publish";
        }
    }
}
