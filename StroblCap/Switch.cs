using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.StroblCap
{
    public class Switch    {
        public enum enumSwitchType
        {
            analog, dio
        }

        #region Properties

        public short Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public enumSwitchType SwType { get; set; }

        public bool CanWrite { get; set; }

        #endregion

        #region Ctor

        public Switch(short id, string name, string description, string value, enumSwitchType type, bool canWrite)
        {
            Id = id;
            Name = name;
            Description = description;
            Value = value;
            SwType = type;
            CanWrite = canWrite;
        }

        #endregion
    }
}
