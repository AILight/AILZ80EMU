using AILZ80LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.OperationPacks
{
    public class OperationPackOpcodeFetchExtend2 : OperationPack
    {
        public byte OPCode { get; set; }

        public OperationPackOpcodeFetchExtend2(CPUZ80 cpu)
            : base(cpu)
        {
            TimingCycles = new TimingCycleEnum[] {
                                TimingCycleEnum.M2_T1_H,
                                TimingCycleEnum.M2_T1_L,
                                TimingCycleEnum.M2_T2_H,
                                TimingCycleEnum.M2_T2_L,
                                TimingCycleEnum.M2_T3_H,
                                TimingCycleEnum.M2_T3_L,
                            };

            TimingCycleActions = new Dictionary<TimingCycleEnum, Func<OperationPack?>>()
            {
                [TimingCycleEnum.M2_T1_H] = () =>
                {
                    cpu.M1 = false;
                    return default;
                },
                [TimingCycleEnum.M2_T1_L] = () =>
                {
                    switch (OPCode)
                    {
                        case 0x03:   // INC BC
                            CPU.Register.BC++;
                            break;
                        case 0x13:   // INC DE
                            CPU.Register.DE++;
                            break;
                        case 0x23:   // INC HL
                            CPU.Register.HL++;
                            break;
                        case 0x33:   // INC SP
                            CPU.Register.SP++;
                            break;
                        default:
                            break;
                    }
                    return default;
                },
                [TimingCycleEnum.M2_T2_H] = () =>
                {
                    return default;
                },
                [TimingCycleEnum.M2_T2_L] = () =>
                {
                    return default;
                },
                [TimingCycleEnum.M2_T3_H] = () =>
                {
                    return default;
                },
                [TimingCycleEnum.M2_T3_L] = () =>
                {
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