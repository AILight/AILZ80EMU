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

        public OperationFetch(Dictionary<byte, OperationItem> operationItems)
        {
            MachineCycle = MachineCycleEnum.OpcodeFetch;
            OperationItems = operationItems;
        }

        public override void Execute(CPUZ80 cpu)
        {
            if (cpu.TimingCycle == TimingCycleEnum.M1_T2_L)
            {
                var opCode = cpu.Bus.Data;
                if (OperationItems.ContainsKey(opCode))
                {
                    var oprationItem = OperationItems[opCode];
                    oprationItem.Execute(cpu);
                    NextOperationItem = oprationItem.NextOperationItem;
                }
            }
        }
    }
}
