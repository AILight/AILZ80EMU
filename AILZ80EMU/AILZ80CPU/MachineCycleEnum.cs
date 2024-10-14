using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU
{
    public enum MachineCycleEnum
    {
        None,
        OpcodeFetch,
        Process_1,
        Process_2,
        Process_5,
        MemoryRead,
        MemoryWrite,
        IORead,
        IOWrite,
    }
}
