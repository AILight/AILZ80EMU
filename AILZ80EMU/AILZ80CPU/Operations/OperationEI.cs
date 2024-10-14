using AILZ80CPU.Extensions;
using AILZ80CPU.InstructionSet;
using System;
using System.Collections.Generic;

namespace AILZ80CPU.Operations
{
    public class OperationEI : OperationItem
    {
        private Action<CPUZ80>? ExecuterForFetch { get; set; }

        private OperationEI(InstructionItem instructionItem)
            : base(instructionItem)
        {
        }

        public static new OperationEI Create(InstructionItem instructionItem)
        {
            if (instructionItem.OpCode != OpCodeEnum.EI)
            {
                return default!;
            }

            var operationItem = new OperationEI(instructionItem);

            operationItem.ExecuterForFetch = (cpu) =>
            {
                cpu.Register.IFF1 = true;
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
