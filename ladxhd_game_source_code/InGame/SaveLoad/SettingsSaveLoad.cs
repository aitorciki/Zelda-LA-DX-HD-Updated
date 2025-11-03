using ProjectZ.InGame.Controls;
using ProjectZ.InGame.Things;

namespace ProjectZ.InGame.SaveLoad
{
    class SettingsSaveLoad
    {
        private static readonly string SettingsFilePath = SaveManager.GetSettingsFile();

        public static void LoadSettings()
        {
            if (new SaveManager() is not { } saveManager || !saveManager.LoadFile(SettingsFilePath))
                return;

            // Game Settings
            Game1.LanguageManager.CurrentLanguageIndex = saveManager.GetInt("CurrentLanguage", Game1.LanguageManager.CurrentLanguageIndex);
            Game1.LanguageManager.CurrentSubLanguageIndex = saveManager.GetInt("CurrentSubLanguage", Game1.LanguageManager.CurrentSubLanguageIndex);
            GameSettings.Controller = saveManager.GetString("Controller", GameSettings.Controller);
            GameSettings.SwapButtons = saveManager.GetBool("SwapButtons", GameSettings.SwapButtons);
            GameSettings.Autosave = saveManager.GetBool("Autosave", GameSettings.Autosave);
            GameSettings.ItemsOnRight = saveManager.GetBool("ItemsOnRight", GameSettings.ItemsOnRight);
            GameSettings.OldMovement = saveManager.GetBool("OldMovement", GameSettings.OldMovement);

            // Video Settings
            GameSettings.GameScale = saveManager.GetInt("GameScale", GameSettings.GameScale);
            GameSettings.UiScale = saveManager.GetInt("UIScale", GameSettings.UiScale);
            GameSettings.IsFullscreen = saveManager.GetBool("IsFullscreen", GameSettings.IsFullscreen);
            GameSettings.ExFullscreen = saveManager.GetBool("ExFullscreen", GameSettings.ExFullscreen);
            GameSettings.EnableShadows = saveManager.GetBool("EnableShadows", GameSettings.EnableShadows);
            GameSettings.VerticalSync = saveManager.GetBool("VerticalSync", GameSettings.VerticalSync);
            GameSettings.ScreenShake = saveManager.GetBool("ScreenShake", GameSettings.ScreenShake);
            GameSettings.EpilepsySafe = saveManager.GetBool("EpilepsySafe", GameSettings.EpilepsySafe);

            // Audio Settings
            GameSettings.MusicVolume = saveManager.GetInt("MusicVolume", GameSettings.MusicVolume);
            GameSettings.EffectVolume = saveManager.GetInt("EffectVolume", GameSettings.EffectVolume);
            GameSettings.MuteInactive = saveManager.GetBool("MuteInactive", GameSettings.MuteInactive);
            GameSettings.HeartBeep = saveManager.GetBool("HeartBeep", GameSettings.HeartBeep);
            GameSettings.MutePowerups = saveManager.GetBool("MutePowerups", GameSettings.MutePowerups);

            // Camera Settings
            GameSettings.CameraLock = saveManager.GetBool("CameraLock", GameSettings.CameraLock);
            GameSettings.SmoothCamera = saveManager.GetBool("SmoothCamera", GameSettings.SmoothCamera);
            GameSettings.ClassicCamera = saveManager.GetBool("ClassicCamera", GameSettings.ClassicCamera);
            GameSettings.ClassicBorder = saveManager.GetBool("ClassicBorder", GameSettings.ClassicBorder);
            GameSettings.ClassicAlpha = saveManager.GetFloat("ClassicAlpha", GameSettings.ClassicAlpha);

            // Redux Settings
            GameSettings.MenuBorder = saveManager.GetInt("MenuBorder", GameSettings.MenuBorder);
            GameSettings.VarWidthFont = saveManager.GetBool("VarWidthFont", GameSettings.VarWidthFont);
            GameSettings.NoHelperText = saveManager.GetBool("NoHelperText", GameSettings.NoHelperText);
            GameSettings.DialogSkip = saveManager.GetBool("DialogSkip", GameSettings.DialogSkip);
            GameSettings.Uncensored = saveManager.GetBool("Uncensored", GameSettings.Uncensored);
            GameSettings.Unmissables = saveManager.GetBool("Unmissables", GameSettings.Unmissables);
            GameSettings.PhotosColor = saveManager.GetBool("PhotosColor", GameSettings.PhotosColor);
            GameSettings.NoAnimalDamage = saveManager.GetBool("NoAnimalDamage", GameSettings.NoAnimalDamage);

            // Modifiers Settings
            GameSettings.EnemyBonusHP = saveManager.GetInt("EnemyBonusHP", GameSettings.EnemyBonusHP);
            GameSettings.DmgMultiplier = saveManager.GetInt("DmgMultiplier", GameSettings.DmgMultiplier);
            GameSettings.MoveSpeedAdded = saveManager.GetFloat("MoveSpeedAdded", GameSettings.MoveSpeedAdded);
            GameSettings.NoDamageLaunch = saveManager.GetBool("NoDamageLaunch", GameSettings.NoDamageLaunch);
            GameSettings.NoHeartDrops = saveManager.GetBool("NoHeartDrops", GameSettings.NoHeartDrops);
            GameSettings.SwordBlock = saveManager.GetBool("SwordBlock", GameSettings.SwordBlock);

            // Controller stuff: Deadzone is currently not configurable.
            Values.ControllerDeadzone = saveManager.GetFloat("ControllerDeadzone", Values.ControllerDeadzone);
            ControlHandler.LoadButtonMap(saveManager);
            ControlHandler.SetControllerIndex();
        }

