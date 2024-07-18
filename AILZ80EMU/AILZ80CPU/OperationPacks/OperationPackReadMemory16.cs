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
    public class OperationPackReadMemory16 : OperationPack
    {
        private byte OPCode { get; set; }
        private RegisterEnum ReadRegister { get; set; }
        private OperationPackReadMemory8 LocalOperationPackReadMemory8 { get; set; }
        private OperationPackReadMemory16 LocalOperationPackReadMemory16 { get; set; }
        private OperationPackReadMemory16Extend1 OperationPackReadMemory16Extend1 { get; set; }

        public OperationPackReadMemory16(CPUZ80 cpu)
        : base(cpu)

        {
            LocalOperationPackReadMemory8 = new OperationPackReadMemory8(cpu);
            LocalOperationPackReadMemory16 = new OperationPackReadMemory16(cpu);
            OperationPackReadMemory16Extend1 = new OperationPackReadMemory16Extend1(cpu);

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
                    switch (ReadRegister)
                    {
                        case RegisterEnum.PC:
                            CPU.Bus.Address = CPU.Register.PC;
                            CPU.Register.PC++;
                            break;
                        case RegisterEnum.DirectAddress:
                            CPU.Bus.Address = CPU.Register.DirectAdress;
                            break;
                        case RegisterEnum.SP:
                            CPU.Bus.Address = CPU.Register.SP;
                            CPU.Register.SP++;
                            break;
                        default:
                            break;
                    }
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
                    switch (OPCode)
                    {
                        case 0x01:  // LD BC,n'n
                        case 0xC1:  // POP BC
                            cpu.Register.C = data;
                            break;
                        case 0x11:  // LD DE,n'n
                        case 0xD1:  // POP DE
                            cpu.Register.E = data;
                            break;
                        case 0x21:  // LD HL,n'n
                        case 0xE1:  // POP HL
                            cpu.Register.L = data;
                            break;
                        case 0xF1:  // POP AF
                            cpu.Register.F = data;
                            break;
                        case 0x31:  // LD SP,n'n
                            cpu.Register.SP_L = data;
                            break;
                        case 0x2A:  // LD HL,(n'n)
                            if (ReadRegister == RegisterEnum.PC)
                            {
                                cpu.Register.DirectAddress_L = data;
                            }
                            else
                            {
                                cpu.Register.L = data;
                            }
                            break;
                        case 0xC3:  // JP n'n
                        case 0xC2:  // JP NZ,n'n
                        case 0xD2:  // JP NC,n'n
                        case 0xE2:  // JP PO,n'n
                        case 0xF2:  // JP P,n'n
                        case 0xCA:  // JP Z,n'n
                        case 0xDA:  // JP C,n'n
                        case 0xEA:  // JP PE,n'n
                        case 0xFA:  // JP M,n'n
                            cpu.Register.DirectAddress_L = data;
                            break;
                        case 0x3A:  // LD A,(n'n)
                            cpu.Register.DirectAddress_L = data;
                            break;
                        case 0xC9:  // RET
                            cpu.Register.PC_L = data;
                            break;
                        case 0xC4:  // CALL NZ,n'n
                        case 0xD4:  // CALL NC,n'n
                        case 0xE4:  // CALL PO,n'n
                        case 0xF4:  // CALL P,n'n
                        case 0xCC:  // CALL Z,n'n
                        case 0xDC:  // CALL C,n'n
                        case 0xEC:  // CALL PE,n'n
                        case 0xFC:  // CALL M,n'n
                            cpu.Register.DirectAddress_L = data;
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
                    switch (ReadRegister)
                    {
                        case RegisterEnum.PC:
                            CPU.Bus.Address = CPU.Register.PC;
                            CPU.Register.PC++;
                            break;
                        case RegisterEnum.DirectAddress:
                            CPU.Bus.Address = (ushort)(CPU.Register.DirectAdress + 1);
                            break;
                        case RegisterEnum.SP:
                            CPU.Bus.Address = CPU.Register.SP;
                            CPU.Register.SP++;
                            break;
                        default:
                            break;
                    }
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
                        case 0xC1:  // POP BC
                            cpu.Register.B = data;
                            break;
                        case 0x11:  // LD DE,n'n
                        case 0xD1:  // POP DE
                            cpu.Register.D = data;
                            break;
                        case 0x21:  // LD HL,n'n
                        case 0xE1:  // POP HL
                            cpu.Register.H = data;
                            break;
                        case 0xF1:  // POP AF
                            cpu.Register.A = data;
                            break;
                        case 0x31:  // LD SP,n'n
                            cpu.Register.SP_H = data;
                            break;
                        case 0x2A:  // LD HL,(n'n)
                            if (ReadRegister == RegisterEnum.PC)
                            {
                                cpu.Register.DirectAddress_H = data;
                                LocalOperationPackReadMemory16.SetOPCode(OPCode, RegisterEnum.DirectAddress);
                                return LocalOperationPackReadMemory16;
                            }
                            else
                            {
                                cpu.Register.H = data;
                            }
                            break;
                        case 0xC3:  // JP n'n
                            cpu.Register.DirectAddress_H = data;
                            cpu.Register.PC = cpu.Register.DirectAdress;
                            break;
                        case 0xC2:  // JP NZ,n'n
                        case 0xD2:  // JP NC,n'n
                        case 0xE2:  // JP PO,n'n
                        case 0xF2:  // JP P,n'n
                        case 0xCA:  // JP Z,n'n
                        case 0xDA:  // JP C,n'n
                        case 0xEA:  // JP PE,n'n
                        case 0xFA:  // JP M,n'n
                            cpu.Register.DirectAddress_H = data;
                            if (IsFlagOn(Select_cc(OPCode, 2)))
                            {
                                cpu.Register.PC = cpu.Register.DirectAdress;
                            }    
                            break;
                        case 0x3A:  // LD A,(n'n)
                            cpu.Register.DirectAddress_H = data;
                            LocalOperationPackReadMemory8.SetOPCode(OPCode, RegisterEnum.DirectAddress);
                            return LocalOperationPackReadMemory8;
                        case 0xC9:  // RET
                            cpu.Register.PC_H = data;
                            break;
                        case 0xC4:  // CALL NZ,n'n
                        case 0xD4:  // CALL NC,n'n
                        case 0xE4:  // CALL PO,n'n
                        case 0xF4:  // CALL P,n'n
                        case 0xCC:  // CALL Z,n'n
                        case 0xDC:  // CALL C,n'n
                        case 0xEC:  // CALL PE,n'n
                        case 0xFC:  // CALL M,n'n
                            cpu.Register.DirectAddress_H = data;
                            if (IsFlagOn(Select_cc(OPCode, 2)))
                            {
                                OperationPackReadMemory16Extend1.SetOPCode(OPCode);
                                return OperationPackReadMemory16Extend1;
                            }
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

        public void SetOPCode(byte opCode, RegisterEnum readRegister = RegisterEnum.PC)
        {
            OPCode = opCode;
            ReadRegister = readRegister;
            ExecuteIndex = 0;
        }
    }
}