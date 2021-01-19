using ASCOM.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ASCOM.StroblCap.Switch;

namespace ASCOM.StroblCap
{
    public class Switches
    {
        #region Const

        public readonly int _maxSwitches = 14;

        #endregion

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

        private TraceLogger _log;
        private Dictionary<short, SwitchObj> _switches;

        public string ComPort { get; set; }

        #endregion
        #region Ctor

        public Switches(TraceLogger log)
        {
            _log = log;
            _switches = new Dictionary<short, SwitchObj>();

            SwitchObj sw1 = new SwitchObj((int)enumSwitch.PowerCh1, "AnalogSW1", "Channel 1 power, 1-100%", "50", SwitchObj.enumSwitchType.analog, true);
            SwitchObj sw2 = new SwitchObj((int)enumSwitch.PowerCh2, "AnalogSW2", "Channel 2 power, 1-100%", "50", SwitchObj.enumSwitchType.analog, true);
            SwitchObj sw3 = new SwitchObj((int)enumSwitch.OnOffCh1, "Channel1OnOff", "Channel 1 activation", "true", SwitchObj.enumSwitchType.dio, true);
            SwitchObj sw4 = new SwitchObj((int)enumSwitch.OnOffCh2, "Channel2OnOff", "Channel 2 activation", "true", SwitchObj.enumSwitchType.dio, true);
            SwitchObj sw5 = new SwitchObj((int)enumSwitch.AutoCh1, "Channel1Auto", "Channel 1 auto mode using environmental sensor", "true", SwitchObj.enumSwitchType.dio, true);
            SwitchObj sw6 = new SwitchObj((int)enumSwitch.AutoCh2, "Channel2Auto", "Channel 2 auto mode using environmental sensor", "true", SwitchObj.enumSwitchType.dio, true);
            SwitchObj sw7 = new SwitchObj((int)enumSwitch.TempCh1, "TempCh1", "Temperature of channel 1's sensor", "255", SwitchObj.enumSwitchType.analog, false);
            SwitchObj sw8 = new SwitchObj((int)enumSwitch.HumCh1, "HumidityCh1", "Humidity of channel 1's sensor", "255", SwitchObj.enumSwitchType.analog, false);
            SwitchObj sw9 = new SwitchObj((int)enumSwitch.DewCh1, "DewCh1", "Dewpoint of channel 1's sensor", "255", SwitchObj.enumSwitchType.analog, false);
            SwitchObj sw10 = new SwitchObj((int)enumSwitch.PwrCh1, "PwrCh1", "Power setting of channel 1", "255", SwitchObj.enumSwitchType.analog, false);
            SwitchObj sw11 = new SwitchObj((int)enumSwitch.TempCh2, "TempCh2", "Temperature of channel 2's sensor", "255", SwitchObj.enumSwitchType.analog, false);
            SwitchObj sw12 = new SwitchObj((int)enumSwitch.HumCh2, "HumidityCh2", "Humidity of channel 2's sensor", "255", SwitchObj.enumSwitchType.analog, false);
            SwitchObj sw13 = new SwitchObj((int)enumSwitch.DewCh2, "DewCh2", "Dewpoint of channel 2's sensor", "255", SwitchObj.enumSwitchType.analog, false);
            SwitchObj sw14 = new SwitchObj((int)enumSwitch.PwrCh2, "PwrCh2", "Power setting of channel 2", "255", SwitchObj.enumSwitchType.analog, false);
            _switches.Add(sw1.Id, sw1);
            _switches.Add(sw2.Id, sw2);
            _switches.Add(sw3.Id, sw3);
            _switches.Add(sw4.Id, sw4);
            _switches.Add(sw5.Id, sw5);
            _switches.Add(sw6.Id, sw6);
            _switches.Add(sw7.Id, sw7);
            _switches.Add(sw8.Id, sw8);

            _switches.Add(sw9.Id, sw9);
            _switches.Add(sw10.Id, sw10);
            _switches.Add(sw11.Id, sw11);
            _switches.Add(sw12.Id, sw12);
            _switches.Add(sw13.Id, sw13);
            _switches.Add(sw14.Id, sw14);
        }

        #endregion

        public void Load()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Switch";
                _log.Enabled = Convert.ToBoolean(driverProfile.GetValue(Switch.driverID, Switch.traceStateProfileName, string.Empty, Switch.traceStateDefault));
                ComPort = driverProfile.GetValue(Switch.driverID, Switch.comPortProfileName, string.Empty, Switch.comPortDefault);
                for(int i = 0; i < _maxSwitches; i++)
                {
                    string name = driverProfile.GetValue(Switch.driverID, "name_" + i.ToString(), string.Empty, _switches[(short)i].Name);
                    string desc = driverProfile.GetValue(Switch.driverID, "desc_" + i.ToString(), string.Empty, _switches[(short)i].Description);
                    string value = driverProfile.GetValue(Switch.driverID, "value_" + i.ToString(), string.Empty, _switches[(short)i].Value);
                    _switches[(short)i].Name = name;
                    _switches[(short)i].Description = desc;
                    _switches[(short)i].Value = value;
                }
            }
        }

        public void Save()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Switch";
                driverProfile.WriteValue(Switch.driverID, Switch.traceStateProfileName, _log.Enabled.ToString());
                driverProfile.WriteValue(Switch.driverID, Switch.comPortProfileName, ComPort.ToString());

                for (int i = 0; i < _maxSwitches; i++)
                {
                    driverProfile.WriteValue(Switch.driverID, "name_" + i.ToString(), _switches[(short)i].Name);
                    driverProfile.WriteValue(Switch.driverID, "desc_" + i.ToString(), _switches[(short)i].Description);
                    driverProfile.WriteValue(Switch.driverID, "value_" + i.ToString(), _switches[(short)i].Value);
                }
            }
        }

        public SwitchObj Get(short id)
        {
            if (_switches.ContainsKey(id))
                return _switches[id];
            return null;
        }
    }
}
