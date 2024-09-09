using AILZ80CPU.InstructionSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.Operations
{
    public class OperationLD_8 : OperationItem
    {
        private Action<CPUZ80>? ExecuterForFetch { get; set; }
        private Action<CPUZ80>? ExecuterForRead { get; set; }

        private static Dictionary<string, Action<CPUZ80>> operandExecuterMapForR8R8 = new Dictionary<string, Action<CPUZ80>>()
        {
            { "A, A", (cpu) => cpu.Register.A = cpu.Register.A },
            { "A, B", (cpu) => cpu.Register.A = cpu.Register.B },
            { "A, C", (cpu) => cpu.Register.A = cpu.Register.C },
            { "A, D", (cpu) => cpu.Register.A = cpu.Register.D },
            { "A, E", (cpu) => cpu.Register.A = cpu.Register.E },
            { "A, H", (cpu) => cpu.Register.A = cpu.Register.H },
            { "A, L", (cpu) => cpu.Register.A = cpu.Register.L },
            { "B, A", (cpu) => cpu.Register.B = cpu.Register.A },
            { "B, B", (cpu) => cpu.Register.B = cpu.Register.B },
            { "B, C", (cpu) => cpu.Register.B = cpu.Register.C },
            { "B, D", (cpu) => cpu.Register.B = cpu.Register.D },
            { "B, E", (cpu) => cpu.Register.B = cpu.Register.E },
            { "B, H", (cpu) => cpu.Register.B = cpu.Register.H },
            { "B, L", (cpu) => cpu.Register.B = cpu.Register.L },
            { "C, A", (cpu) => cpu.Register.C = cpu.Register.A },
            { "C, B", (cpu) => cpu.Register.C = cpu.Register.B },
            { "C, C", (cpu) => cpu.Register.C = cpu.Register.C },
            { "C, D", (cpu) => cpu.Register.C = cpu.Register.D },
            { "C, E", (cpu) => cpu.Register.C = cpu.Register.E },
            { "C, H", (cpu) => cpu.Register.C = cpu.Register.H },
            { "C, L", (cpu) => cpu.Register.C = cpu.Register.L },
            { "D, A", (cpu) => cpu.Register.D = cpu.Register.A },
            { "D, B", (cpu) => cpu.Register.D = cpu.Register.B },
            { "D, C", (cpu) => cpu.Register.D = cpu.Register.C },
            { "D, D", (cpu) => cpu.Register.D = cpu.Register.D },
            { "D, E", (cpu) => cpu.Register.D = cpu.Register.E },
            { "D, H", (cpu) => cpu.Register.D = cpu.Register.H },
            { "D, L", (cpu) => cpu.Register.D = cpu.Register.L },
            { "E, A", (cpu) => cpu.Register.E = cpu.Register.A },
            { "E, B", (cpu) => cpu.Register.E = cpu.Register.B },
            { "E, C", (cpu) => cpu.Register.E = cpu.Register.C },
            { "E, D", (cpu) => cpu.Register.E = cpu.Register.D },
            { "E, E", (cpu) => cpu.Register.E = cpu.Register.E },
            { "E, H", (cpu) => cpu.Register.E = cpu.Register.H },
            { "E, L", (cpu) => cpu.Register.E = cpu.Register.L },
            { "H, A", (cpu) => cpu.Register.H = cpu.Register.A },
            { "H, B", (cpu) => cpu.Register.H = cpu.Register.B },
            { "H, C", (cpu) => cpu.Register.H = cpu.Register.C },
            { "H, D", (cpu) => cpu.Register.H = cpu.Register.D },
            { "H, E", (cpu) => cpu.Register.H = cpu.Register.E },
            { "H, H", (cpu) => cpu.Register.H = cpu.Register.H },
            { "H, L", (cpu) => cpu.Register.H = cpu.Register.L },
            { "L, A", (cpu) => cpu.Register.L = cpu.Register.A },
            { "L, B", (cpu) => cpu.Register.L = cpu.Register.B },
            { "L, C", (cpu) => cpu.Register.L = cpu.Register.C },
            { "L, D", (cpu) => cpu.Register.L = cpu.Register.D },
            { "L, E", (cpu) => cpu.Register.L = cpu.Register.E },
            { "L, H", (cpu) => cpu.Register.L = cpu.Register.H },
            { "L, L", (cpu) => cpu.Register.L = cpu.Register.L }
        };
        private static Dictionary<string, Action<CPUZ80>> operandExecuterForFetch_MapForR8N = new Dictionary<string, Action<CPUZ80>>()
        {
            { "A, n", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.PC; cpu.Register.PC++; } },
            { "B, n", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.PC; cpu.Register.PC++; } },
            { "C, n", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.PC; cpu.Register.PC++; } },
            { "D, n", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.PC; cpu.Register.PC++; } },
            { "E, n", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.PC; cpu.Register.PC++; } },
            { "H, n", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.PC; cpu.Register.PC++; } },
            { "L, n", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.PC; cpu.Register.PC++; } },
        };
        private static Dictionary<string, Action<CPUZ80>> operandExecuterForRead_MapForR8N = new Dictionary<string, Action<CPUZ80>>()
        {
            { "A, n", (cpu) => { cpu.Register.A = cpu.Bus.Data; } },
            { "B, n", (cpu) => { cpu.Register.B = cpu.Bus.Data; } },
            { "C, n", (cpu) => { cpu.Register.C = cpu.Bus.Data; } },
            { "D, n", (cpu) => { cpu.Register.D = cpu.Bus.Data; } },
            { "E, n", (cpu) => { cpu.Register.E = cpu.Bus.Data; } },
            { "H, n", (cpu) => { cpu.Register.H = cpu.Bus.Data; } },
            { "L, n", (cpu) => { cpu.Register.L = cpu.Bus.Data; } },
        };
        private static Dictionary<string, Action<CPUZ80>> operandExecuterForRead_MapForR8AdrHL = new Dictionary<string, Action<CPUZ80>>()
        {
            { "A, (HL)", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.HL; cpu.Register.A = cpu.Bus.Data; } },
            { "B, (HL)", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.HL; cpu.Register.B = cpu.Bus.Data; } },
            { "C, (HL)", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.HL; cpu.Register.C = cpu.Bus.Data; } },
            { "D, (HL)", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.HL; cpu.Register.D = cpu.Bus.Data; } },
            { "E, (HL)", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.HL; cpu.Register.E = cpu.Bus.Data; } },
            { "H, (HL)", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.HL; cpu.Register.H = cpu.Bus.Data; } },
            { "L, (HL)", (cpu) => { cpu.Register.Internal_Memory_Pointer = cpu.Register.HL; cpu.Register.L = cpu.Bus.Data; } },
        };

        private OperationLD_8(InstructionItem instructionItem)
            : base(instructionItem)
        {
        }

        public static OperationLD_8 Create(InstructionItem instructionItem)
        {
            if (instructionItem.OpCode != OpCodeEnum.LD)
            {
                return default;
            }
            var executer = default(Action<CPUZ80>);
            var operationItem = new OperationLD_8(instructionItem);

            if (operandExecuterMapForR8R8.TryGetValue(instructionItem.Operand, out executer))
            {
                operationItem.ExecuterForFetch = executer;
                return operationItem;
            }
            else if (operandExecuterForFetch_MapForR8N.TryGetValue(instructionItem.Operand, out executer))
            {
                operationItem.ExecuterForRead = executer;
                return operationItem;
            }
            else if (operandExecuterForRead_MapForR8AdrHL.TryGetValue(instructionItem.Operand, out executer))
            {
                operationItem.ExecuterForRead = executer;
                return operationItem;
            }

            return default;
        }



        public override OperationItem Execute(CPUZ80 cpu, int machineCycleIndex)
        {
            if (cpu.TimingCycle == TimingCycleEnum.M1_T2_L)
            {
                ExecuterForFetch?.Invoke(cpu);
            }
            else if (cpu.TimingCycle == TimingCycleEnum.R1_T3_H)
            {
                ExecuterForRead?.Invoke(cpu);
            }

            return this;
        }
    }
}
