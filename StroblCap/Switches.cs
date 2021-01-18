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
        private Dictionary<short, Switch> _switches;

        public string ComPort { get; set; }

        #endregion
        #region Ctor

        public Switches(TraceLogger log)
        {
            _log = log;
            _switches = new Dictionary<short, Switch>();

            Switch sw1 = new Switch((int)enumSwitch.PowerCh1, "AnalogSW1", "Channel 1 power, 1-100%", "50", enumSwitchType.analog, true);
            Switch sw2 = new Switch((int)enumSwitch.PowerCh2, "AnalogSW2", "Channel 2 power, 1-100%", "50", enumSwitchType.analog, true);
            Switch sw3 = new Switch((int)enumSwitch.OnOffCh1, "Channel1OnOff", "Channel 1 activation", "true", enumSwitchType.dio, true);
            Switch sw4 = new Switch((int)enumSwitch.OnOffCh2, "Channel2OnOff", "Channel 2 activation", "true", enumSwitchType.dio, true);
            Switch sw5 = new Switch((int)enumSwitch.AutoCh1, "Channel1Auto", "Channel 1 auto mode using environmental sensor", "true", enumSwitchType.dio, true);
            Switch sw6 = new Switch((int)enumSwitch.AutoCh2, "Channel2Auto", "Channel 2 auto mode using environmental sensor", "true", enumSwitchType.dio, true);
            Switch sw7 = new Switch((int)enumSwitch.TempCh1, "TempCh1", "Temperature of channel 1's sensor", "255", enumSwitchType.analog, false);
            Switch sw8 = new Switch((int)enumSwitch.HumCh1, "HumidityCh1", "Humidity of channel 1's sensor", "255", enumSwitchType.analog, false);
            Switch sw9 = new Switch((int)enumSwitch.DewCh1, "DewCh1", "Dewpoint of channel 1's sensor", "255", enumSwitchType.analog, false);
            Switch sw10 = new Switch((int)enumSwitch.PwrCh1, "PwrCh1", "Power setting of channel 1", "255", enumSwitchType.analog, false);
            Switch sw11 = new Switch((int)enumSwitch.TempCh2, "TempCh2", "Temperature of channel 2's sensor", "255", enumSwitchType.analog, false);
            Switch sw12 = new Switch((int)enumSwitch.HumCh2, "HumidityCh2", "Humidity of channel 2's sensor", "255", enumSwitchType.analog, false);
            Switch sw13 = new Switch((int)enumSwitch.DewCh2, "DewCh2", "Dewpoint of channel 2's sensor", "255", enumSwitchType.analog, false);
            Switch sw14 = new Switch((int)enumSwitch.PwrCh2, "PwrCh2", "Power setting of channel 2", "255", enumSwitchType.analog, false);
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
                _log.Enabled = Convert.ToBoolean(driverProfile.GetValue(SwitchObj.driverID, SwitchObj.traceStateProfileName, string.Empty, SwitchObj.traceStateDefault));
                ComPort = driverProfile.GetValue(SwitchObj.driverID, SwitchObj.comPortProfileName, string.Empty, SwitchObj.comPortDefault);
                for(int i = 0; i < _maxSwitches; i++)
                {
                    string name = driverProfile.GetValue(SwitchObj.driverID, "name_" + i.ToString(), string.Empty, _switches[(short)i].Name);
                    string desc = driverProfile.GetValue(SwitchObj.driverID, "desc_" + i.ToString(), string.Empty, _switches[(short)i].Description);
                    string value = driverProfile.GetValue(SwitchObj.driverID, "value_" + i.ToString(), string.Empty, _switches[(short)i].Value);
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
                driverProfile.WriteValue(SwitchObj.driverID, SwitchObj.traceStateProfileName, _log.Enabled.ToString());
                driverProfile.WriteValue(SwitchObj.driverID, SwitchObj.comPortProfileName, ComPort.ToString());

                for (int i = 0; i < _maxSwitches; i++)
                {
                    driverProfile.WriteValue(SwitchObj.driverID, "name_" + i.ToString(), _switches[(short)i].Name);
                    driverProfile.WriteValue(SwitchObj.driverID, "desc_" + i.ToString(), _switches[(short)i].Description);
                    driverProfile.WriteValue(SwitchObj.driverID, "value_" + i.ToString(), _switches[(short)i].Value);
                }
            }
        }

        public Switch Get(short id)
        {
            if (_switches.ContainsKey(id))
                return _switches[id];
            return null;
        }
    }
}
