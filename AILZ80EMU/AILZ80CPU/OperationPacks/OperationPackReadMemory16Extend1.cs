using AILZ80LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AILZ80CPU.OperationPacks
{
    public class OperationPackReadMemory16Extend1 : OperationPack
    {
        private byte OPCode { get; set; }
        private OperationPackWriteMemory16 LocalOperationPackWriteMemory16 { get; set; }

        public OperationPackReadMemory16Extend1(CPUZ80 cpu)
        : base(cpu)

        {
            LocalOperationPackWriteMemory16 = new OperationPackWriteMemory16(cpu);

            TimingCycles = new TimingCycleEnum[] {
                                TimingCycleEnum.R2_T4_H,
                                TimingCycleEnum.R2_T4_L };

            TimingCycleActions = new Dictionary<TimingCycleEnum, Func<OperationPack?>>()
            {
                [TimingCycleEnum.R2_T4_H] = () =>
                {
                    return default;
                },
                [TimingCycleEnum.R2_T4_L] = () =>
                {
                    switch (OPCode)
                    {
                        case 0xC4:  // CALL NZ,n'n
                        case 0xD4:  // CALL NC,n'n
                        case 0xE4:  // CALL PO,n'n
                        case 0xF4:  // CALL P,n'n
                        case 0xCC:  // CALL Z,n'n
                        case 0xDC:  // CALL C,n'n
                        case 0xEC:  // CALL PE,n'n
                        case 0xFC:  // CALL M,n'n
                            LocalOperationPackWriteMemory16.SetOPCode(OPCode, RegisterEnum.SP);
                            return LocalOperationPackWriteMemory16;
                        default:
                            break;
                    }
                    return default;
                }
            };
        }

        public void SetOPCode(byte opCode)
        {
            OPCode = opCode;
            ExecuteIndex = 0;
        }
    }
}