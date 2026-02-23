using System;
using ProjectZ.InGame.Things;

namespace ProjectZ
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
#if !WINDOWS
            Environment.SetEnvironmentVariable("MONOGAME_SDL_VIDEO_ALLOW_HIGHDPI", "1");
#endif
            var editorMode = false;


            var loadSave = false;
            var saveSlot = 0;

            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];

                if (arg.Equals("editor", StringComparison.OrdinalIgnoreCase))
                {
                    editorMode = true;
                }
                else if (arg.Equals("loadSave", StringComparison.OrdinalIgnoreCase))
                {
                    loadSave = true;

                    if (i + 1 < args.Length && int.TryParse(args[i + 1], out int parsedSlot))
                    {
                        saveSlot = parsedSlot;
                        i++;
                    }
                }
                else if (arg.Equals("exclusive", StringComparison.OrdinalIgnoreCase))
                {
                    GameSettings.ExFullscreen = true;
                }
            }

            try
            {
                using (var game = new Game1(editorMode, loadSave, saveSlot))
                    game.Run();
            }

            catch (Exception exception)
            {
                NativeDialogs.ShowError(exception.Message, exception.StackTrace);
                throw;
            }
        }
    }
}