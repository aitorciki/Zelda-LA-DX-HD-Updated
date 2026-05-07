using System;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using static LADXHD_Patcher.Config;
using static LADXHD_Patcher.Functions;

namespace LADXHD_Patcher
{
    public partial class MainWindow : Window, IProgressWindow
    {
        public void UpdateProgressBar(int value)
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() => progressBar.Value = value);
        }
        public void ShowSuccessLabel()
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() => successLabel.IsVisible = true);
        }
        public void HideSuccessLabel()
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() => successLabel.IsVisible = false);
        }

        public MainWindow()
        {
            InitializeComponent();
            combBox_Platform.SelectedIndex = 0;
            combBox_API.SelectedIndex = 0;
            Config.ActiveWindow = this;
//            checkBox_AndroidMods.Visible = false;
        }

        public void EnableComponents(bool toggle)
        {
            combBox_Platform.IsEnabled = toggle;
            combBox_API.IsEnabled      = toggle;
            PatchButton.IsEnabled      = toggle;
            ChangeLogButton.IsEnabled  = toggle;
            ExitButton.IsEnabled       = toggle;
        }

        private void comboBox_Platform_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // The combobox will be null on initialization.
            if (combBox_API == null) return;

            // Get the combobox that was triggered.
            ComboBox comboBox = (ComboBox)sender;

            if (comboBox.SelectedIndex == 0)
            {
                Config.SelectedPlatform = Platform.Windows;

                combBox_API.Items.Clear();
                combBox_API.Items.Add("DirectX");
                combBox_API.Items.Add("OpenGL");
                PatchButton.Content = "Patch";
            }
            if (comboBox.SelectedIndex == 1)
            {
                Config.SelectedPlatform = Platform.Android;

                combBox_API.Items.Clear();
                combBox_API.Items.Add("OpenGL");
                PatchButton.Content = "Create APK";
            }
            if (comboBox.SelectedIndex == 2)
            {
                Config.SelectedPlatform = Platform.Linux_x86;

                combBox_API.Items.Clear();
                combBox_API.Items.Add("OpenGL");
                PatchButton.Content = "Patch";
            }
            if (comboBox.SelectedIndex == 3)
            {
                Config.SelectedPlatform = Platform.Linux_Arm64;

                combBox_API.Items.Clear();
                combBox_API.Items.Add("OpenGL");
                PatchButton.Content = "Patch";
            }
            if (comboBox.SelectedIndex == 4)
            {
                Config.SelectedPlatform = Platform.MacOS_x86;

                combBox_API.Items.Clear();
                combBox_API.Items.Add("OpenGL");
                PatchButton.Content = "Patch";
            }
            if (comboBox.SelectedIndex == 5)
            {
                Config.SelectedPlatform = Platform.MacOS_Arm64;

                combBox_API.Items.Clear();
                combBox_API.Items.Add("OpenGL");
                PatchButton.Content = "Patch";
            }
            combBox_API.SelectedIndex = 0;
            ShowAndroidCheckbox(comboBox.SelectedIndex == 1);
        }

        private void ShowAndroidCheckbox(bool isAndroid)
        {
            Config.AndroidMods = isAndroid && (CheckBox_AndroidMods.IsChecked == true);
            androidOptionsPanel.IsVisible = isAndroid;

            // Offset for the extra row when Android is selected
            double offset = isAndroid ? 30 : 0;

            Canvas.SetTop(progressBar,   498 + offset);
            Canvas.SetTop(successLabel,  498 + offset);
            Canvas.SetTop(buttonGrid,    522 + offset);

            this.Height    = isAndroid ? 600 : 570;
            this.MinHeight = isAndroid ? 600 : 570;
            this.MaxHeight = isAndroid ? 600 : 570;
        }

        private void CheckBox_AndroidMods_CheckedChanged(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            Config.AndroidMods = checkBox.IsChecked == true;
        }

        private void combBox_API_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            if (comboBox.SelectedIndex == 0)
                Config.SelectedGraphics = GraphicsAPI.DirectX;
            else if (comboBox.SelectedIndex == 1)
                Config.SelectedGraphics = GraphicsAPI.OpenGL;
        }

        private async void PatchButton_Click(object sender, RoutedEventArgs e)
        {
            await Functions.StartPatching();
        }

        private void ChangeLogButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/BigheadSMZ/Zelda-LA-DX-HD-Updated/blob/main/CHANGELOG.md",
                UseShellExecute = true
            });
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}