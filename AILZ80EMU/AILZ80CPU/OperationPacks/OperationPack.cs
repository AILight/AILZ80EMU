using AILZ80LIB;
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
    }
}