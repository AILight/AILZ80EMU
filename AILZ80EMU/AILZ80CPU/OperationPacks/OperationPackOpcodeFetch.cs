using AILZ80LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.OperationPacks
{
    public class OperationPackOpcodeFetch : OperationPack
    {
        private OperationPackWriteMemory8 OperationPackWriteMemory8 { get; set; } 
        private OperationPackReadMemory16 OperationPackReadMemory16 { get; set; }
        private OperationPackOpcodeFetchExtend2 OperationPackOpcodeFetchExtend2 { get; set; }

        public OperationPackOpcodeFetch(CPUZ80 cpu)
            : base(cpu)
        {
            OperationPackWriteMemory8 = new OperationPackWriteMemory8(cpu);
            OperationPackReadMemory16 = new OperationPackReadMemory16(cpu);
            OperationPackOpcodeFetchExtend2 = new OperationPackOpcodeFetchExtend2(cpu);

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
                        case 0x01:  // LD BC,n'n
                        case 0x11:  // LD DE,n'n
                        case 0x21:  // LD HL,n'n
                        case 0x31:  // LD SP,n'n
                            OperationPackReadMemory16.SetOPCode(opCode);
                            result = OperationPackReadMemory16;
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
                        case 0x03:  // INC BC
                        case 0x13:  // INC DE
                        case 0x23:  // INC HL
                        case 0x33:  // INC SP
                            OperationPackOpcodeFetchExtend2.SetOPCode(opCode);
                            result = OperationPackOpcodeFetchExtend2;
                            break;
                        case 0x04:  // INC B
                            ExecuteINC8(RegisterEnum.B);
                            break;
                        case 0x0C:  // INC C
                            ExecuteINC8(RegisterEnum.C);
                            break;
                        case 0x14:  // INC D
                            ExecuteINC8(RegisterEnum.D);
                            break;
                        case 0x1C:  // INC E
                            ExecuteINC8(RegisterEnum.E);
                            break;
                        case 0x24:  // INC H
                            ExecuteINC8(RegisterEnum.H);
                            break;
                        case 0x2C:  // INC L
                            ExecuteINC8(RegisterEnum.L);
                            break;
                        case 0x3C:  // INC A
                            ExecuteINC8(RegisterEnum.A);
                            break;
                        case 0x05:  // DEC B
                            ExecuteDEC8(RegisterEnum.B);
                            break;
                        case 0x0D:  // DEC C
                            ExecuteDEC8(RegisterEnum.C);
                            break;
                        case 0x15:  // DEC D
                            ExecuteDEC8(RegisterEnum.D);
                            break;
                        case 0x1D:  // DEC E
                            ExecuteDEC8(RegisterEnum.E);
                            break;
                        case 0x25:  // DEC H
                            ExecuteDEC8(RegisterEnum.H);
                            break;
                        case 0x2D:  // DEC L
                            ExecuteDEC8(RegisterEnum.L);
                            break;
                        case 0x3D:  // DEC A
                            ExecuteDEC8(RegisterEnum.A);
                            break;
                        case 0x06:  // LD B,n
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