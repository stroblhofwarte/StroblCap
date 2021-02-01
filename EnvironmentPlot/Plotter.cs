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

using ASCOM.DriverAccess;
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

        public enum enumSwitch
        {
            PowerCh1 = 0,
            OnOffCh1 = 1,
            AutoCh1 = 2,
            TempCh1 = 3,
            HumCh1 = 4,
            DewCh1 = 5,
            PwrCh1 = 6,

            PowerCh2 = 7,
            OnOffCh2 = 8,
            AutoCh2 = 9,
            TempCh2 = 10,
            HumCh2 = 11,
            DewCh2 = 12,
            PwrCh2 = 13
        }

        #region Properties

        private readonly string sensor1File = "ASCOM.StroblCap.Sensor1";
        private readonly string sensor2File = "ASCOM.StroblCap.Sensor2";

        private double _xCh1 = 0.0;
        private double _xCh2 = 0.0;

        private double _maxXinDiagram = 600.0;
        private int _errorCounter = 0;

        private ASCOM.DriverAccess.Switch _switch;
        private bool _switcheReady;

        private bool _leftExpanded = false;
        private bool _rightExpanded = false;
        private bool _systemExpanded = true;

        #endregion



        public Plotter()
        {
            InitializeComponent();
            String[] ports = SerialPort.GetPortNames();

        }

        private void ReadSwitched()
        {
            if (!_switcheReady) return;
            if (!_switch.Connected) return;

            // Channel 1
            labelChannel1.Text = _switch.GetSwitchName((short)enumSwitch.PowerCh1);
            labelDesc1.Text = _switch.GetSwitchDescription((short)enumSwitch.PowerCh1);
            if (_switch.GetSwitch((short)enumSwitch.OnOffCh1))
                buttonCh1OnOff.Image = global::EnvironmentPlot.Properties.Resources.On;
            else
                buttonCh1OnOff.Image = global::EnvironmentPlot.Properties.Resources.Off;

            if (_switch.GetSwitch((short)enumSwitch.AutoCh1))
                buttonAuto1.Image = global::EnvironmentPlot.Properties.Resources.On;
            else
                buttonAuto1.Image = global::EnvironmentPlot.Properties.Resources.Off;

            numericUpDownChannel1.Value = (int)_switch.GetSwitchValue((short)enumSwitch.PowerCh1);
            _switch.SetSwitchValue((short)enumSwitch.PowerCh1, (double)numericUpDownChannel1.Value);

            // Channel 2
            labelChannel2.Text = _switch.GetSwitchName((short)enumSwitch.PowerCh2);
            labelDesc2.Text = _switch.GetSwitchDescription((short)enumSwitch.PowerCh2);
            if (_switch.GetSwitch((short)enumSwitch.OnOffCh2))
                buttonCh2OnOff.Image = global::EnvironmentPlot.Properties.Resources.On;
            else
                buttonCh2OnOff.Image = global::EnvironmentPlot.Properties.Resources.Off;

            if (_switch.GetSwitch((short)enumSwitch.AutoCh2))
                buttonAuto2.Image = global::EnvironmentPlot.Properties.Resources.On;
            else
                buttonAuto2.Image = global::EnvironmentPlot.Properties.Resources.Off;

            numericUpDownChannel2.Value = (int)_switch.GetSwitchValue((short)enumSwitch.PowerCh2);
            _switch.SetSwitchValue((short)enumSwitch.PowerCh2, (double)numericUpDownChannel2.Value);

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

            ExpandLeft(_leftExpanded);
            ExpandRight(_rightExpanded);
            ExpandSystem(_systemExpanded);
        }

        private void timerDataRead_Tick(object sender, EventArgs e)
        {
            double temp1 = 0.0;
            double hum1 = 0.0;
            double dew1 = 0.0;
            long stamp1 = 0;

            double temp2 = 0.0;
            double hum2 = 0.0;
            double dew2 = 0.0;
            long stamp2 = 0;

            ReadSensorFile(sensor1File, out temp1, out hum1, out dew1, out stamp1);
            ReadSensorFile(sensor2File, out temp2, out hum2, out dew2, out stamp2);

            _xCh1 = ScrapSerie(chartChannel1, enumChart.Temperature, _xCh1);
            _xCh1 = AddPoint(chartChannel1, enumChart.Temperature, _xCh1, temp1);

            AddPoint(chartChannel1, enumChart.Dewpoint, _xCh1, dew1);
            ScrapSerie(chartChannel1, enumChart.Dewpoint, _xCh1);
            
            _xCh2 = ScrapSerie(chartChannel2, enumChart.Temperature, _xCh2);
            _xCh2 = AddPoint(chartChannel2, enumChart.Temperature, _xCh2, temp2);
            AddPoint(chartChannel2, enumChart.Dewpoint, _xCh2, dew2);
            ScrapSerie(chartChannel2, enumChart.Dewpoint, _xCh1);

            string unit1 = " sec";
            string unit2 = " sec";
            if(stamp1 > 60)
            {
                stamp1 = stamp1 / 60;
                unit1 = " min";
            }
            if (stamp1 > 60)
            {
                stamp1 = stamp1 / 60;
                unit1 = " h";
            }
            if (stamp2 > 60)
            {
                stamp2 = stamp2 / 60;
                unit2 = " min";
            }
            if (stamp2 > 60)
            {
                stamp2 = stamp2 / 60;
                unit2 = " h";
            }

            labelAge1.Text = stamp1.ToString(CultureInfo.InvariantCulture) + unit1;
            labelAge2.Text = stamp2.ToString(CultureInfo.InvariantCulture) + unit2;

            if (!_switcheReady) return;
            if (!_switch.Connected) return;
            labelTemp1.Text = (_switch.GetSwitchValue((short)enumSwitch.TempCh1)/100).ToString(CultureInfo.InvariantCulture) + "°C";
            labelDew1.Text = (_switch.GetSwitchValue((short)enumSwitch.DewCh1)/100).ToString(CultureInfo.InvariantCulture) + "°C";
            labelHum1.Text = (_switch.GetSwitchValue((short)enumSwitch.HumCh1)/100).ToString(CultureInfo.InvariantCulture) + " %";
            labelPower1.Text = _switch.GetSwitchValue((short)enumSwitch.PwrCh1).ToString(CultureInfo.InvariantCulture) + " %";

            labelTemp2.Text = (_switch.GetSwitchValue((short)enumSwitch.TempCh2) / 100).ToString(CultureInfo.InvariantCulture) + "°C";
            labelDew2.Text = (_switch.GetSwitchValue((short)enumSwitch.DewCh2) / 100).ToString(CultureInfo.InvariantCulture) + "°C";
            labelHum2.Text = (_switch.GetSwitchValue((short)enumSwitch.HumCh2) / 100).ToString(CultureInfo.InvariantCulture) + " %";
            labelPower2.Text = _switch.GetSwitchValue((short)enumSwitch.PwrCh2).ToString(CultureInfo.InvariantCulture) + " %";
        }

        private void ReadSensorFile(string filename, out double temp, out double hum, out double dew, out long timestamp)
        {
            temp = 0.0;
            hum = 0.0;
            dew = 0.0;
            timestamp = 0;
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
                        if (vals.Length == 4)
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
                            try
                            {
                                timestamp = long.Parse(vals[3], CultureInfo.InvariantCulture);
                                timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() - timestamp;

                            }
                            catch (Exception ex)
                            {
                                timestamp = 0;
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

        private void buttonSetup_Click(object sender, EventArgs e)
        {
            try
            {
                if (_switcheReady)
                    _switch.SetupDialog();
                else
                {
                    _switch = new Switch("ASCOM.StroblCap.Switch");
                    _switch.SetupDialog();
                    _switcheReady = true;
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void buttonPanelLeft_Click(object sender, EventArgs e)
        {
            if(_leftExpanded)
            {
                _leftExpanded = false;
            }
            else
            {
                _leftExpanded = true;
            }
            ExpandLeft(_leftExpanded);
        }

        private void ExpandLeft(bool expand)
        {
            if(expand)
                panelLeft.Location = new Point(0, 0);
            else
                panelLeft.Location = new Point(-(panelLeft.Width - 18), 0);
        }

        private void ExpandRight(bool expand)
        {
            if (expand)
                panelRight.Location = new Point(this.Width - panelRight.Width - 10, 0);
            else
                panelRight.Location = new Point(this.Width - 35, 0);
        }

        private void ExpandSystem(bool expand)
        {
            if (expand)
                panelSystem.Location = new Point((this.Width - panelSystem.Width) / 2, 0);
            else
                panelSystem.Location = new Point((this.Width - panelSystem.Width) / 2, -(panelSystem.Height - 20));
        }

        private void buttonPanelSystem_Click(object sender, EventArgs e)
        {
            if (_systemExpanded)
            {
                _systemExpanded = false;
            }
            else
            {
                _systemExpanded = true;
            }
            ExpandSystem(_systemExpanded);
        }

        private void buttonPanelRight_Click(object sender, EventArgs e)
        {
            if (_rightExpanded)
            {
                _rightExpanded = false;
            }
            else
            {
                _rightExpanded = true;
            }
            ExpandRight(_rightExpanded);
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if(_switcheReady)
            {
                if(_switch.Connected)
                {
                    _switch.Connected = false;
                    _switch.Dispose();
                    _switcheReady = false;
                    this.buttonConnect.Image = global::EnvironmentPlot.Properties.Resources.Off;
                }
                else
                {
                    _switch.Connected = true;
                    if (_switch.Connected)
                        this.buttonConnect.Image = global::EnvironmentPlot.Properties.Resources.On;
                    else
                        this.buttonConnect.Image = global::EnvironmentPlot.Properties.Resources.Off;
                }
            }
            else
            {
                _switch = new Switch("ASCOM.StroblCap.Switch");
                _switcheReady = true;
                _switch.Connected = true;
                if (_switch.Connected)
                    this.buttonConnect.Image = global::EnvironmentPlot.Properties.Resources.On;
                else
                    this.buttonConnect.Image = global::EnvironmentPlot.Properties.Resources.Off;
            }
            ReadSwitched();
        }

        private void buttonCh1OnOff_Click(object sender, EventArgs e)
        {
            if (!_switcheReady) return;
            if (!_switch.Connected) return;

            if (_switch.GetSwitch((short)enumSwitch.OnOffCh1))
            {
                _switch.SetSwitch((short)enumSwitch.OnOffCh1, false);
                buttonCh1OnOff.Image = global::EnvironmentPlot.Properties.Resources.Off;
            }
            else
            {
                _switch.SetSwitch((short)enumSwitch.OnOffCh1, true);
                buttonCh1OnOff.Image = global::EnvironmentPlot.Properties.Resources.On;
            }
           
        }

        private void numericUpDownChannel1_ValueChanged(object sender, EventArgs e)
        {
            if (!_switcheReady) return;
            if (!_switch.Connected) return;

            _switch.SetSwitchValue((short)enumSwitch.PowerCh1, (double)numericUpDownChannel1.Value);
        }

        private void buttonAuto1_Click(object sender, EventArgs e)
        {
            if (!_switcheReady) return;
            if (!_switch.Connected) return;

            if (_switch.GetSwitch((short)enumSwitch.AutoCh1))
            {
                _switch.SetSwitch((short)enumSwitch.AutoCh1, false);
                buttonAuto1.Image = global::EnvironmentPlot.Properties.Resources.Off;
                _switch.SetSwitchValue((short)enumSwitch.PowerCh1, (double)numericUpDownChannel1.Value);
            }
            else
            {
                _switch.SetSwitch((short)enumSwitch.AutoCh1, true);
                buttonAuto1.Image = global::EnvironmentPlot.Properties.Resources.On;
            }
        }

        private void buttonCh2OnOff_Click(object sender, EventArgs e)
        {
            if (!_switcheReady) return;
            if (!_switch.Connected) return;

            if (_switch.GetSwitch((short)enumSwitch.OnOffCh2))
            {
                _switch.SetSwitch((short)enumSwitch.OnOffCh2, false);
                buttonCh2OnOff.Image = global::EnvironmentPlot.Properties.Resources.Off;
            }
            else
            {
                _switch.SetSwitch((short)enumSwitch.OnOffCh2, true);
                buttonCh2OnOff.Image = global::EnvironmentPlot.Properties.Resources.On;
            }

        }

        private void buttonAuto2_Click(object sender, EventArgs e)
        {
            if (!_switcheReady) return;
            if (!_switch.Connected) return;

            if (_switch.GetSwitch((short)enumSwitch.AutoCh2))
            {
                _switch.SetSwitch((short)enumSwitch.AutoCh2, false);
                buttonAuto2.Image = global::EnvironmentPlot.Properties.Resources.Off;
                _switch.SetSwitchValue((short)enumSwitch.PowerCh2, (double)numericUpDownChannel2.Value);
            }
            else
            {
                _switch.SetSwitch((short)enumSwitch.AutoCh2, true);
                buttonAuto2.Image = global::EnvironmentPlot.Properties.Resources.On;
            }
        }

        private void numericUpDownChannel2_ValueChanged(object sender, EventArgs e)
        {
            if (!_switcheReady) return;
            if (!_switch.Connected) return;

            _switch.SetSwitchValue((short)enumSwitch.PowerCh2, (double)numericUpDownChannel2.Value);
        }
    }
}
