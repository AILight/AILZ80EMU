using AILZ80LIB;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AILZ80CPU
{
    public class CPUZ80 : Device, IAddressBus, IDataBus
    {
        public Register Register { get; private set; }
        public IO IO { get; private set; }

        public MachineCycleEnum MachineCycle { get; private set; }
        /// <summary>
        /// メモリリクエスト
        /// </summary>
        public bool MREQ { get; private set; }
        /// <summary>
        /// IOリクエスト
        /// </summary>
        public bool IORQ { get; private set; }　
        /// <summary>
        /// リード
        /// </summary>
        public bool RD { get; private set; }
        /// <summary>
        /// ライト
        /// </summary>
        public bool WR { get; private set; }
        /// <summary>
        /// マスカブル割り込み
        /// </summary>
        public bool INT { get; private set; }
        /// <summary>
        /// ノンマスカブル割り込み
        /// </summary>
        public bool NMI { get; private set; }
        /// <summary>
        /// HALT
        /// </summary>
        public bool HALT { get; private set; }
        public bool RFSH { get; private set; }
        public bool M1 { get; private set; }
        public bool RESET { get; private set; }
        public bool BUSRQ { get; private set; }
        public bool WAIT { get; private set; }
        public bool BUSACK { get; private set; }

        public ushort Address { get; set; }
        public byte Data { get; set; }

        private Bus Bus { get; set; }

        //public InstructionSet InstructionSet { get; private set; }
        //public Clock Clock { get; private set; }
        //public InterruptController InterruptController { get; private set; }
        private int StepCounter { get; set; } = 0;

        public CPUZ80(Bus bus)
        {
            Register = new Register();
            IO = new IO(256, bus);
            Bus = bus;

            MachineCycle = MachineCycleEnum.M1_T1;
            //InstructionSet = new InstructionSet();
            //Clock = new Clock();
            //InterruptController = new InterruptController();
        }

        public override void ExecuteClock()
        {
            base.ExecuteClock();
            // クロックで実行する

            switch (MachineCycle)
            {
                case MachineCycleEnum.M1_T1:
                    Bus.Address = Register.PC;

                    MREQ = false;
                    RD = false;
                    M1 = false;
                    RFSH = false;
                    break;
                case MachineCycleEnum.M1_T2:
                    break;
                case MachineCycleEnum.M1_T3:
                    break;
                case MachineCycleEnum.M1_T4:
                    break;
                case MachineCycleEnum.M2_T1:
                    break;
                case MachineCycleEnum.M2_T2:
                    break;
                case MachineCycleEnum.M2_T3:
                    break;
                default:
                    break;
            }
            Bus.MREQ = MREQ;
            Bus.RD = RD;
            Bus.M1 = M1;
            Bus.RFSH = RFSH;
        }

        /*
        public void ExecuteNextInstruction()
        {
            var opcode = FetchOpcode();
            InstructionSet.Execute(opcode, this);
        }

        private byte FetchOpcode()
        {
            var pc = Registers.PC;
            var opcode = Memory.ReadByte(pc);
            Registers.PC++;
            return opcode;
        }
        */
    }
}
