using AILZ80CPU.Extensions;
using AILZ80CPU.InstructionSet;
using System;

namespace AILZ80CPU.Operations
{
    public class OperationRST : OperationItem
    {
        private Action<CPUZ80>? ExecuterForFetch { get; set; }
        private Action<CPUZ80>? ExecuterForWrite1 { get; set; }
        private Action<CPUZ80>? ExecuterForWrite2 { get; set; }
        
        private OperationRST(InstructionItem instructionItem)
            : base(instructionItem)
        {
        }

        private static Dictionary<string, ushort> jumpAddresses = new Dictionary<string, ushort>()
        {
            { "00h", 0x00 },
            { "08h", 0x08 },
            { "10h", 0x10 },
            { "18h", 0x18 },
            { "20h", 0x20 },
            { "28h", 0x28 },
            { "30h", 0x30 },
            { "38h", 0x38 }
        };

        public static new OperationRST Create(InstructionItem instructionItem)
        {
            if (instructionItem.OpCode != OpCodeEnum.RST)
            {
                return default!;
            }

            if (!jumpAddresses.TryGetValue(instructionItem.Operand, out var address))
            {
                return default!;
            }

            var operationItem = new OperationRST(instructionItem);

            operationItem.ExecuterForFetch = (cpu) =>
            {
                // PCの上位バイトをスタックにプッシュ
                cpu.Register.SP--;
                cpu.Bus.Address = cpu.Register.SP;
                cpu.Bus.Data = cpu.Register.PC_H;
            };

            operationItem.ExecuterForWrite1 = (cpu) =>
            {
                // PCの下位バイトをスタックにプッシュ
                cpu.Register.SP--;
                cpu.Bus.Address = cpu.Register.SP;
                cpu.Bus.Data = cpu.Register.PC_L;
            };

            operationItem.ExecuterForWrite2 = (cpu) =>
            {
                // ジャンプ先アドレスをPCに設定
                cpu.Register.PC = address;
            };

            return operationItem;
        }

        public override OperationItem Execute(CPUZ80 cpu, int machineCycleIndex)
        {
            if (cpu.TimingCycle == TimingCycleEnum.M1_T2_L)
            {
                ExecuterForFetch?.Invoke(cpu); // フェッチサイクルでPCの上位バイトをスタックにプッシュ
            }
            else if (cpu.TimingCycle == TimingCycleEnum.W1_T3_H)
            {
                if (machineCycleIndex == 1)
                {
                    ExecuterForWrite1?.Invoke(cpu);  // スタックへのPC下位バイトの書き込み
                }
                else if (machineCycleIndex == 2)
                {
                    ExecuterForWrite2?.Invoke(cpu);  // スタックへのPC上位バイトの書き込み & ジャンプ
                }
            }

            return this;
        }
    }
}
