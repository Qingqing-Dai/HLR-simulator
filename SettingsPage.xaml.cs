using System.Windows;
using System.Windows.Controls;

namespace HLR_Simulator
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Settings saved successfully!");
        }
        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainPanel.Visibility = Visibility.Visible;
                mainWindow.ResultPanel.Visibility = Visibility.Collapsed;
                mainWindow.MainFrame.Visibility = Visibility.Collapsed;
            }
        }
    }
}
