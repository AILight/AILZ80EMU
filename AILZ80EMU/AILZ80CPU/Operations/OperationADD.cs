using AILZ80CPU.Extensions;
using AILZ80CPU.InstructionSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.Operations
{
    public class OperationADD : OperationItem
    {
        private Action<CPUZ80>? ExecuterForFetch { get; set; }
        private Action<CPUZ80>? ExecuterForRead { get; set; }
        private Action<CPUZ80>? ExecuterForWrite { get; set; }

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForFetch = new Dictionary<string, Action<CPUZ80>>()
        {
            { @"A, A", (cpu) => { cpu.Register.ADD_8(RegisterEnum.A); } },
            { @"A, B", (cpu) => { cpu.Register.ADD_8(RegisterEnum.B); } },
            { @"A, C", (cpu) => { cpu.Register.ADD_8(RegisterEnum.C); } },
            { @"A, D", (cpu) => { cpu.Register.ADD_8(RegisterEnum.D); } },
            { @"A, E", (cpu) => { cpu.Register.ADD_8(RegisterEnum.E); } },
            { @"A, H", (cpu) => { cpu.Register.ADD_8(RegisterEnum.H); } },
            { @"A, L", (cpu) => { cpu.Register.ADD_8(RegisterEnum.L); } },
            { @"A, n", (cpu) => { cpu.Address = cpu.Register.PC; cpu.Register.PC++; } },
        };

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForRead = new Dictionary<string, Action<CPUZ80>>()
        {
            { @"A, n", (cpu) => { cpu.Register.Internal_8bit_Register = cpu.Bus.Data; cpu.Register.ADD_8(RegisterEnum.Internal_8bit_Register); } },
        };

        private OperationADD(InstructionItem instructionItem)
            : base(instructionItem)
        {
        }

        public static new OperationADD Create(InstructionItem instructionItem)
        {
            if (instructionItem.OpCode != OpCodeEnum.ADD)
            {
                return default!;
            }
            
            var executer = default(Action<CPUZ80>);
            var operationItem = new OperationADD(instructionItem);

            if (operandExecuterForFetch.TryGetValue(instructionItem.Operand, out executer))
            {
                operationItem.ExecuterForFetch = executer;
                if (operandExecuterForFetch.TryGetValue(instructionItem.Operand, out var executerForRead))
                {
                    operationItem.ExecuterForRead = executerForRead;
                }
                return operationItem;
            }
            else if (instructionItem.Operand == "A, (HL)")
            {
                operationItem.ExecuterForFetch = (cpu) =>
                {
                    cpu.Bus.Address = cpu.Register.HL;
                };
                operationItem.ExecuterForRead = (cpu) =>
                {
                    cpu.Register.Internal_8bit_Register = cpu.Bus.Data;
                    cpu.Register.ADD_8(RegisterEnum.Internal_8bit_Register);
                };
                operationItem.ExecuterForWrite = (cpu) =>
                {
                    cpu.Bus.Data = cpu.Register.Internal_8bit_Register;
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
                ExecuterForRead?.Invoke(cpu);
            }
            else if (cpu.TimingCycle == TimingCycleEnum.W1_T3_H)
            {
                ExecuterForWrite?.Invoke(cpu);
            }
            return this;
        }
    }
}
