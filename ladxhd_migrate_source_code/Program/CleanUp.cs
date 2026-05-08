using System.IO;

namespace LADXHD_Migrater
{
    internal class CleanUp
    {
        private static readonly string[] miniMapData =
        {
            "three_1.txt",
            "three_2.txt",
            "three_3.txt"
        };

       private static readonly string[] removeMaps =
        {
            "cave bird.map.data",
            "dungeon 7_2d.map.data",
            "dungeon_end.map.data",
        };

        private static readonly string[] removeMapPatterns =
        {
            "0 test map*",
            "dungeon3_*"
        };

        public static void RemoveJunkMapFiles()
        {
            string mapsPath = Path.Combine(Config.Update_Data, "Maps");

            foreach (string file in miniMapData)
                Path.Combine(Config.Update_Data, "Dungeon", file).RemovePath();

            foreach (string file in removeMaps)
                Path.Combine(mapsPath, file).RemovePath();

            foreach (string pattern in removeMapPatterns)
                foreach (string file in mapsPath.GetFiles(pattern))
                    file.RemovePath();
        }
    }
}