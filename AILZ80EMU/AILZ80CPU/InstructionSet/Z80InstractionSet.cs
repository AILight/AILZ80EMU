using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILZ80CPU.InstructionSet
{
    public class Z80InstractionSet
    {
        public InstructionItem[] InstructionItems { get; set; }

        private Dictionary<string, string> RegisterOperandDic { get; set; } = new Dictionary<string, string>
        {
            ["B"] = "000",
            ["C"] = "001",
            ["D"] = "010",
            ["E"] = "011",
            ["H"] = "100",
            ["L"] = "101",
            ["A"] = "111",
        };

        public Z80InstractionSet() 
        {
            var instructionItems = new[]
            {
                //new InstructionItem("r ← r'", OpCodeEnum.LD, "r, r'", new []{ "01rrrrrr'" }, new[] { MachineCycleEnum.OpcodeFetch }),
                //new InstructionItem("r ← n", OpCodeEnum.LD, "r, n", new []{ "00rrr110","nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                //new InstructionItem("r ← (HL)", OpCodeEnum.LD, "r, (HL)", new []{ "01rrr110" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                //new InstructionItem("r ← (IX+d)", OpCodeEnum.LD, "r, (IX+d)", new []{ "11011101", "01rrr110", "dddddddd" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_5, MachineCycleEnum.MemoryRead }),
                //new InstructionItem("r ← (IY+d)", OpCodeEnum.LD, "r, (IY+d)", new []{ "11111101", "01rrr110", "dddddddd" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_5, MachineCycleEnum.MemoryRead }),
                //new InstructionItem("(HL) ← r", OpCodeEnum.LD, "(HL), r", new []{ "01110r" }, new[] { MachineCycleEnum.OpcodeFetch }),
                //new InstructionItem("(IX+d) ← r", OpCodeEnum.LD, "(IX+d), r", new []{ "11011101", "01110rrrr", "dddddddd" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_5, MachineCycleEnum.MemoryWrite }),
                //new InstructionItem("(IY+d) ← r", OpCodeEnum.LD, "(IY+d), r", new []{ "11111101", "01110rrrr", "dddddddd" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_5, MachineCycleEnum.MemoryWrite }),
                //new InstructionItem("(HL) ← n", OpCodeEnum.LD, "(HL), n", new []{ "00110110", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                //new InstructionItem("(IX+d) ← n", OpCodeEnum.LD, "(IX+d), n", new []{ "11111101", "00110110", "dddddddd", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_2, MachineCycleEnum.MemoryWrite }),
                //new InstructionItem("(IY+d) ← n", OpCodeEnum.LD, "(IY+d), n", new []{ "11011101", "00110110", "dddddddd", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_2, MachineCycleEnum.MemoryWrite }),
                //new InstructionItem("A ← (BC)", OpCodeEnum.LD, "A, (BC)", new []{ "00001010" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                //new InstructionItem("A ← (DE)", OpCodeEnum.LD, "A, (DE)", new []{ "00011010" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                //new InstructionItem("A ← (nn)", OpCodeEnum.LD, "A, (nn)", new []{ "00111010", "nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead }),
                //new InstructionItem("(BC) ← A", OpCodeEnum.LD, "(BC), A", new []{ "00000010" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryWrite }),
                //new InstructionItem("(DE) ← A", OpCodeEnum.LD, "(DE), A", new []{ "00010010" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryWrite }),
                //new InstructionItem("(nn) ← A", OpCodeEnum.LD, "(nn), A", new []{ "00010010", "nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryWrite }),
                //new InstructionItem("A ← I", OpCodeEnum.LD, "A, I", new []{ "11101101","01010111" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_1 }),
                //new InstructionItem("A ← R", OpCodeEnum.LD, "A, R", new []{ "11101101","01011111" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_1 }),
                //new InstructionItem("I ← A", OpCodeEnum.LD, "I, A", new []{ "11101101","01000111" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_1 }),
                //new InstructionItem("R ← A", OpCodeEnum.LD, "R, A", new []{ "11101101","01001111" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_1 }),
                //new InstructionItem("dd ← nn", OpCodeEnum.LD, "dd, nn", new []{ "00dd0001","nnnnnnnn","nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead }),
                //new InstructionItem("IX ← nn", OpCodeEnum.LD, "IX, nn", new []{ "11011101","00100001","nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead }),
                //new InstructionItem("IY ← nn", OpCodeEnum.LD, "IY, nn", new []{ "11111101","00100001","nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead }),
                //new InstructionItem("H ← (nn + 1), L ← (nn)", OpCodeEnum.LD, "HL, (nn)", new []{ "00101010", "nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead }),
                //new InstructionItem("ddh ← (nn + 1) ddl ← (nn)", OpCodeEnum.LD, "dd, (nn)", new []{ "11101101", "01dd1011", "nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead }),
                //new InstructionItem("IXh ← (nn + 1), IXI ← (nn)", OpCodeEnum.LD, "IX, (nn)", new []{ "11011101", "00101010", "nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead }),
                //new InstructionItem("IYh ← (nn + 1), IYI ← (nn)", OpCodeEnum.LD, "IY, (nn)", new []{ "11111101", "00101010", "nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead }),
                //new InstructionItem("(nn + 1) ← H, (nn) ← L", OpCodeEnum.LD, "(nn), HL", new []{ "00100010", "nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryWrite, MachineCycleEnum.MemoryWrite }),
                //new InstructionItem("(nn + 1) ← ddh, (nn) ← ddl", OpCodeEnum.LD, "(nn), dd", new []{ "11101101", "01dd0011", "nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryWrite, MachineCycleEnum.MemoryWrite }),
                //new InstructionItem("(nn + 1) ← IXh, (nn) ← IXI", OpCodeEnum.LD, "(nn), IX", new []{ "11011101", "00100010", "nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryWrite, MachineCycleEnum.MemoryWrite }),
                //new InstructionItem("(nn + 1) ← IYh, (nn) ← IYI", OpCodeEnum.LD, "(nn), IY", new []{ "11011101", "00100010", "nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryWrite, MachineCycleEnum.MemoryWrite }),
                //new InstructionItem("SP ← HL", OpCodeEnum.LD, "SP, HL", new []{ "11111001" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_2 }),
                //new InstructionItem("SP ← IX", OpCodeEnum.LD, "SP, IX", new []{ "11011101", "11111001" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_2 }),
                //new InstructionItem("SP ← IY", OpCodeEnum.LD, "SP, IY", new []{ "11111101", "11111001" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_2 }),
                //new InstructionItem("(SP – 2) ← qqL, (SP – 1) ← qqH", OpCodeEnum.PUSH, "qq", new []{ "11qq0101" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_1, MachineCycleEnum.MemoryWrite, MachineCycleEnum.MemoryWrite }),
                //new InstructionItem("(SP – 2) ← IXL, (SP – 1) ← IXH", OpCodeEnum.PUSH, "IX", new []{ "11011101", "11100101" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_1, MachineCycleEnum.MemoryWrite, MachineCycleEnum.MemoryWrite }),
                //new InstructionItem("(SP – 2) ← IYL, (SP – 1) ← IYH", OpCodeEnum.PUSH, "IY", new []{ "11111101", "11100101" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_1, MachineCycleEnum.MemoryWrite, MachineCycleEnum.MemoryWrite }),
                //new InstructionItem("qqH ← (SP+1), qqL ← (SP)", OpCodeEnum.POP, "qq", new []{ "11qq0001" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead }),
                //new InstructionItem("IXH ← (SP+1), IXL ← (SP)", OpCodeEnum.POP, "IX", new []{ "11011101", "11100001" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead }),
                //new InstructionItem("IYH ← (SP+1), IYL ← (SP)", OpCodeEnum.POP, "IY", new []{ "11111101", "11100001" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead }),
                //new InstructionItem("E ↔ HL", OpCodeEnum.EX, "DE, HL", new []{ "11101011" }, new[] { MachineCycleEnum.OpcodeFetch }),
                //new InstructionItem("(BC) ↔ (BC'), (DE) ↔ (DE'), (HL) ↔ (HL')", OpCodeEnum.EXX, "", new []{ "11011001" }, new[] { MachineCycleEnum.OpcodeFetch }),
                //new InstructionItem(" H ↔ (SP+1), L ↔ (SP)", OpCodeEnum.EX, "(SP), HL", new []{ "11100011" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_1, MachineCycleEnum.MemoryWrite, MachineCycleEnum.MemoryWrite, MachineCycleEnum.Process_2 }),
                //new InstructionItem(" IXH ↔ (SP+1), IXL ↔ (SP)", OpCodeEnum.EX, "(SP), IX", new []{ "11011101", "11100011" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_1, MachineCycleEnum.MemoryWrite, MachineCycleEnum.MemoryWrite, MachineCycleEnum.Process_2 }),
                new InstructionItem(" IYH ↔ (SP+1), IYL ↔ (SP)", OpCodeEnum.EX, "(SP), IY", new []{ "11111101", "11100011" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_1, MachineCycleEnum.MemoryWrite, MachineCycleEnum.MemoryWrite, MachineCycleEnum.Process_2 }),
            };

            InstructionItems = MakeInstructionItems(instructionItems);
        }

        public InstructionItem[] MakeInstructionItems(InstructionItem[] instructionItems)
        {
            var result = new List<InstructionItem>();

            foreach (var instructionItem in instructionItems)
            {
                var notFound = true;
                if (instructionItem.Operation.IndexOf("r'") != -1)
                {
                    notFound = false;
                    foreach (var registerOperand in RegisterOperandDic)
                    {
                        result.AddRange(MakeInstructionItems(new[] { instructionItem.Replace("r'", registerOperand.Key, registerOperand.Key, registerOperand.Value) }));
                    }
                }
                else if (instructionItem.Operation.IndexOf("r") != -1)
                {
                    notFound = false;
                    foreach (var registerOperand in RegisterOperandDic)
                    {
                        result.AddRange(MakeInstructionItems(new[] { instructionItem.Replace("r", registerOperand.Key, registerOperand.Key, registerOperand.Value) }));
                    }
                }
                if (notFound)
                {
                    result.Add(instructionItem);
                }
            }

            return result.ToArray();
        }
    }
}
