
# HLR (CPR) Simulator

This is a custom-built WPF application for simulating CPR (Cardiopulmonary Resuscitation) performance, including compression and ventilation tracking. It was developed as a demonstration of interactive medical simulation and software design.

# Visual Breakdown of the MVVM Project Structure
│
├── MainWindow.xaml               ← View (UI layout: buttons, chart, text blocks)
├── MainWindow.xaml.cs            ← Code-behind (sets DataContext, handles UI events)
│     └── Uses MainViewModel
│
├── MainViewModel.cs              ← ViewModel (holds logic, chart data, async loop)
│     └── ChartValues<double> for compression & ventilation
│     └── Start/Stop/Reset methods
│     └── Starts async simulation loop
│
├── SimulationResult.cs           ← Model (simple data structure)
│     └── Timestamp
│     └── Compressions
│     └── Ventilations
│
├── Other folders (optional for growth)
│     └── Models/                 ← Store SimulationResult and future data classes
│     └── ViewModels/            ← Store MainViewModel and others
│     └── Views/                 ← Store MainWindow and other XAML views

## Features
- Live simulation of compressions and ventilations  
- Real-time feedback with color indicators and CPM/VPM calculation  
- Compression and Ventilation rhythm chart using LiveCharts  
- Session data saved to a CSV file  
- Result viewer with summary table  
- Tabbed settings panel (Trainee, Compression, Ventilation)  
- Login page with password hashing  
- Bluetooth panel (mock UI for testing device connection logic)  
- Help page with instructions  
- Fully navigable via WPF `<Frame>`

## Technologies Used

- C# (.NET 6) with WPF  
- LiveCharts for real-time charting  
- MV-like UI logic separation  
- SHA256 hashing for simulated authentication  
- File I/O for CSV saving

## How to Run

1. Open the solution in Visual Studio 2022+
2. Restore NuGet packages (LiveCharts.Wpf)
3. Build and run (Ctrl+F5)
4. Use username: `Vincent` and password: `1234` to log in

## Notes

- Bluetooth functionality is simulated; no real hardware required.
- Ideal for future extensions like hardware integration or multi-user session logging.

## Author

Qingqing Dai  
April 2025