using AILZ80CPU.Extensions;
using AILZ80CPU.InstructionSet;
using System;
using System.Collections.Generic;

namespace AILZ80CPU.Operations
{
    public class OperationOUT : OperationItem
    {
        private Action<CPUZ80>? ExecuterForFetch { get; set; }
        private Action<CPUZ80>? ExecuterForRead { get; set; }
        private Action<CPUZ80>? ExecuterForIOWrite { get; set; }

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForFetch = new Dictionary<string, Action<CPUZ80>>()
        {
            { "(n), A", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.PC; cpu.Register.PC++; } },
            { "(C), B", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.BC; } },
            { "(C), C", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.BC; } },
            { "(C), D", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.BC; } },
            { "(C), E", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.BC; } },
            { "(C), H", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.BC; } },
            { "(C), L", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.BC; } },
            { "(C), A", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.BC; } },
        };

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForRead = new Dictionary<string, Action<CPUZ80>>()
        {
            { "(n), A", (cpu) => {
                cpu.Register.Internal_Memory_Pointer = (ushort)((cpu.Register.B * 256) + cpu.Bus.Data);
            }},
        };

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForIOWrite = new Dictionary<string, Action<CPUZ80>>()
        {
            { "(n), A", (cpu) => {
                cpu.Bus.Data = cpu.Register.A;
            }},
            { "(C), B", (cpu) => {
                cpu.Bus.Data = cpu.Register.B;
            }},
            { "(C), C", (cpu) => {
                cpu.Bus.Data = cpu.Register.C;
            }},
            { "(C), D", (cpu) => {
                cpu.Bus.Data = cpu.Register.D;
            }},
            { "(C), E", (cpu) => {
                cpu.Bus.Data = cpu.Register.E;
            }},
            { "(C), H", (cpu) => {
                cpu.Bus.Data = cpu.Register.H;
            }},
            { "(C), L", (cpu) => {
                cpu.Bus.Data = cpu.Register.L;
            }},
            { "(C), A", (cpu) => {
                cpu.Bus.Data = cpu.Register.A;
            }},
        };

        private OperationOUT(InstructionItem instructionItem)
            : base(instructionItem)
        {
        }

        public static new OperationOUT Create(InstructionItem instructionItem)
        {
            if (instructionItem.OpCode != OpCodeEnum.OUT)
            {
                return default!;
            }

            var executer = default(Action<CPUZ80>);
            var operationItem = new OperationOUT(instructionItem);

            if (operandExecuterForFetch.TryGetValue(instructionItem.Operand, out executer))
            {
                operationItem.ExecuterForFetch = executer;
                if (operandExecuterForRead.TryGetValue(instructionItem.Operand, out var executerForRead))
                {
                    operationItem.ExecuterForRead = executerForRead;
                }
                if (operandExecuterForIOWrite.TryGetValue(instructionItem.Operand, out var executerForIOWrite))
                {
                    operationItem.ExecuterForIOWrite = executerForIOWrite;
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
            else if (cpu.TimingCycle == TimingCycleEnum.IW_T3_H)
            {
                ExecuterForIOWrite?.Invoke(cpu);  // IO読み取り
            }

            return this;
        }
    }
}
