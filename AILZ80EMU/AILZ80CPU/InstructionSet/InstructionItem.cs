using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.InstructionSet
{
    public class InstructionItem
    {
        public string Operation { get; set; } = string.Empty;
        public OpCodeEnum OpCode { get; set; }

        public string Operand { get; set; } = string.Empty;

        public string[] OperandPatterns { get; set; }

        public MachineCycleEnum[] MachineCycles { get; set; }

        public InstructionItem(string operation, OpCodeEnum opCode, string operand, string[] operandPatterns, MachineCycleEnum[] machineCycles) 
        {
            Operation = operation;
            OpCode = opCode;
            Operand = operand;
            OperandPatterns = operandPatterns;
            MachineCycles = machineCycles;
        }

        public InstructionItem Replace(string operationOldValue, string operationNewValue, string operandOldValue, string operandNewValue, string OperandPatternOldValue, string OperandPatternNewValue)
        {
            var instructionItem = new InstructionItem(
                Operation.Replace(operationOldValue, operationNewValue),
                OpCode,
                Operand.Replace(operandOldValue, operandNewValue),
                OperandPatterns.Select(m => m.Replace(OperandPatternOldValue, OperandPatternNewValue)).ToArray(),
                MachineCycles
                );
            
            return instructionItem;
        }
    }
}
