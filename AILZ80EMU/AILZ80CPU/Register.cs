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
        private static readonly bool[] ParityTable = new bool[256];

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

        // Internal OpCode
        private byte _internal_op_code;
        // Internal Memory Pointer
        private ushort _internal_memory_pointer;

        // Internal 8bit Register
        private byte _internal_8bit_register;

        // Internal 16bit Register
        private ushort _internal_16bit_register;

        // nn レジスタ
        //private ushort _direct_address;

        // (HL) 読み取り値
        //private byte _indirect_HL;

        // トラップハンドラ
        public Action<RegisterEnum, AccessType, ushort>? OnRegisterAccess;

        static Register()
        {
            int CountBits(int value)
            {
                var count = 0;
                while (value != 0)
                {
                    count += value & 1;
                    value >>= 1;
                }
                return count;
            }

            for (var i = 0; i < 256; i++)
            {
                ParityTable[i] = CountBits(i) % 2 == 0;
            }
        }

        public ushort AF
        {
            get
            {
                OnRegisterAccess?.Invoke(RegisterEnum.AF, AccessType.Read, _af);
                return _af;
            }
            set
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
            set
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
            set
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
            set
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
            set
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
            set
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

        /*
        public ushort DirectAdress
        {
            get
            {
                OnRegisterAccess?.Invoke(RegisterEnum.DirectAddress, AccessType.Read, _pc);
                return _direct_address;
            }
            set
            {
                _direct_address = value;
                OnRegisterAccess?.Invoke(RegisterEnum.DirectAddress, AccessType.Write, _pc);
            }
        }
        */

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

        /*
        public byte Indirect_HL
        {
            get
            {
                OnRegisterAccess?.Invoke(RegisterEnum.IndirectHL, AccessType.Read, _indirect_HL);
                return _indirect_HL;
            }
            set
            {
                _indirect_HL = value;
                OnRegisterAccess?.Invoke(RegisterEnum.DirectAddress, AccessType.Write, _indirect_HL);
            }
        }
        */

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

        public byte SP_H
        {
            get => (byte)(SP >> 8);
            set => SP = (ushort)((value << 8) | (SP & 0x00FF));
        }

        public byte SP_L
        {
            get => (byte)(SP & 0x00FF);
            set => SP = (ushort)((SP & 0xFF00) | value);
        }

        public byte PC_H
        {
            get => (byte)(PC >> 8);
            set => PC = (ushort)((value << 8) | (PC & 0x00FF));
        }

        public byte PC_L
        {
            get => (byte)(PC & 0x00FF);
            set => PC = (ushort)((PC & 0xFF00) | value);
        }

        /*
        public byte DirectAddress_H
        {
            get => (byte)(DirectAdress >> 8);
            set => DirectAdress = (ushort)((value << 8) | (DirectAdress & 0x00FF));
        }

        public byte DirectAddress_L
        {
            get => (byte)(DirectAdress & 0x00FF);
            set => DirectAdress = (ushort)((DirectAdress & 0xFF00) | value);
        }
        */

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

        // フラグを取得するメソッド
        public bool IsFlagSet(FlagEnum flagEnum)
        {
            return (F & (byte)flagEnum) != 0;
        }

        // フラグをセットするメソッド
        public void SetFlag(FlagEnum flagEnum)
        {
            F |= (byte)flagEnum;
        }

        // フラグをクリアするメソッド
        public void ClearFlag(FlagEnum flagEnum)
        {
            F &= (byte)~flagEnum;
        }

        // フラグを条件で設定するメソッド
        public void UpdateFlag(FlagEnum flagEnum, bool condition)
        {
            if (condition)
            {
                F |= (byte)flagEnum; // フラグをセット
            }
            else
            {
                F &= (byte)~flagEnum; // フラグをクリア
            }
        }

        public void SwapMainAndShadowRegisters()
        {
            ushort tempBC = BC;
            ushort tempDE = DE;
            ushort tempHL = HL;

            BC = BC_S;
            DE = DE_S;
            HL = HL_S;

            BC_S = tempBC;
            DE_S = tempDE;
            HL_S = tempHL;
        }

        public void INC_8(RegisterEnum register)
        {
            // レジスタの値を取得
            var value = GetRegisterValue_8(register);

            // 新しい値を計算
            var newValue = (byte)(value + 1);

            // フラグの更新
            UpdateFlag(FlagEnum.Zero, newValue == 0);
            UpdateFlag(FlagEnum.Sign, (newValue & 0x80) != 0);
            UpdateFlag(FlagEnum.HalfCarry, (value & 0x0F) == 0x0F);  // 下位4ビットがキャリーするかどうか
            UpdateFlag(FlagEnum.ParityOverflow, value == 0x7F);  // オーバーフロー検出
            UpdateFlag(FlagEnum.AddSubtract, false);  // INCは常に加算

            // 新しい値をレジスタにセット
            SetRegisterValue_8(register, newValue);
        }

        public void INC_16(RegisterEnum register)
        {
            switch (register)
            {
                case RegisterEnum.BC:
                    BC++;
                    break;
                case RegisterEnum.DE:
                    DE++;
                    break;
                case RegisterEnum.HL:
                    HL++;
                    break;
                case RegisterEnum.SP:
                    SP++;
                    break;
                case RegisterEnum.IX:
                    IX++;
                    break;
                case RegisterEnum.IY:
                    IY++;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            UpdateFlag(FlagEnum.AddSubtract, false);
        }

        public void DEC_8(RegisterEnum register)
        {
            // レジスタの値を取得
            var value = GetRegisterValue_8(register);

            // 新しい値を計算
            var newValue = (byte)(value - 1);

            // フラグの更新
            UpdateFlag(FlagEnum.Zero, newValue == 0);
            UpdateFlag(FlagEnum.Sign, (newValue & 0x80) != 0);
            UpdateFlag(FlagEnum.HalfCarry, (value & 0x0F) == 0);  // 下位4ビットの借り判定
            UpdateFlag(FlagEnum.ParityOverflow, value == 0x80);   // 0x80 -> 0x7F の時オーバーフロー
            UpdateFlag(FlagEnum.AddSubtract, true);  // DECは常に減算

            // 新しい値をレジスタにセット
            SetRegisterValue_8(register, newValue);
        }

        public void DEC_16(RegisterEnum register)
        {
            switch (register)
            {
                case RegisterEnum.BC:
                    BC--;
                    break;
                case RegisterEnum.DE:
                    DE--;
                    break;
                case RegisterEnum.HL:
                    HL--;
                    break;
                case RegisterEnum.SP:
                    SP--;
                    break;
                case RegisterEnum.IX:
                    IX--;
                    break;
                case RegisterEnum.IY:
                    IY--;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            // 16ビット命令ではフラグの影響は Add/Subtract のみ
            UpdateFlag(FlagEnum.AddSubtract, true);
        }

        public void ADD_8(RegisterEnum register)
        {
            // レジスタの値を取得
            var value = GetRegisterValue_8(register);

            // Aレジスタの現在の値に加算
            var newValue = (byte)(A + value);

            // フラグの更新
            UpdateFlag(FlagEnum.Zero, newValue == 0);
            UpdateFlag(FlagEnum.Sign, (newValue & 0x80) != 0);
            UpdateFlag(FlagEnum.HalfCarry, ((A & 0x0F) + (value & 0x0F)) > 0x0F); // 下位4ビットのキャリー
            UpdateFlag(FlagEnum.ParityOverflow, ((A ^ newValue) & (value ^ newValue) & 0x80) != 0); // オーバーフロー
            UpdateFlag(FlagEnum.Carry, (A + value) > 0xFF);   // 全体のキャリー
            UpdateFlag(FlagEnum.AddSubtract, false);          // ADDは加算なのでリセット

            // 結果をAレジスタにセット
            A = newValue;
        }

        public void SUB_8(RegisterEnum register)
        {
            // レジスタの値を取得
            var value = GetRegisterValue_8(register);

            // Aレジスタの現在の値に減算
            var newValue = (byte)(A - value);

            // フラグの更新
            UpdateFlag(FlagEnum.Zero, newValue == 0);
            UpdateFlag(FlagEnum.Sign, (newValue & 0x80) != 0);
            UpdateFlag(FlagEnum.HalfCarry, (A & 0x0F) < (value & 0x0F));  // 下位4ビットの借り
            UpdateFlag(FlagEnum.ParityOverflow, ((A ^ value) & (A ^ newValue) & 0x80) != 0); // オーバーフロー検出
            UpdateFlag(FlagEnum.Carry, A < value);  // キャリー（借り）発生かどうか
            UpdateFlag(FlagEnum.AddSubtract, true); // SUBは減算なのでセット

            // 結果をAレジスタにセット
            A = newValue;
        }

        public void AND_8(RegisterEnum register)
        {
            // レジスタの値を取得
            var value = GetRegisterValue_8(register);

            // Aレジスタと指定レジスタのビット単位の論理積
            var newValue = (byte)(A & value);

            // フラグの更新
            UpdateFlag(FlagEnum.Zero, newValue == 0);
            UpdateFlag(FlagEnum.Sign, (newValue & 0x80) != 0);
            UpdateFlag(FlagEnum.ParityOverflow, ParityTable[newValue]); // 事前計算されたテーブルを使用して偶数パリティを判断
            UpdateFlag(FlagEnum.HalfCarry, true);  // AND命令は常にHalfCarryがセットされる
            UpdateFlag(FlagEnum.Carry, false);     // AND命令はCarryをリセット
            UpdateFlag(FlagEnum.AddSubtract, false); // AND命令は加算フラグをリセット

            // 結果をAレジスタにセット
            A = newValue;
        }

        public void OR_8(RegisterEnum register)
        {
            // レジスタの値を取得
            var value = GetRegisterValue_8(register);

            // Aレジスタと指定レジスタのビット単位の論理和
            var newValue = (byte)(A | value);

            // フラグの更新
            UpdateFlag(FlagEnum.Zero, newValue == 0);
            UpdateFlag(FlagEnum.Sign, (newValue & 0x80) != 0);
            UpdateFlag(FlagEnum.ParityOverflow, ParityTable[newValue]);  // 事前計算されたテーブルを使用して偶数パリティを判断
            UpdateFlag(FlagEnum.HalfCarry, false);  // OR命令はHalfCarryをリセット
            UpdateFlag(FlagEnum.Carry, false);      // OR命令はCarryをリセット
            UpdateFlag(FlagEnum.AddSubtract, false); // OR命令は加算/減算フラグをリセット

            // 結果をAレジスタにセット
            A = newValue;
        }

        public void XOR_8(RegisterEnum register)
        {
            // レジスタの値を取得
            var value = GetRegisterValue_8(register);

            // Aレジスタと指定レジスタのビット単位の排他的論理和
            var newValue = (byte)(A ^ value);

            // フラグの更新
            UpdateFlag(FlagEnum.Zero, newValue == 0);
            UpdateFlag(FlagEnum.Sign, (newValue & 0x80) != 0);
            UpdateFlag(FlagEnum.ParityOverflow, ParityTable[newValue]);  // 事前計算されたテーブルを使用して偶数パリティを判断
            UpdateFlag(FlagEnum.HalfCarry, false);  // XOR命令はHalfCarryをリセット
            UpdateFlag(FlagEnum.Carry, false);      // XOR命令はCarryをリセット
            UpdateFlag(FlagEnum.AddSubtract, false); // XOR命令は加算/減算フラグをリセット

            // 結果をAレジスタにセット
            A = newValue;
        }

        public void CP_8(RegisterEnum register)
        {
            // レジスタの値を取得
            var value = GetRegisterValue_8(register);

            // Aレジスタの現在の値を保持し、比較を実行
            var result = (byte)(A - value);

            // フラグの更新
            UpdateFlag(FlagEnum.Zero, result == 0);                            // 結果がゼロかどうか
            UpdateFlag(FlagEnum.Sign, (result & 0x80) != 0);                   // 結果の最上位ビット（符号ビット）
            UpdateFlag(FlagEnum.HalfCarry, (A & 0x0F) < (value & 0x0F));       // 下位4ビットの借り
            UpdateFlag(FlagEnum.ParityOverflow, ((A ^ value) & (A ^ result) & 0x80) != 0); // 符号付きオーバーフローの検出
            UpdateFlag(FlagEnum.Carry, A < value);                             // キャリー（借り）発生かどうか
            UpdateFlag(FlagEnum.AddSubtract, true);                            // CPは常に減算扱い

            // Aレジスタの値は変更しない
        }

        private byte GetRegisterValue_8(RegisterEnum register)
        {
            return register switch
            {
                RegisterEnum.A => A,
                RegisterEnum.B => B,
                RegisterEnum.C => C,
                RegisterEnum.D => D,
                RegisterEnum.E => E,
                RegisterEnum.H => H,
                RegisterEnum.L => L,
                RegisterEnum.IXH => IXH,
                RegisterEnum.IXL => IXL,
                RegisterEnum.IYH => IYH,
                RegisterEnum.IYL => IYL,
                RegisterEnum.Internal_8bit_Register => Internal_8bit_Register,
                _ => throw new InvalidOperationException()
            };
        }

        private void SetRegisterValue_8(RegisterEnum register, byte value)
        {
            switch (register)
            {
                case RegisterEnum.A: A = value; break;
                case RegisterEnum.B: B = value; break;
                case RegisterEnum.C: C = value; break;
                case RegisterEnum.D: D = value; break;
                case RegisterEnum.E: E = value; break;
                case RegisterEnum.H: H = value; break;
                case RegisterEnum.L: L = value; break;
                case RegisterEnum.IXH: IXH = value; break;
                case RegisterEnum.IXL: IXL = value; break;
                case RegisterEnum.IYH: IYH = value; break;
                case RegisterEnum.IYL: IYL = value; break;
                case RegisterEnum.Internal_8bit_Register: Internal_8bit_Register = value; break;
                default: throw new InvalidOperationException();
            }
        }

        public byte Internal_OpCode
        {
            get => _internal_op_code;
            set => _internal_op_code = value;
        }

        public ushort Internal_Memory_Pointer
        {
            get => _internal_memory_pointer;
            set => _internal_memory_pointer = value;
        }

        public byte Internal_8bit_Register
        {
            get => _internal_8bit_register;
            set => _internal_8bit_register = value;
        }

        public ushort Internal_16bit_Register
        {
            get => _internal_16bit_register;
            set => _internal_16bit_register = value;
        }

        public byte Internal_16bit_Register_H
        {
            get => (byte)(Internal_16bit_Register >> 8);
            set => Internal_16bit_Register = (ushort)((value << 8) | (Internal_16bit_Register & 0x00FF));
        }

        public byte Internal_16bit_Register_L
        {
            get => (byte)(Internal_16bit_Register & 0x00FF);
            set => Internal_16bit_Register = (ushort)((Internal_16bit_Register & 0xFF00) | value);
        }

    }
}
