using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.Operations
{
    public class OperationFetch : OperationItem
    {
        private Dictionary<byte, OperationItem> OperationItems { get; set; }

        public OperationFetch(byte baseOpeCode, OperationItem[] operationItems)
            : this(operationItems)
        {
            OpeCode = baseOpeCode;
        }

        public OperationFetch(OperationItem[] operationItems)
        {
            OpeCode = default;
            MachineCycles = new[] { MachineCycleEnum.OpcodeFetch };
            OperationItems = operationItems.ToDictionary(m => m.OpeCode!.Value, m => m);
        }

        public override OperationItem Execute(CPUZ80 cpu, int machineCycleIndex)
        {
            if (cpu.TimingCycle == TimingCycleEnum.M1_T2_L)
            {
                var opCode = cpu.Bus.Data;
                cpu.Register.Internal_OpCode = opCode;
                if (OperationItems.ContainsKey(opCode))
                {
                    var oprationItem = OperationItems[opCode];
                    return oprationItem.Execute(cpu, machineCycleIndex);
                }
            }

            return this;
        }
    }
}
