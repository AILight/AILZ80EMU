using AILZ80LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AILZ80CPU.OperationPacks
{
    public class OperationPackWriteMemory16 : OperationPack
    {
        private RegisterEnum WriteRegister { get; set; }
        public byte OPCode { get; set; }

        public OperationPackWriteMemory16(CPUZ80 cpu)
            : base(cpu)
        {
            TimingCycles = new TimingCycleEnum[] {
                                TimingCycleEnum.W1_T1_H,
                                TimingCycleEnum.W1_T1_L,
                                TimingCycleEnum.W1_T2_H,
                                TimingCycleEnum.W1_T2_L,
                                TimingCycleEnum.W1_T3_H,
                                TimingCycleEnum.W1_T3_L,
                                TimingCycleEnum.W2_T1_H,
                                TimingCycleEnum.W2_T1_L,
                                TimingCycleEnum.W2_T2_H,
                                TimingCycleEnum.W2_T2_L,
                                TimingCycleEnum.W2_T3_H,
                                TimingCycleEnum.W2_T3_L,
                            };

            TimingCycleActions = new Dictionary<TimingCycleEnum, Func<OperationPack?>>()
            {
                [TimingCycleEnum.W1_T1_H] = () =>
                {
                    switch (WriteRegister)
                    {
                        case RegisterEnum.PC:
                            CPU.Bus.Address = CPU.Register.PC;
                            CPU.Register.PC++;
                            break;
                        case RegisterEnum.DirectAddress:
                            CPU.Bus.Address = CPU.Register.DirectAdress;
                            break;
                        case RegisterEnum.SP:
                            CPU.Register.SP--;
                            CPU.Bus.Address = CPU.Register.SP;
                            break;
                        default:
                            break;
                    }

                    return default;
                },
                [TimingCycleEnum.W1_T1_L] = () =>
                {
                    CPU.MREQ = false;

                    switch (OPCode)
                    {
                        case 0xC4:  // CALL NZ,n'n
                        case 0xD4:  // CALL NC,n'n
                        case 0xE4:  // CALL PO,n'n
                        case 0xF4:  // CALL P,n'n
                        case 0xCC:  // CALL Z,n'n
                        case 0xDC:  // CALL C,n'n
                        case 0xEC:  // CALL PE,n'n
                        case 0xFC:  // CALL M,n'n
                            CPU.Bus.Data = CPU.Register.PC_H;
                            break;
                        case 0xC5:  // PUSH BC
                            CPU.Bus.Data = CPU.Register.B;
                            break;
                        case 0xD5:  // PUSH DE
                            CPU.Bus.Data = CPU.Register.D;
                            break;
                        case 0xE5:  // PUSH HL
                            CPU.Bus.Data = CPU.Register.H;
                            break;
                        case 0xF5:  // PUSH AF
                            CPU.Bus.Data = CPU.Register.A;
                            break;
                        default:
                            break;
                    }

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
                [TimingCycleEnum.W2_T1_H] = () =>
                {
                    switch (WriteRegister)
                    {
                        case RegisterEnum.PC:
                            CPU.Bus.Address = CPU.Register.PC;
                            CPU.Register.PC++;
                            break;
                        case RegisterEnum.DirectAddress:
                            CPU.Bus.Address = CPU.Register.DirectAdress;
                            break;
                        case RegisterEnum.SP:
                            CPU.Register.SP--;
                            CPU.Bus.Address = CPU.Register.SP;
                            break;
                        default:
                            break;
                    }

                    return default;
                },
                [TimingCycleEnum.W2_T1_L] = () =>
                {
                    CPU.MREQ = false;

                    switch (OPCode)
                    {
                        case 0xC4:  // CALL NZ,n'n
                        case 0xD4:  // CALL NC,n'n
                        case 0xE4:  // CALL PO,n'n
                        case 0xF4:  // CALL P,n'n
                        case 0xCC:  // CALL Z,n'n
                        case 0xDC:  // CALL C,n'n
                        case 0xEC:  // CALL PE,n'n
                        case 0xFC:  // CALL M,n'n
                            CPU.Bus.Data = CPU.Register.PC_L;
                            CPU.Register.PC = CPU.Register.DirectAdress;
                            break;
                        case 0xC5:  // PUSH BC
                            CPU.Bus.Data = CPU.Register.C;
                            break;
                        case 0xD5:  // PUSH DE
                            CPU.Bus.Data = CPU.Register.E;
                            break;
                        case 0xE5:  // PUSH HL
                            CPU.Bus.Data = CPU.Register.L;
                            break;
                        case 0xF5:  // PUSH AF
                            CPU.Bus.Data = CPU.Register.F;
                            break;
                        default:
                            break;
                    }

                    return default;
                },
                [TimingCycleEnum.W2_T2_H] = () =>
                {
                    return default;
                },
                [TimingCycleEnum.W2_T2_L] = () =>
                {
                    CPU.WR = false;

                    return default;
                },
                [TimingCycleEnum.W2_T3_H] = () =>
                {
                    return default;
                },
                [TimingCycleEnum.W2_T3_L] = () =>
                {
                    CPU.MREQ = true;
                    CPU.WR = true;

                    return default;
                },
            };
        }

        public void SetOPCode(byte opCode, RegisterEnum writeRegister = RegisterEnum.PC)
        {
            OPCode = opCode;
            WriteRegister = writeRegister;
            ExecuteIndex = 0;
        }
    }
}