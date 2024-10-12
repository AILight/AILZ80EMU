using AILZ80CPU.Extensions;
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
        private Action<CPUZ80>? ExecuterForRead1 { get; set; }
        private Action<CPUZ80>? ExecuterForRead2 { get; set; }
        private Action<CPUZ80>? ExecuterForRead3 { get; set; }
        private Action<CPUZ80>? ExecuterForWrite { get; set; }

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForFetch = new Dictionary<string, Action<CPUZ80>>()
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

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForFetch_Regex = new Dictionary<string, Action<CPUZ80>>()
        {
            { @",\sn", (cpu) => { cpu.Bus.Address = cpu.Register.PC; cpu.Register.PC++; } },
            { @",\s\(nn\)", (cpu) => { cpu.Bus.Address = cpu.Register.PC; cpu.Register.PC++; } },

            { @",\sA", (cpu) => { cpu.Register.Internal_8bit_Register = cpu.Register.A; } },
            { @",\sB", (cpu) => { cpu.Register.Internal_8bit_Register = cpu.Register.B; } },
            { @",\sC", (cpu) => { cpu.Register.Internal_8bit_Register = cpu.Register.C; } },
            { @",\sD", (cpu) => { cpu.Register.Internal_8bit_Register = cpu.Register.D; } },
            { @",\sE", (cpu) => { cpu.Register.Internal_8bit_Register = cpu.Register.E; } },
            { @",\sH", (cpu) => { cpu.Register.Internal_8bit_Register = cpu.Register.H; } },
            { @",\sL", (cpu) => { cpu.Register.Internal_8bit_Register = cpu.Register.L; } },

            { @",\s\(HL\)", (cpu) => { cpu.Bus.Address = cpu.Register.HL; } },
            { @",\s\(BC\)", (cpu) => { cpu.Bus.Address = cpu.Register.BC; } },
            { @",\s\(DE\)", (cpu) => { cpu.Bus.Address = cpu.Register.DE; } },
        };

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForRead1_Regex = new Dictionary<string, Action<CPUZ80>>()
        {
            { @"^A,", (cpu) => { cpu.Register.A = cpu.Bus.Data; } },
            { @"^B,", (cpu) => { cpu.Register.B = cpu.Bus.Data; } },
            { @"^C,", (cpu) => { cpu.Register.C = cpu.Bus.Data; } },
            { @"^D,", (cpu) => { cpu.Register.D = cpu.Bus.Data; } },
            { @"^E,", (cpu) => { cpu.Register.E = cpu.Bus.Data; } },
            { @"^H,", (cpu) => { cpu.Register.H = cpu.Bus.Data; } },
            { @"^L,", (cpu) => { cpu.Register.L = cpu.Bus.Data; } },
            { @",\sn", (cpu) => { cpu.Register.Internal_8bit_Register = cpu.Bus.Data; } },
            { @",\s\(nn\)", (cpu) => { cpu.Register.Internal_8bit_Register = cpu.Bus.Data; cpu.Bus.Address = cpu.Register.PC; cpu.Register.PC++; } },
        };

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForRead2_Regex = new Dictionary<string, Action<CPUZ80>>()
        {
            { @",\s\(nn\)", (cpu) => { cpu.Bus.Address = (UInt16)(cpu.Register.Internal_8bit_Register * 256 + cpu.Bus.Data); } },
        };

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForRead3_Regex = new Dictionary<string, Action<CPUZ80>>()
        {
            { @"A,\s\(nn\)", (cpu) => { cpu.Register.A = cpu.Bus.Data; } },
            { @"B,\s\(nn\)", (cpu) => { cpu.Register.B = cpu.Bus.Data; } },
            { @"C,\s\(nn\)", (cpu) => { cpu.Register.C = cpu.Bus.Data; } },
            { @"D,\s\(nn\)", (cpu) => { cpu.Register.D = cpu.Bus.Data; } },
            { @"E,\s\(nn\)", (cpu) => { cpu.Register.E = cpu.Bus.Data; } },
            { @"H,\s\(nn\)", (cpu) => { cpu.Register.H = cpu.Bus.Data; } },
            { @"L,\s\(nn\)", (cpu) => { cpu.Register.L = cpu.Bus.Data; } },
        };

        private static Dictionary<string, Action<CPUZ80>> operandExecuterForWrite_Regex = new Dictionary<string, Action<CPUZ80>>()
        {
            { @"^\(HL\),", (cpu) => { cpu.Bus.Data = cpu.Register.Internal_8bit_Register; cpu.Register.Internal_Memory_Pointer = cpu.Register.HL; } },
            { @"^\(DE\),", (cpu) => { cpu.Bus.Data = cpu.Register.Internal_8bit_Register; cpu.Register.Internal_Memory_Pointer = cpu.Register.DE; } },
            { @"^\(BC\),", (cpu) => { cpu.Bus.Data = cpu.Register.Internal_8bit_Register; cpu.Register.Internal_Memory_Pointer = cpu.Register.BC; } },
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

            if (operandExecuterForFetch.TryGetValue(instructionItem.Operand, out executer))
            {
                operationItem.ExecuterForFetch = executer;
            }
            else if (operandExecuterForFetch_Regex.TryGetValueRegex(instructionItem.Operand, out executer))
            {
                operationItem.ExecuterForFetch = executer;
            }
            
            if (operandExecuterForRead1_Regex.TryGetValue(instructionItem.Operand, out executer))
            {
                operationItem.ExecuterForRead1 = executer;
            }
            if (operandExecuterForRead2_Regex.TryGetValue(instructionItem.Operand, out executer))
            {
                operationItem.ExecuterForRead2 = executer;
            }
            if (operandExecuterForRead3_Regex.TryGetValue(instructionItem.Operand, out executer))
            {
                operationItem.ExecuterForRead3 = executer;
            }
            if (operandExecuterForWrite_Regex.TryGetValue(instructionItem.Operand, out executer))
            {
                operationItem.ExecuterForWrite = executer;
            }

            if (operationItem.ExecuterForFetch != default ||
                operationItem.ExecuterForRead1 != default ||
                operationItem.ExecuterForRead2 != default ||
                operationItem.ExecuterForRead3 != default ||
                operationItem.ExecuterForWrite != default)
            {
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
                if (machineCycleIndex == 1)
                {
                    ExecuterForRead1?.Invoke(cpu);
                }
                else if (machineCycleIndex == 2)
                {
                    ExecuterForRead2?.Invoke(cpu);
                }
                else if (machineCycleIndex == 3)
                {
                    ExecuterForRead3?.Invoke(cpu);
                }
            }
            else if (cpu.TimingCycle == TimingCycleEnum.W1_T3_H)
            {
                ExecuterForWrite?.Invoke(cpu);
            }

            return this;
        }
    }
}
