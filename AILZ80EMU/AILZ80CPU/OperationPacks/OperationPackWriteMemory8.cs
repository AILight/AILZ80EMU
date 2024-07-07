using AILZ80LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.OperationPacks
{
    public class OperationPackWriteMemory8 : OperationPack
    {
        public byte OPCode { get; set; }

        public OperationPackWriteMemory8(CPUZ80 cpu, byte opCode)
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

            TimingCycleActions = new Dictionary<TimingCycleEnum, Func<OperationPack?>>()
            {
                [TimingCycleEnum.W1_T1_H] = () =>
                {
                    switch (OPCode)
                    {
                        case 0x02: // LD (BC),A
                            CPU.Bus.Address = CPU.Register.BC;
                            break;
                        default:
                            break;
                    }

                    return default;
                },
                [TimingCycleEnum.W1_T1_L] = () =>
                {
                    switch (OPCode)
                    {
                        case 0x02: // LD (BC),A
                            CPU.Bus.Data = CPU.Register.A;
                            break;
                        default:
                            break;
                    }

                    CPU.MREQ = false;

                    return default;
                },
                [TimingCycleEnum.W1_T2_H] = () =>
                {
                    return default;
                },
                [TimingCycleEnum.W1_T2_L] = () =>
                {
                    CPU.WR = false;

                    return default;
                },
                [TimingCycleEnum.W1_T3_H] = () =>
                {
                    return default;
                },
                [TimingCycleEnum.W1_T3_L] = () =>
                {
                    CPU.MREQ = true;
                    CPU.WR = true;

                    return default;
                },
            };
        }
    }
}