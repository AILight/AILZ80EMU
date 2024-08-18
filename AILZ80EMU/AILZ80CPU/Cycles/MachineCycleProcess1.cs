using AILZ80LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.Cycles
{
    public class MachineCycleProcess1 : MachineCycle
    {
        public MachineCycleProcess1(CPUZ80 cpu)
            : base(cpu)
        {
            TimingCycles = new TimingCycleEnum[] {
                                TimingCycleEnum.M2_T1_H,
                                TimingCycleEnum.M2_T1_L,
                            };

            TimingCycleActions = new Dictionary<TimingCycleEnum, Action>()
            {
                [TimingCycleEnum.M2_T1_H] = () =>
                {
                    cpu.M1 = false;
                },
                [TimingCycleEnum.M2_T1_L] = () =>
                {
                },
            };
        }
    }
}