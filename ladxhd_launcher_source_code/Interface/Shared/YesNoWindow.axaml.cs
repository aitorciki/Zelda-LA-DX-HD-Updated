using Avalonia.Controls;

namespace LADXHD_Launcher
{
    public partial class YesNoWindow : Window
    {
        public bool Result { get; private set; } = false;

        public YesNoWindow()
        {
            InitializeComponent();
        }

        public void Display(string title, string message)
        {
            Title = title;
            MessageLabel.Text = message;
        }

        private void YesButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Result = true;
            Close();
        }

        private void NoButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Result = false;
            Close();
        }
    }
}