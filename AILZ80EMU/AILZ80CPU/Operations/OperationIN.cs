using AILZ80CPU.Extensions;
using AILZ80CPU.InstructionSet;
using System;

namespace AILZ80CPU.Operations
{
    public class OperationIN : OperationItem
    {
        private Action<CPUZ80>? ExecuterForFetch { get; set; }
        private Action<CPUZ80>? ExecuterForReadPort { get; set; }

        private OperationIN(InstructionItem instructionItem)
            : base(instructionItem)
        {
        }

        public static new OperationIN Create(InstructionItem instructionItem)
        {
            if (instructionItem.OpCode != OpCodeEnum.IN)
            {
                return default!;
            }

            var operationItem = new OperationIN(instructionItem);

            operationItem.ExecuterForFetch = (cpu) =>
            {
                // メモリからポートアドレス（n）をフェッチ
                cpu.Address = cpu.Register.PC;
                cpu.Register.PC++;
            };

            operationItem.ExecuterForReadPort = (cpu) =>
            {
                // I/Oポートからデータを読み取り、Aレジスタに格納
                byte portAddress = cpu.Bus.Data; // メモリからフェッチされたポートアドレス
                cpu.Register.A = cpu.ReadPort(portAddress); // ポートからデータを読み取りAに格納

                // フラグの更新 (ここでは具体的にフラグが変化しないが、必要なら条件に応じてフラグを更新)
                cpu.Register.UpdateFlag(FlagEnum.Zero, cpu.Register.A == 0);  // Aが0ならZeroフラグをセット
                cpu.Register.UpdateFlag(FlagEnum.Sign, (cpu.Register.A & 0x80) != 0); // Aの最上位ビットが1ならSignフラグをセット
                cpu.Register.UpdateFlag(FlagEnum.ParityOverflow, cpu.CheckParity(cpu.Register.A)); // パリティフラグをセット
                // AddSubtractフラグやHalfCarryフラグは変更しない
            };

            return operationItem;
        }

        public override OperationItem Execute(CPUZ80 cpu, int machineCycleIndex)
        {
            if (cpu.TimingCycle == TimingCycleEnum.M1_T2_L)
            {
                ExecuterForFetch?.Invoke(cpu); // フェッチサイクル
            }
            else if (cpu.TimingCycle == TimingCycleEnum.R1_T3_H)
            {
                ExecuterForReadPort?.Invoke(cpu);  // I/Oポートからデータを読み取り、Aに格納
            }

            return this;
        }
    }
}
