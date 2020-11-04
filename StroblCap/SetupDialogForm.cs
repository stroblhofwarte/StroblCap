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

        public SetupDialogForm(TraceLogger tlDriver)
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
            Switch.ipAddress = (string)textBoxIpAddress.Text;
            Switch._names[0] = textBoxName1.Text;
            Switch._names[1] = textBoxName2.Text;
            Switch._names[2] = textBoxName1.Text + " Activation";
            Switch._names[3] = textBoxName2.Text + " Activation";
            Switch._names[4] = textBoxName1.Text + " Auto";
            Switch._names[5] = textBoxName2.Text + " Auto";

            Switch._namesDesc[0] = textBoxDesc1.Text;
            Switch._namesDesc[1] = textBoxDesc2.Text;
            Switch._namesDesc[2] = "Activation of the " + textBoxName1.Text + " Channel at all";
            Switch._namesDesc[3] = "Activation of the " + textBoxName2.Text + " Channel at all";
            Switch._namesDesc[4] = "Use environmental sensor to control the " + textBoxName1.Text + "Channel";
            Switch._namesDesc[5] = "Use environmental sensor to control the " + textBoxName2.Text + "Channel";

            Switch._swValues[0] = textBoxDefault1.Text;
            Switch._swValues[1] = textBoxDefault2.Text;
            Switch._swValues[2] = "true";
            Switch._swValues[3] = "true";
            Switch._swValues[4] = "true";
            Switch._swValues[5] = "true";

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
            textBoxIpAddress.Text = Switch.ipAddress;

            textBoxName1.Text = Switch._names[0];
            textBoxName2.Text = Switch._names[1];
          
            textBoxDesc1.Text = Switch._namesDesc[0];
            textBoxDesc2.Text = Switch._namesDesc[1];     

            textBoxDefault1.Text = Switch._swValues[0].ToString();
            textBoxDefault2.Text = Switch._swValues[1].ToString();
         

        }
    }
}