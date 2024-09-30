using AILZ80CPU.InstructionSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.Operations
{
    public class OperationLD_16 : OperationItem
    {
        private Action<CPUZ80>? ExecuterForFetch { get; set; }
        private Action<CPUZ80>? ExecuterForRead1 { get; set; }
        private Action<CPUZ80>? ExecuterForRead2 { get; set; }

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForFetch_MapForR16NN = new Dictionary<string, Action<CPUZ80>>()
        {
            { "BC, nn", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.PC; cpu.Register.PC++; } },
            { "DE, nn", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.PC; cpu.Register.PC++; } },
            { "HL, nn", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.PC; cpu.Register.PC++; } },
            { "SP, nn", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.PC; cpu.Register.PC++; } },
        };

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForRead1_MapForR16NN = new Dictionary<string, Action<CPUZ80>>()
        {
            { "BC, nn", (cpu) => { cpu.Register.C = cpu.Bus.Data; cpu.Register.Internal_Memory_Pointer = cpu.Register.PC; cpu.Register.PC++; } },
            { "DE, nn", (cpu) => { cpu.Register.E = cpu.Bus.Data; cpu.Register.Internal_Memory_Pointer = cpu.Register.PC; cpu.Register.PC++; } },
            { "HL, nn", (cpu) => { cpu.Register.L = cpu.Bus.Data; cpu.Register.Internal_Memory_Pointer = cpu.Register.PC; cpu.Register.PC++; } },
            { "SP, nn", (cpu) => { cpu.Register.SP_L = cpu.Bus.Data; cpu.Register.Internal_Memory_Pointer = cpu.Register.PC; cpu.Register.PC++; } },
        };

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForRead2_MapForR16NN = new Dictionary<string, Action<CPUZ80>>()
        {
            { "BC, nn", (cpu) => { cpu.Register.B = cpu.Bus.Data; } },
            { "DE, nn", (cpu) => { cpu.Register.D = cpu.Bus.Data; } },
            { "HL, nn", (cpu) => { cpu.Register.H = cpu.Bus.Data; } },
            { "SP, nn", (cpu) => { cpu.Register.SP_H = cpu.Bus.Data; } },
        };

        private OperationLD_16(InstructionItem instructionItem)
            : base(instructionItem)
        {
        }

        public static OperationLD_16? Create(InstructionItem instructionItem)
        {
            if (instructionItem.OpCode != OpCodeEnum.LD)
            {
                return default;
            }
            
            if (operandExecuterForFetch_MapForR16NN.TryGetValue(instructionItem.Operand, out var executerForFetch) &&
                operandExecuterForRead1_MapForR16NN.TryGetValue(instructionItem.Operand, out var executerForRead1) &&
                operandExecuterForRead2_MapForR16NN.TryGetValue(instructionItem.Operand, out var executerForRead2))
            {
                var operationItem = new OperationLD_16(instructionItem);

                operationItem.ExecuterForFetch = executerForFetch;
                operationItem.ExecuterForRead1 = executerForRead1;
                operationItem.ExecuterForRead2 = executerForRead2;

                return operationItem;
            }

            return default;
        }



        public override OperationItem Execute(CPUZ80 cpu, int machineCycleIndex)
        {
            if (cpu.TimingCycle == TimingCycleEnum.M1_T2_L)
            {
                ExecuterForFetch?.Invoke(cpu);
            }
            else if (cpu.TimingCycle == TimingCycleEnum.R1_T3_H)
            {
                if (machineCycleIndex == 1)
                {
                    ExecuterForRead1?.Invoke(cpu);
                }
                else
                {
                    ExecuterForRead2?.Invoke(cpu);
                }
            }

            return this;
        }
    }
}
