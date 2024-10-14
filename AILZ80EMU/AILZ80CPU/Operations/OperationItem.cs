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
            var opcodeFetchCount = instructionItem.MachineCycles.Count(m => m == MachineCycleEnum.OpcodeFetch);
            OpeCode = Convert.ToByte(instructionItem.OperandPatterns[opcodeFetchCount - 1], 2);
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
            operationItem = operationItem ?? OperationLD_8_IDX.Create(instructionItem);
            operationItem = operationItem ?? OperationLD_16_W.Create(instructionItem);
            operationItem = operationItem ?? OperationLD_16.Create(instructionItem);
            operationItem = operationItem ?? OperationLDIR.Create(instructionItem);
            operationItem = operationItem ?? OperationPUSH.Create(instructionItem);
            operationItem = operationItem ?? OperationPOP.Create(instructionItem);
            operationItem = operationItem ?? OperationEX.Create(instructionItem);
            operationItem = operationItem ?? OperationEXX.Create(instructionItem);
            operationItem = operationItem ?? OperationINC.Create(instructionItem);
            operationItem = operationItem ?? OperationDEC.Create(instructionItem);
            operationItem = operationItem ?? OperationADD.Create(instructionItem);
            operationItem = operationItem ?? OperationSUB.Create(instructionItem);
            operationItem = operationItem ?? OperationAND.Create(instructionItem);
            operationItem = operationItem ?? OperationOR.Create(instructionItem);
            operationItem = operationItem ?? OperationXOR.Create(instructionItem);
            operationItem = operationItem ?? OperationCP.Create(instructionItem);
            operationItem = operationItem ?? OperationJP.Create(instructionItem);
            operationItem = operationItem ?? OperationJR.Create(instructionItem);
            operationItem = operationItem ?? OperationCALL.Create(instructionItem);
            operationItem = operationItem ?? OperationRET.Create(instructionItem);
            operationItem = operationItem ?? OperationRLCA.Create(instructionItem);
            operationItem = operationItem ?? OperationRLA.Create(instructionItem);
            operationItem = operationItem ?? OperationRRCA.Create(instructionItem);
            operationItem = operationItem ?? OperationRRA.Create(instructionItem);
            operationItem = operationItem ?? OperationRST.Create(instructionItem);
            operationItem = operationItem ?? OperationIN.Create(instructionItem);
            operationItem = operationItem ?? OperationOUT.Create(instructionItem);
            operationItem = operationItem ?? OperationEI.Create(instructionItem);
            operationItem = operationItem ?? OperationDI.Create(instructionItem);

            return operationItem;
        }
    }
}
