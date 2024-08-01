using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.InstructionSet
{
    public enum OpCodeEnum
    {
        // Data Transfer Group
        LD,         // Load
        PUSH,       // Push
        POP,        // Pop
        EX,         // Exchange
        EXX,         // ExchangeX
        LDI,        // Load Increment
        LDD,        // Load Decrement

        // Arithmetic Group
        ADD,        // Add
        ADC,        // Add with Carry
        SUB,        // Subtract
        SBC,        // Subtract with Carry
        AND,        // Logical AND
        OR,         // Logical OR
        XOR,        // Logical XOR
        CP,         // Compare
        INC,        // Increment
        DEC,        // Decrement

        // Control Group
        JP,         // Jump
        JR,         // Jump Relative
        CALL,       // Call
        RET,        // Return
        RST,        // Restart

    }
}
