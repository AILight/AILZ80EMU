using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU
{
    public enum RegisterEnum
    {
        AF, BC, DE, HL,
        AF_S, BC_S, DE_S, HL_S,
        IX, IY, SP, PC,
        I, R,
        A, F, B, C, D, E, H, L,
        IXH, IXL, IYH, IYL,
        IndirectHL,
        DirectAddress,
    }
}
