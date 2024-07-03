using AILZ80LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.OperationPacks
{
    public abstract class OperationPackOpcodeFetch : OperationPack
    {
        public OperationPackOpcodeFetch()
        {
            TimingCycles = new TimingCycleEnum[] {
                                TimingCycleEnum.M1_T1_H,
                                TimingCycleEnum.M1_T1_L,
                                TimingCycleEnum.M1_T2_H,
                                TimingCycleEnum.M1_T2_L,
                                TimingCycleEnum.M1_T3_H,
                                TimingCycleEnum.M1_T3_L,
                                TimingCycleEnum.M1_T4_H,
                                TimingCycleEnum.M1_T4_L };
        }
    }
}