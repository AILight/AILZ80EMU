using AILZ80LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.OperationPacks
{
    public abstract class OperationPack
    {
        public TimingCycleEnum[]? TimingCycles { get; set; }
        public Dictionary<TimingCycleEnum, Action<CPUZ80>>? TimingCycleActions { get; set; }

        public OperationPack()
        {
        }
    }
}