
namespace ProjectZ.InGame.Things
{
    class GameSettings
    {
        public static int GameScale = 11;
        public static int UiScale = 4;
        private static int _musicVolume = 100;
        private static int _effectVolume = 100;
        public static bool MuteInactive = true;
        public static bool MutePowerups = false;
        public static bool EnableShadows  = true;
        public static bool Autosave = true;
        public static bool HeartBeep = true;
        public static bool ScreenShake = true;
        public static bool SmoothCamera = true;
        public static bool IsFullscreen = false;
        public static bool ExFullscreen = false;
        public static bool LockFps = true;
        public static bool ItemsOnRight = false;
        public static string Controller = "XBox";
        public static bool OldMenuBorder = false;
        public static bool VarWidthFont = false;
        public static bool NoHelperText = false;
        public static bool Uncensored = false;

        public static int MusicVolume
        {
            get => _musicVolume;
            set { _musicVolume = value; Game1.GbsPlayer.SetVolume(value / 100.0f); }
        }

        public static int EffectVolume
        {
            get => _effectVolume;
            set { _effectVolume = value; }
        }

    }
}