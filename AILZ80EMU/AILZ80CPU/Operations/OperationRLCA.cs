using AILZ80CPU.Extensions;
using AILZ80CPU.InstructionSet;
using System;

namespace AILZ80CPU.Operations
{
    public class OperationRLCA : OperationItem
    {
        private Action<CPUZ80>? ExecuterForFetch { get; set; }

        private static Action<CPUZ80> operandExecuterForFetch = (cpu) =>
        {
            // Aレジスタの最上位ビット（ビット7）を取得
            var carryFlag = (cpu.Register.A & 0x80) != 0;

            // Aレジスタを左に回転（Circular）
            cpu.Register.A = (byte)((cpu.Register.A << 1) | (carryFlag ? 1 : 0));

            // キャリーフラグの更新（ビット7をキャリーフラグにセット）
            cpu.Register.UpdateFlag(FlagEnum.Carry, carryFlag);

            // 他のフラグには影響しないため変更はなし
        };

        private OperationRLCA(InstructionItem instructionItem)
            : base(instructionItem)
        {
        }

        public static new OperationRLCA Create(InstructionItem instructionItem)
        {
            if (instructionItem.OpCode != OpCodeEnum.RLCA)
            {
                return default!;
            }

            var operationItem = new OperationRLCA(instructionItem)
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
