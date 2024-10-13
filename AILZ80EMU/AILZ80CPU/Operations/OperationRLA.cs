using AILZ80CPU.Extensions;
using AILZ80CPU.InstructionSet;
using System;

namespace AILZ80CPU.Operations
{
    public class OperationRLA : OperationItem
    {
        private Action<CPUZ80>? ExecuterForFetch { get; set; }

        private static Action<CPUZ80> operandExecuterForFetch = (cpu) =>
        {
            // Aレジスタの最上位ビット（ビット7）を取得
            var carryFlag = (cpu.Register.A & 0x80) != 0;

            // キャリーフラグの現在の値を最下位ビットに入れ、Aレジスタを左にシフト
            cpu.Register.A = (byte)((cpu.Register.A << 1) | (cpu.Register.IsFlagSet(FlagEnum.Carry) ? 1 : 0));

            // キャリーフラグの更新（ビット7をキャリーフラグにセット）
            cpu.Register.UpdateFlag(FlagEnum.Carry, carryFlag);

            // 他のフラグには影響しない
        };

        private OperationRLA(InstructionItem instructionItem)
            : base(instructionItem)
        {
        }

        public static new OperationRLA Create(InstructionItem instructionItem)
        {
            if (instructionItem.OpCode != OpCodeEnum.RLA)
            {
                return default!;
            }

            var operationItem = new OperationRLA(instructionItem)
            {
                ExecuterForFetch = operandExecuterForFetch
            };

            return operationItem;
        }

        public override OperationItem Execute(CPUZ80 cpu, int machineCycleIndex)
        {
            if (cpu.TimingCycle == TimingCycleEnum.M1_T2_L)
            {
                ExecuterForFetch?.Invoke(cpu); // フェッチサイクル
            }

            return this;
        }
    }
}
