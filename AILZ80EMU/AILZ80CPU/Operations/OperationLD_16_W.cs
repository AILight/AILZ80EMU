using AILZ80CPU.InstructionSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.Operations
{
    public class OperationLD_16_W : OperationItem
    {
        private Action<CPUZ80>? ExecuterForFetch { get; set; }
        private Action<CPUZ80>? ExecuterForRead1 { get; set; }
        private Action<CPUZ80>? ExecuterForRead2 { get; set; }
        private Action<CPUZ80>? ExecuterForWrite3 { get; set; }
        private Action<CPUZ80>? ExecuterForWrite4 { get; set; }

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForFetch = new Dictionary<string, Action<CPUZ80>>()
        {
            { "(nn), HL", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.PC; cpu.Register.PC++; } },
        };

        private OperationLD_16_W(InstructionItem instructionItem)
            : base(instructionItem)
        {
        }

        public static OperationLD_16? Create(InstructionItem instructionItem)
        {
            if (instructionItem.OpCode != OpCodeEnum.LD)
            {
                return default;
            }

            if (operandExecuterForFetch.TryGetValue(instructionItem.Operand, out var executerForFetch))
            {
                var operationItem = new OperationLD_16_W(instructionItem);
                operationItem.ExecuterForFetch = executerForFetch;
                operationItem.ExecuterForRead1 = (cpu) =>
                {
                    cpu.Register.Internal_16bit_Register_L = cpu.Bus.Data;
                    cpu.Register.Internal_Memory_Pointer = cpu.Register.PC; cpu.Register.PC++;
                };
                operationItem.ExecuterForRead2 = (cpu) =>
                {
                    cpu.Register.Internal_16bit_Register_H = cpu.Bus.Data;
                    cpu.Register.Internal_Memory_Pointer = cpu.Register.Internal_16bit_Register;
                    cpu.Register.Internal_16bit_Register++;
                };
                operationItem.ExecuterForWrite3 = (cpu) =>
                {
                    cpu.Bus.Data = cpu.Register.L;
                    cpu.Register.Internal_Memory_Pointer = cpu.Register.Internal_16bit_Register;
                };
                operationItem.ExecuterForWrite4 = (cpu) =>
                {
                    cpu.Bus.Data = cpu.Register.H;
                };
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
            else if (cpu.TimingCycle == TimingCycleEnum.W1_T3_H)
            {
                if (machineCycleIndex == 3)
                {
                    ExecuterForWrite3?.Invoke(cpu);
                }
                else
                {
                    ExecuterForWrite4?.Invoke(cpu);
                }
            }

            return this;
        }
    }
}
