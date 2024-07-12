using AILZ80LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.OperationPacks
{
    public class OperationPackOpcodeFetchExtend7 : OperationPack
    {
        public byte OPCode { get; set; }
        public bool IsCarry { get; set; }

        public OperationPackOpcodeFetchExtend7(CPUZ80 cpu)
            : base(cpu)
        {
            TimingCycles = new TimingCycleEnum[] {
                                TimingCycleEnum.M2_T1_H,
                                TimingCycleEnum.M2_T1_L,
                                TimingCycleEnum.M2_T2_H,
                                TimingCycleEnum.M2_T2_L,
                                TimingCycleEnum.M2_T3_H,
                                TimingCycleEnum.M2_T3_L,
                                TimingCycleEnum.M2_T4_H,
                                TimingCycleEnum.M2_T4_L,
                                TimingCycleEnum.M3_T1_H,
                                TimingCycleEnum.M3_T1_L,
                                TimingCycleEnum.M3_T2_H,
                                TimingCycleEnum.M3_T2_L,
                                TimingCycleEnum.M3_T3_H,
                                TimingCycleEnum.M3_T3_L,
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
                    return default;
                },
                [TimingCycleEnum.M2_T2_H] = () =>
                {
                    cpu.MREQ = false;
                    cpu.RD = false;
                    return default;
                },
                [TimingCycleEnum.M2_T2_L] = () =>
                {
                    return default;
                },
                [TimingCycleEnum.M2_T3_H] = () =>
                {
                    cpu.M1 = true;
                    cpu.MREQ = true;
                    cpu.RFSH = false;
                    return default;
                },
                [TimingCycleEnum.M2_T3_L] = () =>
                {
                    IsCarry = false;
                    switch (OPCode)
                    {
                        case 0x09:   // ADD HL, BC (ADD L, C)
                            var tmp = CPU.Register.L + CPU.Register.C;
                            if (tmp >= 0x100)
                            {
                                IsCarry = true;
                            }
                            CPU.Register.L = (byte)(tmp & 0xFF);
                            break;
                        default:
                            break;
                    }
                    cpu.MREQ = false;
                    return default;
                },
                [TimingCycleEnum.M2_T4_H] = () =>
                {
                    return default;
                },
                [TimingCycleEnum.M2_T4_L] = () =>
                {
                    cpu.MREQ = true;

                    return default;
                },
                [TimingCycleEnum.M3_T1_H] = () =>
                {
                    cpu.RFSH = true;
                    return default;
                },
                [TimingCycleEnum.M3_T1_L] = () =>
                {
                    return default;
                },
                [TimingCycleEnum.M3_T2_H] = () =>
                {
                    return default;
                },
                [TimingCycleEnum.M3_T2_L] = () =>
                {
                    return default;
                },
                [TimingCycleEnum.M3_T3_H] = () =>
                {
                    return default;
                },
                [TimingCycleEnum.M3_T3_L] = () =>
                {
                    switch (OPCode)
                    {
                        case 0x09:   // ADD HL, BC (ADD H, B)
                            var tmp = CPU.Register.H + CPU.Register.B;
                            if (IsCarry)
                            {
                                tmp++;
                            }
                            CPU.Register.H = (byte)(tmp & 0xFF);
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