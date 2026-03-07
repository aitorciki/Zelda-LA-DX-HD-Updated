using System;
using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using ProjectZ.Base;
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
            var window = Window;
            if (window != null)
            {
                window.AddFlags(WindowManagerFlags.Fullscreen);
                window.AddFlags(WindowManagerFlags.LayoutNoLimits);
                window.ClearFlags(WindowManagerFlags.ForceNotFullscreen);

                if (OperatingSystem.IsAndroidVersionAtLeast(28) && window.Attributes is { } attributes)
                    attributes.LayoutInDisplayCutoutMode = LayoutInDisplayCutoutMode.ShortEdges;
            }

            base.OnCreate(savedInstanceState);

            var root = Application.Context.GetExternalFilesDir(null)!.AbsolutePath;

            // Point Values to a writable location on Android.
            Values.SetUserDataRoot(root);

            // Ensure folders exist.
            System.IO.Directory.CreateDirectory(Values.PathMods);
            System.IO.Directory.CreateDirectory(Values.PathLAHDMods);
            System.IO.Directory.CreateDirectory(Values.PathGraphicsMods);
            System.IO.Directory.CreateDirectory(Values.PathSaveFolder);

            // Get real display size for proper fullscreen rendering.
            var surfaceWidth = 0;
            var surfaceHeight = 0;

            if (OperatingSystem.IsAndroidVersionAtLeast(30))
            {
                var metrics = WindowManager?.CurrentWindowMetrics;
                var bounds = metrics?.Bounds;
                if (bounds != null)
                {
                    surfaceWidth = bounds.Width();
                    surfaceHeight = bounds.Height();
                }
            }
            else
            {
                var display = WindowManager?.DefaultDisplay;
                if (display != null)
                {
                    var size = new global::Android.Graphics.Point();
#pragma warning disable CS0618
                    display.GetRealSize(size);
#pragma warning restore CS0618
                    surfaceWidth = size.X;
                    surfaceHeight = size.Y;
                }
            }

            if (surfaceWidth > 0 && surfaceHeight > 0)
            {
                // Ensure landscape orientation (wider dimension first).
                if (surfaceWidth < surfaceHeight)
                {
                    var swap = surfaceWidth;
                    surfaceWidth = surfaceHeight;
                    surfaceHeight = swap;
                }
                Game1.SetAndroidSurfaceSizeHint(surfaceWidth, surfaceHeight);
            }

            // construct your real game here:
            var game = new Game1(
                editorMode: false,
                loadSave: false,
                loadSlot: 0
            );
            game.Services.AddService(typeof(AssetManager), Assets);

            var view = (View)game.Services.GetService(typeof(View))!;
            var matchParent = new ViewGroup.LayoutParams(
                ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.MatchParent);
            view.LayoutParameters = matchParent;
            SetContentView(view, matchParent);

            ApplyFullscreenFlags();

            view.Focusable = true;
            view.FocusableInTouchMode = true;
            view.RequestFocus();
            game.Run();
        }

        private void ApplyFullscreenFlags()
        {
            var window = Window;
            if (window == null)
                return;

            if (OperatingSystem.IsAndroidVersionAtLeast(30))
            {
                window.SetDecorFitsSystemWindows(false);
                var controller = window.InsetsController;
                if (controller != null)
                {
                    controller.Hide(global::Android.Views.WindowInsets.Type.StatusBars() |
                                   global::Android.Views.WindowInsets.Type.NavigationBars());
                    controller.SystemBarsBehavior =
                        (int)global::Android.Views.WindowInsetsControllerBehavior.ShowTransientBarsBySwipe;
                }
            }
            else
            {
                var decorView = window.DecorView;
                if (decorView == null)
                    return;

#pragma warning disable CS0618
                decorView.SystemUiVisibility =
                    (StatusBarVisibility)(
                        SystemUiFlags.LayoutStable |
                        SystemUiFlags.LayoutHideNavigation |
                        SystemUiFlags.LayoutFullscreen |
                        SystemUiFlags.HideNavigation |
                        SystemUiFlags.Fullscreen |
                        SystemUiFlags.ImmersiveSticky);
#pragma warning restore CS0618
            }
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);
            if (hasFocus)
                ApplyFullscreenFlags();
        }

        public override bool DispatchKeyEvent(KeyEvent? e)
        {
            if (e == null)
                return base.DispatchKeyEvent(e);

            // Only treat "down" as a press (avoid repeats).
            if (e.Action == KeyEventActions.Down && e.RepeatCount == 0)
            {
                // Catch a wider net than just Back/Select.
                if (e.KeyCode == Keycode.Back ||
                    e.KeyCode == Keycode.ButtonSelect ||
                    e.KeyCode == Keycode.ButtonMode ||
                    e.KeyCode == Keycode.Menu ||
                    e.KeyCode == Keycode.Escape) // many controllers map select/back to ESC
                {
                    PlatformInput.SelectPressed = true;
                    return true;
                }
            }
            return base.DispatchKeyEvent(e);
        }

        public override void OnBackPressed()
        {
            PlatformInput.SelectPressed = true;
        }
    }
}