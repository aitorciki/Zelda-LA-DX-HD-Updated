using System;
using Avalonia.Controls;
using Avalonia.Threading;

namespace LADXHD_Launcher
{
    public partial class OkayWindow : Window
    {
        private DispatcherTimer? _timer;

        public OkayWindow()
        {
            InitializeComponent();
        }

        public void Display(string title, string message, int timeoutSeconds = 0)
        {
            Title = title;
            MessageLabel.Text = message;

            if (timeoutSeconds > 0)
            {
                _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(timeoutSeconds) };
                _timer.Tick += (_, _) => Close();
                _timer.Start();
            }
        }

        private void OkButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            _timer?.Stop();
            Close();
        }
    }
}