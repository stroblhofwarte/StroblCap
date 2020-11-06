using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
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
        public Plotter()
        {
            InitializeComponent();
            _client = new MqttClient("192.168.42.32");

            // register a callback-function (we have to implement, see below) which is called by the library when a message was received
            _client.MqttMsgPublishReceived += _client_MqttMsgPublishReceived;

            // use a unique id as client id, each time we start the application
            String clientId = Guid.NewGuid().ToString();
            _client.Connect(clientId);
            // Subcribe to the chennels with QoS 1
            String[] topics = { "Astro/StroblCap/Env/ch1/Temp",
                                "Astro/StroblCap/Env/ch1/Dewpoint",
                                "Astro/StroblCap/Env/ch1/Humidity",
                                "Astro/StroblCap/ch1/state",
                                "Astro/StroblCap/Env/ch2/Temp",
                                "Astro/StroblCap/Env/ch2/Dewpoint",
                                "Astro/StroblCap/Env/ch2/Humidity",
                                "Astro/StroblCap/ch2/state"};
            _client.Subscribe(topics, new byte[] { 1, 1, 1, 1, 1, 1, 1, 1});
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
            _client.MqttMsgPublishReceived -= _client_MqttMsgPublishReceived;
            _client.Disconnect();
            
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
    }
}
