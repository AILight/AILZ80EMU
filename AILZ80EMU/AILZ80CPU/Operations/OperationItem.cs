using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.Operations
{
    public abstract class OperationItem
    {
        public byte OpeCode { get; set; }
        public MachineCycleEnum[] MachineCycles { get; set; }

        public OperationItem()
        {
            MachineCycles = new[] { MachineCycleEnum.OpcodeFetch };
        }

        public virtual OperationItem Execute(CPUZ80 cpu, int machineCycleIndex)
        {
            return this;
        }

    }
}
