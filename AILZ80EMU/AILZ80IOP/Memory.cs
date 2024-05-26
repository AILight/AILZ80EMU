using AILZ80LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80IOP
{
    public class Memory : Device, IAddressBus, IDataBus
    {
        private byte[] _memory;
        public Action<ushort, byte, AccessType>? OnMemoryAccess;

        private Bus Bus { get; set; }

        public Memory(int size, Bus bus) // Z80のアドレス空間は64KB
        {
            _memory = new byte[size];
            Bus = bus;
        }


        public ushort Address
        {
            get => _address;
            set => _address = value;
        }

        public byte Data
        {
            get => ReadByte(_address);
            set => WriteByte(_address, value);
        }
        public bool IsPower { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private ushort _address;

        private byte ReadByte(ushort address)
        {
            byte value = _memory[address];
            OnMemoryAccess?.Invoke(address, value, AccessType.Read);
            return value;
        }

        private void WriteByte(ushort address, byte value)
        {
            _memory[address] = value;
            OnMemoryAccess?.Invoke(address, value, AccessType.Write);
        }

        private ushort ReadWord(ushort address)
        {
            byte lowByte = ReadByte(address);
            byte highByte = ReadByte((ushort)(address + 1));
            return (ushort)((highByte << 8) | lowByte);
        }

        private void WriteWord(ushort address, ushort value)
        {
            WriteByte(address, (byte)(value & 0x00FF)); // Low byte
            WriteByte((ushort)(address + 1), (byte)(value >> 8)); // High byte
        }

        private void Clear()
        {
            Array.Clear(_memory, 0, _memory.Length);
        }

    }
}
