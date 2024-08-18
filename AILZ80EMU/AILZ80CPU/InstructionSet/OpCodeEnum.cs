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
        EXX,        // ExchangeX
        LDI,        // Load Increment
        LDIR,       // Load Increment and Repeat
        LDD,        // Load Decrement
        LDDR,       // Load Decrement and Repeat
        CPI,        // Compare and Increment
        CPIR,       // Compare and Increment and Repeat
        CPD,        // Compare and Decrement
        CPDR,       // Compare and Decrement and Repeat

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

        // Rotate and Shift Group
        RLCA,       // Rotate Left Circular Accumulator
        RLA,        // Rotate Left Accumulator
        RRCA,       // Rotate Right Circular Accumulator
        RRA,        // Rotate Right Accumulator
        RLC,        // Rotate Left Circular
        RL,         // Rotate Left
        RRC,        // Rotate Right Circular
        RR,         // Rotate Right
        SLA,        // Shift Left Arithmetic
        SRA,        // Shift Right Arithmetic
        RLD,        // Rotate Left Decimal Adjust
        RRD,        // Rotate Right Decimal Adjust

        // Bit Manipulation Group
        BIT,        // Test Bit
        SET,        // Set Bit

        // Interrupt and Restart Group
        RETI,       // Return from Interrupt
        RETN,       // Return from Non-Maskable Interrupt

        // Input/Output Group
        IN,         // Input from Port
        OUT,        // Output to Port
        OUTI,       // Output and Increment
        OTIR,       // Output, Increment and Repeat
        OUTD,       // Output and Decrement
        OTDR,       // Output, Decrement and Repeat

        // CPU Control Group
        EI,         // Enable Interrupts
        DI,         // Disable Interrupts

        // Control Group
        JP,         // Jump
        JR,         // Jump Relative
        CALL,       // Call
        RET,        // Return
        RST,        // Restart

    }
}
