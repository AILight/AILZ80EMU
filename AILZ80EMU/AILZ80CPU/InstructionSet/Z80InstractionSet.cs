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
                //new InstructionItem("r ← r'", OpCodeEnum.LD, "r, r'", new []{ "01rr'" }, new[] { MachineCycleEnum.OpcodeFetch }),
                //new InstructionItem("r ← n", OpCodeEnum.LD, "r, n", new []{ "00r110" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
                new InstructionItem("r ← (HL)", OpCodeEnum.LD, "r, (HL)", new []{ "01r110" }, new[] { MachineCycleEnum.OpcodeFetch, MachineCycleEnum.MemoryRead }),
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
