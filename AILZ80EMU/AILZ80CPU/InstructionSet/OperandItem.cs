using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.InstructionSet
{
    public class OperandItem
    {
        public Operand[] Operands { get; set; }
        public InstructionItem BaseInstructionItem { get; set; }

        public OperandItem() 
        {
        }
    }
}