        public static void SaveSettings()
        {
            var saveManager = new SaveManager();

            // Game Settings
            saveManager.SetInt("CurrentLanguage", Game1.LanguageManager.CurrentLanguageIndex);
            saveManager.SetInt("CurrentSubLanguage", Game1.LanguageManager.CurrentSubLanguageIndex);
            saveManager.SetString("Controller", GameSettings.Controller);
            saveManager.SetBool("SwapButtons", GameSettings.SwapButtons);
            saveManager.SetBool("Autosave", GameSettings.Autosave);
            saveManager.SetBool("ItemsOnRight", GameSettings.ItemsOnRight);
            saveManager.SetBool("OldMovement", GameSettings.OldMovement);

            // Video Settings
            saveManager.SetInt("GameScale", GameSettings.GameScale);
            saveManager.SetInt("UIScale", GameSettings.UiScale);
            saveManager.SetBool("IsFullscreen", GameSettings.IsFullscreen);
            saveManager.SetBool("ExFullscreen", GameSettings.ExFullscreen);
            saveManager.SetBool("EnableShadows", GameSettings.EnableShadows);
            saveManager.SetBool("VerticalSync", GameSettings.VerticalSync);
            saveManager.SetBool("ScreenShake", GameSettings.ScreenShake);
            saveManager.SetBool("EpilepsySafe", GameSettings.EpilepsySafe);

            // Audio Settings
            saveManager.SetInt("MusicVolume", GameSettings.MusicVolume);
            saveManager.SetInt("EffectVolume", GameSettings.EffectVolume);
            saveManager.SetBool("MuteInactive", GameSettings.MuteInactive);
            saveManager.SetBool("HeartBeep", GameSettings.HeartBeep);
            saveManager.SetBool("MutePowerups", GameSettings.MutePowerups);

            // Camera Settings
            saveManager.SetBool("CameraLock", GameSettings.CameraLock);
            saveManager.SetBool("SmoothCamera", GameSettings.SmoothCamera);
            saveManager.SetBool("ClassicCamera", GameSettings.ClassicCamera);
            saveManager.SetBool("ClassicBorder", GameSettings.ClassicBorder);
            saveManager.SetFloat("ClassicAlpha", GameSettings.ClassicAlpha);

            // Redux Settings
            saveManager.SetInt("MenuBorder", GameSettings.MenuBorder);
            saveManager.SetBool("VarWidthFont", GameSettings.VarWidthFont);
            saveManager.SetBool("NoHelperText", GameSettings.NoHelperText);
            saveManager.SetBool("DialogSkip", GameSettings.DialogSkip);
            saveManager.SetBool("Uncensored", GameSettings.Uncensored);
            saveManager.SetBool("Unmissables", GameSettings.Unmissables);
            saveManager.SetBool("PhotosColor", GameSettings.PhotosColor);
            saveManager.SetBool("NoAnimalDamage", GameSettings.NoAnimalDamage);

            // Modifiers Settings
            saveManager.SetInt("EnemyBonusHP", GameSettings.EnemyBonusHP);
            saveManager.SetInt("DmgMultiplier", GameSettings.DmgMultiplier);
            saveManager.SetFloat("MoveSpeedAdded", GameSettings.MoveSpeedAdded);
            saveManager.SetBool("NoDamageLaunch", GameSettings.NoDamageLaunch);
            saveManager.SetBool("NoHeartDrops", GameSettings.NoHeartDrops);
            saveManager.SetBool("SwordBlock", GameSettings.SwordBlock);

            // Controller stuff: Deadzone is currently not configurable.
            saveManager.SetFloat("ControllerDeadzone", Values.ControllerDeadzone);
            ControlHandler.SaveButtonMaps(saveManager);

            // Write the save file.
            saveManager.Save(SettingsFilePath, Values.SaveRetries);
        }
    }
}