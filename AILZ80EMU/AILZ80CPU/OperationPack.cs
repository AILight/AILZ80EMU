using AILZ80LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU
{
    public class OperationPack
    {
        private CPUZ80 CPU { get; set; }
        public TimingCycleEnum[]? TimingCycles { get; set; }
        public Dictionary<TimingCycleEnum, Action>? TimingCycleActions { get; set; }
        //public Dictionary<TimingCycleEnum, TimingCycleEnum>? NextTimingCycleDic { get; set; }
        //public TimingCycleEnum EndTimingCycle { get; set; }
        public Dictionary<byte, OperationPack>? OperationPackDictionary { get; set; }

        //public static TimingCycleEnum[] OpCodeFetchTimingCycles = {  }
        public OperationPack(CPUZ80 cpu)
        {
            CPU = cpu;
        }

        public OperationPack(CPUZ80 cpu, MachineCycleEnum machineCycle)
            : this(cpu)
        {
            if (machineCycle.HasFlag(MachineCycleEnum.OpcodeFetch))
            {
                OperationPackForOpecodeFetch();
            }
            if (machineCycle.HasFlag(MachineCycleEnum.MemoryRead))
            {

            }
            if (machineCycle.HasFlag(MachineCycleEnum.MemoryWrite))
            {

            }
        }

        private void OperationPackForOpecodeFetch() 
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
                    CPU.Bus.Address = CPU.Register.PC;
                    CPU.Register.PC++;
                    CPU.M1 = false;
                },
                [TimingCycleEnum.M1_T1_L] = () =>
                {
                    CPU.MREQ = false;
                    CPU.RD = false;
                },
                [TimingCycleEnum.M1_T2_H] = () =>
                {
                },
                [TimingCycleEnum.M1_T2_L] = () =>
                {
                    OP1 = Bus.Data;
                    ExecuteOperation();
                },
                [TimingCycleEnum.M1_T3_H] = () =>
                {
                    CPU.Bus.Address = (UInt16)(CPU.Register.R * 256);
                    CPU.Register.R = (byte)((CPU.Register.R + 1) & 0x7F);
                    CPU.MREQ = true;
                    CPU.RD = true;
                    CPU.M1 = true;
                    CPU.RFSH = false;
                },

            };

        /*
         *             switch (TimingCycle)
            {
                case TimingCycleEnum.M1_T1_H:
                    Bus.Address = Register.PC;
                    Register.PC++;
                    M1 = false;
                    break;
                case TimingCycleEnum.M1_T1_L:
                    MREQ = false;
                    RD = false;
                    break;
                case TimingCycleEnum.M1_T2_H:
                    break;
                case TimingCycleEnum.M1_T2_L:
                    OP1 = Bus.Data;
                    ExecuteOperation();
                    break;
                case TimingCycleEnum.M1_T3_H:
                    Bus.Address = (UInt16)(Register.R * 256);
                    Register.R = (byte)((Register.R + 1) & 0x7F);
                    MREQ = true;
                    RD = true;
                    M1 = true;
                    RFSH = false;
                    break;
                case TimingCycleEnum.M1_T3_L:
                    MREQ = false;
                    break;
                case TimingCycleEnum.M1_T4_H:
                    break;
                case TimingCycleEnum.M1_T4_L:
                    MREQ = true;
                    break;
            }
         */

    }
}