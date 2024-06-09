using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU
{
    [Flags]
    public enum MachineCycleEnum
    {
        OpcodeFetch,
        MemoryRead,
        MemoryWrite,
    }
}
