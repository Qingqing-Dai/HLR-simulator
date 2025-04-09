using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HLR_Simulator
{
    public partial class BluetoothSettings : Window
    {
        public BluetoothSettings()
        {
            InitializeComponent();
        }


        private void PairButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Pairing new device...");
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (PairedDevicesList.SelectedItem != null)
            {
                MessageBox.Show($"Removed {((ListBoxItem)PairedDevicesList.SelectedItem).Content}");
                PairedDevicesList.Items.Remove(PairedDevicesList.SelectedItem);
            }
            else
            {
                MessageBox.Show("Please select a device to remove.");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

