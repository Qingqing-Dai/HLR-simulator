using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using System.Collections.Generic;
using OfficeOpenXml;
using System.IO;
using System.Text;
using LiveCharts;
using LiveCharts.Wpf;



namespace HLR_Simulator
{  
    public partial class MainWindow : Window
    {
        private int compressionCount = 0;
        private int ventilationCount = 0;
        private double lastCompressionTime = 0;
        private double lastVentilationTime = 0;
        private DateTime startTime;
        private DispatcherTimer timer;
        private List<double> compressionIntervals;
        private List<double> ventilationIntervals;
        private List<SimulationResult> simulationResults = new List<SimulationResult>();
        
        private MainViewModel viewModel;




        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            compressionIntervals = new List<double>();
            ventilationIntervals = new List<double>();
            startTime = DateTime.Now;
            lastCompressionTime = 0;
            lastVentilationTime = 0;
            compressionCount = 0;
            ventilationCount = 0;
            
            viewModel = new MainViewModel();
            DataContext = viewModel;

            List<SimulationResult> simulationResults = new List<SimulationResult>();
        }
        public void ShowMainPanel()
        {
            MainPanel.Visibility = Visibility.Visible;
            ResultPanel.Visibility = Visibility.Collapsed;
            MainFrame.Content = null;
        }
        public void SetLoggedInUser(string username)
        {
            LoggedInUserText.Text = $"Logged in as: {username}";
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        // ---------------------- Menu bar - button click handlers ----------------------------------
        private void BluetoothSettings_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Navigate to BluetoothSettings.xaml
            HideAllPanels();
            MainFrame.Navigate(new BluetoothSettings());
        }

