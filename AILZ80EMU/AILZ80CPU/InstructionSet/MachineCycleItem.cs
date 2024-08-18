using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.InstructionSet
{
    public class MachineCycleItem
    {
        public Dictionary<byte, Operand> Operands { get; set; }

        public MachineCycleItem() { }
    }
}
