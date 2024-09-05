using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.Operations
{
    public class OperationItem
    {
        //public MachineCycleEnum MachineCycle { get; set; }
        //public OperationItem? NextOperationItem { get; set; }
        public bool IsEnd => (TimingCycles?.Length ?? 0) <= ExecuteIndex;

        public OperationItem()
        {
        }

        public virtual void Execute(CPUZ80 cpu)
        {

        }
    }
}
