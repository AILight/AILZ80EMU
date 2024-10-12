using AILZ80CPU.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AILZ80CPU.InstructionSet
{
    public class Z80InstractionSet
    {
        public InstructionItem[] InstructionItems { get; set; }

        private Dictionary<string, string> NumberOperandDic_b { get; set; } = new Dictionary<string, string>
        {
            ["0"] = "000",
            ["1"] = "001",
            ["2"] = "010",
            ["3"] = "011",
            ["4"] = "100",
            ["5"] = "101",
            ["6"] = "110",
            ["7"] = "111",
        };

        private Dictionary<string, string> NumberOperandDic_p { get; set; } = new Dictionary<string, string>
        {
            ["00h"] = "000",
            ["08h"] = "001",
            ["10h"] = "010",
            ["18h"] = "011",
            ["20h"] = "100",
            ["28h"] = "101",
            ["30h"] = "110",
            ["38h"] = "111",
        };

        private Dictionary<string, string> RegisterOperandDicFor8Bit { get; set; } = new Dictionary<string, string>
        {
            ["B"] = "000",
            ["C"] = "001",
            ["D"] = "010",
            ["E"] = "011",
            ["H"] = "100",
            ["L"] = "101",
            ["A"] = "111",
        };

        private Dictionary<string, string> RegisterOperandDicFor16Bit_dd { get; set; } = new Dictionary<string, string>
        {
            ["BC"] = "00",
            ["DE"] = "01",
            ["HL"] = "10",
            ["SP"] = "11",
        };

        private Dictionary<string, string> RegisterOperandDicFor16Bit_qq { get; set; } = new Dictionary<string, string>
        {
            ["BC"] = "00",
            ["DE"] = "01",
            ["HL"] = "10",
            ["AF"] = "11",
        };

        private Dictionary<string, string> RegisterOperandDicFor16Bit_ss { get; set; } = new Dictionary<string, string>
        {
            ["BC"] = "00",
            ["DE"] = "01",
            ["HL"] = "10",
            ["SP"] = "11",
        };


        public Z80InstractionSet()
        {
            var instructionItems = new[]
            {
                // LD
                new InstructionItem("r ← r'", OpCodeEnum.LD, "r, r'", new []{ "01rrrrrr'" }, new[] { MachineCycleEnum.OpcodeFetch }),
                new InstructionItem("r ← n", OpCodeEnum.LD, "r, n", new []{ "00rrr110","nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("r ← (HL)", OpCodeEnum.LD, "r, (HL)", new []{ "01rrr110" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("r ← (IX+d)", OpCodeEnum.LD, "r, (IX+d)", new []{ "11011101", "01rrr110", "dddddddd" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_5, MachineCycleEnum.MemoryRead }),
                new InstructionItem("r ← (IY+d)", OpCodeEnum.LD, "r, (IY+d)", new []{ "11111101", "01rrr110", "dddddddd" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_5, MachineCycleEnum.MemoryRead }),
                new InstructionItem("(HL) ← r", OpCodeEnum.LD, "(HL), r", new []{ "01110rrr" }, new[] { MachineCycleEnum.OpcodeFetch }),
                new InstructionItem("(IX+d) ← r", OpCodeEnum.LD, "(IX+d), r", new []{ "11011101", "01110rrr", "dddddddd" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_5, MachineCycleEnum.MemoryWrite }),
                new InstructionItem("(IY+d) ← r", OpCodeEnum.LD, "(IY+d), r", new []{ "11111101", "01110rrr", "dddddddd" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_5, MachineCycleEnum.MemoryWrite }),
                new InstructionItem("(HL) ← n", OpCodeEnum.LD, "(HL), n", new []{ "00110110", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("(IX+d) ← n", OpCodeEnum.LD, "(IX+d), n", new []{ "11111101", "00110110", "dddddddd", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_2, MachineCycleEnum.MemoryWrite }),
                new InstructionItem("(IY+d) ← n", OpCodeEnum.LD, "(IY+d), n", new []{ "11011101", "00110110", "dddddddd", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_2, MachineCycleEnum.MemoryWrite }),
                new InstructionItem("A ← (BC)", OpCodeEnum.LD, "A, (BC)", new []{ "00001010" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("A ← (DE)", OpCodeEnum.LD, "A, (DE)", new []{ "00011010" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("A ← (nn)", OpCodeEnum.LD, "A, (nn)", new []{ "00111010", "nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead }),
                new InstructionItem("(BC) ← A", OpCodeEnum.LD, "(BC), A", new []{ "00000010" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryWrite }),
                new InstructionItem("(DE) ← A", OpCodeEnum.LD, "(DE), A", new []{ "00010010" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryWrite }),
                new InstructionItem("(nn) ← A", OpCodeEnum.LD, "(nn), A", new []{ "00110010", "nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryWrite }),
                new InstructionItem("A ← I", OpCodeEnum.LD, "A, I", new []{ "11101101","01010111" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_1 }),
                new InstructionItem("A ← R", OpCodeEnum.LD, "A, R", new []{ "11101101","01011111" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_1 }),
                new InstructionItem("I ← A", OpCodeEnum.LD, "I, A", new []{ "11101101","01000111" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_1 }),
                new InstructionItem("R ← A", OpCodeEnum.LD, "R, A", new []{ "11101101","01001111" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_1 }),
                new InstructionItem("dd ← nn", OpCodeEnum.LD, "dd, nn", new []{ "00dd0001","nnnnnnnn","nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead }),
                new InstructionItem("IX ← nn", OpCodeEnum.LD, "IX, nn", new []{ "11011101","00100001","nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead }),
                new InstructionItem("IY ← nn", OpCodeEnum.LD, "IY, nn", new []{ "11111101","00100001","nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead }),
                new InstructionItem("H ← (nn + 1), L ← (nn)", OpCodeEnum.LD, "HL, (nn)", new []{ "00101010", "nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead }),
                new InstructionItem("ddh ← (nn + 1) ddl ← (nn)", OpCodeEnum.LD, "dd, (nn)", new []{ "11101101", "01dd1011", "nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead }),
                new InstructionItem("IXh ← (nn + 1), IXI ← (nn)", OpCodeEnum.LD, "IX, (nn)", new []{ "11011101", "00101010", "nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead }),
                new InstructionItem("IYh ← (nn + 1), IYI ← (nn)", OpCodeEnum.LD, "IY, (nn)", new []{ "11111101", "00101010", "nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead }),
                new InstructionItem("(nn + 1) ← H, (nn) ← L", OpCodeEnum.LD, "(nn), HL", new []{ "00100010", "nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryWrite, MachineCycleEnum.MemoryWrite }),
                new InstructionItem("(nn + 1) ← ddh, (nn) ← ddl", OpCodeEnum.LD, "(nn), dd", new []{ "11101101", "01dd0011", "nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryWrite, MachineCycleEnum.MemoryWrite }),
                new InstructionItem("(nn + 1) ← IXh, (nn) ← IXI", OpCodeEnum.LD, "(nn), IX", new []{ "11011101", "00100010", "nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryWrite, MachineCycleEnum.MemoryWrite }),
                new InstructionItem("(nn + 1) ← IYh, (nn) ← IYI", OpCodeEnum.LD, "(nn), IY", new []{ "11011101", "00100010", "nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryWrite, MachineCycleEnum.MemoryWrite }),
                new InstructionItem("SP ← HL", OpCodeEnum.LD, "SP, HL", new []{ "11111001" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_2 }),
                new InstructionItem("SP ← IX", OpCodeEnum.LD, "SP, IX", new []{ "11011101", "11111001" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_2 }),
                new InstructionItem("SP ← IY", OpCodeEnum.LD, "SP, IY", new []{ "11111101", "11111001" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_2 }),
                
                // PUSH
                new InstructionItem("(SP – 2) ← qqL, (SP – 1) ← qqH", OpCodeEnum.PUSH, "qq", new []{ "11qq0101" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_1, MachineCycleEnum.MemoryWrite, MachineCycleEnum.MemoryWrite }),
                new InstructionItem("(SP – 2) ← IXL, (SP – 1) ← IXH", OpCodeEnum.PUSH, "IX", new []{ "11011101", "11100101" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_1, MachineCycleEnum.MemoryWrite, MachineCycleEnum.MemoryWrite }),
                new InstructionItem("(SP – 2) ← IYL, (SP – 1) ← IYH", OpCodeEnum.PUSH, "IY", new []{ "11111101", "11100101" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_1, MachineCycleEnum.MemoryWrite, MachineCycleEnum.MemoryWrite }),
                
                // POP
                new InstructionItem("qqH ← (SP+1), qqL ← (SP)", OpCodeEnum.POP, "qq", new []{ "11qq0001" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead }),
                new InstructionItem("IXH ← (SP+1), IXL ← (SP)", OpCodeEnum.POP, "IX", new []{ "11011101", "11100001" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead }),
                new InstructionItem("IYH ← (SP+1), IYL ← (SP)", OpCodeEnum.POP, "IY", new []{ "11111101", "11100001" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead }),
                
                // EX
                new InstructionItem("DE ↔ HL", OpCodeEnum.EX, "DE, HL", new []{ "11101011" }, new[] { MachineCycleEnum.OpcodeFetch }),
                new InstructionItem("AF ↔ AF'", OpCodeEnum.EX, "AF, AF'", new []{ "00001000" }, new[] { MachineCycleEnum.OpcodeFetch }),
                new InstructionItem("H ↔ (SP+1), L ↔ (SP)", OpCodeEnum.EX, "(SP), HL", new []{ "11100011" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_1, MachineCycleEnum.MemoryWrite, MachineCycleEnum.MemoryWrite, MachineCycleEnum.Process_2 }),
                new InstructionItem("IXH ↔ (SP+1), IXL ↔ (SP)", OpCodeEnum.EX, "(SP), IX", new []{ "11011101", "11100011" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_1, MachineCycleEnum.MemoryWrite, MachineCycleEnum.MemoryWrite, MachineCycleEnum.Process_2 }),
                new InstructionItem("IYH ↔ (SP+1), IYL ↔ (SP)", OpCodeEnum.EX, "(SP), IY", new []{ "11111101", "11100011" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_1, MachineCycleEnum.MemoryWrite, MachineCycleEnum.MemoryWrite, MachineCycleEnum.Process_2 }),
                
                // EXX
                new InstructionItem("(BC) ↔ (BC'), (DE) ↔ (DE'), (HL) ↔ (HL')", OpCodeEnum.EXX, "", new []{ "11011001" }, new[] { MachineCycleEnum.OpcodeFetch }),
                
                // LDI
                new InstructionItem("(DE) ← (HL), DE ← DE + 1, HL ← HL + 1, BC ← BC – 1", OpCodeEnum.LDI, "", new []{ "11101101", "10100000" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryWrite, MachineCycleEnum.Process_2 }),
                // LDIR
                new InstructionItem("(DE) ← (HL), DE ← DE + 1, HL ← HL + 1, BC ← BC – 1; repeat until BC = 0 or Z", OpCodeEnum.LDIR, "", new []{ "11101101", "10100000" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryWrite, MachineCycleEnum.Process_2 }),
                // LDD
                new InstructionItem("(DE) ← (HL), DE ← DE - 1, HL ← HL - 1, BC ← BC - 1", OpCodeEnum.LDD, "", new []{ "11101101", "10101000" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryWrite, MachineCycleEnum.Process_2 }),
                // LDDR
                new InstructionItem("(DE) ← (HL), DE ← DE - 1, HL ← HL - 1, BC ← BC - 1; repeat until BC = 0 or Z", OpCodeEnum.LDDR, "", new []{ "11101101", "10111000" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryWrite, MachineCycleEnum.Process_2, MachineCycleEnum.Process_2 }),
                // CPI
                new InstructionItem("A - (HL), HL ← HL + 1, BC ← BC - 1", OpCodeEnum.CPI, "", new []{ "11101101", "10100001" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_2 }),
                // CPIR
                new InstructionItem("A - (HL), HL ← HL + 1, BC ← BC - 1; repeat until BC = 0 or Z", OpCodeEnum.CPIR, "", new []{ "11101101", "10110001" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_2, MachineCycleEnum.Process_2 }),
                // CPD
                new InstructionItem("A - (HL), HL ← HL - 1, BC ← BC - 1", OpCodeEnum.CPD, "", new []{ "11101101", "10101001" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_2 }),
                // CPDR
                new InstructionItem("A - (HL), HL ← HL - 1, BC ← BC - 1; repeat until BC = 0 or Z", OpCodeEnum.CPDR, "", new []{ "11101101", "10111001" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_2, MachineCycleEnum.Process_2 }),

                // INC命令
                new InstructionItem("r ← r + 1", OpCodeEnum.INC, "r", new []{ "00rrr100" }, new[] { MachineCycleEnum.OpcodeFetch }),
                new InstructionItem("(HL) ← (HL) + 1", OpCodeEnum.INC, "(HL)", new []{ "00110100" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_1, MachineCycleEnum.MemoryWrite }),
                new InstructionItem("(IX+d) ← (IX+d) + 1", OpCodeEnum.INC, "(IX+d)", new []{ "11011101", "00110100", "dddddddd" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_5, MachineCycleEnum.MemoryWrite }),
                new InstructionItem("(IY+d) ← (IY+d) + 1", OpCodeEnum.INC, "(IY+d)", new []{ "11111101", "00110100", "dddddddd" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_5, MachineCycleEnum.MemoryWrite }),
                new InstructionItem("ss ← ss + 1", OpCodeEnum.INC, "ss", new []{ "00ss0011" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_1 }),

                // DEC命令
                new InstructionItem("r ← r - 1", OpCodeEnum.DEC, "r", new []{ "00rrr101" }, new[] { MachineCycleEnum.OpcodeFetch }),
                new InstructionItem("(HL) ← (HL) - 1", OpCodeEnum.DEC, "(HL)", new []{ "00110101" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_1, MachineCycleEnum.MemoryWrite }),
                new InstructionItem("(IX+d) ← (IX+d) - 1", OpCodeEnum.DEC, "(IX+d)", new []{ "11011101", "00110101", "dddddddd" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_5, MachineCycleEnum.MemoryWrite }),
                new InstructionItem("(IY+d) ← (IY+d) - 1", OpCodeEnum.DEC, "(IY+d)", new []{ "11111101", "00110101", "dddddddd" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_5, MachineCycleEnum.MemoryWrite }),
                new InstructionItem("ss ← ss - 1", OpCodeEnum.DEC, "ss", new []{ "00ss1011" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_1 }),

                // ADD命令
                new InstructionItem("A ← A + r", OpCodeEnum.ADD, "A, r", new []{ "10000rrr" }, new[] { MachineCycleEnum.OpcodeFetch }),
                new InstructionItem("A ← A + n", OpCodeEnum.ADD, "A, n", new []{ "11000110", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("A ← A + (HL)", OpCodeEnum.ADD, "A, (HL)", new []{ "10000110" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("A ← A + (IX+d)", OpCodeEnum.ADD, "A, (IX+d)", new []{ "11011101", "10000110", "dddddddd" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("A ← A + (IY+d)", OpCodeEnum.ADD, "A, (IY+d)", new []{ "11111101", "10000110", "dddddddd" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),

                // SUB命令
                new InstructionItem("A ← A - r", OpCodeEnum.SUB, "A, r", new []{ "10010rrr" }, new[] { MachineCycleEnum.OpcodeFetch }),
                new InstructionItem("A ← A - n", OpCodeEnum.SUB, "A, n", new []{ "11010110", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("A ← A - (HL)", OpCodeEnum.SUB, "A, (HL)", new []{ "10010110" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("A ← A - (IX+d)", OpCodeEnum.SUB, "A, (IX+d)", new []{ "11011101", "10010110", "dddddddd" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("A ← A - (IY+d)", OpCodeEnum.SUB, "A, (IY+d)", new []{ "11111101", "10010110", "dddddddd" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),

                // AND命令
                new InstructionItem("A ← A & r", OpCodeEnum.AND, "A, r", new []{ "10100rrr" }, new[] { MachineCycleEnum.OpcodeFetch }),
                new InstructionItem("A ← A & n", OpCodeEnum.AND, "A, n", new []{ "11100110", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("A ← A & (HL)", OpCodeEnum.AND, "A, (HL)", new []{ "10100110" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("A ← A & (IX+d)", OpCodeEnum.AND, "A, (IX+d)", new []{ "11011101", "10100110", "dddddddd" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("A ← A & (IY+d)", OpCodeEnum.AND, "A, (IY+d)", new []{ "11111101", "10100110", "dddddddd" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),

                // OR命令
                new InstructionItem("A ← A | r", OpCodeEnum.OR, "A, r", new []{ "10110rrr" }, new[] { MachineCycleEnum.OpcodeFetch }),
                new InstructionItem("A ← A | n", OpCodeEnum.OR, "A, n", new []{ "11110110", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("A ← A | (HL)", OpCodeEnum.OR, "A, (HL)", new []{ "10110110" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("A ← A | (IX+d)", OpCodeEnum.OR, "A, (IX+d)", new []{ "11011101", "10110110", "dddddddd" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("A ← A | (IY+d)", OpCodeEnum.OR, "A, (IY+d)", new []{ "11111101", "10110110", "dddddddd" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),

                // XOR命令
                new InstructionItem("A ← A ^ r", OpCodeEnum.XOR, "A, r", new []{ "10101rrr" }, new[] { MachineCycleEnum.OpcodeFetch }),
                new InstructionItem("A ← A ^ n", OpCodeEnum.XOR, "A, n", new []{ "11101110", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("A ← A ^ (HL)", OpCodeEnum.XOR, "A, (HL)", new []{ "10101110" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("A ← A ^ (IX+d)", OpCodeEnum.XOR, "A, (IX+d)", new []{ "11011101", "10101110", "dddddddd" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("A ← A ^ (IY+d)", OpCodeEnum.XOR, "A, (IY+d)", new []{ "11111101", "10101110", "dddddddd" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),

                // CP命令
                new InstructionItem("A - r", OpCodeEnum.CP, "A, r", new []{ "10111rrr" }, new[] { MachineCycleEnum.OpcodeFetch }),
                new InstructionItem("A - n", OpCodeEnum.CP, "A, n", new []{ "11111110", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("A - (HL)", OpCodeEnum.CP, "A, (HL)", new []{ "10111110" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("A - (IX+d)", OpCodeEnum.CP, "A, (IX+d)", new []{ "11011101", "10111110", "dddddddd" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("A - (IY+d)", OpCodeEnum.CP, "A, (IY+d)", new []{ "11111101", "10111110", "dddddddd" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),

                // JP命令
                new InstructionItem("PC ← nn", OpCodeEnum.JP, "nn", new []{ "11000011", "nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead }),
                new InstructionItem("PC ← HL", OpCodeEnum.JP, "(HL)", new []{ "11101001" }, new[] { MachineCycleEnum.OpcodeFetch }),
                new InstructionItem("PC ← IX", OpCodeEnum.JP, "(IX)", new []{ "11011101", "11101001" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch }),
                new InstructionItem("PC ← IY", OpCodeEnum.JP, "(IY)", new []{ "11111101", "11101001" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch }),

                // JR命令
                new InstructionItem("PC ← PC + e", OpCodeEnum.JR, "e", new []{ "00011000", "eeeeeeee" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),

                // CALL命令
                new InstructionItem("SP ← SP - 2, (SP) ← PC, PC ← nn", OpCodeEnum.CALL, "nn", new []{ "11001101", "nnnnnnnn", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_2, MachineCycleEnum.MemoryWrite, MachineCycleEnum.MemoryWrite }),

                // RET命令
                new InstructionItem("PC ← (SP), SP ← SP + 2", OpCodeEnum.RET, "", new []{ "11001001" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_2 }),

                // 新しい命令
                new InstructionItem("A ← A << 1 (bit 7 to carry)", OpCodeEnum.RLCA, "", new []{ "00000111" }, new[] { MachineCycleEnum.OpcodeFetch }),
                new InstructionItem("A ← A << 1 (bit 0 from carry)", OpCodeEnum.RLA, "", new []{ "00010111" }, new[] { MachineCycleEnum.OpcodeFetch }),
                new InstructionItem("A ← A >> 1 (bit 0 to carry)", OpCodeEnum.RRCA, "", new []{ "00001111" }, new[] { MachineCycleEnum.OpcodeFetch }),
                new InstructionItem("A ← A >> 1 (bit 7 from carry)", OpCodeEnum.RRA, "", new []{ "00011111" }, new[] { MachineCycleEnum.OpcodeFetch }),

                // Rotate instructions
                new InstructionItem("r ← r << 1 (bit 7 to carry)", OpCodeEnum.RLC, "r", new []{ "11001011", "00000rrr" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch }),
                new InstructionItem("(HL) ← (HL) << 1 (bit 7 to carry)", OpCodeEnum.RLC, "(HL)", new []{ "11001011", "00000110" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_1, MachineCycleEnum.MemoryWrite }),
                new InstructionItem("r ← r << 1 (bit 0 from carry)", OpCodeEnum.RL, "r", new []{ "11001011", "00010rrr" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch }),
                new InstructionItem("(HL) ← (HL) << 1 (bit 0 from carry)", OpCodeEnum.RL, "(HL)", new []{ "11001011", "00010110" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_1, MachineCycleEnum.MemoryWrite }),
                new InstructionItem("r ← r >> 1 (bit 0 to carry)", OpCodeEnum.RRC, "r", new []{ "11001011", "00001rrr" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch }),
                new InstructionItem("(HL) ← (HL) >> 1 (bit 0 to carry)", OpCodeEnum.RRC, "(HL)", new []{ "11001011", "00001110" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_1, MachineCycleEnum.MemoryWrite }),
                new InstructionItem("r ← r >> 1 (bit 7 from carry)", OpCodeEnum.RR, "r", new []{ "11001011", "00011rrr" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch }),
                new InstructionItem("(HL) ← (HL) >> 1 (bit 7 from carry)", OpCodeEnum.RR, "(HL)", new []{ "11001011", "00011110" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_1, MachineCycleEnum.MemoryWrite }),
                new InstructionItem("(HL) ← (HL) << 1 (bit 7 to carry, bit 0 set to 0)", OpCodeEnum.SLA, "(HL)", new []{ "11001011", "00100110" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_1, MachineCycleEnum.MemoryWrite }),
                new InstructionItem("(HL) ← (HL) >> 1 (bit 0 to carry, bit 7 unchanged)", OpCodeEnum.SRA, "(HL)", new []{ "11001011", "00101110" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_1, MachineCycleEnum.MemoryWrite }),
    
                // Rotate Left Decimal Adjust
                new InstructionItem("RLD", OpCodeEnum.RLD, "", new []{ "11101101", "01101111" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_2, MachineCycleEnum.MemoryWrite }),

                // Rotate Right Decimal Adjust
                new InstructionItem("RRD", OpCodeEnum.RRD, "", new []{ "11101101", "01100111" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_2, MachineCycleEnum.MemoryWrite }),

                // Bit Test, Set and Reset Instructions
                new InstructionItem("BIT b, r", OpCodeEnum.BIT, "b, r", new []{ "11001011", "01bbbrrr" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch }),
                new InstructionItem("BIT b, (HL)", OpCodeEnum.BIT, "b, (HL)", new []{ "11001011", "01bbb110" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("SET b, r", OpCodeEnum.SET, "b, r", new []{ "11001011", "11bbbrrr" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch }),
                new InstructionItem("SET b, (HL)", OpCodeEnum.SET, "b, (HL)", new []{ "11001011", "11bbb110" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_1, MachineCycleEnum.MemoryWrite }),

                // Return from Interrupt Instructions
                new InstructionItem("Return from Interrupt", OpCodeEnum.RETI, "", new []{ "11101101", "01001011" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_1 }),
                new InstructionItem("Return from Non-Maskable Interrupt", OpCodeEnum.RETN, "", new []{ "11101101", "01000101" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_1 }),

                // Restart Instructions
                new InstructionItem("Restart", OpCodeEnum.RST, "p", new []{ "11ttt111" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_1, MachineCycleEnum.MemoryWrite, MachineCycleEnum.MemoryWrite }),

                // Input and Output Instructions
                new InstructionItem("A ← (n)", OpCodeEnum.IN, "A, (n)", new []{ "11011011", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.Process_1 }),
                new InstructionItem("(n) ← A", OpCodeEnum.OUT, "(n), A", new []{ "11010011", "nnnnnnnn" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryWrite, MachineCycleEnum.Process_1 }),
                new InstructionItem("A ← (C)", OpCodeEnum.IN, "A, (C)", new []{ "11110011" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_1 }),
                new InstructionItem("(C) ← A", OpCodeEnum.OUT, "(C), A", new []{ "11110001" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_1 }),

                // Block Input and Output Instructions
                new InstructionItem("(C) ← A, B ← B - 1", OpCodeEnum.OUTI, "", new []{ "11101101", "10110011" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryWrite, MachineCycleEnum.Process_1 }),
                new InstructionItem("(C) ← A, B ← B - 1; repeat until B = 0", OpCodeEnum.OTIR, "", new []{ "11101101", "10110111" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryWrite, MachineCycleEnum.Process_1 }),
                new InstructionItem("(C) ← A, B ← B - 1, HL ← HL - 1", OpCodeEnum.OUTD, "", new []{ "11101101", "10111011" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryWrite, MachineCycleEnum.Process_1 }),
                new InstructionItem("(C) ← A, B ← B - 1; repeat until B = 0, HL ← HL - 1", OpCodeEnum.OTDR, "", new []{ "11101101", "10111111" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead, MachineCycleEnum.MemoryWrite, MachineCycleEnum.Process_1 }),

                // Enable and Disable Interrupts Instructions
                new InstructionItem("Enable Interrupts", OpCodeEnum.EI, "", new []{ "11111011" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_1 }),
                new InstructionItem("Disable Interrupts", OpCodeEnum.DI, "", new []{ "11110011" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.Process_1 })

                
            };
            MakeInstructionItems(instructionItems);
        }

        // 階層構造の作成
        private static void SettingOperationItem(OperationFetch operationFetch, int level, InstructionItem[] instructionItems)
        {
            var singleOpecodeFetchInstructionItems = instructionItems.Where(m => m.MachineCycles.Count(n => n == MachineCycleEnum.OpcodeFetch) == level);
            foreach (var instructionItem in singleOpecodeFetchInstructionItems)
            {
                var operationItem = OperationItem.Create(instructionItem);
                if (operationItem != default)
                {
                    operationFetch.AddOperationItem(operationItem);
                }
                else
                {
                    throw new Exception($"Level:{level} OperationItem:{instructionItem.OpCode} {instructionItem.Operand}");
                }
            }
            var nextLevelOpecodeFetchInstructionItems = instructionItems.Where(m => m.MachineCycles.Count(n => n == MachineCycleEnum.OpcodeFetch) == (level + 1)).GroupBy(m => m.OperandPatterns[level - 1]);
            if (nextLevelOpecodeFetchInstructionItems.Any())
            {
                foreach (var item in nextLevelOpecodeFetchInstructionItems)
                {
                    var nextLevelOperationFetch = new OperationFetch(Convert.ToByte(item.Key, 2));
                    operationFetch.AddOperationItem(nextLevelOperationFetch);

                    SettingOperationItem(nextLevelOperationFetch, level + 1, item.ToArray());
                }
            }
        }

        public OperationItem CreateOperationItem()
        {
            // エラーチェック
            foreach (var item in InstructionItems)
            {
                // OpcodeFetchと命令のつながりを調査
                var opcodeFetchCount = 0;
                foreach (var machineCycle in item.MachineCycles)
                {
                    // 連続するOpcodeFetchをカウントする
                    if (machineCycle == MachineCycleEnum.OpcodeFetch)
                    {
                        opcodeFetchCount++;
                    }
                    else
                    {
                        break;
                    }
                }
                if (opcodeFetchCount != item.MachineCycles.Count(m => m == MachineCycleEnum.OpcodeFetch))
                {
                    throw new Exception($"OpcodeFetchの数に間違いがあります。Operand: {item.OpCode} {item.Operand}");
                }

                var operandPatternCount = 0;
                foreach (var operandPattern in item.OperandPatterns)
                {
                    if (Regex.IsMatch(operandPattern, @"^[01]+$"))
                    {
                        operandPatternCount++;
                    }
                    else
                    {
                        break;
                    }
                }
                if (opcodeFetchCount != operandPatternCount)
                {
                    throw new Exception($"OperandPatternsの数に間違いがあります。Operand: {item.OpCode} {item.Operand}");
                }
            }

            // データの積み込み
            var baseOperationItem = new OperationFetch();
            SettingOperationItem(baseOperationItem, 1, InstructionItems);

            return baseOperationItem;
        }

        public void MakeInstructionItems(InstructionItem[] instructionItems)
        {
            var result = new List<InstructionItem>();

            // 命令の展開
            foreach (var instructionItem in instructionItems)
            {
                result.AddRange(MakeInstructionItem(instructionItem));
            }

            InstructionItems = result.ToArray();
        }

        public InstructionItem[] MakeInstructionItem(InstructionItem instructionItem)
        {
            var result = new List<InstructionItem>();

            if (instructionItem.Operand.IndexOf("r'") != -1)
            {
                foreach (var registerOperand in RegisterOperandDicFor8Bit)
                {
                    var item = instructionItem.Replace("r'", registerOperand.Key, "r'", registerOperand.Key, "rrr'", registerOperand.Value);
                    result.AddRange(MakeInstructionItem(item));
                }
            }
            else if (instructionItem.Operand.IndexOf("r") != -1)
            {
                foreach (var registerOperand in RegisterOperandDicFor8Bit)
                {
                    var item = instructionItem.Replace("r", registerOperand.Key, "r", registerOperand.Key, "rrr", registerOperand.Value);
                    result.AddRange(MakeInstructionItem(item));
                }
            }
            else if (Regex.IsMatch(instructionItem.Operand, @"(?<!d)dd(?!d)"))
            {
                foreach (var registerOperand in RegisterOperandDicFor16Bit_dd)
                {
                    var item = instructionItem.Replace("dd", registerOperand.Key, "dd", registerOperand.Key, "dd", registerOperand.Value);
                    result.AddRange(MakeInstructionItem(item));
                }
            }
            else if (Regex.IsMatch(instructionItem.Operand, @"(?<!q)qq(?!q)"))
            {
                foreach (var registerOperand in RegisterOperandDicFor16Bit_qq)
                {
                    var item = instructionItem.Replace("qq", registerOperand.Key, "qq", registerOperand.Key, "qq", registerOperand.Value);
                    result.AddRange(MakeInstructionItem(item));
                }
            }
            else if (Regex.IsMatch(instructionItem.Operand, @"(?<!s)ss(?!s)"))
            {
                foreach (var registerOperand in RegisterOperandDicFor16Bit_ss)
                {
                    var item = instructionItem.Replace("ss", registerOperand.Key, "ss", registerOperand.Key, "ss", registerOperand.Value);
                    result.AddRange(MakeInstructionItem(item));
                }
            }
            else if (Regex.IsMatch(instructionItem.Operand, @"(?<!b)b(?!b)"))
            {
                foreach (var numberOperand in NumberOperandDic_b)
                {
                    var item = instructionItem.Replace("b", numberOperand.Key, "b", numberOperand.Key, "bbb", numberOperand.Value);
                    result.AddRange(MakeInstructionItem(item));
                }
            }
            else if (Regex.IsMatch(instructionItem.Operand, @"(?<!p)p(?!p)"))
            {
                foreach (var numberOperand in NumberOperandDic_p)
                {
                    var item = instructionItem.Replace("p", numberOperand.Key, "p", numberOperand.Key, "ttt", numberOperand.Value);
                    result.AddRange(MakeInstructionItem(item));
                }
            }
            else
            {
                return new[] { instructionItem };
            }

            return result.ToArray();
        }
    }
}
