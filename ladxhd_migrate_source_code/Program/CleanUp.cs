using System.IO;
using System.Collections.Generic;

namespace LADXHD_Migrater
{
    internal class CleanUp
    {
        public static List<string>miniMapData = new List<string>();
        public static List<string>removeMaps = new List<string>();

        public static void Init()
        {
            // Junk minimap data files in Data\Dungeon that are no longer needed.
            miniMapData.Add("three_1.txt");
            miniMapData.Add("three_2.txt");
            miniMapData.Add("three_3.txt");

            // Junk maps from the Data\Maps folder.
            removeMaps.Add("0 test map - barrier.map");
            removeMaps.Add("0 test map - barrier.map.data");
            removeMaps.Add("0 test map - beamos.map");
            removeMaps.Add("0 test map - beamos.map.data");
            removeMaps.Add("0 test map - blured cave.map");
            removeMaps.Add("0 test map - blured cave.map.data");
            removeMaps.Add("0 test map - color jump tile.map");
            removeMaps.Add("0 test map - color jump tile.map.data");
            removeMaps.Add("0 test map - Copy - Copy.map");
            removeMaps.Add("0 test map - Copy - Copy.map.data");
            removeMaps.Add("0 test map - Copy (2).map");
            removeMaps.Add("0 test map - Copy (2).map.data");
            removeMaps.Add("0 test map - Copy.map.data");
            removeMaps.Add("0 test map - dialog.map");
            removeMaps.Add("0 test map - dialog.map.data");
            removeMaps.Add("0 test map - enemies - damage - holes.map");
            removeMaps.Add("0 test map - enemies - damage - holes.map.data");
            removeMaps.Add("0 test map - enemies - damage.map");
            removeMaps.Add("0 test map - enemies - damage.map.data");
            removeMaps.Add("0 test map - enemies small.map");
            removeMaps.Add("0 test map - enemies small.map.data");
            removeMaps.Add("0 test map - enemies.map");
            removeMaps.Add("0 test map - enemies.map.data");
            removeMaps.Add("0 test map - fence.map");
            removeMaps.Add("0 test map - fence.map.data");
            removeMaps.Add("0 test map - floor layer.map");
            removeMaps.Add("0 test map - floor layer.map.data");
            removeMaps.Add("0 test map - gbs test.map");
            removeMaps.Add("0 test map - gbs test.map.data");
            removeMaps.Add("0 test map - holes.map");
            removeMaps.Add("0 test map - holes.map.data");
            removeMaps.Add("0 test map - hookshot.map");
            removeMaps.Add("0 test map - hookshot.map.data");
            removeMaps.Add("0 test map - item test.map");
            removeMaps.Add("0 test map - item test.map.data");
            removeMaps.Add("0 test map - kirby.map");
            removeMaps.Add("0 test map - kirby.map.data");
            removeMaps.Add("0 test map - levels.map");
            removeMaps.Add("0 test map - levels.map.data");
            removeMaps.Add("0 test map - mermaid.map");
            removeMaps.Add("0 test map - mermaid.map.data");
            removeMaps.Add("0 test map - music test.map");
            removeMaps.Add("0 test map - music test.map.data");
            removeMaps.Add("0 test map - new draw pool.map");
            removeMaps.Add("0 test map - new draw pool.map.data");
            removeMaps.Add("0 test map - qicksand.map");
            removeMaps.Add("0 test map - qicksand.map.data");
            removeMaps.Add("0 test map - raft.map");
            removeMaps.Add("0 test map - raft.map.data");
            removeMaps.Add("0 test map - rail jump.map");
            removeMaps.Add("0 test map - rail jump.map.data");
            removeMaps.Add("0 test map - sequence.map");
            removeMaps.Add("0 test map - sequence.map.data");
            removeMaps.Add("0 test map - sprites.map");
            removeMaps.Add("0 test map - sprites.map.data");
            removeMaps.Add("0 test map - stop music test.map");
            removeMaps.Add("0 test map - stop music test.map.data");
            removeMaps.Add("0 test map - transition bug.map");
            removeMaps.Add("0 test map - transition bug.map.data");
            removeMaps.Add("0 test map - turtle head.map");
            removeMaps.Add("0 test map - turtle head.map.data");
            removeMaps.Add("0 test map - walrus.map");
            removeMaps.Add("0 test map - walrus.map.data");
            removeMaps.Add("0 test map color jump tile.map.data");
            removeMaps.Add("0 test map levels.map.data");
            removeMaps.Add("0 test map new draw pool.map.data");
            removeMaps.Add("0 test map qicksand.map.data");
            removeMaps.Add("0 test map.map");
            removeMaps.Add("0 test map.map.data");
            removeMaps.Add("cave bird.map.data");
            removeMaps.Add("dungeon 7_2d.map.data");
            removeMaps.Add("dungeon_end.map.data");
            removeMaps.Add("dungeon3_1.map");
            removeMaps.Add("dungeon3_1.map.data");
            removeMaps.Add("dungeon3_2.map");
            removeMaps.Add("dungeon3_2.map.data");
            removeMaps.Add("dungeon3_3.map");
            removeMaps.Add("dungeon3_3.map.data");
            removeMaps.Add("dungeon3_4.map");
            removeMaps.Add("dungeon3_4.map.data");
        }

        public static void RemoveJunkMapFiles()
        {
            foreach (string file in miniMapData)
            {
                string CurrentFile = Path.Combine(Config.Update_Data, "Dungeon", file);
                CurrentFile.RemovePath();
            }

            foreach (string file in removeMaps)
            {
                string CurrentFile = Path.Combine(Config.Update_Data, "Maps", file);
                CurrentFile.RemovePath();
            }
        }
    }
}
