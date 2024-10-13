using AILZ80CPU.Extensions;
using AILZ80CPU.InstructionSet;
using System;
using System.Collections.Generic;

namespace AILZ80CPU.Operations
{
    public class OperationJP : OperationItem
    {
        private Action<CPUZ80>? ExecuterForFetch { get; set; }
        private Action<CPUZ80>? ExecuterForRead1 { get; set; }
        private Action<CPUZ80>? ExecuterForRead2 { get; set; }

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForFetch = new Dictionary<string, Action<CPUZ80>>()
        {
            { "nn", (cpu) => { cpu.Address = cpu.Register.PC; cpu.Register.PC++; } }, 
            { "(HL)", (cpu) => { cpu.Register.PC = cpu.Register.HL; } },
            { "(IX)", (cpu) => { cpu.Register.PC = cpu.Register.IX; } },
            { "(IY)", (cpu) => { cpu.Register.PC = cpu.Register.IY; } }
        };

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForRead1 = new Dictionary<string, Action<CPUZ80>>()
        {
            { "nn", (cpu) => { cpu.Register.Internal_16bit_Register_L = cpu.Bus.Data; cpu.Address = cpu.Register.PC; cpu.Register.PC++; } }, 
        };

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForRead2 = new Dictionary<string, Action<CPUZ80>>()
        {
            { "nn", (cpu) => { cpu.Register.Internal_16bit_Register_H = cpu.Bus.Data; cpu.Register.PC = cpu.Register.Internal_16bit_Register; } }, 
        };

        private OperationJP(InstructionItem instructionItem)
            : base(instructionItem)
        {
        }

        public static new OperationJP Create(InstructionItem instructionItem)
        {
            if (instructionItem.OpCode != OpCodeEnum.JP)
            {
                return default!;
            }

            var executer = default(Action<CPUZ80>);
            var operationItem = new OperationJP(instructionItem);

            if (operandExecuterForFetch.TryGetValue(instructionItem.Operand, out executer))
            {
                operationItem.ExecuterForFetch = executer;
                if (operandExecuterForRead1.TryGetValue(instructionItem.Operand, out var executerForRead1))
                {
                    operationItem.ExecuterForRead1 = executerForRead1;
                }
                if (operandExecuterForRead2.TryGetValue(instructionItem.Operand, out var executerForRead2))
                {
                    operationItem.ExecuterForRead2 = executerForRead2;
                }

                return operationItem;
            }

            return default!;
        }

        public override OperationItem Execute(CPUZ80 cpu, int machineCycleIndex)
        {
            if (cpu.TimingCycle == TimingCycleEnum.M1_T2_L)
            {
                ExecuterForFetch?.Invoke(cpu); // フェッチサイクル
            }
            else if (cpu.TimingCycle == TimingCycleEnum.R1_T3_H)
            {
                if (machineCycleIndex == 1)
                {
                    ExecuterForRead1?.Invoke(cpu);  // メモリの読み取り
                }
                else
                {
                    ExecuterForRead2?.Invoke(cpu);  // メモリの読み取り
                }
            }

            return this;
        }
    }
}
