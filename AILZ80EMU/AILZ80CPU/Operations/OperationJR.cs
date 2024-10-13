using AILZ80CPU.Extensions;
using AILZ80CPU.InstructionSet;
using System;
using System.Collections.Generic;

namespace AILZ80CPU.Operations
{
    public class OperationJR : OperationItem
    {
        private Action<CPUZ80>? ExecuterForFetch { get; set; }
        private Action<CPUZ80>? ExecuterForRead { get; set; }

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForFetch = new Dictionary<string, Action<CPUZ80>>()
        {
            { "e", (cpu) => { cpu.Address = cpu.Register.PC; cpu.Register.PC++; } },  // フェッチ: オフセット取得
        };

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForRead = new Dictionary<string, Action<CPUZ80>>()
        {
            { "e", (cpu) => {
                // メモリから読み取ったオフセットを符号拡張して相対ジャンプ
                var offset = (sbyte)cpu.Bus.Data;
                cpu.Register.PC = (ushort)(cpu.Register.PC + offset);
            }},
        };

        private OperationJR(InstructionItem instructionItem)
            : base(instructionItem)
        {
        }

        public static new OperationJR Create(InstructionItem instructionItem)
        {
            if (instructionItem.OpCode != OpCodeEnum.JR)
            {
                return default!;
            }

            var executer = default(Action<CPUZ80>);
            var operationItem = new OperationJR(instructionItem);

            if (operandExecuterForFetch.TryGetValue(instructionItem.Operand, out executer))
            {
                operationItem.ExecuterForFetch = executer;
                if (operandExecuterForRead.TryGetValue(instructionItem.Operand, out var executerForRead))
                {
                    operationItem.ExecuterForRead = executerForRead;
                }

                return operationItem;
            }

            return default!;
        }

        public override OperationItem Execute(CPUZ80 cpu, int machineCycleIndex)
        {
            if (cpu.TimingCycle == TimingCycleEnum.M1_T2_L)
            {
                ExecuterForFetch?.Invoke(cpu); // フェッチサイクル（オフセット取得）
            }
            else if (cpu.TimingCycle == TimingCycleEnum.R1_T3_H)
            {
                ExecuterForRead?.Invoke(cpu);  // メモリ読み取り（相対ジャンプ）
            }

            return this;
        }
    }
}
