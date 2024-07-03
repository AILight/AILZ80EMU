using AILZ80CPU.OperationPacks;
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

        public TimingCycleEnum TimingCycle { get; private set; }
        public bool ClockState { get; private set; }
        /// <summary>
        /// メモリリクエスト
        /// </summary>
        public bool MREQ { get; internal set; }
        /// <summary>
        /// IOリクエスト
        /// </summary>
        public bool IORQ { get; internal set; }　
        /// <summary>
        /// リード
        /// </summary>
        public bool RD { get; internal set; }
        /// <summary>
        /// ライト
        /// </summary>
        public bool WR { get; internal set; }
        /// <summary>
        /// マスカブル割り込み
        /// </summary>
        public bool INT { get; internal set; }
        /// <summary>
        /// ノンマスカブル割り込み
        /// </summary>
        public bool NMI { get; internal set; }
        /// <summary>
        /// HALT
        /// </summary>
        public bool HALT { get; internal set; }
        public bool RFSH { get; internal set; }
        public bool M1 { get; internal set; }
        public bool RESET { get; internal set; }
        public bool BUSRQ { get; internal set; }
        public bool WAIT { get; internal set; }
        public bool BUSACK { get; internal set; }

        public ushort Address { get; set; }
        public byte Data { get; set; }

        public Bus Bus { get; internal set; }

        public OperationPack RootOperationPack { get; private set; }

        /*
        private byte OP1 { get; set; }
        private byte OP2 { get; set; }
        private byte RD1 { get; set; }
        private byte RD2 { get; set; }
        private byte WD1 { get; set; }
        private byte WD2 { get; set; }
        */

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
            var a = new OperationPack()
            {
                OPs = new byte[] { 0x00 },
                EndTimingCycle = TimingCycleEnum.M1_T4_L
            };
            var b = new OperationPack()
            {
                OPs = new byte[] { 0x01 },
                Actions = new Dictionary<TimingCycleEnum, Action>()
                {
                    [TimingCycleEnum.]
                }
                EndTimingCycle = TimingCycleEnum.M1_T4_L
            };
            RootOperationPack = new OperationPack()
            {
                TimingCycles = 
                OperationPackDictionary = new Dictionary<byte, OperationPack>()
                {
                    { 0x00, a },
                }
            }


        }

        public void Reset()
        {
            TimingCycle = TimingCycleEnum.None;
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
            if (TimingCycle == TimingCycleEnum.None && clockState)
            {
                TimingCycle = TimingCycleEnum.M1_T1_H;
            }
            else
            {
                // ステートを変更する
                if ((((int)TimingCycle & 1) == 1 && clockState) ||
                    (((int)TimingCycle & 1) == 0 && !clockState))
                {
                    return;
                }
                TimingCycle = TimingCycle + 1;
            }
 
            // 実際の処理を動作させる
            switch (TimingCycle)
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
            Bus.MREQ = MREQ;
            Bus.RD = RD;
            Bus.WR = WR;
            Bus.M1 = M1;
            Bus.RFSH = RFSH;
        }

        // CB, DD, ED, FD


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
