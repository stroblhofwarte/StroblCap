using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.Utilities;
using ASCOM.StroblCap;

namespace ASCOM.StroblCap
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        TraceLogger tl; // Holder for a reference to the driver's trace logger
        SwitchObj _driver;
        public SetupDialogForm(SwitchObj driver)
        {
            _driver = driver;
            InitializeComponent();

            // Save the provided trace logger for use within the setup dialogue
            tl = driver.Logger;

            // Initialise current values of user settings from the ASCOM Profile
            InitUI();
        }

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here
            // Update the state variables with results from the dialogue
            _driver.Switches.ComPort = (string)comboBoxComPort.SelectedItem;

            _driver.Switches.Get((int)Switches.enumSwitch.PowerCh1).Name = textBoxName1.Text;
            _driver.Switches.Get((int)Switches.enumSwitch.PowerCh2).Name = textBoxName2.Text;
            _driver.Switches.Get((int)Switches.enumSwitch.OnOffCh1).Name = textBoxName1.Text + " Activation";
            _driver.Switches.Get((int)Switches.enumSwitch.OnOffCh2).Name = textBoxName2.Text + " Activation";
            _driver.Switches.Get((int)Switches.enumSwitch.AutoCh1).Name = textBoxName1.Text + " Auto";
            _driver.Switches.Get((int)Switches.enumSwitch.AutoCh2).Name = textBoxName2.Text + " Auto";
            _driver.Switches.Get((int)Switches.enumSwitch.TempCh1).Name = textBoxName1.Text + " Temp.";
            _driver.Switches.Get((int)Switches.enumSwitch.HumCh1).Name = textBoxName1.Text + " Humidity";
            _driver.Switches.Get((int)Switches.enumSwitch.DewCh1).Name = textBoxName1.Text + " Dewpoint";
            _driver.Switches.Get((int)Switches.enumSwitch.PwrCh1).Name = textBoxName1.Text + " Power";

            _driver.Switches.Get((int)Switches.enumSwitch.PowerCh1).Description = textBoxDesc1.Text;
            _driver.Switches.Get((int)Switches.enumSwitch.PowerCh2).Description = textBoxDesc2.Text;
            _driver.Switches.Get((int)Switches.enumSwitch.OnOffCh1).Description = "Activation of the " + textBoxName1.Text + " Channel at all";
            _driver.Switches.Get((int)Switches.enumSwitch.OnOffCh2).Description = "Activation of the " + textBoxName2.Text + " Channel at all";
            _driver.Switches.Get((int)Switches.enumSwitch.AutoCh1).Description = "Use environmental sensor to control the " + textBoxName1.Text + "Channel";
            _driver.Switches.Get((int)Switches.enumSwitch.AutoCh2).Description = "Use environmental sensor to control the " + textBoxName2.Text + "Channel";
            _driver.Switches.Get((int)Switches.enumSwitch.TempCh1).Description = "Temperature in °C";
            _driver.Switches.Get((int)Switches.enumSwitch.HumCh1).Description = "Humidity in %";
            _driver.Switches.Get((int)Switches.enumSwitch.DewCh1).Description = "Dewpoint in °C";
            _driver.Switches.Get((int)Switches.enumSwitch.PwrCh1).Description = "Powersetting in %";

            _driver.Switches.Get((int)Switches.enumSwitch.PowerCh1).Value = textBoxDefault1.Text;
            _driver.Switches.Get((int)Switches.enumSwitch.PowerCh2).Value = textBoxDefault2.Text;
            _driver.Switches.Get((int)Switches.enumSwitch.OnOffCh1).Value = "true";
            _driver.Switches.Get((int)Switches.enumSwitch.OnOffCh2).Value = "true";
            _driver.Switches.Get((int)Switches.enumSwitch.AutoCh1).Value = "true";
            _driver.Switches.Get((int)Switches.enumSwitch.AutoCh2).Value = "true";

            tl.Enabled = chkTrace.Checked;
        }

        private void cmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Close();
        }

        private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
        {
            try
            {
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void InitUI()
        {
            chkTrace.Checked = tl.Enabled;
            // set the list of com ports to those that are currently available
            comboBoxComPort.Items.Clear();
            comboBoxComPort.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());      // use System.IO because it's static
            // select the current port if possible
            if (comboBoxComPort.Items.Contains(_driver.Switches.ComPort))
            {
                comboBoxComPort.SelectedItem = _driver.Switches.ComPort;
            }

            textBoxName1.Text = _driver.Switches.Get((int)Switches.enumSwitch.PowerCh1).Name;
            textBoxName2.Text = _driver.Switches.Get((int)Switches.enumSwitch.PowerCh2).Name;


            textBoxDesc1.Text = _driver.Switches.Get((int)Switches.enumSwitch.PowerCh1).Description;
            textBoxDesc2.Text = _driver.Switches.Get((int)Switches.enumSwitch.PowerCh2).Description;

            textBoxDefault1.Text = _driver.Switches.Get((int)Switches.enumSwitch.PowerCh1).Value;
            textBoxDefault2.Text = _driver.Switches.Get((int)Switches.enumSwitch.PowerCh2).Value;



        }
    }
}