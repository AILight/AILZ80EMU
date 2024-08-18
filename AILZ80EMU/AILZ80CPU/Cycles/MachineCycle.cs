using AILZ80CPU.InstructionSet;
using AILZ80CPU.Operations;
using AILZ80LIB;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.Cycles
{
    public abstract class MachineCycle
    {
        
        public bool IsEnd => (TimingCycles?.Length ?? 0) <= ExecuteIndex;
        public OperationItem? NextOperationItem => IsEnd ? OperationItem?.NextOperationItem : default;

        protected TimingCycleEnum[]? TimingCycles { get; set; }
        protected Dictionary<TimingCycleEnum, Action>? TimingCycleActions { get; set; }

        private CPUZ80 CPU { get; set; }
        private int ExecuteIndex { get; set; } = 0;
        private OperationItem? OperationItem { get; set; }

        public MachineCycle(CPUZ80 cpu)
        {
            CPU = cpu;
        }

        public void Initialize(OperationItem operationItem)
        {
            ExecuteIndex = 0;
            OperationItem = operationItem;
        }

        public virtual void Execute()
        {
            CPU.TimingCycle = TimingCycles![ExecuteIndex++];
            TimingCycleActions![CPU.TimingCycle].Invoke();
            OperationItem?.Execute(CPU);
        }
    }
}