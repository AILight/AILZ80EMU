using AILZ80CPU.Extensions;
using AILZ80CPU.InstructionSet;
using System;
using System.Collections.Generic;

namespace AILZ80CPU.Operations
{
    public class OperationRET : OperationItem
    {
        private Action<CPUZ80>? ExecuterForRead1 { get; set; }
        private Action<CPUZ80>? ExecuterForRead2 { get; set; }

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForRead1 = new Dictionary<string, Action<CPUZ80>>()
        {
            { "nn", (cpu) => {
                cpu.Bus.Address = cpu.Register.SP;    // スタックポインタからデータを読み込む
                cpu.Register.PC_L = cpu.Bus.Data;     // PCの下位バイトに保存
                cpu.Register.SP++;                    // スタックポインタをインクリメント
            } },
        };

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForRead2 = new Dictionary<string, Action<CPUZ80>>()
        {
            { "nn", (cpu) => {
                cpu.Bus.Address = cpu.Register.SP;    // スタックポインタからデータを読み込む
                cpu.Register.PC_H = cpu.Bus.Data;     // PCの上位バイトに保存
                cpu.Register.SP++;                    // スタックポインタをインクリメント
            } },
        };

        private OperationRET(InstructionItem instructionItem)
            : base(instructionItem)
        {
        }

        public static new OperationRET Create(InstructionItem instructionItem)
        {
            if (instructionItem.OpCode != OpCodeEnum.RET)
            {
                return default!;
            }

            var operationItem = new OperationRET(instructionItem);

            if (operandExecuterForRead1.TryGetValue("nn", out var executerForRead1))
            {
                operationItem.ExecuterForRead1 = executerForRead1;
            }
            if (operandExecuterForRead2.TryGetValue("nn", out var executerForRead2))
            {
                operationItem.ExecuterForRead2 = executerForRead2;
            }

            return operationItem;
        }

        public override OperationItem Execute(CPUZ80 cpu, int machineCycleIndex)
        {
            if (cpu.TimingCycle == TimingCycleEnum.R1_T3_H)
            {
                if (machineCycleIndex == 1)
                {
                    ExecuterForRead1?.Invoke(cpu);  // スタックからPCの下位バイトを読み込む
                }
                else if (machineCycleIndex == 2)
                {
                    ExecuterForRead2?.Invoke(cpu);  // スタックからPCの上位バイトを読み込む
                }
            }

            return this;
        }
    }
}
