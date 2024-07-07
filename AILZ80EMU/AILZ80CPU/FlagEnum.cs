using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU
{
    [Flags]
    public enum FlagEnum : byte
    {
        Carry = 1 << 0,           // キャリーフラグ
        AddSubtract = 1 << 1,     // 加減算フラグ (Nフラグ)
        ParityOverflow = 1 << 2,  // パリティ/オーバーフローフラグ (P/Vフラグ)
        HalfCarry = 1 << 4,       // ハーフキャリーフラグ (Hフラグ)
        Zero = 1 << 6,            // ゼロフラグ (Zフラグ)
        Sign = 1 << 7             // サインフラグ (Sフラグ)
    }
}
