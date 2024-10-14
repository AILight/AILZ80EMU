using AILZ80CPU.Extensions;
using AILZ80CPU.InstructionSet;
using System;
using System.Collections.Generic;

namespace AILZ80CPU.Operations
{
    public class OperationIN : OperationItem
    {
        private Action<CPUZ80>? ExecuterForFetch { get; set; }
        private Action<CPUZ80>? ExecuterForRead { get; set; }
        private Action<CPUZ80>? ExecuterForIORead { get; set; }

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForFetch = new Dictionary<string, Action<CPUZ80>>()
        {
            { "A, (n)", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.PC; cpu.Register.PC++; } },
            { "B, (C)", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.BC; } },
            { "C, (C)", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.BC; } },
            { "D, (C)", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.BC; } },
            { "E, (C)", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.BC; } },
            { "H, (C)", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.BC; } },
            { "L, (C)", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.BC; } },
            { "A, (C)", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.BC; } },
        };

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForRead = new Dictionary<string, Action<CPUZ80>>()
        {
            { "A, (n)", (cpu) => {
                cpu.Register.Internal_Memory_Pointer = (ushort)((cpu.Register.B * 256) + cpu.Bus.Data);
            }},
        };

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForIORead = new Dictionary<string, Action<CPUZ80>>()
        {
            { "A, (n)", (cpu) => {
                cpu.Register.A = cpu.Bus.Data;
            }},
            { "B, (C)", (cpu) => {
                cpu.Register.B = cpu.Bus.Data;
            }},
            { "C, (C)", (cpu) => {
                cpu.Register.C = cpu.Bus.Data;
            }},
            { "D, (C)", (cpu) => {
                cpu.Register.D = cpu.Bus.Data;
            }},
            { "E, (C)", (cpu) => {
                cpu.Register.E = cpu.Bus.Data;
            }},
            { "H, (C)", (cpu) => {
                cpu.Register.H = cpu.Bus.Data;
            }},
            { "L, (C)", (cpu) => {
                cpu.Register.L = cpu.Bus.Data;
            }},
            { "A, (C)", (cpu) => {
                cpu.Register.A = cpu.Bus.Data;
            }},
        };

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

            var executer = default(Action<CPUZ80>);
            var operationItem = new OperationIN(instructionItem);

            if (operandExecuterForFetch.TryGetValue(instructionItem.Operand, out executer))
            {
                operationItem.ExecuterForFetch = executer;
                if (operandExecuterForRead.TryGetValue(instructionItem.Operand, out var executerForRead))
                {
                    operationItem.ExecuterForRead = executerForRead;
                }
                if (operandExecuterForIORead.TryGetValue(instructionItem.Operand, out var executerForIORead))
                {
                    operationItem.ExecuterForIORead = executerForIORead;
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
                ExecuterForRead?.Invoke(cpu);  // メモリ読み取り
            }
            else if (cpu.TimingCycle == TimingCycleEnum.IR_T3_H)
            {
                ExecuterForIORead?.Invoke(cpu);  // IO読み取り
            }

            return this;
        }
    }
}
