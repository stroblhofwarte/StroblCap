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
            Switch._names[2] = textBoxName3.Text;
            Switch._names[3] = textBoxName4.Text;

            Switch._namesDesc[0] = textBoxDesc1.Text;
            Switch._namesDesc[1] = textBoxDesc2.Text;
            Switch._namesDesc[2] = textBoxDesc3.Text;
            Switch._namesDesc[3] = textBoxDesc4.Text;

            Switch._swValues[0] = double.Parse(textBoxDefault1.Text);
            Switch._swValues[1] = double.Parse(textBoxDefault2.Text);
            Switch._swValues[2] = double.Parse(textBoxDefault3.Text);
            Switch._swValues[3] = double.Parse(textBoxDefault4.Text);


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
            textBoxName3.Text = Switch._names[2];
            textBoxName4.Text = Switch._names[3];

            textBoxDesc1.Text = Switch._namesDesc[0];
            textBoxDesc2.Text = Switch._namesDesc[1];
            textBoxDesc3.Text = Switch._namesDesc[2];
            textBoxDesc4.Text = Switch._namesDesc[3];

            textBoxDefault1.Text = Switch._swValues[0].ToString();
            textBoxDefault2.Text = Switch._swValues[1].ToString();
            textBoxDefault3.Text = Switch._swValues[2].ToString();
            textBoxDefault4.Text = Switch._swValues[3].ToString();

        }
    }
}