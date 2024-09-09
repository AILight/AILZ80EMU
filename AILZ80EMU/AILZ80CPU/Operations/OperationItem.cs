using AILZ80CPU.InstructionSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.Operations
{
    public abstract class OperationItem
    {
        public byte OpeCode { get; private set; }
        public MachineCycleEnum[] MachineCycles { get; private set; }
        public InstructionItem? InstructionItem { get; private set; }

        public OperationItem()
        {
            MachineCycles = new[] { MachineCycleEnum.OpcodeFetch };
        }

        public OperationItem(InstructionItem instructionItem)
        {
            MachineCycles = instructionItem.MachineCycles;
            InstructionItem = instructionItem;

        }

        public virtual OperationItem Execute(CPUZ80 cpu, int machineCycleIndex)
        {
            return this;
        }

    }
}
