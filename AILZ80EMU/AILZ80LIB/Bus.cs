using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80LIB
{
    public class Bus : Device
    {
        public UInt16 Address { get; set; }
        public Byte Data { get; set; }

        /// <summary>
        /// メモリリクエスト
        /// </summary>
        public bool MREQ { get; set; }
        /// <summary>
        /// IOリクエスト
        /// </summary>
        public bool IORQ { get; set; }
        /// <summary>
        /// リード
        /// </summary>
        public bool RD { get; set; }
        /// <summary>
        /// ライト
        /// </summary>
        public bool WR { get; set; }
        /// <summary>
        /// マシンサイクル1
        /// </summary>
        public bool M1 { get; set; }
    }
}
