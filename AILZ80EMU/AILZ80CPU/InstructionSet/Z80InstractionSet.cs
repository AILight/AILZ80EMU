using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.InstructionSet
{
    public class Z80InstractionSet
    {
        public InstructionItem[] InstructionItems { get; set; }
        public Z80InstractionSet() 
        {
            InstructionItems = new[]
            {
                new InstructionItem("r, ← r'", OpCodeEnum.LD, "r, r'", new []{ "01rr'" }, new[] { MachineCycleEnum.OpcodeFetch }),
            };
        }
    }
}
