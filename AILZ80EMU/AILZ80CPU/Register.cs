using AILZ80LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU
{
    public class Register
    {
        // 主レジスタ
        private ushort _af;
        private ushort _bc;
        private ushort _de;
        private ushort _hl;

        // 裏レジスタ
        private ushort _af_s;
        private ushort _bc_s;
        private ushort _de_s;
        private ushort _hl_s;

        // インデックスレジスタ
        private ushort _ix;
        private ushort _iy;

        // スタックポインタとプログラムカウンタ
        private ushort _sp;
        private ushort _pc;

        // その他のレジスタ
        private byte _i;
        private byte _r;

        // トラップハンドラ
        public Action<RegisterEnum, AccessType, ushort>? OnRegisterAccess;

        public ushort AF
        {
            get
            {
                OnRegisterAccess?.Invoke(RegisterEnum.AF, AccessType.Read, _af);
                return _af;
            }
            private set
            {
                _af = value;
                OnRegisterAccess?.Invoke(RegisterEnum.AF, AccessType.Write, _af);
            }
        }

        public ushort BC
        {
            get
            {
                OnRegisterAccess?.Invoke(RegisterEnum.BC, AccessType.Read, _bc);
                return _bc;
            }
            private set
            {
                _bc = value;
                OnRegisterAccess?.Invoke(RegisterEnum.BC, AccessType.Write, _bc);
            }
        }

        public ushort DE
        {
            get
            {
                OnRegisterAccess?.Invoke(RegisterEnum.DE, AccessType.Read, _de);
                return _de;
            }
            private set
            {
                _de = value;
                OnRegisterAccess?.Invoke(RegisterEnum.DE, AccessType.Write, _de);
            }
        }

        public ushort HL
        {
            get
            {
                OnRegisterAccess?.Invoke(RegisterEnum.HL, AccessType.Read, _hl);
                return _hl;
            }
            private set
            {
                _hl = value;
                OnRegisterAccess?.Invoke(RegisterEnum.HL, AccessType.Write, _hl);
            }
        }

        public ushort AF_S
        {
            get
            {
                OnRegisterAccess?.Invoke(RegisterEnum.AF_S, AccessType.Read, _af_s);
                return _af_s;
            }
            private set
            {
                _af_s = value;
                OnRegisterAccess?.Invoke(RegisterEnum.AF_S, AccessType.Write, _af_s);
            }
        }

        public ushort BC_S
        {
            get
            {
                OnRegisterAccess?.Invoke(RegisterEnum.BC_S, AccessType.Read, _bc_s);
                return _bc_s;
            }
            private set
            {
                _bc_s = value;
                OnRegisterAccess?.Invoke(RegisterEnum.BC_S, AccessType.Write, _bc_s);
            }
        }

        public ushort DE_S
        {
            get
            {
                OnRegisterAccess?.Invoke(RegisterEnum.DE_S, AccessType.Read, _de_s);
                return _de_s;
            }
            private set
            {
                _de_s = value;
                OnRegisterAccess?.Invoke(RegisterEnum.DE_S, AccessType.Write, _de_s);
            }
        }

        public ushort HL_S
        {
            get
            {
                OnRegisterAccess?.Invoke(RegisterEnum.HL_S, AccessType.Read, _hl_s);
                return _hl_s;
            }
            private set
            {
                _hl_s = value;
                OnRegisterAccess?.Invoke(RegisterEnum.HL_S, AccessType.Write, _hl_s);
            }
        }

        public ushort IX
        {
            get
            {
                OnRegisterAccess?.Invoke(RegisterEnum.IX, AccessType.Read, _ix);
                return _ix;
            }
            private set
            {
                _ix = value;
                OnRegisterAccess?.Invoke(RegisterEnum.IX, AccessType.Write, _ix);
            }
        }

        public ushort IY
        {
            get
            {
                OnRegisterAccess?.Invoke(RegisterEnum.IY, AccessType.Read, _iy);
                return _iy;
            }
            private set
            {
                _iy = value;
                OnRegisterAccess?.Invoke(RegisterEnum.IY, AccessType.Write, _iy);
            }
        }

        public ushort SP
        {
            get
            {
                OnRegisterAccess?.Invoke(RegisterEnum.SP, AccessType.Read, _sp);
                return _sp;
            }
            set
            {
                _sp = value;
                OnRegisterAccess?.Invoke(RegisterEnum.SP, AccessType.Write, _sp);
            }
        }

        public ushort PC
        {
            get
            {
                OnRegisterAccess?.Invoke(RegisterEnum.PC, AccessType.Read, _pc);
                return _pc;
            }
            set
            {
                _pc = value;
                OnRegisterAccess?.Invoke(RegisterEnum.PC, AccessType.Write, _pc);
            }
        }

        public byte I
        {
            get
            {
                OnRegisterAccess?.Invoke(RegisterEnum.I, AccessType.Read, _i);
                return _i;
            }
            set
            {
                _i = value;
                OnRegisterAccess?.Invoke(RegisterEnum.I, AccessType.Write, _i);
            }
        }

        public byte R
        {
            get
            {
                OnRegisterAccess?.Invoke(RegisterEnum.R, AccessType.Read, _r);
                return _r;
            }
            set
            {
                _r = value;
                OnRegisterAccess?.Invoke(RegisterEnum.R, AccessType.Write, _r);
            }
        }

        public byte A
        {
            get => (byte)(AF >> 8);
            set => AF = (ushort)((value << 8) | (AF & 0x00FF));
        }

        public byte F
        {
            get => (byte)(AF & 0x00FF);
            set => AF = (ushort)((AF & 0xFF00) | value);
        }

        public byte B
        {
            get => (byte)(BC >> 8);
            set => BC = (ushort)((value << 8) | (BC & 0x00FF));
        }

        public byte C
        {
            get => (byte)(BC & 0x00FF);
            set => BC = (ushort)((BC & 0xFF00) | value);
        }

        public byte D
        {
            get => (byte)(DE >> 8);
            set => DE = (ushort)((value << 8) | (DE & 0x00FF));
        }

        public byte E
        {
            get => (byte)(DE & 0x00FF);
            set => DE = (ushort)((DE & 0xFF00) | value);
        }

        public byte H
        {
            get => (byte)(HL >> 8);
            set => HL = (ushort)((value << 8) | (HL & 0x00FF));
        }

        public byte L
        {
            get => (byte)(HL & 0x00FF);
            set => HL = (ushort)((HL & 0xFF00) | value);
        }

        public byte IXH
        {
            get => (byte)(IX >> 8);
            set => IX = (ushort)((value << 8) | (IX & 0x00FF));
        }

        public byte IXL
        {
            get => (byte)(IX & 0x00FF);
            set => IX = (ushort)((IX & 0xFF00) | value);
        }

        public byte IYH
        {
            get => (byte)(IY >> 8);
            set => IY = (ushort)((value << 8) | (IY & 0x00FF));
        }

        public byte IYL
        {
            get => (byte)(IY & 0x00FF);
            set => IY = (ushort)((IY & 0xFF00) | value);
        }

        public void SwapMainAndShadowRegisters()
        {
            ushort tempAF = AF;
            ushort tempBC = BC;
            ushort tempDE = DE;
            ushort tempHL = HL;

            AF = AF_S;
            BC = BC_S;
            DE = DE_S;
            HL = HL_S;

            AF_S = tempAF;
            BC_S = tempBC;
            DE_S = tempDE;
            HL_S = tempHL;
        }
    }
}
