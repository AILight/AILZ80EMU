using AILZ80LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.OperationPacks
{
    public class OperationPackOpcodeFetchExtend1 : OperationPack
    {
        public byte OPCode { get; set; }
        private OperationPackReadMemory16 OperationPackReadMemory16 { get; set; }
        private OperationPackWriteMemory16 OperationPackWriteMemory16 { get; set; }

        public OperationPackOpcodeFetchExtend1(CPUZ80 cpu)
            : base(cpu)
        {
            OperationPackReadMemory16 = new OperationPackReadMemory16(cpu);
            OperationPackWriteMemory16 = new OperationPackWriteMemory16(cpu);

            TimingCycles = new TimingCycleEnum[] {
                                TimingCycleEnum.M2_T1_H,
                                TimingCycleEnum.M2_T1_L,
                            };

            TimingCycleActions = new Dictionary<TimingCycleEnum, Func<OperationPack?>>()
            {
                [TimingCycleEnum.M2_T1_H] = () =>
                {
                    return default;
                },
                [TimingCycleEnum.M2_T1_L] = () =>
                {
                    switch (OPCode)
                    {
                        case 0xC0:  // RET NZ
                        case 0xC8:  // RET Z
                        case 0xD0:  // RET NC
                        case 0xD8:  // RET C
                        case 0xE0:  // RET PO
                        case 0xE8:  // RET PE
                        case 0xF0:  // RET P
                        case 0xF8:  // RET M
                            if (IsFlagOn(Select_cc(OPCode, 2)))
                            {
                                OperationPackReadMemory16.SetOPCode(OPCode, RegisterEnum.SP);
                                return  OperationPackReadMemory16;
                            }
                            break;
                        case 0xC5:  // PUSH BC
                        case 0xD5:  // PUSH DE
                        case 0xE5:  // PUSH HL
                        case 0xF5:  // PUSH AF
                            OperationPackWriteMemory16.SetOPCode(OPCode);
                            return OperationPackWriteMemory16;
                            break;
                        default:
                            break;
                    }
                    return default;
                },
            };
        }

        public void SetOPCode(byte opCode)
        {
            OPCode = opCode;
            ExecuteIndex = 0;
        }
    }
}