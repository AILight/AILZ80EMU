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

        public OperationPack? ExecutingOperationPack { get; set; } = default;
        public Queue<OperationPack> OperationPacks { get; private set; } = new Queue<OperationPack>();
        private int StepCounter { get; set; } = 0;

        public CPUZ80(Bus bus)
        {
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
        }

        public override void ExecuteClock(bool clockState)
        {
            base.ExecuteClock(clockState);

            if (ExecutingOperationPack == default)
            {
                if (OperationPacks.Count == 0)
                {
                    ExecutingOperationPack = new OperationPackOpcodeFetch(this);
                }
                else
                {
                    ExecutingOperationPack = OperationPacks.Dequeue();
                }
            }

            ExecutingOperationPack.Execute();
        }

    }
}
