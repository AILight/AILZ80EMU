using AILZ80LIB;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.OperationPacks
{
    public abstract class OperationPack
    {
        public CPUZ80 CPU { get; set; }
        public int ExecuteIndex { get; set; } = 0;
        public TimingCycleEnum[]? TimingCycles { get; set; }
        public Dictionary<TimingCycleEnum, Func<OperationPack?>>? TimingCycleActions { get; set; }

        public OperationPack(CPUZ80 cpu)
        {
            CPU = cpu;
        }

        public virtual OperationPack? Execute()
        {
            CPU.TimingCycle = TimingCycles![ExecuteIndex];

            return TimingCycleActions![CPU.TimingCycle].Invoke();
        }

        protected static RegisterEnum Select_r(byte opCode, int position)
        {
            var value = (opCode >> (8 - position - 3)) & 0x03;
            var register = value switch
            {
                0x00 => RegisterEnum.B,
                0x01 => RegisterEnum.C,
                0x02 => RegisterEnum.D,
                0x03 => RegisterEnum.E,
                0x04 => RegisterEnum.H,
                0x05 => RegisterEnum.L,
                //0x06 => RegisterEnum.HL,
                0x07 => RegisterEnum.A,
                _ => throw new NotImplementedException()
            };

            return register;
        }

        protected void ExecuteINC8(RegisterEnum register)
        {
            var value = register switch
            {
                RegisterEnum.A => CPU.Register.A,
                RegisterEnum.B => CPU.Register.B,
                RegisterEnum.C => CPU.Register.C,
                RegisterEnum.D => CPU.Register.D,
                RegisterEnum.E => CPU.Register.E,
                RegisterEnum.H => CPU.Register.H,
                RegisterEnum.L => CPU.Register.L,
                _ => throw new NotImplementedException()
            };

            var originalValue = value;
            value++;

            // 更新された値をレジスタに戻す
            switch (register)
            {
                case RegisterEnum.A:
                    CPU.Register.A = value;
                    break;
                case RegisterEnum.B:
                    CPU.Register.B = value;
                    break;
                case RegisterEnum.C:
                    CPU.Register.C = value;
                    break;
                case RegisterEnum.D:
                    CPU.Register.D = value;
                    break;
                case RegisterEnum.E:
                    CPU.Register.E = value;
                    break;
                case RegisterEnum.H:
                    CPU.Register.H = value;
                    break;
                case RegisterEnum.L:
                    CPU.Register.L = value;
                    break;
            }

            // サインフラグの設定
            if ((value & 0x80) != 0)
            {
                CPU.Register.F |= (byte)FlagEnum.Sign;
            }
            else
            {
                CPU.Register.F &= (byte)~FlagEnum.Sign;
            }

            // ゼロフラグの設定
            if (value == 0)
            {
                CPU.Register.F |= (byte)FlagEnum.Zero;
            }
            else
            {
                CPU.Register.F &= (byte)~FlagEnum.Zero;
            }

            // ハーフキャリーフラグの設定
            if ((originalValue & 0x0F) == 0x0F)
            {
                CPU.Register.F |= (byte)FlagEnum.HalfCarry;
            }
            else
            {
                CPU.Register.F &= (byte)~FlagEnum.HalfCarry;
            }

            // パリティ/オーバーフローフラグの設定（INC命令ではオーバーフローの考慮は不要）
            // 実際のZ80のINC命令ではパリティフラグは影響しないため、この部分は削除

            // サブトラクトフラグの設定（INC命令なのでクリア）
            CPU.Register.F &= (byte)~FlagEnum.AddSubtract;

            // キャリーフラグは変化しないので設定しない
        }

        protected void ExecuteDEC8(RegisterEnum register)
        {
            var value = register switch
            {
                RegisterEnum.A => CPU.Register.A,
                RegisterEnum.B => CPU.Register.B,
                RegisterEnum.C => CPU.Register.C,
                RegisterEnum.D => CPU.Register.D,
                RegisterEnum.E => CPU.Register.E,
                RegisterEnum.H => CPU.Register.H,
                RegisterEnum.L => CPU.Register.L,
                _ => throw new NotImplementedException()
            };

            var originalValue = value;
            value--;

            // 更新された値をレジスタに戻す
            switch (register)
            {
                case RegisterEnum.A:
                    CPU.Register.A = value;
                    break;
                case RegisterEnum.B:
                    CPU.Register.B = value;
                    break;
                case RegisterEnum.C:
                    CPU.Register.C = value;
                    break;
                case RegisterEnum.D:
                    CPU.Register.D = value;
                    break;
                case RegisterEnum.E:
                    CPU.Register.E = value;
                    break;
                case RegisterEnum.H:
                    CPU.Register.H = value;
                    break;
                case RegisterEnum.L:
                    CPU.Register.L = value;
                    break;
            }

            // サインフラグの設定
            if ((value & 0x80) != 0)
            {
                CPU.Register.F |= (byte)FlagEnum.Sign;
            }
            else
            {
                CPU.Register.F &= (byte)~FlagEnum.Sign;
            }

            // ゼロフラグの設定
            if (value == 0)
            {
                CPU.Register.F |= (byte)FlagEnum.Zero;
            }
            else
            {
                CPU.Register.F &= (byte)~FlagEnum.Zero;
            }

            // ハーフキャリーフラグの設定
            if ((originalValue & 0x0F) == 0)
            {
                CPU.Register.F |= (byte)FlagEnum.HalfCarry;
            }
            else
            {
                CPU.Register.F &= (byte)~FlagEnum.HalfCarry;
            }

            // オーバーフローフラグの設定
            if (originalValue == 0x80)
            {
                CPU.Register.F |= (byte)FlagEnum.ParityOverflow;
            }
            else
            {
                CPU.Register.F &= (byte)~FlagEnum.ParityOverflow;
            }

            // サブトラクトフラグの設定（DEC命令なのでセット）
            CPU.Register.F |= (byte)FlagEnum.AddSubtract;

            // キャリーフラグは変化しないので設定しない
        }

        protected void ExecuteRLCA()
        {
            // アキュムレータの値を取得
            var value = CPU.Register.A;

            // ビット7を取得
            var bit7 = (value & 0x80) != 0;

            // 1ビット左に回転
            value = (byte)((value << 1) | (bit7 ? 1 : 0));

            // アキュレータに結果を保存
            CPU.Register.A = value;

            // キャリーフラグの設定
            if (bit7)
            {
                CPU.Register.F |= (byte)FlagEnum.Carry;
            }
            else
            {
                CPU.Register.F &= (byte)~FlagEnum.Carry;
            }

            // 他のフラグは影響を受けない
            // サブトラクトフラグ、ゼロフラグ、ハーフキャリーフラグ、パリティ/オーバーフローフラグは影響しないので、変更しない
        }

        protected void ExecuteRLA()
        {
            // アキュムレータの値を取得
            var value = CPU.Register.A;

            // 現在のキャリーフラグの値を取得
            var carryFlag = (CPU.Register.F & (byte)FlagEnum.Carry) != 0;

            // ビット7を取得
            var bit7 = (value & 0x80) != 0;

            // 1ビット左に回転し、ビット0に元のキャリーフラグの値を設定
            value = (byte)((value << 1) | (carryFlag ? 1 : 0));

            // アキュレータに結果を保存
            CPU.Register.A = value;

            // ビット7の値に基づいてキャリーフラグを設定
            if (bit7)
            {
                CPU.Register.F |= (byte)FlagEnum.Carry;
            }
            else
            {
                CPU.Register.F &= (byte)~FlagEnum.Carry;
            }

            // 他のフラグは影響を受けない
            // サブトラクトフラグ、ゼロフラグ、ハーフキャリーフラグ、パリティ/オーバーフローフラグは影響しないので、変更しない
        }

        protected void ExecuteDAA()
        {
            byte correction = 0;
            bool carryFlag = (CPU.Register.F & (byte)FlagEnum.Carry) != 0;
            bool halfCarryFlag = (CPU.Register.F & (byte)FlagEnum.HalfCarry) != 0;
            bool subtractFlag = (CPU.Register.F & (byte)FlagEnum.AddSubtract) != 0;

            // アキュムレータの値を取得
            var value = CPU.Register.A;

            if (halfCarryFlag || (!subtractFlag && (value & 0x0F) > 9))
            {
                correction |= 0x06;
            }

            if (carryFlag || (!subtractFlag && value > 0x99))
            {
                correction |= 0x60;
                carryFlag = true;
            }
            else
            {
                carryFlag = false;
            }

            if (subtractFlag)
            {
                value -= correction;
            }
            else
            {
                value += correction;
            }

            // アキュレータに結果を保存
            CPU.Register.A = (byte)value;

            // ゼロフラグの設定
            if (value == 0)
            {
                CPU.Register.F |= (byte)FlagEnum.Zero;
            }
            else
            {
                CPU.Register.F &= (byte)~FlagEnum.Zero;
            }

            // キャリーフラグの設定
            if (carryFlag)
            {
                CPU.Register.F |= (byte)FlagEnum.Carry;
            }
            else
            {
                CPU.Register.F &= (byte)~FlagEnum.Carry;
            }

            // ハーフキャリーフラグの設定
            CPU.Register.F &= (byte)~FlagEnum.HalfCarry;

            // パリティ/オーバーフローフラグは変更しない
        }

        protected void ExecuteSCF()
        {
            // キャリーフラグをセット
            CPU.Register.F |= (byte)FlagEnum.Carry;

            // ハーフキャリーフラグをリセット
            CPU.Register.F &= (byte)~FlagEnum.HalfCarry;

            // サブトラクトフラグをリセット
            CPU.Register.F &= (byte)~FlagEnum.AddSubtract;

            // 他のフラグは影響を受けない
            // ゼロフラグ、サインフラグ、パリティ/オーバーフローフラグは変更しない
        }

        protected void ExecuteRRCA()
        {
            // アキュムレータの値を取得
            var value = CPU.Register.A;

            // ビット0を取得
            var bit0 = (value & 0x01) != 0;

            // 1ビット右に回転
            value = (byte)((value >> 1) | (bit0 ? 0x80 : 0));

            // アキュレータに結果を保存
            CPU.Register.A = value;

            // キャリーフラグの設定
            if (bit0)
            {
                CPU.Register.F |= (byte)FlagEnum.Carry;
            }
            else
            {
                CPU.Register.F &= (byte)~FlagEnum.Carry;
            }

            // 他のフラグは影響を受けない
            // サブトラクトフラグ、ゼロフラグ、ハーフキャリーフラグ、パリティ/オーバーフローフラグは影響しないので、変更しない
        }

        protected void ExecuteRRA()
        {
            // アキュムレータの値を取得
            var value = CPU.Register.A;

            // 現在のキャリーフラグの値を取得
            var carryFlag = (CPU.Register.F & (byte)FlagEnum.Carry) != 0;

            // ビット0を取得
            var bit0 = (value & 0x01) != 0;

            // 1ビット右に回転し、ビット7に元のキャリーフラグの値を設定
            value = (byte)((value >> 1) | (carryFlag ? 0x80 : 0));

            // アキュレータに結果を保存
            CPU.Register.A = value;

            // ビット0の値に基づいてキャリーフラグを設定
            if (bit0)
            {
                CPU.Register.F |= (byte)FlagEnum.Carry;
            }
            else
            {
                CPU.Register.F &= (byte)~FlagEnum.Carry;
            }

            // 他のフラグは影響を受けない
            // サブトラクトフラグ、ゼロフラグ、ハーフキャリーフラグ、パリティ/オーバーフローフラグは変更しない
        }

        protected void ExecuteCPL()
        {
            // アキュムレータの値をビットごとに反転
            CPU.Register.A = (byte)~CPU.Register.A;

            // サブトラクトフラグをセット
            CPU.Register.F |= (byte)FlagEnum.AddSubtract;

            // ハーフキャリーフラグをセット
            CPU.Register.F |= (byte)FlagEnum.HalfCarry;

            // 他のフラグは影響を受けない
            // キャリーフラグ、ゼロフラグ、サインフラグ、パリティ/オーバーフローフラグは変更しない
        }

        protected void ExecuteCCF()
        {
            // キャリーフラグを反転
            if ((CPU.Register.F & (byte)FlagEnum.Carry) != 0)
            {
                CPU.Register.F &= (byte)~FlagEnum.Carry;
            }
            else
            {
                CPU.Register.F |= (byte)FlagEnum.Carry;
            }

            // ハーフキャリーフラグを設定
            // CCF命令はハーフキャリーフラグをアキュムレータのビット3にする
            if ((CPU.Register.A & 0x08) != 0)
            {
                CPU.Register.F |= (byte)FlagEnum.HalfCarry;
            }
            else
            {
                CPU.Register.F &= (byte)~FlagEnum.HalfCarry;
            }

            // サブトラクトフラグをクリア
            CPU.Register.F &= (byte)~FlagEnum.AddSubtract;

            // 他のフラグは影響を受けない
            // ゼロフラグ、サインフラグ、パリティ/オーバーフローフラグは変更しない
        }

        protected void ExecuteEXAFAF_S()
        {
            // 主レジスタセットと代替レジスタセットの内容を交換
            var temp = CPU.Register.AF;

            CPU.Register.AF = CPU.Register.AF_S;

            CPU.Register.AF_S = temp;
        }

        protected void ExecuteADDHL(RegisterEnum register)
        {
            var hl = CPU.Register.HL;
            var valueToAdd = register switch
            {
                RegisterEnum.BC => CPU.Register.BC,
                RegisterEnum.DE => CPU.Register.DE,
                RegisterEnum.HL => CPU.Register.HL,
                RegisterEnum.SP => CPU.Register.SP,
                _ => throw new NotImplementedException()
            };

            // 16ビット加算
            var result = (uint)(hl + valueToAdd);

            // ハーフキャリーフラグの設定
            if (((hl & 0x0FFF) + (valueToAdd & 0x0FFF)) > 0x0FFF)
            {
                CPU.Register.F |= (byte)FlagEnum.HalfCarry;
            }
            else
            {
                CPU.Register.F &= (byte)~FlagEnum.HalfCarry;
            }

            // キャリーフラグの設定
            if (result > 0xFFFF)
            {
                CPU.Register.F |= (byte)FlagEnum.Carry;
            }
            else
            {
                CPU.Register.F &= (byte)~FlagEnum.Carry;
            }

            // 結果をHLレジスタに設定
            CPU.Register.HL = (ushort)result;

            // サブトラクトフラグをクリア
            CPU.Register.F &= (byte)~FlagEnum.AddSubtract;
        }
    }
}