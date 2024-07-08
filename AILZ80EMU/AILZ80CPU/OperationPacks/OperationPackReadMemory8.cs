using AILZ80LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.OperationPacks
{
    public class OperationPackReadMemory8 : OperationPack
    {
        public byte OPCode { get; set; }

        public OperationPackReadMemory8(CPUZ80 cpu)
            : base(cpu)
        {
            TimingCycles = new TimingCycleEnum[] {
                                TimingCycleEnum.R1_T1_H,
                                TimingCycleEnum.R1_T1_L,
                                TimingCycleEnum.R1_T2_H,
                                TimingCycleEnum.R1_T2_L,
                                TimingCycleEnum.R1_T3_H,
                                TimingCycleEnum.R1_T3_L,
                            };

            TimingCycleActions = new Dictionary<TimingCycleEnum, Func<OperationPack?>>()
            {
                [TimingCycleEnum.R1_T1_H] = () =>
                {
                    CPU.Bus.Address = CPU.Register.PC;
                    CPU.Register.PC++;
                    CPU.M1 = false;

                    return default;
                },
                [TimingCycleEnum.R1_T1_L] = () =>
                {
                    CPU.MREQ = false;
                    CPU.RD = false;

                    return default;
                },
                [TimingCycleEnum.R1_T2_H] = () =>
                {
                    return default;
                },
                [TimingCycleEnum.R1_T2_L] = () =>
                {
                    return default;
                },
                [TimingCycleEnum.R1_T3_H] = () =>
                {
                    var data = CPU.Bus.Data;
                    switch (data)
                    {
                        case 0x06:  // LD B,n
                            cpu.Register.B = data;
                            break;
                        case 0x0E:  // LD C,n
                            cpu.Register.C = data;
                            break;
                        case 0x16:  // LD D,n
                            cpu.Register.D = data;
                            break;
                        case 0x1E:  // LD E,n
                            cpu.Register.E = data;
                            break;
                        case 0x26:  // LD H,n
                            cpu.Register.H = data;
                            break;
                        case 0x2E:  // LD L,n
                            cpu.Register.L = data;
                            break;
                        case 0x3E:  // LD A,n
                            cpu.Register.A = data;
                            break;
                        default:
                            break;
                    }

                    return default;
                },
                [TimingCycleEnum.R1_T3_L] = () =>
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