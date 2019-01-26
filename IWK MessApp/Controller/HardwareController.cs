using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinkerforge;

namespace IWK_MessApp.Controller
{
    internal static class HardwareController
    {
        private static string HOST = "localhost";
        private static int PORT = 4223;
        private static string UID_TCB = "xxx"; // 
        private static string UID_SD7 = "xxx"; // Segmentdisplay
        private static byte[] DIGITS = {0x3f,0x06,0x5b,0x4f,
                                    0x66,0x6d,0x7d,0x07,
                                    0x7f,0x6f,0x77,0x7c,
                                    0x39,0x5e,0x79,0x71}; // 0~9,A,b,C,d,E,F

        /// <summary>
        /// Anzeigen des Wertes am 7-Segment-Display
        /// </summary>
        /// <param name="value">Maximal 4 Stellen, nur Ganzzahlen</param>
        internal static void DisplayValue(string value)
        {
            try
            {
                int i3 =0;
                int i2 = 0;
                int i1 =0;
                int i0= 0;

                int sl = value.Length;
                switch (sl)
                { 
                    case 1:
                        i0 = Convert.ToInt32(value[0]);
                        break;
                    case 2:
                        i1 = Convert.ToInt32(value[0].ToString());
                        i0 = Convert.ToInt32(value[1].ToString());
                        break;
                    case 3:
                        i2 = Convert.ToInt32(value[0].ToString());
                        i1 = Convert.ToInt32(value[1].ToString());
                        i0 = Convert.ToInt32(value[2].ToString());
                        break;
                    case 4:
                        i3 = Convert.ToInt32(value[0].ToString());
                        i2 = Convert.ToInt32(value[1].ToString());
                        i1 = Convert.ToInt32(value[2].ToString());
                        i0 = Convert.ToInt32(value[3].ToString());
                        break;
                }
                //int i3 = Convert.ToInt32(value[3]), i2 = Convert.ToInt32(value[2]), i1 = Convert.ToInt32( value[1]), i0 = Convert.ToInt32(value[0]);
                IPConnection ipcon = new IPConnection(); // Create IP connection
                BrickletSegmentDisplay4x7 sd = new BrickletSegmentDisplay4x7(UID_SD7, ipcon); // Create device object

                ipcon.Connect(HOST, PORT);
                byte[] segments = { DIGITS[i3], DIGITS[i2], DIGITS[i1], DIGITS[i0] };
                sd.SetSegments(segments, 7, false);
               
                ipcon.Disconnect();
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Anzeigen des Wertes", ex);
            }
        }

        /// <summary>
        /// Ruft die aktuelle Wassertemperatur ab
        /// </summary>
        /// <returns>Temperatur in C</returns>
        internal static double GetCurrentTemperature()
        {
            try
            {
                IPConnection ipcon = new IPConnection(); // Create IP connection
                BrickletThermocouple t = new BrickletThermocouple(UID_TCB, ipcon); // Create device object
                ipcon.Connect(HOST, PORT);
                int temperature = t.GetTemperature();

                ipcon.Disconnect();
                return temperature / 100.0;
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Auslesen der Temperatur", ex);
            }
        }
    }
}
