using AILZ80CPU.Extensions;
using AILZ80CPU.InstructionSet;
using System;

namespace AILZ80CPU.Operations
{
    public class OperationRRCA : OperationItem
    {
        private Action<CPUZ80>? ExecuterForFetch { get; set; }

        private static Action<CPUZ80> operandExecuterForFetch = (cpu) =>
        {
            // Aレジスタの最下位ビット（ビット0）を取得
            var carryFlag = (cpu.Register.A & 0x01) != 0;

            // Aレジスタを右に回転（Circular）させる
            cpu.Register.A = (byte)((cpu.Register.A >> 1) | (carryFlag ? 0x80 : 0));

            // キャリーフラグの更新（ビット0をキャリーフラグにセット）
            cpu.Register.UpdateFlag(FlagEnum.Carry, carryFlag);

            // 他のフラグには影響しない
        };

        private OperationRRCA(InstructionItem instructionItem)
            : base(instructionItem)
        {
        }

        public static new OperationRRCA Create(InstructionItem instructionItem)
        {
            if (instructionItem.OpCode != OpCodeEnum.RRCA)
            {
                return default!;
            }

            var operationItem = new OperationRRCA(instructionItem)
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
