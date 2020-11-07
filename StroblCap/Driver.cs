//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Switch driver for StroblCap
//
// Description:	The StroblCap is a simple Dew Cap controller to control four caps.
//              The Device is a aimple WEMOS D1 mini WLAN device, connected to
//              a MQTT broker and supports eight topics:
//                      Astro/StroblCap/ch[1..4] to set the output power between a value of 0-100 (%)
//              and
//                      Astro/StroblCap/ch[1..4]/state to report back the power level (also 0-100).
// 
//              For a complete setup a MQTT broker is required in the network setup of the telescope.
//              For Win10 the mosquitto broker is a good decision. Please do not forgett to open the
//              firewall for the mosquitto process!
//
// Implements:	ASCOM Switch interface version: <To be completed by driver developer>
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	6.0.0	Initial edit, created from ASCOM driver template
// --------------------------------------------------------------------------------
//


// This is used to define code in the template that is specific to one class implementation
// unused code can be deleted and this definition removed.
#define Switch

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;
using System.Globalization;
using System.Collections;
using System.Threading;

namespace ASCOM.StroblCap
{
    //
    // DeviceID is ASCOM.StroblCap.Switch
    //
    // The Guid attribute sets the CLSID for ASCOM.StroblCap.Switch
    // The ClassInterface/None attribute prevents an empty interface called
    // _StroblCap from being created and used as the [default] interface
    //

    /// <summary>
    /// ASCOM Switch Driver for StroblCap.
    /// </summary>
    [Guid("8405433a-edcf-4c74-bd39-62972541afe4")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Switch : ISwitchV2
    {
        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        /// 
         #region Properties

        private enum enumSwitchType
        {
            analog, dio
        }

        private enum enumSwitch
        {
            PowerCh1 = 0,
            PowerCh2 = 1,
            OnOffCh1 = 2,
            OnOffCh2 = 3,
            AutoCh1 = 4,
            AutoCh2 = 5
        }

        internal static string driverID = "ASCOM.StroblCap.Switch";
        // TODO Change the descriptive string for your driver then remove this line
        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private static string driverDescription = "ASCOM Switch Driver for StroblCap.";

        internal static string traceStateProfileName = "Trace Level";
        internal static string traceStateDefault = "false";
        internal static string comPortProfileName = "COM Port"; // Constants used for Profile persistence
        internal static string comPortDefault = "COM1";
        internal static string comPort; // Variables to hold the current device configuration

        internal static string[] _names = { "", "", "", "", "", "" };
        private string[] _namesDefault = {"AnalogSW1", "AnalogSW2", "Channel1OnOff", "Channel1OnOff", "Channel1Auto", "Channel2Auto" };
        private string[] _nameKeys = { "sw1", "sw2", "sw3", "sw4", "sw5", "sw6" };

        internal static string[] _namesDesc = { "", "", "", "", "", "" };
        private string[] _namesDescDefault = { "Channel 1 power, 1-100%", "Channel 2 power, 1-100%", "Channel 1 activation", "Channel 2 activation", "Channel 1 auto mode using environmental sensor", "Channel 2 auto mode using environmental sensor" };
        private string[] _nameDescKeys = { "swDesc1", "swDesc2", "swDesc3", "swDesc4", "swDesc5", "swDesc6" };

        internal static string[] _swValues = { "50.0", "50.0", "true", "true", "true", "true" };
        private string[] _valuesDefault = { "50", "50", "true", "true", "true", "true" };
        private string[] _valuesKeys = { "startup1", "startup2", "startup3", "startup4", "startup5", "startup6" };
        private enumSwitchType[] _switchTypes = { enumSwitchType.analog, enumSwitchType.analog, enumSwitchType.dio, enumSwitchType.dio, enumSwitchType.dio, enumSwitchType.dio };

        #endregion

        /// <summary>
        /// Private variable to hold the connected state
        /// </summary>
        private bool connectedState;

        /// <summary>
        /// Private variable to hold an ASCOM Utilities object
        /// </summary>
        private Util utilities;

        /// <summary>
        /// Private variable to hold an ASCOM AstroUtilities object to provide the Range method
        /// </summary>
        private AstroUtils astroUtilities;

        /// <summary>
        /// Variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        /// </summary>
        internal TraceLogger tl;

        private ASCOM.Utilities.Serial _serial;

        /// <summary>
        /// Initializes a new instance of the <see cref="StroblCap"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Switch()
        {
            tl = new TraceLogger("", "StroblCap");
            ReadProfile(); // Read device configuration from the ASCOM Profile store

            tl.LogMessage("Switch", "Starting initialisation");

            connectedState = false; // Initialise connected to false
            utilities = new Util(); //Initialise util object
            astroUtilities = new AstroUtils(); // Initialise astro-utilities object


            tl.LogMessage("Switch", "Completed initialisation");
        }


        //
        // PUBLIC COM INTERFACE ISwitchV2 IMPLEMENTATION
        //

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            // consider only showing the setup dialog if not connected
            // or call a different dialog if connected
            if (IsConnected)
                System.Windows.Forms.MessageBox.Show("Already connected, just press OK");

            using (SetupDialogForm F = new SetupDialogForm(tl))
            {
                var result = F.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    WriteProfile(); // Persist device configuration values to the ASCOM Profile store
                }
            }
        }

