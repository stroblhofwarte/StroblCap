using ASCOM.StroblCap;
using ASCOM.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.StroblCap
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogObservingConditionsForm : Form
    {
        TraceLogger tl; // Holder for a reference to the driver's trace logger

        public SetupDialogObservingConditionsForm(TraceLogger tlDriver)
        {
            InitializeComponent();

            // Save the provided trace logger for use within the setup dialogue
            tl = tlDriver;

            // Initialise current values of user settings from the ASCOM Profile
            InitUI();
        }

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here
            // Update the state variables with results from the dialogue
            ObservingConditions.channel1forWeather = false;
            ObservingConditions.channel2forWeather = false;
            if (radioButtonChannel1.Checked)
                ObservingConditions.channel1forWeather = true;
            if (radioButtonChannel2.Checked)
                ObservingConditions.channel2forWeather = true;

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

            radioButtonNothing.Checked = true;
            if(ObservingConditions.channel1forWeather)
            {
                radioButtonChannel1.Checked = true;
            }
            if (ObservingConditions.channel2forWeather)
            {
                radioButtonChannel2.Checked = true;
            }
        }
    }
}