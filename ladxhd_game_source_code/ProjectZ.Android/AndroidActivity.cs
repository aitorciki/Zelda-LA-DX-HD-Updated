using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using ProjectZ.InGame.Things;

namespace ProjectZ.Android
{
    [Activity(
        Label = "@string/app_name",
        MainLauncher = false ,
        Theme = "@style/Theme.Game",
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.FullSensor,
        ConfigurationChanges =
            ConfigChanges.Orientation |
            ConfigChanges.ScreenSize |
            ConfigChanges.KeyboardHidden |
            ConfigChanges.UiMode)]

    public class MainActivity : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var root = Application.Context.GetExternalFilesDir(null)!.AbsolutePath;

            // Point Values to a writable location on Android.
            Values.SetUserDataRoot(root);

            // Ensure folders exist.
            System.IO.Directory.CreateDirectory(Values.PathMods);
            System.IO.Directory.CreateDirectory(Values.PathLAHDMods);
            System.IO.Directory.CreateDirectory(Values.PathGraphicsMods);

            // construct your real game here:
            var game = new Game1(
                editorMode: false,
                loadSave: false,
                loadSlot: 0
            );
            game.Services.AddService(typeof(AssetManager), Assets);

            var view = (View)game.Services.GetService(typeof(View))!;
            SetContentView(view);

            game.Run();
        }
    }
}