        private void SearchDevices_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Searching for Bluetooth devices...");
        }

        private void PairDevice_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Pairing new device...");
        }

        private void RemoveDevice_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Removing paired device...");
        }


        private void TraineeSettings_Click(object sender, RoutedEventArgs e)
        {
            MainPanel.Visibility = Visibility.Collapsed;
            ResultPanel.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Visible;
            MainFrame.Navigate(new SettingsPage());
        }

        private void CompressionSettings_Click(object sender, RoutedEventArgs e)
        {
            MainPanel.Visibility = Visibility.Collapsed;
            ResultPanel.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Visible;
            MainFrame.Navigate(new SettingsPage());
        }

        private void VentilationSettings_Click(object sender, RoutedEventArgs e)
        {
            MainPanel.Visibility = Visibility.Collapsed;
            ResultPanel.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Visible;
            MainFrame.Navigate(new SettingsPage());
        }


        private void ToolsButton_Click(object sender, RoutedEventArgs e)
        {
            HideAllPanels();
            MessageBox.Show("Tools menu clicked.");
        }
        private void ResultButton_Click(object sender, RoutedEventArgs e)
        {
            HideAllPanels();
            ResultPanel.Visibility = Visibility.Visible;
            
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            HideAllPanels();
            MainFrame.Visibility = Visibility.Visible;
            MainFrame.Navigate(new HelpPage());
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            HideAllPanels();
            MainFrame.Visibility = Visibility.Visible;
            MainFrame.Navigate(new LoginPage());
        }

        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            ResultPanel.Visibility = Visibility.Collapsed;
            MainPanel.Visibility = Visibility.Visible;
        }

        // --------------- left panel - button click handlers ---------------

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // Reset values
            startTime = DateTime.Now;
            compressionCount = 0;
            ventilationCount = 0;

            // Clear lists
            compressionIntervals?.Clear();
            ventilationIntervals?.Clear();
            simulationResults?.Clear();

            // Update ViewModel properties
            viewModel.StatusMessage = "Simulation started!";
            viewModel.StatusColor = Brushes.Black;
            viewModel.UpdateElapsedTime(0);
            viewModel.IsRunning = true;

            // These force label updates
            viewModel.CompressionCount = 0;
            viewModel.VentilationCount = 0;
            viewModel.OnPropertyChanged(nameof(viewModel.CompressionDisplay));
            viewModel.OnPropertyChanged(nameof(viewModel.VentilationDisplay));

            // Start the timer
            timer.Start();
        }




        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            viewModel.StopSimulation();
        }


        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            compressionCount = 0;
            ventilationCount = 0;
            lastCompressionTime = 0;
            lastVentilationTime = 0;
            startTime = DateTime.Now;

            // Clear ViewModel data
            viewModel.ResetSimulation();

            // Clear local lists (if ViewModel doesn't handle them)
            compressionIntervals?.Clear();
            ventilationIntervals?.Clear();
            simulationResults?.Clear();

            // Reset UI elements
            ResultText.Text = "";

            // Overwrite CSV file with header
            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HLR_Results.csv");
            System.IO.File.WriteAllText(filePath, "Timestamp, Compressions, Ventilations, CPM, VPM\n");
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create a StringBuilder to collect the result
                StringBuilder resultBuilder = new StringBuilder();
                resultBuilder.AppendLine("Timestamp, Compressions, Ventilations, CPM, VPM");

                // Build the result as a string for displaying
                StringBuilder resultDisplay = new StringBuilder();
                resultDisplay.AppendLine("Summary of Results:");
                resultDisplay.AppendLine(string.Format("{0,-20} {1,-15} {2,-15} {3,-10} {4,-10}",
                    "Timestamp", "Compressions", "Ventilations", "CPM", "VPM"));

                // Iterate through the simulation results and format them
                foreach (var result in simulationResults)
                {
                    double cpm = result.Compressions > 0 ? 60 / (compressionIntervals.Count > 0 ? compressionIntervals.Average() : 1) : 0;
                    double vpm = result.Ventilations > 0 ? 60 / (ventilationIntervals.Count > 0 ? ventilationIntervals.Average() : 1) : 0;

                    resultDisplay.AppendLine(string.Format("{0,-20:dd.MM.yyyy HH:mm:ss} {1,-20} {2,-20} {3,-15} {4,-15}",
                        result.Timestamp, result.Compressions, result.Ventilations, Math.Round(cpm), Math.Round(vpm)));
                    
                    resultBuilder.AppendLine($"{result.Timestamp:g},{result.Compressions},{result.Ventilations},{Math.Round(cpm)},{Math.Round(vpm)}");
                }


                // Define the file path and save the results to a CSV file
                string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HLR_Results.csv");
                System.IO.File.WriteAllText(filePath, resultBuilder.ToString());

                // Display the result in the result page
                ResultPanel.Visibility = Visibility.Visible;
                MainPanel.Visibility = Visibility.Collapsed;
                ResultText.Text = resultDisplay.ToString();
                viewModel.StatusMessage = "Results saved successfully!";
                viewModel.StatusColor = Brushes.Green;

            }
            catch (Exception ex)
            {
                viewModel.StatusMessage = $"Error saving results: {ex.Message}";
                viewModel.StatusColor = Brushes.Red;

            }

        }

        private void CompressionButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.RegisterCompression();
            PlaySound("buttonClick.mp3");
            simulationResults.Add(new SimulationResult
            {
                Timestamp = DateTime.Now,
                Compressions = viewModel.CompressionCount,
                Ventilations = viewModel.VentilationCount
            });
        }

        private void VentilationButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.RegisterVentilation();
            PlaySound("breathStinger.mp3");
            simulationResults.Add(new SimulationResult
            {
                Timestamp = DateTime.Now,
                Compressions = viewModel.CompressionCount,
                Ventilations = viewModel.VentilationCount
            });
        }


        //---------------- other functions -----------------

        private void PlaySound(string fileName)
        {
            try
            {
                string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds", fileName);

                if (System.IO.File.Exists(path))
                {
                    MediaPlayer player = new MediaPlayer();
                    player.Open(new Uri(path, UriKind.Absolute));
                    player.Play();
                }
                else
                {
                    MessageBox.Show($"Sound file '{fileName}' was not found at the path: {path}");
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show($"Error playing sound: {ex.Message}");
            }
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            int elapsed = (int)(DateTime.Now - startTime).TotalSeconds;
            viewModel.UpdateElapsedTime(elapsed);  // updates TimerDisplay
        }

        private void HideAllPanels()
        {
            MainPanel.Visibility = Visibility.Collapsed;
            ResultPanel.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Collapsed;
        }



    }
}

