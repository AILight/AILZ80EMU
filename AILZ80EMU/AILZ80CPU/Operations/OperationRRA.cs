using AILZ80CPU.Extensions;
using AILZ80CPU.InstructionSet;
using System;

namespace AILZ80CPU.Operations
{
    public class OperationRRA : OperationItem
    {
        private Action<CPUZ80>? ExecuterForFetch { get; set; }

        private static Action<CPUZ80> operandExecuterForFetch = (cpu) =>
        {
            // Aレジスタの最下位ビット（ビット0）を取得
            var carryFlag = (cpu.Register.A & 0x01) != 0;

            // キャリーフラグの現在の値を最上位ビットに入れ、Aレジスタを右にシフト
            cpu.Register.A = (byte)((cpu.Register.A >> 1) | (cpu.Register.IsFlagSet(FlagEnum.Carry) ? 0x80 : 0));

            // キャリーフラグの更新（ビット0をキャリーフラグにセット）
            cpu.Register.UpdateFlag(FlagEnum.Carry, carryFlag);

            // 他のフラグには影響しない
        };

        private OperationRRA(InstructionItem instructionItem)
            : base(instructionItem)
        {
        }

        public static new OperationRRA Create(InstructionItem instructionItem)
        {
            if (instructionItem.OpCode != OpCodeEnum.RRA)
            {
                return default!;
            }

            var operationItem = new OperationRRA(instructionItem)
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