        public ArrayList SupportedActions
        {
            get
            {
                tl.LogMessage("SupportedActions Get", "Returning empty arraylist");
                return new ArrayList();
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            LogMessage("", "Action {0}, parameters {1} not implemented", actionName, actionParameters);
            throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        }

        public void CommandBlind(string command, bool raw)
        {
            CheckConnected("CommandBlind");
            // Call CommandString and return as soon as it finishes
            this.CommandString(command, raw);
            // or
            //throw new ASCOM.MethodNotImplementedException("CommandBlind");
            // DO NOT have both these sections!  One or the other
        }

        public bool CommandBool(string command, bool raw)
        {
            CheckConnected("CommandBool");
            string ret = CommandString(command, raw);
            return true;
            // TODO decode the return string and return true or false
            // or
            //throw new ASCOM.MethodNotImplementedException("CommandBool");
            // DO NOT have both these sections!  One or the other
        }

        public string CommandString(string command, bool raw)
        {
            CheckConnected("CommandString");
            // it's a good idea to put all the low level communication with the device here,
            // then all communication calls this function
            // you need something to ensure that only one command is in progress at a time

            throw new ASCOM.MethodNotImplementedException("CommandString");
        }

        public void Dispose()
        {
            // Clean up the trace logger and util objects
            tl.Enabled = false;
            tl.Dispose();
            tl = null;
            utilities.Dispose();
            utilities = null;
            astroUtilities.Dispose();
            astroUtilities = null;
        }


        private bool CheckForStroblCapDevice()
        {
            _serial.Connected = true;
            _serial.Transmit("ID:");
            string ret = _serial.ReceiveTerminated("#");
            _serial.Connected = false;
            if (ret == "STROBLCAP#")
                return true;
            return false;
        }
        public bool Connected
        {
            get
            {
                LogMessage("Connected", "Get {0}", IsConnected);
                return IsConnected;
            }
            set
            {
                tl.LogMessage("Connected", "Set {0}", value);
                if (value == IsConnected)
                    return;

                if (value)
                {
                    LogMessage("Connected Set", "Connecting to address {0}", comPort);
                    try
                    {
                        LogMessage("Connected Set", "Connecting to port {0}", comPort);
                        _serial = new ASCOM.Utilities.Serial();
                        _serial.PortName = comPort;
                        _serial.StopBits = SerialStopBits.One;
                        _serial.Parity = SerialParity.None;
                        _serial.Speed = SerialSpeed.ps115200;
                        _serial.DTREnable = false;

                        if (CheckForStroblCapDevice())
                            connectedState = true;
                        else
                            connectedState = false;
                    }
                    catch (Exception ex)
                    {
                        LogMessage("Connected Set", ex.ToString());
                    }
                }
                else
                {
                    connectedState = false;
                    LogMessage("Connected Set", "Disconnecting from adress {0}", comPort);
                }
            }
        }

        public string Description
        {
            // TODO customise this device description
            get
            {
                tl.LogMessage("Description Get", driverDescription);
                return driverDescription;
            }
        }

        public string DriverInfo
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                // TODO customise this driver description
                string driverInfo = "Information about the driver itself. Version: " + String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        public short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                LogMessage("InterfaceVersion Get", "2");
                return Convert.ToInt16("2");
            }
        }

        public string Name
        {
            get
            {
                string name = "Short driver name - please customise";
                tl.LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion

        #region ISwitchV2 Implementation

        private short numSwitch = 6;

        /// <summary>
        /// The number of switches managed by this driver
        /// </summary>
        public short MaxSwitch
        {
            get
            {
                tl.LogMessage("MaxSwitch Get", numSwitch.ToString());
                return this.numSwitch;
            }
        }

        /// <summary>
        /// Return the name of switch n
        /// </summary>
        /// <param name="id">The switch number to return</param>
        /// <returns>
        /// The name of the switch
        /// </returns>
        public string GetSwitchName(short id)
        {
            Validate("GetSwitchName", id);
            tl.LogMessage("GetSwitchName", string.Format("GetSwitchName({0}) - default Switch{0}", id));
            return _names[id];
        }

        /// <summary>
        /// Sets a switch name to a specified value
        /// </summary>
        /// <param name="id">The number of the switch whose name is to be set</param>
        /// <param name="name">The name of the switch</param>
        public void SetSwitchName(short id, string name)
        {
            Validate("SetSwitchName", id);
            tl.LogMessage("SetSwitchName", string.Format("SetSwitchName({0}) = {1} - not implemented", id, name));
            _names[id] = name;
        }

        /// <summary>
        /// Gets the switch description.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public string GetSwitchDescription(short id)
        {
            Validate("GetSwitchDescription", id);
            return _namesDesc[id];
        }

        /// <summary>
        /// Reports if the specified switch can be written to.
        /// This is false if the switch cannot be written to, for example a limit switch or a sensor.
        /// The default is true.
        /// </summary>
        /// <param name="id">The number of the switch whose write state is to be returned</param><returns>
        ///   <c>true</c> if the switch can be written to, otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
        public bool CanWrite(short id)
        {
            Validate("CanWrite", id);
            // default behavour is to report true
            tl.LogMessage("CanWrite", string.Format("CanWrite({0}) - default true", id));
            return true;
        }

        #region boolean switch members

        /// <summary>
        /// Return the state of switch n
        /// a multi-value switch must throw a not implemented exception
        /// </summary>
        /// <param name="id">The switch number to return</param>
        /// <returns>
        /// True or false
        /// </returns>
        public bool GetSwitch(short id)
        {
            Validate("GetSwitch", id);
            tl.LogMessage("GetSwitch", string.Format("GetSwitch({0}) - not implemented", id));
            if(_switchTypes[id] == enumSwitchType.analog)
                throw new MethodNotImplementedException("GetSwitch");
            bool ret = bool.Parse(_swValues[id]);
            return ret;
        }

        /// <summary>
        /// Sets a switch to the specified state
        /// If the switch cannot be set then throws a MethodNotImplementedException.
        /// A multi-value switch must throw a not implemented exception
        /// setting it to false will set it to its minimum value.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        public void SetSwitch(short id, bool state)
        {
            Validate("SetSwitch", id);
            if (!CanWrite(id))
            {
                var str = string.Format("SetSwitch({0}) - Cannot Write", id);
                tl.LogMessage("SetSwitch", str);
                throw new MethodNotImplementedException(str);
            }
            if (_switchTypes[id] == enumSwitchType.analog)
                throw new MethodNotImplementedException("SetSwitch");
            SetBooleanSwitch(state, (enumSwitch)id);
        }

        void SetBooleanSwitch(bool val, enumSwitch sw)
        {
            _serial.Connected = true;
            String cmd = String.Empty;
            if (val && sw == enumSwitch.OnOffCh1) cmd = "E1:";
            if (val && sw == enumSwitch.OnOffCh2) cmd = "E2:";
            if (!val && sw == enumSwitch.OnOffCh1) cmd = "D1:";
            if (!val && sw == enumSwitch.OnOffCh2) cmd = "D2:";

            if (val && sw == enumSwitch.AutoCh1) cmd = "A1:";
            if (val && sw == enumSwitch.AutoCh2) cmd = "A2:";
            if (!val && sw == enumSwitch.AutoCh1) cmd = "M1:";
            if (!val && sw == enumSwitch.AutoCh2) cmd = "M2:";

            _serial.Transmit(cmd);
            String ret = _serial.ReceiveTerminated("#");
            if(ret == "1#")
            {
                if (val)
                    _swValues[(int)sw] = "true";
                else
                    _swValues[(int)sw] = "false";
            }
            _serial.Connected = false;
        }

        #endregion

        #region analogue members

        /// <summary>
        /// returns the maximum value for this switch
        /// boolean switches must return 1.0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double MaxSwitchValue(short id)
        {
            Validate("MaxSwitchValue", id);
            return 100;
           
        }

        /// <summary>
        /// returns the minimum value for this switch
        /// boolean switches must return 0.0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double MinSwitchValue(short id)
        {
            Validate("MinSwitchValue", id);
            return 0;
        }

        /// <summary>
        /// returns the step size that this switch supports. This gives the difference between
        /// successive values of the switch.
        /// The number of values is ((MaxSwitchValue - MinSwitchValue) / SwitchStep) + 1
        /// boolean switches must return 1.0, giving two states.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double SwitchStep(short id)
        {
            Validate("SwitchStep", id);
            return 1;

        }

        /// <summary>
        /// returns the analogue switch value for switch id
        /// boolean switches must throw a not implemented exception
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double GetSwitchValue(short id)
        {
            Validate("GetSwitchValue", id);
            if (_switchTypes[id] == enumSwitchType.dio)
                throw new MethodNotImplementedException("GetSwitchValue");
            return double.Parse(_swValues[id]);
        }

        /// <summary>
        /// set the analogue value for this switch.
        /// If the switch cannot be set then throws a MethodNotImplementedException.
        /// If the value is not between the maximum and minimum then throws an InvalidValueException
        /// boolean switches must throw a not implemented exception.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void SetSwitchValue(short id, double value)
        {
            Validate("SetSwitchValue", id, value);
            if (!CanWrite(id))
            {
                tl.LogMessage("SetSwitchValue", string.Format("SetSwitchValue({0}) - Cannot write", id));
                return;
            }
            if (_switchTypes[id] == enumSwitchType.dio)
                throw new MethodNotImplementedException("SetSwitchValue");

            SetAnalogSwitch(value, (enumSwitch)id);
        }

        void SetAnalogSwitch(double val, enumSwitch sw)
        {
            _serial.Connected = true;
            String cmd = String.Empty;

            int intVal = (int)val;

            if(sw == enumSwitch.PowerCh1)
            {
                cmd = "S1" + intVal.ToString("D3") + ":";
            }
            if (sw == enumSwitch.PowerCh2)
            {
                cmd = "S2" + intVal.ToString("D3") + ":";
            }

            _serial.Transmit(cmd);
            String ret = _serial.ReceiveTerminated("#");
            if (ret == "1#")
            {
                _swValues[(int)sw] = val.ToString(CultureInfo.InvariantCulture);
            }
            _serial.Connected = false;
        }


        #endregion
        #endregion

        #region private methods

        /// <summary>
        /// Checks that the switch id is in range and throws an InvalidValueException if it isn't
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="id">The id.</param>
        private void Validate(string message, short id)
        {
            if (id < 0 || id >= numSwitch)
            {
                tl.LogMessage(message, string.Format("Switch {0} not available, range is 0 to {1}", id, numSwitch - 1));
                throw new InvalidValueException(message, id.ToString(), string.Format("0 to {0}", numSwitch - 1));
            }
        }

        /// <summary>
        /// Checks that the switch id and value are in range and throws an
        /// InvalidValueException if they are not.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="id">The id.</param>
        /// <param name="value">The value.</param>
        private void Validate(string message, short id, double value)
        {
            Validate(message, id);
            var min = MinSwitchValue(id);
            var max = MaxSwitchValue(id);
            if (value < min || value > max)
            {
                tl.LogMessage(message, string.Format("Value {1} for Switch {0} is out of the allowed range {2} to {3}", id, value, min, max));
                throw new InvalidValueException(message, value.ToString(), string.Format("Switch({0}) range {1} to {2}", id, min, max));
            }
        }

        /// <summary>
        /// Checks that the number of states for the switch is correct and throws a methodNotImplemented exception if not.
        /// Boolean switches must have 2 states and multi-value switches more than 2.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="id"></param>
        /// <param name="expectBoolean"></param>
        //private void Validate(string message, short id, bool expectBoolean)
        //{
        //    Validate(message, id);
        //    var ns = (int)(((MaxSwitchValue(id) - MinSwitchValue(id)) / SwitchStep(id)) + 1);
        //    if ((expectBoolean && ns != 2) || (!expectBoolean && ns <= 2))
        //    {
        //        tl.LogMessage(message, string.Format("Switch {0} has the wriong number of states", id, ns));
        //        throw new MethodNotImplementedException(string.Format("{0}({1})", message, id));
        //    }
        //}

        #endregion

        #region Private properties and methods
        // here are some useful properties and methods that can be used as required
        // to help with driver development

        #region ASCOM Registration

        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        /// <summary>
        /// Register or unregister the driver with the ASCOM Platform.
        /// This is harmless if the driver is already registered/unregistered.
        /// </summary>
        /// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        private static void RegUnregASCOM(bool bRegister)
        {
            using (var P = new ASCOM.Utilities.Profile())
            {
                P.DeviceType = "Switch";
                if (bRegister)
                {
                    P.Register(driverID, driverDescription);
                }
                else
                {
                    P.Unregister(driverID);
                }
            }
        }

        /// <summary>
        /// This function registers the driver with the ASCOM Chooser and
        /// is called automatically whenever this class is registered for COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is successfully built.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During setup, when the installer registers the assembly for COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually register a driver with ASCOM.
        /// </remarks>
        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        /// <summary>
        /// This function unregisters the driver from the ASCOM Chooser and
        /// is called automatically whenever this class is unregistered from COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is cleaned or prior to rebuilding.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
        /// </remarks>
        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }

        #endregion

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool IsConnected
        {
            get
            {
                // TODO check that the driver hardware connection exists and is connected to the hardware
                return connectedState;
            }
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new ASCOM.NotConnectedException(message);
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal void ReadProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Switch";
                tl.Enabled = Convert.ToBoolean(driverProfile.GetValue(driverID, traceStateProfileName, string.Empty, traceStateDefault));
                comPort = driverProfile.GetValue(driverID, comPortProfileName, string.Empty, comPortDefault);
                for (int i = 0; i < numSwitch; i++)
                    _names[i] = driverProfile.GetValue(driverID, _nameKeys[i], string.Empty, _namesDefault[i]);
                for (int i = 0; i < numSwitch; i++)
                    _namesDesc[i] = driverProfile.GetValue(driverID, _nameDescKeys[i], string.Empty, _namesDescDefault[i]);
                for (int i = 0; i < numSwitch; i++)
                    _swValues[i] = driverProfile.GetValue(driverID, _valuesKeys[i], string.Empty, _valuesDefault[i]);
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal void WriteProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Switch";
                driverProfile.WriteValue(driverID, traceStateProfileName, tl.Enabled.ToString());
                driverProfile.WriteValue(driverID, comPortProfileName, comPort.ToString());
                for (int i = 0; i < numSwitch; i++)
                    driverProfile.WriteValue(driverID, _nameKeys[i], _names[i]);
                for (int i = 0; i < numSwitch; i++)
                    driverProfile.WriteValue(driverID, _nameDescKeys[i], _namesDesc[i]);
                for (int i = 0; i < numSwitch; i++)
                    driverProfile.WriteValue(driverID, _valuesKeys[i], _swValues[i].ToString());
            }
        }

        /// <summary>
        /// Log helper function that takes formatted strings and arguments
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal void LogMessage(string identifier, string message, params object[] args)
        {
            var msg = string.Format(message, args);
            tl.LogMessage(identifier, msg);
        }
        #endregion

        #region CommandImpl


        #endregion

    }
}
