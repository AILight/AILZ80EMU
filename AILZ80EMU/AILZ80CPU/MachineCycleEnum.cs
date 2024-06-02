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
        R1_T1_H = 101,
        R1_T1_L = 102,
        R1_T2_H = 103,
        R1_T2_L = 104,
        R1_T3_H = 105,
        R1_T3_L = 106,
        W1_T1_H = 201,
        W1_T1_L = 202,
        W1_T2_H = 203,
        W1_T2_L = 204,
        W1_T3_H = 205,
        W1_T3_L = 206,
    }
}
