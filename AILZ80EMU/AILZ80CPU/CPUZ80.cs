using AILZ80CPU.InstructionSet;
using AILZ80CPU.Cycles;
using AILZ80LIB;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AILZ80CPU.Operations;

namespace AILZ80CPU
{
    public class CPUZ80 : Device, IAddressBus, IDataBus
    {
        public Register Register { get; private set; }
        public IO IO { get; private set; }

        public TimingCycleEnum TimingCycle { get; internal set; }
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

        private MachineCycle? MachineCycle { get; set; } = default;
        private MachineCycleOpcodeFetch MachineCycleOpcodeFetch { get; set; }
        private MachineCycleProcess1 MachineCycleOpcodeFetchExtend1 { get; set; }
        private MachineCycleProcess2 MachineCycleOpcodeFetchExtend2 { get; set; }
        private MachineCycleProcess5 MachineCycleOpcodeFetchExtend5 { get; set; }
        private MachineCycleMemoryRead MachineCycleReadMemory { get; set; }
        private MachineCycleMemoryWrite MachineCycleWriteMemory { get; set; }

        private OperationItem BaseOperationItem { get; set; }

        public CPUZ80(Bus bus)
        {
            // 命令を処理する
            var z80InstractionSet = new Z80InstractionSet();
            BaseOperationItem = z80InstractionSet.CreateOperationItem();

            MachineCycleOpcodeFetch = new MachineCycleOpcodeFetch(this);
            MachineCycleOpcodeFetchExtend1 = new MachineCycleProcess1(this);
            MachineCycleOpcodeFetchExtend2 = new MachineCycleProcess2(this);
            MachineCycleOpcodeFetchExtend5 = new MachineCycleProcess5(this);
            MachineCycleReadMemory = new MachineCycleMemoryRead(this);
            MachineCycleWriteMemory = new MachineCycleMemoryWrite(this);

            Register = new Register();
            IO = new IO(256, bus);
            Bus = bus;

            Reset();
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

            MachineCycle = MachineCycleOpcodeFetch;
            MachineCycle.Initialize(BaseOperationItem);
        }

        public override void ExecuteClock(bool clockState)
        {
            base.ExecuteClock(clockState);


            MachineCycle!.Execute();

            if (MachineCycle.IsEnd)
            {
                if (MachineCycle.NextOperationItem == default)
                {
                    // 最初に戻る
                    MachineCycle = MachineCycleOpcodeFetch;
                    MachineCycle.Initialize(BaseOperationItem);
                }
                else
                {
                    switch (MachineCycle.NextOperationItem.MachineCycle)
                    {
                        case MachineCycleEnum.OpcodeFetch:
                            MachineCycle = MachineCycleOpcodeFetch;
                            break;
                        case MachineCycleEnum.Process_1:
                            MachineCycle = MachineCycleOpcodeFetchExtend1;
                            break;
                        case MachineCycleEnum.Process_2:
                            MachineCycle = MachineCycleOpcodeFetchExtend2;
                            break;
                        case MachineCycleEnum.Process_5:
                            MachineCycle = MachineCycleOpcodeFetchExtend5;
                            break;
                        case MachineCycleEnum.MemoryRead:
                            MachineCycle = MachineCycleReadMemory;
                            break;
                        case MachineCycleEnum.MemoryWrite:
                            MachineCycle = MachineCycleWriteMemory;
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                    MachineCycle.Initialize(MachineCycle.NextOperationItem!);
                }
            }
        }
    }
}
