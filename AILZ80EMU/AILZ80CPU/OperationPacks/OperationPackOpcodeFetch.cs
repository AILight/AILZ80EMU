using AILZ80LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.OperationPacks
{
    public abstract class OperationPackOpcodeFetch : OperationPack
    {
        public OperationPackOpcodeFetch()
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

            TimingCycleActions = new Dictionary<TimingCycleEnum, Action<CPUZ80>>()
            {
                [TimingCycleEnum.M1_T1_H] = (CPUZ80 cpu) =>
                {
                    cpu.Bus.Address = cpu.Register.PC;
                    cpu.Register.PC++;
                    cpu.M1 = false;
                },
                [TimingCycleEnum.M1_T1_L] = (CPUZ80 cpu) =>
                {
                    cpu.MREQ = false;
                    cpu.RD = false;
                },
                [TimingCycleEnum.M1_T2_H] = (CPUZ80 cpu) =>
                {
                },
                [TimingCycleEnum.M1_T2_L] = (CPUZ80 cpu) =>
                {
                    var opCode = cpu.Bus.Data;
                    //OP1 = Bus.Data;
                    //ExecuteOperation();
                },
                [TimingCycleEnum.M1_T3_H] = (CPUZ80 cpu) =>
                {
                    cpu.Bus.Address = (UInt16)(cpu.Register.R * 256);
                    cpu.Register.R = (byte)((cpu.Register.R + 1) & 0x7F);
                    cpu.MREQ = true;
                    cpu.RD = true;
                    cpu.M1 = true;
                    cpu.RFSH = false;
                },

            };
        }
    }
}