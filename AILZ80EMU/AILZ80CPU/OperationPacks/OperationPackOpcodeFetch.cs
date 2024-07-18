using AILZ80LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AILZ80CPU.OperationPacks
{
    public class OperationPackOpcodeFetch : OperationPack
    {
        private OperationPackReadMemory8 OperationPackReadMemory8 { get; set; }
        private OperationPackReadMemory16 OperationPackReadMemory16 { get; set; }
        private OperationPackWriteMemory8 OperationPackWriteMemory8 { get; set; }
        private OperationPackWriteMemory16 OperationPackWriteMemory16 { get; set; }
        private OperationPackOpcodeFetchExtend1 OperationPackOpcodeFetchExtend1 { get; set; }
        private OperationPackOpcodeFetchExtend2 OperationPackOpcodeFetchExtend2 { get; set; }
        private OperationPackOpcodeFetchExtend7 OperationPackOpcodeFetchExtend7 { get; set; }

        public OperationPackOpcodeFetch(CPUZ80 cpu)
            : base(cpu)
        {
            OperationPackReadMemory8 = new OperationPackReadMemory8(cpu);
            OperationPackReadMemory16 = new OperationPackReadMemory16(cpu);
            OperationPackWriteMemory8 = new OperationPackWriteMemory8(cpu);
            OperationPackWriteMemory16 = new OperationPackWriteMemory16(cpu);

            OperationPackOpcodeFetchExtend1 = new OperationPackOpcodeFetchExtend1(cpu);
            OperationPackOpcodeFetchExtend2 = new OperationPackOpcodeFetchExtend2(cpu);
            OperationPackOpcodeFetchExtend7 = new OperationPackOpcodeFetchExtend7(cpu);

            TimingCycles = new TimingCycleEnum[] {
                                TimingCycleEnum.M1_T1_H,
                                TimingCycleEnum.M1_T1_L,
                                TimingCycleEnum.M1_T2_H,
                                TimingCycleEnum.M1_T2_L,
                                TimingCycleEnum.M1_T3_H,
                                TimingCycleEnum.M1_T3_L,
                                TimingCycleEnum.M1_T4_H,
                                TimingCycleEnum.M1_T4_L };

            TimingCycleActions = new Dictionary<TimingCycleEnum, Func<OperationPack?>>()
            {
                [TimingCycleEnum.M1_T1_H] = () =>
                {
                    CPU.Bus.Address = CPU.Register.PC;
                    CPU.Register.PC++;
                    CPU.M1 = false;

                    return default;
                },
                [TimingCycleEnum.M1_T1_L] = () =>
                {
                    CPU.MREQ = false;
                    CPU.RD = false;

                    return default;
                },
                [TimingCycleEnum.M1_T2_H] = () =>
                {
                    return default;
                },
                [TimingCycleEnum.M1_T2_L] = () =>
                {
                    var opCode = CPU.Bus.Data;
                    var result = default(OperationPack);
                    switch (opCode)
                    {
                        case 0x00:
                            break;
                        case 0x06:  // LD B,n
                        case 0x0E:  // LD C,n
                        case 0x16:  // LD D,n
                        case 0x1E:  // LD E,n
                        case 0x26:  // LD H,n
                        case 0x2E:  // LD L,n
                        case 0x3E:  // LD A,n
                            OperationPackReadMemory8.SetOPCode(opCode);
                            result = OperationPackReadMemory8;
                            break;
                        case 0x01:  // LD BC,n'n
                        case 0x11:  // LD DE,n'n
                        case 0x21:  // LD HL,n'n
                        case 0x31:  // LD SP,n'n
                        case 0x2A:  // LD HL,(n'n)
                            OperationPackReadMemory16.SetOPCode(opCode);
                            result = OperationPackReadMemory16;
                            break;
                        case 0x40:  // LD B,B
                        case 0x41:  // LD B,C
                        case 0x42:  // LD B,D
                        case 0x43:  // LD B,E
                        case 0x44:  // LD B,H
                        case 0x45:  // LD B,L
                        case 0x47:  // LD B,A
                        case 0x48:  // LD C,B
                        case 0x49:  // LD C,C
                        case 0x4A:  // LD C,D
                        case 0x4B:  // LD C,E
                        case 0x4C:  // LD C,H
                        case 0x4D:  // LD C,L
                        case 0x4F:  // LD C,A
                        case 0x50:  // LD D,B
                        case 0x51:  // LD D,C
                        case 0x52:  // LD D,D
                        case 0x53:  // LD D,E
                        case 0x54:  // LD D,H
                        case 0x55:  // LD D,L
                        case 0x57:  // LD D,A
                        case 0x58:  // LD E,B
                        case 0x59:  // LD E,C
                        case 0x5A:  // LD E,D
                        case 0x5B:  // LD E,E
                        case 0x5C:  // LD E,H
                        case 0x5D:  // LD E,L
                        case 0x5F:  // LD E,A
                        case 0x60:  // LD H,B
                        case 0x61:  // LD H,C
                        case 0x62:  // LD H,D
                        case 0x63:  // LD H,E
                        case 0x64:  // LD H,H
                        case 0x65:  // LD H,L
                        case 0x67:  // LD H,A
                        case 0x68:  // LD L,B
                        case 0x69:  // LD L,C
                        case 0x6A:  // LD L,D
                        case 0x6B:  // LD L,E
                        case 0x6C:  // LD L,H
                        case 0x6D:  // LD L,L
                        case 0x6F:  // LD L,A
                            ExecuteLD(Select_r(opCode, 2), Select_r(opCode, 5));
                            break;
                        case 0x0A:  // LD A,(BC)
                            OperationPackReadMemory8.SetOPCode(opCode, RegisterEnum.BC);
                            result = OperationPackReadMemory8;
                            break;
                        case 0x1A:  // LD A,(DE)
                            OperationPackReadMemory8.SetOPCode(opCode, RegisterEnum.DE);
                            result = OperationPackReadMemory8;
                            break;
                        case 0x7E:  // LD A,(HL)
                        case 0x46:  // LD B,(HL)
                        case 0x4E:  // LD C,(HL)
                        case 0x56:  // LD D,(HL)
                        case 0x5E:  // LD E,(HL)
                        case 0x66:  // LD H,(HL)
                        case 0x6E:  // LD L,(HL)
                            OperationPackReadMemory8.SetOPCode(opCode, RegisterEnum.HL);
                            result = OperationPackReadMemory8;
                            break;
                        case 0x02:  // LD (BC),A
                        case 0x12:  // LD (DE),A
                        case 0x70:  // LD (HL),B
                        case 0x71:  // LD (HL),C
                        case 0x72:  // LD (HL),D
                        case 0x73:  // LD (HL),E
                        case 0x74:  // LD (HL),H
                        case 0x75:  // LD (HL),L
                        case 0x77:  // LD (HL),A
                            OperationPackWriteMemory8.SetOPCode(opCode);
                            result = OperationPackWriteMemory8;
                            break;
                        case 0x3A:  // LD A,(n'n)
                            OperationPackReadMemory8.SetOPCode(opCode);
                            result = OperationPackReadMemory8;
                            break;
                        case 0xC1:  // POP BC
                        case 0xD1:  // POP DE
                        case 0xE1:  // POP HL
                        case 0xF1:  // POP AF
                            OperationPackReadMemory16.SetOPCode(opCode, RegisterEnum.SP);
                            result = OperationPackReadMemory16;
                            break;
                        case 0xC5:  // PUSH BC
                        case 0xD5:  // PUSH DE
                        case 0xE5:  // PUSH HL
                        case 0xF5:  // PUSH AF
                            OperationPackOpcodeFetchExtend1.SetOPCode(opCode);
                            result = OperationPackOpcodeFetchExtend1;
                            break;
                        case 0x80:  // ADD A,B
                        case 0x81:  // ADD A,C
                        case 0x82:  // ADD A,D
                        case 0x83:  // ADD A,E
                        case 0x84:  // ADD A,H
                        case 0x85:  // ADD A,L
                        case 0x87:  // ADD A,A
                            ExecuteADD(Select_r(opCode, 5));
                            break;
                        case 0x86:  // ADD A,(HL)
                            OperationPackReadMemory8.SetOPCode(opCode, RegisterEnum.HL);
                            result = OperationPackReadMemory8;
                            break;
                        case 0x88:  // ADC A,B
                        case 0x89:  // ADC A,C
                        case 0x8A:  // ADC A,D
                        case 0x8B:  // ADC A,E
                        case 0x8C:  // ADC A,H
                        case 0x8D:  // ADC A,L
                        case 0x8F:  // ADC A,A
                            ExecuteADC(Select_r(opCode, 5));
                            break;
                        case 0x90:  // SUB B
                        case 0x91:  // SUB C
                        case 0x92:  // SUB D
                        case 0x93:  // SUB E
                        case 0x94:  // SUB H
                        case 0x95:  // SUB L
                        case 0x97:  // SUB A
                            ExecuteSUB(Select_r(opCode, 5));
                            break;
                        case 0x98:  // SBC B
                        case 0x99:  // SBC C
                        case 0x9A:  // SBC D
                        case 0x9B:  // SBC E
                        case 0x9C:  // SBC H
                        case 0x9D:  // SBC L
                        case 0x9F:  // SBC A
                            ExecuteSBC(Select_r(opCode, 5));
                            break;
                        case 0xA0:  // AND B
                        case 0xA1:  // AND C
                        case 0xA2:  // AND D
                        case 0xA3:  // AND E
                        case 0xA4:  // AND H
                        case 0xA5:  // AND L
                        case 0xA7:  // AND A
                            ExecuteAND(Select_r(opCode, 5));
                            break;
                        case 0xA8:  // XOR B
                        case 0xA9:  // XOR C
                        case 0xAA:  // XOR D
                        case 0xAB:  // XOR E
                        case 0xAC:  // XOR H
                        case 0xAD:  // XOR L
                        case 0xAF:  // XOR A
                            ExecuteXOR(Select_r(opCode, 5));
                            break;
                        case 0xB0:  // OR B
                        case 0xB1:  // OR C
                        case 0xB2:  // OR D
                        case 0xB3:  // OR E
                        case 0xB4:  // OR H
                        case 0xB5:  // OR L
                        case 0xB7:  // OR A
                            ExecuteOR(Select_r(opCode, 5));
                            break;
                        case 0xB8:  // CP B
                        case 0xB9:  // CP C
                        case 0xBA:  // CP D
                        case 0xBB:  // CP E
                        case 0xBC:  // CP H
                        case 0xBD:  // CP L
                        case 0xBF:  // CP A
                            ExecuteCP(Select_r(opCode, 5));
                            break;
                        case 0x09:  // ADD HL,BC
                        case 0x19:  // ADD HL,DE
                        case 0x29:  // ADD HL,HL
                        case 0x39:  // ADD HL,SP
                            OperationPackOpcodeFetchExtend7.SetOPCode(opCode);
                            result = OperationPackOpcodeFetchExtend7;
                            break;
                        case 0x03:  // INC BC
                        case 0x13:  // INC DE
                        case 0x23:  // INC HL
                        case 0x33:  // INC SP
                            OperationPackOpcodeFetchExtend2.SetOPCode(opCode);
                            result = OperationPackOpcodeFetchExtend2;
                            break;
                        case 0x04:  // INC B
                        case 0x0C:  // INC C
                        case 0x14:  // INC D
                        case 0x1C:  // INC E
                        case 0x24:  // INC H
                        case 0x2C:  // INC L
                        case 0x3C:  // INC A
                            ExecuteINC8(Select_r(opCode, 2));
                            break;
                        case 0x05:  // DEC B
                        case 0x0D:  // DEC C
                        case 0x15:  // DEC D
                        case 0x1D:  // DEC E
                        case 0x25:  // DEC H
                        case 0x2D:  // DEC L
                        case 0x3D:  // DEC A
                            ExecuteDEC8(Select_r(opCode, 2));
                            break;
                        case 0x0B:  // DEC BC
                        case 0x1B:  // DEC DE
                        case 0x2B:  // DEC HL
                        case 0x3B:  // DEC SP
                            OperationPackOpcodeFetchExtend2.SetOPCode(opCode);
                            result = OperationPackOpcodeFetchExtend2;
                            break;
                        case 0x07:  // RLCA
                            ExecuteRLCA();
                            break;
                        case 0x17:  // RLA
                            ExecuteRLA();
                            break;
                        case 0x27:  // DAA
                            ExecuteDAA();
                            break;
                        case 0x37:  // SCF
                            ExecuteSCF();
                            break;
                        case 0x0F:  // RRCA
                            ExecuteRRCA();
                            break;
                        case 0x1F:  // RRA
                            ExecuteRRA();
                            break;
                        case 0x2F:  // CPL
                            ExecuteCPL();
                            break;
                        case 0x3F:  // CCF
                            ExecuteCCF();
                            break;
                        case 0x08:  // EX AF,AF'
                            ExecuteEXAFAF_S();
                            break;
                        case 0xC9:  // RET
                            OperationPackReadMemory16.SetOPCode(opCode, RegisterEnum.SP);
                            result = OperationPackReadMemory16;
                            break;
                        case 0xC0:  // RET NZ
                        case 0xC8:  // RET Z
                        case 0xD0:  // RET NC
                        case 0xD8:  // RET C
                        case 0xE0:  // RET PO
                        case 0xE8:  // RET PE
                        case 0xF0:  // RET P
                        case 0xF8:  // RET M
                            OperationPackOpcodeFetchExtend1.SetOPCode(opCode);
                            result = OperationPackOpcodeFetchExtend1;
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
                            OperationPackReadMemory16.SetOPCode(opCode);
                            result = OperationPackReadMemory16;
                            break;
                        case 0xC4:  // CALL NZ,n'n
                        case 0xD4:  // CALL NC,n'n
                        case 0xE4:  // CALL PO,n'n
                        case 0xF4:  // CALL P,n'n
                        case 0xCC:  // CALL Z,n'n
                        case 0xDC:  // CALL C,n'n
                        case 0xEC:  // CALL PE,n'n
                        case 0xFC:  // CALL M,n'n
                            OperationPackReadMemory16.SetOPCode(opCode);
                            result = OperationPackReadMemory16;
                            break;
                        default:
                            break;
                    }

                    //OP1 = Bus.Data;
                    //ExecuteOperation();
                    return result;
                },
                [TimingCycleEnum.M1_T3_H] = () =>
                {
                    CPU.Bus.Address = (UInt16)(CPU.Register.R * 256);
                    CPU.Register.R = (byte)((CPU.Register.R + 1) & 0x7F);
                    CPU.MREQ = true;
                    CPU.RD = true;
                    CPU.M1 = true;
                    CPU.RFSH = false;

                    return default;
                },
                [TimingCycleEnum.M1_T3_L] = () => 
                {
                    return default;
                }
            };
        }

    }
}