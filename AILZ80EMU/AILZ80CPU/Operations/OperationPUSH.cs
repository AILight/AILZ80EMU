using AILZ80CPU.Extensions;
using AILZ80CPU.InstructionSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.Operations
{
    public class OperationPUSH : OperationItem
    {
        private Action<CPUZ80>? ExecuterForFetch { get; set; }
        private Action<CPUZ80>? ExecuterForWrite1 { get; set; }
        private Action<CPUZ80>? ExecuterForWrite2 { get; set; }

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForFetch = new Dictionary<string, Action<CPUZ80>>()
        {
            { @"BC", (cpu) => { cpu.Register.Internal_16bit_Register = cpu.Register.BC; } },
            { @"DE", (cpu) => { cpu.Register.Internal_16bit_Register = cpu.Register.DE; } },
            { @"HL", (cpu) => { cpu.Register.Internal_16bit_Register = cpu.Register.HL; } },
            { @"AF", (cpu) => { cpu.Register.Internal_16bit_Register = cpu.Register.AF; } },
        };


        private OperationPUSH(InstructionItem instructionItem)
            : base(instructionItem)
        {
        }

        public static new OperationPUSH Create(InstructionItem instructionItem)
        {
            if (instructionItem.OpCode != OpCodeEnum.PUSH)
            {
                return default!;
            }
            
            var executer = default(Action<CPUZ80>);
            var operationItem = new OperationPUSH(instructionItem);

            if (operandExecuterForFetch.TryGetValue(instructionItem.Operand, out executer))
            {
                operationItem.ExecuterForFetch = executer;
                operationItem.ExecuterForWrite1 = (cpu) =>
                {
                    cpu.Register.SP--; 
                    cpu.Bus.Address = cpu.Register.SP;
                    cpu.Bus.Data = cpu.Register.Internal_16bit_Register_H;
                };
                operationItem.ExecuterForWrite2 = (cpu) =>
                {
                    cpu.Register.SP--;
                    cpu.Bus.Address = cpu.Register.SP;
                    cpu.Bus.Data = cpu.Register.Internal_16bit_Register_L;
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
            else if (cpu.TimingCycle == TimingCycleEnum.W1_T3_H)
            {
                if (machineCycleIndex == 1)
                {
                    ExecuterForWrite1?.Invoke(cpu);
                }
                else
                {
                    ExecuterForWrite2?.Invoke(cpu);
                }
            }

            return this;
        }
    }
}
