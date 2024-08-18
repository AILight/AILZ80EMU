using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.Cycles
{
    public class OperationItem
    {
        public MachineCycleEnum MachineCycle { get; set; }
        public OperationItem? NextOperationItem { get; set; }

        public OperationItem() 
        {
        }

        public void Execute(CPUZ80 cpu)
        {

        }
    }
}
