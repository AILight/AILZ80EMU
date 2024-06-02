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
        public bool ClockState { get; private set; }
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
        private byte OP1 { get; set; }
        private byte OP2 { get; set; }
        private byte RD1 { get; set; }
        private byte RD2 { get; set; }
        private byte WD1 { get; set; }
        private byte WD2 { get; set; }

        //public InstructionSet InstructionSet { get; private set; }
        //public Clock Clock { get; private set; }
        //public InterruptController InterruptController { get; private set; }
        private int StepCounter { get; set; } = 0;

        public CPUZ80(Bus bus)
        {
            Register = new Register();
            IO = new IO(256, bus);
            Bus = bus;

            Reset();
            //InstructionSet = new InstructionSet();
            //Clock = new Clock();
            //InterruptController = new InterruptController();
        }

        public void Reset()
        {
            MachineCycle = MachineCycleEnum.None;
            MREQ = true;
            RD = true;
            WR = true;
            M1 = true;
            RFSH = true;

            Bus.MREQ = MREQ;
            Bus.RD = RD;
            Bus.WR = WR;
            Bus.M1 = M1;
            Bus.RFSH = RFSH;
        }

        public override void ExecuteClock(bool clockState)
        {
            base.ExecuteClock(clockState);

            // ステートを変更する
            if (MachineCycle == MachineCycleEnum.None && clockState)
            {
                MachineCycle = MachineCycleEnum.M1_T1_H;
            }
            else
            {
                // ステートを変更する
                if ((((int)MachineCycle & 1) == 1 && clockState) ||
                    (((int)MachineCycle & 1) == 0 && !clockState))
                {
                    return;
                }
                MachineCycle = MachineCycle + 1;
            }
 
            // 実際の処理を動作させる
            switch (MachineCycle)
            {
                case MachineCycleEnum.M1_T1_H:
                    Bus.Address = Register.PC;
                    Register.PC++;
                    M1 = false;
                    break;
                case MachineCycleEnum.M1_T1_L:
                    MREQ = false;
                    RD = false;
                    break;
                case MachineCycleEnum.M1_T2_H:
                    break;
                case MachineCycleEnum.M1_T2_L:
                    OP1 = Bus.Data;
                    ExecuteOperation();
                    break;
                case MachineCycleEnum.M1_T3_H:
                    Bus.Address = (UInt16)(Register.R * 256);
                    Register.R = (byte)((Register.R + 1) & 0x7F);
                    MREQ = true;
                    RD = true;
                    M1 = true;
                    RFSH = false;
                    break;
                case MachineCycleEnum.M1_T3_L:
                    MREQ = false;
                    break;
                case MachineCycleEnum.M1_T4_H:
                    break;
                case MachineCycleEnum.M1_T4_L:
                    MREQ = true;
                    break;
            }
            Bus.MREQ = MREQ;
            Bus.RD = RD;
            Bus.WR = WR;
            Bus.M1 = M1;
            Bus.RFSH = RFSH;
        }

        private void ExecuteOperation()
        {
            switch (OP1)
            {
                case 0x00: // NOP
                    break;
                case 0x01: // LD BC,n'n

                default:
                    break;
            }
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
