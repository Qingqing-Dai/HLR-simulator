using System.Windows;
using System.Windows.Controls;

namespace HLR_Simulator
{
    public partial class HelpPage : Page
    {
        public HelpPage()
        {
            InitializeComponent();
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
