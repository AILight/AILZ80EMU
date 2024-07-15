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
        private byte OPCode { get; set; }
        private RegisterEnum ReadRegister { get; set; }

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
                    switch (ReadRegister)
                    {
                        case RegisterEnum.BC:
                            CPU.Bus.Address = CPU.Register.BC;
                            break;
                        case RegisterEnum.DE:
                            CPU.Bus.Address = CPU.Register.DE;
                            break;
                        case RegisterEnum.HL:
                        case RegisterEnum.IndirectHL:
                            CPU.Bus.Address = CPU.Register.HL;
                            break;
                        case RegisterEnum.SP:
                            CPU.Bus.Address = CPU.Register.SP;
                            break;
                        case RegisterEnum.PC:
                            CPU.Bus.Address = CPU.Register.PC;
                            CPU.Register.PC++;
                            break;
                        case RegisterEnum.DirectAddress:
                            CPU.Bus.Address = CPU.Register.DirectAdress;
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
                        case 0x06:  // LD B,n
                        case 0x46:  // LD B,(HL)
                            cpu.Register.B = data;
                            break;
                        case 0x0E:  // LD C,n
                        case 0x4E:  // LD C,(HL)
                            cpu.Register.C = data;
                            break;
                        case 0x16:  // LD D,n
                        case 0x56:  // LD D,(HL)
                            cpu.Register.D = data;
                            break;
                        case 0x1E:  // LD E,n
                        case 0x5E:  // LD E,(HL)
                            cpu.Register.E = data;
                            break;
                        case 0x26:  // LD H,n
                        case 0x66:  // LD H,(HL)
                            cpu.Register.H = data;
                            break;
                        case 0x2E:  // LD L,n
                        case 0x6E:  // LD L,(HL)
                            cpu.Register.L = data;
                            break;
                        case 0x0A:  // LD A,(BC)
                        case 0x1A:  // LD A,(DE)
                        case 0x7E:  // LD A,(HL)
                        case 0x3E:  // LD A,n
                        case 0x3A:  // LD A,(n'n);
                            cpu.Register.A = data;
                            break;
                        case 0x86:  // ADD A,(HL)
                            cpu.Register.Indirect_HL = data;
                            ExecuteADD(RegisterEnum.IndirectHL);
                            break;
                        case 0x8E:  // ADC A,(HL)
                            cpu.Register.Indirect_HL = data;
                            ExecuteADC(RegisterEnum.IndirectHL);
                            break;
                        case 0x96:  // SUB (HL)
                            cpu.Register.Indirect_HL = data;
                            ExecuteSUB(RegisterEnum.IndirectHL);
                            break;
                        case 0x9E:  // SBC (HL)
                            cpu.Register.Indirect_HL = data;
                            ExecuteSBC(RegisterEnum.IndirectHL);
                            break;
                        case 0xA6:  // AND (HL)
                            cpu.Register.Indirect_HL = data;
                            ExecuteAND(RegisterEnum.IndirectHL);
                            break;
                        case 0xAE:  // XOR (HL)
                            cpu.Register.Indirect_HL = data;
                            ExecuteXOR(RegisterEnum.IndirectHL);
                            break;
                        case 0xB6:  // OR (HL)
                            cpu.Register.Indirect_HL = data;
                            ExecuteOR(RegisterEnum.IndirectHL);
                            break;
                        case 0xBE:  // CP (HL)
                            cpu.Register.Indirect_HL = data;
                            ExecuteCP(RegisterEnum.IndirectHL);
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

        public void SetOPCode(byte opCode, RegisterEnum readRegister = RegisterEnum.PC)
        {
            OPCode = opCode;
            ReadRegister = readRegister;
            ExecuteIndex = 0;
        }
    }
}