using AILZ80LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.Cycles
{
    public class MachineCycleMemoryWrite : MachineCycle
    {
        public MachineCycleMemoryWrite(CPUZ80 cpu)
            : base(cpu)
        {
            TimingCycles = new TimingCycleEnum[] {
                                TimingCycleEnum.W1_T1_H,
                                TimingCycleEnum.W1_T1_L,
                                TimingCycleEnum.W1_T2_H,
                                TimingCycleEnum.W1_T2_L,
                                TimingCycleEnum.W1_T3_H,
                                TimingCycleEnum.W1_T3_L,
                            };

            TimingCycleActions = new Dictionary<TimingCycleEnum, Action>()
            {
                [TimingCycleEnum.W1_T1_H] = () =>
                {
                    /*
                    switch (OPCode)
                    {
                        case 0x02: // LD (BC),A
                            CPU.Bus.Address = CPU.Register.BC;
                            break;
                        case 0x12: // LD (DE),A
                            CPU.Bus.Address = CPU.Register.DE;
                            break;
                        case 0x70: // LD (HL),B
                        case 0x71: // LD (HL),C
                        case 0x72: // LD (HL),D
                        case 0x73: // LD (HL),E
                        case 0x74: // LD (HL),H
                        case 0x75: // LD (HL),L
                        case 0x77: // LD (HL),A
                            CPU.Bus.Address = CPU.Register.HL;
                            break;
                        default:
                            break;
                    }
                    */
                },
                [TimingCycleEnum.W1_T1_L] = () =>
                {
                    /*
                    switch (OPCode)
                    {
                        case 0x02: // LD (BC),A
                        case 0x12: // LD (DE),A
                        case 0x77: // LD (HL),A
                            CPU.Bus.Data = CPU.Register.A;
                            break;
                        case 0x70: // LD (HL),B
                            CPU.Bus.Data = CPU.Register.B;
                            break;
                        case 0x71: // LD (HL),C
                            CPU.Bus.Data = CPU.Register.C;
                            break;
                        case 0x72: // LD (HL),D
                            CPU.Bus.Data = CPU.Register.D;
                            break;
                        case 0x73: // LD (HL),E
                            CPU.Bus.Data = CPU.Register.E;
                            break;
                        case 0x74: // LD (HL),H
                            CPU.Bus.Data = CPU.Register.H;
                            break;
                        case 0x75: // LD (HL),L
                            CPU.Bus.Data = CPU.Register.L;
                            break;
                        default:
                            break;
                    }
                    */
                    cpu.MREQ = false;
                },
                [TimingCycleEnum.W1_T2_H] = () =>
                {
                },
                [TimingCycleEnum.W1_T2_L] = () =>
                {
                    cpu.WR = false;
                },
                [TimingCycleEnum.W1_T3_H] = () =>
                {
                },
                [TimingCycleEnum.W1_T3_L] = () =>
                {
                    cpu.MREQ = true;
                    cpu.WR = true;
                },
            };
        }
    }
}