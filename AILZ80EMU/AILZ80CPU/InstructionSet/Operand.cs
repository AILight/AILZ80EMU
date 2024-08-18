using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.InstructionSet
{
    public class Operand
    {
        public byte Data { get; set; }
        public OperandDataTypeEnum OperandDataType { get; set; }
    }
}
