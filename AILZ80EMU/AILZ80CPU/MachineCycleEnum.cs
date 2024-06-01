using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU
{
    public enum MachineCycleEnum
    {
        None = 0,
        M1_T1_H = 1,
        M1_T1_L = 2,
        M1_T2_H = 3,
        M1_T2_L = 4,
        M1_T3_H = 5,
        M1_T3_L = 6,
        M1_T4_H = 7,
        M1_T4_L = 8,
        M2_T1_H = 11,
        M2_T1_L = 12,
        M2_T2_H = 13,
        M2_T2_L = 14,
        M2_T3_H = 15,
        M2_T3_L = 16,
    }
}
