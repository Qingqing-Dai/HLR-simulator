using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using LiveCharts;

namespace HLR_Simulator
{   //define the property with change notification that links to data binding in xaml.
    public class MainViewModel : INotifyPropertyChanged
    {
        public ChartValues<double> CompressionRates { get; set; } = new ChartValues<double>();
        public ChartValues<double> VentilationRates { get; set; } = new ChartValues<double>();

        public int CompressionCount { get; set; }
        public int VentilationCount { get; set; }

        private List<double> compressionIntervals = new();
        private List<double> ventilationIntervals = new();

        private DateTime startTime = DateTime.Now;
        private double lastCompressionTime = 0;
        private double lastVentilationTime = 0;

        private int elapsedSeconds = 0;
        private double currentDepth = 0;


        // binding to xaml
        public string StatusMessage { get; set; } = "Welcome to CPR Simulator!";
        public Brush StatusColor { get; set; } = Brushes.Black;
        public string CompressionDisplay => $"Compressions: {CompressionCount}";
        public string VentilationDisplay => $"Ventilations: {VentilationCount}";
        public string TimerDisplay => $"Time: {elapsedSeconds} seconds";
        public string DepthDisplay => $"Pressure depth: {currentDepth} cm";


        private bool _isRunning = false;



        public void RegisterCompression()
        {
            CompressionCount++;
            double currentTime = (DateTime.Now - startTime).TotalSeconds;

            if (lastCompressionTime > 0)
            {
                double interval = currentTime - lastCompressionTime;
                compressionIntervals.Add(interval);
                double cpm = 60 / interval;

                // Update chart and trim data, Rolling buffer to display only the latest 30 values on chart.
                CompressionRates.Add(cpm);
                if (CompressionRates.Count > 30) CompressionRates.RemoveAt(0);

                // Update UI feedback
                if (cpm >= 100 && cpm <= 120)
                {
                    StatusMessage = $"Compression: {CompressionCount} (Good Rate: {Math.Round(cpm)} CPM)";
                    StatusColor = Brushes.Green;
                }
                else if (cpm < 100)
                {
                    StatusMessage = $"Compression: {CompressionCount} (Too Slow: {Math.Round(cpm)} CPM)";
                    StatusColor = Brushes.Orange;
                }
                else
                {
                    StatusMessage = $"Compression: {CompressionCount} (Too Fast: {Math.Round(cpm)} CPM)";
                    StatusColor = Brushes.Red;
                }
            }

            lastCompressionTime = currentTime;
            OnPropertyChanged(nameof(StatusMessage));
            OnPropertyChanged(nameof(StatusColor));
            OnPropertyChanged(nameof(CompressionDisplay));

        }

        public void RegisterVentilation()
        {
            VentilationCount++;
            double currentTime = (DateTime.Now - startTime).TotalSeconds;

            if (lastVentilationTime > 0)
            {
                double interval = currentTime - lastVentilationTime;
                ventilationIntervals.Add(interval);
                double vpm = 60 / interval;

                VentilationRates.Add(vpm);
                if (VentilationRates.Count > 30) VentilationRates.RemoveAt(0);

                if (vpm >= 10 && vpm <= 12)
                {
                    StatusMessage = $"Ventilation: {VentilationCount} (Good Rate: {Math.Round(vpm)} VPM)";
                    StatusColor = Brushes.Green;
                }
                else if (vpm < 10)
                {
                    StatusMessage = $"Ventilation: {VentilationCount} (Too Slow: {Math.Round(vpm)} VPM)";
                    StatusColor = Brushes.Orange;
                }
                else
                {
                    StatusMessage = $"Ventilation: {VentilationCount} (Too Fast: {Math.Round(vpm)} VPM)";
                    StatusColor = Brushes.Red;
                }
            }

            lastVentilationTime = currentTime;
            OnPropertyChanged(nameof(StatusMessage));
            OnPropertyChanged(nameof(StatusColor));
            OnPropertyChanged(nameof(VentilationDisplay));

        }
        //Uses INotifyPropertyChanged to notify the UI when bound properties change
        //so elements like charts, and status messages update automatically
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public void UpdateElapsedTime(int seconds)
        {
            elapsedSeconds = seconds;
            OnPropertyChanged(nameof(TimerDisplay));
        }


        /*public void UpdateDepth(double depth)
        {
            currentDepth = depth;
            OnPropertyChanged(nameof(DepthDisplay));
        }*/


        //Keeps track of whether the simulation is running
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                OnPropertyChanged(nameof(IsRunning));
            }
        }
        public void StopSimulation()
        {
            IsRunning = false;
            StatusMessage = "Simulation stopped.";
            StatusColor = Brushes.Black;
            OnPropertyChanged(nameof(StatusMessage));
            OnPropertyChanged(nameof(StatusColor));
        }

        public void ResetSimulation()
        {
            StopSimulation();
            CompressionRates.Clear();
            VentilationRates.Clear();
            CompressionCount = 0;
            VentilationCount = 0;
            StatusMessage = "Simulation reset.";
            StatusColor = Brushes.Black;
            OnPropertyChanged(nameof(StatusMessage));
            OnPropertyChanged(nameof(StatusColor));
            OnPropertyChanged(nameof(CompressionDisplay));
            OnPropertyChanged(nameof(VentilationDisplay));
            OnPropertyChanged(nameof(IsRunning));

        }



    }
}
