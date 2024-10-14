using AILZ80CPU.Extensions;
using AILZ80CPU.InstructionSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.Operations
{
    public class OperationPOP : OperationItem
    {
        private Action<CPUZ80>? ExecuterForFetch { get; set; }
        private Action<CPUZ80>? ExecuterForRead1 { get; set; }
        private Action<CPUZ80>? ExecuterForRead2 { get; set; }

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForRead1 = new Dictionary<string, Action<CPUZ80>>()
        {
            { @"BC", (cpu) => { cpu.Register.C = cpu.Bus.Data; cpu.Register.SP++; cpu.Register.Internal_Memory_Pointer = cpu.Register.SP; } },
            { @"DE", (cpu) => { cpu.Register.E = cpu.Bus.Data; cpu.Register.SP++; cpu.Register.Internal_Memory_Pointer = cpu.Register.SP; } },
            { @"HL", (cpu) => { cpu.Register.L = cpu.Bus.Data; cpu.Register.SP++; cpu.Register.Internal_Memory_Pointer = cpu.Register.SP; } },
            { @"AF", (cpu) => { cpu.Register.F = cpu.Bus.Data; cpu.Register.SP++; cpu.Register.Internal_Memory_Pointer = cpu.Register.SP; } },
        };

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForRead2 = new Dictionary<string, Action<CPUZ80>>()
        {
            { @"BC", (cpu) => { cpu.Register.B = cpu.Bus.Data; cpu.Register.SP++; } },
            { @"DE", (cpu) => { cpu.Register.D = cpu.Bus.Data; cpu.Register.SP++; } },
            { @"HL", (cpu) => { cpu.Register.H = cpu.Bus.Data; cpu.Register.SP++; } },
            { @"AF", (cpu) => { cpu.Register.A = cpu.Bus.Data; cpu.Register.SP++; } },
        };

        private OperationPOP(InstructionItem instructionItem)
            : base(instructionItem)
        {
        }

        public static new OperationPOP Create(InstructionItem instructionItem)
        {
            if (instructionItem.OpCode != OpCodeEnum.POP)
            {
                return default!;
            }
            
            var operationItem = new OperationPOP(instructionItem);

            if (operandExecuterForRead1.TryGetValue(instructionItem.Operand, out var executer1) &&
                operandExecuterForRead2.TryGetValue(instructionItem.Operand, out var executer2))
            {
                operationItem.ExecuterForFetch = (cpu) => 
                {
                    cpu.Register.SP++;
                    cpu.Register.Internal_Memory_Pointer = cpu.Register.SP;
                };

                operationItem.ExecuterForRead1 = executer1;
                operationItem.ExecuterForRead1 = executer2;

                return operationItem;
            }

            return default!;
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
