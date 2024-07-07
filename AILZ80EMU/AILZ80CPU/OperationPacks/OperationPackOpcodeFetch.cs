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
        public OperationPackOpcodeFetch(CPUZ80 cpu)
            : base(cpu)
        {
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
                            result = new OperationPackReadMemory16(CPU, opCode);
                            break;
                        case 0x02:  // LD (BC),A
                            result = new OperationPackWriteMemory8(CPU, opCode);
                            break;
                        case 0x03:  // INC BC
                            result = new OperationPackOpcodeFetchExtend2(CPU, opCode);
                            break;
                        case 0x04:  // INC B
                            ExecuteINC8(RegisterEnum.B);
                            break;
                        case 0x0C:  // INC C
                            ExecuteINC8(RegisterEnum.C);
                            break;
                        case 0x11:  // LD DE,n'n
                            result = new OperationPackReadMemory16(CPU, opCode);
                            break;
                        case 0x13:  // INC DE
                            result = new OperationPackOpcodeFetchExtend2(CPU, opCode);
                            break;
                        case 0x14:  // INC D
                            ExecuteINC8(RegisterEnum.D);
                            break;
                        case 0x1C:  // INC E
                            ExecuteINC8(RegisterEnum.E);
                            break;
                        case 0x21:  // LD HL,n'n
                            result = new OperationPackReadMemory16(CPU, opCode);
                            break;
                        case 0x23:  // INC HL
                            result = new OperationPackOpcodeFetchExtend2(CPU, opCode);
                            break;
                        case 0x24:  // INC H
                            ExecuteINC8(RegisterEnum.H);
                            break;
                        case 0x2C:  // INC L
                            ExecuteINC8(RegisterEnum.L);
                            break;
                        case 0x31:  // LD SP,n'n
                            result = new OperationPackReadMemory16(CPU, opCode);
                            break;
                        case 0x33:  // INC SP
                            result = new OperationPackOpcodeFetchExtend2(CPU, opCode);
                            break;
                        case 0x3C:  // INC A
                            ExecuteINC8(RegisterEnum.A);
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