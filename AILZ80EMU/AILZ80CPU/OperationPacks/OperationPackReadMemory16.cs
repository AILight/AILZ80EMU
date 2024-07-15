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

        public OperationPackReadMemory16(CPUZ80 cpu)
        : base(cpu)

        {
            LocalOperationPackReadMemory8 = new OperationPackReadMemory8(cpu);
            LocalOperationPackReadMemory16 = new OperationPackReadMemory16(cpu);

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
                            CPU.Bus.Address = CPU.Register.PC;
                            CPU.Register.SP--;
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
                        case 0x3A:  // LD A,(n'n)
                            cpu.Register.DirectAddress_L = data;
                            break;
                        case 0xC9:  // RET
                            cpu.Register.PC_L = data;
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
                            CPU.Bus.Address = CPU.Register.PC;
                            CPU.Register.SP--;
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
                        case 0x3A:  // LD A,(n'n)
                            cpu.Register.DirectAddress_H = data;
                            LocalOperationPackReadMemory8.SetOPCode(OPCode, RegisterEnum.DirectAddress);
                            return LocalOperationPackReadMemory8;
                        case 0xC9:  // RET
                            cpu.Register.PC_H = data;
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