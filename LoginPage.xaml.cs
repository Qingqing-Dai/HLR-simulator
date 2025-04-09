using System.Windows;
using System.Windows.Controls;
using System.Security.Cryptography;
using System.Text;

namespace HLR_Simulator
{
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        public static string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }


        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string password = PasswordBox.Password;

            string storedUsername = "Vincent";
            // SHA256 hash of "1234"
            string storedPasswordHash = "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4";  

            string inputHash = ComputeHash(password);

            if (username == storedUsername && inputHash == storedPasswordHash)
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.SetLoggedInUser(username);
                    mainWindow.ShowMainPanel();  
                }
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
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

