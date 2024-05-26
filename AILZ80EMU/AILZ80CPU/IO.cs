using AILZ80LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU
{
    public class IO
    {
        private byte[] _ioPorts;
        public Action<byte, byte, AccessType>? OnIOAccess;

        private Bus Bus { get; set; }

        public IO(int size, Bus bus) // Z80のI/Oポートは256個
        {
            _ioPorts = new byte[size];
            Bus = bus;
        }

        public byte ReadPort(byte port)
        {
            byte value = _ioPorts[port];
            OnIOAccess?.Invoke(port, value, AccessType.Read);
            return value;
        }

        public void WritePort(byte port, byte value)
        {
            _ioPorts[port] = value;
            OnIOAccess?.Invoke(port, value, AccessType.Write);
        }

        public void Clear()
        {
            Array.Clear(_ioPorts, 0, _ioPorts.Length);
        }
    }
}
