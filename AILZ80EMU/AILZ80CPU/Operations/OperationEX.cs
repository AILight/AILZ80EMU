using AILZ80CPU.Extensions;
using AILZ80CPU.InstructionSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.Operations
{
    public class OperationEX : OperationItem
    {
        private Action<CPUZ80>? ExecuterForFetch { get; set; }
        private Action<CPUZ80>? ExecuterForRead1 { get; set; }
        private Action<CPUZ80>? ExecuterForRead2 { get; set; }
        private Action<CPUZ80>? ExecuterForWrite1 { get; set; }
        private Action<CPUZ80>? ExecuterForWrite2 { get; set; }

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForFetch = new Dictionary<string, Action<CPUZ80>>()
        {
            { @"DE, HL", (cpu) => 
                { 
                    cpu.Register.Internal_16bit_Register = cpu.Register.DE;
                    cpu.Register.DE = cpu.Register.HL;
                    cpu.Register.DE = cpu.Register.Internal_16bit_Register;
                }},
            { @"AF, AF'", (cpu) =>
                {
                    cpu.Register.Internal_16bit_Register = cpu.Register.AF;
                    cpu.Register.AF_S = cpu.Register.AF;
                    cpu.Register.AF = cpu.Register.Internal_16bit_Register;
                }}
        };


        private OperationEX(InstructionItem instructionItem)
            : base(instructionItem)
        {
        }

        public static new OperationEX Create(InstructionItem instructionItem)
        {
            if (instructionItem.OpCode != OpCodeEnum.EX)
            {
                return default!;
            }

            var executer = default(Action<CPUZ80>);
            var operationItem = new OperationEX(instructionItem);

            if (operandExecuterForFetch.TryGetValue(instructionItem.Operand, out executer))
            {
                operationItem.ExecuterForFetch = executer;
                return operationItem;
            }
            else if (instructionItem.Operand == "(SP), HL")
            {
                operationItem.ExecuterForFetch = (cpu) =>
                {
                    cpu.Address = cpu.Register.SP;
                };
                operationItem.ExecuterForRead1 = (cpu) =>
                {
                    cpu.Register.Internal_16bit_Register_L = cpu.Bus.Data;
                    cpu.Register.Internal_Memory_Pointer = (ushort)(cpu.Register.SP + 1);
                };
                operationItem.ExecuterForRead2 = (cpu) =>
                {
                    cpu.Register.Internal_16bit_Register_H = cpu.Bus.Data;
                    cpu.Register.Internal_Memory_Pointer = (ushort)(cpu.Register.SP);
                };
                operationItem.ExecuterForWrite1 = (cpu) =>
                {
                    cpu.Bus.Data = cpu.Register.L;
                    cpu.Register.Internal_Memory_Pointer = (ushort)(cpu.Register.SP + 1);
                };
                operationItem.ExecuterForWrite2 = (cpu) =>
                {
                    cpu.Bus.Data = cpu.Register.H;
                    cpu.Register.HL = cpu.Register.Internal_16bit_Register;
                };

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
                else if (machineCycleIndex == 2)
                {
                    ExecuterForRead2?.Invoke(cpu);
                }
            }
            else if (cpu.TimingCycle == TimingCycleEnum.W1_T3_H)
            {
                if (machineCycleIndex == 4)
                {
                    ExecuterForWrite1?.Invoke(cpu);
                }
                else if (machineCycleIndex == 5)
                {
                    ExecuterForWrite2?.Invoke(cpu);
                }
            }
            return this;
        }
    }
}
