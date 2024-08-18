using AILZ80LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AILZ80CPU.Cycles
{
    public class MachineCycleOpcodeFetch : MachineCycle
    {
        public MachineCycleOpcodeFetch(CPUZ80 cpu)
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

            TimingCycleActions = new Dictionary<TimingCycleEnum, Action>()
            {
                [TimingCycleEnum.M1_T1_H] = () =>
                {
                    cpu.Bus.Address = cpu.Register.PC;
                    cpu.Register.PC++;
                    cpu.M1 = false;
                },
                [TimingCycleEnum.M1_T1_L] = () =>
                {
                    cpu.MREQ = false;
                    cpu.RD = false;
                },
                [TimingCycleEnum.M1_T2_H] = () =>
                {
                },
                [TimingCycleEnum.M1_T2_L] = () =>
                {
                    /*
                    var opCode = CPU.Bus.Data;

                    var operand = Operand.Select(opCode);
                    operand.Execute(CPU);
                    return default;
                    */
                },
                [TimingCycleEnum.M1_T3_H] = () =>
                {
                    cpu.Bus.Address = (UInt16)(cpu.Register.R * 256);
                    cpu.Register.R = (byte)((cpu.Register.R + 1) & 0x7F);
                    cpu.MREQ = true;
                    cpu.RD = true;
                    cpu.M1 = true;
                    cpu.RFSH = false;
                },
                [TimingCycleEnum.M1_T3_L] = () => 
                {
                }
            };
        }

    }
}