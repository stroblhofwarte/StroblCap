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
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using uPLibrary.Networking.M2Mqtt;

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

        private MqttClient _client;
        private double _xCh1 = 0.0;
        private double _xCh2 = 0.0;

        private double _maxXinDiagram = 600.0;
        private int _errorCounter = 0;
        #endregion

        private SerialPort _serial;
        private string _myPort;


        public Plotter()
        {
            InitializeComponent();
            String[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
                comboBoxPort.Items.Add(port);
            _myPort = Properties.Settings.Default.Port;
            if (comboBoxPort.Items.Contains(_myPort))
            {
                comboBoxPort.SelectedItem = _myPort;
            }
            if (_myPort == String.Empty)
                _myPort = "COM1";
            _serial = new SerialPort(_myPort, 115200, Parity.None, 8, StopBits.One);

        }

        private void comboBoxPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            string comport = comboBoxPort.Text;
            if(_myPort != comport)
            {
                _myPort = comport;
                Properties.Settings.Default.Port = _myPort;
                _serial = new SerialPort(_myPort, 115200, Parity.None, 8, StopBits.One);
            }
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

        private void _client_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            try
            {
                if (e.Topic == "Astro/StroblCap/Env/ch1/Temp")
                {
                    _xCh1 = ScrapSerie(chartChannel1, enumChart.Temperature, _xCh1);
                    double val = double.Parse(ByteArrayToString(e.Message), CultureInfo.InvariantCulture);
                    _xCh1 = AddPoint(chartChannel1, enumChart.Temperature, _xCh1, val);
                    
                }
                if (e.Topic == "Astro/StroblCap/Env/ch1/Humidity" && checkBoxHumidityChannel1.Checked)
                {
                    double val = double.Parse(ByteArrayToString(e.Message), CultureInfo.InvariantCulture);
                    AddPoint(chartChannel1, enumChart.Humidity, _xCh1, val);
                    ScrapSerie(chartChannel1, enumChart.Humidity, _xCh1);
                }
                if (e.Topic == "Astro/StroblCap/Env/ch1/Dewpoint")
                {
                    double val = double.Parse(ByteArrayToString(e.Message), CultureInfo.InvariantCulture);
                    AddPoint(chartChannel1, enumChart.Dewpoint, _xCh1, val);
                    ScrapSerie(chartChannel1, enumChart.Dewpoint, _xCh1);
                }
                if (e.Topic == "Astro/StroblCap/ch1/state" && checkBoxPowerCh1.Checked)
                {
                    int intermVal = e.Message[0];
                    double val = (double)intermVal - 100.0;
                    AddPoint(chartChannel1, enumChart.Power, _xCh1, val);
                    ScrapSerie(chartChannel1, enumChart.Power, _xCh1);
                }
                _errorCounter = 0;
            } catch (Exception ex)
            {
                _errorCounter++;
            }

            try
            {
                if (e.Topic == "Astro/StroblCap/Env/ch2/Temp")
                {
                    _xCh2 = ScrapSerie(chartChannel2, enumChart.Temperature, _xCh2);
                    double val = double.Parse(ByteArrayToString(e.Message), CultureInfo.InvariantCulture);
                    _xCh2 = AddPoint(chartChannel2, enumChart.Temperature, _xCh2, val);

                }
                if (e.Topic == "Astro/StroblCap/Env/ch2/Humidity" && checkBoxHumidityChannel2.Checked)
                {
                    double val = double.Parse(ByteArrayToString(e.Message), CultureInfo.InvariantCulture);
                    AddPoint(chartChannel2, enumChart.Humidity, _xCh2, val);
                    ScrapSerie(chartChannel2, enumChart.Humidity, _xCh2);
                }
                if (e.Topic == "Astro/StroblCap/Env/ch2/Dewpoint")
                {
                    double val = double.Parse(ByteArrayToString(e.Message), CultureInfo.InvariantCulture);
                    AddPoint(chartChannel2, enumChart.Dewpoint, _xCh2, val);
                    ScrapSerie(chartChannel2, enumChart.Dewpoint, _xCh2);
                }
                if (e.Topic == "Astro/StroblCap/ch2/state" && checkBoxPowerCh2.Checked)
                {
                    int intermVal = e.Message[0];
                    double val = (double)intermVal - 100.0;
                    AddPoint(chartChannel2, enumChart.Power, _xCh2, val);
                    ScrapSerie(chartChannel2, enumChart.Power, _xCh2);
                }
                _errorCounter = 0;
            }
            catch (Exception ex)
            {
                _errorCounter++;
            }
        }

        private string ByteArrayToString(byte[] arr)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetString(arr);
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
            size.Height = height / 2;
            chartChannel1.Size = size;
            chartChannel1.Height = (height / 2) -15;

            Point chart2Location = chartChannel2.Location;
            chart1Location.Y = (height/2) -15;
            chartChannel2.Location = chart1Location;
            size = chartChannel2.Size;
            size.Height = height / 2;
            chartChannel2.Size = size;
            chartChannel2.Height = (height / 2) -15;
            Point checkboxCalc = checkBoxHumidityChannel2.Location;
            Point referenceCheck = checkBoxHumidityChannel1.Location;
            checkboxCalc.Y = height / 2 + referenceCheck.Y;
            checkBoxHumidityChannel2.Location = checkboxCalc;
            checkboxCalc = checkBoxPowerCh2.Location;
            referenceCheck = checkBoxPowerCh1.Location;
            checkboxCalc.Y = height / 2 + referenceCheck.Y;
            checkBoxPowerCh2.Location = checkboxCalc;
        }

        private void checkBoxHumidityChannel1_CheckedChanged(object sender, EventArgs e)
        {
            if(!checkBoxHumidityChannel1.Checked)
            {
                chartChannel1.Series[(int)enumChart.Humidity].Points.Clear();
            }
            
        }

        private void checkBoxPowerCh1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxPowerCh1.Checked)
            {
                chartChannel1.Series[(int)enumChart.Power].Points.Clear();
            }
           
        }

        private void checkBoxHumidityChannel2_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxHumidityChannel2.Checked)
            {
                chartChannel2.Series[(int)enumChart.Humidity].Points.Clear();
            }
        }

        private void checkBoxPowerCh2_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxPowerCh2.Checked)
            {
                chartChannel2.Series[(int)enumChart.Power].Points.Clear();
            }
        }

        private void timerDataRead_Tick(object sender, EventArgs e)
        {
            string channel1 = Communicate("G1:");
            string channel2 = Communicate("G2:");
            string power1 = Communicate("P1:");
            string power2 = Communicate("P2:");
            int pwm1 = int.Parse(power1);
            int pwm2 = int.Parse(power2);


            if (channel1 != String.Empty)
            {
                double temp, hum, dew, pres;
                if (ParseSensor(channel1, out temp, out hum, out dew, out pres))
                {
                    if (temp != 0.0 || hum != 0.0 || dew != 0.0 || pres != 0.0)
                    {
                        _xCh1 = ScrapSerie(chartChannel1, enumChart.Temperature, _xCh1);
                        _xCh1 = AddPoint(chartChannel1, enumChart.Temperature, _xCh1, temp);

                        if (checkBoxHumidityChannel1.Checked)
                        {
                            AddPoint(chartChannel1, enumChart.Humidity, _xCh1, hum);
                            ScrapSerie(chartChannel1, enumChart.Humidity, _xCh1);
                        }

                        AddPoint(chartChannel1, enumChart.Dewpoint, _xCh1, dew);
                        ScrapSerie(chartChannel1, enumChart.Dewpoint, _xCh1);
                    }
                    if (checkBoxPowerCh1.Checked)
                    {
                        double val = (double)pwm1;
                        AddPoint(chartChannel1, enumChart.Power, _xCh1, val);
                        ScrapSerie(chartChannel1, enumChart.Power, _xCh1);
                    }
                }
            }
            if (channel2 != String.Empty)
            {
                double temp, hum, dew, pres;
                if (ParseSensor(channel2, out temp, out hum, out dew, out pres))
                {
                    if (temp != 0.0 || hum != 0.0 || dew != 0.0 || pres != 0.0)
                    {
                        _xCh2 = ScrapSerie(chartChannel2, enumChart.Temperature, _xCh2);
                        _xCh2 = AddPoint(chartChannel2, enumChart.Temperature, _xCh2, temp);

                        if (checkBoxHumidityChannel2.Checked)
                        {
                            AddPoint(chartChannel2, enumChart.Humidity, _xCh2, hum);
                            ScrapSerie(chartChannel2, enumChart.Humidity, _xCh2);
                        }

                        AddPoint(chartChannel2, enumChart.Dewpoint, _xCh2, dew);
                        ScrapSerie(chartChannel2, enumChart.Dewpoint, _xCh1);
                    }
                    if (checkBoxPowerCh2.Checked)
                    {
                        double val = (double)pwm2;
                        AddPoint(chartChannel2, enumChart.Power, _xCh2, val);
                        ScrapSerie(chartChannel2, enumChart.Power, _xCh2);
                    }
                }
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

        private string Communicate(string cmd)
        {
            string ret = String.Empty;
            int timeout = 500;
            while (true)
            {
                try
                {
                    _serial.Open();
                    _serial.Write(cmd);
                    while(timeout >0 )
                    {
                        if(_serial.BytesToRead > 0)
                        {
                            ret += _serial.ReadExisting();
                            if (ret.Contains("#"))
                                break;
                        }
                        Thread.Sleep(1);
                        timeout--;
                    }
                    _serial.Close();
                    break;
                }
                catch (Exception ex)
                {
                    // Port in use (by ASCOM driver)
                    _serial.Close();
                    Thread.Sleep(100);
                    continue;
                }
            }
            // Remove all chars other than 0-9 and ;
            StringBuilder sb = new StringBuilder();
            foreach (char c in ret)
            {
                if ((c >= '0' && c <= '9') || c == ';')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        
    }
}
