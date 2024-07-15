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
        private bool IsCarry { get; set; }
        private UInt16 Value1 { get; set; }
        private UInt16 Value2 { get; set; }

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
                        case 0x09:  // ADD HL,BC
                        case 0x19:  // ADD HL,DE
                        case 0x29:  // ADD HL,HL
                        case 0x39:  // ADD HL,SP
                            Add16BitLow(Select_rp(OPCode, 2));
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
                        case 0x09:  // ADD HL,BC
                        case 0x19:  // ADD HL,DE
                        case 0x29:  // ADD HL,HL
                        case 0x39:  // ADD HL,SP
                            Add16BitHigh(Select_rp(OPCode, 2));
                            break;
                        default:
                            break;
                    }
                    return default;
                },

            };
        }
        
        public void Add16BitLow(RegisterEnum register)
        {
            Value1 = CPU.Register.HL;
            Value2 = register switch
            {
                RegisterEnum.BC => CPU.Register.BC,
                RegisterEnum.DE => CPU.Register.DE,
                RegisterEnum.SP => CPU.Register.SP,
                _ => throw new NotImplementedException()
            };
            

            var tmp = CPU.Register.L + (Value2 & 0x00FF);
            if (tmp >= 0x100)
            {
                IsCarry = true;
            }
            SetFlagForAdd16(tmp);
            CPU.Register.L = (byte)(tmp & 0xFF);
        }

        public void Add16BitHigh(RegisterEnum register)
        {
            var reg = register switch
            {
                RegisterEnum.BC => CPU.Register.B,
                RegisterEnum.DE => CPU.Register.D,
                RegisterEnum.SP => CPU.Register.SP_H,
                _ => throw new NotImplementedException()
            };

            var tmp = CPU.Register.H + reg;
            if (IsCarry)
            {
                tmp++;
            }
            CPU.Register.H = (byte)(tmp & 0xFF);
            SetFlagForAdd16(tmp);
        }

        public void SetFlagForAdd16(int value)
        {
            if (((Value1 & 0x0FFF) + (Value2 & 0x0FFF)) > 0x0FFF)
            {
                CPU.Register.F |= (byte)FlagEnum.HalfCarry;
            }
            else
            {
                CPU.Register.F &= (byte)~FlagEnum.HalfCarry;
            }

            if (value > 0xFF)
            {
                CPU.Register.F |= (byte)FlagEnum.Carry;
            }
            else
            {
                CPU.Register.F &= (byte)~FlagEnum.Carry;
            }
        }

        public void SetOPCode(byte opCode)
        {
            OPCode = opCode;
            ExecuteIndex = 0;
        }
    }
}