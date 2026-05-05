using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using NativeFileDialogSharp;
using static LADXHD_Launcher.ModMaker_Functions;

namespace LADXHD_Launcher
{
    public partial class CreateModsWindow : Window, IProgressWindow
    {
        public void UpdateProgressBar(int value)
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() => progressBar.Value = value);
        }

        public CreateModsWindow()
        {
            InitializeComponent();

            // I'm not sure how to register these events in the XML so just do it here.
            ImageFileBox.AddHandler(DragDrop.DragOverEvent, ImageFileBox_DragOver);
            ImageFileBox.AddHandler(DragDrop.DropEvent, ImageFileBox_Drop);
            OutputPathBox.AddHandler(DragDrop.DragOverEvent, OutputPathBox_DragOver);
            OutputPathBox.AddHandler(DragDrop.DropEvent, OutputPathBox_Drop);

            // Update the title with the current version.
            this.Title = "Link's Awakening DX HD Mod Maker v" + Config.Version;
        }

        private void EnableComponents(bool toggle)
        {
            ModNameBox.IsEnabled       = toggle;
            ModDescBox.IsEnabled       = toggle;
            ImageFileBox.IsEnabled     = toggle;
            ImageFileButton.IsEnabled  = toggle;
            OutputPathBox.IsEnabled    = toggle;
            OutputPathButton.IsEnabled = toggle;
            CreateModButton.IsEnabled  = toggle;
            ModsGitHubButton.IsEnabled = toggle;
            ExitButton.IsEnabled       = toggle;
        }

/*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        MOD NAME

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        private void ModNameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Update the mod's name when leaving the textbox.
            Config.ModName = ModNameBox.Text;
        }

/*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        MOD DESCRIPTION

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        private void ModDescBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // If the string is empty return.
            if (string.IsNullOrEmpty(ModDescBox.Text))
            {
                Config.Description = "";
                return;
            }
            // Update the mod's description when leaving the textbox.
            Config.Description = ModDescBox.Text.Replace("\r\n", "\\n").Replace("\n", "\\n");
        }

/*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        IMAGE FILE PATH

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        private void ImageFileBox_DragOver(object sender, DragEventArgs e)
        {
            e.DragEffects = e.Data.Contains(DataFormats.FileNames) ? DragDropEffects.Copy : DragDropEffects.None;
        }
        private void ImageFileBox_Drop(object sender, DragEventArgs e)
        {
            // If there is no data then exit early.
            if (!e.Data.Contains(DataFormats.FileNames))
                return;

            // Get the first file in the drop stack.
            var file = e.Data.GetFileNames()?.FirstOrDefault();

            // Update the image file.
            ImageFileFinish(file);
        }
        private void ImageFileBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Get the text from the box.
            string text = ImageFileBox.Text;

            // Update the image file.
            ImageFileFinish(text);
        }
        private void ImageFileButton_Click(object sender, RoutedEventArgs e)
        {
            // Open a dialog to select an image file.
            DialogResult? selected = Dialog.FileOpen("png");
            if (!selected.IsOk)
                return;
            string path = selected.Path;

            // Update the image file.
            ImageFileFinish(path);
        }
        private void ImageFileFinish(string path)
        {
            // If the path is invalid reset the text.
            if (string.IsNullOrEmpty(path) || !path.TestPath())
            {
                ImageFileBox.Text = Config.ImagePath;
                return;
            }
            // Get the file as an item.
            FileItem fileItem = new FileItem(path);

            // The image must be a PNG image.
            if (fileItem.Extension != ".png")
                return;

            // Update the path and textbox.
            Config.ImagePath = path;
            ImageFileBox.Text = path;
        }

/*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        OUTPUT PATH

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/


        private void OutputPathBox_DragOver(object sender, DragEventArgs e)
        {
            e.DragEffects = e.Data.Contains(DataFormats.FileNames) ? DragDropEffects.Copy : DragDropEffects.None;
        }
        private void OutputPathBox_Drop(object sender, DragEventArgs e)
        {
            // If there is no data then exit early.
            if (!e.Data.Contains(DataFormats.FileNames))
                return;

            // Get the path that was dropped.
            string folder = e.Data.GetFileNames()?.FirstOrDefault();

            // Update the image file.
            OutputPathFinish(folder);
        }
        private void OutputPathBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Get the text from the box.
            string text = OutputPathBox.Text;

            // Update the image file.
            OutputPathFinish(text);
        }
        private void OutputPathButton_Click(object sender, RoutedEventArgs e)
        {
            // Open a dialog to select a folder.
            DialogResult? folder = Dialog.FolderPicker();
            if (!folder.IsOk)
                return;
            string path = folder.Path;

            // Update the image file.
            OutputPathFinish(path);
        }
        private void OutputPathFinish(string path)
        {
            // If the path is invalid reset the text.
            if (string.IsNullOrEmpty(path) || !path.TestPath(true))
            {
                OutputPathBox.Text = Config.OutputPath;
                return;
            }
            // Update the paths and textbox.
            Config.UpdateOutputPaths(path);
            OutputPathBox.Text = path;
        }

/*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        BUTTONS

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        private async void CreateModButton_Click(object sender, RoutedEventArgs e)
        {
            var okayWindow = new OkayWindow();
            string message;

            // Mod name is empty.
            if (string.IsNullOrEmpty(Config.ModName))
            {
                message = "Error: You must enter a \"Mod Name\" to continue.";
                okayWindow.Display("Mod Name Empty", message, timeoutSeconds: 10);
                SoundPlayer.PlayWAVSound("avares://Launcher/Resources/beep.wav");
                await okayWindow.ShowDialog(this);
                return;
            }

            // Description is empty.
            if (string.IsNullOrEmpty(Config.Description))
            {
                message = "Error: You must enter a \"Mod Description\" to continue.";
                okayWindow.Display("Description Empty", message, timeoutSeconds: 10);
                SoundPlayer.PlayWAVSound("avares://Launcher/Resources/beep.wav");
                await okayWindow.ShowDialog(this);
                return;
            }
            // Crop off the added folder so it can be properly checked.
            string realOutputPath = Config.OutputPath != null 
                ? Config.OutputPath.Replace("~ModOutput","")
                : "";

            // Output path invalid or empty.
            if (string.IsNullOrEmpty(Config.OutputPath) || !realOutputPath.TestPath(true))
            {
                message = "Error: Select a valid \"Output Path\" to continue.";
                okayWindow.Display("Output Path Invalid", message, timeoutSeconds: 10);
                SoundPlayer.PlayWAVSound("avares://Launcher/Resources/beep.wav");
                await okayWindow.ShowDialog(this);
                return;
            }
            // Disable form, create patches, enable components.
            EnableComponents(false);
            await Task.Run(() => ModMaker_Functions.CreateModPatches());
            EnableComponents(true);

            // Display that it's done.
            message = "Finished generating. Mod can be found in the \"Output Path\" in a folder named \"~ModOutput\".";
            okayWindow.Display("Mod Created", message, timeoutSeconds: 10);
            SoundPlayer.PlayWAVSound("avares://Launcher/Resources/success.wav");
            await okayWindow.ShowDialog(this);
        }

        private void ModsGitHubButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/BigheadSMZ/Zelda-LA-DX-HD-Mods") { UseShellExecute = true });
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}