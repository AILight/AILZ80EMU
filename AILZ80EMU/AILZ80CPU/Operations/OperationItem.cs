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
        public byte? OpeCode { get; protected set; } = default(byte?);
        public InstructionItem? InstructionItem { get; private set; }
        protected MachineCycleEnum[] MachineCycles { get; set; }

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

        public virtual MachineCycleEnum GetMachineCycleEnum(CPUZ80 cpu, int machineCycleIndex)
        {
            if (MachineCycles.Length > machineCycleIndex)
            {
                return MachineCycles[machineCycleIndex];
            }
            else
            {
                return MachineCycleEnum.None;
            }
        }

        public static OperationItem Create(InstructionItem instructionItem)
        {
            var operationItem = default(OperationItem);
            operationItem = operationItem ?? OperationLD_8.Create(instructionItem);
            operationItem = operationItem ?? OperationLDIR.Create(instructionItem);

            return operationItem;
        }
    }
}
