using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static IWK_MessApp.Controller.MeasurementController;
namespace IWK_MessApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnRecord_Click(object sender, RoutedEventArgs e)
        {
            _volume = txtbxFilling.Text;
            _measurementInterval = Convert.ToInt32(txtbxInterval.Text.ToString());
            _measurementName = txtbxMessName.Text;
            StartMeasurement();
            _heatCheck = !chkbxHeatOff.IsChecked.Value;
            MessageBox.Show("Messung gestartet");
        }

        private void btnRecStop_Click(object sender, RoutedEventArgs e)
        {
            StopMeasurement();
            MessageBox.Show("Messung beendet");
        }


        private void chkbxHeatOff_Checked(object sender, RoutedEventArgs e)
        {
            _heatCheck = false;
        }

        private void chkbxHeatOff_Unchecked(object sender, RoutedEventArgs e)
        {
            _heatCheck = true;
        }
    }
}
