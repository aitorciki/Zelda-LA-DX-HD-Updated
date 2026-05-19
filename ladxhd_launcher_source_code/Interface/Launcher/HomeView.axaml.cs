using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform;

namespace LADXHD_Launcher;

public partial class HomeView : UserControl
{
    private MainWindow? _parent;

    public HomeView()
    {
        InitializeComponent();
    }

    public HomeView(MainWindow parent)
    {
        InitializeComponent();
        SoundToggle_SetImage();
        _parent = parent;

        Config.UpdateAvailable = () =>
        {
            // Must update UI on the UI thread since CheckForUpdateAsync runs async
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                UpdateButton.IsVisible = true;
            });
        };
    }

    private string GetGameDirectory()
    {
        return AppContext.BaseDirectory;
    }

    private void SoundToggle_SetImage()
    {
        SoundButtonImage.Source = SoundPlayer.Enabled
            ? new Avalonia.Media.Imaging.Bitmap(
                AssetLoader.Open(new Uri("avares://Launcher/Resources/sound_on.png")))
            : new Avalonia.Media.Imaging.Bitmap(
                AssetLoader.Open(new Uri("avares://Launcher/Resources/sound_off.png")));
    }

    public void ForceUpdate()
    {
        // Forces an update by pressing Shift + F1.
        UpdateButton.IsVisible = true;
        UpdateButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
    }

    private async void UpdateButton_Click(object sender, RoutedEventArgs e)
    {
        // Ask the user if they really want to update.
        bool confirmed = await YesNoWindow.ShowAsync("Update Game", $"Update the game to {Config.GithubVersion}?");

        // If the pressed "No" then do nothing.
        if (!confirmed) return;

        // Show the progress window.
        var progress = await ProgressWindow.ShowAsync("Updating Game", "Downloading launcher...");

        try
        {
            // Set up the temporary folders.
            Config.TempPath = Path.Combine(Config.RootPath, "~temp").CreatePath();
            Config.PatchesPath = Path.Combine(Config.TempPath, "patches").CreatePath();
            Config.PatchedPath = Path.Combine(Config.TempPath, "patchedFiles").CreatePath();

            // Download the new launcher.
            progress.UpdateStatus("Downloading launcher...");
            progress.UpdateProgressBar(0);
            string launcherZip = Github.GetLauncherZipName();
            string launcherPath = Path.Combine(Config.TempPath, launcherZip);
            var downloadProg = new Progress<int>(v => progress.UpdateProgressBar(v));
            await Github.DownloadFileAsync(launcherZip, launcherPath, downloadProg);

            // Replace launcher with the new version.
            progress.UpdateStatus("Installing launcher...");
            Patcher_Functions.ReplaceLauncher(launcherPath);

            // Download the patches to the temp folder.
            progress.UpdateStatus("Downloading patches...");
            progress.UpdateProgressBar(0);
            string patchZip = Github.GetPatchesZipName();
            string zipPath = Path.Combine(Config.TempPath, patchZip);
            await Github.DownloadFileAsync(patchZip, zipPath, downloadProg);

            // Extract the patches to temp patches folder.
            progress.UpdateStatus("Extracting patches...");
            progress.UpdateProgressBar(0);
            ZipFile.ExtractToDirectory(zipPath, Config.PatchesPath);

            // Apply the patches.
            progress.UpdateStatus("Applying patches...");
            progress.UpdateProgressBar(0);
            Config.ActiveWindow = progress;
            await Patcher_Functions.StartPatching();

            // Finish up and close out the window.
            progress.Finish();
            progress.CloseWindow();

            // Cleanup the temporary folder.
            if (Directory.Exists(Config.TempPath))
                Directory.Delete(Config.TempPath, true);

            // Show the confirmation window.
            string message = $"The game has been updated to {Config.GithubVersion}. Please restart the launcher.";
            await OkayWindow.ShowAsync("Update Complete", message, 20, true);

            // Shut the patcher down so the user is forced to restart it.
            App.MainWindowInstance.Close();
        }
        catch (Exception ex)
        {
            // If an error happened close the progress window.
            progress.CloseWindow();

            // Attempt to remove the temp folder.
            try 
            {
                if (Directory.Exists(Config.TempPath)) Directory.Delete(Config.TempPath, true); 
            } 
            catch { }

            // Show the reason for failure to the user.
            await OkayWindow.ShowAsync("Update Failed", ex.Message);
        }
    }

    private void SoundToggle_Click(object sender, RoutedEventArgs e)
    {
        SoundPlayer.Enabled = !SoundPlayer.Enabled;
        Config.SaveLauncherConfig();
        SoundToggle_SetImage();
    }

    private void PlayButton_Click(object sender, RoutedEventArgs e)
    {
        if (!File.Exists(Config.ZeldaEXE))
        {
            return;
        }

        Process.Start(new ProcessStartInfo
        {
            FileName = Config.ZeldaEXE,
            WorkingDirectory = Config.RootPath,
            UseShellExecute = true
        });
        _parent?.Close();
    }

    private void SettingsButton_Click(object sender, RoutedEventArgs e)
    {
        int maxGameScale = AdvancedSettings.LoadMaxGameScale(GetGameDirectory());
        GameSettings.Load(GetGameDirectory());
        _parent?.SettingsView.LoadValues(maxGameScale);
        _parent?.NavigateTo(_parent.SettingsView);
        SoundPlayer.PlayXnbSound(SoundPlayer.SoundOpen);
    }

    private async void ModsButton_Click(object sender, RoutedEventArgs e)
    {
        _parent?.HideNotifications();
        _parent?.ShowLoadingMessage();

        await System.Threading.Tasks.Task.Run(() =>
        {
            AdvancedSettings.Load(AppContext.BaseDirectory);
        });

        _parent?.ModsView.LoadValues();
        _parent?.HideLoadingMessage();
        _parent?.NavigateTo(_parent.ModsView);

        // Wait for the UI to actually render before playing sound
        await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(
            () => SoundPlayer.PlayXnbSound(SoundPlayer.SoundOpen),
            Avalonia.Threading.DispatcherPriority.Background);
    }

    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        _parent?.Close();
    }
}