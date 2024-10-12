using AILZ80CPU.Extensions;
using AILZ80CPU.InstructionSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.Operations
{
    public class OperationEXX : OperationItem
    {
        private Action<CPUZ80>? ExecuterForFetch { get; set; }

        private OperationEXX(InstructionItem instructionItem)
            : base(instructionItem)
        {
        }

        public static new OperationEXX Create(InstructionItem instructionItem)
        {
            if (instructionItem.OpCode != OpCodeEnum.EXX)
            {
                return default!;
            }

            var operationItem = new OperationEXX(instructionItem);
            operationItem.ExecuterForFetch = (cpu) =>
            {
                cpu.Register.SwapMainAndShadowRegisters();
            };

            return operationItem!;
        }



        public override OperationItem Execute(CPUZ80 cpu, int machineCycleIndex)
        {
            if (cpu.TimingCycle == TimingCycleEnum.M1_T2_L)
            {
                ExecuterForFetch?.Invoke(cpu);
            }
            
            return this;
        }
    }
}
