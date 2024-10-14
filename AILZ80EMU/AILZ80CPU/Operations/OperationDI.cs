using AILZ80CPU.Extensions;
using AILZ80CPU.InstructionSet;
using System;
using System.Collections.Generic;

namespace AILZ80CPU.Operations
{
    public class OperationDI : OperationItem
    {
        private Action<CPUZ80>? ExecuterForFetch { get; set; }

        private OperationDI(InstructionItem instructionItem)
            : base(instructionItem)
        {
        }

        public static new OperationDI Create(InstructionItem instructionItem)
        {
            if (instructionItem.OpCode != OpCodeEnum.DI)
            {
                return default!;
            }

            var operationItem = new OperationDI(instructionItem);

            operationItem.ExecuterForFetch = (cpu) =>
            {
                cpu.Register.IFF1 = false;
            };

            return operationItem!;
        }

        public override OperationItem Execute(CPUZ80 cpu, int machineCycleIndex)
        {
            if (cpu.TimingCycle == TimingCycleEnum.M1_T2_L)
            {
                ExecuterForFetch?.Invoke(cpu);
            }

            return this;
        }
    }
}
