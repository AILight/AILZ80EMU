using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU
{
    public class OperationPack
    {
        public byte[]? OPs { get; set; }
        public Dictionary<MachineCycleEnum, Action>? Actions { get; set; }
        public Dictionary<MachineCycleEnum, MachineCycleEnum>? NextMachineCycleDic { get; set; }
        public MachineCycleEnum EndMachineCycle { get; set; }
    }
}