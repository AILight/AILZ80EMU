using AILZ80LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.OperationPacks
{
    public class OperationPackReadMemory16 : OperationPack
    {
        public byte OPCode { get; set; }

        public OperationPackReadMemory16(CPUZ80 cpu, byte opCode)
        : base(cpu)

        {
            OPCode = opCode;

            TimingCycles = new TimingCycleEnum[] {
                                TimingCycleEnum.R1_T1_H,
                                TimingCycleEnum.R1_T1_L,
                                TimingCycleEnum.R1_T2_H,
                                TimingCycleEnum.R1_T2_L,
                                TimingCycleEnum.R1_T3_H,
                                TimingCycleEnum.R1_T3_L,
                                TimingCycleEnum.R2_T1_H,
                                TimingCycleEnum.R2_T1_L,
                                TimingCycleEnum.R2_T2_H,
                                TimingCycleEnum.R2_T2_L,
                                TimingCycleEnum.R2_T3_H,
                                TimingCycleEnum.R2_T3_L };

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
                        case 0x01:  // LD BC,n'n
                            cpu.Register.C = data;
                            break;
                        case 0x11:  // LD DE,n'n
                            cpu.Register.E = data;
                            break;
                        case 0x21:  // LD HL,n'n
                            cpu.Register.L = data;
                            break;
                        case 0x31:  // LD SP,n'n
                            cpu.Register.SP_L = data;
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
                [TimingCycleEnum.R2_T1_H] = () =>
                {
                    CPU.Bus.Address = CPU.Register.PC;
                    CPU.Register.PC++;
                    CPU.M1 = false;

                    return default;
                },
                [TimingCycleEnum.R2_T1_L] = () =>
                {
                    CPU.MREQ = false;
                    CPU.RD = false;

                    return default;
                },
                [TimingCycleEnum.R2_T2_H] = () =>
                {
                    return default;
                },
                [TimingCycleEnum.R2_T2_L] = () =>
                {
                    return default;
                },
                [TimingCycleEnum.R2_T3_H] = () =>
                {
                    var data = CPU.Bus.Data;
                    switch (data)
                    {
                        case 0x01:  // LD BC,n'n
                            cpu.Register.B = data;
                            break;
                        case 0x11:  // LD DE,n'n
                            cpu.Register.D = data;
                            break;
                        case 0x21:  // LD HL,n'n
                            cpu.Register.H = data;
                            break;
                        case 0x31:  // LD SP,n'n
                            cpu.Register.SP_H = data;
                            break;
                        default:
                            break;
                    }

                    return default;
                },
                [TimingCycleEnum.R2_T3_L] = () =>
                {
                    return default;
                },
            };
        }
    }
}