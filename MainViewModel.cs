using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;

namespace HLR_Simulator
{
    internal class MainViewModel
    {
        private CancellationTokenSource _cts;
        private bool _isRunning = false;
        public ChartValues<double> CompressionRates { get; set; } = new ChartValues<double>();
        public ChartValues<double> VentilationRates { get; set; } = new ChartValues<double>();
        private async Task StartSimulationLoop(CancellationToken token)
        {
            _isRunning = true;
            Random rand = new Random();

            while (!token.IsCancellationRequested)
            {
                double cpm = rand.Next(95, 125);
                double vpm = rand.Next(10, 20);

                CompressionRates.Add(cpm);
                VentilationRates.Add(vpm);

                if (CompressionRates.Count > 30) CompressionRates.RemoveAt(0);
                if (VentilationRates.Count > 30) VentilationRates.RemoveAt(0);

                await Task.Delay(1000);
            }

            _isRunning = false;
        }
        public void StartSimulation()
        {
            if (_isRunning) return;

            _cts = new CancellationTokenSource();
            _ = StartSimulationLoop(_cts.Token);
        }

        public void StopSimulation()
        {
            _cts?.Cancel(); // tells the loop to exit
            _isRunning = false;
        }

        public void ResetSimulation()
        {
            StopSimulation();
            CompressionRates.Clear();
            VentilationRates.Clear();
        }



    }
}
