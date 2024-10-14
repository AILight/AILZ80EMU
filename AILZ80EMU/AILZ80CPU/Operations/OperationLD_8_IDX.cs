using AILZ80CPU.Extensions;
using AILZ80CPU.InstructionSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.Operations
{
    public class OperationLD_8_IDX : OperationItem
    {
        private Action<CPUZ80>? ExecuterForFetch1 { get; set; }
        private Action<CPUZ80>? ExecuterForRead2 { get; set; }
        private Action<CPUZ80>? ExecuterForRead4 { get; set; }

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForReadRegex = new Dictionary<string, Action<CPUZ80>>()
        {
            { @"^A,\s*\(I", (cpu) => cpu.Register.A = cpu.Bus.Data },
            { @"^B,\s*\(I", (cpu) => cpu.Register.A = cpu.Bus.Data },
            { @"^C,\s*\(I", (cpu) => cpu.Register.A = cpu.Bus.Data },
            { @"^D,\s*\(I", (cpu) => cpu.Register.A = cpu.Bus.Data },
            { @"^E,\s*\(I", (cpu) => cpu.Register.A = cpu.Bus.Data },
            { @"^H,\s*\(I", (cpu) => cpu.Register.A = cpu.Bus.Data },
            { @"^L,\s*\(I", (cpu) => cpu.Register.A = cpu.Bus.Data },
        };


        private OperationLD_8_IDX(InstructionItem instructionItem)
            : base(instructionItem)
        {
        }

        public static new OperationLD_8_IDX Create(InstructionItem instructionItem)
        {
            if (instructionItem.OpCode != OpCodeEnum.LD)
            {
                return default!;
            }
            
            var operationItem = new OperationLD_8_IDX(instructionItem);

            if (operandExecuterForReadRegex.TryGetValueRegex(instructionItem.Operand, out var executerForRead4))
            {
                operationItem.ExecuterForFetch1 = (cpu) =>
                {
                    cpu.Register.Internal_Memory_Pointer = cpu.Register.PC;
                    cpu.Register.PC++;
                };
                if (instructionItem.Operand.Contains("IX"))
                {
                    operationItem.ExecuterForRead2 = (cpu) => 
                    {
                        cpu.Register.Internal_Memory_Pointer = (ushort)(cpu.Register.IX + (sbyte)cpu.Bus.Data);
                    };
                }
                else
                {
                    operationItem.ExecuterForRead2 = (cpu) =>
                    {
                        cpu.Register.Internal_Memory_Pointer = (ushort)(cpu.Register.IY + (sbyte)cpu.Bus.Data);
                    };
                }
                operationItem.ExecuterForRead4 = executerForRead4;

                return operationItem;
            }

            return default!;
        }



        public override OperationItem Execute(CPUZ80 cpu, int machineCycleIndex)
        {
            if (cpu.TimingCycle == TimingCycleEnum.M1_T2_L)
            {
                if (machineCycleIndex == 1)
                {
                    ExecuterForFetch1?.Invoke(cpu);
                }
            }
            else if (cpu.TimingCycle == TimingCycleEnum.R1_T3_H)
            {
                if (machineCycleIndex == 2)
                {
                    ExecuterForRead2?.Invoke(cpu);
                }
                else if (machineCycleIndex == 4)
                {
                    ExecuterForRead4?.Invoke(cpu);
                }
            }

            return this;
        }
    }
}
