/*
 *This file is part of the StroblCap projekt (https://astro.stroblhof-oberrohrbach.de)
 *Copyright(c) 2020 Othmar Ehrhardt
 * 
 * This program is free software: you can redistribute it and/or modify  
 * it under the terms of the GNU General Public License as published by  
 * the Free Software Foundation, version 3.
 *
 * This program is distributed in the hope that it will be useful, but 
 * WITHOUT ANY WARRANTY; without even the implied warranty of 
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU 
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License 
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
 *
 * This file contains the main code for the simple environment plotter
 * plotting the data from the StroblCap device received via MQTT.
 * 
 * This program will remember its position and size on the screen for the
 * next start. 
 *
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace EnvironmentPlot
{
    public partial class Plotter : Form
    {
        private enum enumChart
        {
            Temperature = 0,
            Humidity = 2,
            Dewpoint = 1,
            Power = 3
        }

        #region Properties

        private readonly string sensor1File = "ASCOM.StroblCap.Sensor1";
        private readonly string sensor2File = "ASCOM.StroblCap.Sensor2";

        private double _xCh1 = 0.0;
        private double _xCh2 = 0.0;

        private double _maxXinDiagram = 600.0;
        private int _errorCounter = 0;

        #endregion



        public Plotter()
        {
            InitializeComponent();
            String[] ports = SerialPort.GetPortNames();

        }

        private double ScrapSerie(Chart widget, enumChart chart, double xValue)
        {
            if (xValue > _maxXinDiagram)
            {
                MethodInvoker scrap = delegate
                {
                    widget.Series[(int)chart].Points.RemoveAt(0);
                    foreach (System.Windows.Forms.DataVisualization.Charting.DataPoint point in widget.Series[(int)chart].Points)
                    {
                        point.XValue -= 1.0;
                    }
                };
                widget.Invoke(scrap);
                return _maxXinDiagram;
            }
            return xValue;
        }

        private double AddPoint(Chart widget, enumChart chart, double x, double y)
        {
            MethodInvoker del = delegate
            {
                widget.Series[(int)chart].Points.AddXY(x, y);
            };
            widget.Invoke(del);

            x += 1.0;
            return x;
        }

        private void Plotter_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            if (WindowState == FormWindowState.Maximized)
            {
                Properties.Settings.Default.Location = RestoreBounds.Location;
                Properties.Settings.Default.Size = RestoreBounds.Size;
                Properties.Settings.Default.Maximised = true;
                Properties.Settings.Default.Minimised = false;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.Location = Location;
                Properties.Settings.Default.Size = Size;
                Properties.Settings.Default.Maximised = false;
                Properties.Settings.Default.Minimised = false;
            }
            else
            {
                Properties.Settings.Default.Location = RestoreBounds.Location;
                Properties.Settings.Default.Size = RestoreBounds.Size;
                Properties.Settings.Default.Maximised = false;
                Properties.Settings.Default.Minimised = true;
            }
            Properties.Settings.Default.Save();
        }

        private void Plotter_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Maximised)
            {
                Location = Properties.Settings.Default.Location;
                WindowState = FormWindowState.Maximized;
                Size = Properties.Settings.Default.Size;
            }
            else if (Properties.Settings.Default.Minimised)
            {
                Location = Properties.Settings.Default.Location;
                WindowState = FormWindowState.Minimized;
                Size = Properties.Settings.Default.Size;
            }
            else
            {
                Location = Properties.Settings.Default.Location;
                Size = Properties.Settings.Default.Size;
            }
        }

        private void Plotter_SizeChanged(object sender, EventArgs e)
        {
            int height = this.Size.Height;
            Point chart1Location = chartChannel1.Location;
            chart1Location.Y = 0;
            chartChannel1.Location = chart1Location;
            Size size = chartChannel1.Size;
            size.Height = height / 3;
            chartChannel1.Size = size;
            chartChannel1.Height = (height / 3) -15;

            Point chart2Location = chartChannel2.Location;
            chart1Location.Y = (height/3) -15;
            chartChannel2.Location = chart1Location;
            size = chartChannel2.Size;
            size.Height = height / 3;
            chartChannel2.Size = size;
            chartChannel2.Height = (height / 3) -15;

            
            panelControl.Size = size;
            panelControl.Height = (height / 3) - 15;

        }

        private void timerDataRead_Tick(object sender, EventArgs e)
        {
            double temp1 = 0.0;
            double hum1 = 0.0;
            double dew1 = 0.0;

            double temp2 = 0.0;
            double hum2 = 0.0;
            double dew2 = 0.0;

            ReadSensorFile(sensor1File, out temp1, out hum1, out dew1);
            ReadSensorFile(sensor2File, out temp2, out hum2, out dew2);

            _xCh1 = ScrapSerie(chartChannel1, enumChart.Temperature, _xCh1);
            _xCh1 = AddPoint(chartChannel1, enumChart.Temperature, _xCh1, temp1);

            AddPoint(chartChannel1, enumChart.Dewpoint, _xCh1, dew1);
            ScrapSerie(chartChannel1, enumChart.Dewpoint, _xCh1);
            
            _xCh2 = ScrapSerie(chartChannel2, enumChart.Temperature, _xCh2);
            _xCh2 = AddPoint(chartChannel2, enumChart.Temperature, _xCh2, temp2);
            AddPoint(chartChannel2, enumChart.Dewpoint, _xCh2, dew2);
            ScrapSerie(chartChannel2, enumChart.Dewpoint, _xCh1);
            
        }

        private void ReadSensorFile(string filename, out double temp, out double hum, out double dew)
        {
            temp = 0.0;
            hum = 0.0;
            dew = 0.0;
            int retry = 5;
            while (true)
            {
                try
                {
                    string test = Path.GetTempPath() + filename;
                    using (FileStream fs = new FileStream(Path.GetTempPath() + filename, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        StreamReader rd = new StreamReader(fs);
                        string line = rd.ReadLine();
                        string[] vals = line.Split(';');
                        if (vals.Length == 3)
                        {
                            try
                            {
                                temp = double.Parse(vals[0], CultureInfo.InvariantCulture);

                            }
                            catch (Exception ex)
                            {
                                temp = 0.0;
                            }
                            try
                            {
                                hum = double.Parse(vals[1], CultureInfo.InvariantCulture);

                            }
                            catch (Exception ex)
                            {
                                hum = 0.0;
                            }
                            try
                            {
                                dew = double.Parse(vals[2], CultureInfo.InvariantCulture);

                            }
                            catch (Exception ex)
                            {
                                dew = 0.0;
                            }
                        }
                        rd.Close();
                        fs.Close();
                    }
                }
                catch (Exception ex)
                {
                    retry--;
                    continue;
                }
                break;
            }
        }
        private bool ParseSensor(string str, out double temp, out double hum, out double dew, out double pre)
        {
            String[] fields = str.Split(';');
            temp = hum = dew = pre = 0.0;
            if (fields.Count() != 4) return false;
            temp = (double)((int.Parse(fields[0]) / 100.0));
            hum = (double)((int.Parse(fields[1]) / 100.0));
            dew = (double)((int.Parse(fields[2]) / 100.0));
            pre = (double)((int.Parse(fields[3]) / 10.0));
            return true;
        }

        
    }
}
