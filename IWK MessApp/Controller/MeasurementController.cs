using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWK_MessApp.Controller
{
    internal static class MeasurementController
    {
        internal static bool _heatCheck;
        internal static string _measurementName;
        internal static string _volume;
        internal static int _measurementInterval = 1000;
        internal static BackgroundWorker _worker;
        private static bool stopworker = false;

        internal static bool Stopworker { get => stopworker; set => stopworker = value; }

        internal static void StopMeasurement()
        {
            _worker.CancelAsync();
            Stopworker = true;
            HardwareController.DisplayValue("");

        }

        internal static void StartMeasurement()
        {
            InitializeWorker();
            _worker.RunWorkerAsync();
            Stopworker = false;
            
        }

        private static void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            DateTime timeStart;
            string heating = "ON";
            WriteCSVLine("TIME", "TEMPERATURE", "VOLUME", "HEATING");
            timeStart = DateTime.Now;
            while (!Stopworker)
            {
                if (_heatCheck)
                {
                    heating = "ON";
                }
                else
                {
                    heating = "OFF";
                }
                double time = (DateTime.Now - timeStart).TotalSeconds;
                double curTemperature = HardwareController.GetCurrentTemperature();
                HardwareController.DisplayValue(Convert.ToInt32((int)curTemperature).ToString());
                //WriteCSVLine(time.ToString(), "50", _volume.ToString(), heating);
                WriteCSVLine(time.ToString(), curTemperature.ToString(), _volume.ToString(),heating);
                System.Threading.Thread.Sleep(_measurementInterval);
            }
        }
        private static void WriteCSVLine(string time, string temperature, string volume, string heating)
        {
            string csvline = String.Format("{0};{1};{2};{3}", time, temperature, volume, heating);
            using (StreamWriter w = File.AppendText(String.Format("{0}.csv",_measurementName)))
            {
                w.WriteLine(csvline);
            }
        }

        private static void InitializeWorker()
        {
            _worker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true               
            };
            _worker.DoWork += _worker_DoWork;
        }
    }
}
