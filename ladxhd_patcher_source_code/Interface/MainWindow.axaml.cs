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
            ComboBox_Platform.SelectedIndex = 0;
            ComboBox_API.SelectedIndex = 0;
            Config.ActiveWindow = this;
        }

        public void EnableComponents(bool toggle)
        {
            ComboBox_Platform.IsEnabled = toggle;
            ComboBox_API.IsEnabled      = toggle;
            Button_Patch.IsEnabled      = toggle;
            Button_ChangeLog.IsEnabled  = toggle;
            Button_Exit.IsEnabled       = toggle;
        }

        private void comboBox_Platform_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // The combobox will be null on initialization.
            if (ComboBox_API == null) return;

            // Get the combobox that was triggered.
            ComboBox comboBox = (ComboBox)sender;

            if (comboBox.SelectedIndex == 0)
            {
                Config.SelectedPlatform = Platform.Windows;

                ComboBox_API.Items.Clear();
                ComboBox_API.Items.Add("DirectX");
                ComboBox_API.Items.Add("OpenGL");
                Button_Patch.Content = "Patch";
            }
            else if (comboBox.SelectedIndex == 1)
            {
                Config.SelectedPlatform = Platform.Android;

                ComboBox_API.Items.Clear();
                ComboBox_API.Items.Add("OpenGL");
                Button_Patch.Content = "Create APK";
            }
            else if (comboBox.SelectedIndex == 2)
            {
                Config.SelectedPlatform = Platform.Linux_x86;

                ComboBox_API.Items.Clear();
                ComboBox_API.Items.Add("OpenGL");
                Button_Patch.Content = "Patch";
            }
            else if (comboBox.SelectedIndex == 3)
            {
                Config.SelectedPlatform = Platform.Linux_Arm64;

                ComboBox_API.Items.Clear();
                ComboBox_API.Items.Add("OpenGL");
                Button_Patch.Content = "Patch";
            }
            else if (comboBox.SelectedIndex == 4)
            {
                Config.SelectedPlatform = Platform.MacOS_x86;

                ComboBox_API.Items.Clear();
                ComboBox_API.Items.Add("OpenGL");
                Button_Patch.Content = "Patch";
            }
            else if (comboBox.SelectedIndex == 5)
            {
                Config.SelectedPlatform = Platform.MacOS_Arm64;

                ComboBox_API.Items.Clear();
                ComboBox_API.Items.Add("OpenGL");
                Button_Patch.Content = "Patch";
            }
            ComboBox_API.SelectedIndex = 0;
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

        private async void Button_Patch_Click(object sender, RoutedEventArgs e)
        {
            await Functions.StartPatching();
        }

        private void Button_ChangeLog_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/BigheadSMZ/Zelda-LA-DX-HD-Updated/blob/main/CHANGELOG.md",
                UseShellExecute = true
            });
        }

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}