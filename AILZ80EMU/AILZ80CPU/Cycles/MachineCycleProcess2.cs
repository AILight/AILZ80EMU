using AILZ80LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.Cycles
{
    public class MachineCycleProcess2 : MachineCycle
    {
        public MachineCycleProcess2(CPUZ80 cpu)
            : base(cpu)
        {
            TimingCycles = new TimingCycleEnum[] {
                                TimingCycleEnum.M2_T1_H,
                                TimingCycleEnum.M2_T1_L,
                                TimingCycleEnum.M2_T2_H,
                                TimingCycleEnum.M2_T2_L,
                            };

            TimingCycleActions = new Dictionary<TimingCycleEnum, Action>()
            {
                [TimingCycleEnum.M2_T1_H] = () =>
                {
                },
                [TimingCycleEnum.M2_T1_L] = () =>
                {
                },
                [TimingCycleEnum.M2_T2_H] = () =>
                {
                },
                [TimingCycleEnum.M2_T2_L] = () =>
                {
                },
            };
        }
    }
}