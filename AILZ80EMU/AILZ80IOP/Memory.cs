using AILZ80LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80IOP
{
    public class Memory : Device
    {
        private byte[] MemoryBytes { get; set; }
        private Bus Bus { get; set; }

        public Memory(int size, Bus bus) // Z80のアドレス空間は64KB
        {
            MemoryBytes = new byte[size];
            Bus = bus;
        }

        public override void ExecuteClock(bool clockState)
        {
            base.ExecuteClock(clockState);

            if (!Bus.MREQ)
            {
                if (!Bus.RD)
                {
                    Bus.Data = MemoryBytes[Bus.Address];
                }
                if (!Bus.WR)
                {
                    MemoryBytes[Bus.Address] = Bus.Data;
                }
            }
        }
            
        private void Clear()
        {
            Array.Clear(MemoryBytes, 0, MemoryBytes.Length);
        }

    }
}
