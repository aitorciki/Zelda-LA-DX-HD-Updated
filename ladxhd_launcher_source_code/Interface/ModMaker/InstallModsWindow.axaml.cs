using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using static LADXHD_Launcher.ModMaker_Functions;

namespace LADXHD_Launcher
{
    public partial class InstallModsWindow : Window, IProgressWindow
    {
        public void UpdateProgressBar(int value)
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() => progressBar.Value = value);
        }

        public InstallModsWindow()
        {
            InitializeComponent();

            // When the window is closed delete the temp folder.
            this.Closed += (_, _) =>
            {
                if (Directory.Exists(Config.TempFolder))
                    Directory.Delete(Config.TempFolder, true);
            };

            // If there is no INI file there is no mod to install.
            string IniPath = Path.Combine(Config.TempFolder, "LAHDMOD.ini");
            if (!IniPath.TestPath())
                this.Close();

            // Load the INI file.
            LADXHD_IniFile.Initialize(IniPath);
            LADXHD_IniFile.LoadINIValues();

            // Load the values from the INI file.
            Title = Config.ModName;
            ModNameLabel.Text = Config.ModName;
            ModDescBox.Text = Config.Description.Replace("\\n", "\n");

            // Test for an image to display.
            string[] validExt = { ".png", ".jpg", ".bmp" };
            string imagePath = "";

            // If the extensions is in the list then set the image path.
            foreach (string ext in validExt) 
            {
                string testImage = Path.Combine(Config.TempFolder, "Image" + ext);

                if (File.Exists(testImage))
                {
                    imagePath = testImage;
                    break;
                }
            }
            // Load in the image if it exists.
            if (imagePath.TestPath())
                ImageBox.Source = new Bitmap(imagePath);
        }

        private void EnableComponents(bool toggle)
        {
            ModDescBox.IsEnabled       = toggle;
            InstallModButton.IsEnabled = toggle;
            ExitButton.IsEnabled       = toggle;
        }

/*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        BUTTONS

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        private async void InstallModButton_Click(object sender, RoutedEventArgs e)
        {
            var okayWindow = new OkayWindow();
            string message;

            // The game executable was not found.
            string GameExePath = Config.GetGameExecutable(Config.BaseFolder);
            if (!Config.BaseFolder.TestPath(true) || !GameExePath.TestPath())
            {
                message = "The Game Path must point to a valid installation that contains the file \"Link's Awakening DX HD.exe\".";
                okayWindow.Display("Game Path Invalid", message, timeoutSeconds: 10);
                SoundPlayer.PlayWAVSound("avares://Launcher/Resources/beep.wav");
                await okayWindow.ShowDialog(this);
                return;
            }

            // Disable form, apply patches, enable form.
            EnableComponents(false);
            await Task.Run(() => ModMaker_Functions.ApplyModPatches());
            EnableComponents(true);

            // Display that it's done.
            message = "Finished applying patches. Any \"Graphics\" mods or \"LAHDMods\" were generated in the \"Mods\" folder.";
            okayWindow.Display("Mod Installed", message, timeoutSeconds: 10);
            SoundPlayer.PlayWAVSound("avares://Launcher/Resources/success.wav");
            await okayWindow.ShowDialog(this);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}