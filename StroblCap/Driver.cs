//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Switch driver for StroblCap
//
// Description:	The StroblCap is a simple Dew Cap controller to control four caps.
//              It supports two enwironment sensors for Channel 1 and Channel 2. One of 
//              this sensors can be used as weather sensor.
//
// Implements:	ASCOM Switch interface version: 2.0
// Author:		Othmar Ehrhardt, <othmar.ehrhardt@t-online.de>, https://astro.stroblhof-oberrohrbach.de
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 31.01.2021               Weather device interface added.
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
using static ASCOM.StroblCap.Switch;
using System.IO;

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

        public static readonly string sensor1File = "ASCOM.StroblCap.Sensor1";
        public static readonly string sensor2File = "ASCOM.StroblCap.Sensor2";

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
        private Switches _switches;
        public Switches Switches { get { return _switches; } }

        private static System.Timers.Timer _readbackTimer;

        private object _lock = new object();
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
        private TraceLogger tl;

        public TraceLogger Logger { get{ return tl; } }

        private ASCOM.Utilities.Serial _serial;

        /// <summary>
        /// Initializes a new instance of the <see cref="StroblCap"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Switch()
        {
            tl = new TraceLogger("", "StroblCap");
            _switches = new Switches(tl);
            _switches.Load();

            tl.LogMessage("Switch", "Starting initialisation");

            connectedState = false; // Initialise connected to false
            utilities = new Util(); //Initialise util object
            astroUtilities = new AstroUtils(); // Initialise astro-utilities object

            // Startup the readback timer, where all settings from the hardware is readed:
            _readbackTimer = new System.Timers.Timer(10000);
            // Hook up the Elapsed event for the timer. 
            _readbackTimer.Elapsed += _readbackTimer_Elapsed;
            _readbackTimer.AutoReset = true;
            _readbackTimer.Enabled = true;
            
            tl.LogMessage("Switch", "Completed initialisation");
        }

        private void _readbackTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!connectedState) return;
            lock(_lock)
            {
                _serial.Transmit("G1:");
                String ret = _serial.ReceiveTerminated("#");
                String[] values = ret.Split(';');
                double tempCh1 = (double)((double)Int32.Parse(values[0]) / 100.0);
                double humCh1 = (double)((double)Int32.Parse(values[1]) / 100.0);
                double dewCh1 = (double)((double)Int32.Parse(values[2]) / 100.0);
                WriteSensorFile(sensor1File, tempCh1, humCh1, dewCh1);

                _switches.Get((int)Switches.enumSwitch.TempCh1).Value = ((double)Int32.Parse(values[0]) / 100.0).ToString(CultureInfo.InvariantCulture);
                _switches.Get((int)Switches.enumSwitch.HumCh1).Value = ((double)Int32.Parse(values[1]) / 100.0).ToString(CultureInfo.InvariantCulture);
                _switches.Get((int)Switches.enumSwitch.DewCh1).Value = ((double)Int32.Parse(values[2]) / 100.0).ToString(CultureInfo.InvariantCulture);

                _serial.Transmit("G2:");
                ret = _serial.ReceiveTerminated("#");
                values = ret.Split(';');
                double tempCh2 = (double)(double)(Int32.Parse(values[0]) / 100.0);
                double humCh2 = (double)((double)Int32.Parse(values[1]) / 100.0);
                double dewCh2 = (double)((double)Int32.Parse(values[2]) / 100.0);
                WriteSensorFile(sensor2File, tempCh2, humCh2, dewCh2);

                _switches.Get((int)Switches.enumSwitch.TempCh2).Value = ((double)Int32.Parse(values[0]) / 100.0).ToString(CultureInfo.InvariantCulture);
                _switches.Get((int)Switches.enumSwitch.HumCh2).Value = ((double)Int32.Parse(values[1]) / 100.0).ToString(CultureInfo.InvariantCulture);
                _switches.Get((int)Switches.enumSwitch.DewCh2).Value = ((double)Int32.Parse(values[2]) / 100.0).ToString(CultureInfo.InvariantCulture);

                _serial.Transmit("P1:");
                ret = _serial.ReceiveTerminated("#");
                _switches.Get((int)Switches.enumSwitch.PwrCh1).Value = ret.Replace("#", "");

                _serial.Transmit("P2:");
                ret = _serial.ReceiveTerminated("#");
                _switches.Get((int)Switches.enumSwitch.PwrCh2).Value = ret.Replace("#", "");


            }
        }

        private void WriteSensorFile(string filename, double temp, double hum, double dew)
        {
            int retry = 5;
            while (true)
            {
                try
                {
                    string test = Path.GetTempPath() + filename;
                    long timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                    using (FileStream fs = new FileStream(Path.GetTempPath() + filename, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        StreamWriter wr = new StreamWriter(fs);
                        wr.WriteLine(temp.ToString(CultureInfo.InvariantCulture) + ";" + hum.ToString(CultureInfo.InvariantCulture) + ";" + dew.ToString(CultureInfo.InvariantCulture) + ";" + timestamp.ToString());
                        wr.Close();
                        fs.Close();
                    }
                } catch (Exception ex)
                {
                    retry--;
                    continue;
                }
                break;
            }
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

            using (SetupDialogForm F = new SetupDialogForm(this))
            {
                var result = F.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    _switches.Save();
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
            Switches.Save();
            if(_serial != null) _serial.Connected = false;
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
            lock (_lock)
            {
                _serial.Transmit("ID:");
                string ret = _serial.ReceiveTerminated("#");

                if (ret == "STROBLCAP#")
                    return true;
                return false;
            }
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
                    LogMessage("Connected Set", "Connecting to address {0}", _switches.ComPort);
                    try
                    {
                        LogMessage("Connected Set", "Connecting to port {0}", _switches.ComPort);
                        lock (_lock)
                        {
                            _serial = new ASCOM.Utilities.Serial();
                            _serial.PortName = _switches.ComPort;
                            _serial.StopBits = SerialStopBits.One;
                            _serial.Parity = SerialParity.None;
                            _serial.Speed = SerialSpeed.ps115200;
                            _serial.DTREnable = false;
                            _serial.Connected = true;
                            if (CheckForStroblCapDevice())
                            {
                                connectedState = true;
                                InitializeSwitches();
                            }
                            else
                                connectedState = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogMessage("Connected Set", ex.ToString());
                    }
                }
                else
                {
                    connectedState = false;
                    LogMessage("Connected Set", "Disconnecting from adress {0}", _switches.ComPort);
                }
            }
        }

        public string Description
        {
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


        /// <summary>
        /// The number of switches managed by this driver
        /// </summary>
        public short MaxSwitch
        {
            get
            {
                tl.LogMessage("MaxSwitch Get", Switches._maxSwitches.ToString());
                return (short)Switches._maxSwitches;
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
            return _switches.Get(id).Name;
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
            _switches.Get(id).Name = name;
        }

        /// <summary>
        /// Gets the switch description.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public string GetSwitchDescription(short id)
        {
            Validate("GetSwitchDescription", id);
            return _switches.Get(id).Description;
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
            tl.LogMessage("CanWrite", string.Format("CanWrite({0})", id));
            return _switches.Get(id).CanWrite;
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
            if(_switches.Get(id).SwType == SwitchObj.enumSwitchType.analog)
                throw new MethodNotImplementedException("GetSwitch");
            bool ret = bool.Parse(_switches.Get(id).Value);
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
            if (_switches.Get(id).SwType == SwitchObj.enumSwitchType.analog)
                throw new MethodNotImplementedException("SetSwitch");
            SetBooleanSwitch(state, id);
            Switches.Save();
        }

        void SetBooleanSwitch(bool val, short sw)
        {
            String cmd = String.Empty;
            if (val && sw == (int)Switches.enumSwitch.OnOffCh1) cmd = "E1:";
            else if (val && sw == (int)Switches.enumSwitch.OnOffCh2) cmd = "E2:";
            else if (!val && sw == (int)Switches.enumSwitch.OnOffCh1) cmd = "D1:";
            else if (!val && sw == (int)Switches.enumSwitch.OnOffCh2) cmd = "D2:";

            else if (val && sw == (int)Switches.enumSwitch.AutoCh1) cmd = "A1:";
            else if (val && sw == (int)Switches.enumSwitch.AutoCh2) cmd = "A2:";
            else if (!val && sw == (int)Switches.enumSwitch.AutoCh1) cmd = "M1:";
            else if (!val && sw == (int)Switches.enumSwitch.AutoCh2) cmd = "M2:";
            else return;
            lock (_lock)
            {
                _serial.Transmit(cmd);
                String ret = _serial.ReceiveTerminated("#");

                if (ret == "1#")
                {
                    if (val)
                        _switches.Get(sw).Value = "true";
                    else
                        _switches.Get(sw).Value = "false";
                }
            }
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
        private double test = 0.0;
        public double GetSwitchValue(short id)
        {
            Validate("GetSwitchValue", id);
            if (_switches.Get(id).SwType == SwitchObj.enumSwitchType.dio)
                throw new MethodNotImplementedException("GetSwitchValue");
            return double.Parse(_switches.Get(id).Value);
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
            if (_switches.Get(id).SwType == SwitchObj.enumSwitchType.dio)
                throw new MethodNotImplementedException("SetSwitchValue");

            SetAnalogSwitch(value, id);
            Switches.Save();
        }

        void SetAnalogSwitch(double val, short sw)
        {
            String cmd = String.Empty;

            int intVal = (int)val;

            if (sw == (int)Switches.enumSwitch.PowerCh1)
            {
                cmd = "S1" + intVal.ToString("D3") + ":";
            }
            else if (sw == (int)Switches.enumSwitch.PowerCh2)
            {
                cmd = "S2" + intVal.ToString("D3") + ":";
            }
            else return;

            lock (_lock)
            {
                _serial.Transmit(cmd);
                String ret = _serial.ReceiveTerminated("#");

                if (ret == "1#")
                {
                    _switches.Get(sw).Value = val.ToString(CultureInfo.InvariantCulture);
                }
            }
        }


        #endregion
        #endregion

        #region private methods

        private void InitializeSwitches()
        {
            SetSwitch((int)Switches.enumSwitch.AutoCh1, bool.Parse(Switches.Get((int)Switches.enumSwitch.AutoCh1).Value));
            SetSwitch((int)Switches.enumSwitch.OnOffCh1, bool.Parse(Switches.Get((int)Switches.enumSwitch.OnOffCh1).Value));
            SetSwitch((int)Switches.enumSwitch.AutoCh2, bool.Parse(Switches.Get((int)Switches.enumSwitch.AutoCh2).Value));
            SetSwitch((int)Switches.enumSwitch.OnOffCh2, bool.Parse(Switches.Get((int)Switches.enumSwitch.OnOffCh2).Value));

            SetSwitchValue((int)Switches.enumSwitch.PowerCh1, double.Parse(Switches.Get((int)Switches.enumSwitch.PowerCh1).Value, CultureInfo.InvariantCulture));
            SetSwitchValue((int)Switches.enumSwitch.PowerCh2, double.Parse(Switches.Get((int)Switches.enumSwitch.PowerCh2).Value, CultureInfo.InvariantCulture));


        }

        /// <summary>
        /// Checks that the switch id is in range and throws an InvalidValueException if it isn't
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="id">The id.</param>
        private void Validate(string message, short id)
        {
            if (id < 0 || id >= _switches._maxSwitches)
            {
                tl.LogMessage(message, string.Format("Switch {0} not available, range is 0 to {1}", id, Switches._maxSwitches - 1));
                throw new InvalidValueException(message, id.ToString(), string.Format("0 to {0}", Switches._maxSwitches - 1));
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
