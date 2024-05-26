using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80LIB
{
    public class Device
    {
        public bool Vcc { get; set; } = false;
        public bool GND { get; set; } = false;
        public bool IsPowerOn => Vcc && !GND;

        public virtual void PowerOn()
        {
            Vcc = true;
            GND = false;
        }

        public virtual void PowerOff()
        {
            Vcc = false;
            GND = false;
        }

        public virtual void Drive()
        {
        }

        public virtual void ExecuteClock()
        {
        }

    }
}
